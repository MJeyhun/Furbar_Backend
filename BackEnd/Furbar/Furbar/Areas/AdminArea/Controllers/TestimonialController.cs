using Furbar.DAL;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.ViewModels.TestimonialViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        public TestimonialController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            TestimonialVM testimonialVM = new();
            testimonialVM.Testimonials=_appDbContext.Testimonials.AsNoTracking().Where(t=>!t.IsDeleted).Include(t=>t.AppUser).ToList();
            return View(testimonialVM);
        }


        public ActionResult Details(int id)
        {
            if (id == null) return NotFound();
            Testimonial testimonial=_appDbContext.Testimonials.AsNoTracking().Where(t=>!t.IsDeleted).Include(t => t.AppUser).FirstOrDefault(t=>t.Id == id);
            if(testimonial == null) return NotFound();
            return View(new TestimonialDetailVM {Content=testimonial.Content, Username=testimonial.AppUser.UserName });
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();
            Testimonial testimonial= new();
            testimonial.Content=createVM.Content;
            AppUser user= new AppUser();
            user= await _userManager.FindByNameAsync(createVM.Username);
            testimonial.AppUserId = user.Id;
            testimonial.AppUser = user;
            testimonial.CreatedDate = DateTime.Now;
            testimonial.IsDeleted=false;
            _appDbContext.Testimonials.Add(testimonial);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

 
        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Testimonial testimonial = _appDbContext.Testimonials.Where(t => !t.IsDeleted).Include(t => t.AppUser).FirstOrDefault(t => t.Id == id);
            if (testimonial == null) return NotFound();

            return View(new TestimonialEditVM {Content=testimonial.Content,Username=testimonial.AppUser.UserName });
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TestimonialEditVM editVM)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return NotFound();
            Testimonial testimonial = _appDbContext.Testimonials.Where(t => !t.IsDeleted).Include(t => t.AppUser).FirstOrDefault(t => t.Id == id);
            if (testimonial == null) return NotFound();
            AppUser user = new AppUser();
            user = await _userManager.FindByNameAsync(editVM.Username);
            testimonial.AppUserId = user.Id;
            testimonial.AppUser = user;
            testimonial.UpdatedDate= DateTime.Now;
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Testimonial testimonial = _appDbContext.Testimonials.Where(t => !t.IsDeleted).Include(t => t.AppUser).FirstOrDefault(t => t.Id == id);
            if (testimonial == null) return NotFound();
            testimonial.DeletedDate= DateTime.Now;
            testimonial.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
