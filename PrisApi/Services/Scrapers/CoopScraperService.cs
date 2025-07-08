using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.Scrapers
{
    public class CoopScraperService
    {
        private readonly ScraperConfig _config;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly bool _isCloud;
        private const string StoreId = "coop";

        public CoopScraperService(IScrapeHelper scrapeHelper, ScraperConfig config = null)
        {
            _scrapeHelper = scrapeHelper;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreId = StoreId,
                BaseUrl = "https://www.coop.se/handla/varor/",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string category, int location)
        {
            using var playwright = await Playwright.CreateAsync();

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
                await page.GotoAsync(_config.BaseUrl + category, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync("[id=\"cmpbox\"]");
                await page.ClickAsync("[id=\"cmpwelcomebtnno\"]");

                await Task.Delay(500);

                await page.WaitForSelectorAsync("[class=\"eJadUlKd\"]");
                await page.ClickAsync("[class=\"eJadUlKd\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Ange ditt postnummer\"]");
                await page.FillAsync("input[placeholder=\"Ange ditt postnummer\"]", location.ToString());

                await page.WaitForSelectorAsync("[class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"]");
                await page.ClickAsync("[class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"]");

                try
                {
                    await page.WaitForSelectorAsync("[data-key=\"pickup\"]", new PageWaitForSelectorOptions { Timeout = 2000 });
                    await page.ClickAsync("[data-key=\"pickup\"]", new PageClickOptions { Timeout = 500 });
                }
                catch
                {
                    Console.WriteLine("No home delivery option detected, continuing..");
                }

                await page.WaitForSelectorAsync("[class=\"yWvaV7fj\"]");
                await page.ClickAsync("[class=\"yWvaV7fj\"]");

                await page.WaitForSelectorAsync("[class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"]");
                await page.ClickAsync("[class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"]");


                page.Response += async (sender, response) =>
                {
                    try
                    {
                        if (response.Url.Contains("graphql") || response.Url.Contains("by-attribute"))
                        {
                            Console.WriteLine($"API Response: {response.Url} - Status: {response.Status}");

                            if (response.Status == 200)
                            {
                                var contentType = response.Headers.ContainsKey("content-type")
                                    ? response.Headers["content-type"]
                                    : "";

                                if (contentType.Contains("application/json"))
                                {

                                    var content = await response.TextAsync();
                                    apiResponses.Add(content);

                                    var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, StoreId);

                                    foreach (var product in extractedProducts)
                                    {
                                        if (!processedProductIds.Contains($"{product.RawName} {product?.ID}"))
                                        {
                                            products.Add(product);
                                            processedProductIds.Add($"{product.RawName} {product?.ID}");
                                            Console.WriteLine($"Extracted from API: {product.RawBrand} {product.RawName} {product?.RawOrdPrice} {product?.RawUnit} {product?.OrdJmfPrice} {product?.RawDiscount} {product?.RawDiscountPrice} {product?.DiscountJmfPrice} {product.DiscountPer} {product?.MaxQuantity} {product.MemberDiscount}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing response: {ex.Message}");
                    }
                };
                int j = 2;
                const int maxLoadMoreAttempts = 30;

                for (int i = 0; i < maxLoadMoreAttempts; i++)
                {
                    try
                    {
                        var loadMoreButtonSelector = $"a[href*=\"page={j}\"]";

                        await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                        var buttonExists = await page.QuerySelectorAsync(loadMoreButtonSelector) != null;

                        if (buttonExists)
                        {
                            await Task.Delay(500);
                            await page.ClickAsync(loadMoreButtonSelector, new PageClickOptions { Force = true });
                            Console.WriteLine($"Successfully clicked \"load more\" ({i + 1}/{maxLoadMoreAttempts})");
                            j++;
                            await Task.Delay(500);
                        }
                        else
                        {
                            Console.WriteLine($"Page {i + 1} button not found, pagination might be complete");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No more \"load more\" button found or error after {i} clicks: {ex.Message}");
                        break;
                    }
                }


                Console.WriteLine($"Total products scraped: {products.Count}");

                if (apiResponses.Count > 0)
                {
                    File.WriteAllText("api_coop_responses_debug.json", string.Join("\n---\n", apiResponses));
                    Console.WriteLine("API responses saved to api_coop_responses_debug.json for debugging");
                }

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred during scraping: {ex.Message}");
                throw;
            }
        }
    }
}