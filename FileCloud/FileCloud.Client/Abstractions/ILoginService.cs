using FileCloud.Shared.Models.Auth;

namespace FileCloudClient.Abstractions
{
    public delegate void RedirectHandler(string url);

    public interface ILoginService
    {
        event RedirectHandler OnRedirected;

        Task<AuthenticateResponseModel> LoginAsync(AuthenticateRequestModel authData);

        void Logout();
    }
}
