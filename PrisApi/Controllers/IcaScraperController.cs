using Microsoft.AspNetCore.Mvc;
using PrisApi.Data;
using PrisApi.Helper;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IcaScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<IcaScraperController> _logger;
        private readonly IRepository<Store> _repository;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly AppDbContext _dbContext;
        public IcaScraperController(ScraperService scraperService, ILogger<IcaScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> repository, AppDbContext dbContext)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
            _dbContext = dbContext;
        }
        [HttpPost("IcaMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaMeat()
        {
            _logger.LogInformation("Scrape of Ica meat initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavMeat, loc, 1);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }

            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaDariy")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaDariy()
        {
            _logger.LogInformation("Scrape of Ica dariy initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavDairy, loc, 2);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFruit()
        {
            _logger.LogInformation("Scrape of Ica fruit initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFruitAndVegetables, loc, 3);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaPantry()
        {
            _logger.LogInformation("Scrape of Ica pantry initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavPantry, loc, 4);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFrozen()
        {
            _logger.LogInformation("Scrape of Ica frozen initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFrozen, loc, 5);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaBread")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaBread()
        {
            _logger.LogInformation("Scrape of Ica bread initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBreadAndCookies, loc, 6);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaFish")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFish()
        {
            _logger.LogInformation("Scrape of Ica fish initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFishAndSeafood, loc, 7);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaVege")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaVege()
        {
            _logger.LogInformation("Scrape of Ica vege initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavVegetarian, loc, 8);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaSnacks()
        {
            _logger.LogInformation("Scrape of Ica snacks initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc, 9);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaDrinks()
        {
            _logger.LogInformation("Scrape of Ica drinks initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBeverage, loc, 10);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaPrePackagedMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaPrePackagedMeal()
        {
            _logger.LogInformation("Scrape of Ica prepackaged initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavReadyMeals, loc, 11);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaKids")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaKids()
        {
            _logger.LogInformation("Scrape of Ica kids initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavKids, loc, 12);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaCleaning()
        {
            _logger.LogInformation("Scrape of Ica cleaning initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHomeAndCleaning, loc, 13);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaHealth()
        {
            _logger.LogInformation("Scrape of Ica health initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHealth, loc, 14);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaAnimal()
        {
            _logger.LogInformation("Scrape of Ica animal initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavAnimals, loc, 16);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("IcaTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaTobak()
        {
            _logger.LogInformation("Scrape of Ica tobak initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavTobacco, loc, 17);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }

        // [HttpPost("IcaOffers")] // Add location on this
        // public async Task<IActionResult> ScrapeIcaOffers()
        // {
        //     _logger.LogInformation("Scrape of Ica offers initiated");

        //     var job = await _scraperService.ScrapeIcaOffersAsync();

        //     return Ok(new
        //     {
        //         Success = job.Success,
        //         ProductsScraped = job.ProductsScraped,
        //         NewProducts = job.NewProducts,
        //         UpdatedProducts = job.UpdatedProducts,
        //         StartedAt = job.StartedAt,
        //         CompletedAt = job.CompletedAt,
        //         ErrorMessage = job.ErrorMessage
        //     });
        // }

        // [HttpPost("IcaKitchen")]
        // public async Task<IActionResult> ScrapeIcaKitchen()
        // {
        //     _logger.LogInformation("Scrape of Ica kitchen initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[14], Zipcode[0].zip);

        //     return Ok(new
        //     {
        //         Success = job.Success,
        //         ProductsScraped = job.ProductsScraped,
        //         NewProducts = job.NewProducts,
        //         UpdatedProducts = job.UpdatedProducts,
        //         StartedAt = job.StartedAt,
        //         CompletedAt = job.CompletedAt,
        //         ErrorMessage = job.ErrorMessage
        //     });
        // }

        // [HttpPost("IcaTraining")]
        // public async Task<IActionResult> ScrapeIcaTraining()
        // {
        //     _logger.LogInformation("Scrape of Ica training initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[16], Zipcode[0].zip);

        //     return Ok(new
        //     {
        //         Success = job.Success,
        //         ProductsScraped = job.ProductsScraped,
        //         NewProducts = job.NewProducts,
        //         UpdatedProducts = job.UpdatedProducts,
        //         StartedAt = job.StartedAt,
        //         CompletedAt = job.CompletedAt,
        //         ErrorMessage = job.ErrorMessage
        //     });
        // }

        // [HttpPost("IcaGarden")]
        // public async Task<IActionResult> ScrapeIcaGarden()
        // {
        //     _logger.LogInformation("Scrape of Ica garden initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[18], Zipcode[0].zip);

        //     return Ok(new
        //     {
        //         Success = job.Success,
        //         ProductsScraped = job.ProductsScraped,
        //         NewProducts = job.NewProducts,
        //         UpdatedProducts = job.UpdatedProducts,
        //         StartedAt = job.StartedAt,
        //         CompletedAt = job.CompletedAt,
        //         ErrorMessage = job.ErrorMessage
        //     });
        // }
    }
}