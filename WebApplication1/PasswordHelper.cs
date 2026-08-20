using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1
{
    public class PasswordHelper
    {
        
            public static string SHA256Hash(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);

                    byte[] hash = sha256.ComputeHash(bytes);

                    StringBuilder result = new StringBuilder();

                    for (int i = 0; i < hash.Length; i++)
                    {
                        result.Append(hash[i].ToString("x2"));
                    }

                    return result.ToString();
                }
            }
        
    }
}