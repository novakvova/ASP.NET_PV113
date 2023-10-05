using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebBomba.Models.Product
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле 'Назва' є обов'язковим.")]
        [MaxLength(255, ErrorMessage = "Максимальна довжина поля 'Назва' - 255 символів.")]
        public string Name { get; set; }

        [Display(Name = "Категорія")]
        [Required(ErrorMessage = "Поле 'Категорія' є обов'язковим.")]
        public int CategoryId { get; set; } //change

        [Display(Name = "Ціна")]
        [Required(ErrorMessage = "Поле 'Ціна' є обов'язковим.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        [Display(Name = "Кількість")]
        [Required(ErrorMessage = "Поле 'Кількість' є обов'язковим.")]
        public int StockQuantity { get; set; }

        public ICollection<string> Images { get; set; }

        [Required(ErrorMessage = "Поле 'Фото' є обов'язковим.")]
        public ICollection<IFormFile> Photos { get; set; }
        public SelectList CategoryList { get; set; }
    }
}
