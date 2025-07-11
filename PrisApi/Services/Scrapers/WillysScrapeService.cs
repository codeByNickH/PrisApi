using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Services.Scrapers
{
    public class WillysScrapeService
    {
        private readonly ScraperConfig _config;
        private readonly IScrapeHelper _scrapeHelper;
        private readonly bool _isCloud;
        private const string StoreId = "willys";

        public WillysScrapeService(IScrapeHelper scrapeHelper, ScraperConfig config = null)
        {
            _scrapeHelper = scrapeHelper;

            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreId = StoreId,
                BaseUrl = "https://www.willys.se/",
                ProductListSelector = "[data-testid=\"product\"]",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }

        public async Task<List<ScrapedProduct>> ScrapeDiscountProductsAsync(int location)
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

            try
            {
                await page.GotoAsync(_config.BaseUrl + "erbjudanden/butik", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                await page.WaitForSelectorAsync("[class=\"sc-8db9fd1a-0 haTzby\"]");
                await page.ClickAsync("[class=\"sc-59a4afd4-0 fTQtwW sc-59bd60e8-1 fnKnyn\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Sök efter din butik\"]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.FillAsync("input[placeholder=\"Sök efter din butik\"]", location.ToString());

                await page.WaitForSelectorAsync("[data-testid=\"pickup-location-list-item\"]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.ClickAsync("[data-testid=\"pickup-location-list-item\"]");

                // Close Login pop-up
                // await page.WaitForSelectorAsync("[class=\"sc-56561d8a-5 eQuJvz\"]");
                // await page.ClickAsync("[data-testid=\"modal-close-btn\"]");

                var loadMoreButtonSelector = "[data-testid=\"load-more-btn\"]";
                const int maxLoadMoreAttempts = 10;

                for (int i = 0; i < maxLoadMoreAttempts; i++)
                {
                    try
                    {
                        await page.WaitForSelectorAsync(loadMoreButtonSelector, new PageWaitForSelectorOptions { Timeout = 5000 });
                        await page.ClickAsync(loadMoreButtonSelector);

                        Console.WriteLine($"Successfully clicked \"load more\" ({i + 1}/{maxLoadMoreAttempts})");
                        await Task.Delay(500); // Give time for content to load
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No more \"load more\" button found or error after {i} clicks: {ex.Message}");
                        break;
                    }
                }

                var articleElements = await page.QuerySelectorAllAsync(_config.ProductListSelector);

                foreach (var element in articleElements)
                {
                    var product = new ScrapedProduct
                    {
                        StoreId = StoreId,
                        ScrapedAt = DateTime.UtcNow
                    };

                    var nameElement = await element.QuerySelectorAsync("[itemprop=\"name\"]");
                    if (nameElement != null)
                    {
                        product.RawName = await nameElement.TextContentAsync() ?? string.Empty;
                        product.RawName = product.RawName.Trim();
                    }

                    var priceElement1 = await element.QuerySelectorAsync("[class=\"sc-6467c3d8-14 bwnHuF\"]");
                    var priceElement2 = await element.QuerySelectorAsync("[class=\"sc-4b8cc2f9-2 cCZiOx\"]");
                    var priceElement3 = await element.QuerySelectorAsync("[class=\"sc-4b8cc2f9-5 ggAScU\"]");
                    var priceElement4 = await element.QuerySelectorAsync("[class=\"sc-4b8cc2f9-6 jxQDEl\"]");

                    var price1 = priceElement1 != null ? await priceElement1.TextContentAsync() : null;
                    var price2 = priceElement2 != null ? await priceElement2.TextContentAsync() : null;
                    var price3 = priceElement3 != null ? await priceElement3.TextContentAsync() : null;
                    var price4 = priceElement4 != null ? await priceElement4.TextContentAsync() : null;

                    if (!string.IsNullOrEmpty(price2))
                    {
                        if (!price2.EndsWith("."))
                            price2 += ".";

                        price2 += string.Join("", price3);
                    }

                    var saveElement = await element.QuerySelectorAsync("[class=\"sc-6467c3d8-15 iFyTse\"]");
                    var maxQElement = await element.QuerySelectorAsync("[class=\"sc-6467c3d8-16 HoYMj\"]");

                    var save = saveElement != null ? await saveElement.TextContentAsync() : null;
                    var maxQ = maxQElement != null ? await maxQElement.TextContentAsync() : null;
                    if (maxQ != null)
                    {
                        product.MaxQuantity = maxQ;
                    }
                    else
                    {
                        product.MaxQuantity = "Inget max antal";
                    }

                    var memberElement = await element.QuerySelectorAsync("[class=\"sc-e20bc8d3-1 bdMExn sc-4b8cc2f9-7 chdLmu\"]");
                    var memberDiscount = memberElement != null ? await memberElement.TextContentAsync() : null;
                    if (memberDiscount != null)
                    {
                        product.MemberDiscount = true;
                    }

                    var savingElement = await element.QuerySelectorAsync("[class=\"sc-6467c3d8-15 iFyTse\"]");
                    if (savingElement != null)
                    {
                        var rawDiscount = await savingElement.TextContentAsync() ?? string.Empty;
                        product.RawDiscount = rawDiscount.Trim();
                    }

                    product.RawOrdPrice = string.Join(" ", new[] { price1, price2, price4 }
                        .Where(p => !string.IsNullOrEmpty(p))
                        .Select(p => p.Trim()));

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

                    if (!string.IsNullOrEmpty(product.RawName))
                    {
                        products.Add(product);
                        Console.WriteLine(product.RawBrand.ToString() + " " + product.RawName.ToString() + " " + product.RawUnit.ToString() + " " + product.RawOrdPrice.ToString() + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString() + " " + product.MemberDiscount.ToString());
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

                                var extractedProducts = await _scrapeHelper.ExtractProductsFromJson(content, StoreId);

                                foreach (var product in extractedProducts)
                                {
                                    if (!processedProductIds.Contains($"{product.RawName} {product.ID}"))
                                    {
                                        products.Add(product);
                                        processedProductIds.Add($"{product.RawName} {product.ID}");
                                        Console.WriteLine($"Extracted from API: {product.RawBrand} {product.RawName} {product?.RawOrdPrice} {product?.RawUnit} {product?.OrdJmfPrice} {product?.RawDiscount} {product?.RawDiscountPrice} {product?.DiscountJmfPrice} {product?.MaxQuantity} {product.MemberDiscount}");
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
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                });

                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                await page.WaitForSelectorAsync("[data-testid=\"delivery-picker-toggle\"]");
                await page.ClickAsync("[data-testid=\"delivery-picker-toggle\"]");

                await page.WaitForSelectorAsync("[data-testid=\"delivery-method-pickUpInStore\"]");
                await page.ClickAsync("[data-testid=\"delivery-method-pickUpInStore\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Sök på butik\"]");
                await page.FillAsync("input[placeholder=\"Sök på butik\"]", location.ToString()); 

                await page.WaitForSelectorAsync("[class=\"sc-4b41f1b4-2 cIxYss\"]");
                await page.ClickAsync("[class=\"sc-4b41f1b4-2 cIxYss\"]");

                await Task.Delay(500); // Picking store needs await to go through does not work otherwise.

                // await page.WaitForLoadStateAsync(LoadState.NetworkIdle); // Why does this not work?

                await page.WaitForSelectorAsync("[data-testid=\"slidein-close-button\"]");
                await page.ClickAsync("[data-testid=\"slidein-close-button\"]");

                await page.WaitForSelectorAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");
                await page.ClickAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");

                await page.WaitForSelectorAsync($"a[href=\"{category}\"]");
                await page.ClickAsync($"a[href=\"{category}\"]");

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
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Products scraped: {products.Count}");
                }

                Console.WriteLine($"Total products scraped: {products.Count}");

                if (apiResponses.Count > 0)
                {
                    File.WriteAllText("api_willys_responses_debug.json", string.Join("\n---\n", apiResponses));
                    Console.WriteLine("API responses saved to api_willys_responses_debug.json for debugging");
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