namespace WebRozetka.Models.Product
{
    public class ProductPhoto
    {
        public string Photo { get; set; }
        public byte Priority { get; set; }
    }
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public List<ProductPhoto> OldPhotos { get; set; }
        public List<ProductPhoto> NewPhotos { get; set; }
    }
}
