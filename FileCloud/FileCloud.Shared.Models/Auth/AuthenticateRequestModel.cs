using System.ComponentModel.DataAnnotations;

namespace FileCloud.Shared.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateRequestModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно для заполнения")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
