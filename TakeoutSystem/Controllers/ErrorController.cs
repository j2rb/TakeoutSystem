using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TakeoutSystem.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message
            );
        }
    }
}