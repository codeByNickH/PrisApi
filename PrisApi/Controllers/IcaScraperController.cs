using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IcaScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<IcaScraperController> _logger;
        private readonly List<string> category = [
            "kött-chark-fågel/3b5facca-3c54-421b-b46c-5397d7548270?source=navigation",
            "mejeri-ost/c4af8a37-6d03-478e-9c4d-f76907a52c3c?source=navigation"
            ];
        public IcaScraperController(ScraperService scraperService, ILogger<IcaScraperController> logger)
        {
            _scraperService = scraperService;
            _logger = logger;
        }
        [HttpPost("IcaOffers")]
        public async Task<IActionResult> ScrapeIcaOffers()
        {
            _logger.LogInformation("Manual scraping of Ica offers initiated");

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
        public async Task<IActionResult> ScrapeIcaMeat()
        {
            _logger.LogInformation("Manual scraping of Ica meat initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[0]);

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
        [HttpPost("IcaDariy")]
        public async Task<IActionResult> ScrapeIcaDariy()
        {
            _logger.LogInformation("Manual scraping of Ica dariy initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[1]);

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