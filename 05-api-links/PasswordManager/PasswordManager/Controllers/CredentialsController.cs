﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Model;

namespace PasswordManager.Controllers
{
    public class CredentialsController : Controller
    {
        private readonly WebsiteContext context;

        public CredentialsController(WebsiteContext context)
        {
            this.context = context;
        }

        [HttpPost("/websites/{url}/password/{login}")]
        public IActionResult GeneratePassword(string url, string login)
        {
            var password = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            if (website == null)
            {
                return NotFound($"Witryna {url} nie została zarejestrowana.");
            }

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

        [HttpPut("/websites/{url}/password/{login}")]
        public IActionResult StorePassword(string url, string login, string password)
        {
            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            if (website == null)
            {
                return NotFound($"Witryna {url} nie została zarejestrowana.");
            }

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

        [HttpGet("/websites/{url}/password")]
        public IActionResult GetPassword(string url)
        {
            var website = context.Websites
                .Include(w => w.Credentials)
                .FirstOrDefault(w => w.Url == url);

            if (website == null)
            {
                return NotFound($"Witryna {url} nie została zarejestrowana.");
            }

            return Json(new ViewModel.Credentials
            {
                Url = website.Url,
                Login = website.Credentials.Login,
                Password = website.Credentials.Password
            });
        }
    }
}