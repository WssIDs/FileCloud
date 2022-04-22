using FileCloudClient.Abstractions;

namespace FileCloudClient.Services
{
    public class UserProfileStorageService : IUserProfileStorageService
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpired { get; set; }
    }
}