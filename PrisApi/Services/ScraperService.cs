using PrisApi.Helper.IHelper;
using PrisApi.Mapper.IMapper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services.Scrapers;

namespace PrisApi.Services
{
    public class ScraperService
    {
        private readonly ILogger<ScraperService> _logger;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IProductRepository _repository;
        private readonly IMapping<Product> _mapping;
        public ScraperService(IScrapeHelper scrapeHelper, ILogger<ScraperService> logger, IScrapeConfigHelper configHelper, IProductRepository repository, IMapping<Product> mapping)
        {
            _scrapeHelper = scrapeHelper;
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
            _mapping = mapping;
        }

        public async Task<ScrapingJob> ScrapeWillysAsync(string navigation, Store storeConfig, int category)
        {
            var job = new ScrapingJob
            {
                StoreName = storeConfig.Name,
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting {store} scraping job at {time}", job.StoreName, job.StartedAt);
                
                var scraper = new WillysScrapeService(_scrapeHelper, _configHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(navigation, storeConfig);
                
                var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                job.NewProducts = savedProduct[0];
                job.UpdatedProducts = savedProduct[1];
                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("{store} scraping completed successfully at {completedAt}. Scraped {count} products.", job.StoreName, job.CompletedAt, job.ProductsScraped);
                await DelayBetweenRequests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Willys scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }
            return job;
        }
        public async Task<ScrapingJob> ScrapeIcaAsync(string navigation, Store storeConfig, int category)
        {
            var job = new ScrapingJob
            {
                StoreName = "Ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting {store} scraping job at {time}", job.StoreName, job.StartedAt);

                var scraper = new IcaScrapeService(_scrapeHelper, _configHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(navigation, storeConfig);

                var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                job.NewProducts = savedProduct[0];
                job.UpdatedProducts = savedProduct[1];
                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("{store} scraping completed successfully at {completedAt}. Scraped {count} products.", job.StoreName, job.CompletedAt, job.ProductsScraped);
                await DelayBetweenRequests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {store} scraping", job.StoreName);

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            return job;
        }
        public async Task<ScrapingJob> ScrapeCoopAsync(string navigation, Store storeConfig, int category)
        {
            var job = new ScrapingJob
            {
                StoreName = "Coop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Coop scraping job at {time}", job.StartedAt);

                var scraper = new CoopScraperService(_scrapeHelper, _configHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(navigation, storeConfig);

                var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                job.NewProducts = savedProduct[0];
                job.UpdatedProducts = savedProduct[1];
                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("Coop scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
                await DelayBetweenRequests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Coop scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            return job;
        }
        public async Task<ScrapingJob> ScrapeHemkopAsync(string navigation, Store storeConfig, int category)
        {
            var job = new ScrapingJob
            {
                StoreName = "Hemkop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Hemkop scraping job at {time}", job.StartedAt);

                var scraper = new HemkopScraperService(_scrapeHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(navigation, storeConfig);

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

            return job;
        }
        public async Task<ScrapingJob> ScrapeCityGrossAsync(string navigation, Store storeConfig, int category)
        {
            var job = new ScrapingJob
            {
                StoreName = "City Gross",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting City Gross scraping job at {time}", job.StartedAt);

                var scraper = new CitygrossScraperService(_scrapeHelper, _configHelper);
                var scrapedProducts = await scraper.ScrapeProductsAsync(navigation, storeConfig);

                var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                job.NewProducts = savedProduct[0];
                job.UpdatedProducts = savedProduct[1];
                job.ProductsScraped = scrapedProducts.Count;
                job.Success = true;
                job.CompletedAt = DateTime.UtcNow;

                _logger.LogInformation("CityGross scraping completed successfully at {completedAt}. Scraped {count} products.", job.CompletedAt, job.ProductsScraped);
                await DelayBetweenRequests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CityGross scraping");

                job.Success = false;
                job.ErrorMessage = ex.Message;
                job.CompletedAt = DateTime.UtcNow;
            }

            return job;
        }
        private async Task DelayBetweenRequests(int milliseconds = 10000)
        {
            await Task.Delay(milliseconds);
        }
        // public async Task<List<ScrapingJob>> ScrapeWillysAsync(ScraperConfig config, Store storeConfig)
        // {
        //     var job = new ScrapingJob
        //     {
        //         StoreName = config.StoreName,
        //         StartedAt = DateTime.UtcNow
        //     };
        //     var jobList = new List<ScrapingJob>();

        //     var navigation = new List<(string navigation, int category)>
        //     {
        //     (config.ScraperNavigation.NavMeat, 1),
        //     (config.ScraperNavigation.NavDairy, 2),
        //     (config.ScraperNavigation.NavFruitAndVegetables, 3),
        //     (config.ScraperNavigation.NavPantry, 4),
        //     (config.ScraperNavigation.NavFrozen, 5),
        //     (config.ScraperNavigation.NavBreadAndCookies, 6),
        //     (config.ScraperNavigation.NavFishAndSeafood, 7),
        //     (config.ScraperNavigation.NavVegetarian, 8),
        //     (config.ScraperNavigation.NavIceCreamCandyAndSnacks, 9),
        //     (config.ScraperNavigation.NavBeverage, 10),
        //     (config.ScraperNavigation.NavReadyMeals, 11),
        //     (config.ScraperNavigation.NavKids, 12),
        //     (config.ScraperNavigation.NavHomeAndCleaning, 13),
        //     (config.ScraperNavigation.NavHealth, 14),
        //     (config.ScraperNavigation.NavAnimals, 16),
        //     (config.ScraperNavigation.NavTobacco, 17)
        //     };
        //     if (!string.IsNullOrEmpty(config.ScraperNavigation.NavPharmacy))
        //     {
        //         navigation.Add((config.ScraperNavigation.NavPharmacy, 15));
        //     }
        //     if (!string.IsNullOrEmpty(config.ScraperNavigation.NavChark))
        //     {
        //         navigation.Add((config.ScraperNavigation.NavChark, 1));
        //     }
        //     if (!string.IsNullOrEmpty(config.ScraperNavigation.NavCheese))
        //     {
        //         navigation.Add((config.ScraperNavigation.NavCheese, 2));
        //     }
        //     try                                                     
        //     {
        //         _logger.LogInformation("Starting {store} scraping job at {time}", job.StoreName, job.StartedAt);

        //         var scraper = new WillysScrapeService(_scrapeHelper, _configHelper);
        //         foreach (var nav in navigation) //  Loop triggers to many request so gets blocked, adding Delay to make it work.
        //         {
        //             var scrapedProducts = await scraper.ScrapeProductsAsync(nav.navigation, storeConfig);

        //             var mappedProducts = await _mapping.ToProduct(scrapedProducts);
        //             var savedProduct = await _repository.SaveAsync(mappedProducts, nav.category);

        //             job.NewProducts = savedProduct[0];
        //             job.UpdatedProducts = savedProduct[1];
        //             job.ProductsScraped = scrapedProducts.Count;
        //             job.Success = true;
        //             job.CompletedAt = DateTime.UtcNow;
        //             jobList.Add(job);
        //             await DelayBetweenRequests();
        //         }


        //         _logger.LogInformation("{store} scraping completed successfully at {completedAt}. Scraped {count} products.", job.StoreName, job.CompletedAt, job.ProductsScraped);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error during {store} scraping", job.StoreName);

        //         job.Success = false;
        //         job.ErrorMessage = ex.Message;
        //         job.CompletedAt = DateTime.UtcNow;
        //         jobList.Add(job);
        //     }
        //     return jobList;
        // }
        public async Task<ScrapingJob> ScrapeWillysOffersAsync(int location)
        {
            var job = new ScrapingJob
            {
                StoreName = "willys",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Willys scraping job at {time}", job.StartedAt);

                var scraper = new WillysScrapeService(_scrapeHelper, _configHelper);
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

            return job;
        }
        public async Task<ScrapingJob> ScrapeIcaOffersAsync()
        {
            var job = new ScrapingJob
            {
                StoreName = "ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Ica scraping job at {time}", job.StartedAt);

                var scraper = new IcaScrapeService(_scrapeHelper, _configHelper);
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

            return job;
        }
    }
}