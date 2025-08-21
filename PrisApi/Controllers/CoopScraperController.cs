using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
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
        public CoopScraperController(ScraperService scraperService, ILogger<CoopScraperController> logger, IScrapeConfigHelper configHelper)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
        }
        [HttpPost("CoopMeat")]
        public async Task<IActionResult> ScrapeCoopMeat()
        {
            _logger.LogInformation("Scrape of Coop meat initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavMeat, zipcode[0].zip, 1);

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
        [HttpPost("CoopDairy")]
        public async Task<IActionResult> ScrapeCoopDairy()
        {
            _logger.LogInformation("Scrape of Coop dairy initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavDairy, zipcode[0].zip, 2);

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
        [HttpPost("CoopCheese")]
        public async Task<IActionResult> ScrapeCoopCheese()
        {
            _logger.LogInformation("Scrape of Coop cheese initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavCheese, zipcode[0].zip, 2);

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
        [HttpPost("CoopFruit")]
        public async Task<IActionResult> ScrapeCoopFruit()
        {
            _logger.LogInformation("Scrape of Coop fruit initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFruitAndVegetables, zipcode[0].zip, 3);

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
        [HttpPost("CoopPantry")]
        public async Task<IActionResult> ScrapeCoopPantry()
        {
            _logger.LogInformation("Scrape of Coop pantry initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavPantry, zipcode[0].zip, 4);

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
        [HttpPost("CoopFrozen")]
        public async Task<IActionResult> ScrapeCoopFrozen()
        {
            _logger.LogInformation("Scrape of Coop frozen initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFrozen, zipcode[0].zip, 5);

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
        [HttpPost("CoopBread")]
        public async Task<IActionResult> ScrapeCoopBread()
        {
            _logger.LogInformation("Scrape of Coop bread initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBreadAndCookies, zipcode[0].zip, 6);

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
        [HttpPost("CoopFish")]
        public async Task<IActionResult> ScrapeCoopFish()
        {
            _logger.LogInformation("Scrape of Coop fish initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavFishAndSeafood, zipcode[0].zip, 7);

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
        [HttpPost("CoopVege")]
        public async Task<IActionResult> ScrapeCoopVege()
        {
            _logger.LogInformation("Scrape of Coop vege initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavVegetarian, zipcode[0].zip, 8);

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
        [HttpPost("CoopSnacks")]
        public async Task<IActionResult> ScrapeCoopSnacks()
        {
            _logger.LogInformation("Scrape of Coop snacks initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, zipcode[0].zip, 9);

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
        [HttpPost("CoopDrinks")]
        public async Task<IActionResult> ScrapeCoopDrinks()
        {
            _logger.LogInformation("Scrape of Coop drinks initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavBeverage, zipcode[0].zip, 10);

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
        [HttpPost("CoopPrePackageMeal")]
        public async Task<IActionResult> ScrapeCoopPrePackageMeal()
        {
            _logger.LogInformation("Scrape of Coop pre-packaged initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavReadyMeals, zipcode[0].zip, 11);

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
        [HttpPost("CoopKids")]
        public async Task<IActionResult> ScrapeCoopKids()
        {
            _logger.LogInformation("Scrape of Coop kids initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavKids, zipcode[0].zip, 12);

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
        [HttpPost("CoopCleaning")]
        public async Task<IActionResult> ScrapeCoopCleaning()
        {
            _logger.LogInformation("Scrape of Coop cleaning initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHomeAndCleaning, zipcode[0].zip, 13);

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
        [HttpPost("CoopHealth")]
        public async Task<IActionResult> ScrapeCoopHealth()
        {
            _logger.LogInformation("Scrape of Coop health initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHealth, zipcode[0].zip, 14);

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
        [HttpPost("CoopHygien")]
        public async Task<IActionResult> ScrapeCoopHygien()
        {
            _logger.LogInformation("Scrape of Coop hygien initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavHygien, zipcode[0].zip, 14);

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
        [HttpPost("CoopAnimal")]
        public async Task<IActionResult> ScrapeCoopAnimal()
        {
            _logger.LogInformation("Scrape of Coop animal initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavAnimals, zipcode[0].zip, 16);

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
        [HttpPost("CoopTobak")]
        public async Task<IActionResult> ScrapeCoopTobak()
        {
            _logger.LogInformation("Scrape of Coop tobak initiated");
            var config = await _configHelper.GetConfig(3);

            var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavTobacco, zipcode[0].zip, 17);

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
        // [HttpPost("CoopSpices")]
        // public async Task<IActionResult> ScrapeCoopSpices()
        // {
        //     _logger.LogInformation("Scrape of Coop spices initiated");
        //     var config = await _configHelper.GetConfig(3);

        //     var job = await _scraperService.ScrapeCoopAsync(config.ScraperNavigation.NavMeat, zipcode[0].zip, 1);

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