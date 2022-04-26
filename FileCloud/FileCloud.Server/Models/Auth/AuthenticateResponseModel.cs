namespace FileCloud.Server.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateResponseModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }
    }
}