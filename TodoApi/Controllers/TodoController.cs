using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Mappings;
using AutoMapper;

namespace TodoApi.Controllers {
    //[ServiceFilter(typeof(ActionFilters.ValidatorFilterAttribute))]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoController(TodoContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
            if (_context.TodoItems.Count() == 0) {
                _context.TodoItems.Add(new TodoItem { Name = "Item1"});
                _context.SaveChanges();
            }
        }

        [HttpGet("{id}")]
        //[ServiceFilter(typeof(ActionFilters.ValidatorFilterAttribute))]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
            }

/* 
            TodoItemDTO dto = new TodoItemDTO();
            dto.Id = todoItem.Id;
            dto.Name = todoItem.Name;
            dto.IsComplete = todoItem.IsComplete;*/
            TodoItemDTO dto = _mapper.Map<TodoItemDTO>(todoItem);

            return dto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO itemDTO) {
            var item = _mapper.Map<TodoItem>(itemDTO);
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id}, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItem item) {
            if (id != item.Id) {
                return BadRequest();
            }

#if false
            _context.Entry(item).State = EntityState.Modified;
#else
            var myItem = await _context.TodoItems.FindAsync(id);
            if (myItem == null) {
                return BadRequest();
            }

            myItem.Name = item.Name;
            myItem.IsComplete = item.IsComplete;
#endif

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TodoItem>> PatchTodoItem(long id, TodoItem item) {
            if (id != item.Id) {
                return BadRequest();
            }

#if false
            _context.Entry(item).State = EntityState.Modified;
#else
            var myItem = await _context.TodoItems.FindAsync(id);
            if (myItem == null) {
                return BadRequest();
            }

            myItem.Name = item.Name;
            myItem.IsComplete = item.IsComplete;
#endif

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("search")]
        public ActionResult<List<TodoItem>> Get(string searchString) {
            List<TodoItem> result = null;
            if (searchString == null) {
                result = _context.TodoItems.ToList();
            }
            else {
                result = _context.TodoItems.Where(x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant())).ToList();
            }

            return result;
        }
    }

}