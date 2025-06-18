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

            var job = await _scraperService.ScrapeCoopAsync(category[0]);

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