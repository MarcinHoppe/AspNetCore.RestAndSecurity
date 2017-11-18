using System;

namespace PasswordManager.Model
{
    public class Credentials
    {
        public Guid Id { get; set; }
        public Guid WebsiteId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}