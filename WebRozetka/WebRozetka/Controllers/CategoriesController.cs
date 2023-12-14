using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRozetka.Data;
using WebRozetka.Data.Entities;
using WebRozetka.Helpers;
using WebRozetka.Models.Category;

namespace WebRozetka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IMapper _mapper;

        public CategoriesController(AppEFContext appEFContext, IMapper mapper)
        {
            _appEFContext = appEFContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await _appEFContext.Categories
                .Where(c => !c.IsDeleted)
                .Select(x=>_mapper.Map<CategoryItemViewModel>(x))
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            var cat = _mapper.Map<CategoryEntity>(model);
            if (model.Image != null)
            {
                cat.Image = await ImageWorker.SaveImageAsync(model.Image);
            }
            await _appEFContext.Categories.AddAsync(cat);
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model)
        {
            var cat = _appEFContext.Categories
                .Where(c => !c.IsDeleted)
                .SingleOrDefault(x=>x.Id==model.Id);
            if (cat == null)
            {
                return NotFound();
            }

            if (model.Image != null)
            {
                string fileRemove = Path.Combine(Directory.GetCurrentDirectory(),"images",cat.Image);
                if(System.IO.File.Exists(fileRemove))
                {
                    System.IO.File.Delete(fileRemove);
                }
                cat.Image = await ImageWorker.SaveImageAsync(model.Image);
            }
            cat.Name=model.Name;
            cat.Description=model.Description;
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = _appEFContext.Categories
                .Where(c => !c.IsDeleted)
                .SingleOrDefault(x => x.Id == id);
            if (cat == null)
            {
                return NotFound();
            }
            cat.IsDeleted = true;
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _appEFContext.Categories
                .Where(c => !c.IsDeleted)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CategoryItemViewModel>(cat));
        }
    }
}
