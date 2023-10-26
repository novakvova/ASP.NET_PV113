using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBomba.Data.Entities.Identity;
using WebBomba.Interfaces;
using WebBomba.Models;
using WebBomba.Models.Account;
using WebBomba.Services;

namespace WebBomba.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IImageWorker _imageWorker;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, IImageWorker imageWorker, 
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageWorker = imageWorker;
            _emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect("/");
                    }
                }
            }

            ModelState.AddModelError("", "Дані вказано не вірно");
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Користувач з такою електронною поштою вже зареєстрований.");
                return View(model);
            }

            var user = new UserEntity
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            // Додайте обробку для збереження зображення
            var imageName = _imageWorker.ImageSave(model.Image);
            user.Image = imageName;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

                var email = new EmailViewModel {
                    To = model.Email,
                    Subject = "Підтвердіть свою електронну пошту",
                    Body = $"Будь ласка, підтвердіть свою електронну пошту, перейшовши за цим <a href='{callbackUrl}'>посиланням</a>."
                };

                await _emailSender.SendAsync(email);
                
                
                // Реєстрація успішна, перенаправлення на сторінку входу
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code) {
            if (userId == null || code == null) {
                return RedirectToAction("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return RedirectToAction("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) {
                return RedirectToAction("Error");
            }

            return View("ConfirmedEmail");
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return Redirect(nameof(Login));
        }
    }
    
    
}
