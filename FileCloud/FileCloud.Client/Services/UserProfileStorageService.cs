using FileCloud.Shared.Models;
using FileCloudClient.Abstractions;

namespace FileCloudClient.Services
{
    public class UserProfileStorageService : IUserProfileStorageService
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
        public int TokenExpired { get; set; }
    }
}