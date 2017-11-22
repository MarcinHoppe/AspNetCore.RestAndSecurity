using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Model;

namespace SimpleApi.Controllers
{
    [Route("contacts")]
    [Authorize]
    public class ContactController : Controller
    {
        private static readonly List<ContactInfo> contacts = new List<ContactInfo>()
        {
            new ContactInfo
            {
                FirstName = "Marcin",
                LastName = "Hoppe",
                Email = "marcin.hoppe@acm.org",
                PhoneNumber = "+48 123 456 789"
            },
            new ContactInfo
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@gmail.com",
                PhoneNumber = "+48 987 654 321"
            }
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Json(contacts);
        }

        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var contactInfo = contacts.FirstOrDefault(c => c.Email == email);
            if (contactInfo == null)
            {
                return NotFound();
            }
            return Json(contactInfo);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ContactInfo contactInfo)
        {
            contacts.Add(contactInfo);
            return Ok();
        }

        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            var contactInfo = contacts.FirstOrDefault(c => c.Email == email);
            if (contactInfo == null)
            {
                return NotFound();
            }
            contacts.Remove(contactInfo);
            return Ok();
        }
    }
}