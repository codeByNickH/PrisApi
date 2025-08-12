using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WillysScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<WillysScraperController> _logger;
        private readonly List<string> category = [
            "/sortiment/kott-chark-och-fagel",
            "/sortiment/mejeri-ost-och-agg",
            "/sortiment/frukt-och-gront",
            "/sortiment/skafferi",
            "/sortiment/fryst",
            "/sortiment/brod-och-kakor",
            "/sortiment/fisk-och-skaldjur",
            "/sortiment/vegetariskt",
            "/sortiment/glass-godis-och-snacks",
            "/sortiment/dryck",
            "/sortiment/fardigmat",
            "/sortiment/barn",
            "/sortiment/blommor-och-tradgard",
            "/sortiment/hem-och-stad",
            "/sortiment/halsa-och-skonhet",
            "/sortiment/apotek",
            "/sortiment/djur",
            "/sortiment/tobak",
            "/sortiment/kiosk"
        ];
        private readonly List<(int zip, string city)> Zipcode = [
            (82130, "Bollnäs"),
            (80257, "Gävle"),
            (75318, "Uppsala Kungsgatan 95"),
            (0, "Uppsala "),
            (0, "Stockholm, -")
        ];
        public WillysScraperController(ScraperService scrapingService, ILogger<WillysScraperController> logger)
        {
            _scrapingService = scrapingService;
            _logger = logger;
        }

        [HttpPost("WillysOffers")]
        public async Task<IActionResult> ScrapeWillysOffers()
        {
            _logger.LogInformation("Manual scrape of Willys initiated");

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
            _logger.LogInformation("Manual scrape of Willys loop initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category, Zipcode[0].zip);

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
        [HttpPost("WillysMeat")]
        public async Task<IActionResult> ScrapeWillysMeat()
        {
            _logger.LogInformation("Manual scrape of Willys meat initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[0], Zipcode[0].zip);

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
        [HttpPost("WillysDariy")]
        public async Task<IActionResult> ScrapeWillysDariy()
        {
            _logger.LogInformation("Manual scrape of Willys dariy initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[1], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys fruit initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[2], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys pantry initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[3], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys frozen initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[4], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys bread initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[5], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys fish initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[6], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys vege initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[7], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys snacks initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[8], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys drinks initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[9], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys prepackage initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[10], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys kids initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[11], Zipcode[0].zip);

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
        [HttpPost("WillysGarden")]
        public async Task<IActionResult> ScrapeWillysGarden()
        {
            _logger.LogInformation("Manual scrape of Willys garden initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[12], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys cleaning initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[13], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys health initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[14], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys pharmacy initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[15], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys animal initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[16], Zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of Willys tobak initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[17], Zipcode[0].zip);

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
        [HttpPost("WillysKiosk")]
        public async Task<IActionResult> ScrapeWillysKiosk()
        {
            _logger.LogInformation("Manual scrape of Willys kiosk initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[18], Zipcode[0].zip);

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