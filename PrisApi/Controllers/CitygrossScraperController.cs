using Microsoft.AspNetCore.Mvc;
using PrisApi.Data;
using PrisApi.Helper;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services;
using PrisApi.Services.IService;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitygrossScraperController : ControllerBase
    {
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IScrapeOrchestrator _scrapeOrchestrator;
        public CitygrossScraperController(IScrapeOrchestrator scrapeOrchestrator, IScrapeConfigHelper configHelper)
        {
            _scrapeOrchestrator = scrapeOrchestrator;
            _configHelper = configHelper;
        }
        [HttpPost("CityGrossMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossMeat()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavMeat,
                CategoryId = 1,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross meat",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossDeli")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDeli()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavChark,
                CategoryId = 1,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross deli",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDairy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavDairy,
                CategoryId = 2,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross dairy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFruit()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavFruitAndVegetables,
                CategoryId = 3,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross fruit",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPantry()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavPantry,
                CategoryId = 4,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross pantry",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFrozen()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavFrozen,
                CategoryId = 5,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross frozen",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossBread")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossBread()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavBreadAndCookies,
                CategoryId = 6,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross bread",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossFish")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossFish()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavFishAndSeafood,
                CategoryId = 7,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross fish",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossVege")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossVege()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavVegetarian,
                CategoryId = 8,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross vegetarian",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossSnacks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavSnacks,
                CategoryId = 9,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross snacks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossCandy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossCandy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavIceCreamCandyAndSnacks,
                CategoryId = 9,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross candy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossDrinks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavBeverage,
                CategoryId = 10,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross drinks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPrePackageMeal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavReadyMeals,
                CategoryId = 11,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross prepackage meal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossKids")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossKids()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavKids,
                CategoryId = 12,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross kids",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossCleaning()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavHomeAndCleaning,
                CategoryId = 13,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross cleaning",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossHygiene")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossHygiene()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavHygien,
                CategoryId = 14,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross hygiene",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossHealth")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossHealth()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavHealth,
                CategoryId = 14,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross health",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossPharmacy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavPharmacy,
                CategoryId = 15,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross pharmacy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossAnimal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavAnimals,
                CategoryId = 16,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross animal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CityGrossTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeCityGrossTobak()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 4,
                NavigationPath = (await _configHelper.GetConfig(4)).ScraperNavigation.NavTobacco,
                CategoryId = 17,
                AllowedCities = new[] { "Gävle" },
                ActionName = "CityGross tobacco",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
    }
}