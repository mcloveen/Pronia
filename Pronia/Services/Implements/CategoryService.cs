using Microsoft.EntityFrameworkCore;
using P137Pronia.Services.Interfaces;
using Pronia.DataAccess;
using Pronia.Models;

namespace Pronia.Services.Implements
{
    public class CategoryService : ICategoryService 
    {
        readonly ProniaDBContext _context;
        public CategoryService(ProniaDBContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetTable => _context.Set<Category>();

        IQueryable<Category> ICategoryService.GetTable => throw new NotImplementedException();

        public async Task Create(string name)
        {
            if (name == null) throw new ArgumentNullException();
            if(await _context.Categories.AnyAsync(c=>c.Name==name))
            {
                throw new Exception();
            }
            await _context.Categories.AddAsync(new Category() { Name = name });
            await _context.SaveChangesAsync();
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<Category> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isAllExist(List<int> ids)
        {
            foreach (var id in ids)
            {
                if(!await isExist(id))
                     return false;
            }
            return true;
        }

        public Task<bool> isExist(int id)
            => _context.Categories.AnyAsync(c=>c.Id==id);

        public Task Update(int? id, string name)
        {
            throw new NotImplementedException();
        }

        Task<ICollection<Category>> ICategoryService.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<Category> ICategoryService.GetById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}

