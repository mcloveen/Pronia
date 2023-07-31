using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;

namespace Pronia.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        using (ProniaDbContext context = new ProniaDbContext())

            return View (await  context.Sliders.ToListAsync());
       
    }

}

