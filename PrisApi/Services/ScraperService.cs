using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Services.Scrapers;

namespace PrisApi.Services
{
    public class ScraperService
    {
        private readonly ILogger<ScraperService> _logger;
        private readonly IScrapeHelper _scrapeHelper;
        public ScraperService(IScrapeHelper scrapeHelper, ILogger<ScraperService> logger)
        {
            _scrapeHelper = scrapeHelper;
            _logger = logger;
        }

        public async Task<ScrapingJob> ScrapeWillysOffersAsync(int location)
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeDiscountProductsAsync(location);

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

        public async Task<ScrapingJob> ScrapeIcaOffersAsync()
        {
            var job = new ScrapingJob
            {
                StoreId = "ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Ica scraping job at {time}", job.StartedAt);
                
                var scraper = new IcaScrapeService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeDiscountProductsAsync();



                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Ica scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Ica scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            // _dbContext.ScrapingJobs.Add(job);
            // await _dbContext.SaveChangesAsync();

            return job;
        }
        public async Task<ScrapingJob> ScrapeWillysAsync(string category, int location)
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(category, location);



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
        public async Task<ScrapingJob> ScrapeIcaAsync(string category, int location)
        {
            var job = new ScrapingJob
            {
                StoreId = "ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Ica scraping job at {time}", job.StartedAt);

                var scraper = new IcaScrapeService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(category, location);



                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Ica scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Ica scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            // _dbContext.ScrapingJobs.Add(job);
            // await _dbContext.SaveChangesAsync();

            return job;
        }
        public async Task<ScrapingJob> ScrapeCoopAsync(string category, int location)
        {
            var job = new ScrapingJob
            {
                StoreId = "coop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Coop scraping job at {time}", job.StartedAt);

                var scraper = new CoopScraperService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(category, location);



                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Coop scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Coop scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            // _dbContext.ScrapingJobs.Add(job);
            // await _dbContext.SaveChangesAsync();

            return job;
        }
        public async Task<ScrapingJob> ScrapeHemkopAsync(string category)
        {
            var job = new ScrapingJob
            {
                StoreId = "hemkop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Hemkop scraping job at {time}", job.StartedAt);

                var scraper = new HemkopScraperService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(category);



                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Hemkop scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Hemkop scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            // _dbContext.ScrapingJobs.Add(job);
            // await _dbContext.SaveChangesAsync();

            return job;
        }
        public async Task<ScrapingJob> ScrapeCityGrossAsync(string category, int location)
        {
            var job = new ScrapingJob
            {
                StoreId = "citygross",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting CityGross scraping job at {time}", job.StartedAt);

                var scraper = new CitygrossScraperService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(category, location);



                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("CityGross scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CityGross scraping");

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