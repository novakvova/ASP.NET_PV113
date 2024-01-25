using System.ComponentModel.DataAnnotations.Schema;
using WebRozetka.Data.Entities.Identity;

namespace WebRozetka.Data.Entities.Orders
{
    [Table("tblOrders")]
    public class OrderEntity : BaseEntity<int>
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual OrderStatusEntity OrderStatus { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
