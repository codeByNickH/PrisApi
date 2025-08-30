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
        private readonly List<(int zip, string city)> Zipcode = [
            (82141, "Bollnäs"),
            (80293, "Gävle Hemlingby Köpcentrum"),
            (75267, "Uppsala Stenhagen"),
            (0, ""),
            (0, ""),
            (0, ""),
            (0, ""),
            (0, ""),
        ];
        public IcaScraperController(ScraperService scraperService, ILogger<IcaScraperController> logger, IScrapeConfigHelper configHelper, IRepository<Store> repository, AppDbContext dbContext)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
            _dbContext = dbContext;
        }
        [HttpPost("IcaOffers")] // Add location on this
        public async Task<IActionResult> ScrapeIcaOffers()
        {
            _logger.LogInformation("Scrape of Ica offers initiated");

            var job = await _scraperService.ScrapeIcaOffersAsync();

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
        [HttpPost("IcaMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaMeat()
        {
            _logger.LogInformation("Scrape of Ica meat initiated");
            var config = await _configHelper.GetConfig(1);
            var location = await _repository.GetListOnFilterAsync(l => l.Name.Contains(config.StoreName));

            var jobList = new List<ScrapingJob>();
            foreach (var loc in location)
            {
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavMeat, loc.StoreLocation.PostalCode, 1);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }

            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavDairy, loc.StoreLocation.PostalCode, 2);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFruitAndVegetables, loc.StoreLocation.PostalCode, 3);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavPantry, loc.StoreLocation.PostalCode, 4);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFrozen, loc.StoreLocation.PostalCode, 5);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBreadAndCookies, loc.StoreLocation.PostalCode, 6);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFishAndSeafood, loc.StoreLocation.PostalCode, 7);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavVegetarian, loc.StoreLocation.PostalCode, 8);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, loc.StoreLocation.PostalCode, 9);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBeverage, loc.StoreLocation.PostalCode, 10);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavReadyMeals, loc.StoreLocation.PostalCode, 11);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavKids, loc.StoreLocation.PostalCode, 12);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHomeAndCleaning, loc.StoreLocation.PostalCode, 13);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHealth, loc.StoreLocation.PostalCode, 14);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavAnimals, loc.StoreLocation.PostalCode, 16);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
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
                var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavTobacco, loc.StoreLocation.PostalCode, 17);
                jobList.Add(job);
                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }
            return ResponseExtention.CreateResponse(jobList);
        }
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