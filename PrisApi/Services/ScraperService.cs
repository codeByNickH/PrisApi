using System.Transactions;
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
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IProductRepository _repository;
        private readonly IMapping<Product> _mapping;
        private readonly WillysScrapeService _willysScrapeService;
        private readonly IcaScrapeService _icaScrapeService;
        private readonly CoopScrapeService _coopScrapeService;
        private readonly CitygrossScrapeService _citygrossScrapeService;
        public ScraperService(ILogger<ScraperService> logger, IScrapeConfigHelper configHelper, IProductRepository repository, IMapping<Product> mapping, WillysScrapeService willysScrapeService, IcaScrapeService icaScrapeService, CoopScrapeService coopScrapeService, CitygrossScrapeService citygrossScrapeService)
        {
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
            _mapping = mapping;
            _willysScrapeService = willysScrapeService;
            _icaScrapeService = icaScrapeService;
            _coopScrapeService = coopScrapeService;
            _citygrossScrapeService = citygrossScrapeService;
        }

        public async Task<ScrapingJob> ScrapeWillysAsync(string navigation, Store storeConfig, int category)
        {
            if (string.IsNullOrWhiteSpace(navigation)) throw new ArgumentException("Navigation path cannot be null or empty.", nameof(navigation));
            ArgumentNullException.ThrowIfNull(storeConfig);

            var job = new ScrapingJob
            {
                StoreName = storeConfig.Name,
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting {store} scraping job at {time}", job.StoreName, job.StartedAt);

                var scrapedProducts = await ScrapeWillys(navigation, storeConfig);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                    var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                    job.NewProducts = savedProduct[0];
                    job.UpdatedProducts = savedProduct[1];
                    scope.Complete();
                }

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
            if (string.IsNullOrWhiteSpace(navigation)) throw new ArgumentException("Navigation path cannot be null or empty.", nameof(navigation));
            ArgumentNullException.ThrowIfNull(storeConfig);

            var job = new ScrapingJob
            {
                StoreName = "Ica",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting {store} scraping job at {time}", job.StoreName, job.StartedAt);

                var scrapedProducts = await ScrapeIca(navigation, storeConfig);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                    var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                    job.NewProducts = savedProduct[0];
                    job.UpdatedProducts = savedProduct[1];

                    scope.Complete();
                }

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
            if (string.IsNullOrWhiteSpace(navigation)) throw new ArgumentException("Navigation path cannot be null or empty.", nameof(navigation));
            ArgumentNullException.ThrowIfNull(storeConfig);

            var job = new ScrapingJob
            {
                StoreName = "Coop",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting Coop scraping job at {time}", job.StartedAt);

                var scrapedProducts = await ScrapeCoop(navigation, storeConfig);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                    var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                    job.NewProducts = savedProduct[0];
                    job.UpdatedProducts = savedProduct[1];

                    scope.Complete();
                }

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
        public async Task<ScrapingJob> ScrapeCityGrossAsync(string navigation, Store storeConfig, int category)
        {
            if (string.IsNullOrWhiteSpace(navigation)) throw new ArgumentException("Navigation path cannot be null or empty.", nameof(navigation));
            ArgumentNullException.ThrowIfNull(storeConfig);

            var job = new ScrapingJob
            {
                StoreName = "City Gross",
                StartedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Starting City Gross scraping job at {time}", job.StartedAt);

                var scrapedProducts = await ScrapeCitygross(navigation, storeConfig);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var mappedProducts = await _mapping.ToProduct(scrapedProducts);
                    var savedProduct = await _repository.SaveAsync(mappedProducts, category);

                    job.NewProducts = savedProduct[0];
                    job.UpdatedProducts = savedProduct[1];

                    scope.Complete();
                }

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
        
        protected virtual Task<List<ScrapedProduct>> ScrapeWillys(string navigation, Store storeConfig)
        {
            return _willysScrapeService.ScrapeProductsAsync(navigation, storeConfig);
        }

        protected virtual Task<List<ScrapedProduct>> ScrapeIca(string navigation, Store storeConfig)
        {
            return _icaScrapeService.ScrapeProductsAsync(navigation, storeConfig);
        }

        protected virtual Task<List<ScrapedProduct>> ScrapeCoop(string navigation, Store storeConfig)
        {
            return _coopScrapeService.ScrapeProductsAsync(navigation, storeConfig);
        }

        protected virtual Task<List<ScrapedProduct>> ScrapeCitygross(string navigation, Store storeConfig)
        {
            return _citygrossScrapeService.ScrapeProductsAsync(navigation, storeConfig);
        }
        protected virtual async Task DelayBetweenRequests(int milliseconds = 10000)
        {
            await Task.Delay(milliseconds);
        }
    }
}