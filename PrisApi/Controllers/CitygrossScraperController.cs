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
    public class CitygrossScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<CitygrossScraperController> _logger;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IRepository<Store> _locationRepository;
        private readonly AppDbContext _dbContext;
        private readonly List<(int zip, string city)> zipcode = [
            (80293, "Gävle Ingenjörsgatan 15"),
        ];
        public CitygrossScraperController(ScraperService scraperService, ILogger<CitygrossScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext)
        {
            _scrapingService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
        }
        [HttpPost("CityGrossMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossMeat()
        {
            _logger.LogInformation("Scrape of CityGross meat initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavMeat, loc.StoreLocation.PostalCode, 1);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossDeli")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDeli()
        {
            _logger.LogInformation("Scrape of CityGross deli initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavChark, loc.StoreLocation.PostalCode, 1);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDairy()
        {
            _logger.LogInformation("Scrape of CityGross dairy initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavDairy, loc.StoreLocation.PostalCode, 2);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFruit()
        {
            _logger.LogInformation("Scrape of CityGross fruit initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFruitAndVegetables, loc.StoreLocation.PostalCode, 3);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPantry()
        {
            _logger.LogInformation("Scrape of CityGross pantry initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPantry, loc.StoreLocation.PostalCode, 4);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFrozen()
        {
            _logger.LogInformation("Scrape of CityGross frozen initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFrozen, loc.StoreLocation.PostalCode, 5);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossBread")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossBread()
        {
            _logger.LogInformation("Scrape of CityGross bread initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBreadAndCookies, loc.StoreLocation.PostalCode, 6);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossFish")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFish()
        {
            _logger.LogInformation("Scrape of CityGross fish initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFishAndSeafood, loc.StoreLocation.PostalCode, 7);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossVege")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossVege()
        {
            _logger.LogInformation("Scrape of CityGross vege initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavVegetarian, loc.StoreLocation.PostalCode, 8);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossSnacks()
        {
            _logger.LogInformation("Scrape of CityGross snacks initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavSnacks, loc.StoreLocation.PostalCode, 9);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossCandy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossCandy()
        {
            _logger.LogInformation("Scrape of CityGross candy initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc.StoreLocation.PostalCode, 9);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDrinks()
        {
            _logger.LogInformation("Scrape of CityGross drinks initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBeverage, loc.StoreLocation.PostalCode, 10);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPrePackageMeal()
        {
            _logger.LogInformation("Scrape of CityGross prepackage initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavReadyMeals, loc.StoreLocation.PostalCode, 11);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossKids")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossKids()
        {
            _logger.LogInformation("Scrape of CityGross kids initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavKids, loc.StoreLocation.PostalCode, 12);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossCleaning()
        {
            _logger.LogInformation("Scrape of CityGross cleaning initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHomeAndCleaning, loc.StoreLocation.PostalCode, 13);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossHygiene")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossHygiene()
        {
            _logger.LogInformation("Scrape of CityGross hygiene initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHygien, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossHealt()
        {
            _logger.LogInformation("Scrape of CityGross health initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHealth, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPharmacy()
        {
            _logger.LogInformation("Scrape of CityGross pharmacy initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPharmacy, loc.StoreLocation.PostalCode, 15);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossAnimal()
        {
            _logger.LogInformation("Scrape of CityGross animal initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavAnimals, loc.StoreLocation.PostalCode, 16);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CityGrossTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossTobak()
        {
            _logger.LogInformation("Scrape of CityGross tobak initiated");
            var config = await _configHelper.GetConfig(4);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavTobacco, loc.StoreLocation.PostalCode, 17);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        // [HttpPost("CityGrossKitchen")]
        // public async Task<IActionResult> ScrapeCityGrossKitchen()
        // {
        //     _logger.LogInformation("Scrape of CityGross kitchen initiated");

        //     var job = await _scrapingService.ScrapeCityGrossAsync(category[18], zipcode[0].zip);

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
        // [HttpPost("CityGrossGarden")]
        // public async Task<IActionResult> ScrapeCityGrossGarden()
        // {
        //     _logger.LogInformation("Scrape of CityGross garden initiated");
        //     var config = await _configHelper.GetConfig(4);

        //     var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation., zipcode[0].zip, 18);

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