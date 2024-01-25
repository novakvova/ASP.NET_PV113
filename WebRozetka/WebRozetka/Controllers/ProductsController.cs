using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRozetka.Data;
using WebRozetka.Data.Entities;
using WebRozetka.Helpers;
using WebRozetka.Models.Product;

namespace WebRozetka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IMapper _mapper;

        public ProductsController(AppEFContext appEFContext, IMapper mapper)
        {
            _appEFContext = appEFContext;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous] //До списку продутів дати доступ не авторизованим корисувачам
        public async Task<IActionResult> List()
        {
            var model = await _appEFContext.Products
                .Include(x=>x.Category)
                .Include(x=>x.ProductImages)
                .Select(x=>_mapper.Map<ProductItemViewModel>(x))
                .ToListAsync();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
        {
            var product = _mapper.Map<ProductEntity>(model);
            _appEFContext.Products.Add(product);
            await _appEFContext.SaveChangesAsync();
            byte p = 1;
            foreach(var image in model.Images)
            {
                if(image != null)
                {
                    ProductImageEntity pi = new ProductImageEntity();
                    pi.Priority = p;
                    pi.Name = await ImageWorker.SaveImageAsync(image);
                    pi.ProductId = product.Id;
                    _appEFContext.ProductImages.Add(pi);
                    await _appEFContext.SaveChangesAsync();
                    p++;
                }
            }
            return Ok();
        }

    }
}
