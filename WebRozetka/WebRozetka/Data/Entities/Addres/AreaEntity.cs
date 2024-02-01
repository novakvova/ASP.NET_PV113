using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebRozetka.Data.Entities.Addres
{

    [Table("tblAreas")]
    public class AreaEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Ref { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        public virtual ICollection<SettlementEntity> Settlements { get; set; }
    }
}
