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
        public ScraperController(ScraperService scrapingService, ILogger<ScraperController> logger)
        {
            _scrapingService = scrapingService;
            _logger = logger;
        }

        [HttpPost("willys")]
        public async Task<IActionResult> ScrapeWillys()
        {
            _logger.LogInformation("Manual scrape of Willys initiated");
            
            var job = await _scrapingService.ScrapeWillysAsync();
            
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