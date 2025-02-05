using Furbar.DAL;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Furbar.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager; 
        private readonly AppDbContext _appDbContext;

        public HeaderViewComponent(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Fullname = string.Empty;
            List<CartVM> products = new();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (Request.Cookies["basket"] != null)
                {
                    products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["basket"]);
                    foreach (var item in products)
                    {
                        Product currentProduct = _appDbContext.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                        item.Name = currentProduct.Name;
                        item.Price = currentProduct.Price;
                        item.ImageUrl = currentProduct.ProductImages.FirstOrDefault().ImageUrl;
                    }
                    if (products == null)
                    {
                        ViewBag.CartCount = 0;
                    }
                    else
                    {

                        ViewBag.CartCount = products.Sum(p => p.BasketCount);
                        ViewBag.TotalPrice = products.Sum(p => p.BasketCount * p.Price);

                    }
                    ViewBag.CartProducts = products;
                }

                ViewBag.Fullname = user.Fullname;

             
               
            }
            return View();
        }
    }
}
