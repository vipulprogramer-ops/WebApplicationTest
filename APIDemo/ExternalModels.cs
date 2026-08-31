using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIDemo
{
    public class ExternalModels
    {
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
    }

    public class ExternalUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
    }
}