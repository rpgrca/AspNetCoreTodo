using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace TodoApi.ActionFilters {
    public class ValidatorFilterAttribute : IActionFilter {
        public void OnActionExecuting(ActionExecutingContext context) {
            var client = context.HttpContext.Request.Headers["client"];
            if (client == StringValues.Empty || client != "1") {
                context.Result = new BadRequestObjectResult("Invalid Client");
                return;
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}