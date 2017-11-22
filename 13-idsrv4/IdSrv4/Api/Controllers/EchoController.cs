using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/echo")]
    [Authorize]
    public class EchoController : Controller
    {
        [HttpGet("{message}")]
        public IActionResult Echo(string message)
        {
            return Content(message);
        }
    }
}