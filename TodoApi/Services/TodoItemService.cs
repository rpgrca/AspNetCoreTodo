using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApi.Services {
    public class TodoItemService : ITodoItemService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TodoItemService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoItemDTO> DeleteTodoItemAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemDTO>(todoItem);
        }

        public async Task<TodoItemDTO> GetTodoItemAsync(long id)
        {
            TodoItem todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            TodoItemDTO todoItemDTO = _mapper.Map<TodoItemDTO>(todoItem);
            return todoItemDTO;
        }

        public async Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            var todoItemsDTO = _mapper.Map<List<TodoItemDTO>>(todoItems);

            return todoItemsDTO;
        }

        public async Task<TodoItem> PatchTodoItemAsync(long id, JsonPatchDocument<TodoItemDTO> patch)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            var todoItemDTO = _mapper.Map<TodoItemDTO>(todoItem);
            patch.ApplyTo(todoItemDTO);
            CopyFromDTO(todoItem, todoItemDTO);

            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> PostTodoItemAsync(TodoItemDTO itemDTO)
        {
            var item = _mapper.Map<TodoItem>(itemDTO);
            _context.TodoItems.Add(item);

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem> PutTodoItemAsync(long id, TodoItemDTO itemDTO)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            CopyFromDTO(todoItem, itemDTO);

            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<List<TodoItemDTO>> SearchTodoItemsAsync(string text)
        {
            List<TodoItem> result = null;
            if (string.IsNullOrEmpty(text)) {
                result = _context.TodoItems.ToList();
            }
            else {
                result = await _context.TodoItems.Where(x => x.Name.ToLowerInvariant().Contains(text.ToLowerInvariant())).ToListAsync();
            }

            var todoItemsDTO = _mapper.Map<List<TodoItemDTO>>(result);
            return todoItemsDTO;
        }

        private void CopyFromDTO(TodoItem todoItem, TodoItemDTO todoItemDTO)
        {
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;
            todoItem.Description = todoItemDTO.Description;
            todoItem.DueAt = todoItemDTO.DueAt;
            todoItem.Order = todoItemDTO.Order;
        }
    }
}