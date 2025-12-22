using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.IService
{
    public interface IDiscordService
    {
        Task SendToDiscordAsync(List<ProductPriceChange> changes);
        Task SendErrorToDiscordAsync(List<ScrapingJob> scrapingJobs);
    }
}