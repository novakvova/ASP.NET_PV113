using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBomba.Data.Entities;
using WebBomba.Data;
using WebBomba.Interfaces;
using WebBomba.Areas.Admin.Models.Category;
using Microsoft.AspNetCore.Authorization;
using WebBomba.Constants;

namespace WebBomba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class CategoryController : Controller
    {
        private readonly DataEFContext _dataEFContext;
        private readonly IImageWorker _imageWorker;
        private readonly IConfiguration _configuration;
        public CategoryController(DataEFContext dataEFContext,
            IImageWorker imageWorker,
            IConfiguration configuration)
        {
            _dataEFContext = dataEFContext;
            _imageWorker = imageWorker;
            _configuration = configuration;

        }
        //private static List<CategoryViewModel> list = new List<CategoryViewModel>();
        public IActionResult Index()
        {
            var list = _dataEFContext.Categories
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image
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

        //Метод використовуєть для відображення сторінки, де ми заповняємо інфомацію
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editProduct = _dataEFContext.Categories.SingleOrDefault(x => x.Id == id);
            if (editProduct == null)
            {
                return NotFound();
            }
            var model = new CategoryEditViewModel
            {
                Id = id,
                Name = editProduct.Name,
                Description = editProduct.Description,
                ImageView = "/images/300_" + editProduct.Image
            };
            return View(model);
        }

        //Метод використовуєть для відображення сторінки, де ми заповняємо інфомацію
        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            var category = _dataEFContext.Categories.SingleOrDefault(x => x.Id == model.Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Image != null)
            {
                //видаляю старі фото
                var imageSizes = _configuration.GetValue<string>("ImageSizes");
                var sizes = imageSizes.Split(",");
                foreach (var size in sizes)
                {
                    int width = int.Parse(size);
                    var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    System.IO.File.Delete(Path.Combine(dir, size + "_" + category.Image));
                }
                //зберігаємо нове фото
                string imageName = _imageWorker.ImageSave(model.Image);
                category.Image = imageName;
            }
            category.Name = model.Name;
            category.Description = model.Description;
            _dataEFContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
