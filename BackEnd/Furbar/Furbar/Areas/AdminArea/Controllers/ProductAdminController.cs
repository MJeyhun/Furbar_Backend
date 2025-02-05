using Furbar.DAL;
using Furbar.Extensions;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.Services.Subscribtion;
using Furbar.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Data;
using System.Reflection.Metadata;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class ProductAdminController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductAdminController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;

            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            ProductVM productVM = new();
            productVM.Products = _appDbContext.Products.AsNoTracking()
                .Include(p => p.ProductImages)
                .Where(p=>!p.IsDeleted)
                .ToList();
            return View(productVM);
        }

        public ActionResult Details(int id)
        {
            if (id == null) return NotFound();
            Product product= _appDbContext.Products
                .Include(p=>p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .Include(p => p.ProductCategories)
                .ThenInclude(p=>p.Category)
                .Include(p=>p.ProductImages)
                .Where(p=>!p.IsDeleted).SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            ProductDetailVM detailVM = new();
            detailVM.Name = product.Name;
            detailVM.Description=product.Description;
            detailVM.Price= product.Price;
            detailVM.Size = product.Size;
            detailVM.FrameSize= product.FrameSize;
            detailVM.Material= product.Material;    
            detailVM.Count= product.Count;
            detailVM.ProductImages= product.ProductImages;
            detailVM.ProductColors=product.ProductColors;
            detailVM.ProductCategories= product.ProductCategories;
            detailVM.Information= product.Information;
            return View(detailVM);
        }

 
        public ActionResult Create()
        {
            ViewBag.Colors = new SelectList(_appDbContext.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_appDbContext.Categories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateVM createVM)
        {
            ViewBag.Colors = new SelectList(_appDbContext.Colors.Where(c=>!c.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_appDbContext.Categories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            Product product = new();

            product.Name = createVM.Name;
            product.Description = createVM.Description;
            product.Price = createVM.Price;
            product.Size = createVM.Size;
            product.FrameSize = createVM.FrameSize;
            product.Material= createVM.Material;
            product.Count= createVM.Count;
            product.Information= createVM.Information;

            List<ProductImage>? productImages = new();

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
                ProductImage productImage = new();
                productImage.ImageUrl = photo.SaveImage(_webHostEnvironment, "images/product", photo.FileName);
                productImages.Add(productImage);
            }

            List<ProductColor> productColors = new();
            foreach (var item in createVM.ColorIds)
            {
                ProductColor colorProduct = new();
                colorProduct.ProductId = product.Id;
                colorProduct.ColorId = item;
                _appDbContext.ProductColors.Add(colorProduct);
                productColors.Add(colorProduct);

            }
            List<ProductCategory> productCategories = new();
            foreach (var item in createVM.CategoryIds)
            {
                ProductCategory productCategory = new();
                productCategory.ProductId = product.Id;
                productCategory.CategoryId = item;
                _appDbContext.ProductCategories.Add(productCategory);
                productCategories.Add(productCategory);

            }

            product.ProductImages = productImages;
            product.ProductColors = productColors;
            product.ProductCategories = productCategories;
            product.CreatedDate= DateTime.Now;
            product.IsDeleted= false;
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            List<AppUser> users = _userManager.Users.ToList();
            MessageToSubscribe message = new();
            message.SendMessageSubscribed(users, "Product");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Colors = new SelectList(_appDbContext.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_appDbContext.Categories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            List<Color> colors = new();
            List<Category> categories = new();
            if (id == null) return NotFound();
            Product product = _appDbContext.Products
                .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .Include(P => P.ProductImages)
                .Where(p => !p.IsDeleted).SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            foreach (var color in product.ProductColors)
            {
                Color existColor =  _appDbContext.Colors.Where(c => !c.IsDeleted).FirstOrDefault(c=>c.Id==color.ColorId);
                colors.Add(existColor);
            }
            ViewBag.ExistColors = new SelectList(colors, "Id", "Name");

            foreach (var category in product.ProductCategories)
            {
                Category existCategory = _appDbContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == category.CategoryId);
                categories.Add(existCategory);
            }
            ViewBag.ExistCategories = new SelectList(colors, "Id", "Name");

            return View( new ProductEditVM {Name= product.Name, 
                Description= product.Description,
                Price=product.Price
                ,Size=product.Size,
                FrameSize=product.FrameSize,
                Material=product.Material,
                Count=product.Count,
                Information=product.Information,
                ProductColors=product.ProductColors,
                ProductCategories=product.ProductCategories
    
        });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM editVM)
        {
            ViewBag.Colors = new SelectList(_appDbContext.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_appDbContext.Categories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            List<Color> colors = new();
            List<Category> categories = new();
            if (id == null) return NotFound();
            Product product = _appDbContext.Products
                .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .Include(P => P.ProductImages)
                .Where(p => !p.IsDeleted).SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();




            foreach (var color in product.ProductColors)
            {
                Color existColor = _appDbContext.Colors.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == color.ColorId);
                colors.Add(existColor);
            }
            ViewBag.ExistColors = new SelectList(colors, "Id", "Name");

            foreach (var category in product.ProductCategories)
            {
                Category existCategory = _appDbContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == category.CategoryId);
                categories.Add(existCategory);
            }
            ViewBag.ExistCategories = new SelectList(colors, "Id", "Name");


            if (editVM.ColorIds != null)
            {
                foreach (var item in editVM.ColorIds)
                {
                    ProductColor productColor = new();
                    productColor.ProductId = product.Id;
                    productColor.ColorId = item;
                    bool existColor = product.ProductColors.Any(x => x.Color.Id == item);
                    if (!existColor)
                    {
                        _appDbContext.ProductColors.Add(productColor);
                        product.ProductColors.Add(productColor);
                    }
                    else
                    {
                        ModelState.AddModelError("ColorIds", "This color exists!");
                        return View(editVM);
                    }


                }
            }

            if (editVM.ExistColorIds != null)
            {
                foreach (var item in editVM.ExistColorIds)
                {
                    ProductColor productColor = product.ProductColors.FirstOrDefault(th => th.ColorId == item);
                    product.ProductColors.Remove(productColor);
                }
            }
            if (editVM.CategoryIds != null)
            {
                foreach (var item in editVM.CategoryIds)
                {
                    ProductCategory productCategory = new();
                    productCategory.ProductId = product.Id;
                    productCategory.CategoryId = item;
                    bool existCategory = product.ProductCategories.Any(x => x.Category.Id == item);
                    if (!existCategory)
                    {
                        _appDbContext.ProductCategories.Add(productCategory);
                        product.ProductCategories.Add(productCategory);
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryIds", "This category exists!");
                        return View(editVM);
                    }


                }
            }

            if (editVM.ExistColorIds != null)
            {
                foreach (var item in editVM.ExistColorIds)
                {
                    ProductColor productColor = product.ProductColors.FirstOrDefault(th => th.ColorId == item);
                    product.ProductColors.Remove(productColor);
                }
            }
            List<ProductImage> productImages = new();
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
                    ProductImage productImage = new();
                    productImage.ImageUrl = photo.SaveImage(_webHostEnvironment, "images/product", photo.FileName);
                    productImages.Add(productImage);


                }
                product.ProductImages = productImages;
            }
            product.Name = editVM.Name;
            product.Description = editVM.Description;
            product.Price = editVM.Price;
            product.Size = editVM.Size;
            product.FrameSize = editVM.FrameSize;
            product.Material = editVM.Material;
            product.Count = editVM.Count;
            product.Information = editVM.Information;
            product.UpdatedDate= DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Product product = _appDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            product.IsDeleted= true;
            product.DeletedDate = DateTime.Now;
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

 
    }
}
