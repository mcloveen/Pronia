using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            using ProniaDbContext context = new ProniaDbContext();
            return View(await context.Sliders.ToListAsync());
        }
    }
}

