using System;
using System.Threading.Tasks;
using System.Linq;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services {
    public class TodoItemService : ITodoItemService {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync() {
            return await _context.Items.Where(x => x.IsDone == false).ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem) {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;

            // Esto es logica de negocios: puede ir en el servicio o en el modelo
            if (newItem.DueAt == null) {
                newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            }

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id) {
            var item = await _context.Items
                                .Where(x => x.Id == id)
                                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }
    }
}