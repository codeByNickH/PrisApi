using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Services.Scrapers
{
    public class CoopScrapeService
    {
        private readonly IScrapeHelper _scrapeHelper;
        private readonly IScrapeConfigHelper _scraperConfig;
        private readonly ILogger<CoopScrapeService> _logger;
        private readonly bool _isCloud;
        public CoopScrapeService(IScrapeHelper scrapeHelper, IScrapeConfigHelper scrapeConfig, ILogger<CoopScrapeService> logger)
        {
            _scrapeHelper = scrapeHelper;
            _scraperConfig = scrapeConfig;
            _logger = logger;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

        }
        private async Task<ScraperConfig> GetConfig()
        {
            return await _scraperConfig.GetConfig(3);
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, Store storeConfig)
        {
            using var playwright = await Playwright.CreateAsync();
            var _config = await GetConfig();
            var name = storeConfig.Name.ToLower()?.Split(' ', 2)[0] == "stora" ? storeConfig.Name.ToLower()?.Split(' ', 2)[1] : storeConfig.Name.ToLower();

            var options = new BrowserTypeLaunchOptions
            {
                Headless = true,
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
            else
            {

            }

            await using var browser = await playwright.Chromium.LaunchAsync(options);
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var products = new List<ScrapedProduct>();
            var processedProductIds = new HashSet<string>();
            var apiResponses = new List<string>();



            try
            {
                await page.GotoAsync(_config.BaseUrl + navigation, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CookieBannerSelector}]"); // CookieBanner
                await page.ClickAsync($"[{_config.ScraperSelector.RejectCookiesSelector}]"); // RejectCookies

                await Task.Delay(500);

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]"); // OpenChooseStore
                await page.ClickAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]");

                await page.WaitForSelectorAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]"); // SearchStore
                await page.FillAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]", storeConfig.StoreLocation.PostalCode.ToString());
                await Task.Delay(3000);

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.SearchButtonSelector}]"); // SearchButton
                await page.ClickAsync($"[{_config.ScraperSelector.SearchButtonSelector}]");

                try
                {
                    await page.WaitForSelectorAsync($"[{_config.ScraperSelector.PickupOptionSelector}]", new PageWaitForSelectorOptions { Timeout = 2000 }); // StorePickupOption
                    await page.ClickAsync($"[{_config.ScraperSelector.PickupOptionSelector}]", new PageClickOptions { Timeout = 500 });
                }
                catch
                {
                    _logger.LogInformation("No home delivery option detected, continuing..");
                }

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.SelectStoreSelector}]"); // SelectStore
                await page.ClickAsync($"[{_config.ScraperSelector.SelectStoreSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CloseChooseTabSelector}]"); // CloseChooseTab
                await page.ClickAsync($"[{_config.ScraperSelector.CloseChooseTabSelector}]");


                page.Response += async (sender, response) =>
                {
                    try
                    {
                        if (response.Url.Contains("graphql") || response.Url.Contains("by-attribute"))
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

                                    var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, name);

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
                int j = 2;
                const int maxLoadMoreAttempts = 4;

                for (int i = 0; i < maxLoadMoreAttempts; i++)
                {
                    try
                    {
                        var loadMoreButtonSelector = $"a[href*=\"page={j}\"]";

                        await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                        var buttonExists = await page.QuerySelectorAsync(loadMoreButtonSelector) != null;

                        if (buttonExists)
                        {
                            await Task.Delay(10000);
                            await page.ClickAsync(loadMoreButtonSelector, new PageClickOptions { Force = true });
                            _logger.LogInformation($"Successfully clicked \"load more\" ({i + 1}/{maxLoadMoreAttempts})");
                            j++;
                        }
                        else
                        {
                            _logger.LogInformation($"Page {i + 1} button not found, pagination might be complete");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "No more \"load more\" button found or error after {Attempts} clicks: {Message}", i, ex.Message);
                        break;
                    }
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