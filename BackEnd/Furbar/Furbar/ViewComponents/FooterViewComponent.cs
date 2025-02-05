using Microsoft.AspNetCore.Mvc;

namespace Furbar.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            return View();
        }
    }
}
