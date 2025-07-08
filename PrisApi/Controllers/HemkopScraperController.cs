using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HemkopScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<HemkopScraperController> _logger;
        private readonly List<string> category = [
            "sortiment/kott-fagel-och-chark",
            "sortiment/mejeri-ost-och-agg",
            "sortiment/frukt-och-gront",
            "sortiment/skafferi",
            "sortiment/fryst",
            "sortiment/brod-och-kakor",
            "sortiment/fisk-och-skaldjur",
            "sortiment/vegetariskt",
            "sortiment/glass-godis-och-snacks",
            "sortiment/dryck",
            "sortiment/fardigmat",
            "sortiment/barn",
            "sortiment/blommor-och-tradgard",
            "sortiment/hem-och-stad",
            "sortiment/halsa-och-skonhet",
            "sortiment/apotek",
            "sortiment/djur",
            "sortiment/tobak",
            "sortiment/kiosk"
        ];
        public HemkopScraperController(ScraperService scrapingService, ILogger<HemkopScraperController> logger)
        {
            _scrapingService = scrapingService;
            _logger = logger;
        }
        [HttpPost("HemkopMeat")]
        public async Task<IActionResult> ScrapeHemkopMeat()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[0]);

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
        [HttpPost("HemkopDairy")]
        public async Task<IActionResult> ScrapeHemkopDairy()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[1]);

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
        [HttpPost("HemkopFruit")]
        public async Task<IActionResult> ScrapeHemkopFruit()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[2]);

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
        [HttpPost("HemkopPantry")]
        public async Task<IActionResult> ScrapeHemkopPantry()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[3]);

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
        [HttpPost("HemkopFrozen")]
        public async Task<IActionResult> ScrapeHemkopFrozen()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[4]);

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
        [HttpPost("HemkopBread")]
        public async Task<IActionResult> ScrapeHemkopBread()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[5]);

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
        [HttpPost("HemkopFish")]
        public async Task<IActionResult> ScrapeHemkopFish()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[6]);

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
        [HttpPost("HemkopVege")]
        public async Task<IActionResult> ScrapeHemkopVege()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[7]);

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
        [HttpPost("HemkopSnacks")]
        public async Task<IActionResult> ScrapeHemkopSnacks()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[8]);

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
        [HttpPost("HemkopDrinks")]
        public async Task<IActionResult> ScrapeHemkopDrinks()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[9]);

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
        [HttpPost("HemkopPrePackagedMeal")]
        public async Task<IActionResult> ScrapeHemkopPrePackagedMeal()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[10]);

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
        [HttpPost("HemkopKids")]
        public async Task<IActionResult> ScrapeHemkopKids()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[11]);

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
        [HttpPost("HemkopGarden")]
        public async Task<IActionResult> ScrapeHemkopGarden()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[12]);

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
        [HttpPost("HemkopHome")]
        public async Task<IActionResult> ScrapeHemkopHome()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[13]);

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
        [HttpPost("HemkopHealth")]
        public async Task<IActionResult> ScrapeHemkopHealth()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[14]);

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
        [HttpPost("HemkopPharmacy")]
        public async Task<IActionResult> ScrapeHemkopPharmacy()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[15]);

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
        [HttpPost("HemkopAnimal")]
        public async Task<IActionResult> ScrapeHemkopAnimal()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[16]);

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
        [HttpPost("HemkopTobak")]
        public async Task<IActionResult> ScrapeHemkopTobak()
        {
            _logger.LogInformation("Manual scrape of Hemkop initiated");

            var job = await _scrapingService.ScrapeHemkopAsync(category[17]);

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