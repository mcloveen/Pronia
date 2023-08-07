using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Models;
using Pronia.ViewModels.ProductVMs;
using P137Pronia.Services.Interfaces;

namespace Pronia.Services.Implements
{
    public class ProductService : IProductService 
    {
        private readonly ProniaDBContext _context;
        readonly IFileService _fileService;
        readonly ICategoryService _catService;

        public IQueryable<Product> GetTable { get => _context.Set<Product>(); }

        IQueryable<Product> IProductService.GetTable => throw new NotImplementedException();

        public ProductService(ProniaDBContext context, IFileService fileService, ICategoryService catService)
        {
            _context = context;
            _fileService = fileService;
            _catService = catService;
        }

        public async Task Create(CreateProductVM productVm)
        {
            if(productVm.CategoryIds.Count>4)
                throw new Exception();
            if(!await _catService.isAllExist(productVm.CategoryIds))
                throw new ArgumentException();
            List<ProductCategory> prodCategories = new List<ProductCategory>();
            foreach (var id in productVm.CategoryIds)
            {
                prodCategories.Add(new ProductCategory
                {
                    CategoryId = id
                });
            }
            
            Product entity = new Product()
            {
                Name = productVm.Name,
                Description = productVm.Description,
                Discount = productVm.Discount,
                Price = productVm.Price,
                Raiting = productVm.Raiting,
                StockCount = productVm.StockCount,
                MainImage = await _fileService.UploadAsync(productVm.MainImageFile, Path.Combine
                ("assets", "imgs", "products")),
                ProductCategories=prodCategories
            };
            if(productVm.ImageFiles != null)
            {
                List<ProductImage> imgs = new();
                foreach (var file in productVm.ImageFiles)
                {
                    string fileName = await _fileService.UploadAsync(file, Path.Combine("assets", "imgs", "products"));
                    imgs.Add(new ProductImage
                    {
                        Name = fileName
                    });
                }
                entity.ProductImages = imgs;
            }
            if(productVm.HoverImageFile !=null)
            {
                entity.HoverImage = await _fileService.UploadAsync(productVm.HoverImageFile, Path.Combine
                ("assets", "imgs", "products"));
            }
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var entity = await GetById(id,true);
            _context.Remove(entity);
            _fileService.Delete(entity.MainImage);
            if(entity.HoverImage != null)
            {
                _fileService.Delete(entity.HoverImage);
            }
            if(entity.ProductImages != null)
            {
            foreach (var item in entity.ProductImages)
            {
                _fileService.Delete(item.Name);
            }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll(bool takeAll)
        {
            if(takeAll)
            {
                return await _context.Products.ToListAsync();
            }
            return await _context.Products.Where(p=>p.IsDeleted==false).ToListAsync();
        }

        public async Task<Product> GetById(int? id,bool takeAll=false)
        {
            if (id < 1 || id == null) throw new ArgumentException();
            Product? entity;
            if (takeAll)
            {
                entity=await _context.Products.FindAsync(id);
            }
            else
            {
                entity=await _context.Products.SingleOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
            }
            if (entity == null) throw new NullReferenceException();
            return entity;
        }

        public async Task SoftDelete(int? id)
        { 
            var entity = await GetById(id);
            entity.IsDeleted = !entity.IsDeleted;
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? id,UpdateProductVM productVm)
        {
            if (productVm.CategoryIds.Count > 4)
                throw new Exception();
            if (!await _catService.isAllExist(productVm.CategoryIds))
                throw new ArgumentException();
            List<ProductCategory> prodCategories = new List<ProductCategory>();
            foreach (var catId in productVm.CategoryIds)
            {
                prodCategories.Add(new ProductCategory
                {
                    CategoryId = catId
                });
            }

            var entity = await _context.Products.Include(p=>p.ProductCategories).SingleOrDefaultAsync(p=>p.Id==id);
            if (entity.ProductCategories != null) 
            {
                entity.ProductCategories.Clear();
            }
            entity.Name = productVm.Name;
            entity.Description = productVm.Description;
            entity.Price = productVm.Price;
            entity.Discount = productVm.Discount;
            entity.StockCount = productVm.StockCount;
            entity.Raiting = productVm.Raiting;
            entity.ProductCategories = prodCategories;
            if(productVm.MainImage != null)
            {
                _fileService.Delete(entity.MainImage);
                entity.MainImage = await _fileService.UploadAsync(productVm.MainImage, Path.Combine("assets", "imgs", "products"));
            }
            if(productVm.HoverImage != null)
            {
                if (entity.HoverImage != null)
                {
                    _fileService.Delete(entity.HoverImage);
                }
                entity.HoverImage = await _fileService.UploadAsync(productVm.HoverImage, Path.Combine("assets", "imgs", "products"));
            }
            if(productVm.ProductImages != null)
            {
                if (entity.ProductImages == null) entity.ProductImages = new List<ProductImage>();
                foreach (var img in productVm.ProductImages)
                {
                    ProductImage prodImg = new ProductImage
                    {
                        Name= await _fileService.UploadAsync(img, Path.Combine("assets", "imgs", "products"))
                    };
                    entity.ProductImages.Add(prodImg);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(int? id)
        {
            if (id == null || id <= 0) throw new ArgumentException();
            var entity=await _context.ProductImages.FindAsync(id);
            if (entity == null) throw new NullReferenceException();
            _fileService.Delete(entity.Name);
            _context.ProductImages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        Task<List<Product>> IProductService.GetAll(bool takeAll)
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductService.GetById(int? id, bool takeAll)
        {
            throw new NotImplementedException();
        }
    }
}

