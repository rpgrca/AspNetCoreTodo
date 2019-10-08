using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.DTO;

namespace TodoApi.Services {
    public interface ITodoItemService {
        Task<TodoItemDTO> GetTodoItemAsync(long id);
        Task<List<TodoItemDTO>> GetTodoItemsAsync();
        Task<TodoItem> PutTodoItemAsync(long id, TodoItemDTO itemDTO);
        Task<TodoItem> PostTodoItemAsync(TodoItemDTO itemDTO);
        Task<TodoItem> PatchTodoItemAsync(long id, TodoItemDTO itemDTO);
        Task<TodoItemDTO> DeleteTodoItemAsync(long id);
        Task<List<TodoItemDTO>> SearchTodoItemsAsync(string text);
    }
}