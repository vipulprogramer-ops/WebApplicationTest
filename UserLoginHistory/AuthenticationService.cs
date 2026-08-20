using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace UserLoginHistory
{
        public class AuthenticationResult
        {
            public bool Success;
            public string Message;
            public int UserId;
            public string Username;
        }

        public class AuthenticationService
        {
            private UserRepository userRepository;

            public AuthenticationService()
            {
                userRepository = new UserRepository();
            }

            public AuthenticationResult Login(string username,string password)
            {
                AuthenticationResult result = new AuthenticationResult();

                result.Success = false;
                result.Message = "Invalid username or password.";

                try
                {
                    // -----------------------------------------
                    // Validate input
                    // -----------------------------------------

                    if (String.IsNullOrEmpty(username))
                    {
                        result.Message = "Please enter username.";
                        return result;
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        result.Message = "Please enter password.";
                        return result;
                    }


                    // -----------------------------------------
                    // Get user
                    // -----------------------------------------

                    DataTable dt = userRepository.GetUserByUsername(username);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        result.Message = "Invalid username or password.";
                        return result;
                    }


                    // -----------------------------------------
                    // Read user information
                    // -----------------------------------------

                    DataRow row = dt.Rows[0];

                    int userId = Convert.ToInt32(row["UserId"]);

                    string dbUsername = Convert.ToString(row["Username"]);

                    string passwordHash = Convert.ToString(row["PasswordHash"]);

                    string passwordSalt = Convert.ToString(row["PasswordSalt"]);

                    bool isActive = Convert.ToBoolean(row["IsActive"]);


                    // -----------------------------------------
                    // Check active account
                    // -----------------------------------------

                    if (!isActive)
                    {
                        result.Message = "Your account is disabled.";
                        return result;
                    }


                    // -----------------------------------------
                    // Check password
                    // -----------------------------------------

                    bool passwordValid = PasswordHasher.VerifyPassword(password,passwordHash,passwordSalt);

                    if (!passwordValid)
                    {
                        result.Message = "Invalid username or password.";
                        return result;
                    }


                    // -----------------------------------------
                    // LOGIN SUCCESS
                    // -----------------------------------------

                    result.Success = true;

                    result.Message = "Login successful.";

                    result.UserId = userId;

                    result.Username = dbUsername;

                    return result;
                }
                catch (Exception ex)
                {
                    // IMPORTANT:
                    // During development, return the real
                    // error so we can identify problems.

                    result.Success = false;
                    result.Message = ex.Message;
                    return result;
                }
            }
        }

}