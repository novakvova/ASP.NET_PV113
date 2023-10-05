using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(int itemId)
        {
            var categoryList = new ProductListViewModel();
            categoryList.CategoryId = itemId;
            categoryList.CategoryName = _dataEFContext.Categories.Where(p => p.Id == itemId).FirstOrDefault().Name;
            categoryList.Products = _dataEFContext.Products
                    .Include(x => x.Category)
                    .Include(x => x.ProductImages)
                    .Where(p => p.CategoryId == itemId)
                    .Select(x => new ProductViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        Photos = x.ProductImages,
                        Category = x.Category,
                    })
                    .ToList();

            return View(categoryList);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categories = _dataEFContext.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editProduct = _dataEFContext.Products.FirstOrDefault(x => x.Id == id);

            if (editProduct == null)
            {
                return NotFound();
            }

            var categories = _dataEFContext.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            var images = _dataEFContext.ProductImages
                .Where(c => c.ProductId == id)
                .Select(x => $"/images/1200_{x.Name}")
                .ToList();

            var model = new ProductEditViewModel
            {
                Id = editProduct.Id,
                Name = editProduct.Name,
                CategoryId = editProduct.CategoryId,
                CategoryList = new SelectList(categories, "Value", "Text"),
                Images = images
            };

            SelectListItem selectedCategory = model.CategoryList.FirstOrDefault(x => x.Value == editProduct.CategoryId.ToString());

            if (selectedCategory != null)
            {
                selectedCategory.Selected = true;
            }

            return View(model);
        }

    }
}
