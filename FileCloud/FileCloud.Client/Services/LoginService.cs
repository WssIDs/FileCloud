using FileCloudClient.Abstractions;
using FileCloudClient.Models.Auth;
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

        public async Task<AuthenticateResponseModel> LoginAsync(AuthenticateRequestModel authData)
        {
            var content = new StringContent(JsonSerializer.Serialize(authData), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("api/Users/Authenticate", content);

            var successResponseMessage = responseMessage.EnsureSuccessStatusCode();

            var responseModel = await successResponseMessage.Content.ReadFromJsonAsync<AuthenticateResponseModel>();
            Debug.WriteLine($"{responseModel.Id} {responseModel.UserName} - {responseModel.JwtToken}");

            _userProfileStorageService.Token = responseModel.JwtToken;

            if (responseMessage.Headers.TryGetValues("JwtTokenExpires", out var values))
            {
                string expires = values.FirstOrDefault();

                if (expires != null)
                {
                    _userProfileStorageService.TokenExpired = DateTime.Parse(expires);
                }
            }

            return responseModel;
        }

        public void Logout()
        {
            _userProfileStorageService.Token = null;
            OnRedirected?.Invoke("/");
        }
    }
}
