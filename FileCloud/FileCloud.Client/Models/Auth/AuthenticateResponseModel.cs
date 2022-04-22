namespace FileCloudClient.Models.Auth
{
    public class AuthenticateResponseModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JwtToken { get; set; }
    }
}
