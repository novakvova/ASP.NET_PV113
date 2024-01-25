using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRozetka.Data.Entities.Orders
{
    [Table("tblOrderContactInfos")]
    public class OrderContactInfoEntity
    {
        [Key]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required, StringLength(200)]
        public string FirstName { get; set; }
        [Required, StringLength(200)]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
