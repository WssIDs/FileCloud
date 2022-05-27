using FileCloud.Shared.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace FileCloud.Shared.Models
{
    public class CreateUserModel
    {
        private string _userName;

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Логин обязателен для запонения")]
        [EqualValue(nameof(IsChecked), false, ErrorMessage = "Пользователь с текущим логином существует")]
        public string UserName
        { 
            get => _userName;
            set 
            {
                _userName = value;
                IsChecked = null;
            }
        }

        public bool? IsChecked { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно к заполнению")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна к заполнению")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email обязателен к заполнению")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Пароль обязателен к заполнению")]
        [DataType(DataType.Password)]
        [Compare(nameof(ConfirmPassword), ErrorMessage = "Поле 'Пароль' и 'Подтверждение пароля' не совпадают")]
        public string Password { get; set; }

        [Display(Name = "Подтверждение пароля")]
        [Required(ErrorMessage = "Пароль обязателен к заполнению")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Поле 'Пароль' и 'Подтверждение пароля' не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
