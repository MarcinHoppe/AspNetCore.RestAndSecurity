using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Model;

namespace PasswordManager.Controllers
{
    public class ApiController : Controller
    {
        private readonly WebsiteContext context;

        public ApiController(WebsiteContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult RegisterWebsite(string url)
        {
            context.Websites.Add(new Website
            {
                Url = url
            });
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult GeneratePassword(string url, string login)
        {
            var password = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            if (website.Credentials == null)
            {
                website.Credentials = new Credentials();
            }

            website.Credentials.Login = login;
            website.Credentials.Password = password;

            context.Websites.Update(website);

            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult StorePassword(string url, string login, string password)
        {
            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            if (website.Credentials == null)
            {
                website.Credentials = new Credentials();
            }

            website.Credentials.Login = login;
            website.Credentials.Password = password;

            context.Websites.Update(website);

            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult GetPassword(string url)
        {
            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            return Json(website.Credentials.Password);
        }
    }
}