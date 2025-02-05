using Furbar.DAL;
using Furbar.Models.Accounts;
using Furbar.Models;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class WishListController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public WishListController(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task< IActionResult> Index()
        {
            List<CartVM> products = new();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (Request.Cookies["wishitems"] != null)
                {
                    products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["wishitems"]);
                    foreach (var item in products)
                    {
                        Product currentProduct = _appDbContext.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                        item.Name = currentProduct.Name;
                        item.Price = currentProduct.Price;
                        item.ImageUrl = currentProduct.ProductImages.FirstOrDefault().ImageUrl;
                    }
                
                }



            }
            return View(products);
        }
        public async Task<IActionResult> Add(int id)
        {
            if (id == null) return NotFound();

            Product product = await _appDbContext.Products.FindAsync(id);

            List<CartVM> products;
            if (Request.Cookies["wishitems"] == null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["wishitems"]);
            }
            CartVM existProduct = products.FirstOrDefault(p => p.Id == id);
            if (existProduct == null)
            {
                CartVM basketVM = new();
                basketVM.Id = product.Id;
                basketVM.BasketCount = 1;
                basketVM.Price = product.Price;
                products.Add(basketVM);
            }
            else
            {
                existProduct.BasketCount++;
            }
            Response.Cookies.Append("wishitems", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index), "Home");



        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            List<CartVM> products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["wishitems"]);
            products.Remove(products.FirstOrDefault(p => p.Id == id));
            Response.Cookies.Append("wishitems", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index));

        }
    }
}
