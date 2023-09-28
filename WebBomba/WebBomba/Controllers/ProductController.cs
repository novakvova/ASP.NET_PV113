using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBomba.Data;
using WebBomba.Interfaces;
using WebBomba.Models.Product;

namespace WebBomba.Controllers
{
    public class ProductController : Controller
    {

        private readonly DataEFContext _dataEFContext;

        public ProductController(DataEFContext dataEFContext)
        {
            _dataEFContext = dataEFContext;
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
    }
}
