using System.Net;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Error;

namespace TodoApi.Controllers {
    [ApiController]
    [Route("/errors")]
    [ApiExplorerSettings()]
    public class ErrorController : ControllerBase {
        [Route("{code")]
        public IActionResult Error(int code) {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            Error.Error error = new Error.Error(code, parsedCode.ToString());

            return Ok(error);
        }
    }
}