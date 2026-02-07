using Microsoft.Extensions.Options;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Services
{
    public class DiscordService : IDiscordService
    {
        private readonly string _bnasWebhookUrl;
        private readonly string _gvlWebhookUrl;
        public DiscordService(IOptions<BnasDiscordSettings> bnasOptions, IOptions<GvlDiscordSettings> gvlOptions)
        {
            _bnasWebhookUrl = bnasOptions.Value.BnasWebhookUrl;
            _gvlWebhookUrl = gvlOptions.Value.GvlWebhookUrl;
        }
        public async Task SendToDiscordAsync(List<ProductPriceChange> changes)
        {
            if (changes == null || !changes.Any()) return;

            var webhookUrl = changes.First().City == "Gävle" ? _gvlWebhookUrl : _bnasWebhookUrl;

            if (string.IsNullOrEmpty(webhookUrl))
            {
                Console.WriteLine($"Error: Webhook URL is missing for city: {changes.First().City}");
                return;
            }

            var contentList = new List<string>();

            foreach (var p in changes)
            {
                var priceArrow = p.OldPrice == null ? "🆕 NY" : (p.NewPrice < p.OldPrice ? "📉 NER" : "📈 UPP");
                var compareArrow = p.OldComparePrice == null ? "🆕 NY" : (p.NewComparePrice < p.OldComparePrice ? "📉 NER" : "📈 UPP");

                var jmfChange = p.OldComparePrice == null ? 0 : Math.Abs(p.NewComparePrice.GetValueOrDefault() - p.OldComparePrice.GetValueOrDefault());
                var stChange = p.OldPrice == null ? 0 : Math.Abs(p.NewPrice - p.OldPrice.GetValueOrDefault());

                var content = $"""
                **🚨 Produkt: {p.ProductName}**
                **Märke: {p?.Brand}**
                **Land: {p?.CountryOfOrigin}**
                **Butik: {p.StoreName},** {p.City}
                {p.Address}
                
                {(p.MultiOffer != null ? $"**{p.MultiOffer}**" : "")}
                **Pris:** {p.OldPrice}kr ➡ **{p.NewPrice}kr** {priceArrow} **{stChange:F2}**
                **Jmf/{p.Unit} Pris:** {p.OldComparePrice}kr ➡ **{p.NewComparePrice}kr** {compareArrow} **{jmfChange:F2}**
                **Unit:** {p.Size}{p.Unit}
                
                ----------------------------------
                """;
                contentList.Add(content);
            }

            await ProcessBatchesAsync(contentList, webhookUrl);
        }
        public async Task SendErrorToDiscordAsync(List<ScrapingJob> scrapingJobs)
        {
            if (scrapingJobs == null || !scrapingJobs.Any()) return;

            if (string.IsNullOrEmpty(_bnasWebhookUrl))
            {
                Console.WriteLine("Error: BNäs Discord Webhook URL is missing.");
                return;
            }

            var contentList = new List<string>();

            foreach (var j in scrapingJobs)
            {
                if (!j.Success)
                {
                    var content = $"""
                    **🚨 Error Alert: {j.StoreLocation}**
                    **Store:** {j.StoreName}
                
                    **Error Message:** {j?.ErrorMessage}

                    ----------------------------------
                    """;
                    contentList.Add(content);
                }
            }

            await ProcessBatchesAsync(contentList, _bnasWebhookUrl);
        }

        private async Task ProcessBatchesAsync(List<string> messages, string webhookUrl)
        {
            if (!messages.Any()) return;

            var currentPayload = string.Empty;
            const int maxLength = 2000;

            foreach (var message in messages)
            {
                // Check if adding this message exceeds limit
                if (currentPayload.Length + message.Length > maxLength)
                {
                    // Send current batch
                    await SendWithRetryAsync(webhookUrl, currentPayload);
                    
                    // Reset payload with current message
                    currentPayload = message + "\n";
                    
                    // Small delay to prevent rate limiting
                    await Task.Delay(500);
                }
                else
                {
                    currentPayload += message + "\n";
                }
            }

            // Send remaining payload
            if (!string.IsNullOrWhiteSpace(currentPayload))
            {
                await SendWithRetryAsync(webhookUrl, currentPayload);
            }
        }

        private async Task SendWithRetryAsync(string url, string contentString)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            var client = new HttpClient();
            var payload = new { content = contentString };
            int maxRetries = 3;
            int delay = 1000;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var response = await client.PostAsJsonAsync(url, payload);

                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Discord Webhook Failed (Attempt {i + 1}/{maxRetries}): {response.StatusCode}. Details: {responseBody}");

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception sending to Discord (Attempt {i + 1}/{maxRetries}): {ex.Message}");
                }

                if (i < maxRetries - 1)
                {
                    await Task.Delay(delay * (i + 1));
                }
            }
        }
    }
}