using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserLoginHistory
{
    public class AuthenticationResult1
    {
        public bool Success { get; set; }

        public bool IsLocked { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int FailedAttempts { get; set; }

        public int MaxFailedAttempts { get; set; }

        public System.DateTime? LockoutUntil { get; set; }
    }
}