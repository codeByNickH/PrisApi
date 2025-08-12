using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using PrisApi.Helper.IHelper;
using PrisApi.Models.Scraping;

namespace PrisApi.Helper
{
    public class ScrapeHelper : IScrapeHelper
    {
        public async Task<List<ScrapedProduct>> ExtractProductsFromJson(string jsonContent, string storeName, string category)
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
                        var product = await ExtractProductFromElement(productElement, storeName, category);
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
                        var product = await ExtractProductFromElement(item, storeName, category); // Call new ExtractCoopProd....() ?
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
                    var product = await ExtractProductFromElement(element, storeName, category);
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

        public async Task<ScrapedProduct> ExtractProductFromElement(JsonElement element, string storeName, string category) // Add a SizeUnit and have other as PriceUnit?  // Add Pack for Drinks  // Make one for each store and make call in FromJson() with switch on storeid?
        {
            var product = new ScrapedProduct
            {
                StoreName = storeName,
                Category = category,
                ScrapedAt = DateTime.UtcNow
            };

            try
            {
                switch (storeName)
                {
                    case "ica":
                        product.RawName = GetStringProperty(element, "name", "productName", "title", "displayName");
                        product.RawBrand = GetStringProperty(element, "brand", "brandName", "manufacturer");
                        product.CountryOfOrigin = GetStringProperty(element, "countryOfOrigin");
                        if (element.TryGetProperty("size", out var size) && size.ValueKind == JsonValueKind.Object)
                        {
                            product.Size = ParseDecimal(Regex.Replace(GetStringProperty(size, "value"), "[a-zA-Z]", ""));
                        }
                        if (element.TryGetProperty("packSizeDescription", out var sizeDesc) && sizeDesc.ValueKind == JsonValueKind.String && !sizeDesc.ToString().Contains("-") && product.Size == 0)
                        {
                            product.Size = ParseDecimal(Regex.Replace(sizeDesc.ToString(), "[a-zA-Z]", ""));
                        }
                        decimal icaOriginalPrice = 0m;
                        decimal icaOriginalJmfPrice = 0m;
                        decimal icaCurrentPrice = 0m;
                        decimal icaCurrentJmfPrice = 0m;
                        string unitDisplay = null;
                        if (element.TryGetProperty("price", out var price) && price.ValueKind == JsonValueKind.Object)
                        {
                            if (price.TryGetProperty("original", out var original) && original.ValueKind == JsonValueKind.Object)
                            {
                                icaOriginalPrice = ParseDecimal(GetStringProperty(original, "amount"));
                            }
                            if (price.TryGetProperty("current", out var current) && current.ValueKind == JsonValueKind.Object)
                            {
                                icaCurrentPrice = ParseDecimal(GetStringProperty(current, "amount"));
                            }
                            if (price.TryGetProperty("unit", out var unit) && unit.ValueKind == JsonValueKind.Object)
                            {
                                product.RawUnit = GetStringProperty(unit, "label")
                                    ?.Replace("without.liquid", " utan spad")
                                    ?.Replace("litre.without.deposit", "LTR")
                                    ?.Replace("fop.price.per.", "")
                                    ?.Replace("each", "st");
                                if (unit.TryGetProperty("original", out var originalUnit) && originalUnit.ValueKind == JsonValueKind.Object)
                                {
                                    icaOriginalJmfPrice = ParseDecimal(GetStringProperty(originalUnit, "amount"));
                                }
                                if (unit.TryGetProperty("current", out var currentUnit) && currentUnit.ValueKind == JsonValueKind.Object)
                                {
                                    icaCurrentJmfPrice = ParseDecimal(GetStringProperty(currentUnit, "amount"));
                                }
                            }
                            if (icaOriginalPrice != 0)
                            {
                                product.RawOrdPrice = icaOriginalPrice;
                                product.OrdJmfPrice = icaOriginalJmfPrice;
                                product.RawDiscountPrice = icaCurrentPrice;
                                product.DiscountJmfPrice = icaCurrentJmfPrice;
                            }
                            else if (icaCurrentPrice != 0)
                            {
                                product.RawOrdPrice = icaCurrentPrice;
                                product.OrdJmfPrice = icaCurrentJmfPrice;
                            }
                            else
                            {
                                product.RawOrdPrice = ParseDecimal(GetStringProperty(price, "amount"));
                                if (element.TryGetProperty("unitPrice", out var unitPrice) && unitPrice.ValueKind == JsonValueKind.Object)
                                {
                                    product.RawUnit = GetStringProperty(unitPrice, "unit")
                                        ?.Replace("without.liquid", " utan spad")
                                        ?.Replace("litre.without.deposit", "LTR")
                                        ?.Replace("fop.price.per.", "")
                                        ?.Replace("each", "st");
                                    if (unitPrice.TryGetProperty("price", out var priceInnerObj) && priceInnerObj.ValueKind == JsonValueKind.Object)
                                    {
                                        product.OrdJmfPrice = ParseDecimal(GetStringProperty(priceInnerObj, "amount"));
                                    }
                                }
                            }
                        }

                        if (element.TryGetProperty("catchweight", out var weight) && weight.ValueKind == JsonValueKind.Object)
                        {
                            if (weight.TryGetProperty("typicalQuantity", out var quantity) && quantity.ValueKind == JsonValueKind.Object)
                            {
                                product.Size = ParseDecimal(GetStringProperty(quantity, "value"));
                                string ifGram = GetStringProperty(quantity, "uom");
                                if (ifGram == "G" && product.Size > 9)
                                {
                                    product.Size /= 1000m;
                                }
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
                            product.RawDiscountPrice = ParseDecimal(promoPriceStr);
                            if (!string.IsNullOrEmpty(promoUnitPriceStr) && !string.IsNullOrEmpty(promoUnitStr))
                            {
                                product.DiscountJmfPrice = ParseDecimal(promoUnitPriceStr);
                            }
                        }

                        if (product.RawOrdPrice > 0 && product.RawDiscountPrice > 0)
                        {
                            product.RawDiscount = product.RawOrdPrice - product.RawDiscountPrice;
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

                        if (string.IsNullOrEmpty(promoPriceStr))
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
                                    var parts = promoPriceStr.Split(" ", 4);
                                    var j = ParseDecimal(parts[0]);
                                    var i = ParseDecimal(parts[2]);
                                    product.RawDiscountPrice = Math.Round(i / j, 2);
                                    product.MinQuantity = $"{j} för";
                                    product.RawDiscount = product.RawOrdPrice - product.RawDiscountPrice;
                                    product.TotalPrice = i;
                                    product.DiscountJmfPrice = Math.Round(product.RawDiscountPrice / product.Size, 2);
                                }
                                product.DiscountPer = Math.Round(product.OrdJmfPrice - product.DiscountJmfPrice, 2);
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
                                    var parts = promoPriceStr.Split(" ", 4);
                                    var j = ParseDecimal(parts[0]);
                                    var i = ParseDecimal(parts[2]);
                                    product.RawDiscountPrice = Math.Round(i / j, 2);
                                    product.MinQuantity = $"{j} för";
                                    product.RawDiscount = product.RawOrdPrice - product.RawDiscountPrice;
                                    product.TotalPrice = i;
                                    product.DiscountJmfPrice = Math.Round(product.RawDiscountPrice / product.Size, 2);
                                }

                                product.DiscountPer = Math.Round(product.OrdJmfPrice - product.DiscountJmfPrice, 2);
                            }
                        }

                        if (product.RawDiscount != 0)
                        {
                            product.HasDiscount = true;
                        }

                        product.ImageSrc = GetStringProperty(element, "image", "imageUrl", "imageSrc", "productImage");

                        product.ID = GetStringProperty(element, "retailerProductId");
                        break;

                    case "coop":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturerName");
                        product.Size = ParseDecimal(GetStringProperty(element, "packageSize"));

                        if (element.TryGetProperty("originCountry", out var origin) && origin.ValueKind == JsonValueKind.Object)
                        {
                            product.CountryOfOrigin = GetStringProperty(origin, "value");
                        }

                        string packageSizeUnit = GetStringProperty(element, "packageSizeUnit");
                        if (!string.IsNullOrEmpty(packageSizeUnit) && product.Size != 0)
                        {
                            if (packageSizeUnit.ToLower().Contains("cl"))
                            {
                                product.Size /= 100m;
                            }
                            else if (packageSizeUnit.ToLower().Contains("gr") || packageSizeUnit.ToLower().Contains("milli"))
                            {
                                product.Size /= 1000m;
                            }
                        }

                        // if (element.TryGetProperty("image", out var imageObj) && imageObj.ValueKind == JsonValueKind.Object)
                        // {
                        //     product.ImageSrc = GetStringProperty(imageObj, "url");
                        // }

                        product.RawOrdPrice = ParseDecimal(GetStringProperty(element, "piecePrice"));

                        string coopDiscPrice = GetStringProperty(element, "promotionPrice");
                        string coopOrdComparePrice = GetStringProperty(element, "comparativePrice");
                        string coopComparePriceUnit = GetStringProperty(element, "comparativePriceText")?.Replace("kr/", "");
                        product.RawUnit = coopComparePriceUnit;

                        if (!string.IsNullOrEmpty(coopOrdComparePrice) && !string.IsNullOrEmpty(coopComparePriceUnit))
                        {
                            product.OrdJmfPrice = ParseDecimal(coopOrdComparePrice);
                        }

                        if (product.OrdJmfPrice == 0)
                        {
                            product.OrdJmfPrice = Math.Round(product.RawOrdPrice / product.Size, 2);
                        }

                        if (element.TryGetProperty("onlinePromotions", out var coopPromotions) &&
                            coopPromotions.ValueKind == JsonValueKind.Array &&
                            coopPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = coopPromotions[0];

                            if (string.IsNullOrEmpty(coopDiscPrice))
                            {
                                product.RawDiscountPrice = ParseDecimal(GetStringProperty(firstPromo, "price"));
                            }
                            else
                            {
                                product.RawDiscountPrice = ParseDecimal(coopDiscPrice);
                            }

                            if (firstPromo.TryGetProperty("comparativePrice", out var discJmfPrice) &&
                                discJmfPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.DiscountJmfPrice = ParseDecimal(GetStringProperty(discJmfPrice, "b2cPrice"));
                            }

                            var requiredAmount = GetStringProperty(firstPromo, "numberOfProductRequired");

                            decimal savingsPer = product.OrdJmfPrice - product.DiscountJmfPrice;
                            product.DiscountPer = Math.Round(savingsPer, 2);

                            if (string.IsNullOrEmpty(requiredAmount))
                            {
                                product.RawDiscount = Math.Round(product.RawOrdPrice - product.RawDiscountPrice, 2);
                            }
                            else if (!string.IsNullOrEmpty(requiredAmount))
                            {
                                int.TryParse(requiredAmount, out int i);
                                product.RawDiscountPrice = Math.Round(product.RawDiscountPrice / i, 2);
                                product.MinQuantity = $"{i} för";
                                product.RawDiscount = Math.Round(product.RawOrdPrice - product.RawDiscountPrice, 2);
                                product.TotalPrice = ParseDecimal(GetStringProperty(firstPromo, "price"));
                            }

                            if (firstPromo.TryGetProperty("maxNumberOfUseWithUnit", out var redeemLimit) &&
                                redeemLimit.ValueKind == JsonValueKind.Object)
                            {
                                product.MaxQuantity = $"Max {GetStringProperty(redeemLimit, "value")}/hushåll";
                            }

                            var campaignType = GetStringProperty(firstPromo, "medMeraRequired");
                            if (campaignType == "True")
                            {
                                product.MemberDiscount = true;
                            }
                        }

                        if (string.IsNullOrEmpty(product.MaxQuantity))
                        {
                            product.MaxQuantity = "Inget max antal";
                        }

                        if (product.RawDiscount != 0)
                        {
                            product.HasDiscount = true;
                        }

                        product.ID = GetStringProperty(element, "id");

                        break;
                    case "hemkop":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturer");
                        product.Size = ParseDecimal(Regex.Replace(GetStringProperty(element, "displayVolume"), "[^0-9 .]", ""));
                        if (product.Size > 9)
                        {
                            product.Size /= 1000m;
                        }

                        if (element.TryGetProperty("image", out var hemkopImageObj) && hemkopImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(hemkopImageObj, "url");
                        }

                        product.RawOrdPrice = ParseDecimal(GetStringProperty(element, "priceValue"));

                        product.OrdJmfPrice = decimal.Parse(Regex.Replace(GetStringProperty(element, "comparePrice"), "[a-zA-Z /]", ""));
                        product.RawUnit = GetStringProperty(element, "comparePriceUnit");

                        if (element.TryGetProperty("potentialPromotions", out var hemkopPromotions) &&
                            hemkopPromotions.ValueKind == JsonValueKind.Array &&
                            hemkopPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = hemkopPromotions[0];

                            if (firstPromo.TryGetProperty("price", out var hemkopPromoPrice) &&
                                hemkopPromoPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.RawDiscountPrice = ParseDecimal(GetStringProperty(hemkopPromoPrice, "value"));
                            }

                            product.DiscountJmfPrice = decimal.Parse(Regex.Replace(GetStringProperty(firstPromo, "comparePrice"), "[a-zA-Z /]", ""));
                            product.DiscountPer = Math.Round(product.OrdJmfPrice - product.DiscountJmfPrice, 2);

                            string conditionLabel = GetStringProperty(firstPromo, "conditionLabel");
                            decimal totalPrice = ParseDecimal(Regex.Replace(GetStringProperty(firstPromo, "rewardLabel"), "[a-zA-Z /]", ""));
                            decimal savings = ParseDecimal(GetStringProperty(element, "savingsAmount"));


                            if (conditionLabel.Contains("för"))
                            {
                                product.RawDiscount = savings;
                                product.MinQuantity = conditionLabel;
                                product.TotalPrice = totalPrice;
                            }
                            else
                            {
                                product.RawDiscount = savings;
                            }

                            string redeemLimit = GetStringProperty(firstPromo, "redeemLimitLabel");
                            if (!string.IsNullOrEmpty(redeemLimit))
                            {
                                product.MaxQuantity = redeemLimit;
                            }

                            string campaignType = GetStringProperty(firstPromo, "campaignType");
                            if (campaignType.ToLower() == "loyalty")
                            {
                                product.MemberDiscount = true;
                            }
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

                        if (product.RawDiscount != 0)
                        {
                            product.HasDiscount = true;
                        }

                        product.ID = GetStringProperty(element, "code");

                        break;
                    case "willys":

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "manufacturer");
                        product.RawUnit = GetStringProperty(element, "comparePriceUnit").Replace("kr/", "");

                        if (GetStringProperty(element, "depositPrice") != "")
                        {
                            byte.TryParse(Regex.Replace(GetStringProperty(element, "depositPrice"), "[^0-9 .]", ""), out byte deposit);
                            product.DepositPrice = deposit;
                        }
                        string displayVolume = GetStringProperty(element, "displayVolume");
                        if (displayVolume.Contains("p") || displayVolume.Contains("x"))
                        {
                            string[] checkedForPack = displayVolume.Contains("p") ? displayVolume.Split("p", 2) : displayVolume.Split("x", 2);
                            product.Size = !string.IsNullOrEmpty(checkedForPack[1]) ? decimal.Parse(Regex.Replace(checkedForPack[1], "[a-zA-Z /]", "")) : 0;
                        }
                        else if (displayVolume.Contains("l/"))
                        {
                            string[] checkedForMixingDrink = displayVolume.Split("/", 2);
                            product.Size = decimal.Parse(Regex.Replace(checkedForMixingDrink[0], "[a-zA-Z /]", ""));
                        }
                        else if (displayVolume != "")
                        {
                            decimal.TryParse(Regex.Replace(displayVolume, "[^0-9 , .]", ""), out decimal volume);
                            product.Size = volume != 0m ? volume : ParseDecimal(Regex.Replace(displayVolume, "[^0-9 , .]", ""));
                        }
                        if (displayVolume.Contains("cl"))
                        {
                            product.Size /= 100m;
                        }
                        if (displayVolume.Contains('g') && !displayVolume.Contains("kg") || displayVolume.Contains("ml"))
                        {
                            product.Size /= 1000m;
                        }

                        if (element.TryGetProperty("image", out var willysImageObj) && willysImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(willysImageObj, "url");
                        }

                        product.RawOrdPrice = ParseDecimal(GetStringProperty(element, "priceValue"));
                        string comparePrice = GetStringProperty(element, "comparePrice");
                        if (comparePrice != "")
                        {
                            product.OrdJmfPrice = decimal.Parse(Regex.Replace(GetStringProperty(element, "comparePrice"), "[a-zA-z /]", ""));
                        }
                        else
                        {
                            product.OrdJmfPrice = Math.Round(product.RawOrdPrice / product.Size, 2);
                            product.RawUnit = "kg";
                        }
                        if (product.RawOrdPrice == product.OrdJmfPrice)
                        {
                            product.RawOrdPrice = Math.Round(product.OrdJmfPrice * product.Size, 2);
                        }

                        if (element.TryGetProperty("potentialPromotions", out var wilysPromotions) &&
                            wilysPromotions.ValueKind == JsonValueKind.Array &&
                            wilysPromotions.GetArrayLength() > 0)
                        {
                            var firstPromo = wilysPromotions[0];


                            if (firstPromo.TryGetProperty("price", out var willysPromoPrice) &&
                                willysPromoPrice.ValueKind == JsonValueKind.Object)
                            {
                                product.RawDiscountPrice = ParseDecimal(GetStringProperty(willysPromoPrice, "value"));
                            }
                            string discComparePrice = GetStringProperty(firstPromo, "comparePrice");
                            if (discComparePrice != "")
                            {
                                product.DiscountJmfPrice = decimal.Parse(Regex.Replace(discComparePrice, "[a-zA-Z /]", ""));
                            }
                            else
                            {
                                product.DiscountJmfPrice = Math.Round(product.RawDiscountPrice / product.Size, 2);
                            }
                            if (product.RawDiscountPrice == product.DiscountJmfPrice)
                            {
                                product.RawDiscountPrice = Math.Round(product.DiscountJmfPrice * product.Size, 2);
                            }

                            string conditionLabel = GetStringProperty(firstPromo, "conditionLabelFormatted");
                            decimal savings = Math.Round(ParseDecimal(GetStringProperty(element, "savingsAmount")), 2);

                            if (conditionLabel.Contains("för"))
                            {
                                decimal totalPrice = decimal.Parse(Regex.Replace(GetStringProperty(firstPromo, "rewardLabel"), "[a-zA-Z /]", ""));
                                product.RawDiscount = savings;
                                product.MinQuantity = conditionLabel;
                                product.TotalPrice = totalPrice;
                                string[] minQint = conditionLabel.Split(" ", 2);
                                product.RawDiscountPrice = Math.Round(product.TotalPrice / int.Parse(minQint[0]), 2);
                            }
                            else if (product.RawDiscount == 0 && savings > 0)
                            {
                                product.RawDiscount = Math.Round(product.RawOrdPrice - product.RawDiscountPrice, 2);
                            }
                            if (product.DiscountPer == 0 && savings > 0)
                            {
                                product.DiscountPer = Math.Round(product.OrdJmfPrice - product.DiscountJmfPrice, 2);
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

                        if (product.RawDiscount != 0)
                        {
                            product.HasDiscount = true;
                        }

                        product.ID = GetStringProperty(element, "code");
                        break;
                    case "citygross":
                        // countryOfManufacture, Add country of origin. Get from country tag and if null use OfManu.. instead.

                        product.RawName = GetStringProperty(element, "name");
                        product.RawBrand = GetStringProperty(element, "brand");
                        product.CountryOfOrigin = GetStringProperty(element, "countryOfOrigin");
                        string descriptiveSize = GetStringProperty(element, "descriptiveSize");

                        if (element.TryGetProperty("image", out var citygrossImageObj) && citygrossImageObj.ValueKind == JsonValueKind.Object)
                        {
                            product.ImageSrc = GetStringProperty(citygrossImageObj, "url");
                        }

                        decimal weightInKg = 0;
                        if (element.TryGetProperty("netContent", out var cityWeight) &&
                            cityWeight.ValueKind == JsonValueKind.Object)
                        {
                            if (descriptiveSize.Contains("ML"))
                            {
                                product.Size = ParseDecimal(GetStringProperty(cityWeight, "value")) / 1000000m;
                            }
                            else if (descriptiveSize.Contains("CL"))
                            {
                                product.Size = ParseDecimal(GetStringProperty(cityWeight, "value")) / 100000m;
                            }
                            else if (descriptiveSize.Contains("DL"))
                            {
                                product.Size = ParseDecimal(GetStringProperty(cityWeight, "value")) / 10000m;
                            }
                            else
                            {
                                product.Size = ParseDecimal(GetStringProperty(cityWeight, "value")) / 1000m;
                            }
                            weightInKg = product.Size;
                        }
                        if (element.TryGetProperty("productStoreDetails", out var details) &&
                            details.ValueKind == JsonValueKind.Object)
                        {
                            if (details.TryGetProperty("prices", out var prices) &&
                                prices.ValueKind == JsonValueKind.Object)
                            {
                                if (prices.TryGetProperty("currentPrice", out var currentPrice) &&
                                    currentPrice.ValueKind == JsonValueKind.Object)
                                {
                                    decimal citygrossPrice = ParseDecimal(GetStringProperty(currentPrice, "price"));
                                    decimal ordComparePrice = ParseDecimal(GetStringProperty(currentPrice, "comparativePrice"));
                                    if (citygrossPrice != ordComparePrice)
                                    {
                                        product.RawOrdPrice = citygrossPrice;
                                    }
                                    else
                                    {
                                        product.RawOrdPrice = Math.Round(weightInKg * ordComparePrice, 2);
                                    }
                                    product.RawUnit = GetStringProperty(currentPrice, "comparativePriceUnit").Replace("M", "").ToLower();
                                    if (ordComparePrice != 0)
                                    {
                                        product.OrdJmfPrice = ordComparePrice;
                                    }
                                }
                                if (prices.TryGetProperty("promotions", out var promotions) &&
                                    promotions.ValueKind == JsonValueKind.Array &&
                                    promotions.GetArrayLength() > 0)
                                {
                                    var firstPromo = promotions[0];
                                    if (firstPromo.TryGetProperty("priceDetails", out var citygrossPromoPrice) &&
                                        citygrossPromoPrice.ValueKind == JsonValueKind.Object)
                                    {
                                        decimal citygrossPrice = ParseDecimal(GetStringProperty(citygrossPromoPrice, "price"));
                                        decimal discComparePrice = ParseDecimal(GetStringProperty(citygrossPromoPrice, "comparativePrice"));
                                        if (citygrossPrice != discComparePrice)
                                        {
                                            product.RawDiscountPrice = citygrossPrice;
                                        }
                                        else
                                        {
                                            product.RawDiscountPrice = Math.Round(weightInKg * discComparePrice, 2);
                                        }
                                        product.DiscountJmfPrice = discComparePrice;
                                    }

                                    decimal savingsPer = product.OrdJmfPrice - product.DiscountJmfPrice;
                                    product.DiscountPer = Math.Round(savingsPer, 2);

                                    var minQuantity = GetStringProperty(firstPromo, "minQuantity");
                                    int.TryParse(minQuantity, out int minQint);
                                    if (!string.IsNullOrEmpty(minQuantity) && minQint == 1)
                                    {
                                        product.RawDiscount = Math.Round(product.RawOrdPrice - product.RawDiscountPrice, 2);
                                    }
                                    else if (!string.IsNullOrEmpty(minQuantity) && minQint > 1)
                                    {
                                        product.MinQuantity = $"{minQuantity} för";
                                        product.TotalPrice = ParseDecimal(GetStringProperty(firstPromo, "value"));
                                        product.RawDiscount = Math.Round(product.RawOrdPrice - product.RawDiscountPrice, 2);
                                    }

                                    var redeemLimit = GetStringProperty(firstPromo, "maxAppliedPerReceipt");
                                    if (!string.IsNullOrEmpty(redeemLimit))
                                    {
                                        int.TryParse(redeemLimit, out int limit);
                                        if (limit > 0)
                                        {
                                            product.MaxQuantity = $"Max {limit}/hushåll";
                                        }
                                    }

                                    var campaignType = GetStringProperty(firstPromo, "membersOnly");
                                    if (campaignType == "True")
                                    {
                                        product.MemberDiscount = true;
                                    }
                                }
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
                        }

                        if (product.RawDiscount != 0)
                        {
                            product.HasDiscount = true;
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
        private static decimal ParseDecimal(string number)
        {
            decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal i);

            return i;
        }
    }
}