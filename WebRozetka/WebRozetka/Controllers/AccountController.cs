using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebRozetka.Data.Entities.Identity;
using WebRozetka.Interfaces;
using WebRozetka.Models.Account;

namespace WebRozetka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private IJwtTokenService _jwtTokenService;
        public AccountController(UserManager<UserEntity> userManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest("Не вірно вказано дані");

                if(!await _userManager.CheckPasswordAsync(user, model.Password))
                    return BadRequest("Не вірно вказано дані");

                var token = await _jwtTokenService.CreateTokenAsync(user);
                return Ok(new { token });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
