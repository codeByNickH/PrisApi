using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
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
        private string ProductListSelector = "[data-promotion-list-name=\"erbjudanden\"]";
        public IcaScrapeService(IScrapeHelper scrapeHelper, IScrapeConfigHelper configHelper)
        {
            _scrapeHelper = scrapeHelper;
            _configHelper = configHelper;

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
                    Console.WriteLine($"Error processing response: {ex.Message}");
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

                await page.WaitForSelectorAsync($"[{_config.ScraperSelector.CategoryNavSelector}]"); // OpenCategoryNav
                await page.ClickAsync($"[{_config.ScraperSelector.CategoryNavSelector}]");

                await page.WaitForSelectorAsync($"[data-test=\"{navigation}\"]");
                await page.ClickAsync($"[data-test=\"{navigation}\"]");

                await page.WaitForSelectorAsync("[data-first=\"nav-pane-1\"]");
                await page.ClickAsync("[data-first=\"nav-pane-1\"]");

                const int maxScrollAttempts = 200;
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
                            Console.WriteLine($"No height change for {maxNoChangeAttempts} attempts - all content likely loaded");
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
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Products found via API: {products.Count}");
                }

                if (products.Count == 0)
                {
                    Console.WriteLine("No products found via API monitoring.");
                }

                // if (apiResponses.Count > 0)
                // {
                //     File.WriteAllText("api_responses_debug.json", string.Join("\n---\n", apiResponses));
                //     Console.WriteLine("API responses saved to api_ica_responses_debug.json for debugging");
                // }

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred during scraping: {ex.Message}");
                throw;
            }
        }
        public async Task<List<ScrapedProduct>> ScrapeDiscountProductsAsync()
        {
            var _config = await GetConfig();
            _config.BaseUrl = "https://www.ica.se/erbjudanden/maxi-ica-stormarknad-bollnas-1051012/";

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

            try
            {
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });
                // Reject cookies
                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                await Task.Delay(500);
                const int maxScrollAttempts = 100;
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
                            Console.WriteLine($"No height change for {maxNoChangeAttempts} attempts - assuming all content loaded");
                            break;
                        }
                    }
                    else
                    {
                        noChangeCount = 0;
                    }

                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(500);

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                }

                await Task.Delay(500);

                var articleElements = await page.QuerySelectorAllAsync(ProductListSelector);
                System.Console.WriteLine(articleElements.Count.ToString());
                foreach (var element in articleElements)
                {
                    var product = new ScrapedProduct
                    {
                        // StoreName = _config.StoreName,
                        ScrapedAt = DateTime.UtcNow
                    };

                    var nameElement = await element.QuerySelectorAsync("[class=\"offer-card__title\"]");
                    if (nameElement != null)
                    {
                        product.RawName = await nameElement.TextContentAsync() ?? string.Empty;
                        product.RawName = product.RawName.Trim();
                    }

                    var priceElement1 = await element.QuerySelectorAsync("[class=\"price-splash__text__prefix\"]");
                    var priceElement2 = await element.QuerySelectorAsync("[class=\"price-splash__text__firstValue\"]");
                    var priceElement3 = await element.QuerySelectorAsync("[class=\"price-splash__text__secondaryValue\"]");
                    var priceElement4 = await element.QuerySelectorAsync("[class=\"price-splash__text__suffix\"]");

                    var price1 = priceElement1 != null ? await priceElement1.TextContentAsync() : null;
                    var price2 = priceElement2 != null ? await priceElement2.TextContentAsync() : null;
                    var price3 = priceElement3 != null ? await priceElement3.TextContentAsync() : null;
                    var price4 = priceElement4 != null ? await priceElement4.TextContentAsync() : null;

                    if (!string.IsNullOrEmpty(price2))
                    {
                        if (!price2.EndsWith(".") && !price2.EndsWith(":-"))
                            price2 += ".";

                        price2 += string.Join("", price3);
                    }

                    // product.RawOrdPrice = string.Join(" ", new[] { price1, price2, price4 }
                    //     .Where(p => !string.IsNullOrEmpty(p))
                    //     .Select(p => p.Trim()));

                    var savingElement = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-14 kTSKTN\"]");
                    if (savingElement != null)
                    {
                        // product.RawDiscount = await savingElement.TextContentAsync() ?? string.Empty;
                        // product.RawDiscount = product.RawDiscount.Trim();
                    }

                    var brandElement = await element.QuerySelectorAsync("[itemprop=\"brand\"]");
                    if (brandElement != null)
                    {
                        var brandText = await brandElement.TextContentAsync();
                        if (!string.IsNullOrEmpty(brandText))
                        {
                            var parts = brandText.Split(new[] { ' ' }, 2);

                            product.RawBrand = parts[0].Trim();

                            if (parts.Length > 1)
                            {
                                var unitPart = parts[1].TrimEnd(' ').Trim();
                                product.RawUnit = unitPart;
                            }
                        }
                    }

                    var imageElement = await element.QuerySelectorAsync("[itemprop=\"image\"]");
                    if (imageElement != null)
                    {
                        product.ImageSrc = await imageElement.GetAttributeAsync("src") ?? string.Empty;
                    }

                    var maxQuantityElement = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-15 NqJbq\"]");

                    var maxQuantity = maxQuantityElement != null ?
                        await maxQuantityElement.TextContentAsync() : "Inget max antal";

                    product.MaxQuantity = maxQuantity;

                    if (!string.IsNullOrEmpty(product.RawName))
                    {
                        products.Add(product);
                        Console.WriteLine(product?.RawBrand?.ToString() + " " + product.RawName.ToString() + " " + product?.RawUnit?.ToString() + " " + product?.RawOrdPrice.ToString() + " " + product?.RawDiscount.ToString() + " " + product.MaxQuantity.ToString());
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