using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using AutoMapper;

namespace TodoApi.Services {
    public class TodoItemService : ITodoItemService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TodoItemService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoItem> DeleteTodoItemAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<TodoItemDTO> GetTodoItemAsync(long id)
        {
            TodoItem todoItem = await _context.TodoItems.FindAsync(id);
            TodoItemDTO todoItemDTO = _mapper.Map<TodoItem,TodoItemDTO>(todoItem);

            return todoItemDTO;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> PatchTodoItemAsync(long id, TodoItemDTO itemDTO)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            todoItem.Name = itemDTO.Name;
            todoItem.IsComplete = itemDTO.IsComplete;

            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> PostTodoItemAsync(TodoItemDTO itemDTO)
        {
            var item = _mapper.Map<TodoItemDTO,TodoItem>(itemDTO);
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

            todoItem.Name = itemDTO.Name;
            todoItem.IsComplete = itemDTO.IsComplete;

            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<List<TodoItem>> SearchTodoItemAsync(string text)
        {
            List<TodoItem> result = null;
            if (string.IsNullOrEmpty(text)) {
                result = _context.TodoItems.ToList();
            }
            else {
                result = await _context.TodoItems.Where(x => x.Name.ToLowerInvariant().Contains(text.ToLowerInvariant())).ToListAsync();
            }

            return result;
        }
    }
}