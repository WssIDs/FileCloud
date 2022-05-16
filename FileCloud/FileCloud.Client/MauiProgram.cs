using FileCloudClient.Abstractions;
using FileCloudClient.Services;
using System.Net.Http.Headers;

namespace FileCloudClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<IUserProfileStorageService, UserProfileStorageService>();

            builder.Services.AddHttpClient("FileCloud", (provider, client) =>
            {
                var userProfileService = provider.GetRequiredService<IUserProfileStorageService>();

                // TODO Вынести адрес в конфиг
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userProfileService.Token);
            });

            builder.Services.AddSingleton<ILoginService, LoginService>();
            builder.Services.AddSingleton<ITokenUpdateService, TokenUpdateService>();
            builder.Services.AddTransient<IAdminService, AdminService>();

            return builder.Build();
        }
    }
}