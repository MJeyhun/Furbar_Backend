using Furbar.DAL;
using Furbar.Extensions;
using Furbar.Models;
using Furbar.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class SliderAdminController : Controller
    {
        private readonly AppDbContext _appDbContext;

       

        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderAdminController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Index()
        {
            SliderVM sliderVM = new();
            sliderVM.Sliders=_appDbContext.Sliders.AsNoTracking().ToList();
            return View(sliderVM);
        }
        public ActionResult Details(int id)
        {
            if(id==null) return NotFound();
            Slider slider=_appDbContext.Sliders.Where(s=>!s.IsDeleted).FirstOrDefault(s=>s.Id==id);    
            if(slider==null) return NotFound();
            SliderDetailVM sliderDetailVM = new();
            sliderDetailVM.Title= slider.Title;
            sliderDetailVM.Description= slider.Description;
            sliderDetailVM.ImageUrl= slider.ImageUrl;

            return View(sliderDetailVM);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SliderCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();

            Slider slider = new();

            if (!createVM.Image.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }

            slider.ImageUrl = createVM.Image.SaveImage(_webHostEnvironment, "images/slider", createVM.Image.FileName);
            slider.Title = createVM.Title;
            slider.Description = createVM.Description;
            slider.CreatedDate= DateTime.Now;
            slider.IsDeleted = false;

            _appDbContext.Sliders.Add(slider);
             _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Sliders.Where(s => !s.IsDeleted).FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();

            return View(new SliderEditVM {Title=slider.Title,Description=slider.Description });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SliderEditVM editVM)
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Sliders.Where(s => !s.IsDeleted).FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            if (!ModelState.IsValid) return View();

            if (editVM.Image != null)

            {
                if (!editVM.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "only image");
                    return View();
                }

                slider.ImageUrl = editVM.Image.SaveImage(_webHostEnvironment, "images/slider", editVM.Image.FileName);
            }

            slider.Title = editVM.Title ?? slider.Title;
            slider.Description = editVM.Description ?? slider.Description;
            slider.UpdatedDate= DateTime.Now;


             _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Sliders.Where(s => !s.IsDeleted).FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            slider.IsDeleted= true;
            slider.DeletedDate= DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
