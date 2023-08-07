 using System;
using P137Pronia.Models;
using P137Pronia.ViewModels.ProductVMs;
using Pronia.ViewModels.ProductVMs;

namespace P137Pronia.Services.Interfaces
{
	public interface IProductService
	{
		IQueryable<Product> GetTable { get; }
		public Task<List<Product>> GetAll(bool takeAll);
		public Task<Product> GetById(int? id,bool takeAll=false);
		public Task Create(CreateProductVM productVm);
		public Task Update(int? id,UpdateProductVM productVm);
		public Task Delete(int? id);
		public Task SoftDelete(int? id);
		public Task DeleteImage(int? id);
        Task Create(CreateProductVM productVM);
    }
}


