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
    public class CoopScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<CoopScraperController> _logger;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IRepository<Store> _locationRepository;
        private readonly AppDbContext _dbContext;
        private readonly List<(int zip, string city)> zipcode = [
            (82136, "Bollnäs"),
            (81835, "Gävle Valbo"),
            (75323, "Uppsala"),
            (85753,"Sundsvall"),
            (90621,"Ersboda, Umeå"),
            (97345, "Storheden, Luleå"),
            (16867, "Bromma, Stockholm"),
            (12630, "Västberga, Stockholm"),
            (41730, "Backaplan, Göteborg"),
            (43633, "Sisjön, Göteborg"),
            (23234, "Burlöv, Malmö")
        ];
        public CoopScraperController(ScraperService scraperService, ILogger<CoopScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
        }
        [HttpPost("CoopMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopMeat()
        {
            _logger.LogInformation("Scrape of Coop meat initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavMeat, loc.StoreLocation.PostalCode, 1);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList); // Add store location to scraping job city/district
        }
        [HttpPost("CoopDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopDairy()
        {
            _logger.LogInformation("Scrape of Coop dairy initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavDairy, loc.StoreLocation.PostalCode, 2);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopCheese")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopCheese()
        {
            _logger.LogInformation("Scrape of Coop cheese initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavCheese, loc.StoreLocation.PostalCode, 2);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFruit()
        {
            _logger.LogInformation("Scrape of Coop fruit initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFruitAndVegetables, loc.StoreLocation.PostalCode, 3);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPantry()
        {
            _logger.LogInformation("Scrape of Coop pantry initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavPantry, loc.StoreLocation.PostalCode, 4);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFrozen()
        {
            _logger.LogInformation("Scrape of Coop frozen initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFrozen, loc.StoreLocation.PostalCode, 5);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopBread")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopBread()
        {
            _logger.LogInformation("Scrape of Coop bread initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBreadAndCookies, loc.StoreLocation.PostalCode, 6);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopFish")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFish()
        {
            _logger.LogInformation("Scrape of Coop fish initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFishAndSeafood, loc.StoreLocation.PostalCode, 7);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopVege")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopVege()
        {
            _logger.LogInformation("Scrape of Coop vege initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavVegetarian, loc.StoreLocation.PostalCode, 8);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopSnacks()
        {
            _logger.LogInformation("Scrape of Coop snacks initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc.StoreLocation.PostalCode, 9);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopDrinks()
        {
            _logger.LogInformation("Scrape of Coop drinks initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBeverage, loc.StoreLocation.PostalCode, 10);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPrePackageMeal()
        {
            _logger.LogInformation("Scrape of Coop pre-packaged initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavReadyMeals, loc.StoreLocation.PostalCode, 11);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopKids")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopKids()
        {
            _logger.LogInformation("Scrape of Coop kids initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavKids, loc.StoreLocation.PostalCode, 12);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopCleaning()
        {
            _logger.LogInformation("Scrape of Coop cleaning initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHomeAndCleaning, loc.StoreLocation.PostalCode, 13);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopHealth()
        {
            _logger.LogInformation("Scrape of Coop health initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHealth, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopHygien")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopHygien()
        {
            _logger.LogInformation("Scrape of Coop hygien initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHygien, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopAnimal()
        {
            _logger.LogInformation("Scrape of Coop animal initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavAnimals, loc.StoreLocation.PostalCode, 16);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        [HttpPost("CoopTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopTobak()
        {
            _logger.LogInformation("Scrape of Coop tobak initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavTobacco, loc.StoreLocation.PostalCode, 17);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
        // [HttpPost("CoopSpices")]
        // public async Task<ActionResult<APIResponse>> ScrapeCoopSpices()
        // {
        //     _logger.LogInformation("Scrape of Coop spices initiated");

        //   var config = await _configHelper.GetConfig(3);
        //   var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavMeat, loc.zip, 1);

        //     return ResponseExtention.CreateResponse(job);
        // }
    }
}