using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Services;
using Xunit;

namespace PrisApi.Tests
{
    public class DiscordServiceChunkingTests
    {
        private class TestDiscordService : DiscordService
        {
            public List<string> SentPayloads { get; } = new List<string>();

            public TestDiscordService(IOptions<BnasDiscordSettings> bnasOptions, IOptions<GvlDiscordSettings> gvlOptions)
                : base(bnasOptions, gvlOptions, new NullLogger<DiscordService>())
            {
            }

            protected override Task SendWithRetryAsync(string url, string contentString)
            {
                SentPayloads.Add(contentString ?? string.Empty);
                return Task.CompletedTask;
            }
        }

        private static ProductPriceChange MakeChange(int nameLength, string city = "Bollnäs")
        {
            return new ProductPriceChange
            {
                StoreName = "Store",
                City = city,
                Address = "Addr",
                Brand = "Brand",
                ProductName = new string('A', nameLength),
                NewPrice = 10.0m,
                Size = 1,
                Unit = "st",
                NewComparePrice = 10.0m,
                OldPrice = 9.0m,
                OldComparePrice = 9.0m,
                MultiOffer = "",
                CountryOfOrigin = "SE",
                MemberDiscount = false
            };
        }

        [Fact]
        public async Task Chunking_SplitsIntoMultiplePayloads_WhenMessagesExceedLimit()
        {
            var bnasOptions = Options.Create(new BnasDiscordSettings { BnasWebhookUrl = "http://bnas" });
            var gvlOptions = Options.Create(new GvlDiscordSettings { GvlWebhookUrl = "http://gvl" });

            var svc = new TestDiscordService(bnasOptions, gvlOptions);

            // Create 3 messages that individually are fairly large so that two fit under 2000 but three exceed it
            var changes = new List<ProductPriceChange>
            {
                MakeChange(700),
                MakeChange(700),
                MakeChange(700)
            };

            await svc.SendToDiscordAsync(changes);

            // Expect batching into 2 payloads
            Assert.True(svc.SentPayloads.Count >= 1, "No payloads were sent.");
            Assert.Equal(2, svc.SentPayloads.Count);

            // Each payload must be <= 2000 characters (Discord limit)
            foreach (var p in svc.SentPayloads)
            {
                Assert.True(p.Length <= 2000, $"Payload exceeded limit: {p.Length}");
            }

            // Ensure all product names are present in the sent payloads
            var combined = string.Concat(svc.SentPayloads);
            Assert.Contains(new string('A', 700), combined);
        }

        [Fact]
        public async Task Chunking_DoesNotSplit_WhenMessagesWithinLimit()
        {
            var bnasOptions = Options.Create(new BnasDiscordSettings { BnasWebhookUrl = "http://bnas" });
            var gvlOptions = Options.Create(new GvlDiscordSettings { GvlWebhookUrl = "http://gvl" });

            var svc = new TestDiscordService(bnasOptions, gvlOptions);

            var changes = new List<ProductPriceChange>
            {
                MakeChange(50),
                MakeChange(50)
            };

            await svc.SendToDiscordAsync(changes);

            // Expect a single payload
            Assert.Single(svc.SentPayloads);
            Assert.True(svc.SentPayloads[0].Length <= 2000);
        }
    }
}
