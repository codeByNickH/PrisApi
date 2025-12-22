using Microsoft.AspNetCore.Mvc;
using PrisApi.Data;
using PrisApi.Helper;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services;
using PrisApi.Services.IService;

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
        private readonly IDiscordService _discordService;
        public CitygrossScraperController(ScraperService scraperService, ILogger<CitygrossScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext, IDiscordService discordService)
        {
            _scrapingService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
            _discordService = discordService;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavMeat, loc, 1);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavChark, loc, 1);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavDairy, loc, 2);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFruitAndVegetables, loc, 3);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPantry, loc, 4);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFrozen, loc, 5);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBreadAndCookies, loc, 6);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFishAndSeafood, loc, 7);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavVegetarian, loc, 8);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavSnacks, loc, 9);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc, 9);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBeverage, loc, 10);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavReadyMeals, loc, 11);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavKids, loc, 12);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHomeAndCleaning, loc, 13);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHygien, loc, 14);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHealth, loc, 14);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPharmacy, loc, 15);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavAnimals, loc, 16);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavTobacco, loc, 17);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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