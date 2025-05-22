using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Services.Scrapers;

namespace PrisApi.Services
{
    public class ScraperService
    {
        private readonly ILogger<ScraperService> _logger;

        public ScraperService(ILogger<ScraperService> logger)
        {

            _logger = logger;
        }

        public async Task<ScrapingJob> ScrapeWillysAsync()
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScraper();
                var scrapedProducts = await scraper.ScrapeProductsAsync();

                

                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Willys scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Willys scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            // _dbContext.ScrapingJobs.Add(job);
            // await _dbContext.SaveChangesAsync();

            return job;
        }
    }
}