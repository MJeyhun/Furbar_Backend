using Furbar.DAL;
using Furbar.Models;
using Furbar.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ActionResult Index()
        {
            CategoryVM categoryVM = new();
            categoryVM.Categories=_appDbContext.Categories.AsNoTracking().Where(c=>!c.IsDeleted).ToList();
            return View(categoryVM);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            if(id==null ) return NotFound();
            Category category= _appDbContext.Categories.Where(c=>!c.IsDeleted).FirstOrDefault(c=>c.Id==id);
            if (category == null) return NotFound();
            CategoryDetailsVM categoryDetailsVM = new();
            categoryDetailsVM.Name = category.Name;
            return View(categoryDetailsVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();
            Category category = new();
            if (_appDbContext.Colors.Any(c => c.Name == createVM.Name && !c.IsDeleted))
            {
                ModelState.AddModelError("Name", "This category name exists!Please choose other one");
                return View();
            }
            category.Name = createVM.Name;
            category.CreatedDate = DateTime.Now;
            category.IsDeleted = false;
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == id);
           if(category==null) return NotFound();
            return View(new CategoryEditVM{Name=category.Name });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryEditVM editVM)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View();

            if (_appDbContext.Colors.Any(c => c.Name == editVM.Name && c.Id != category.Id && !c.IsDeleted))
            {
                ModelState.AddModelError("Name", "This category name exists!Please choose other one");
                return View();
            }
            category.Name = editVM.Name;
            category.UpdatedDate = DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            category.IsDeleted= true;
            category.DeletedDate = DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
