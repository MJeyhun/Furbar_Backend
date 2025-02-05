using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Furbar.Models.Accounts;
using Furbar.ViewModels.Account.Users;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index(string search)
        {
            var users=search!=null ?
                _userManager.Users
                .Where(u => u.Fullname.ToLower().Trim()
                .Contains(search.ToLower().Trim())).ToList():
                _userManager.Users.ToList();

            return View(users);
        }
        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles=await _userManager.GetRolesAsync(user);
            return View(new UserDetailVM 
            {
                User= user,
                UserRoles=userRoles
            });
        }
        public async Task<IActionResult> Edit(string id) 
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else 
            {
                user.IsActive = true;
            }
            var result = await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
           user.IsDeleted=true;
            var result = await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditRole(string id) 
        {
            if(id==null) return NotFound();
            AppUser user =await _userManager.FindByIdAsync(id);
            var userRolers = await _userManager.GetRolesAsync(user);
            var allRoles =await  _roleManager.Roles.ToListAsync();
            if(user==null) return NotFound();

            return View(new ChangeRoleVM
            {
                User = user,

                UserRoles = userRolers,
                AllRoles=allRoles
            }) ;
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string id,List<string> roles) 
        {
            AppUser user=await _userManager.FindByIdAsync(id);
            if(user==null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roles);
            return RedirectToAction(nameof(Index),"user");
        }
    }
}
