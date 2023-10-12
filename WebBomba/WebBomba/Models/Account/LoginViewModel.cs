using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebBomba.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Електронна пошта")]
        [Required(ErrorMessage = "Вкажіть пошту")]
        public string Email { get; set; }
        [Display(Name = "Вкажіть пароль")]
        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
