using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserLoginHistory
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public bool Locked { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }
    }
}