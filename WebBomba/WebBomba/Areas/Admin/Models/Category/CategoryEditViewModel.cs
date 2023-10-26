using System.ComponentModel.DataAnnotations;

namespace WebBomba.Areas.Admin.Models.Category
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву категорії")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// Фото категорії
        /// </summary>
        public String ImageView { get; set; }
        /// <summary>
        /// якщо користувач хоче змінить фото
        /// </summary>
        [Display(Name = "Фото")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
