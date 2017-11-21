using Microsoft.AspNetCore.Mvc;

namespace EchoApi.Controllers
{
    [Route("echo")]
    public class EchoController : Controller
    {
        [HttpGet("{message}")]
        public IActionResult Echo(string message)
        {
            return Content(message);
        }
    }
}