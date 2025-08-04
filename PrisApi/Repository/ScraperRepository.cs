using PrisApi.Repository.IRepository;
using PrisApi.Models.Scraping;
using PrisApi.Models;
using PrisApi.Data;

namespace PrisApi.Repository
{
    public class ScraperRepository : IRepository<Product>
    {
        // private readonly DbContext _dbContext;
        public ScraperRepository(/*DbContext dbContext*/)
        {

        }

        public async Task<Product> SaveProductAsync()
        {
            // Should it save all and be done and check if price i new add that to PriceHistory?
            // Or check for changes/new and save that?

            return new Product();
        }
    }
       
}