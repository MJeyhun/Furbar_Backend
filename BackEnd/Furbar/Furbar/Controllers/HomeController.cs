using Furbar.DAL;
using Furbar.Models;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Furbar.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            homeVM HomeVm = new();
            HomeVm.Products = _appDbContext.Products.Include(p=>p.ProductImages).AsNoTracking().ToList();
            HomeVm.Sliders= _appDbContext.Sliders.AsNoTracking().ToList();
            return View(HomeVm);
        }

        

      
    }
}