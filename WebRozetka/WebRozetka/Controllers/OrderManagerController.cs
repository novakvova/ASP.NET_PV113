using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebRozetka.Data;
using WebRozetka.Data.Entities.Identity;
using WebRozetka.Data.Entities.Orders;
using WebRozetka.Models.Category;
using WebRozetka.Models.Orders;

namespace WebRozetka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderManagerController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public OrderManagerController(AppEFContext appEFContext,
            UserManager<UserEntity> userManager,
            IMapper mapper)
        {
            _appEFContext = appEFContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("add-basket")]
        public async Task<IActionResult> AddBasket([FromBody] BasketAddViewModel model)
        {
            string email = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(email);
            var entity = _appEFContext.Baskets
                .SingleOrDefault(x => x.UserId == user.Id && x.ProductId == model.PoductId);
            if (entity == null)
            {
                entity = new BasketEntity
                {
                    ProductId = model.PoductId,
                    UserId = user.Id,
                    Count = model.Count,
                    DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };
                _appEFContext.Baskets.Add(entity);
            }
            else
            {
                entity.Count += model.Count;
            }
            _appEFContext.SaveChanges();
            var result = _mapper.Map<BasketItemViewModel>(entity);
            return Ok(result);
        }

        [HttpGet("list-basket")]
        public async Task<IActionResult> ListBasket()
        {
            string email = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(email);
            var model = _appEFContext.Baskets
                .Include(x => x.Product)
                .Where(x => x.UserId == user.Id)
                .Select(x => _mapper.Map<BasketItemViewModel>(x));
            return Ok(model);
        }
    }
}
