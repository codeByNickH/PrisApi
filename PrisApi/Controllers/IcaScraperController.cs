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
            "Kött, Chark & Fågel",
            "Mejeri & Ost",
            "Frukt & Grönt",
            "Fisk & Skaldjur",
            "Bröd & Kakor",
            "Vegetariskt",
            "Färdigmat",
            "Barn",
            "Glass, Godis & Snacks",
            "Dryck",
            "Skafferi",
            "Fyst",
            "Tobak",
            "Städ, Tvätt & Papper",
            "Kök",
            "Apotek, Skönhet & Hälsa",
            "Träning & Återhämtning",
            "Djur",
            "Blommor & Trädgård",
            ];
        private readonly List<(int zip, string city)> Zipcode = [
            (82391, "Bollnäs"),
            (0, "Gävle"),
            (75267, "Uppsala Stenhagen"),
            (75267, "Uppsala Stenhagen"),
            (75267, "Uppsala Stenhagen"),
            (75267, "Uppsala Stenhagen"),
            (75267, "Uppsala Stenhagen"),
            (75267, "Uppsala Stenhagen"),
        ];
        public IcaScraperController(ScraperService scraperService, ILogger<IcaScraperController> logger)
        {
            _scraperService = scraperService;
            _logger = logger;
        }
        [HttpPost("IcaOffers")] // Add location on this
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

            var job = await _scraperService.ScrapeIcaAsync(category[0], Zipcode[0].zip);

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

            var job = await _scraperService.ScrapeIcaAsync(category[1], Zipcode[0].zip);

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
        [HttpPost("IcaFruit")]
        public async Task<IActionResult> ScrapeIcaFruit()
        {
            _logger.LogInformation("Manual scraping of Ica fruit initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[2], Zipcode[0].zip);

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
        [HttpPost("IcaFish")]
        public async Task<IActionResult> ScrapeIcaFish()
        {
            _logger.LogInformation("Manual scraping of Ica fish initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[3], Zipcode[0].zip);

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
        [HttpPost("IcaBread")]
        public async Task<IActionResult> ScrapeIcaBread()
        {
            _logger.LogInformation("Manual scraping of Ica bread initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[4], Zipcode[0].zip);

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
        [HttpPost("IcaVege")]
        public async Task<IActionResult> ScrapeIcaVege()
        {
            _logger.LogInformation("Manual scraping of Ica vege initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[5], Zipcode[0].zip);

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
        [HttpPost("IcaPrePackagedMeal")]
        public async Task<IActionResult> ScrapeIcaPrePackagedMeal()
        {
            _logger.LogInformation("Manual scraping of Ica prepackaged initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[6], Zipcode[0].zip);

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
        [HttpPost("IcaKids")]
        public async Task<IActionResult> ScrapeIcaKids()
        {
            _logger.LogInformation("Manual scraping of Ica kids initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[7], Zipcode[0].zip);

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
        [HttpPost("IcaSnacks")]
        public async Task<IActionResult> ScrapeIcaSnacks()
        {
            _logger.LogInformation("Manual scraping of Ica snacks initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[8], Zipcode[0].zip);

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
        [HttpPost("IcaDrinks")]
        public async Task<IActionResult> ScrapeIcaDrinks()
        {
            _logger.LogInformation("Manual scraping of Ica drinks initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[9], Zipcode[0].zip);

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
        [HttpPost("IcaPantry")]
        public async Task<IActionResult> ScrapeIcaPantry()
        {
            _logger.LogInformation("Manual scraping of Ica pantry initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[10], Zipcode[0].zip);

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
        [HttpPost("IcaFrozen")]
        public async Task<IActionResult> ScrapeIcaFrozen()
        {
            _logger.LogInformation("Manual scraping of Ica frozen initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[11], Zipcode[0].zip);

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
        [HttpPost("IcaTobak")]
        public async Task<IActionResult> ScrapeIcaTobak()
        {
            _logger.LogInformation("Manual scraping of Ica tobak initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[12], Zipcode[0].zip);

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
        [HttpPost("IcaCleaning")]
        public async Task<IActionResult> ScrapeIcaCleaning()
        {
            _logger.LogInformation("Manual scraping of Ica cleaning initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[13], Zipcode[0].zip);

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
        [HttpPost("IcaKitchen")]
        public async Task<IActionResult> ScrapeIcaKitchen()
        {
            _logger.LogInformation("Manual scraping of Ica kitchen initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[14], Zipcode[0].zip);

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
        [HttpPost("IcaPharmacy")]
        public async Task<IActionResult> ScrapeIcaPharmacy()
        {
            _logger.LogInformation("Manual scraping of Ica pharmacy initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[15], Zipcode[0].zip);

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
        [HttpPost("IcaTraining")]
        public async Task<IActionResult> ScrapeIcaTraining()
        {
            _logger.LogInformation("Manual scraping of Ica training initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[16], Zipcode[0].zip);

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
        [HttpPost("IcaAnimal")]
        public async Task<IActionResult> ScrapeIcaAnimal()
        {
            _logger.LogInformation("Manual scraping of Ica animal initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[17], Zipcode[0].zip);

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
        [HttpPost("IcaGarden")]
        public async Task<IActionResult> ScrapeIcaGarden()
        {
            _logger.LogInformation("Manual scraping of Ica garden initiated");

            var job = await _scraperService.ScrapeIcaAsync(category[18], Zipcode[0].zip);

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