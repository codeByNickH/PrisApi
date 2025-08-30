using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrisApi.Data;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Repository
{
    public class StoreRepository : IRepository<Store>
    {
        private readonly AppDbContext _dbContext;
        public StoreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Store>> GetListOnFilterAsync(Expression<Func<Store, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Store> store = _dbContext.Stores.Include(l => l.StoreLocation);
            if (!tracked == true)
            {
                store = store.AsNoTracking();
            }
            if (filter != null)
            {
                store = store.Where(filter);
            }
            return await store.Select(s => new Store
            {
                Id = s.Id,
                Name = s.Name,
                StoreLocation = s.StoreLocation
            }).ToListAsync();
        }
        public async Task<Store> GetOnFilterAsync(Expression<Func<Store, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Store> store = _dbContext.Stores.Include(l => l.StoreLocation);
            if (!tracked == true)
            {
                store = store.AsNoTracking();
            }
            if (filter != null)
            {
                store = store.Where(filter);
            }
            return await store.Select(s => new Store
            {
                Id = s.Id,
                Name = s.Name,
                StoreLocation = s.StoreLocation
            }).FirstOrDefaultAsync();
        }
        public Task<Store> SaveAsync(Store store)
        {
            throw new NotImplementedException();
        }
    }
}