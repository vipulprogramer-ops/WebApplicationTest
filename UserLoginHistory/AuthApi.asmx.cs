using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace UserLoginHistory
{
    [WebService(Namespace = "http://userloginhistory.local/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class AuthApi : WebService
    {
        private const long TokenLifetimeMilliseconds = 60 * 60 * 1000;
        private const long MaximumRequestAgeMilliseconds = 5 * 60 * 1000;
        private static readonly object TokenLock = new object();
        private static readonly Dictionary<string, TokenRecord> Tokens = new Dictionary<string, TokenRecord>();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AuthenticationTokenResponse GetAuthenticationToken(string Username, long time)
        {
            AuthenticationTokenRequest request = new AuthenticationTokenRequest();
            request.Username = Username;
            request.Time = time;

            if (!IsJsonPost())
            {
                return AuthenticationTokenResponse.Failure("Invalid request method or content type.");
            }

            if (String.IsNullOrEmpty(request.Username) || request.Time <= 0)
            {
                return AuthenticationTokenResponse.Failure("Invalid post data.");
            }

            if (!IsAuthorizationHeaderMatch(request))
            {
                return AuthenticationTokenResponse.Failure("Authorization token and post data do not match.");
            }

            long now = ToUnixMilliseconds(DateTime.UtcNow);

            if (Math.Abs(now - request.Time) > MaximumRequestAgeMilliseconds)
            {
                return AuthenticationTokenResponse.Failure("Invalid request time.");
            }

            try
            {
                UserRepository repository = new UserRepository();
                DataTable users = repository.GetUserByUsername(request.Username.Trim());

                if (users == null || users.Rows.Count == 0)
                {
                    return AuthenticationTokenResponse.Failure("No matching record found.");
                }

                string accessToken = CreateAccessToken();
                long validityEnd = now + TokenLifetimeMilliseconds;

                lock (TokenLock)
                {
                    RemoveExpiredTokens(now);
                    Tokens[accessToken] = new TokenRecord(validityEnd);
                }

                return AuthenticationTokenResponse.SuccessResult(accessToken, validityEnd);
            }
            catch (Exception)
            {
                return AuthenticationTokenResponse.Failure("Invalid request. Try again.");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public UserDetailsResponse FetchUserDetails(string access_token, int UserId)
        {
            UserDetailsRequest request = new UserDetailsRequest();
            request.AccessToken = access_token;
            request.UserId = UserId;

            if (!IsJsonPost())
            {
                return UserDetailsResponse.Failure("Invalid request method or content type.");
            }

            if (String.IsNullOrEmpty(request.AccessToken) || request.UserId <= 0)
            {
                return UserDetailsResponse.Failure("Invalid post data.");
            }

            if (!IsAuthorizationHeaderMatch(request))
            {
                return UserDetailsResponse.Failure("Authorization token and post data do not match.");
            }

            if (!IsAccessTokenValid(request.AccessToken))
            {
                return UserDetailsResponse.Failure("Invalid access token.");
            }

            try
            {
                UserRepository repository = new UserRepository();
                DataTable users = repository.GetUserById(request.UserId);

                if (users == null || users.Rows.Count == 0)
                {
                    return UserDetailsResponse.Failure("No matching record found.");
                }

                DataRow row = users.Rows[0];
                UserDetails details = new UserDetails();
                details.UserId = Convert.ToInt32(row["UserId"]);
                details.Username = Convert.ToString(row["Username"]);
                details.IsActive = Convert.ToBoolean(row["IsActive"]);

                if (row["CreatedDate"] != DBNull.Value)
                {
                    details.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                }

                return UserDetailsResponse.SuccessResult(details);
            }
            catch (Exception)
            {
                return UserDetailsResponse.Failure("Invalid request. Try again.");
            }
        }

        private bool IsJsonPost()
        {
            return String.Equals(Context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) &&
                   !String.IsNullOrEmpty(Context.Request.ContentType) &&
                   Context.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsAuthorizationHeaderMatch(AuthenticationTokenRequest request)
        {
            try
            {
                Dictionary<string, object> headerData = ReadAuthorizationHeader();
                return String.Equals(Convert.ToString(headerData["Username"]), request.Username, StringComparison.Ordinal) &&
                       Convert.ToInt64(headerData["time"]) == request.Time;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsAuthorizationHeaderMatch(UserDetailsRequest request)
        {
            try
            {
                Dictionary<string, object> headerData = ReadAuthorizationHeader();
                return String.Equals(Convert.ToString(headerData["access_token"]), request.AccessToken, StringComparison.Ordinal) &&
                       Convert.ToInt32(headerData["UserId"]) == request.UserId;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Dictionary<string, object> ReadAuthorizationHeader()
        {
            string encodedHeader = Context.Request.Headers["Authorization-Token"];

            if (String.IsNullOrEmpty(encodedHeader))
            {
                throw new InvalidOperationException("Authorization header is required.");
            }

            string decodedHeader = Encoding.UTF8.GetString(Convert.FromBase64String(encodedHeader));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<Dictionary<string, object> >(decodedHeader);
        }

        private bool IsAccessTokenValid(string accessToken)
        {
            lock (TokenLock)
            {
                TokenRecord record;

                if (!Tokens.TryGetValue(accessToken, out record))
                {
                    return false;
                }

                long now = ToUnixMilliseconds(DateTime.UtcNow);

                if (record.ValidityEnd <= now)
                {
                    Tokens.Remove(accessToken);
                    return false;
                }

                return true;
            }
        }

        private void RemoveExpiredTokens(long now)
        {
            List<string> expiredTokens = new List<string>();

            foreach (KeyValuePair<string, TokenRecord> token in Tokens)
            {
                if (token.Value.ValidityEnd <= now)
                {
                    expiredTokens.Add(token.Key);
                }
            }

            foreach (string token in expiredTokens)
            {
                Tokens.Remove(token);
            }
        }

        private string CreateAccessToken()
        {
            byte[] bytes = new byte[32];
            using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes);
        }

        private static long ToUnixMilliseconds(DateTime value)
        {
            return (long)(value - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }

    public class AuthenticationTokenRequest
    {
        public string Username { get; set; }
        public long Time { get; set; }
    }

    public class AuthenticationTokenResponse
    {
        public bool success { get; set; }
        public long validity_end { get; set; }
        public string access_token { get; set; }
        public string message { get; set; }

        public static AuthenticationTokenResponse SuccessResult(string accessToken, long validityEnd)
        {
            AuthenticationTokenResponse response = new AuthenticationTokenResponse();
            response.success = true;
            response.validity_end = validityEnd;
            response.access_token = accessToken;
            return response;
        }

        public static AuthenticationTokenResponse Failure(string errorMessage)
        {
            AuthenticationTokenResponse response = new AuthenticationTokenResponse();
            response.success = false;
            response.message = errorMessage;
            return response;
        }
    }

    public class UserDetailsRequest
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
    }

    public class UserDetailsResponse
    {
        public bool success { get; set; }
        public List<UserDetails> user_details { get; set; }
        public string message { get; set; }

        public static UserDetailsResponse SuccessResult(UserDetails user)
        {
            UserDetailsResponse response = new UserDetailsResponse();
            response.success = true;
            response.user_details = new List<UserDetails>();
            response.user_details.Add(user);
            return response;
        }

        public static UserDetailsResponse Failure(string errorMessage)
        {
            UserDetailsResponse response = new UserDetailsResponse();
            response.success = false;
            response.message = errorMessage;
            response.user_details = new List<UserDetails>();
            return response;
        }
    }

    public class UserDetails
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    internal class TokenRecord
    {
        public long ValidityEnd { get; private set; }

        public TokenRecord(long validityEnd)
        {
            ValidityEnd = validityEnd;
        }
    }
}
