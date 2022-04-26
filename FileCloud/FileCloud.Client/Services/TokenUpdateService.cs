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

                    var seconds = 120;

                    var delayDiff = _userProfileStorageService.TokenExpired;

                    if (_userProfileStorageService.TokenExpired > seconds)
                    {
                        delayDiff = _userProfileStorageService.TokenExpired - seconds;

                        if(delayDiff <= 20)
                        {
                            delayDiff = _userProfileStorageService.TokenExpired - (seconds / 3);
                        }
                    }
                    else
                    {
                        delayDiff = _userProfileStorageService.TokenExpired - (_userProfileStorageService.TokenExpired / 2);
                    }

                    Debug.WriteLine($"Время жизни токена {_userProfileStorageService.TokenExpired} секунд. Запуск обновления токена произойдет через {delayDiff} секунд");

                    await Task.Delay(TimeSpan.FromSeconds(delayDiff), stoppingToken);

                    Debug.WriteLine("Проверка обновления токена");

                    var client = _httpClientFactory.CreateClient("FileCloud");

                    if (client != null)
                    {
                        if (client.DefaultRequestHeaders.Authorization != null)
                        {
                            var responseMessage = await client.GetAsync("api/Users/UpdateToken", stoppingToken);

                            var successResponseMessage = responseMessage.EnsureSuccessStatusCode();

                            if (successResponseMessage.IsSuccessStatusCode)
                            {

                                if (responseMessage.Headers.TryGetValues("JwtTokenExpires", out var values))
                                {
                                    string expires = values.FirstOrDefault();

                                    if (expires != null)
                                    {
                                        _userProfileStorageService.TokenExpired = int.Parse(expires);
                                    }
                                }

                                if (responseMessage.Headers.TryGetValues("JwtToken", out var tokenValues))
                                {
                                    string token = tokenValues.FirstOrDefault();

                                    if (token != null)
                                    {
                                        _userProfileStorageService.Token = token;
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
