using Microsoft.AspNetCore.Mvc;

namespace Pronia.Areas.Manage.Controllers
{
    public class ProductController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}

