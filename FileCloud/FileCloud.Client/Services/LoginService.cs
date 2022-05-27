using FileCloud.Shared.Models;
using FileCloud.Shared.Models.Auth;
using FileCloudClient.Abstractions;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FileCloudClient.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserProfileStorageService _userProfileStorageService;

        public LoginService(
            IHttpClientFactory httpClientFactory,
            IUserProfileStorageService userProfileStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("FileCloud");
            _userProfileStorageService = userProfileStorageService;
        }

        public event RedirectHandler OnRedirected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<bool> CheckLoginAsync(string login)
        {
            var response = await _httpClient.GetAsync($"api/Users/CheckLogin/{login}");
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> LoginAsync(AuthenticateRequestModel authData)
        {
            var content = new StringContent(JsonSerializer.Serialize(authData), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("api/Users/Authenticate", content);

            if(responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var message = await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(message);
            }

            var successResponseMessage = responseMessage.EnsureSuccessStatusCode();

            var user = await successResponseMessage.Content.ReadFromJsonAsync<UserModel>();
            Debug.WriteLine($"{user.Id} {user.UserName}");

            _userProfileStorageService.User = user;

            if (responseMessage.Headers.TryGetValues("JwtTokenExpires", out var expiresValues))
            {
                string expires = expiresValues.FirstOrDefault();

                if (expires != null)
                {
                    _userProfileStorageService.TokenExpired = int.Parse(expires);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (responseMessage.Headers.TryGetValues("JwtToken", out var tokenValues))
            {
                string token = tokenValues.FirstOrDefault();

                if (token != null)
                {
                    _userProfileStorageService.Token = token;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Logout()
        {
            _userProfileStorageService.Token = null;
            OnRedirected?.Invoke("/");
        }

        public async Task<bool> RegisterAsync(CreateUserModel user)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Users/Register", user);

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
