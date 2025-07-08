using System.Globalization;
using System.Text.Json;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;

namespace PrisApi.Helper
{
    public class ScrapeHelper : IScrapeHelper
    {
        public async Task<List<ScrapedProduct>> ExtractProductsFromJson(string jsonContent, string storeId)
        {
            var products = new List<ScrapedProduct>();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;

                if (root.TryGetProperty("entities", out var entities) &&
                    entities.ValueKind == JsonValueKind.Object &&
                    entities.TryGetProperty("product", out var productDict) &&
                    productDict.ValueKind == JsonValueKind.Object)
                {
                    foreach (var productProp in productDict.EnumerateObject())
                    {
                        var productElement = productProp.Value;
                        var product = await ExtractProductFromElement(productElement, storeId);
                        if (product != null && !string.IsNullOrEmpty(product.RawName))
                        {
                            products.Add(product);
                        }
                    }

                    Console.WriteLine($"Extracted {products.Count} products from ICA entities.product structure");
                    return products;
                }
                if (root.TryGetProperty("results", out var results) &&
                results.ValueKind == JsonValueKind.Object &&
                results.TryGetProperty("items", out var items) &&
                items.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in items.EnumerateArray())
                    {
                        var product = await ExtractProductFromElement(item, storeId); // Call new ExtractCoopProd....() ?
                        if (product != null && !string.IsNullOrEmpty(product.RawName))
                        {
                            products.Add(product);
                        }
                    }

                    Console.WriteLine($"Extracted {products.Count} products from Coop results.items structure");
                    return products;
                }

                var productElements = FindProductElements(root);

                foreach (var element in productElements)
                {
                    var product = await ExtractProductFromElement(element, storeId);
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

        public List<JsonElement> FindProductElements(JsonElement root)
        {
            var products = new List<JsonElement>();

            if (root.ValueKind == JsonValueKind.Object)
            {
                string[] productArrayNames = { "entities", "products", "items", "data", "results", "product", "productList", "articles", "promotion" };

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

        public bool IsLikelyProduct(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object) return false;

            string[] productProperties = { "name", "title", "productName", "price", "id", "sku", "productId", "entities", "product" };
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

        // Add Regex
        public async Task<ScrapedProduct> ExtractProductFromElement(JsonElement element, string storeId) // Make one for each store and make call in FromJson() with switch on storeid?
        {
            var product = new ScrapedProduct
            {
                StoreId = storeId,
                ScrapedAt = DateTime.UtcNow
            };

            try
            {
                switch (storeId)
                {
                    case "ica":
                        product.RawName = GetStringProperty(element, "name", "productName", "title", "displayName");
                        product.RawBrand = GetStringProperty(element, "brand", "brandName", "manufacturer");

                        string priceStr = null;
                        string priceJmfStr = null;
                        if (element.TryGetProperty("price", out var price) && price.ValueKind == JsonValueKind.Object)
                        {
                            priceStr = GetStringProperty(price, "amount");
                            if (string.IsNullOrEmpty(priceStr))
                            {
                                if (price.TryGetProperty("current", out var current) && current.ValueKind == JsonValueKind.Object)
                                {
                                    priceStr = GetStringProperty(current, "amount");
                                }
                                if (price.TryGetProperty("unit", out var unit) && unit.ValueKind == JsonValueKind.Object)
                                {
                                    if (unit.TryGetProperty("current", out var currentUnit) && currentUnit.ValueKind == JsonValueKind.Object)
                                    {
                                        priceJmfStr = GetStringProperty(currentUnit, "amount");
                                    }
                                }
                            }
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

                        // "catchweight": {
                        // "typicalQuantity": {
                        //     "value": "0.925",
                        //     "uom": "KG"
                        // },

                        if (!string.IsNullOrEmpty(priceStr))
                        {
                            product.RawOrdPrice = priceStr;
                            if (!string.IsNullOrEmpty(priceJmfStr))
                            {
                                product.OrdJmfPrice = priceJmfStr;
                            }
                            if (!string.IsNullOrEmpty(unitPriceStr) && !string.IsNullOrEmpty(unitStr))
                            {
                                unitDisplay = unitStr.Replace("without.liquid", " utan spad").Replace("litre.without.deposit", "L").Replace("fop.price.per.", "").Replace("each", "st");
                                product.OrdJmfPrice = $"{unitPriceStr}kr/{unitDisplay}";
                            }
                        }

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
                            decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal icaOrdPrice);
                            decimal.TryParse(promoPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal discPrice);
                            var saved = (icaOrdPrice - discPrice).ToString();
                            product.RawDiscount = saved;
                            if (element.TryGetProperty("promotions", out var icaPromotions) && icaPromotions.ValueKind == JsonValueKind.Array && icaPromotions.GetArrayLength() > 0)
                            {
                                var promoMaxQ = icaPromotions[0];
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

                        if (string.IsNullOrEmpty(promoPriceStr))   // Change to use Regex
                        {
                            if (element.TryGetProperty("promotions", out var icaPromotions) && icaPromotions.ValueKind == JsonValueKind.Array && icaPromotions.GetArrayLength() > 0)
                            {
                                var firstPromo = icaPromotions[0];
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
                                    decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal icaOrdPrice);
                                    var parts = promoPriceStr.Split(" ", 4);
                                    var j = Int32.Parse(parts[0]);
                                    var i = Int32.Parse(parts[2]);
                                    var saved = i / j;
                                    product.RawDiscount = (icaOrdPrice - saved).ToString();
                                    unitDisplay = "kr/st";
                                }
                                var value = promoPriceStr.Split("-- ", 2);
                                product.RawDiscountPrice = value[0];
                            }
                            else if (element.TryGetProperty("offer", out var offer) && offer.ValueKind == JsonValueKind.Object)
                            {
                                promoPriceStr = GetStringProperty(offer, "description");
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
                                    decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal icaOrdPrice);
                                    var parts = promoPriceStr.Split(" ", 4);
                                    var j = Int32.Parse(parts[0]);
                                    var i = Int32.Parse(parts[2]);
                                    var saved = i / j;
                                    product.RawDiscount = (icaOrdPrice - saved).ToString(); //
                                    unitDisplay = "kr/st";
                                }
                                var value = promoPriceStr.Split("-- ", 2);
                                product.RawDiscountPrice = value[0];
                            }
                        }
                        product.RawUnit = unitDisplay;

                        product.ImageSrc = GetStringProperty(element, "image", "imageUrl", "imageSrc", "productImage");

                        break;

                    case "coop":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturerName");
                        product.RawUnit = GetStringProperty(element, "packageSizeInformation");

                        if (element.TryGetProperty("image", out var imageObj) && imageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(imageObj, "url");
                        }

                        var coopOrdPrice = GetNumProperty(element, "piecePrice");
                        product.RawOrdPrice = coopOrdPrice.ToString();

                        product.RawDiscountPrice = GetStringProperty(element, "promotionPrice");

                        var coopOrdComparePrice = GetNumProperty(element, "comparativePrice");
                        var coopComparePriceUnit = GetStringProperty(element, "comparativePriceText");
                        if (!string.IsNullOrEmpty(coopOrdComparePrice.ToString()) && !string.IsNullOrEmpty(coopComparePriceUnit))
                        {
                            product.OrdJmfPrice = coopOrdComparePrice.ToString();
                        }

                        if (element.TryGetProperty("onlinePromotions", out var coopPromotions) &&
                            coopPromotions.ValueKind == JsonValueKind.Array &&
                            coopPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = coopPromotions[0];

                            if (string.IsNullOrEmpty(product.RawDiscountPrice))
                            {
                                product.RawDiscountPrice = GetStringProperty(firstPromo, "price");
                            }

                            if (firstPromo.TryGetProperty("comparativePrice", out var discJmfPrice) &&
                                discJmfPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.DiscountJmfPrice = GetStringProperty(discJmfPrice, "b2cPrice");
                            }

                            var requiredAmount = GetStringProperty(firstPromo, "numberOfProductRequired");

                            double.TryParse(product.OrdJmfPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out var ordJmf);
                            double.TryParse(product.DiscountJmfPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out var discJmf);
                            double savingsPer = (double)coopOrdComparePrice - discJmf;
                            product.DiscountPer = $"spar ca: {Math.Round(savingsPer, 2)}{coopComparePriceUnit}";

                            double savings;
                            if (string.IsNullOrEmpty(requiredAmount))
                            {
                                double.TryParse(product.RawOrdPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out var a);
                                double.TryParse(product.RawDiscountPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out var b);
                                savings = (double)(coopOrdPrice - b);
                                product.RawDiscount = $"spar ca: {Math.Round(savings, 2)}kr/st";
                            }
                            else if (!string.IsNullOrEmpty(requiredAmount))
                            {
                                double.TryParse(product.RawDiscountPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out double a);
                                double.TryParse(requiredAmount, out double i);
                                var pricePer = a / i;
                                double.TryParse(product.RawOrdPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out double b);
                                savings = (double)((coopOrdPrice - pricePer) / i);
                                product.RawDiscount = $"spar ca: {Math.Round(savings, 2)}kr/st";
                            }

                            if (firstPromo.TryGetProperty("maxNumberOfUseWithUnit", out var redeemLimit) &&
                                redeemLimit.ValueKind == JsonValueKind.Object)
                            {
                                product.MaxQuantity = $"Max {GetStringProperty(redeemLimit, "value")} per hushåll";
                            }

                            var campaignType = GetStringProperty(firstPromo, "medMeraRequired");
                            if (campaignType == "True")
                            {
                                product.MemberDiscount = true;
                            }
                        }

                        product.RawOrdPrice += "kr/st";
                        product.RawDiscountPrice += "kr/st";

                        if (!string.IsNullOrEmpty(product.OrdJmfPrice))
                            product.OrdJmfPrice += coopComparePriceUnit;

                        if (!string.IsNullOrEmpty(product.DiscountJmfPrice))
                            product.DiscountJmfPrice += coopComparePriceUnit;

                        if (string.IsNullOrEmpty(product.MaxQuantity))
                        {
                            product.MaxQuantity = "Inget max antal";
                        }

                        product.ID = GetStringProperty(element, "id");

                        break;
                    case "hemkop":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturer");
                        product.RawUnit = GetStringProperty(element, "displayVolume");

                        if (element.TryGetProperty("image", out var hemkopImageObj) && hemkopImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(hemkopImageObj, "url");
                        }

                        product.RawOrdPrice = GetStringProperty(element, "priceNoUnit"); // chage to priceValue and GetNumProp()

                        var hemkopOrdComparePrice = GetStringProperty(element, "comparePrice");
                        var hemkopComparePriceUnit = GetStringProperty(element, "comparePriceUnit");
                        if (!string.IsNullOrEmpty(hemkopOrdComparePrice) && !string.IsNullOrEmpty(hemkopComparePriceUnit))
                        {
                            product.OrdJmfPrice = $"{hemkopOrdComparePrice}/{hemkopComparePriceUnit}";
                        }

                        if (element.TryGetProperty("potentialPromotions", out var hemkopPromotions) &&
                            hemkopPromotions.ValueKind == JsonValueKind.Array &&
                            hemkopPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = hemkopPromotions[0];

                            if (firstPromo.TryGetProperty("price", out var hemkopPromoPrice) &&
                                hemkopPromoPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.RawDiscountPrice = GetStringProperty(hemkopPromoPrice, "formattedValue");
                                // Add qualifyingCount somewhere if count > 0
                            }

                            product.DiscountJmfPrice = GetStringProperty(firstPromo, "comparePrice");

                            var conditionLabel = GetStringProperty(firstPromo, "conditionLabel");
                            var rewardLabel = GetStringProperty(firstPromo, "rewardLabel");

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

                            var redeemLimit = GetStringProperty(firstPromo, "redeemLimitLabel");
                            if (!string.IsNullOrEmpty(redeemLimit))
                            {
                                product.MaxQuantity = redeemLimit;
                            }

                            var campaignType = GetStringProperty(firstPromo, "campaignType");
                            if (campaignType == "LOYALTY")
                            {
                                product.MemberDiscount = true;
                            }
                        }

                        var hemkopPriceUnit = GetStringProperty(element, "priceUnit");
                        if (!string.IsNullOrEmpty(hemkopPriceUnit) && !product.RawOrdPrice.Contains(hemkopPriceUnit))
                        {
                            product.RawOrdPrice += $" {hemkopPriceUnit}";
                        }

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

                        if (string.IsNullOrEmpty(product.MaxQuantity))
                        {
                            product.MaxQuantity = "Inget max antal";
                        }

                        product.ID = GetStringProperty(element, "code");

                        break;
                    case "willys":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturer");
                        product.RawUnit = GetStringProperty(element, "displayVolume");

                        if (element.TryGetProperty("image", out var willysImageObj) && willysImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(willysImageObj, "url");
                        }

                        product.RawOrdPrice = GetStringProperty(element, "priceNoUnit"); // chage to priceValue

                        var willysOrdComparePrice = GetStringProperty(element, "comparePrice");
                        var willysComparePriceUnit = GetStringProperty(element, "comparePriceUnit");
                        if (!string.IsNullOrEmpty(willysOrdComparePrice) && !string.IsNullOrEmpty(willysComparePriceUnit))
                        {
                            product.OrdJmfPrice = $"{willysOrdComparePrice}/{willysComparePriceUnit}";
                        }

                        if (element.TryGetProperty("potentialPromotions", out var wilysPromotions) &&
                            wilysPromotions.ValueKind == JsonValueKind.Array &&
                            wilysPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = wilysPromotions[0];

                            if (firstPromo.TryGetProperty("price", out var willysPromoPrice) &&
                                willysPromoPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.RawDiscountPrice = GetStringProperty(willysPromoPrice, "formattedValue");
                                // Add qualifyingCount somewhere if count > 0
                            }

                            product.DiscountJmfPrice = GetStringProperty(firstPromo, "comparePrice");

                            var conditionLabel = GetStringProperty(firstPromo, "conditionLabel");
                            var rewardLabel = GetStringProperty(firstPromo, "rewardLabel");

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

                            var redeemLimit = GetStringProperty(firstPromo, "redeemLimitLabel");
                            if (!string.IsNullOrEmpty(redeemLimit))
                            {
                                product.MaxQuantity = redeemLimit;
                            }

                            var campaignType = GetStringProperty(firstPromo, "campaignType");
                            if (campaignType == "LOYALTY")
                            {
                                product.MemberDiscount = true;
                            }
                        }

                        var willysPriceUnit = GetStringProperty(element, "priceUnit");
                        if (!string.IsNullOrEmpty(willysPriceUnit) && !product.RawOrdPrice.Contains(willysPriceUnit))
                        {
                            product.RawOrdPrice += $" {willysPriceUnit}";
                        }

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

                        if (string.IsNullOrEmpty(product.MaxQuantity))
                        {
                            product.MaxQuantity = "Inget max antal";
                        }

                        product.ID = GetStringProperty(element, "code");
                        break;
                    case "citygross":
                        // countryOfManufacture, Add country of origin. Get from country tag and if null use OfManu.. instead.

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "brand");
                        product.RawUnit = GetStringProperty(element, "descriptiveSize");

                        if (element.TryGetProperty("image", out var citygrossImageObj) && citygrossImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(citygrossImageObj, "url");
                        }

                        // To get correct price per item there needs to be a check on weight
                        // then calculate price per item. As of now the price is per kg on some items
                        // which don't give the item price when the item is less or more then 1kg.

                        // "netContent": {
                        //     "unitOfMeasure": 0,
                        //     "value": 900
                        //      if value < 1000 make value 0.9-- else make value 1.--- 
                        // },   multiply by price to get price per item on price per kg items.
                        decimal weightInKg = 0;
                        if (element.TryGetProperty("netContent", out var weight) &&
                            weight.ValueKind == JsonValueKind.Object)
                        {
                            decimal prodWeight = decimal.Parse(GetStringProperty(weight, "value"));
                            weightInKg = prodWeight / 1000m;
                        }
                        if (element.TryGetProperty("productStoreDetails", out var details) &&
                            details.ValueKind == JsonValueKind.Object)
                        {
                            if (details.TryGetProperty("prices", out var prices) &&
                                prices.ValueKind == JsonValueKind.Object)
                            {
                                string comparePriceUnit = null;
                                if (prices.TryGetProperty("currentPrice", out var currentPrice) &&
                                    currentPrice.ValueKind == JsonValueKind.Object)
                                {
                                    string citygrossPriceStr = GetStringProperty(currentPrice, "price");
                                    string compPriceStr = GetStringProperty(currentPrice, "comparativePrice");
                                    decimal.TryParse(citygrossPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal citygrossPrice);
                                    decimal.TryParse(compPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ordComparePrice);
                                    if (citygrossPrice != ordComparePrice)
                                    {
                                        product.RawOrdPrice = citygrossPrice.ToString();
                                    }
                                    else
                                    {
                                        decimal pricePerItem = Math.Round(weightInKg * ordComparePrice, 2);
                                        product.RawOrdPrice = pricePerItem.ToString();
                                    }
                                    comparePriceUnit = GetStringProperty(currentPrice, "comparativePriceUnit").Replace("M", ""); // replace last letter
                                    if (ordComparePrice != 0 && !string.IsNullOrEmpty(comparePriceUnit))
                                    {
                                        product.OrdJmfPrice = ordComparePrice.ToString();
                                    }
                                }
                                if (prices.TryGetProperty("promotions", out var promotions) &&
                                    promotions.ValueKind == JsonValueKind.Array &&
                                    promotions.GetArrayLength() > 0)
                                {
                                    //  "promotions": [
                                    //     {
                                    //         "membersOnly": false,
                                    //         "minQuantity": 1,
                                    //         "value": 99.95,
                                    //         "maxAppliedPerReceipt": 0,
                                    //         "priceDetails": {
                                    //             "price": 99.95,
                                    //             "unit": "KGM",
                                    //             "comparativePrice": 99.95,
                                    //             "comparativePriceUnit": "KGM"
                                    //         }
                                    //     }
                                    // ], 
                                    var firstPromo = promotions[0];

                                    if (firstPromo.TryGetProperty("priceDetails", out var citygrossPromoPrice) &&
                                        citygrossPromoPrice.ValueKind == JsonValueKind.Object)
                                    {
                                        string citygrossPriceStr = GetStringProperty(citygrossPromoPrice, "price");
                                        string compPriceStr = GetStringProperty(citygrossPromoPrice, "comparativePrice");
                                        decimal.TryParse(citygrossPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal citygrossPrice);
                                        decimal.TryParse(compPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ordComparePrice);
                                        if (citygrossPrice != ordComparePrice)
                                        {
                                            product.RawDiscountPrice = citygrossPrice.ToString();
                                        }
                                        else
                                        {
                                            decimal pricePerItem = Math.Round(weightInKg * ordComparePrice, 2);
                                            product.RawDiscountPrice = pricePerItem.ToString();
                                        }
                                        // Add qualifyingCount somewhere if count > 0
                                        product.DiscountJmfPrice = GetStringProperty(citygrossPromoPrice, "comparativePrice");
                                    }

                                    // minQuantity
                                    // maxAppliedPerReceipt

                                    decimal.TryParse(product.OrdJmfPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ordJmf);
                                    decimal.TryParse(product.DiscountJmfPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal discJmf);
                                    decimal savingsPer = ordJmf - discJmf;
                                    product.DiscountPer = $"spar ca: {Math.Round(savingsPer, 2)}{comparePriceUnit}";

                                    decimal savings;
                                    var minQuantity = GetStringProperty(firstPromo, "minQuantity");
                                    Int32.TryParse(minQuantity, out int minQint);
                                    if (!string.IsNullOrEmpty(minQuantity) && minQint == 1)
                                    {
                                        decimal.TryParse(product.RawOrdPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a);
                                        decimal.TryParse(product.RawDiscountPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b);
                                        savings = b - b;
                                        product.RawDiscount = $"spar ca: {Math.Round(savings, 2)}kr/st";
                                    }
                                    else if (minQint >= 2)
                                    {
                                        decimal.TryParse(product.RawDiscountPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a);
                                        var pricePer = a / minQint;
                                        decimal.TryParse(product.RawOrdPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b);
                                        savings = (b - pricePer) / minQint;
                                        product.RawDiscount = $"spar ca: {Math.Round(savings, 2)}kr/st";
                                    }

                                    var redeemLimit = GetStringProperty(firstPromo, "maxAppliedPerReceipt");
                                    if (!string.IsNullOrEmpty(redeemLimit))
                                    {
                                        product.MaxQuantity = redeemLimit;
                                    }

                                    var campaignType = GetStringProperty(firstPromo, "membersOnly");
                                    if (campaignType == "True")
                                    {
                                        product.MemberDiscount = true;
                                    }
                                }
                                if (!string.IsNullOrEmpty(product.OrdJmfPrice))
                                    product.OrdJmfPrice += comparePriceUnit;

                                if (!string.IsNullOrEmpty(product.DiscountJmfPrice))
                                    product.DiscountJmfPrice += comparePriceUnit;
                            }



                            // var priceUnit = GetStringProperty(element, "priceUnit");
                            // if (!string.IsNullOrEmpty(priceUnit) && !product.RawOrdPrice.Contains(priceUnit))
                            // {
                            //     product.RawOrdPrice += $" {priceUnit}";
                            // }


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

                            if (string.IsNullOrEmpty(product.MaxQuantity))
                            {
                                product.MaxQuantity = "Inget max antal";
                            }
                        }

                        product.ID = GetStringProperty(element, "id");

                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product from JSON element: {ex.Message}");
                return null;
            }

            return product;
        }

        public string GetStringProperty(JsonElement element, params string[] propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (element.TryGetProperty(propName, out var prop))
                {
                    return prop.ToString();
                }

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
        public double? GetNumProperty(JsonElement element, params string[] propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (element.TryGetProperty(propName, out var prop))
                {
                    return prop.GetDouble();
                }

                foreach (var actualProp in element.EnumerateObject())
                {
                    if (actualProp.Name.Equals(propName, StringComparison.OrdinalIgnoreCase))
                    {
                        return actualProp.Value.GetDouble();
                    }
                }
            }

            return null;
        }
    }
}