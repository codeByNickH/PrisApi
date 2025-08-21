using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.Scrapers
{
    public class HemkopScraperService
    {
        private readonly ScraperConfig _config;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly bool _isCloud;
        private const string StoreName = "hemkop";
        private string ProductListSelector = "[data-testid=\"product\"]";
        public HemkopScraperService(IScrapeHelper scrapeHelper, ScraperConfig config = null)
        {
            _scrapeHelper = scrapeHelper;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreName = StoreName,
                BaseUrl = "https://www.hemkop.se/",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }

        public async Task<List<ScrapedProduct>> ScrapeDiscountProductsAsync()
        {
            _config.BaseUrl = "erbjudanden";


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
                        StoreName = StoreName,
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
                        Console.WriteLine($"{product?.RawBrand} {product.RawName} {product?.RawUnit} {product?.RawOrdPrice} {product?.RawDiscount} {product.MaxQuantity}");
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
        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string navigation, int location, int category)
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


            page.Response += async (sender, response) =>
            {
                try
                {
                    if (response.Url.Contains("se/c/") || response.Url.Contains("products"))
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
                                        Console.WriteLine($"Extracted from API: {product.RawBrand} {product.RawName} {product.Size}{product.RawUnit} {product?.RawOrdPrice}kr {product?.RawDiscountPrice}kr {product?.RawDiscount}kr {product?.OrdJmfPrice}kr/{product?.RawUnit} {product?.DiscountJmfPrice}kr/{product?.RawUnit} {product?.DiscountPer}kr/{product?.RawUnit} {product.MinQuantity} {product?.TotalPrice}kr {product?.MaxQuantity} {product.MemberDiscount}");
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
                await page.GotoAsync(_config.BaseUrl + navigation, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                // Does not have separate data per location ?

                // await page.WaitForSelectorAsync("//*[@id='header']/div/div[1]/menu/li[5]/a"); // 
                // await page.ClickAsync("//*[@id='header']/div/div[1]/menu/li[5]/a");

                // await page.WaitForSelectorAsync("input[placeholder=\"Sök efter butik, butiksadress eller ort\"]");
                // await page.FillAsync("input[placeholder=\"Sök efter butik, butiksadress eller ort\"]", "alft"); 

                // await page.WaitForSelectorAsync("//*[@id='__next']/div[2]/div[2]/div[3]/div/div[2]/ul/li[1]"); 
                // await page.ClickAsync("//*[@id='__next']/div[2]/div[2]/div[3]/div/div[2]/ul/li[1]");

                await Task.Delay(500);

                // await page.WaitForSelectorAsync("//*[@id='__next']/div[2]/div[4]/div[2]/div[1]/div[3]/div[1]/div[2]/div/div[4]/div/div/div/div[1]/div[2]/div/div/a/button");
                // await page.ClickAsync("//*[@id='__next']/div[2]/div[4]/div[2]/div[1]/div[3]/div[1]/div[2]/div/div[4]/div/div/div/div[1]/div[2]/div/div/a/button");

                // await page.WaitForSelectorAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");
                // await page.ClickAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");

                // await page.WaitForSelectorAsync($"a[href=\"{category}\"]");
                // await page.ClickAsync($"a[href=\"{category}\"]");

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

                if (apiResponses.Count > 0)
                {
                    File.WriteAllText("api_hemkop_responses_debug.json", string.Join("\n---\n", apiResponses));
                    Console.WriteLine("API responses saved to api_hemkop_responses_debug.json for debugging");
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