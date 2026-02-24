using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WillysScraperController : ControllerBase   // Only run between 04:00-08:45
    {
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IScrapeOrchestrator _scrapeOrchestrator;
        public WillysScraperController(IScrapeOrchestrator scrapeOrchestrator, IScrapeConfigHelper configHelper)
        {
            _scrapeOrchestrator = scrapeOrchestrator;
            _configHelper = configHelper;
        }
        [HttpPost("WillysMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysMeat()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavMeat,
                CategoryId = 1,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys meat",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysDairy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavDairy,
                CategoryId = 2,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys dairy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFruit()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavFruitAndVegetables,
                CategoryId = 3,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys fruit",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPantry()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavPantry,
                CategoryId = 4,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys pantry",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFrozen()
        {            
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavFrozen,
                CategoryId = 5,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys meat",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysBread")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysBread()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavBreadAndCookies,
                CategoryId = 6,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys bread",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysFish")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysFish()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavFishAndSeafood,
                CategoryId = 7,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys fish",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysVege")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysVege()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavVegetarian,
                CategoryId = 8,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys vegetarian",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysSnacks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavIceCreamCandyAndSnacks,
                CategoryId = 9,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys snacks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysDrinks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavBeverage,
                CategoryId = 10,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys drinks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPrePackageMeal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavReadyMeals,
                CategoryId = 11,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys prepackage meal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysKids")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysKids()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavKids,
                CategoryId = 12,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys kids",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysCleaning()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavHomeAndCleaning,
                CategoryId = 13,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys cleaning",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysHealth()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavHealth,
                CategoryId = 14,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys health",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysPharmacy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavPharmacy,
                CategoryId = 15,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys pharmacy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysAnimal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavAnimals,
                CategoryId = 16,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys animal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("WillysTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeWillysTobak()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 2,
                NavigationPath = (await _configHelper.GetConfig(2)).ScraperNavigation.NavTobacco,
                CategoryId = 17,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                ActionName = "Willys tobacco",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
    }
}