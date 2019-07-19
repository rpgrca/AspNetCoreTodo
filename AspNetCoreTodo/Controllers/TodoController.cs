using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Controllers;

namespace AspNetCoreTodo.Controllers {
    public class TodoController : Controller {
        private readonly ITodoItemService _todoItemService;

        public async Task<IActionResult> Index() {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            var model = new TodoViewModel() {
                Items = items
            };
            return View(model);
        }

        public TodoController(ITodoItemService todoItemService) {
            _todoItemService = todoItemService;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem) {
            // Si el title viene vacio aunque este requerido por TodoItem
            if (!ModelState.IsValid) {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.AddItemAsync(newItem);
            if (! successful) {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }
    }
}