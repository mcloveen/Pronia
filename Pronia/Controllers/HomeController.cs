﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P137Pronia.Services.Interfaces;
using Pronia.DataAccess;
using Pronia.ViewModels.BasketVMs;
using Pronia.ViewModels.HomeVMs;

namespace Pronia.Controllers;

public class HomeController : Controller
{
    private readonly ProniaDBContext _context;
    private readonly ISliderService _sliderService;
    private readonly IProductService _productService;
    public HomeController(IProductService productService, ISliderService sliderService)
    {
        _productService = productService;
        _sliderService = sliderService;
    }
    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = (ICollection<Models.Slider>)await _sliderService.GetAll(),
            Products = (ICollection<Models.Product>)await _productService.GetTable.Take(4).ToListAsync()
        };
        ViewBag.ProductCount =await _productService.GetTable.CountAsync();
         return View(vm);

    }
    public async Task<IActionResult> LoadMore(int skip,int take)
    {
        return PartialView("_ProductPartial",await _productService.GetTable.Skip(skip).Take(take).ToListAsync());
    }

    public async Task<IActionResult> AddBasket(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        //var model = await _productService.GetById(id);
        if (!await _productService.GetTable.AnyAsync(p=>p.Id==id)) return NotFound();
        var basket = HttpContext.Request.Cookies["basket"];
        List<BasketItemVM> items = basket == null
            ? new List<BasketItemVM>()
            : JsonConvert.DeserializeObject<List<BasketItemVM>>(basket);

        var item = items.SingleOrDefault(i => i.Id == id);

        if(item==null)
        {
            item = new BasketItemVM
            {
                Id = (int)id,
                Count = 1
            };
            items.Add(item);
        }
        else
        {
            item.Count++;
        }
        HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
        return Ok();
    }

    public async Task<IActionResult> GetBasket()
    {
        var basket = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
        List<BasketItemProductVM> vm = new List<BasketItemProductVM>();
        foreach (var item in basket)
        {
            vm.Add(new BasketItemProductVM
            {
                Count=item.Count,
                Product=await _productService.GetById(item.Id)
            });
        }
        return PartialView("_BasketPartial",vm);
    }
}

