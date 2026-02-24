using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IcaScraperController : ControllerBase
    {
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IScrapeOrchestrator _scrapeOrchestrator;
        public IcaScraperController(IScrapeOrchestrator scrapeOrchestrator, IScrapeConfigHelper configHelper)
        {
            _scrapeOrchestrator = scrapeOrchestrator;
            _configHelper = configHelper;
        }
        [HttpPost("IcaMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaMeat()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavMeat,
                CategoryId = 1,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica meat",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaDairy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavDairy,
                CategoryId = 2,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica dairy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFruit()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavFruitAndVegetables,
                CategoryId = 3,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica fruit",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaPantry()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavPantry,
                CategoryId = 4,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica pantry",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFrozen()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavFrozen,
                CategoryId = 5,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica frozen",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaBread")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaBread()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavBreadAndCookies,
                CategoryId = 6,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica bread",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaFish")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaFish()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavFishAndSeafood,
                CategoryId = 7,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica fish",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaVege")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaVege()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavVegetarian,
                CategoryId = 8,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica vegetarian",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaSnacks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavIceCreamCandyAndSnacks,
                CategoryId = 9,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica snacks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaDrinks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavBeverage,
                CategoryId = 10,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica drinks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaPrePackagedMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaPrePackagedMeal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavReadyMeals,
                CategoryId = 11,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica prepackaged meal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaKids")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaKids()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavKids,
                CategoryId = 12,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica kids",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaCleaning()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavHomeAndCleaning,
                CategoryId = 13,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica cleaning",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaHealth()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavHealth,
                CategoryId = 14,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica health",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaAnimal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavAnimals,
                CategoryId = 16,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica animal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("IcaTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeIcaTobak()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 1,
                NavigationPath = (await _configHelper.GetConfig(1)).ScraperNavigation.NavTobacco,
                CategoryId = 17,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Ica tobacco",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
    }
}