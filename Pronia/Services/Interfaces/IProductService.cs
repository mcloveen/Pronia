using Pronia.Models;
using Pronia.ViewModels.ProductVMs;

namespace Pronia.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll(bool takeAll);
        public Task<Product> GetById(int? id);
        Task Create(CreateProductVM productVM);
       
        Task Delete(int? id);
        Task SoftDelete(int? id);
        IQueryable<Product> GetTable { get; }
    }
}

