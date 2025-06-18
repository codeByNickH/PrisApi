using System.Text.Json;
using Microsoft.Playwright;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.Scrapers
{
    public class CoopScraperService
    {
        private readonly ScraperConfig _config;
        private readonly bool _isCloud;
        private const string StoreId = "coop";

        public CoopScraperService(ScraperConfig config = null)
        {
            _isCloud = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;

            _config = config ?? new ScraperConfig
            {
                StoreId = StoreId,
                BaseUrl = "https://www.coop.se/handla/varor/",
                RequestDelayMs = 50,
                UseJavaScript = true
            };
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
                    // https://external.api.coop.se/personalization/search/entities/by-attribute?api-version=v1&store=205140&groups=CUSTOMER_PRIVATE&device=mobile&direct=false
                    if (response.Url.Contains("api") || response.Url.Contains("by-attribute"))
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
                                    if (!processedProductIds.Contains($"{product.RawName} {product?.ID}"))
                                    {
                                        products.Add(product);
                                        processedProductIds.Add($"{product.RawName} {product?.ID}");
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

                await page.WaitForSelectorAsync("[id=\"cmpbox\"]");
                await page.ClickAsync("[id=\"cmpwelcomebtnno\"]");

                await Task.Delay(500);

                await page.WaitForSelectorAsync("[class=\"eJadUlKd\"]");
                await page.ClickAsync("[class=\"eJadUlKd\"]");

                await page.WaitForSelectorAsync("input[placeholder=\"Ange ditt postnummer\"]");
                await page.FillAsync("input[placeholder=\"Ange ditt postnummer\"]", "82330");

                await page.WaitForSelectorAsync("[class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"]");
                await page.ClickAsync("[class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"]");

                await page.WaitForSelectorAsync("[class=\"yWvaV7fj\"]");
                await page.ClickAsync("[class=\"yWvaV7fj\"]");

                await page.WaitForSelectorAsync("[class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"]");
                await page.ClickAsync("[class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"]");


                // const int maxScrollAttempts = 2;
                // int previousHeight = 0;
                // int noChangeCount = 0;
                // const int maxNoChangeAttempts = 3;

                // for (int i = 0; i < maxScrollAttempts; i++)
                // {
                //     var currentHeight = await page.EvaluateAsync<int>("document.documentElement.scrollHeight");

                //     if (currentHeight == previousHeight)
                //     {
                //         noChangeCount++;
                //         if (noChangeCount >= maxNoChangeAttempts)
                //         {
                //             Console.WriteLine($"No height change for {maxNoChangeAttempts} attempts - assuming all content loaded");
                //             break;
                //         }
                //     }
                //     else
                //     {
                //         noChangeCount = 0;
                //     }

                //     // Scroll to bottom
                //     await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");
                //     await Task.Delay(500); // Wait for content to load

                //     // Next page button class=Kr1thI9n, nav class=Pk_thyR9, div class=u-marginTmd

                //     previousHeight = currentHeight;
                //     Console.WriteLine($"Scroll attempt {i + 1}/{maxScrollAttempts}, Height: {currentHeight}");
                // }

                int j = 2;
                const int maxLoadMoreAttempts = 11;

                for (int i = 0; i < maxLoadMoreAttempts; i++)
                {
                    try
                    {
                        var loadMoreButtonSelector = $"a[href*=\"page={j}\"]";

                        await page.EvaluateAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

                        await page.WaitForSelectorAsync(loadMoreButtonSelector, new PageWaitForSelectorOptions { Timeout = 5000 });
                        await Task.Delay(1000); // Wait for content to load
                        await page.ClickAsync(loadMoreButtonSelector, new PageClickOptions { Force = true });

                        Console.WriteLine($"Successfully clicked \"load more\" ({i + 1}/{maxLoadMoreAttempts})");
                        await Task.Delay(500); // Give time for content to load
                        j++;
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

                if (root.TryGetProperty("results", out var results) &&
                results.ValueKind == JsonValueKind.Object &&
                results.TryGetProperty("items", out var items) &&
                items.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in items.EnumerateArray())
                    {
                        var product = ExtractProductFromElement(item);
                        if (product != null && !string.IsNullOrEmpty(product.RawName))
                        {
                            products.Add(product);
                        }
                    }

                    Console.WriteLine($"Extracted {products.Count} products from Coop results.items structure");
                    return products;
                }
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
                product.RawBrand = GetStringProperty(element, "manufacturerName");

                // Extract unit/volume
                product.RawUnit = GetStringProperty(element, "packageSizeInformation");

                // Extract image URL
                if (element.TryGetProperty("image", out var imageObj) && imageObj.ValueKind == JsonValueKind.Object)
                {
                    product.ImageSrc = GetStringProperty(imageObj, "url");
                }

                // var price = double.Parse(GetStringProperty(element, "promotionPrice"));
                // product.RawDiscountPrice = price.ToString();

                // if (string.IsNullOrEmpty(product.RawDiscountPrice))
                // {
                // }
                    product.RawOrdPrice = GetStringProperty(element, "salesPrice");

                // Get compare price if needed
                var ordComparePrice = GetStringProperty(element, "comparativePrice");
                var comparePriceUnit = GetStringProperty(element, "comparativePriceText");
                if (!string.IsNullOrEmpty(ordComparePrice) && !string.IsNullOrEmpty(comparePriceUnit))
                {
                    product.OrdJmfPrice = $"{ordComparePrice}/{comparePriceUnit}";
                }

                // Check if there are active promotions
                if (element.TryGetProperty("onlinePromotions", out var promotions) &&
                    promotions.ValueKind == JsonValueKind.Array &&
                    promotions.GetArrayLength() > 0)
                {
                    var firstPromo = promotions[0];

                    // Get promotional price if available
                    // if (firstPromo.TryGetProperty("price", out var promoPrice) &&
                    //     promoPrice.ValueKind == JsonValueKind.Object)
                    // {
                    //     // Add qualifyingCount somewhere if count > 0
                    // }
                    product.RawDiscountPrice = GetStringProperty(firstPromo, "price");

                    product.DiscountJmfPrice = GetStringProperty(firstPromo, "");

                    // Get discount/savings info
                    var conditionLabel = GetStringProperty(firstPromo, "message");
                    var rewardLabel = GetStringProperty(firstPromo, "numberOfProductRequired");

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
                var priceUnit = GetStringProperty(element, "salesUnit");
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

                product.ID = GetStringProperty(element, "id");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product from Coop JSON element: {ex.Message}");
                return null;
            }

            return product;
        }
        public List<JsonElement> FindProductElements(JsonElement root)
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

        public void SearchForProductArrays(JsonElement element, List<JsonElement> products)
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
        public string GetStringProperty(JsonElement element, params string[] propertyNames)
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