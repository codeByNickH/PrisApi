using Microsoft.AspNetCore.Mvc;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HemkopScraperController : ControllerBase  // Not implemented, does not have store separation on online data.
    {
        private readonly ScraperService _scrapingService;
        private readonly ILogger<HemkopScraperController> _logger;
        public HemkopScraperController(ScraperService scrapingService, ILogger<HemkopScraperController> logger)
        {
            _scrapingService = scrapingService;
            _logger = logger;
        }
        // [HttpPost("HemkopMeat")]
        // public async Task<IActionResult> ScrapeHemkopMeat()
        // {
        //     _logger.LogInformation("Scrape of Hemkop meat initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[0], 82231);

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
        // [HttpPost("HemkopDairy")]
        // public async Task<IActionResult> ScrapeHemkopDairy()
        // {
        //     _logger.LogInformation("Scrape of Hemkop dairy initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[1], 82231);

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
        // [HttpPost("HemkopFruit")]
        // public async Task<IActionResult> ScrapeHemkopFruit()
        // {
        //     _logger.LogInformation("Scrape of Hemkop fruit initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[2], 82231);

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
        // [HttpPost("HemkopPantry")]
        // public async Task<IActionResult> ScrapeHemkopPantry()
        // {
        //     _logger.LogInformation("Scrape of Hemkop pantry initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[3], 82231);

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
        // [HttpPost("HemkopFrozen")]
        // public async Task<IActionResult> ScrapeHemkopFrozen()
        // {
        //     _logger.LogInformation("Scrape of Hemkop frozen initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[4], 82231);

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
        // [HttpPost("HemkopBread")]
        // public async Task<IActionResult> ScrapeHemkopBread()
        // {
        //     _logger.LogInformation("Scrape of Hemkop bread initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[5], 82231);

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
        // [HttpPost("HemkopFish")]
        // public async Task<IActionResult> ScrapeHemkopFish()
        // {
        //     _logger.LogInformation("Scrape of Hemkop fish initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[6], 82231);

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
        // [HttpPost("HemkopVege")]
        // public async Task<IActionResult> ScrapeHemkopVege()
        // {
        //     _logger.LogInformation("Scrape of Hemkop vege initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[7], 82231);

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
        // [HttpPost("HemkopSnacks")]
        // public async Task<IActionResult> ScrapeHemkopSnacks()
        // {
        //     _logger.LogInformation("Scrape of Hemkop snacks initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[8], 82231);

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
        // [HttpPost("HemkopDrinks")]
        // public async Task<IActionResult> ScrapeHemkopDrinks()
        // {
        //     _logger.LogInformation("Scrape of Hemkop drinks initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[9], 82231);

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
        // [HttpPost("HemkopPrePackagedMeal")]
        // public async Task<IActionResult> ScrapeHemkopPrePackagedMeal()
        // {
        //     _logger.LogInformation("Scrape of Hemkop prepackage initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[10], 82231);

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
        // [HttpPost("HemkopKids")]
        // public async Task<IActionResult> ScrapeHemkopKids()
        // {
        //     _logger.LogInformation("Scrape of Hemkop kids initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[11], 82231);

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
        // [HttpPost("HemkopGarden")]
        // public async Task<IActionResult> ScrapeHemkopGarden()
        // {
        //     _logger.LogInformation("Scrape of Hemkop garden initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[12], 82231);

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
        // [HttpPost("HemkopHome")]
        // public async Task<IActionResult> ScrapeHemkopHome()
        // {
        //     _logger.LogInformation("Scrape of Hemkop home initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[13], 82231);

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
        // [HttpPost("HemkopHealth")]
        // public async Task<IActionResult> ScrapeHemkopHealth()
        // {
        //     _logger.LogInformation("Scrape of Hemkop health initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[14], 82231);

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
        // [HttpPost("HemkopPharmacy")]
        // public async Task<IActionResult> ScrapeHemkopPharmacy()
        // {
        //     _logger.LogInformation("Scrape of Hemkop pharmacy initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[15], 82231);

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
        // [HttpPost("HemkopAnimal")]
        // public async Task<IActionResult> ScrapeHemkopAnimal()
        // {
        //     _logger.LogInformation("Scrape of Hemkop animal initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[16], 82231);

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
        // [HttpPost("HemkopTobak")]
        // public async Task<IActionResult> ScrapeHemkopTobak()
        // {
        //     _logger.LogInformation("Scrape of Hemkop tobak initiated");

        //     var job = await _scrapingService.ScrapeHemkopAsync(category[17], 82231);

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