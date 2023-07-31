using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess.Pronia.DataAccess;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.ProductVMs;

namespace Pronia.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly ProniaDbContext _context;
        readonly IFileService _fileService;

        IQueryable<Product> IProductService.GetTable { get => _context.Set<Product>(); }

        public ProductService(ProniaDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }



        public async Task Create(CreateProductVM productVM)
        {
            Product entity = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Discount = productVM.Discount,
                Price = productVM.Price,
                Rating = productVM.Rating,
                StockCount = productVM.StockCount,
                MainImage = await _fileService.UploadAsync(productVM.MainImageFile, Path.Combine(
                    "assets", "imgs", "products")),
            };
            if (productVM.ImageFiles != null)
            {
                List<ProductImage> imgs = new();
                foreach (var item in productVM.ImageFiles)
                {
                    string fileName = await _fileService.UploadAsync(item, Path.Combine(
                    "assets", "imgs", "products"));
                    imgs.Add(new ProductImage
                    {
                        Name = fileName
                    });
                }
                entity.ProductImages = imgs;
            }
            if (productVM.HoverImageFile != null)
                entity.HoverImage = await _fileService.UploadAsync(productVM.HoverImageFile, Path.Combine(
                    "assets", "imgs", "products"));
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }



        public async Task Delete(int? id)
        {
            var entity = await GetById(id);
            _context.Remove(entity);
            _fileService.Delete(entity.MainImage);
            if (entity.HoverImage != null)
            {
                _fileService.Delete(entity.HoverImage);
            }
            await _context.SaveChangesAsync();
        }




        public async Task<List<Product>> GetAll(bool takeAll)
        {
            if (takeAll)
            {
                return await _context.Products.ToListAsync();
            }
            return await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
        }





        public async Task<Product> GetById(int? id)
        {
            if (id == null || id < 1) throw new ArgumentException();
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) throw new ArgumentNullException();
            return entity;
        }

  

        public async Task SoftDelete(int? id)
        {
            var entity = await GetById(id);
            entity.IsDeleted = !entity.IsDeleted;
            await _context.SaveChangesAsync();
        }
    }
}

