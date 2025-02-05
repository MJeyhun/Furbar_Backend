using Furbar.DAL;
using Furbar.Models;
using Furbar.ViewModels;
using Furbar.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index(string search,int page = 1, int take = 5)
        {
            blogVM BlogVM = new();
            var query = _appDbContext.Blogs
                    .Include(p => p.BlogImages).Include(b => b.AppUser).Where(p => !p.IsDeleted);
            if (search == null)
            {
                BlogVM.Blogs = query
                     .Skip(take * (page - 1))
                     .Take(take)
              .AsNoTracking().ToList();
            }
            else 
            {
                BlogVM.Blogs = query
                     .Skip(take * (page - 1))
                     .Take(take)
                    .Where(b=>b.Title.ToLower().Trim().Contains(search.ToLower().Trim()))
                    .AsNoTracking().ToList();
            }
            int pageCount = (int)Math.Ceiling((decimal)query.ToList().Count() / take);

            PaginationVM<Blog> pagination = new(query.ToList(), pageCount, page);
            BlogVM.paginationVM= pagination;
            return View(BlogVM);
        }
    }
}
