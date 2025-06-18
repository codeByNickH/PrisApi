using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.Scrapers
{
    public class IcaScrapeService
    {
        private readonly ScraperConfig _config;
        private readonly bool _isCloud;
        private const string StoreId = "ica";

        public IcaScrapeService(ScraperConfig config = null)
        {
            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreId = StoreId,
                BaseUrl = "https://handlaprivatkund.ica.se/stores/1051012/categories/",
                ProductListSelector = "[data-promotion-list-name=\"erbjudanden\"]",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
        }

        public async Task<List<ScrapedProduct>> ScrapeDiscountProductsAsync()
        {
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

                    // Scroll to bottom
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                    await Task.Delay(500); // Wait for content to load

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                }

                await Task.Delay(500);

                // Extract product data
                var articleElements = await page.QuerySelectorAllAsync(_config.ProductListSelector);
                System.Console.WriteLine(articleElements.Count.ToString());
                foreach (var element in articleElements)
                {
                    var product = new ScrapedProduct
                    {
                        StoreId = StoreId,
                        ScrapedAt = DateTime.UtcNow
                    };

                    // Get product title
                    var nameElement = await element.QuerySelectorAsync("[class=\"offer-card__title\"]");
                    if (nameElement != null)
                    {
                        product.RawName = await nameElement.TextContentAsync() ?? string.Empty;
                        product.RawName = product.RawName.Trim();
                    }


                    // Add if "stammis price"
                    // Get price
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

                    product.RawOrdPrice = string.Join(" ", new[] { price1, price2, price4 }
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
                        Console.WriteLine(product?.RawBrand?.ToString() + " " + product.RawName.ToString() + " " + product?.RawUnit?.ToString() + " " + product?.RawOrdPrice?.ToString() + " " + product?.RawDiscount?.ToString() + " " + product.MaxQuantity.ToString());
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
            _config.BaseUrl += category;

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

            await using var browser = await playwright.Chromium.LaunchAsync(options);
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var products = new List<ScrapedProduct>();
            var processedProductIds = new HashSet<string>();
            var apiResponses = new List<string>();
            var firstApiCallDetected = false;

            // Set up network monitoring
            page.Response += async (sender, response) =>
            {
                try
                {
                    // Log all API calls to help identify the right endpoint
                    if (response.Url.Contains("product"))
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

                                // Log first 500 chars to identify structure
                                // Console.WriteLine($"Response preview: {content.Substring(0, Math.Min(500, content.Length))}...");

                                // Try to parse products from the response
                                List<ScrapedProduct> extractedProducts = null;
                                if (response.Url.Contains("products"))
                                {
                                    firstApiCallDetected = true;
                                    extractedProducts = ExtractProductsFromJson(content);
                                }
                                foreach (var product in extractedProducts)
                                {
                                    if (!processedProductIds.Contains(product.RawName))
                                    {
                                        products.Add(product);
                                        processedProductIds.Add(product.RawName);
                                        Console.WriteLine($"Extracted from API: {product.RawBrand} {product.RawName} {product?.RawOrdPrice} {product?.RawUnit} {product?.OrdJmfPrice} spar:{product?.RawDiscount} {product?.RawDiscountPrice} {product?.DiscountJmfPrice} {product?.MaxQuantity} {product.MemberDiscount}");
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
                // Navigate to the page
                await page.GotoAsync(_config.BaseUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                // Reject cookies
                await page.WaitForSelectorAsync("[id=\"onetrust-banner-sdk\"]", new PageWaitForSelectorOptions { Timeout = 5000 });
                await page.ClickAsync("[id=\"onetrust-reject-all-handler\"]");
                await Task.Delay(500);

                Console.WriteLine("Checking for initially rendered products...");
                await CaptureInitialDomProducts(page, products, processedProductIds);

                // Slow scroll to capture remaining pre-rendered products until API kicks in
                Console.WriteLine("Starting slow scroll to capture pre-rendered products...");
                var viewportHeight = await page.EvaluateAsync<int>("window.innerHeight");
                var scrollIncrement = viewportHeight / 2;
                var lastProductCount = products.Count;
                var scrollAttempts = 0;

                while (!firstApiCallDetected || scrollAttempts < 20) // Max 20 scroll attempts before API
                {
                    // Scroll down gradually
                    await page.EvaluateAsync($"window.scrollBy(0, {scrollIncrement})");
                    await Task.Delay(500); // Wait for any animations/rendering

                    // Capture newly visible products
                    await CaptureDomProductsInViewport(page, products, processedProductIds);

                    // Check if we're still finding new products
                    if (products.Count > lastProductCount)
                    {
                        Console.WriteLine($"Slow scroll {scrollAttempts + 1}: Found {products.Count - lastProductCount} new products (Total: {products.Count})");
                        lastProductCount = products.Count;
                    }
                    else
                    {
                        Console.WriteLine($"Slow scroll {scrollAttempts + 1}: No new products found");
                    }

                    scrollAttempts++;

                    // Check if we've reached the point where API calls would start
                    var currentScrollPosition = await page.EvaluateAsync<int>("window.pageYOffset");
                    var documentHeight = await page.EvaluateAsync<int>("document.documentElement.scrollHeight");

                    // If we're more than 70% down the page and no API yet, break
                    if (currentScrollPosition > documentHeight * 0.7 || lastProductCount > 70)
                    {
                        Console.WriteLine("Reached 50% of page without API calls, continuing with regular scroll");
                        break;
                    }
                }

                Console.WriteLine($"Captured {products.Count} products before API calls started");
                // Wait a moment to catch any initial API calls that might have been triggered
                await Task.Delay(2000);
                Console.WriteLine($"After initial wait: {products.Count} products captured");



                // Scroll to trigger all API calls
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

                    // Scroll down
                    await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                    // Wait for API responses
                    await Task.Delay(1500);

                    previousHeight = currentHeight;
                    Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Products found via API: {products.Count}");
                }

                // If no products found via API, fall back to DOM scraping
                if (products.Count == 0)
                {
                    Console.WriteLine("No products found via API monitoring. Attempting DOM scraping...");
                    products = await FallbackDomScraping(page);
                }

                Console.WriteLine($"Total products scraped: {products.Count}");

                // Save API responses for debugging
                if (apiResponses.Count > 0)
                {
                    File.WriteAllText("api_responses_debug.json", string.Join("\n---\n", apiResponses));
                    Console.WriteLine("API responses saved to api_responses_debug.json for debugging");
                }

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred during scraping: {ex.Message}");
                throw;
            }
        }

        private async Task CaptureInitialDomProducts(IPage page, List<ScrapedProduct> products, HashSet<string> processedProductIds)
        {
            try
            {
                // Wait a bit for any dynamic content to load
                await Task.Delay(1000);

                // First, check for any embedded JSON data in script tags or data attributes
                var scriptData = await page.EvaluateAsync<string>(@"
            () => {
                
                const scripts = document.querySelectorAll('script[type=""application/ld+json""], script[type=""application/json""]');
                const results = [];
                scripts.forEach(script => {
                    try {
                        const data = JSON.parse(script.textContent);
                        results.push(JSON.stringify(data));
                    } catch (e) {}
                });
                
                
                const elements = document.querySelectorAll('[data-products], [data-initial-state], [data-props]');
                elements.forEach(el => {
                    const attrs = el.attributes;
                    for (let attr of attrs) {
                        if (attr.name.startsWith('data-') && attr.value.includes('{')) {
                            results.push(attr.value);
                        }
                    }
                });
                
                return results.join('\n---\n');
            }
        ");

                if (!string.IsNullOrEmpty(scriptData))
                {
                    Console.WriteLine("Found embedded JSON data, attempting to extract products...");
                    var parts = scriptData.Split(new[] { "\n---\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        var extractedProducts = ExtractProductsFromJson(part);
                        foreach (var product in extractedProducts)
                        {
                            if (!processedProductIds.Contains(product.RawName))
                            {
                                products.Add(product);
                                processedProductIds.Add(product.RawName);
                                Console.WriteLine($"Extracted from embedded data: {product.RawBrand} {product.RawName} {product.RawOrdPrice}");
                            }
                        }
                    }
                }

                // Get all initially visible product elements
                var articleElements = await page.QuerySelectorAllAsync("[class=\"sc-filq44-0 csjtZi\"]");
                Console.WriteLine($"Found {articleElements.Count} initial product elements in DOM");

                foreach (var element in articleElements)
                {
                    try
                    {
                        // Check if this is a real product (not a skeleton)
                        var nameElement = await element.QuerySelectorAsync("[tabindex=\"0\"]");
                        if (nameElement == null) continue;

                        var productName = await nameElement.TextContentAsync();
                        if (string.IsNullOrEmpty(productName) || productName.Contains("...")) continue;

                        // Skip if already processed
                        if (processedProductIds.Contains(productName.Trim())) continue;

                        var product = new ScrapedProduct
                        {
                            StoreId = StoreId,
                            ScrapedAt = DateTime.UtcNow,
                            RawName = productName.Trim()
                        };

                        // Get price elements
                        var priceElement1 = await element.QuerySelectorAsync("[class=\"mutation-modified ord-price\"]");
                        var priceElement2 = await element.QuerySelectorAsync("[data-test=\"fop-price\"]");
                        var priceElement4 = await element.QuerySelectorAsync("[data-test=\"fop-price-per-unit\"]");

                        // var priceElement3 = await element.QuerySelectorAsync("[class=\"price-splash__text__prefix\"]");
                        // var price3 = priceElement3 != null ? await priceElement3.TextContentAsync() : null;

                        var price1 = priceElement1 != null ? await priceElement1.TextContentAsync() : null;
                        var price2 = priceElement2 != null ? await priceElement2.TextContentAsync() : null;
                        var price4 = priceElement4 != null ? await priceElement4.TextContentAsync() : null;

                        if (!string.IsNullOrEmpty(price2))
                        {
                            if (!price2.EndsWith(".") && !price2.EndsWith(":-"))
                                price2 += ".";

                            // price2 += string.Join("", price3);
                        }

                        product.RawOrdPrice = string.Join(" ", new[] { price2, price1, price4 }
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
                                    product.RawUnit = parts[1].TrimEnd(' ').Trim();
                                }
                            }
                        }

                        // Get image
                        var imageElement = await element.QuerySelectorAsync("[itemprop=\"image\"]");
                        if (imageElement != null)
                        {
                            product.ImageSrc = await imageElement.GetAttributeAsync("src") ?? string.Empty;
                        }

                        // Get max quantity
                        var maxQuantityElement = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-15 NqJbq\"]");
                        product.MaxQuantity = maxQuantityElement != null
                            ? await maxQuantityElement.TextContentAsync()
                            : "Inget max antal";

                        // Add product if valid
                        if (!string.IsNullOrEmpty(product.RawName) && !string.IsNullOrEmpty(product.RawOrdPrice))
                        {
                            products.Add(product);
                            processedProductIds.Add(product.RawName);
                            Console.WriteLine($"Captured initial DOM product: {product.RawBrand} {product.RawName} {product.RawOrdPrice}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing initial DOM product: {ex.Message}");
                    }
                }

                Console.WriteLine($"Captured {products.Count} products from initial DOM");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CaptureInitialDomProducts: {ex.Message}");
            }
        }
        private async Task CaptureDomProductsInViewport(IPage page, List<ScrapedProduct> products, HashSet<string> processedProductIds)
        {
            try
            {
                // Use the selector to get elements
                await Task.Delay(500);
                var articleElements = await page.QuerySelectorAllAsync("[class=\"sc-filq44-0 csjtZi\"]");

                // Filter to only visible ones using JavaScript
                foreach (var element in articleElements)
                {
                    try
                    {

                        // Check if this is a real product (not a skeleton)
                        var nameElement = await element.QuerySelectorAsync("[tabindex=\"0\"]");
                        if (nameElement == null) continue;

                        var productName = await nameElement.TextContentAsync();
                        if (string.IsNullOrEmpty(productName) || productName.Contains("...")) continue;

                        // Skip if already processed
                        if (processedProductIds.Contains(productName.Trim())) continue;

                        var product = new ScrapedProduct
                        {
                            StoreId = StoreId,
                            ScrapedAt = DateTime.UtcNow,
                            RawName = productName.Trim()
                        };

                        // Get price elements
                        var priceElement1 = await element.QuerySelectorAsync("[class=\"mutation-modified ord-price\"]");
                        var priceElement2 = await element.QuerySelectorAsync("[data-test=\"fop-price\"]");
                        var priceElement4 = await element.QuerySelectorAsync("[data-test=\"fop-price-per-unit\"]");

                        // var priceElement3 = await element.QuerySelectorAsync("[class=\"price-splash__text__prefix\"]");
                        // var price3 = priceElement3 != null ? await priceElement3.TextContentAsync() : null;

                        var price1 = priceElement1 != null ? await priceElement1.TextContentAsync() : null;
                        var price2 = priceElement2 != null ? await priceElement2.TextContentAsync() : null;
                        var price4 = priceElement4 != null ? await priceElement4.TextContentAsync() : null;

                        if (!string.IsNullOrEmpty(price2))
                        {
                            if (!price2.EndsWith(".") && !price2.EndsWith(":-"))
                                price2 += ".";

                            // price2 += string.Join("", price3);
                        }

                        product.RawOrdPrice = string.Join(" ", new[] { price2, price1, price4 }
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
                                    product.RawUnit = parts[1].TrimEnd(' ').Trim();
                                }
                            }
                        }

                        // Get image
                        var imageElement = await element.QuerySelectorAsync("[itemprop=\"image\"]");
                        if (imageElement != null)
                        {
                            product.ImageSrc = await imageElement.GetAttributeAsync("src") ?? string.Empty;
                        }

                        // Get max quantity
                        var maxQuantityElement = await element.QuerySelectorAsync("[class=\"sc-57d5cc93-15 NqJbq\"]");
                        product.MaxQuantity = maxQuantityElement != null
                            ? await maxQuantityElement.TextContentAsync()
                            : "Inget max antal";

                        // Add product if valid
                        if (!string.IsNullOrEmpty(product.RawName) && !string.IsNullOrEmpty(product.RawOrdPrice))
                        {
                            products.Add(product);
                            processedProductIds.Add(product.RawName);
                            Console.WriteLine($"Captured viewport product: {product.RawBrand} {product.RawName} {product.RawOrdPrice}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing viewport product: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CaptureDomProductsInViewport: {ex.Message}");
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
                product.RawName = GetStringProperty(element, "name", "productName", "title", "displayName");
                product.RawBrand = GetStringProperty(element, "brand", "brandName", "manufacturer");

                string priceStr = null;
                if (element.TryGetProperty("price", out var price) && price.ValueKind == JsonValueKind.Object)
                {
                    priceStr = GetStringProperty(price, "amount");
                }

                string unitStr = null;
                string unitPriceStr = null;
                string unitDisplay = null;
                if (element.TryGetProperty("unitPrice", out var unitPrice) && unitPrice.ValueKind == JsonValueKind.Object)
                {
                    unitStr = GetStringProperty(unitPrice, "unit");
                    if (unitPrice.TryGetProperty("price", out var priceInnerObj) && priceInnerObj.ValueKind == JsonValueKind.Object)
                    {
                        unitPriceStr = GetStringProperty(priceInnerObj, "amount");
                    }
                }

                if (!string.IsNullOrEmpty(priceStr))
                {
                    product.RawOrdPrice = priceStr;
                    if (!string.IsNullOrEmpty(unitPriceStr) && !string.IsNullOrEmpty(unitStr))
                    {
                        unitDisplay = unitStr.Replace("without.liquid", " utan spad").Replace("litre.without.deposit", "L").Replace("fop.price.per.", "").Replace("each", "st");
                        product.OrdJmfPrice = $"{unitPriceStr}kr/{unitDisplay}";
                    }
                }

                // Extract discount price
                string promoPriceStr = null;
                if (element.TryGetProperty("promoPrice", out var promoPrice) && promoPrice.ValueKind == JsonValueKind.Object)
                {
                    promoPriceStr = GetStringProperty(promoPrice, "amount");
                }

                string promoUnitPriceStr = null;
                string promoUnitStr = null;
                if (element.TryGetProperty("promoUnitPrice", out var promoUnitPrice) && promoUnitPrice.ValueKind == JsonValueKind.Object)
                {
                    promoUnitStr = GetStringProperty(promoUnitPrice, "unit");
                    if (promoUnitPrice.TryGetProperty("price", out var promoPriceInnerObj) && promoPriceInnerObj.ValueKind == JsonValueKind.Object)
                    {
                        promoUnitPriceStr = GetStringProperty(promoPriceInnerObj, "amount");
                    }
                }

                if (!string.IsNullOrEmpty(promoPriceStr))
                {
                    product.RawDiscountPrice = promoPriceStr;
                    if (!string.IsNullOrEmpty(promoUnitPriceStr) && !string.IsNullOrEmpty(promoUnitStr))
                    {
                        unitDisplay = promoUnitStr.Replace("without.liquid", " utan spad").Replace("litre.without.deposit", "L").Replace("fop.price.per.", "").Replace("each", "st");
                        product.DiscountJmfPrice = $"{promoUnitPriceStr}kr/{unitDisplay}";
                    }
                }

                if (!string.IsNullOrEmpty(priceStr) && !string.IsNullOrEmpty(promoPriceStr))
                {
                    decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ordPrice);
                    decimal.TryParse(promoPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal discPrice);
                    var saved = (ordPrice - discPrice).ToString();
                    product.RawDiscount = saved;
                    if (element.TryGetProperty("promotions", out var promotions) && promotions.ValueKind == JsonValueKind.Array && promotions.GetArrayLength() > 0)
                    {
                        var promoMaxQ = promotions[0];
                        promoPriceStr = GetStringProperty(promoMaxQ, "description");
                        if (promoPriceStr.Contains("Stammispris"))
                        {
                            product.MemberDiscount = true;
                        }
                        if (promoPriceStr.Contains("Max"))
                        {
                            var parts = promoPriceStr.Split("--", 3);
                            product.MaxQuantity = parts[1].Replace(", Stammispris", "");
                        }
                    }
                }

                if (string.IsNullOrEmpty(promoPriceStr))
                {
                    if (element.TryGetProperty("promotions", out var promotions) && promotions.ValueKind == JsonValueKind.Array && promotions.GetArrayLength() > 0)
                    {
                        var firstPromo = promotions[0];
                        promoPriceStr = GetStringProperty(firstPromo, "description");
                        if (promoPriceStr.Contains("Stammispris"))
                        {
                            product.MemberDiscount = true;
                        }
                        if (promoPriceStr.Contains("Max"))
                        {
                            var parts1 = promoPriceStr.Split("--", 3);
                            product.MaxQuantity = parts1[1].Replace(", Stammispris", "");
                        }
                        if (promoPriceStr.Contains("för"))
                        {
                            decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ordPrice);
                            var parts = promoPriceStr.Split(" ", 4);
                            var j = Int32.Parse(parts[0]);
                            var i = Int32.Parse(parts[2]);
                            var saved = i / j;
                            product.RawDiscount = (ordPrice - saved).ToString();
                            unitDisplay = "kr/st";
                        }
                        var value = promoPriceStr.Split("-- ", 2);
                        product.RawDiscountPrice = value[0];
                    }
                }
                product.RawUnit = unitDisplay;

                // Extract image
                product.ImageSrc = GetStringProperty(element, "image", "imageUrl", "imageSrc", "productImage");

                // Extract max quantity
                // var maxQty = GetStringProperty(element, "maxQuantity", "maxOrderQuantity", "limitPerCustomer");
                // product.MaxQuantity = string.IsNullOrEmpty(maxQty) ? "Inget max antal" : maxQty;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product from JSON element: {ex.Message}");
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

        private async Task<List<ScrapedProduct>> FallbackDomScraping(IPage page)
        {
            var products = new List<ScrapedProduct>();

            // Scroll back to top
            await page.EvaluateAsync("window.scrollTo(0, 0)");
            await Task.Delay(1000);

            // Get all visible products
            var articleElements = await page.QuerySelectorAllAsync(_config.ProductListSelector);

            foreach (var element in articleElements)
            {
                try
                {
                    var product = new ScrapedProduct
                    {
                        StoreId = StoreId,
                        ScrapedAt = DateTime.UtcNow
                    };

                    // Extract product details as in your original code
                    var nameElement = await element.QuerySelectorAsync("[tabindex=\"0\"]");
                    if (nameElement != null)
                    {
                        product.RawName = await nameElement.TextContentAsync() ?? string.Empty;
                        product.RawName = product.RawName.Trim();

                        if (!string.IsNullOrEmpty(product.RawName) && !product.RawName.Contains("..."))
                        {
                            // Continue with rest of extraction...
                            // (Include the rest of your original extraction logic here)
                            products.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in fallback DOM scraping: {ex.Message}");
                }
            }

            return products;
        }

    }
}