using Microsoft.AspNetCore.Mvc;
using PasswordManager.Model;

namespace PasswordManager.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly WebsiteContext context;

        public WebsiteController(WebsiteContext context)
        {
            this.context = context;
        }

        [HttpPost("/websites")]
        public IActionResult RegisterWebsite([FromBody] ViewModel.Website website)
        {
            context.Websites.Add(new Website
            {
                Url = website.Url
            });
            context.SaveChanges();

            return Ok();
        }
    }
}