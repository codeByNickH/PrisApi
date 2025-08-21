using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Helper
{
    public class ScrapeConfigHelper : IScrapeConfigHelper
    {
        private readonly IRepository<ScraperConfig> _repository;
        public ScrapeConfigHelper(IRepository<ScraperConfig> repository)
        {
            _repository = repository;
        }
        public async Task<ScraperConfig> GetConfig(int id)
        {
            var config = await _repository.GetOnFilterAsync(x => x.Id == id);

            return config;
        }
        public async Task<List<(string a, int b)>> GetLoopConfig(int id)
        {
            ScraperConfig config = await _repository.GetOnFilterAsync(c => c.Id == id);

            var navigation = new List<(string navigation, int category)>
            {
            (config.ScraperNavigation.NavMeat, 1),
            (config.ScraperNavigation.NavDairy, 2),
            (config.ScraperNavigation.NavFruitAndVegetables, 3),
            (config.ScraperNavigation.NavPantry, 4),
            (config.ScraperNavigation.NavFrozen, 5),
            (config.ScraperNavigation.NavBreadAndCookies, 6),
            (config.ScraperNavigation.NavFishAndSeafood, 7),
            (config.ScraperNavigation.NavVegetarian, 8),
            (config.ScraperNavigation.NavIceCreamCandyAndSnacks, 9),
            (config.ScraperNavigation.NavBeverage, 10),
            (config.ScraperNavigation.NavReadyMeals, 11),
            (config.ScraperNavigation.NavKids, 12),
            (config.ScraperNavigation.NavHomeAndCleaning, 13),
            (config.ScraperNavigation.NavHealth, 14),
            (config.ScraperNavigation.NavAnimals, 16),
            (config.ScraperNavigation.NavTobacco, 17)
            };
            if (!string.IsNullOrEmpty(config.ScraperNavigation.NavPharmacy))
            {
                navigation.Add((config.ScraperNavigation.NavPharmacy, 15));
            }
            if (!string.IsNullOrEmpty(config.ScraperNavigation.NavChark))
            {
                navigation.Add((config.ScraperNavigation.NavChark, 1));
            }
            if (!string.IsNullOrEmpty(config.ScraperNavigation.NavCheese))
            {
                navigation.Add((config.ScraperNavigation.NavCheese, 2));
            }

            return navigation;
        }
    }
}