using Microsoft.AspNetCore.Mvc;

namespace Furbar.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
