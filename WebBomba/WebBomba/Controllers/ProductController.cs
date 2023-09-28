using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBomba.Data;
using WebBomba.Data.Entities;
using WebBomba.Interfaces;
using WebBomba.Models.Product;
using WebBomba.Services;

namespace WebBomba.Controllers
{
    public class ProductController : Controller
    {

        private readonly DataEFContext _dataEFContext;
        private readonly IImageWorker _imageWorker;

        public ProductController(DataEFContext dataEFContext, IImageWorker imageWorker)
        {
            _dataEFContext = dataEFContext;
            _imageWorker = imageWorker;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categories = _dataEFContext.Categories
                .Select(x=> new {Value = x.Id, Text = x.Name})
                .ToList();
            var model = new ProductAddViewModel
            {
                CategoryList = new SelectList(categories, "Value", "Text")
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ProductAddViewModel model)
        {
            var pEntity = new ProductEntity
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Description = model.Description
            };

            _dataEFContext.Products.Add(pEntity);
            _dataEFContext.SaveChanges();

            int i = 0;
            foreach (var p in model.Photos)
            {
                _dataEFContext.ProductImages.Add(new ProductImageEntity
                {
                    Priotity = i++,
                    ProductId = pEntity.Id,
                    Name = _imageWorker.ImageSave(p)
                });
            }

            _dataEFContext.SaveChanges();

            var categories = _dataEFContext.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();
            model.CategoryList = new SelectList(categories, "Value", "Text");

            return View(model);
        }
    }
}
