using System.ComponentModel.DataAnnotations;

namespace WebBomba.Models.Category
{
    public class CategoryAddViewModel
    {
        [Required(ErrorMessage = "Вкажіть назву категорії")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
