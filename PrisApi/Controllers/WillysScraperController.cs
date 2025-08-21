using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;
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
        private readonly IRepository<ScraperConfig> _repository;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly List<(int zip, string city)> Zipcode = [
            (82130, "Bollnäs"),
            (80257, "Gävle"),
            (75318, "Uppsala Kungsgatan 95"),
            (0, "Uppsala "),
            (0, "Stockholm, -")
        ];
        public WillysScraperController(ScraperService scrapingService, ILogger<WillysScraperController> logger, IScrapeConfigHelper configHelper, IRepository<ScraperConfig> repository)
        {
            _scrapingService = scrapingService;
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
        }
        private async Task<List<(string a, int b)>> GetConfig()
        {
            ScraperConfig config = await _repository.GetOnFilterAsync(c => c.Id == 2);

            var navigation = new List<(string a, int b)>
            {
            (config.ScraperNavigation.NavMeat, 1),
            (config.ScraperNavigation.NavDairy, 2),
            (config.ScraperNavigation.NavFruitAndVegetables, 3),
            (config.ScraperNavigation.NavPantry, 4),
            (config.ScraperNavigation.NavFrozen, 5),
            (config.ScraperNavigation.NavBreadAndCookies, 6),
            (config.ScraperNavigation.NavFishAndSeafood, 7),
            (config.ScraperNavigation.NavVegetarian, 8),
            (config.ScraperNavigation.NavIceCreamCandyAndSnacks, 9),
            (config.ScraperNavigation.NavBeverage, 10),
            (config.ScraperNavigation.NavReadyMeals, 11),
            (config.ScraperNavigation.NavKids, 12),
            (config.ScraperNavigation.NavHomeAndCleaning, 13),
            (config.ScraperNavigation.NavHealth, 14),
            (config.ScraperNavigation.NavPharmacy, 15),
            (config.ScraperNavigation.NavAnimals, 16),
            (config.ScraperNavigation.NavTobacco, 17),
            };

            return navigation;
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
        public async Task<IActionResult> ScrapeWillysMeat()
        {
            _logger.LogInformation("Scrape of Willys meat initiated");
            var config = await _configHelper.GetConfig(2);
            System.Console.WriteLine(config.ScraperNavigation.NavMeat);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavMeat, Zipcode[0].zip, 1); // Add Selectors and get zip from Store-StoreLocation

            return Ok(new
            {
                Id = job.Id,
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
        [HttpPost("WillysDariy")]
        public async Task<IActionResult> ScrapeWillysDariy()
        {
            _logger.LogInformation("Scrape of Willys dariy initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavMeat, Zipcode[0].zip, 2);

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
        [HttpPost("WillysFruit")]
        public async Task<IActionResult> ScrapeWillysFruit()
        {
            _logger.LogInformation("Scrape of Willys fruit initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFruitAndVegetables, Zipcode[0].zip, 3);

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
        [HttpPost("WillysPantry")]
        public async Task<IActionResult> ScrapeWillysPantry()
        {
            _logger.LogInformation("Scrape of Willys pantry initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPantry, Zipcode[0].zip, 4);

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
        [HttpPost("WillysFrozen")]
        public async Task<IActionResult> ScrapeWillysFrozen()
        {
            _logger.LogInformation("Scrape of Willys frozen initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFrozen, Zipcode[0].zip, 5);

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
        [HttpPost("WillysBread")]
        public async Task<IActionResult> ScrapeWillysBread()
        {
            _logger.LogInformation("Scrape of Willys bread initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBreadAndCookies, Zipcode[0].zip, 6);

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
        [HttpPost("WillysFish")]
        public async Task<IActionResult> ScrapeWillysFish()
        {
            _logger.LogInformation("Scrape of Willys fish initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavFishAndSeafood, Zipcode[0].zip, 7);

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
        [HttpPost("WillysVege")]
        public async Task<IActionResult> ScrapeWillysVege()
        {
            _logger.LogInformation("Scrape of Willys vege initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavVegetarian, Zipcode[0].zip, 8);

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
        [HttpPost("WillysSnacks")]
        public async Task<IActionResult> ScrapeWillysSnacks()
        {
            _logger.LogInformation("Scrape of Willys snacks initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, Zipcode[0].zip, 9);

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
        [HttpPost("WillysDrinks")]
        public async Task<IActionResult> ScrapeWillysDrinks()
        {
            _logger.LogInformation("Scrape of Willys drinks initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavBeverage, Zipcode[0].zip, 10);

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
        [HttpPost("WillysPrePackageMeal")]
        public async Task<IActionResult> ScrapeWillysPrePackageMeal()
        {
            _logger.LogInformation("Scrape of Willys prepackage initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavReadyMeals, Zipcode[0].zip, 11);

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
        [HttpPost("WillysKids")]
        public async Task<IActionResult> ScrapeWillysKids()
        {
            _logger.LogInformation("Scrape of Willys kids initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavKids, Zipcode[0].zip, 12);
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
        [HttpPost("WillysCleaning")]
        public async Task<IActionResult> ScrapeWillysCleaning()
        {
            _logger.LogInformation("Scrape of Willys cleaning initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHomeAndCleaning, Zipcode[0].zip, 13);

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
        [HttpPost("WillysHealth")]
        public async Task<IActionResult> ScrapeWillysHealth()
        {
            _logger.LogInformation("Scrape of Willys health initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavHealth, Zipcode[0].zip, 14);

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
        [HttpPost("WillysPharmacy")]
        public async Task<IActionResult> ScrapeWillysPharmacy()
        {
            _logger.LogInformation("Scrape of Willys pharmacy initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavPharmacy, Zipcode[0].zip, 15);

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
        [HttpPost("WillysAnimal")]
        public async Task<IActionResult> ScrapeWillysAnimal()
        {
            _logger.LogInformation("Scrape of Willys animal initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavAnimals, Zipcode[0].zip, 16);

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
        [HttpPost("WillysTobak")]
        public async Task<IActionResult> ScrapeWillysTobak()
        {
            _logger.LogInformation("Scrape of Willys tobak initiated");
            var config = await _configHelper.GetConfig(2);

            var job = await _scrapingService.ScrapeWillysAsync(config.ScraperNavigation.NavTobacco, Zipcode[0].zip, 17);

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