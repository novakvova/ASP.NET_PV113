using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBomba.Data.Entities
{
    [Table("tblCategories")]
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(500)]
        public string Name { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Image { get; set; }
    }
}
