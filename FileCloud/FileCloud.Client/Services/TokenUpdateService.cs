using FileCloudClient.Abstractions;
using FileCloudClient.Models.Auth;
using System.Diagnostics;
using System.Net.Http.Json;

namespace FileCloudClient.Services
{
    public class TokenUpdateService : ITokenUpdateService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly IUserProfileStorageService _userProfileStorageService;
        private readonly ILoginService _loginService;

        public TokenUpdateService(IHttpClientFactory httpClientFactory, 
            IUserProfileStorageService userProfileStorageService,
            ILoginService loginService)
        {
            _httpClientFactory = httpClientFactory;
            _userProfileStorageService = userProfileStorageService;
            _loginService = loginService;
        }

        public async Task RunAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var stoppingToken = _cancellationTokenSource.Token;

            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                Debug.WriteLine("Запуск задачи обновления токена");

                do
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        stoppingToken.ThrowIfCancellationRequested();
                        Debug.WriteLine("Отмена обновления токена");
                        return;
                    }

                    var diff = _userProfileStorageService.TokenExpired - TimeSpan.FromSeconds(30);

                    var delayDiff = diff - DateTime.UtcNow;

                    await Task.Delay(delayDiff, stoppingToken);

                    Debug.WriteLine("Проверка обновления токена");

                    var client = _httpClientFactory.CreateClient("FileCloud");

                    if (client != null)
                    {
                        if (client.DefaultRequestHeaders.Authorization != null)
                        {
                            var responseMessage = await client.GetAsync("api/Users/GetToken", stoppingToken);

                            var successResponseMessage = responseMessage.EnsureSuccessStatusCode();

                            if (successResponseMessage.IsSuccessStatusCode)
                            {
                                var responseModel = await responseMessage.Content.ReadFromJsonAsync<AuthenticateTokenResponseModel>();

                                _userProfileStorageService.Token = responseModel.Token;

                                if (responseMessage.Headers.TryGetValues("JwtTokenExpires", out var values))
                                {
                                    string expires = values.FirstOrDefault();

                                    if (expires != null)
                                    {
                                        _userProfileStorageService.TokenExpired = DateTime.Parse(expires);
                                    }
                                }
                            }
                        }
                    }

                    Debug.WriteLine($"Текущее время {DateTime.UtcNow}");
                }
                while (!stoppingToken.IsCancellationRequested);
            }
            catch (TaskCanceledException cancelException)
            {
                Debug.WriteLine($"Задача отменена {cancelException}");
                throw;
            }
            catch(Exception ex)
            {
                // if API server get error
                Debug.WriteLine($"Задача отменена {ex}");
                _loginService.Logout();
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
