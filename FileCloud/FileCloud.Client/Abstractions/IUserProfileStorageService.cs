using FileCloud.Shared.Models;

namespace FileCloudClient.Abstractions
{
    public interface IUserProfileStorageService
    {
        UserModel User { get; set; }

        string Token { get; set; }

        int TokenExpired { get; set; }
    }
}