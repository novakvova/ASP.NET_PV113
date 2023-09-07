using Microsoft.AspNetCore.Mvc;
using WebBomba.Data;
using WebBomba.Data.Entities;
using WebBomba.Models.Category;

namespace WebBomba.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataEFContext _dataEFContext;
        public CategoryController(DataEFContext dataEFContext)
        {
            _dataEFContext = dataEFContext;
        }
        //private static List<CategoryViewModel> list = new List<CategoryViewModel>();
        public IActionResult Index()
        {
            var list = _dataEFContext.Categories
                .Select(x=>new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image=x.Image
                })
                .ToList();
            return View(list);
        }
        //Метод використовуєть для відображення сторінки, де ми заповняємо інфомацію
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //Додати новий елемент
        [HttpPost]
        public IActionResult Add(CategoryAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string fileExt = Path.GetExtension(model.Image.FileName).ToLower();
            string imageName = Guid.NewGuid().ToString() + fileExt;
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
            using (var stream = new FileStream(Path.Combine(dir, imageName), FileMode.Create))
            {
                model.Image.CopyTo(stream);
            }

            CategoryEntity entity = new CategoryEntity();
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Image = imageName;
            _dataEFContext.Categories.Add(entity);
            _dataEFContext.SaveChanges();
            //вертає статус код 302 - потрібно перейти до списку категорій
            return RedirectToAction(nameof(Index));
        }
    }
}
