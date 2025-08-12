using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoopScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<CoopScraperController> _logger;
        private readonly List<string> category = [
            "kott-fagel-chark",
            "mejeri-agg",
            "ost",
            "fisk-skaldjur",
            "frukt-gronsaker",
            "vegetariskt",
            "frys",
            "brod-bageri",
            "skafferi",
            "dryck",
            "godis-glass-snacks",
            "fardigmat-mellanmal",
            "kryddor-smaksattare",
            "aktuella-erbjudanden"
        ];
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
        public CoopScraperController(ScraperService scraperService, ILogger<CoopScraperController> logger)
        {
            _scraperService = scraperService;
            _logger = logger;
        }
        [HttpPost("CoopMeat")]
        public async Task<IActionResult> ScrapeCoopMeat()
        {
            _logger.LogInformation("Manual scraping of Coop meat initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[0], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop dairy initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[1], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop cheese initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[2], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop fish initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[3], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop fruit initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[4], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop vege initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[5], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop frozen initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[6], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop bread initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[7], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop pantry initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[8], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop drinks initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[9], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop snacks initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[10], zipcode[0].zip);

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
            _logger.LogInformation("Manual scraping of Coop pre-packaged initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[11], zipcode[0].zip);

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
        [HttpPost("CoopSpices")]
        public async Task<IActionResult> ScrapeCoopSpices()
        {
            _logger.LogInformation("Manual scraping of Coop spices initiated");

            var job = await _scraperService.ScrapeCoopAsync(category[12], zipcode[0].zip);

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
    }
}