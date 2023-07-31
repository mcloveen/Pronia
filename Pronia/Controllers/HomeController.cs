using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.IndexVMs;

namespace Pronia.Controllers;


public class HomeController : Controller
{
    private readonly ISliderService _sliderService;

    private readonly IProductService _productService;
    public HomeController(ISliderService sliderService, IProductService productService)
    {
        _sliderService = sliderService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVm vm = new()
        {
            Sliders = await _sliderService.GetAll(),
            Products = await _productService.GetAll(false)
        };
        return View(vm);
    }
}

