using Microsoft.AspNetCore.Mvc;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoopScraperController : ControllerBase
    {
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IScrapeOrchestrator _scrapeOrchestrator;
        public CoopScraperController(IScrapeOrchestrator scrapeOrchestrator, IScrapeConfigHelper configHelper)
        {
            _scrapeOrchestrator = scrapeOrchestrator;
            _configHelper = configHelper;
        }
        [HttpPost("CoopMeat")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopMeat()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavMeat,
                CategoryId = 1,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop meat",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopDairy")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopDairy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavDairy,
                CategoryId = 2,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop dairy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopCheese")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopCheese()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavCheese,
                CategoryId = 2,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop cheese",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopFruit")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFruit()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavFruitAndVegetables,
                CategoryId = 3,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop fruit",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopPantry")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPantry()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavPantry,
                CategoryId = 4,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop pantry",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        
        // [HttpPost("CoopSpices")]
        // public async Task<ActionResult<APIResponse>> ScrapeCoopSpices() // kryddor-smaksattare | Add on Pantry category?
        // {
        //   var request = new ScrapeRequest
        //     {
        //         ConfigId = 3,
        //         NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.navSpices,
        //         CategoryId = 4,
        //         AllowedCities = new[] { "Bollnäs", "Gävle" },
        //         AllowedDistricts = new[] { "Valbo" },
        //         ActionName = "Coop spices",
        //     };

        //     return await _scrapeOrchestrator.OrchestrateAsync(request);
        // }

        [HttpPost("CoopFrozen")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFrozen()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavFrozen,
                CategoryId = 5,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop frozen",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopBread")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopBread()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavBreadAndCookies,
                CategoryId = 6,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop bread",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopFish")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopFish()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavFishAndSeafood,
                CategoryId = 7,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop fish",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopVege")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopVege()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavVegetarian,
                CategoryId = 8,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop vegetarian",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopSnacks")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopSnacks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavIceCreamCandyAndSnacks,
                CategoryId = 9,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop snacks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopDrinks")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopDrinks()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavBeverage,
                CategoryId = 10,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop drinks",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopPrePackageMeal")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPrePackageMeal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavReadyMeals,
                CategoryId = 11,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop prepackage meal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopKids")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopKids()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavKids,
                CategoryId = 12,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop kids",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopCleaning")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopCleaning()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavHomeAndCleaning,
                CategoryId = 13,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop cleaning",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopPharmacy")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopPharmacy()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavPharmacy,
                CategoryId = 14,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop pharmacy",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopHygien")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopHygien()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavHygien,
                CategoryId = 14,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop hygien",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopAnimal")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopAnimal()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavAnimals,
                CategoryId = 16,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop animal",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
        [HttpPost("CoopTobak")]
        public async Task<ActionResult<APIResponse>> ScrapeCoopTobak()
        {
            var request = new ScrapeRequest
            {
                ConfigId = 3,
                NavigationPath = (await _configHelper.GetConfig(3)).ScraperNavigation.NavTobacco,
                CategoryId = 17,
                AllowedCities = new[] { "Bollnäs", "Gävle" },
                AllowedDistricts = new[] { "Valbo" },
                ActionName = "Coop tobacco",
            };

            return await _scrapeOrchestrator.OrchestrateAsync(request);
        }
    }
}