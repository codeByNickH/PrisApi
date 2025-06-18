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

        public async Task<ScrapingJob> ScrapeWillysOffersAsync(string store)
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService();
                var scrapedProducts = await scraper.ScrapeDiscountProductsAsync(store);

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

                var scraper = new IcaScrapeService();
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

        public async Task<ScrapingJob> ScrapeWillysAsync(string category)
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService();
                var scrapedProducts = await scraper.ScrapeProductsAsync(category);



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

        public async Task<ScrapingJob> ScrapeWillysMeatAsync()
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService();
                var scrapedProducts = await scraper.ScrapeMeatProductsAsync();



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
        public async Task<ScrapingJob> ScrapeWillysDariyAsync()
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService();
                var scrapedProducts = await scraper.ScrapeDariyProductsAsync();



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
        public async Task<ScrapingJob> ScrapeWillysFruitAsync()
        {
            var job = new ScrapingJob
            {
                StoreId = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService();
                var scrapedProducts = await scraper.ScrapeFruitProductsAsync();



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
        public async Task<ScrapingJob> ScrapeIcaAsync(string category)
        {
            var job = new ScrapingJob
            {
                StoreId = "ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Ica scraping job at {time}", job.StartedAt);

                var scraper = new IcaScrapeService();
                var scrapedProducts = await scraper.ScrapeProductsAsync(category);



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
        public async Task<ScrapingJob> ScrapeCoopAsync(string category)
        {
            var job = new ScrapingJob
            {
                StoreId = "coop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Coop scraping job at {time}", job.StartedAt);

                var scraper = new CoopScraperService();
                var scrapedProducts = await scraper.ScrapeProductsAsync(category);



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
    }
}