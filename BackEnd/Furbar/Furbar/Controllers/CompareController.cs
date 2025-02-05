using Furbar.DAL;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class CompareController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CompareController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            compareVM CompareVM = new();
            CompareVM.Products = _appDbContext.Products
                .Where(p=>p.IsCompared)
                .Include(P => P.ProductImages)
                .Include(P =>P.ProductCategories)
                .Include(p => p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .OrderByDescending(p=>p.Id)
                .Take(3)
                .AsNoTracking().ToList();
            return View(CompareVM);
        }
    }
}
