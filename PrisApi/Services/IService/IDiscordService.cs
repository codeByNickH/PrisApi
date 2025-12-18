using PrisApi.Models;

namespace PrisApi.Services.IService
{
    public interface IDiscordService
    {
        Task SendToDiscordAsync(List<ProductPriceChange> changes);
    }
}