namespace WebBomba.Models.Product
{
    public class ProductListViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
