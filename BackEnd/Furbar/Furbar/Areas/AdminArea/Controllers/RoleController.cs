using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Furbar.ViewModels.Account.Roles;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }
        public async Task<IActionResult> Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName)) 
            {
              var result=  await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index");
                };
                //else 
                //{
                //    result.Errors
                //}
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id) 
        {
            if(id==null) return View();
            var deleteRole = await _roleManager.FindByIdAsync(id);
            if(deleteRole==null) return NotFound();
           await _roleManager.DeleteAsync(deleteRole);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null) return View();
            var role = await _roleManager.FindByIdAsync(id);
            RoleDetailVM roleDetail = new RoleDetailVM { Name = role.Name };
            return View(roleDetail);
        }

    }
}
