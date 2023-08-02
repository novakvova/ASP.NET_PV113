using Microsoft.AspNetCore.Mvc;
using WebBomba.Models.Category;

namespace WebBomba.Controllers
{
    public class CategoryController : Controller
    {
        private static List<CategoryViewModel> list = new List<CategoryViewModel>();
        public IActionResult Index()
        {
            return View(list);
        }
        //Додати новий елемент
        public IActionResult Add(CategoryAddViewModel model)
        {
            list.Add(
            new CategoryViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image
            }
            );
            //вертає статус код 302 - потрібно перейти до списку категорій
            return RedirectToAction(nameof(Index));
        }
    }
}
