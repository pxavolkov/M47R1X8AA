using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI;

namespace WebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }

        //Player
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя игрока")]
        public string PlayerName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Возраст игрока")]
        public string PlayerAge { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Адрес страницы ВК или телефон")]
        public string Info { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Хронические заболевания и противопоказания")]
        public string Allergy { get; set; }

        //Character

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя персонажа")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия персонажа")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Пол")]
        public int Sex { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Возраст")]
        public string Age { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Квента")]
        public HttpPostedFileBase Quenta { get; set; }
    }

}
