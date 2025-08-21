using PrisApi.Models.Scraping;

namespace PrisApi.Helper.IHelper
{
    public interface IScrapeConfigHelper
    {
        Task<ScraperConfig> GetConfig(int id);
        Task<List<(string a, int b)>> GetLoopConfig(int id);
    }
}