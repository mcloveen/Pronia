using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Extensions;
using Pronia.ViewModels.ProductVMs;
using P137Pronia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        readonly IProductService _service;
        readonly ICategoryService _catService;

        public ProductController(IProductService service, ICategoryService catService)
        {
            _service = service;
            _catService = catService;
        }

        public async  Task<IActionResult> Index()
        {
            return View(await _service.GetTable.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_catService.GetTable,"Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (productVM.MainImageFile != null)
            {
                    if (!productVM.MainImageFile.IsTypeValid("image"))
                    {
                    ModelState.AddModelError("MainImageFile", "Wrong file type");
                    }
                if(!productVM.MainImageFile.IsSizeValid(2))
                   {
                    ModelState.AddModelError("MainImageFile", "File max size is 2mb");
                   }
            }

            if(productVM.HoverImageFile != null)
            {
                if (!productVM.HoverImageFile.IsTypeValid("image"))
                {
                    ModelState.AddModelError("HoverImageFile", "Wrong file type");
                }
                if (!productVM.HoverImageFile.IsSizeValid(2))
                {
                    ModelState.AddModelError("HoverImageFile", "File max size is 2mb");
                }
            }

            if(productVM.ImageFiles !=null)
            {
                foreach (var img in productVM.ImageFiles)
                {
                    if (!img.IsTypeValid("image"))
                    {
                        ModelState.AddModelError("ImageFiles", "Wrong file type " + img.FileName);
                    }
                    if (!img.IsSizeValid(2))
                    {
                        ModelState.AddModelError("ImageFiles", "File max size is 2mb " + img.FileName);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_catService.GetTable, "Id", "Name");
                return View();
            }
            await _service.Create(productVM);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            await _service.SoftDelete(id);
            TempData["IsDeleted"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity=await _service.GetTable.Include(p => p.ProductImages).Include(p=>p.ProductCategories).SingleOrDefaultAsync(p=>p.Id==id);
            if (entity == null) return BadRequest();
            ViewBag.Categories = new SelectList(_catService.GetTable, "Id", "Name");
            UpdateProductGetVM vm = new UpdateProductGetVM
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Discount = entity.Discount,
                Raiting = entity.Raiting,
                StockCount = entity.StockCount,
                MainImage = entity.MainImage,
                HoverImage = entity.HoverImage,
                ProductImages = entity.ProductImages,
                ProductCategoryIds = entity.ProductCategories.Select(p => p.CategoryId).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductGetVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity = await _service.GetById(id);
            if (entity == null) return BadRequest();
            UpdateProductVM updateVm = new UpdateProductVM
            {
                Name=vm.Name,
                Description=vm.Description,
                Price=vm.Price,
                Discount=vm.Discount,
                Raiting=vm.Raiting,
                StockCount=vm.StockCount,
                HoverImage=vm.HoverImageFile,
                MainImage=vm.MainImageFile,
                ProductImages=vm.ProductImagesFiles,
                CategoryIds=vm.ProductCategoryIds
            };
            await _service.Update(id,updateVm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteImage(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            await _service.DeleteImage(id);
            return Ok();
        }
    }
}

