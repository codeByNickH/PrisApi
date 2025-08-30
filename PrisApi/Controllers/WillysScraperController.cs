using Microsoft.AspNetCore.Mvc;
using PrisApi.Data;
using PrisApi.Helper;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Repository;
using PrisApi.Repository.IRepository;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WillysScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<WillysScraperController> _logger;
        private readonly IRepository<Store> _locationRepository;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly AppDbContext _dbContext;
        private readonly List<(int zip, string city)> Zipcode = [
            (82130, "Bollnäs"),
            (80257, "Gävle"),
            (0, "Söderhamn"),
            (0, "Hudiksvall"),
            (75318, "Uppsala"),
            (0, "Stockholm")
        ];
        public WillysScraperController(ScraperService scrapingService, ILogger<WillysScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext)
        {
            _scrapingService = scrapingService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
        }
        [HttpPost("WillysOffers")]
        public async Task<IActionResult> ScrapeWillysOffers()
        {
            _logger.LogInformation("Scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysOffersAsync(Zipcode[0].zip);
            return Ok(new
            {
                Success = job.Success,
                ProductsScraped = job.ProductsScraped,
                NewProducts = job.NewProducts,
                UpdatedProducts = job.UpdatedProducts,
                StartedAt = job.StartedAt,
                CompletedAt = job.CompletedAt,
                ErrorMessage = job.ErrorMessage
            });
        }
        [HttpPost("WillysLoop")]
        public async Task<IActionResult> ScrapeWillysLoop()
        {
            _logger.LogInformation("Scrape of Willys loop initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config, Zipcode[0].zip);
            await _dbContext.ScrapingJobs.AddAsync(job);
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                StoreName = job.StoreName,
                Success = job.Success,
                ProductsScraped = job.ProductsScraped,
                NewProducts = job.NewProducts,
                UpdatedProducts = job.UpdatedProducts,
                StartedAt = job.StartedAt,
                CompletedAt = job.CompletedAt,
                ErrorMessage = job.ErrorMessage
            });
        }
        [HttpPost("WillysMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysMeat()
        {
            _logger.LogInformation("Scrape of Willys meat initiated");
            var config = await _configHelper.GetConfig(2);
            // Get all willys stores and loop through all locations?
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavMeat, loc.StoreLocation.PostalCode, 1);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }

            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysDariy")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysDariy()
        {
            _logger.LogInformation("Scrape of Willys dariy initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavDairy, loc.StoreLocation.PostalCode, 2);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFruit()
        {
            _logger.LogInformation("Scrape of Willys fruit initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFruitAndVegetables, loc.StoreLocation.PostalCode, 3);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPantry()
        {
            _logger.LogInformation("Scrape of Willys pantry initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPantry, loc.StoreLocation.PostalCode, 4);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFrozen()
        {
            _logger.LogInformation("Scrape of Willys frozen initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFrozen, loc.StoreLocation.PostalCode, 5);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysBread")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysBread()
        {
            _logger.LogInformation("Scrape of Willys bread initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBreadAndCookies, loc.StoreLocation.PostalCode, 6);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysFish")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFish()
        {
            _logger.LogInformation("Scrape of Willys fish initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFishAndSeafood, loc.StoreLocation.PostalCode, 7);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysVege")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysVege()
        {
            _logger.LogInformation("Scrape of Willys vege initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavVegetarian, loc.StoreLocation.PostalCode, 8);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysSnacks()
        {
            _logger.LogInformation("Scrape of Willys snacks initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc.StoreLocation.PostalCode, 9);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysDrinks()
        {
            _logger.LogInformation("Scrape of Willys drinks initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBeverage, loc.StoreLocation.PostalCode, 10);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPrePackageMeal()
        {
            _logger.LogInformation("Scrape of Willys prepackage initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavReadyMeals, loc.StoreLocation.PostalCode, 11);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysKids")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysKids()
        {
            _logger.LogInformation("Scrape of Willys kids initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavKids, loc.StoreLocation.PostalCode, 12);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysCleaning()
        {
            _logger.LogInformation("Scrape of Willys cleaning initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHomeAndCleaning, loc.StoreLocation.PostalCode, 13);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysHealth()
        {
            _logger.LogInformation("Scrape of Willys health initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHealth, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPharmacy()
        {
            _logger.LogInformation("Scrape of Willys pharmacy initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPharmacy, loc.StoreLocation.PostalCode, 15);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysAnimal()
        {
            _logger.LogInformation("Scrape of Willys animal initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavAnimals, loc.StoreLocation.PostalCode, 16);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("WillysTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysTobak()
        {
            _logger.LogInformation("Scrape of Willys tobak initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavTobacco, loc.StoreLocation.PostalCode, 17);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        // [HttpPost("WillysGarden")]
        // public async Task<IActionResult> ScrapeWillysGarden()
        // {
        //     _logger.LogInformation("Scrape of Willys garden initiated");
        //     var config = await GetConfig();
        //     var job = await _scrapingService.ScrapeWillysAsync(config[12], Zipcode[0].zip);

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

        // [HttpPost("WillysKiosk")]
        // public async Task<IActionResult> ScrapeWillysKiosk()
        // {
        //     _logger.LogInformation("Scrape of Willys kiosk initiated");

        //     var job = await _scrapingService.ScrapeWillysAsync(category[18], Zipcode[0].zip);

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