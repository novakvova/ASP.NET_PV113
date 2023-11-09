namespace WebRozetka.Models.Category
{
    public class CategoryCreateViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
    }
}
