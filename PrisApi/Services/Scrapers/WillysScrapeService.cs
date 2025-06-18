using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Models.Scraping;
using PrisApi.Services.IService;

namespace PrisApi.Services.Scrapers
{
    public class WillysScrapeService
    {
        private readonly ScraperConfig _config;
        private readonly bool _isCloud;
        private const string StoreId = "willys";

        public WillysScrapeService(ScraperConfig config = null)
        {
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

        public async Task<List<ScrapedProduct>> ScrapeDiscountProductsAsync(string store)
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

                // Reject cookies
                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                // Choose store in Bollnäs
                await page.WaitForSelectorAsync("[class=\"sc-8db9fd1a-0 haTzby\"]");
                await page.ClickAsync("[class=\"sc-59a4afd4-0 fTQtwW sc-59bd60e8-1 fnKnyn\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Sök efter din butik\"]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.FillAsync("input[placeholder=\"Sök efter din butik\"]", store);

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

                    // Get discount info
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

                    // Get if member discount
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

                    // Add product to list
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

        public async Task<List<ScrapedProduct>> ScrapeMeatProductsAsync()
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

                // Open side menu
                await page.WaitForSelectorAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");
                await page.ClickAsync("[class=\"sc-59a4afd4-0 dsLRNa sc-5cf2ead7-0 hikgeL\"]");

                // Meat:
                await page.WaitForSelectorAsync("[class=\"sc-176d809f-0 fBiIEk\"]");
                await page.ClickAsync("#__next > div.sc-6d99c2f6-0.dWJGnD.__className_5fe6d7.__className_b41963.__className_d7eab4.__className_1ef194 > div.sc-6d99c2f6-3.dIINno > nav > div.sc-6f029054-1.bbThxH > ul > li:nth-child(1) > div > button.sc-176d809f-2.faXvIH > a");

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

                    // Scroll to bottom
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(1000); // Wait for content to load

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                }

                await Task.Delay(500);
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

                    product.RawOrdPrice = string.Join(" ", new[] { price1, price2 }
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
                        Console.WriteLine(product.RawBrand.ToString() + " " + product.RawName.ToString() + " " + product.RawUnit.ToString() + " " + product.RawOrdPrice.ToString() + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString());
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
        public async Task<List<ScrapedProduct>> ScrapeDariyProductsAsync()
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

                // Open side menu
                await page.WaitForSelectorAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");
                await page.ClickAsync("[class=\"sc-59a4afd4-0 dsLRNa sc-5cf2ead7-0 hikgeL\"]");

                // Dairy:
                await page.WaitForSelectorAsync("[class=\"sc-176d809f-0 fBiIEk\"]");
                await page.ClickAsync("#__next > div.sc-6d99c2f6-0.dWJGnD.__className_5fe6d7.__className_b41963.__className_d7eab4.__className_1ef194 > div.sc-6d99c2f6-3.dIINno > nav > div.sc-6f029054-1.bbThxH > ul > li:nth-child(3) > div > button.sc-176d809f-2.faXvIH > a");

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

                    // Scroll to bottom
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(500); // Wait for content to load

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                }

                await Task.Delay(500);
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

                    product.RawOrdPrice = string.Join(" ", new[] { price1, price2 }
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
                        Console.WriteLine(product.RawBrand.ToString() + " " + product.RawName.ToString() + " " + product.RawUnit.ToString() + " " + product.RawOrdPrice.ToString() + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString());
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
        public async Task<List<ScrapedProduct>> ScrapeFruitProductsAsync()
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

                // Open side menu
                await page.WaitForSelectorAsync("[class=\"sc-5cf2ead7-2 eTtqdC\"]");
                await page.ClickAsync("[class=\"sc-59a4afd4-0 dsLRNa sc-5cf2ead7-0 hikgeL\"]");

                // Fruit:
                await page.WaitForSelectorAsync("[class=\"sc-176d809f-0 fBiIEk\"]");
                await page.ClickAsync("#__next > div.sc-6d99c2f6-0.dWJGnD.__className_5fe6d7.__className_b41963.__className_d7eab4.__className_1ef194 > div.sc-6d99c2f6-3.dIINno > nav > div.sc-6f029054-1.bbThxH > ul > li:nth-child(2) > div > button.sc-176d809f-2.faXvIH > a");

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

                    // Scroll to bottom
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(1000); // Wait for content to load

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                }

                await Task.Delay(500);
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

                    product.RawOrdPrice = string.Join(" ", new[] { price1, price2 }
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
                        Console.WriteLine(product?.RawBrand?.ToString() + " " + product.RawName.ToString() + " " + product?.RawUnit?.ToString() + " " + product.RawOrdPrice.ToString() + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString());
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

        public async Task<List<ScrapedProduct>> ScrapeProductsAsync(string category)
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

                                var extractedProducts = ExtractProductsFromJson(content);

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
                await page.GotoAsync(_config.BaseUrl + category, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                // Reject cookies
                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]");
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");

                await Task.Delay(500);
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

                    // Scroll to bottom
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(500); // Wait for content to load

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
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
        private List<ScrapedProduct> ExtractProductsFromJson(string jsonContent)
        {
            var products = new List<ScrapedProduct>();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Try to parse as a generic JSON document
                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;

                // Look for products in common patterns
                var productElements = FindProductElements(root);

                foreach (var element in productElements)
                {
                    var product = ExtractProductFromElement(element);
                    if (product != null && !string.IsNullOrEmpty(product.RawName))
                    {
                        products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }

            return products;
        }

        private List<JsonElement> FindProductElements(JsonElement root)
        {
            var products = new List<JsonElement>();

            // Common patterns for finding products in JSON responses
            if (root.ValueKind == JsonValueKind.Object)
            {
                // Check for common product array property names
                string[] productArrayNames = { "products", "items", "data", "results", "productList", "articles", "promotion" };

                foreach (var propName in productArrayNames)
                {
                    if (root.TryGetProperty(propName, out var prop) && prop.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in prop.EnumerateArray())
                        {
                            products.Add(item);
                        }
                        break;
                    }
                }

                // If no direct array found, search recursively
                if (products.Count == 0)
                {
                    SearchForProductArrays(root, products);
                }
            }
            else if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in root.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        products.Add(item);
                    }
                }
            }

            return products;
        }

        private void SearchForProductArrays(JsonElement element, List<JsonElement> products)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in element.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Array)
                    {
                        // Check if this looks like a product array
                        var array = prop.Value;
                        if (array.GetArrayLength() > 0)
                        {
                            var firstItem = array[0];
                            if (IsLikelyProduct(firstItem))
                            {
                                foreach (var item in array.EnumerateArray())
                                {
                                    products.Add(item);
                                }
                            }
                        }
                    }
                    else if (prop.Value.ValueKind == JsonValueKind.Object)
                    {
                        SearchForProductArrays(prop.Value, products);
                    }
                }
            }
        }

        private bool IsLikelyProduct(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object) return false;

            // Check for common product properties
            string[] productProperties = { "name", "title", "productName", "price", "id", "sku", "productId" };
            int matchCount = 0;

            foreach (var prop in element.EnumerateObject())
            {
                if (productProperties.Any(p => prop.Name.ToLower().Contains(p.ToLower())))
                {
                    matchCount++;
                }
            }

            return matchCount >= 2;
        }
        private ScrapedProduct ExtractProductFromElement(JsonElement element)
        {
            var product = new ScrapedProduct
            {
                StoreId = StoreId,
                ScrapedAt = DateTime.UtcNow
            };

            try
            {
                // Extract name directly
                product.RawName = GetStringProperty(element, "name");

                // Extract brand/manufacturer
                product.RawBrand = GetStringProperty(element, "manufacturer");

                // Extract unit/volume
                product.RawUnit = GetStringProperty(element, "displayVolume");

                // Extract image URL
                if (element.TryGetProperty("image", out var imageObj) && imageObj.ValueKind == JsonValueKind.Object)
                {
                    product.ImageSrc = GetStringProperty(imageObj, "url");
                }

                product.RawOrdPrice = GetStringProperty(element, "priceNoUnit");

                // Get compare price if needed
                var ordComparePrice = GetStringProperty(element, "comparePrice");
                var comparePriceUnit = GetStringProperty(element, "comparePriceUnit");
                if (!string.IsNullOrEmpty(ordComparePrice) && !string.IsNullOrEmpty(comparePriceUnit))
                {
                    product.OrdJmfPrice = $"{ordComparePrice}/{comparePriceUnit}";
                }

                // Check if there are active promotions
                if (element.TryGetProperty("potentialPromotions", out var promotions) &&
                    promotions.ValueKind == JsonValueKind.Array &&
                    promotions.GetArrayLength() > 0)
                {
                    var firstPromo = promotions[0];

                    // Get promotional price if available
                    if (firstPromo.TryGetProperty("price", out var promoPrice) &&
                        promoPrice.ValueKind == JsonValueKind.Object)
                    {
                        product.RawDiscountPrice = GetStringProperty(promoPrice, "formattedValue");
                        // Add qualifyingCount somewhere if count > 0
                    }

                    product.DiscountJmfPrice = GetStringProperty(firstPromo, "comparePrice");

                    // Get discount/savings info
                    var conditionLabel = GetStringProperty(firstPromo, "conditionLabel");
                    var rewardLabel = GetStringProperty(firstPromo, "rewardLabel");

                    // Combine discount information
                    if (!string.IsNullOrEmpty(conditionLabel) && !conditionLabel.Contains("för"))
                    {
                        product.RawDiscount = conditionLabel;
                    }
                    else if (!string.IsNullOrEmpty(rewardLabel) && !string.IsNullOrEmpty(conditionLabel) && conditionLabel.Contains("för"))
                    {
                        string pricePer = $"{conditionLabel} {rewardLabel}";
                        var savings = GetStringProperty(element, "savingsAmount").ToString();
                        product.RawDiscount = $"{pricePer}, save: {savings}0";
                    }

                    // Get max quantity from redeemLimitLabel
                    var redeemLimit = GetStringProperty(firstPromo, "redeemLimitLabel");
                    if (!string.IsNullOrEmpty(redeemLimit))
                    {
                        product.MaxQuantity = redeemLimit;
                    }

                    // Get campaign type for member discount
                    var campaignType = GetStringProperty(firstPromo, "campaignType");
                    if (campaignType == "LOYALTY")
                    {
                        product.MemberDiscount = true;
                    }
                }

                // Get price unit info (kr/st, kr/kg, etc.)
                var priceUnit = GetStringProperty(element, "priceUnit");
                if (!string.IsNullOrEmpty(priceUnit) && !product.RawOrdPrice.Contains(priceUnit))
                {
                    product.RawOrdPrice += $" {priceUnit}";
                }

                // If brand not found in manufacturer, try extracting from productLine2
                if (string.IsNullOrEmpty(product.RawBrand))
                {
                    var productLine2 = GetStringProperty(element, "productLine2");
                    if (!string.IsNullOrEmpty(productLine2))
                    {
                        var parts = productLine2.Split(',');
                        if (parts.Length > 0)
                        {
                            product.RawBrand = parts[0].Trim();
                        }
                    }
                }

                // Default max quantity if not set
                if (string.IsNullOrEmpty(product.MaxQuantity))
                {
                    product.MaxQuantity = "Inget max antal";
                }

                product.ID = GetStringProperty(element, "code");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product from Willys JSON element: {ex.Message}");
                return null;
            }

            return product;
        }
        private string GetStringProperty(JsonElement element, params string[] propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                // Try exact match
                if (element.TryGetProperty(propName, out var prop))
                {
                    return prop.ToString();
                }

                // Try case-insensitive match
                foreach (var actualProp in element.EnumerateObject())
                {
                    if (actualProp.Name.Equals(propName, StringComparison.OrdinalIgnoreCase))
                    {
                        return actualProp.Value.ToString();
                    }
                }
            }

            return null;
        }
    }
}