using System.ComponentModel.DataAnnotations;
using WebBomba.Data.Entities;

namespace WebBomba.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле Name є обов'язковим.")]
        [MaxLength(255, ErrorMessage = "Максимальна довжина поля Name - 255 символів.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле CategoryId є обов'язковим.")]
        public int CategoryId { get; set; }

        [Display(Name = "Категорія")]
        public CategoryEntity Category { get; set; }

        public ICollection<ProductImageEntity> Photos { get; set; }
    }
}
