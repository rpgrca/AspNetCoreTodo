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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Controllers {
    //[ServiceFilter(typeof(ActionFilters.ValidatorFilterAttribute))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService) {
            _todoItemService = todoItemService;
        }

        [Authorize]
        [HttpGet("{id:regex(^[[0-9]]+$)}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) {
            try {
                TodoItemDTO todoItemDTO = await _todoItemService.GetTodoItemAsync(id);
                return Ok(todoItemDTO);
            }
            catch (ArgumentException) {
                return NotFound();
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems() {
            var list = await _todoItemService.GetTodoItemsAsync();
            return Ok(list);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO itemDTO) {
            var item = await _todoItemService.PostTodoItemAsync(itemDTO);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id}, item);
        }

        [Authorize]
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

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<TodoItem>> PatchTodoItem(long id, JsonPatchDocument<TodoItemDTO> patch) {
            try {
                TodoItem todoItem = await _todoItemService.PatchTodoItemAsync(id, patch);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDTO>> DeleteTodoItem(long id) {
            try {
                 TodoItemDTO todoItem = await _todoItemService.DeleteTodoItemAsync(id);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("search/{searchString}")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> SearchTodoItems(string searchString) {
            List<TodoItemDTO> result = await _todoItemService.SearchTodoItemsAsync(searchString);
            return Ok(result);
        }
    }
}