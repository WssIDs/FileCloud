namespace FileCloud.Server.Auth
{
    public interface IAuthManager
    {
        Task<string> AuthAsync(string userName, string password);
    }
}
