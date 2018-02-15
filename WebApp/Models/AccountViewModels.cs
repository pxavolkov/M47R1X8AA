using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI;

namespace WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Пожалуйста введите email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Пожалуйста введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Пароль не должен быть длинее 50 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }

        //Player
        [Required(ErrorMessage = "Пожалуйста введите имя игрока")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя игрока")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string PlayerName { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите возраст игрока")]
        [DataType(DataType.Text)]
        [Range(0, 100, ErrorMessage = "Введите корректный возраст")]
        [Display(Name = "Возраст игрока")]
        public string PlayerAge { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите контактные данные")]
        [DataType(DataType.Text)]
        [Display(Name = "Адрес страницы ВК или телефон")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string Info { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Хронические заболевания и противопоказания")]
        [StringLength(1000, ErrorMessage = "Максимальная длина - 1000 символов")]
        public string Allergy { get; set; }

        //Character
        [Required(ErrorMessage = "Пожалуйста введите имя персонажа")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя персонажа")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите фамилию персонажа")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия персонажа")]
        public string LastName { get; set; }

        [Display(Name = "Пол")]
        public int Sex { get; set; }

        [Range(0, 1000, ErrorMessage = "Введите корректный возраст")]
        [DataType(DataType.Text)]
        [Display(Name = "Возраст")]
        public string Age { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Квента")]
        public HttpPostedFileBase Quenta { get; set; }
    }

}
