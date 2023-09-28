using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebBomba.Models.Product
{
    public class ProductAddViewModel
    {
            [Display(Name = "Назва")]
            [Required(ErrorMessage = "Вкажіть назву продукту")]
            [StringLength(500, ErrorMessage = "Назва продукту не повинна перевищувати 500 символів")]
            public string Name { get; set; }

            [Display(Name = "Опис")]
            [StringLength(4000, ErrorMessage = "Опис продукту не повинен перевищувати 4000 символів")]
            public string Description { get; set; }

            [Display(Name = "Категорія")]
            [Required(ErrorMessage = "Оберіть категорію")]
            public int CategoryId { get; set; }

            [Display(Name = "Фото")]
            public ICollection<IFormFile> Photos { get; set; }

        //список усіх категорій, які відобрається для продукта
        public SelectList CategoryList { get; set; }
    }
}
