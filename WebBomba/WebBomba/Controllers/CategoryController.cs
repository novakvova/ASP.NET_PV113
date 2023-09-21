using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBomba.Data;
using WebBomba.Data.Entities;
using WebBomba.Helpers;
using WebBomba.Interfaces;
using WebBomba.Models.Category;

namespace WebBomba.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataEFContext _dataEFContext;
        private readonly IImageWorker _imageWorker;
        public CategoryController(DataEFContext dataEFContext, IImageWorker imageWorker)
        {
            _dataEFContext = dataEFContext;
            _imageWorker = imageWorker;
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
            try
            {
                if (model.Image == null)
                {
                    ModelState.AddModelError("Image", "Оберіть фото!");
                }
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                if (!ModelState.IsValid)
                {
                    return View();
                }

                string imageName = _imageWorker.ImageSave(model.Image);

                CategoryEntity entity = new CategoryEntity();
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Image = imageName;
                _dataEFContext.Categories.Add(entity);
                _dataEFContext.SaveChanges();
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("-----------RunTime---------" + elapsedTime);
                //вертає статус код 302 - потрібно перейти до списку категорій
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Щось пішло не так " + ex.Message);
                return View();
            }
           
        }
    }
}
