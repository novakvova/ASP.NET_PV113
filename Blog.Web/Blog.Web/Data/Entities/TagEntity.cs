using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Web.Data.Entities
{
    [Table("tblTags")]
    public class TagEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string UrlSlug { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

    }
}
