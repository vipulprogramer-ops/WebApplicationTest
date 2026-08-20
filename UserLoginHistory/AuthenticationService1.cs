using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace UserLoginHistory
{
    public class AuthenticationService1
    {
        private const int MaxFailedAttempts = 5;

        private const int LockoutMinutes = 15;

        private UserRepository userRepository;

        private AuditRepository auditRepository;
        private const byte AuditFailed = 0;
        private const byte AuditSuccess = 1;
        private const byte AuditLocked = 2;


        public AuthenticationService1()
        {
            userRepository = new UserRepository();

            auditRepository = new AuditRepository();
        }


        // =====================================================
        // LOGIN
        // =====================================================

        public AuthenticationResult1 Login(string username,string password,string ipAddress)
        {
            AuthenticationResult1 result = new AuthenticationResult1();

            result.Success = false;

            result.IsLocked = false;

            result.MaxFailedAttempts = MaxFailedAttempts;

            try
            {
                // -------------------------------------------------
                // Validate input
                // -------------------------------------------------

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


                // -------------------------------------------------
                // Normalize username
                // -------------------------------------------------

                username = username.Trim();

                // -------------------------------------------------
                // Get user
                // -------------------------------------------------

                DataTable dt = userRepository.GetUserByUsername(username);


                // -------------------------------------------------
                // User doesn't exist
                // -------------------------------------------------

                if (dt == null ||
                    dt.Rows.Count == 0)
                {
                    auditRepository.CreateLoginAudit(null,username,ipAddress,AuditFailed,"Invalid username");

                    result.Message = "Invalid username or password.";
                    return result;
                }

                DataRow row = dt.Rows[0];


                // -------------------------------------------------
                // Read database values
                // -------------------------------------------------

                int userId = Convert.ToInt32(row["UserId"]);

                string dbUsername = Convert.ToString(row["Username"]);

                string passwordHash = Convert.ToString(row["PasswordHash"]);

                string passwordSalt = Convert.ToString(row["PasswordSalt"]);

                bool isActive = Convert.ToBoolean(row["IsActive"]);


                // -------------------------------------------------
                // Read existing lockout
                // -------------------------------------------------

                int existingFailedAttempts = 0;

                if (row["FailedLoginAttempts"] != DBNull.Value)
                {
                    existingFailedAttempts = Convert.ToInt32(row["FailedLoginAttempts"]);
                }


                DateTime? existingLockoutUntil = null;

                if (row["LockoutUntil"] != DBNull.Value)
                {
                    existingLockoutUntil = Convert.ToDateTime(row["LockoutUntil"]);
                }


                // -------------------------------------------------
                // Check account disabled
                // -------------------------------------------------

                if (!isActive)
                {
                    auditRepository.CreateLoginAudit(userId,dbUsername,ipAddress,AuditFailed,"Account disabled");

                    result.Message = "Your account is disabled.";
                    return result;
                }


                // -------------------------------------------------
                // Check account lockout
                // -------------------------------------------------

                if (existingLockoutUntil.HasValue && existingLockoutUntil.Value > DateTime.Now)
                {
                    result.IsLocked = true;

                    result.LockoutUntil = existingLockoutUntil;

                    auditRepository.CreateLoginAudit(userId,dbUsername,ipAddress,AuditLocked,"Account is locked");

                    result.Message = "Your account is temporarily locked. " + "Please try again later.";

                    return result;
                }


                // -------------------------------------------------
                // Verify password
                // -------------------------------------------------

                bool passwordValid = PasswordHasher.VerifyPassword(password,passwordHash,passwordSalt);


                // =================================================
                // INVALID PASSWORD
                // =================================================

                if (!passwordValid)
                {
                    FailedLoginResult failedResult = auditRepository.RecordFailedLogin(userId);

                    result.FailedAttempts = failedResult.FailedAttempts;


                    // ---------------------------------------------
                    // Account became locked
                    // ---------------------------------------------

                    if (failedResult.IsLocked)
                    {
                        result.IsLocked = true;

                        result.LockoutUntil = failedResult.LockoutUntil;

                        auditRepository.CreateLoginAudit(userId,dbUsername,ipAddress,AuditLocked,"Maximum failed login attempts");

                        result.Message = "Too many failed login attempts. " +  "Your account has been temporarily locked.";

                        return result;
                    }


                    // ---------------------------------------------
                    // Failed but not locked
                    // ---------------------------------------------

                    auditRepository.CreateLoginAudit(0,username,ipAddress,AuditFailed,"Invalid username");


                    int remaining = MaxFailedAttempts - failedResult.FailedAttempts;

                    if (remaining < 0)
                    {
                        remaining = 0;
                    }

                    result.Message = "Invalid username or password. " + "Attempts remaining: " + remaining.ToString();
                    return result;
                }


                // =================================================
                // SUCCESSFUL LOGIN
                // =================================================

                auditRepository.ResetFailedLogin(userId);

                auditRepository.CreateLoginAudit(userId,dbUsername,ipAddress,AuditSuccess,null);


                result.Success = true;

                result.UserId = userId;

                result.Username = dbUsername;

                result.Message = "Login successful.";

                result.FailedAttempts = 0;

                return result;
            }
            catch (Exception ex)
            {
                // Log the actual exception on the server
                // in production rather than displaying it.

                // result.Success = false;

                //result.Message =     "Unable to process your login.";

                //return result;

                result.Success = false;

                result.Message = ex.ToString();

                return result;
            }
        }
    }
}