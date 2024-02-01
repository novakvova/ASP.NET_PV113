using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRozetka.Data.Entities.Addres
{
    [Table("tblWarehouses")]
    public class WarehouseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Ref { get; set; }
        [Required, StringLength(200)]
        public string Description { get; set; }

        public int Number { get; set; }

        [ForeignKey("Settlement")]
        public int SettlementId { get; set; }
        public virtual SettlementEntity Settlement { get; set; }
    }
}
