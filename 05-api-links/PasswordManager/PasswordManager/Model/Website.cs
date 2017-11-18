using System;
using System.Collections;
using System.Security.AccessControl;

namespace PasswordManager.Model
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Credentials Credentials { get; set; }
    }
}