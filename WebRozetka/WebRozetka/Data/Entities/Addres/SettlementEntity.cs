using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRozetka.Data.Entities.Addres
{
    [Table("tblSettlements")]
    public class SettlementEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Ref { get; set; }
        [Required, StringLength(200)]
        public string Description { get; set; }
        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public virtual AreaEntity Area { get; set; }
    }
}
