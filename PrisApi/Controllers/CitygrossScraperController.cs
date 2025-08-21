using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
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
        private readonly List<(int zip, string city)> zipcode = [
            (80293, "Gävle Ingenjörsgatan 15"),
        ];
        public CitygrossScraperController(ScraperService scraperService, ILogger<CitygrossScraperController> logger, IScrapeConfigHelper configHelper)
        {
            _scrapingService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
        }
        [HttpPost("CityGrossMeat")]
        public async Task<IActionResult> ScrapeCityGrossMeat()
        {
            _logger.LogInformation("Scrape of CityGross meat initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavMeat, zipcode[0].zip, 1);

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
        [HttpPost("CityGrossDeli")]
        public async Task<IActionResult> ScrapeCityGrossDeli()
        {
            _logger.LogInformation("Scrape of CityGross deli initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavChark, zipcode[0].zip, 1);

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
        [HttpPost("CityGrossDairy")]
        public async Task<IActionResult> ScrapeCityGrossDairy()
        {
            _logger.LogInformation("Scrape of CityGross dairy initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavDairy, zipcode[0].zip, 2);

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
        [HttpPost("CityGrossFruit")]
        public async Task<IActionResult> ScrapeCityGrossFruit()
        {
            _logger.LogInformation("Scrape of CityGross fruit initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFruitAndVegetables, zipcode[0].zip, 3);

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
        [HttpPost("CityGrossPantry")]
        public async Task<IActionResult> ScrapeCityGrossPantry()
        {
            _logger.LogInformation("Scrape of CityGross pantry initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPantry, zipcode[0].zip, 4);

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
        [HttpPost("CityGrossFrozen")]
        public async Task<IActionResult> ScrapeCityGrossFrozen()
        {
            _logger.LogInformation("Scrape of CityGross frozen initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFrozen, zipcode[0].zip, 5);

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
        [HttpPost("CityGrossBread")]
        public async Task<IActionResult> ScrapeCityGrossBread()
        {
            _logger.LogInformation("Scrape of CityGross bread initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBreadAndCookies, zipcode[0].zip, 6);

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
        [HttpPost("CityGrossFish")]
        public async Task<IActionResult> ScrapeCityGrossFish()
        {
            _logger.LogInformation("Scrape of CityGross fish initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavFishAndSeafood, zipcode[0].zip, 7);

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
        [HttpPost("CityGrossVege")]
        public async Task<IActionResult> ScrapeCityGrossVege()
        {
            _logger.LogInformation("Scrape of CityGross vege initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavVegetarian, zipcode[0].zip, 8);

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
        [HttpPost("CityGrossSnacks")]
        public async Task<IActionResult> ScrapeCityGrossSnacks()
        {
            _logger.LogInformation("Scrape of CityGross snacks initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavSnacks, zipcode[0].zip, 9);

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
        [HttpPost("CityGrossCandy")]
        public async Task<IActionResult> ScrapeCityGrossCandy()
        {
            _logger.LogInformation("Scrape of CityGross candy initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, zipcode[0].zip, 9);

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
        [HttpPost("CityGrossDrinks")]
        public async Task<IActionResult> ScrapeCityGrossDrinks()
        {
            _logger.LogInformation("Scrape of CityGross drinks initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavBeverage, zipcode[0].zip, 10);

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
        [HttpPost("CityGrossPrePackageMeal")]
        public async Task<IActionResult> ScrapeCityGrossPrePackageMeal()
        {
            _logger.LogInformation("Scrape of CityGross prepackage initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavReadyMeals, zipcode[0].zip, 11);

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
        [HttpPost("CityGrossKids")]
        public async Task<IActionResult> ScrapeCityGrossKids()
        {
            _logger.LogInformation("Scrape of CityGross kids initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavKids, zipcode[0].zip, 12);

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
        [HttpPost("CityGrossCleaning")]
        public async Task<IActionResult> ScrapeCityGrossCleaning()
        {
            _logger.LogInformation("Scrape of CityGross cleaning initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHomeAndCleaning, zipcode[0].zip, 13);

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
        [HttpPost("CityGrossHygiene")]
        public async Task<IActionResult> ScrapeCityGrossHygiene()
        {
            _logger.LogInformation("Scrape of CityGross hygiene initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHygien, zipcode[0].zip, 14);

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
        [HttpPost("CityGrossHealth")]
        public async Task<IActionResult> ScrapeCityGrossHealt()
        {
            _logger.LogInformation("Scrape of CityGross health initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavHealth, zipcode[0].zip, 14);

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
        [HttpPost("CityGrossPharmacy")]
        public async Task<IActionResult> ScrapeCityGrossPharmacy()
        {
            _logger.LogInformation("Scrape of CityGross pharmacy initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavPharmacy, zipcode[0].zip, 15);

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
        [HttpPost("CityGrossAnimal")]
        public async Task<IActionResult> ScrapeCityGrossAnimal()
        {
            _logger.LogInformation("Scrape of CityGross animal initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavAnimals, zipcode[0].zip, 16);

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
        [HttpPost("CityGrossTobak")]
        public async Task<IActionResult> ScrapeCityGrossTobak()
        {
            _logger.LogInformation("Scrape of CityGross tobak initiated");
            var config = await _configHelper.GetConfig(4);

            var job = await _scrapingService.ScrapeCityGrossAsync(config.ScraperNavigation.NavTobacco, zipcode[0].zip, 17);

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