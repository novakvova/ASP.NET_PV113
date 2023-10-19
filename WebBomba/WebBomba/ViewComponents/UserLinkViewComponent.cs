using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBomba.Data.Entities.Identity;
using WebBomba.Models.Account;

namespace WebBomba.ViewComponents;

public class UserLinkViewComponent : ViewComponent
{
    private readonly UserManager<UserEntity> _userManager;
    
    public UserLinkViewComponent(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }
    public IViewComponentResult Invoke()
    {
        var userName = User.Identity?.Name;
        var model = new UserLinkViewModel();
        if(userName != null)
        {
            var user = _userManager.FindByEmailAsync(userName).Result;
            model.Image = user.Image;
            model.Name = $"{user.LastName} {user.FirstName}";
        }

        // Pass the model to the view
        return View(model);
    }
}