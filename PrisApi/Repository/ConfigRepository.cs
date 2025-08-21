using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrisApi.Data;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Repository
{
    public class ConfigRepository : IRepository<ScraperConfig>
    {
        private readonly AppDbContext _dbContext;
        public ConfigRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ScraperConfig> GetOnFilterAsync(Expression<Func<ScraperConfig, bool>> filter = null, bool tracked = true)
        {
            IQueryable<ScraperConfig> config = _dbContext.ScraperConfigs.Include(n=>n.ScraperNavigation).Include(s=>s.ScraperSelector);
            if (!tracked == true)
            {
                config = config.AsNoTracking();
            }
            if (filter != null)
            {
                config = config.Where(filter);
            }
            return await config.Select(c => new ScraperConfig
            {
                Id = c.Id,
                StoreName = c.StoreName,
                BaseUrl = c.BaseUrl,
                RequestDelayMs = c.RequestDelayMs,
                UseJavaScript = c.UseJavaScript,
                ScraperNavigation = c.ScraperNavigation,
                ScraperSelector = c.ScraperSelector
            }).FirstOrDefaultAsync();
        }
        public Task<ScraperConfig> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}