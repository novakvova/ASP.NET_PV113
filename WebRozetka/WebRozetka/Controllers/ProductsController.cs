using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
        {
            var cat = _mapper.Map<ProductEntity>(model);
            //if (model.Image != null)
            //{
            //    cat.Image = await ImageWorker.SaveImageAsync(model.Image);
            //}
            //await _appEFContext.Categories.AddAsync(cat);
            //await _appEFContext.SaveChangesAsync();
            return Ok();
        }

    }
}
