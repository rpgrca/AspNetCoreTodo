using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreTodo.Controllers;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers {
    [Authorize]
    public class TodoController : Controller {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public async Task<IActionResult> Index() {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            var model = new TodoViewModel() {
                Items = items
            };
            return View(model);
        }

        public TodoController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager) {
            _todoItemService = todoItemService;
            _userManager = userManager;
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

        public async Task<IActionResult> MarkDone(Guid id) {
            if (id == Guid.Empty) {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.MarkDoneAsync(id);
            if (! successful) {
                return BadRequest("Could not mark item as done.");
            }

            return RedirectToAction("Index");
        }
    }
}