using System;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;

namespace UserLoginHistory
{
    [WebService(Namespace = "http://userloginhistory.local/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class UserApi : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public UserLookupResponse GetUserByUsername(string username)
        {
            if (String.IsNullOrEmpty(username) || username.Trim().Length == 0)
            {
                return UserLookupResponse.Failure("Username is required.");
            }

            username = username.Trim();

            try
            {
                UserRepository repository = new UserRepository();
                DataTable users = repository.GetUserByUsername(username);

                if (users == null || users.Rows.Count == 0)
                {
                    return UserLookupResponse.Failure("Username was not found.");
                }

                DataRow user = users.Rows[0];
                string databaseUsername = Convert.ToString(user["Username"]);

                if (!String.Equals(username, databaseUsername, StringComparison.OrdinalIgnoreCase))
                {
                    return UserLookupResponse.Failure("Username does not match.");
                }

                UserProfile profile = new UserProfile();
                profile.UserId = Convert.ToInt32(user["UserId"]);
                profile.Username = databaseUsername;
                profile.IsActive = Convert.ToBoolean(user["IsActive"]);

                if (user["CreatedDate"] != DBNull.Value)
                {
                    profile.CreatedDate = Convert.ToDateTime(user["CreatedDate"]);
                }

                return UserLookupResponse.SuccessResult(profile);
            }
            catch (Exception)
            {
                return UserLookupResponse.Failure("Unable to fetch user data.");
            }
        }
    }

    public class UserLookupResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserProfile User { get; set; }

        public static UserLookupResponse SuccessResult(UserProfile user)
        {
            UserLookupResponse response = new UserLookupResponse();
            response.Success = true;
            response.Message = "Username matched.";
            response.User = user;
            return response;
        }

        public static UserLookupResponse Failure(string message)
        {
            UserLookupResponse response = new UserLookupResponse();
            response.Success = false;
            response.Message = message;
            response.User = null;
            return response;
        }
    }

    public class UserProfile
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
