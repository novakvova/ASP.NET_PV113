using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRozetka.Data.Entities
{
    [Table("tblCategories")]
    public class CategoryEntity : BaseEntity<int>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Image { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
