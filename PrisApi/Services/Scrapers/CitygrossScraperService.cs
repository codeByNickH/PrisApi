using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.Scrapers
{
    public class CitygrossScraperService
    {
        private readonly ScraperConfig _config;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly bool _isCloud;
        private const string StoreName = "citygross";
        public CitygrossScraperService(IScrapeHelper scrapeHelper, ScraperConfig config = null)
        {
            _scrapeHelper = scrapeHelper;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreName = StoreName,
                BaseUrl = "https://www.citygross.se/",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, int location, string category)
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
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync("[id=\"CybotCookiebotDialog\"]");
                await page.ClickAsync("[id=\"CybotCookiebotDialogBodyButtonDecline\"]");

                await page.WaitForSelectorAsync("[class=\"c-change-delivery-link\"]");
                await page.ClickAsync("[class=\"c-change-delivery-link\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Sök butik eller stad\"]");
                await page.FillAsync("input[placeholder=\"Sök butik eller stad\"]", location.ToString());

                await page.WaitForSelectorAsync("//*[@id='root']/div[2]/div/div/div/div[2]/div/div[4]/div");
                await page.ClickAsync("//*[@id='root']/div[2]/div/div/div/div[2]/div/div[4]/div");

                await page.WaitForSelectorAsync("//*[@id='root']/div[2]/div/div/div/div[2]/div/div[5]/button");
                await page.ClickAsync("//*[@id='root']/div[2]/div/div/div/div[2]/div/div[5]/button");

                await page.WaitForSelectorAsync("a[href=\"/matvaror\"]");
                await page.ClickAsync("a[href=\"/matvaror\"]");
                await Task.Delay(500);

                page.Response += async (sender, response) =>
                {
                    try
                    {
                        if (response.Url.Contains("products?") && response.Url.Contains("%"))
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

                                    var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, StoreName, category);

                                    foreach (var product in extractedProducts)
                                    {
                                        if (!processedProductIds.Contains($"{product.RawName} {product.ID}"))
                                        {
                                            products.Add(product);
                                            processedProductIds.Add($"{product.RawName} {product.ID}");
                                            Console.WriteLine($"Extracted from API: {product?.RawBrand} {product?.RawName} {product?.Size}{product?.RawUnit} {product?.RawOrdPrice}kr {product?.RawDiscountPrice}kr {product?.RawDiscount}kr {product?.OrdJmfPrice}kr/{product?.RawUnit} {product?.DiscountJmfPrice}kr/{product?.RawUnit} {product?.DiscountPer}kr/{product?.RawUnit} {product?.MinQuantity} {product?.TotalPrice}kr {product?.MaxQuantity} {product?.MemberDiscount}");
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

                await page.WaitForSelectorAsync($"a[href=\"{navigation}\"]");
                await page.ClickAsync($"a[href=\"{navigation}\"]");
                await Task.Delay(500);

                int nextPage = 3;
                int currentPage = 2;
                const int maxLoadMoreAttempts = 100;

                for (int i = 0; i < maxLoadMoreAttempts; i++)
                {
                    try
                    {
                        await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                        var buttonSelector = $"//*[@id='b-main']/div/div/div[4]/div/div/div/div[{nextPage}]";
                        var buttonExists = await page.QuerySelectorAsync($"xpath={buttonSelector}") != null;

                        if (buttonExists)
                        {
                            await page.ClickAsync($"xpath={buttonSelector}");
                            Console.WriteLine($"Successfully clicked page {currentPage} button using XPath ({i + 1}/{maxLoadMoreAttempts}). Products found: {products.Count}");
                            nextPage++;
                            currentPage++;
                            await Task.Delay(500);
                        }
                        else if (nextPage <= 1)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Page {currentPage} button not found, pagination might be complete. Products found: {products.Count}");
                            nextPage -= 11;
                            currentPage -= 1;
                            Console.WriteLine($"<||| {nextPage} |||>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error clicking page {currentPage}: {ex.Message}");
                        break;
                    }
                }

                Console.WriteLine($"Total products scraped: {products.Count}");

                if (apiResponses.Count > 0)
                {
                    File.WriteAllText("api_citygross_responses_debug.json", string.Join("\n---\n", apiResponses));
                    Console.WriteLine("API responses saved to api_citygross_responses_debug.json for debugging");
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