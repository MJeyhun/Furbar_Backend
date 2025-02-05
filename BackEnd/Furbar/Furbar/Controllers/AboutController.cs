using Furbar.DAL;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public AboutController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public ActionResult Index()
        {
            aboutVM AboutVM = new ();
            AboutVM.Testimonials=_appDbContext.Testimonials.AsNoTracking().Include(t=>t.AppUser).ToList();
            AboutVM.Workers = _appDbContext.Workers.AsNoTracking().ToList();
            return View(AboutVM);
        }

    }
}
