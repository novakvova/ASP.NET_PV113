using System.ComponentModel.DataAnnotations;

namespace WebBomba.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вкажіть електронну пошту")]
        [EmailAddress(ErrorMessage = "Недійсна адреса електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Вкажіть ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Вкажіть прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Виберіть зображення")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
