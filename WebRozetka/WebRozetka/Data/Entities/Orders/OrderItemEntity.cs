using System.ComponentModel.DataAnnotations.Schema;

namespace WebRozetka.Data.Entities.Orders
{
    [Table("tblOrderItems")]
    public class OrderItemEntity : BaseEntity<int> 
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public short Quantity { get; set; }
        public decimal PriceBy { get; set; }

        public virtual ProductEntity Product { get; set; }
        public virtual OrderEntity Order { get; set; }

    }
}
