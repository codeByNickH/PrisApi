using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitygrossScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<CitygrossScraperController> _logger;
        private readonly List<string> category = [
            "matvaror/kott-och-fagel",
            "matvaror/frukt-och-gront",
            "matvaror/mejeri-ost-och-agg",
            "matvaror/skafferiet",
            "matvaror/fryst",
            "matvaror/brod-och-bageri",
            "matvaror/hushall",
            "matvaror/godis",
            "matvaror/dryck",
            "matvaror/snacks",
            "matvaror/skonhet-och-hygien",
            "matvaror/chark",
            "matvaror/fisk-och-skaldjur",
            "matvaror/kyld-fardigmat",
            "matvaror/vegetariskt",
            "matvaror/barn",
            "matvaror/blommor",
            "matvaror/hem-och-fritid",
            "matvaror/koket",
            "matvaror/husdjur",
            "matvaror/apotek-och-receptfria-lakemedel",
            "matvaror/halsa",
            "matvaror/tobak",
        ];
        private readonly List<(int zip, string city)> zipcode = [
            (80293, "Gävle"),
        ];
        public CitygrossScraperController(ScraperService scraperService, ILogger<CitygrossScraperController> logger)
        {
            _scrapingService = scraperService;
            _logger = logger;
        }
        [HttpPost("CityGrossMeat")]
        public async Task<IActionResult> ScrapeCityGrossMeat()
        {
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[0], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[1], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[2], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[3], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[4], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[5], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[6], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[7], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[8], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[9], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[10], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[11], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[12], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[13], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[14], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[15], zipcode[0].zip);

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
        [HttpPost("CityGrossGarden")]
        public async Task<IActionResult> ScrapeCityGrossGarden()
        {
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[16], zipcode[0].zip);

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
        [HttpPost("CityGrossHome")]
        public async Task<IActionResult> ScrapeCityGrossHome()
        {
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[17], zipcode[0].zip);

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
        [HttpPost("CityGrossKitchen")]
        public async Task<IActionResult> ScrapeCityGrossKitchen()
        {
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[18], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[19], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[20], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[21], zipcode[0].zip);

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
            _logger.LogInformation("Manual scrape of CityGross initiated");

            var job = await _scrapingService.ScrapeCityGrossAsync(category[22], zipcode[0].zip);

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