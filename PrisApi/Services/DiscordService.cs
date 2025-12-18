using Microsoft.Extensions.Options;
using PrisApi.Models;
using PrisApi.Services.IService;

namespace PrisApi.Services
{
    public class DiscordService : IDiscordService
    {
        private readonly string _webhookUrl;
        public DiscordService(IOptions<DiscordSettings> options)
        {
            _webhookUrl = options.Value.WebhookUrl;
        }
        public async Task SendToDiscordAsync(List<ProductPriceChange> changes)
        {
            if (!changes.Any()) return;

            using var httpClient = new HttpClient();
            // var webhookUrl = ""; // move to appsettings.json and to env vars.

            if (string.IsNullOrEmpty(_webhookUrl))
            {
                Console.WriteLine("Warning: Discord Webhook URL is missing in appsettings.json");
            }

            var contentList = new List<string>();

            foreach (var p in changes)
            {
                var priceArrow = p.OldPrice == null ? "🆕 NEW" : (p.NewPrice < p.OldPrice ? "📉 DOWN" : "📈 UP");
                var compareArrow = p.OldComparePrice == null ? "NEW" : (p.NewComparePrice < p.OldComparePrice ? "📉 DOWN" : "📈 UP");

                var content = $"""
                **🚨 Price Alert: {p.ProductName}**
                **Store:** {p.StoreName}
            
                **Price:** {p.OldPrice}kr ➡ **{p.NewPrice}kr** ({priceArrow})
                **Compare Price:** {p.OldComparePrice}kr ➡ {p.NewComparePrice}kr ({compareArrow})
                **Unit:** {p.Size}{p.Unit}
                
                ----------------------------------
                """;
                contentList.Add(content);
            }
            var payload = new { content = string.Join("\n", contentList) };

            await httpClient.PostAsJsonAsync(_webhookUrl, payload);
        }
    }
}