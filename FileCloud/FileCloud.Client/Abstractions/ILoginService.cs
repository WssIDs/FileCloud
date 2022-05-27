using FileCloud.Shared.Models;
using FileCloud.Shared.Models.Auth;

namespace FileCloudClient.Abstractions
{
    public delegate void RedirectHandler(string url);

    public interface ILoginService
    {
        event RedirectHandler OnRedirected;

        Task<bool> LoginAsync(AuthenticateRequestModel authData);

        Task<bool> CheckLoginAsync(string login);

        Task<bool> RegisterAsync(CreateUserModel user);

        void Logout();
    }
}
