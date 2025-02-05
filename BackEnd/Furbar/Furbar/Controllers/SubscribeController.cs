using Furbar.Models.Accounts;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Furbar.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public SubscribeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Subscribed(string email)
        {
            AppUser user=_userManager.Users.FirstOrDefault(u=>u.Email==email);
            if (user == null) 
            {
                return RedirectToAction("Index", "Home");
            }
            user.IsSubscribed = true;
             await _userManager.UpdateAsync(user);
            return RedirectToAction("Index","Home");
        }
    }
}
