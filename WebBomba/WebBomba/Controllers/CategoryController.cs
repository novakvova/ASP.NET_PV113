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
       

    }
}
