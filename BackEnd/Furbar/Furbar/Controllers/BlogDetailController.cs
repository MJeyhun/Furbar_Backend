using Furbar.DAL;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Furbar.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public BlogDetailController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserId = user.Id;
            }
            Blog blog=_appDbContext.Blogs.Include(b=>b.BlogImages).Include(b=>b.AppUser).Include(b=>b.Comments).ThenInclude(bc=>bc.AppUser).FirstOrDefault(b=>b.Id==id);
            blogDetailVM blogVM = new();
            blogVM.Blog=blog;
            blogVM.Blogs=_appDbContext.Blogs.Include(b => b.BlogImages).AsNoTracking().ToList();
            if (blog.Comments == null)
            {
                blogVM.CommentCount = 0;
            }
            else 
            {
                blogVM.CommentCount=blog.Comments.Count();
           


            }
            
            return View(blogVM);
        }
        public async Task<IActionResult> AddComment(string commentMessage, int blogId)
        {
            if (blogId == null) return NotFound();
            if (commentMessage == null)
            {
                return RedirectToAction("index", new { id = blogId });
            }
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound();
            }
            else
            {
                return RedirectToAction("login", "account");
            }
            Comment comment = new();
            comment.CreatedDate = DateTime.Now;
            comment.Message = commentMessage;
            comment.AppUserId = user.Id;
            comment.BlogId = blogId;
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("index", new { id = blogId });
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (id == null) return NotFound();
            Comment comment = await _appDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
            var result = _appDbContext.ChangeTracker.Entries();
            _appDbContext.Comments.Remove(comment);

            _appDbContext.SaveChanges();
            return RedirectToAction("index", new { id = comment.BlogId });
        }
    }
}
