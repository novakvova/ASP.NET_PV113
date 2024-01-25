namespace WebRozetka.Models.Orders
{
    public class BasketItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public short Count { get; set; }      
    }
}
