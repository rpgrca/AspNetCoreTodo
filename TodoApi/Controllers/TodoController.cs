using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Services;
using TodoApi.Mappings;
using System;

namespace TodoApi.Controllers {
    //[ServiceFilter(typeof(ActionFilters.ValidatorFilterAttribute))]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService) {
            _todoItemService = todoItemService;
        }

        [HttpGet("{id}")]
        //[ServiceFilter(typeof(ActionFilters.ValidatorFilterAttribute))]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) {
            TodoItemDTO todoItemDTO = await _todoItemService.GetTodoItemAsync(id);
            if (todoItemDTO == null) {
                return NotFound();
            }

            return Ok(todoItemDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() {
            IEnumerable<TodoItem> list = await _todoItemService.GetTodoItemsAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO itemDTO) {
            var item = await _todoItemService.PostTodoItemAsync(itemDTO);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id}, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItemDTO itemDTO) {
            try {
                TodoItem todoItem = await _todoItemService.PutTodoItemAsync(id, itemDTO);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TodoItem>> PatchTodoItem(long id, TodoItemDTO itemDTO) {
            try {
                TodoItem todoItem = await _todoItemService.PatchTodoItemAsync(id, itemDTO);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id) {
            try {
                 TodoItem todoItem = await _todoItemService.DeleteTodoItemAsync(id);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<TodoItem>>> SearchTodoItem(string searchString) {
            List<TodoItem> result = await _todoItemService.SearchTodoItemAsync(searchString);
            return Ok(result);
        }
    }
}