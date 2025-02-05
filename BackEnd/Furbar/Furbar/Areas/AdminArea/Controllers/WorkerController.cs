using Furbar.DAL;
using Furbar.Extensions;
using Furbar.Models;
using Furbar.Models.Accounts;
using Furbar.Services.Subscribtion;
using Furbar.ViewModels.WorkerAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furbar.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class WorkerController : Controller
    {
        private readonly AppDbContext _appDbContext;

       
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WorkerController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
          
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Index()
        {
            WorkerVM workerVM = new();
            workerVM.Workers=_appDbContext.Workers.AsNoTracking().Where(w=>!w.IsDeleted).ToList();
            return View(workerVM);
        }

        public ActionResult Details(int id)
        {
            if (id == null) return NotFound();
            Worker worker=_appDbContext.Workers.Where(w=>!w.IsDeleted).FirstOrDefault(w=> w.Id == id);
            if (worker == null) return NotFound();
            WorkerDetailVM workerDetailVM = new();
            workerDetailVM.Fullname = worker.FullName;
            workerDetailVM.Position= worker.Position;
            workerDetailVM.ImageUrl= worker.ImageUrl;
            return View(workerDetailVM);
        }

   
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkerCreateVM createVM)
        {
            if (!ModelState.IsValid) return View();
            Worker worker = new();
            worker.FullName= createVM.FullName;
            worker.Position= createVM.Position;
            if (!createVM.Image.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            worker.CreatedDate= DateTime.Now;
            worker.IsDeleted = false;
            worker.ImageUrl = createVM.Image.SaveImage(_webHostEnvironment, "images/team", createVM.Image.FileName);
            _appDbContext.Workers.Add(worker);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Worker worker= _appDbContext.Workers.Where(w=>!w.IsDeleted).SingleOrDefault(w => w.Id == id);
            if(worker == null) return NotFound();
            return View(new WorkerEditVM {FullName=worker.FullName,Position=worker.Position });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, WorkerEditVM editVM)
        {
            if (id == null) return NotFound();
            Worker worker = _appDbContext.Workers.Where(w => !w.IsDeleted).SingleOrDefault(w => w.Id == id);
            if (worker == null) return NotFound();
            if (!ModelState.IsValid) return View();
            worker.Position= editVM.Position??worker.Position;
            worker.FullName=editVM.FullName??worker.FullName;
            if (editVM.Image != null) 
            {
                if (!editVM.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "only image");
                    return View();
                }

                worker.ImageUrl = editVM.Image.SaveImage(_webHostEnvironment, "images/team", editVM.Image.FileName);
            }
            worker.UpdatedDate= DateTime.Now;
            _appDbContext.SaveChanges();
           return RedirectToAction("Index");    
           
        }
        public ActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Worker worker = _appDbContext.Workers.Where(w => !w.IsDeleted).SingleOrDefault(w => w.Id == id);
            if (worker == null) return NotFound();
            worker.IsDeleted = true;
            worker.DeletedDate = DateTime.Now;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

  
    }
}
