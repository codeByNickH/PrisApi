using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScraperController : ControllerBase
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<ScraperController> _logger;
        // Change to enum or seperate file? 
        private readonly List<string> category = [
            "sortiment/kott-chark-och-fagel",
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
            "sortiment/kiosk"];
        public ScraperController(ScraperService scrapingService, ILogger<ScraperController> logger)
        {
            _scrapingService = scrapingService;
            _logger = logger;
        }

        [HttpPost("WillysOffers")]
        public async Task<IActionResult> ScrapeWillysOffers(string store)
        {
            _logger.LogInformation("Manual scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysOffersAsync(store);

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
            _logger.LogInformation("Manual scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[0]);

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
            _logger.LogInformation("Manual scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[1]);

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
            _logger.LogInformation("Manual scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[2]);

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
        [HttpPost("WillysTest")]
        public async Task<IActionResult> ScrapeWillysTest(int i)
        {
            _logger.LogInformation("Manual scrape of Willys initiated");

            var job = await _scrapingService.ScrapeWillysAsync(category[i]);

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

        // [HttpPost("IcaOffers")]
        // public async Task<IActionResult> ScrapeIcaOffers()
        // {
        //     _logger.LogInformation("Manual scrape of Ica initiated");

        //     var job = await _scrapingService.ScrapeIcaOffersAsync();

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