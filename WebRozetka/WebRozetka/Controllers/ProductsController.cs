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
        public async Task<IActionResult> List([FromQuery] ProductSearchViewModel search)
        {
            var query = _appEFContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .AsQueryable();

            if(!string.IsNullOrEmpty(search.Name))
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));

            if (!string.IsNullOrEmpty(search.Description))
                query = query.Where(x => x.Description.ToLower().Contains(search.Description.ToLower()));

            if(search.CategoryId>0)
                query = query.Where(x => x.CategoryId==search.CategoryId);

            query = query.OrderBy(x => x.Id);

            int page = search.Page ?? 1;
            int pageSize = search.PageSize ?? 5;
            int count = query.Count();

            query = query
                .Skip(pageSize * (page-1))
                .Take(pageSize);

            var list = await query
                .Select(x => _mapper.Map<ProductItemViewModel>(x))
                .ToListAsync();

            ProductResultViewModel result = new ProductResultViewModel
            {
                TotalCount = count,
                List = list,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                PageIndex = page,
                PageSize = pageSize
            };

            return Ok(result);
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

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductEditViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Новий продукт є пустим!");
            }

            try
            {
                var product = _mapper.Map<ProductEntity>(model);
                _appEFContext.Products.Update(product); 
                await _appEFContext.SaveChangesAsync();

                var dbPhotos = _appEFContext.ProductImages.Where(x => x.ProductId == model.Id);

                if (dbPhotos != null)
                {
                    foreach (var photo in dbPhotos)
                    {
                        if (!model.OldPhotos.Any(x => x.Photo == photo.Name))
                        {
                            _appEFContext.ProductImages.Remove(photo);
                            ImageWorker.RemoveImage(photo.Name);
                        }
                        else
                        {
                            photo.Priority = model.OldPhotos.SingleOrDefault(x => x.Photo == photo.Name).Priority;
                        }
                    }
                }

                if (model.NewPhotos != null)
                {
                    foreach (var image in model.NewPhotos)
                    {
                        var imagePath = await ImageWorker.SaveImageAsync(image.Photo);

                        _appEFContext.ProductImages.Add(new ProductImageEntity { Name = imagePath, ProductId = product.Id, Priority = image.Priority });
                    }
                }

                await _appEFContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Сталася помилка при створенні продукту: " + ex.Message);
            }
        }
    }
}
