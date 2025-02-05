using Furbar.DAL;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.Services.Subscribtion;
using Furbar.ViewModels.ColorAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ColorController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
       
        }

        public ActionResult Index()
        {
            ColorAdminVM colorVM = new();
            colorVM.Colors=_appDbContext.Colors.AsNoTracking().Where(c=>!c.IsDeleted).ToList();
            return View(colorVM);
        }


        public ActionResult Details(int id)
        {
            if (id == null) return NotFound();
            ColorDetailVM colorVM = new();
            colorVM.Name = _appDbContext.Colors.FirstOrDefault(c => c.Id == id).Name;

            return View(colorVM);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ColorCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();
         
            Color color = new();
            if (_appDbContext.Colors.Any(c => c.Name == createVM.Name && !c.IsDeleted))
            {
                ModelState.AddModelError("Name", "This color name exists!Please choose other one");
                return View();
            }
            color.Name=createVM.Name;
            color.CreatedDate = DateTime.Now;
            color.IsDeleted= false;
     
            _appDbContext.Colors.Add(color);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Color color = _appDbContext.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null) return NotFound();
            return View(new ColorEditVM{Name=color.Name});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ColorEditVM editVm)
        {
            if (id == null) return NotFound();
            Color color =  _appDbContext.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null) return NotFound();
            if (_appDbContext.Colors.Any(c => c.Name == editVm.Name && c.Id!=color.Id && !c.IsDeleted)) 
            {
                ModelState.AddModelError("Name", "This color name exists!Please choose other one");
                return View();
            }
            color.Name= editVm.Name??color.Name;
            color.UpdatedDate = DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
  
        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Color color = _appDbContext.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null) return NotFound();
            
            color.IsDeleted = true;
            color.DeletedDate = DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }
    }
}
