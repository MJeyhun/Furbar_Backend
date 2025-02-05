using Furbar.DAL;
using Furbar.Extensions;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.Services.Subscribtion;
using Furbar.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")] 
    [Authorize(Roles ="Admin")]
    public class BlogAdminController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogAdminController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;

            _webHostEnvironment = webHostEnvironment;
           _userManager = userManager;
        }
        public ActionResult Index()
        {
            BlogVM blogVM = new();
            blogVM.Blogs = _appDbContext.Blogs.AsNoTracking()
                .Include(b => b.BlogImages)
                .Include(b=>b.AppUser)
                .Where(b => !b.IsDeleted).ToList();
            return View(blogVM);
        }

        public ActionResult Details(int id)
        {
            if (id == null) return NotFound();
            Blog blog=_appDbContext.Blogs.Where(b=>!b.IsDeleted).Include(b => b.AppUser).Include(b=>b.BlogImages).SingleOrDefault(b => b.Id == id);
            if(blog == null) return NotFound();
            BlogDetailVM detailVM = new();
            detailVM.Title= blog.Title;
            detailVM.Content= blog.Content;
            detailVM.AppUser=blog.AppUser;
            detailVM.BlogImages = blog.BlogImages;
            return View(detailVM);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();
            Blog blog = new();
            List<BlogImage>? blogImages = new();

            foreach (var photo in createVM.Images)
            {

                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Images", "only image");
                    return View();
                }
                if (photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Images", "Olcusu boyukdur");
                    return View();
                }
                BlogImage blogImage = new();
                blogImage.ImageUrl = photo.SaveImage(_webHostEnvironment, "images/blog", photo.FileName);
                blogImages.Add(blogImage);
            }


            blog.Title = createVM.Title;
            blog.Content = createVM.Content;
            blog.BlogImages = blogImages;
            blog.CreatedDate= DateTime.Now;
            blog.IsDeleted = false;
            blog.AppUser= await _userManager.FindByNameAsync(createVM.Username);
            _appDbContext.Blogs.Add(blog);
            _appDbContext.SaveChanges(); 
            List<AppUser> users = _userManager.Users.ToList();
            MessageToSubscribe message = new();
            message.SendMessageSubscribed(users, "Blog");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Blog blog = _appDbContext.Blogs.Where(b => !b.IsDeleted).Include(b=>b.AppUser).SingleOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();

            return View(new BlogEditVM {Title=blog.Title,Content=blog.Content,Username=blog.AppUser.UserName });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditVM editVM)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return NotFound();
            Blog blog = _appDbContext.Blogs.Where(b => !b.IsDeleted).SingleOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            List<BlogImage> blogImages = new();
            if (editVM.Images != null)
            {
                foreach (var photo in editVM.Images)
                {
                    if (!photo.IsImage())
                    {
                        ModelState.AddModelError("Images", "only image");
                        return View();
                    }
                    if (photo.CheckImageSize(500))
                    {
                        ModelState.AddModelError("Images", "Olcusu boyukdur");
                        return View();
                    }
                    BlogImage blogImage = new();
                    blogImage.ImageUrl = photo.SaveImage(_webHostEnvironment, "images/blog", photo.FileName);
                    blogImages.Add(blogImage);


                }
                blog.BlogImages = blogImages;
            }
            blog.Title= editVM.Title;
            blog.Content= editVM.Content;
            blog.AppUser = await _userManager.FindByNameAsync(editVM.Username);
            blog.UpdatedDate = DateTime.Now;

            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Blog blog = _appDbContext.Blogs.Where(b => !b.IsDeleted).SingleOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            blog.IsDeleted = true;
            blog.DeletedDate = DateTime.Now;
            return RedirectToAction("Index");
        }
    }
}
