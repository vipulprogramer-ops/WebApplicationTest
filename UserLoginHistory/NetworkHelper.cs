using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserLoginHistory
{
    public class NetworkHelper
    {
        public static string GetClientIPAddress()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!String.IsNullOrEmpty(ip))
            {
                // If multiple IPs exist,
                // first one is normally original client.
                string[] addresses = ip.Split(',');

                if (addresses.Length > 0)
                {
                    return addresses[0].Trim();
                }
            }

            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (String.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }

            return ip;
        }
    }
}