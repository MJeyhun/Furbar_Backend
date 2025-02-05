using Furbar.Extensions;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Furbar.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager; 
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MyAccountController(UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task< IActionResult> Index()
        {
            MyAccountEditVM vm = new();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.Fullname = user.Fullname;
                vm.Fullname=user.Fullname;
                ViewBag.Email = user.Email;
                ViewBag.Username = user.UserName;

                ViewBag.ImageUrl=user.ImageUrl;
            }
            return View(vm);
        }
        public async  Task<IActionResult> Edit(MyAccountEditVM editVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (!editVM.Image.IsImage())
                {
                    ModelState.AddModelError("Images", "only image");
                    return View();
                }
                if (editVM.Image.CheckImageSize(500))
                {
                    ModelState.AddModelError("Images", "Olcusu boyukdur");
                    return View();
                }

                user.ImageUrl = editVM.Image.SaveImage(_webHostEnvironment, "images/author", editVM.Image.FileName);
                user.Fullname = editVM.Fullname;
                
            }
               
    
            return RedirectToAction("Index");   
        }
    }
}
