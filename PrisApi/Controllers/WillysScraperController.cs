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
    public class WillysScraperController : ControllerBase   // Only run between 04:00-08:45
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<WillysScraperController> _logger;
        private readonly IRepository<Store> _locationRepository;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly AppDbContext _dbContext;
        public WillysScraperController(ScraperService scrapingService, ILogger<WillysScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext)
        {
            _scrapingService = scrapingService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavMeat, loc, 1);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }
        [HttpPost("WillysDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysDairy()
        {
            _logger.LogInformation("Scrape of Willys dairy initiated");
            var config = await _configHelper.GetConfig(2);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavDairy, loc, 2);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFruitAndVegetables, loc, 3);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPantry, loc, 4);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFrozen, loc, 5);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBreadAndCookies, loc, 6);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFishAndSeafood, loc, 7);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavVegetarian, loc, 8);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc, 9);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBeverage, loc, 10);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavReadyMeals, loc, 11);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavKids, loc, 12);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHomeAndCleaning, loc, 13);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHealth, loc, 14);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPharmacy, loc, 15);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavAnimals, loc, 16);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavTobacco, loc, 17);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return ResponseHelper.CreateApiResponse(jobList);
        }

        // [HttpPost("WillysLoop")]
        // public async Task<ActionResult<APIResponse>> ScrapeWillysLoop()
        // {
        //     _logger.LogInformation("Scrape of Willys loop initiated");
        //     var config = await _configHelper.GetConfig(2);
        //     var location = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);
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
        //     var jobList = new List<ScrapingJob>();
        //     foreach (var loc in location)
        //     {
        //         int i = 0;
        //         jobList.AddRange(await _scrapingService.ScrapeWillysAsync(config, loc));
        //         jobList[i].StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
        //         i++;
        //             // jobList.Add(job);
        //         // foreach (var nav in navigation)
        //         // {
        //         //     await _dbContext.ScrapingJobs.AddRangeAsync(job);
        //         //     await _dbContext.SaveChangesAsync();
        //         // }
        //     }
        //     return ResponseHelper.CreateApiResponse(jobList);
        // }

        // [HttpPost("WillysOffers")]
        // public async Task<IActionResult> ScrapeWillysOffers()
        // {
        //     _logger.LogInformation("Scrape of Willys initiated");

        //     var job = await _scrapingService.ScrapeWillysOffersAsync(Zipcode[0].zip);
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