
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrisApi.Data;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Repository
{
    public class LocationRepository : IRepository<StoreLocation>
    {
        private readonly AppDbContext _dbContext;
        public LocationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<StoreLocation> GetOnFilterAsync(Expression<Func<StoreLocation, bool>> filter = null, bool tracked = true)
        {
            IQueryable<StoreLocation> location = _dbContext.StoreLocations.Include(s => s.Stores);
            if (!tracked == true)
            {
                location = location.AsNoTracking();
            }
            if (filter != null)
            {
                location = location.Where(filter); 
            }
            return await location.Select(l => new StoreLocation
            {
                Id = l.Id,
                Address = l.Address,
                City = l.City,
                District = l.District,
                PostalCode = l.PostalCode,
                Stores = l.Stores
            }).FirstOrDefaultAsync();
        }
        public async Task<List<StoreLocation>> GetListOnFilterAsync(Expression<Func<StoreLocation, bool>> filter = null, bool tracked = true) // For other API
        {
            IQueryable<StoreLocation> location = _dbContext.StoreLocations.Include(s => s.Stores);
            if (!tracked == true)
            {
                location = location.AsNoTracking();
            }
            if (filter != null)
            {
                location = location.Where(filter);
            }
            return await location.Select(l => new StoreLocation
            {
                Id = l.Id,
                Address = l.Address,
                City = l.City,
                District = l.District,
                PostalCode = l.PostalCode,
                Stores = l.Stores.Select(s => new Store
                {
                    Id = s.Id,
                    Name = s.Name,
                    // Products = s.Products.Select(p => new Product
                    // {
                    //     Name = p.Name,
                    //     // Add rest
                    // }).ToList()
                }).ToList()
            }).ToListAsync();
        }

        public Task<StoreLocation> SaveAsync(StoreLocation storeLocation)
        {
            throw new NotImplementedException();
        }
    }
}