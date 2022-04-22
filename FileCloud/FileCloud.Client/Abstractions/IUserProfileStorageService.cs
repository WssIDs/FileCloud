namespace FileCloudClient.Abstractions
{
    public interface IUserProfileStorageService
    {
        Guid Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string UserName { get; set; }

        string Token { get; set; }

        DateTime TokenExpired { get; set; }
    }
}