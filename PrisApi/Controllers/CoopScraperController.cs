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
    public class CoopScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<CoopScraperController> _logger;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IRepository<Store> _locationRepository;
        private readonly AppDbContext _dbContext;
        private readonly IDiscordService _discordService;
        public CoopScraperController(ScraperService scraperService, ILogger<CoopScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> locationRepository, AppDbContext dbContext, IDiscordService discordService)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _dbContext = dbContext;
            _discordService = discordService;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavMeat, loc, 1);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavDairy, loc, 2);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavCheese, loc, 2);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFruitAndVegetables, loc, 3);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavPantry, loc, 4);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
        }
        
        // [HttpPost("CoopSpices")]
        // public async Task<ActionResult<APIResponse>> ScrapeCoopSpices() kryddor-smaksattare | Add on Pantry category?
        // {
        //     _logger.LogInformation("Scrape of Coop spices initiated");
        //   var config = await _configHelper.GetConfig(3);
        //   var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

        //   var jobList = new List<ScrapingJob>();
        //   foreach (var loc in location)
        //   {
        //      var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavSpices, loc.zip, 4);
        //      job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
        //      jobList.Add(job);
        //      await _dbContext.ScrapingJobs.AddAsync(job);
        //      await _dbContext.SaveChangesAsync();
        //   }
        //   return ResponseHelper.CreateApiResponse(jobList);
        // }

        [HttpPost("CoopFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFrozen()
        {
            _logger.LogInformation("Scrape of Coop frozen initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFrozen, loc, 5);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBreadAndCookies, loc, 6);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFishAndSeafood, loc, 7);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavVegetarian, loc, 8);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc, 9);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBeverage, loc, 10);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavReadyMeals, loc, 11);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavKids, loc, 12);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHomeAndCleaning, loc, 13);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
        }
        [HttpPost("CoopPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPharmacy()
        {
            _logger.LogInformation("Scrape of Coop pharmacy initiated");
            var config = await _configHelper.GetConfig(3);
            var location = await _locationRepository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavPharmacy, loc, 14);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHygien, loc, 14);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavAnimals, loc, 16);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
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
                if (loc.StoreLocation.City == "Bollnäs")
                {
                    var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavTobacco, loc, 17);
                    job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                    jobList.Add(job);
                    await _dbContext.ScrapingJobs.AddAsync(job);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var response = ResponseHelper.CreateApiResponse(jobList);
            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }
            return response;
        }
    }
}