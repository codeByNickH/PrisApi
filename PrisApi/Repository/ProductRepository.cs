using PrisApi.Repository.IRepository;
using PrisApi.Models.Scraping;
using PrisApi.Models;
using PrisApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PrisApi.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Product> GetOnFilterAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }
        public Task<Product> SaveAsync()
        {
            // Should it save all and be done and check if price i new add that to PriceHistory?
            // Or check for changes/new and save that?
            throw new NotImplementedException();
        }

    }
       
}