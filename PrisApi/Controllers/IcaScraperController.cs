using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IcaScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;
        private readonly ILogger<IcaScraperController> _logger;
        private readonly IRepository<ScraperConfig> _repository;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly List<(int zip, string city)> Zipcode = [
            (82141, "Bollnäs"),
            (80293, "Gävle Hemlingby Köpcentrum"),
            (75267, "Uppsala Stenhagen"),
            (0, ""),
            (0, ""),
            (0, ""),
            (0, ""),
            (0, ""),
        ];
        public IcaScraperController(ScraperService scraperService, ILogger<IcaScraperController> logger, IScrapeConfigHelper configHelper, IRepository<ScraperConfig> repository)
        {
            _scraperService = scraperService;
            _logger = logger;
            _configHelper = configHelper;
            _repository = repository;
        }
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
        [HttpPost("IcaOffers")] // Add location on this
        public async Task<IActionResult> ScrapeIcaOffers()
        {
            _logger.LogInformation("Scrape of Ica offers initiated");

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
            _logger.LogInformation("Scrape of Ica meat initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavMeat, Zipcode[0].zip, 1);

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
            _logger.LogInformation("Scrape of Ica dariy initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavDairy, Zipcode[0].zip, 2);

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
            _logger.LogInformation("Scrape of Ica fruit initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFruitAndVegetables, Zipcode[0].zip, 3);

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
            _logger.LogInformation("Scrape of Ica pantry initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavPantry, Zipcode[0].zip, 4);

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
            _logger.LogInformation("Scrape of Ica frozen initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFrozen, Zipcode[0].zip, 5);

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
            _logger.LogInformation("Scrape of Ica bread initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBreadAndCookies, Zipcode[0].zip, 6);

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
            _logger.LogInformation("Scrape of Ica fish initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavFishAndSeafood, Zipcode[0].zip, 7);

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
            _logger.LogInformation("Scrape of Ica vege initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavVegetarian, Zipcode[0].zip, 8);

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
            _logger.LogInformation("Scrape of Ica snacks initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavIceCreamCandyAndSnacks, Zipcode[0].zip, 9);

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
            _logger.LogInformation("Scrape of Ica drinks initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavBeverage, Zipcode[0].zip, 10);

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
            _logger.LogInformation("Scrape of Ica prepackaged initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavReadyMeals, Zipcode[0].zip, 11);

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
            _logger.LogInformation("Scrape of Ica kids initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavKids, Zipcode[0].zip, 12);

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
            _logger.LogInformation("Scrape of Ica cleaning initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHomeAndCleaning, Zipcode[0].zip, 13);

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
        [HttpPost("IcaHealth")]
        public async Task<IActionResult> ScrapeIcaHealth()
        {
            _logger.LogInformation("Scrape of Ica health initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavHealth, Zipcode[0].zip, 14);

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
            _logger.LogInformation("Scrape of Ica animal initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavAnimals, Zipcode[0].zip, 16);

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
            _logger.LogInformation("Scrape of Ica tobak initiated");
            var config = await _configHelper.GetConfig(1);

            var job = await _scraperService.ScrapeIcaAsync(config.ScraperNavigation.NavTobacco, Zipcode[0].zip, 17);

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
        // [HttpPost("IcaKitchen")]
        // public async Task<IActionResult> ScrapeIcaKitchen()
        // {
        //     _logger.LogInformation("Scrape of Ica kitchen initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[14], Zipcode[0].zip);

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
        // [HttpPost("IcaTraining")]
        // public async Task<IActionResult> ScrapeIcaTraining()
        // {
        //     _logger.LogInformation("Scrape of Ica training initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[16], Zipcode[0].zip);

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
        // [HttpPost("IcaGarden")]
        // public async Task<IActionResult> ScrapeIcaGarden()
        // {
        //     _logger.LogInformation("Scrape of Ica garden initiated");

        //     var job = await _scraperService.ScrapeIcaAsync(category[18], Zipcode[0].zip);

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