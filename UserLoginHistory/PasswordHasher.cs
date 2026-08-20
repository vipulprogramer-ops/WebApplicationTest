using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace UserLoginHistory
{
    public class PasswordHasher
    {
        private const int SaltSize = 32;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        // =====================================================
        // Generate cryptographically random salt
        // =====================================================

        public static string GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];

            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            try
            {
                rng.GetBytes(salt);
            }
            finally
            {
                IDisposable disposable = rng as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            return Convert.ToBase64String(salt);
        }


        // =====================================================
        // Generate PBKDF2 password hash
        // =====================================================

        public static string HashPassword(string password,string saltBase64)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (saltBase64 == null)
                throw new ArgumentNullException("saltBase64");

            byte[] salt = Convert.FromBase64String(saltBase64);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password,salt);

            try
            {
                pbkdf2.IterationCount = Iterations;

                byte[] hash = pbkdf2.GetBytes(HashSize);

                return Convert.ToBase64String(hash);
            }
            finally
            {
                IDisposable disposable = pbkdf2 as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }


        // =====================================================
        // Verify password
        // =====================================================

        public static bool VerifyPassword(string password,string storedHash,string storedSalt)
        {
            if (password == null || storedHash == null || storedSalt == null)
            {
                return false;
            }

            string calculatedHash = HashPassword(password,storedSalt);

            return SecureEquals(calculatedHash,storedHash);
        }


        // =====================================================
        // Constant-time comparison
        // =====================================================

        private static bool SecureEquals(string value1,string value2)
        {
            if (value1 == null ||  value2 == null)
            {
                return false;
            }

            byte[] a = Convert.FromBase64String(value1);

            byte[] b = Convert.FromBase64String(value2);

            if (a.Length != b.Length)
            {
                return false;
            }

            int result = 0;

            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}