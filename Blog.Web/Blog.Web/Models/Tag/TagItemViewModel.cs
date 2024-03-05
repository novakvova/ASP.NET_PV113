using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.Tag
{
    public class TagItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
    }
}
