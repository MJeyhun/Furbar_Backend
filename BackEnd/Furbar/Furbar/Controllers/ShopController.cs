using Furbar.DAL;
using Furbar.Models;
using Furbar.ViewModels;
using Furbar.ViewModels.Pagination;
using Furbar.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Furbar.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ShopController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index(string search, int page = 1, int take = 1)
        {
            shopVM ShopVM = new();
            var query = _appDbContext.Products
                .Include(p => p.ProductImages).Where(p => !p.IsDeleted);
            if (search == null)
            {
                ShopVM.Products = query
                    .Skip(take * (page - 1))
                    .Take(take)
                    .AsNoTracking().ToList();
            }
            else
            {
                ShopVM.Products = query
                    .Skip(take * (page - 1))
                    .Take(take)
                    .Where(p => p.Name.ToLower().Trim().Contains(search.ToLower().Trim()))
                    .AsNoTracking().ToList();
            }
            int pageCount = (int)Math.Ceiling((decimal)query.ToList().Count() / take);

            PaginationVM<Product> pagination = new(query.ToList(), pageCount, page);
            ShopVM.paginationVM= pagination;
            return View(ShopVM);
        }
        //private int CalculatePageCount(IIncludableQueryable<Product, Category> products, int take)
        //{
        //    return (int)Math.Ceiling((decimal)products.Count() / take);
        //}
    }
}
