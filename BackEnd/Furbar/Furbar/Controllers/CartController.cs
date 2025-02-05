using Furbar.DAL;
using Furbar.Models.Accounts;
using Furbar.Models;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Furbar.Models.Sales;

namespace Furbar.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public CartController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
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
                }
              
   
               
               }
            return View(products);
            }
        public async Task<IActionResult> RemoveOneItem(int id)
            {
                if (id == null) return NotFound();
                if (User.Identity.IsAuthenticated)
                {
                    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                    Product product = user.CartProducts.FirstOrDefault(p => p.Id == id);
                    user.CartProducts.Remove(product);
                }
                return RedirectToAction("Index");
            }

        public async Task<IActionResult> Add(int id)
        {
            if (id == null) return NotFound();

            Product product = await _appDbContext.Products.FindAsync(id);

            List<CartVM> products;
            if (Request.Cookies["basket"] == null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["basket"]);
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
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index), "Home");



        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            List<CartVM> products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["basket"]);
            products.Remove(products.FirstOrDefault(p => p.Id == id));
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index));

        }
        public IActionResult IncreaseCount(int id)
        {
            List<CartVM> products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["basket"]);
            CartVM product = products.FirstOrDefault(p => p.Id == id);
            product.BasketCount++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DecreaseCount(int id)
        {
            List<CartVM> products = JsonConvert.DeserializeObject<List<CartVM>>(Request.Cookies["basket"]);
            CartVM product = products.FirstOrDefault(p => p.Id == id);
            if (product.BasketCount > 1)
            {
                product.BasketCount--;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Sale()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                Sale sale = new();
                sale.AppUserId = user.Id;
                sale.CreatedDate = DateTime.Now;
                List<SalesProducts> salesProducts = new();
                List<CartVM> basketVMs = JsonConvert.DeserializeObject<List<CartVM>>
                    (Request.Cookies["basket"]);

                foreach (var basketProduct in basketVMs)
                {

                    Product product = _appDbContext.Products.FirstOrDefault(p => p.Id == basketProduct.Id);
                    if (basketProduct.BasketCount > product.Count)
                    {
                        TempData["NotInStock"] = $"There is not enough {product.Name} in stock";
                        return RedirectToAction("ShowBasket");
                    }
                    SalesProducts salesProduct = new();
                    salesProduct.ProductId = product.Id;
                    salesProduct.SaleId = sale.Id;
                    salesProduct.Count = basketProduct.BasketCount;
                    salesProduct.Price = product.Price;
                    product.Count = CalculateCount(product.Count, basketProduct.BasketCount);
                    salesProducts.Add(salesProduct);

                }
                sale.SalesProducts = salesProducts;
                sale.TotalPrice = basketVMs.Sum(p => p.Price * p.BasketCount);
                _appDbContext.Sales.Add(sale);
                _appDbContext.SaveChanges();
                TempData["Success"] = "Order succesfuly complete";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        private int CalculateCount(int stockCount, int orderedCount)
                {
                    return stockCount - orderedCount;
                }
    }
    } 

