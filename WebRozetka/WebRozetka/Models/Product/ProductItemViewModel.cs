using Microsoft.AspNetCore.Mvc;

namespace WebRozetka.Models.Product
{
    public class ProductItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public List<string> Images { get; set; }
    }
}
