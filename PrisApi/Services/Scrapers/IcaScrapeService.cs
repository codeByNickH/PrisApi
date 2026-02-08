using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Services.Scrapers
{
    public class IcaScrapeService
    {
        private readonly bool _isCloud;
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly ILogger<IcaScrapeService> _logger;
        public IcaScrapeService(IScrapeHelper scrapeHelper, IScrapeConfigHelper configHelper, ILogger<IcaScrapeService> logger)
        {
            _scrapeHelper = scrapeHelper;
            _configHelper = configHelper;
            _logger = logger;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;
        }
        private async Task<ScraperConfig> GetConfig()
        {
            return await _configHelper.GetConfig(1);
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, Store storeConfig)
        {
            using var playwright = await Playwright.CreateAsync();
            var _config = await GetConfig();

            var options = new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = _config.RequestDelayMs
            };

            if (_isCloud)
            {
                options.Args = new[]
                {
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-setuid-sandbox",
                    "--no-sandbox"
                };
            }

            await using var browser = await playwright.Chromium.LaunchAsync(options);
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var products = new List<ScrapedProduct>();
            var processedProductIds = new HashSet<string>();
            var apiResponses = new List<string>();

            page.Response += async (sender, response) =>
            {
                try
                {
                    if (response.Url.Contains("products"))
                    {
                        _logger.LogInformation($"API Response: {response.Url} - Status: {response.Status}");

                        if (response.Status == 200)
                        {
                            var contentType = response.Headers.ContainsKey("content-type")
                                ? response.Headers["content-type"]
                                : "";

                            if (contentType.Contains("application/json"))
                            {
                                var content = await response.TextAsync();
                                apiResponses.Add(content);

                                var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, storeConfig.Name.ToLower()?.Split(' ', 2)[0]);

                                foreach (var product in extractedProducts)
                                {
                                    if (!processedProductIds.Contains($"{product.RawName} {product.ProdCode}"))
                                    {
                                        product.StoreId = storeConfig.Id;
                                        products.Add(product);
                                        processedProductIds.Add($"{product.RawName} {product.ProdCode}");
                                        Console.WriteLine($"Extracted from API: {product?.RawBrand} {product?.RawName} {product?.Size}{product?.RawUnit} {product?.RawOrdPrice}kr {product?.RawDiscountPrice}kr {product?.RawDiscount}kr {product?.OrdJmfPrice}kr/{product?.RawUnit} {product?.DiscountJmfPrice}kr/{product?.RawUnit} {product?.DiscountPer}kr/{product?.RawUnit} {product?.MinQuantity} {product?.TotalPrice}kr {product?.MaxQuantity} {product?.MemberDiscount}");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing response: {Message}", ex.Message);
                }
            };

            try
            {
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CookieBannerSelector}]", new PageWaitForSelectorOptions { Timeout = 5000 }); // Cookies
                await page.ClickAsync($"[{_config.ScraperSelector.RejectCookiesSelector}]");

                await page.WaitForSelectorAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]"); // SearchStore
                await page.FillAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]", storeConfig.StoreLocation.PostalCode.ToString());

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.PickupOptionSelector}]"); // StorePickupOption
                await page.ClickAsync($"[{_config.ScraperSelector.PickupOptionSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.SelectStoreSelector}]");// SelectStore
                await page.ClickAsync($"[{_config.ScraperSelector.SelectStoreSelector}]");

                await Task.Delay(500);

                await page.WaitForSelectorAsync($"{_config.ScraperSelector.CategoryNavSelector}"); // OpenCategoryNav
                await page.ClickAsync($"{_config.ScraperSelector.CategoryNavSelector}");

                await page.WaitForSelectorAsync($"[data-test=\"{navigation}\"]");
                await page.ClickAsync($"[data-test=\"{navigation}\"]");

                await page.WaitForSelectorAsync("[data-first=\"nav-pane-1\"]");
                await page.ClickAsync("[data-first=\"nav-pane-1\"]");

                const int maxScrollAttempts = 4;
                int previousHeight = 0;
                int noChangeCount = 0;
                const int maxNoChangeAttempts = 3;

                for (int i = 0; i < maxScrollAttempts; i++)
                {
                    var currentHeight = await page.EvaluateAsync<int>("document.documentElement.scrollHeight");

                    if (currentHeight == previousHeight)
                    {
                        noChangeCount++;
                        if (noChangeCount >= maxNoChangeAttempts)
                        {
                            _logger.LogInformation($"No height change for {maxNoChangeAttempts} attempts - all content likely loaded");
                            break;
                        }
                    }
                    else
                    {
                        noChangeCount = 0;
                    }

                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                    await Task.Delay(10000);

                    previousHeight = currentHeight;
                    _logger.LogInformation($"Scroll attempt {i + 1}/{maxScrollAttempts}, Products found via API: {products.Count}");
                }

                if (products.Count == 0)
                {
                    _logger.LogInformation("No products found via API monitoring.");
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred during scraping: {Message}", ex.Message);
                throw;
            }
        }
    }
}