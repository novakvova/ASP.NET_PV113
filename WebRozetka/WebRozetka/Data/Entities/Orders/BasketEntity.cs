using System.ComponentModel.DataAnnotations.Schema;
using WebRozetka.Data.Entities.Identity;

namespace WebRozetka.Data.Entities.Orders
{
    [Table("tblBaskets")]
    public class BasketEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual ProductEntity Product { get; set; }
        public short Count { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
