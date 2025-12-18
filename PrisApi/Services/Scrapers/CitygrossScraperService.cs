using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;

namespace PrisApi.Services.Scrapers
{
    public class CitygrossScraperService
    {
        private readonly IScrapeHelper _scrapeHelper;
        private readonly IScrapeConfigHelper _scraperConfig;
        private readonly bool _isCloud;
        public CitygrossScraperService(IScrapeHelper scrapeHelper, IScrapeConfigHelper scrapeConfig)
        {
            _scrapeHelper = scrapeHelper;
            _scraperConfig = scrapeConfig;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

        }
        private async Task<ScraperConfig> GetConfig()
        {
            return await _scraperConfig.GetConfig(4);
        }
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, Store storeConfig)
        {
            using var playwright = await Playwright.CreateAsync();
            var _config = await GetConfig();
            
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
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CookieBannerSelector}]");
                await page.ClickAsync($"[{_config.ScraperSelector.RejectCookiesSelector}]");

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]");
                await page.ClickAsync($"[{_config.ScraperSelector.ChooseStoreSelector}]");

                await page.WaitForSelectorAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]");
                await page.FillAsync($"input[{_config.ScraperSelector.SearchStoreSelector}]", storeConfig.StoreLocation.PostalCode.ToString());

                await page.WaitForSelectorAsync($"{_config.ScraperSelector.SelectStoreSelector}");
                await page.ClickAsync($"{_config.ScraperSelector.SelectStoreSelector}");

                await page.WaitForSelectorAsync($"{_config.ScraperSelector.CloseChooseTabSelector}");
                await page.ClickAsync($"{_config.ScraperSelector.CloseChooseTabSelector}");

                await page.WaitForSelectorAsync($"a[{_config.ScraperSelector.CategoryNavSelector}]");
                await page.ClickAsync($"a[{_config.ScraperSelector.CategoryNavSelector}]");
                await Task.Delay(500);

                page.Response += async (sender, response) =>
                {
                    try
                    {
                        if (response.Url.Contains("products?"))
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
                        Console.WriteLine($"Error processing response: {ex.Message}");
                    }
                };

                await page.WaitForSelectorAsync($"a[href=\"{navigation}\"]");
                await page.ClickAsync($"a[href=\"{navigation}\"]");
                await Task.Delay(500);

                int nextPage = 3;
                int currentPage = 2;
                const int maxLoadMoreAttempts = 10;

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
                            await Task.Delay(10000);
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
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error clicking page {currentPage}: {ex.Message}");
                        break;
                    }
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