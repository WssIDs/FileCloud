using System.ComponentModel.DataAnnotations;

namespace FileCloud.Server.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
