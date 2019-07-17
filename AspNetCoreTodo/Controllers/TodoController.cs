using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers {
    public class TodoController : Controller {
        private ITodoItemService _todoItemService;

        public async Task<IActionResult> Index() {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            return View(items);
        }

        public TodoController(ITodoItemService todoItemService) {
            _todoItemService = todoItemService;
        }
    }
}