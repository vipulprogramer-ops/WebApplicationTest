using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIDemo.Controllers
{
    [RoutePrefix("api/userproxy")]
    public class UserProxyController : ApiController
    {
        // Use a static HttpClient to prevent socket reuse bottlenecks in .NET 4.5
        private static readonly HttpClient _httpClient = new HttpClient();

        static UserProxyController()
        {
            _httpClient.BaseAddress = new Uri("https://dummyjson.com");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IHttpActionResult> GetSecureProfile()
        {
            try
            {
                // 1. Authenticate with DummyJSON to fetch a temporary JWT access token
                var credentials = new { username = "emilys", password = "emilyspass" };

                HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync("auth/login", credentials);

                if (!loginResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Authentication failed against remote API server.");
                }

                TokenResponse authData = await loginResponse.Content.ReadAsAsync<TokenResponse>();
                if (authData == null || string.IsNullOrEmpty(authData.AccessToken))
                {
                    return InternalServerError(new Exception("Acquired token was null or missing."));
                }

                // 2. Clone or prepare a custom request message to inject headers securely per call
                using (var profileRequest = new HttpRequestMessage(HttpMethod.Get, "auth/me"))
                {
                    // Attach the Bearer Token to our outgoing execution request
                    profileRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);

                    // Send the request
                    HttpResponseMessage profileResponse = await _httpClient.SendAsync(profileRequest);

                    if (!profileResponse.IsSuccessStatusCode)
                    {
                        return StatusCode(profileResponse.StatusCode);
                    }

                  
                    // 3. Deserialize back into our local model type configuration
                    ExternalUser userProfile = await profileResponse.Content.ReadAsAsync<ExternalUser>();


                    // 4. Return data model back out to your local client
                    return Ok(userProfile);
                }
            }
            catch (HttpRequestException httpEx)
            {
                return InternalServerError(new Exception("Network failure communication exception: " + httpEx.Message));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
    
    
}