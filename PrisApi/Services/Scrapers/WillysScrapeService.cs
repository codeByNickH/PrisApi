using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Data;
using PrisApi.Helper.IHelper;
using PrisApi.Mapper.IMapper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services.IService;

namespace PrisApi.Services.Scrapers
{
    public class WillysScrapeService
    {
        private readonly bool _isCloud;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly IScrapeConfigHelper _scraperConfig;
        private readonly ILogger<WillysScrapeService> _logger;
        public WillysScrapeService(IScrapeHelper scrapeHelper, IScrapeConfigHelper scrapeConfig, ILogger<WillysScrapeService> logger)
        {
            _logger = logger;
            _scrapeHelper = scrapeHelper;
            _scraperConfig = scrapeConfig;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

        }
        private async Task<ScraperConfig> GetConfig()
        {
            return await _scraperConfig.GetConfig(2);
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, Store storeConfig)
        {
            using var playwright = await Playwright.CreateAsync();
            var _config = await GetConfig();

            var options = new BrowserTypeLaunchOptions
            {
                Headless = true,
                SlowMo = _config.RequestDelayMs,
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
            else
            {

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
                    if (response.Url.Contains("se/c/") || response.Url.Contains("products"))
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

                                var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, storeConfig.Name.ToLower());

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
                    _logger.LogError($"Error processing response: {ex.Message}");
                }
            };


            try
            {
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                });

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CookieBannerSelector}]"); // Cookies
                await page.ClickAsync($"[{_config.ScraperSelector.RejectCookiesSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]"); // OpenChooseStore
                await page.ClickAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.PickupOptionSelector}]"); // StorePickupOption
                await page.ClickAsync($"[{_config.ScraperSelector.PickupOptionSelector}]");

                await page.WaitForSelectorAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]"); // SearchStore
                await page.FillAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]", storeConfig.StoreLocation.PostalCode.ToString());

                await page.WaitForSelectorAsync($"{_config.ScraperSelector.SelectStoreSelector}"); // SelectStore 
                await page.ClickAsync($"{_config.ScraperSelector.SelectStoreSelector}");

                await Task.Delay(500); // Picking store needs await to go through does not work otherwise.

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CloseChooseTabSelector}]"); // CloseChooseTab
                await page.ClickAsync($"[{_config.ScraperSelector.CloseChooseTabSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CategoryNavSelector}]"); // OpenCategoryNav
                await page.ClickAsync($"[{_config.ScraperSelector.CategoryNavSelector}]");

                await page.WaitForSelectorAsync($"a[href=\"{navigation}\"]");
                await page.ClickAsync($"a[href=\"{navigation}\"]");

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
                            _logger.LogInformation($"No height change for {maxNoChangeAttempts} attempts - assuming all content loaded");
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
                    _logger.LogInformation($"Scroll attempt {i + 1}/{maxScrollAttempts}, Products scraped: {products.Count}");
                }

                // if (apiResponses.Count > 0)
                // {
                //     File.WriteAllText("api_willys_responses_debug.json", string.Join("\n---\n", apiResponses));
                //     _logger.LogInformation("API responses saved to api_willys_responses_debug.json for debugging");
                // }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred during scraping: {ex.Message}");
                throw;
            }
        }
    }
}