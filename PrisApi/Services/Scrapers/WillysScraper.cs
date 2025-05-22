using Microsoft.Playwright;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Services.Scrapers
{
    public class WillysScraper
    {
        private readonly ScraperConfig _config;
        private readonly bool _isCloud;
        private const string StoreId = "willys";

        public WillysScraper(ScraperConfig config = null)
        {
            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreId = StoreId,
                BaseUrl = "https://www.willys.se/erbjudanden/butik",
                ProductListSelector = "[data-testid=\"product\"]",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }

        public async Task<List<ScrapedProduct>> ScrapeProductsAsync()
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
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                // Reject cookies
                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                // Choose store in Bollnäs
                await page.WaitForSelectorAsync("[class=\"sc-9d327e20-1 hqEwEa\"]");
                await page.ClickAsync("#promotionTabs > div:nth-child(1) > div > div.sc-8db9fd1a-2.gQdgZz > p.sc-3e8f26d7-0.kPRcnQ.sc-9d327e20-6.dECQaN > button");

                await page.WaitForSelectorAsync("input[placeholder=\"Sök efter din butik\"]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.FillAsync("input[placeholder=\"Sök efter din butik\"]", "82130");

                await page.WaitForSelectorAsync("[data-testid=\"pickup-location-list-item\"]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.ClickAsync("[data-testid=\"pickup-location-list-item\"]");

                // Close Login pop-up
                // await page.WaitForSelectorAsync("[class=\"sc-56561d8a-5 eQuJvz\"]");
                // await page.ClickAsync("[data-testid=\"modal-close-btn\"]");

                // Load whole page
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

                // Extract product data
                var articleElements = await page.QuerySelectorAllAsync(_config.ProductListSelector);

                foreach (var element in articleElements)
                {
                    var product = new ScrapedProduct
                    {
                        StoreId = StoreId,
                        ScrapedAt = DateTime.UtcNow
                    };

                    // Get product title
                    var nameElement = await element.QuerySelectorAsync("[itemprop=\"name\"]");
                    if (nameElement != null)
                    {
                        product.RawName = await nameElement.TextContentAsync() ?? string.Empty;
                        product.RawName = product.RawName.Trim();
                    }

                    // Get price
                    // whole price tag: sc-57d5cc93-11 huMxdW
                    // nr of items for price tag: sc-57d5cc93-13 hqMUdY
                    // first nr tag: sc-4b8cc2f9-2 cCZiOx
                    // last nr and st/kg tag: sc-4b8cc2f9-4 wIvZl
                    var priceElement1 = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-13 hqMUdY\"]");
                    var priceElement2 = await element.QuerySelectorAsync("[class=\"sc-4b8cc2f9-2 cCZiOx\"]");
                    var priceElement3 = await element.QuerySelectorAsync("[class=\"sc-4b8cc2f9-4 wIvZl\"]");

                    var price1 = priceElement1 != null ? await priceElement1.TextContentAsync() : null;
                    var price2 = priceElement2 != null ? await priceElement2.TextContentAsync() : null;
                    var price3 = priceElement3 != null ? await priceElement3.TextContentAsync() : null;

                    if (!string.IsNullOrEmpty(price2))
                    {
                        if (!price2.EndsWith("."))
                            price2 += ".";

                        price2 += string.Join("", price3);
                    }

                    product.RawPrice = string.Join(" ", new[] { price1, price2 }
                        .Where(p => !string.IsNullOrEmpty(p))
                        .Select(p => p.Trim()));

                    // Get discount info
                    var savingElement = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-14 kTSKTN\"]");
                    if (savingElement != null)
                    {
                        product.RawDiscount = await savingElement.TextContentAsync() ?? string.Empty;
                        product.RawDiscount = product.RawDiscount.Trim();
                    }

                    // Get brand/additional info
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

                    // Add product to list
                    if (!string.IsNullOrEmpty(product.RawName))
                    {
                        products.Add(product);
                        Console.WriteLine(product.RawBrand.ToString() + " " + product.RawName.ToString() + " " + product.RawUnit.ToString() + " " + product.RawPrice.ToString()  + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString());
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