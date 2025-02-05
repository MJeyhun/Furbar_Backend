using Furbar.DAL;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public ProductDetailController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public IActionResult Index(int id)
        {
            productDetailVM ProductDetailVM = new();
            ProductDetailVM.Product = _appDbContext.Products
                .Include(p=>p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .Include(p=>p.ProductCategories)
                .ThenInclude(pc=>pc.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            ProductDetailVM.Products=_appDbContext.Products.Include(P=>P.ProductImages).ToList();
            return View(ProductDetailVM);
        }

    }
}
