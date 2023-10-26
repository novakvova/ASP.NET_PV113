using System.ComponentModel.DataAnnotations;

namespace WebBomba.Areas.Admin.Models.Category
{
    public class CategoryAddViewModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву категорії")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Фото")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
