using Microsoft.AspNetCore.Mvc;

namespace WebRozetka.Models.Product
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }

        [BindProperty(Name = "images[]")]
        public List<IFormFile> Images { get; set; }
    }
}
