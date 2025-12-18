using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Helper
{
    public class ScrapeHelper : IScrapeHelper
    {
        public async Task<List<ScrapedProduct>> ExtractProductsFromJson(string jsonContent, string storeName)
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
                        var product = await ExtractProductFromElement(productElement, storeName);
                        if (product != null && !string.IsNullOrEmpty(product.RawName))
                        {
                            products.Add(product);
                        }
                    }

                    Console.WriteLine($"Extracted {products.Count} products from ICA entities.products structure");
                    return products;
                }
                if (root.TryGetProperty("results", out var results) &&
                results.ValueKind == JsonValueKind.Object &&
                results.TryGetProperty("items", out var items) &&
                items.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in items.EnumerateArray())
                    {
                        var product = await ExtractProductFromElement(item, storeName); // Call new ExtractCoopProd....() ?
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
                    var product = await ExtractProductFromElement(element, storeName);
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

        public async Task<ScrapedProduct> ExtractProductFromElement(JsonElement element, string storeName) // Add a SizeUnit and have other as PriceUnit?  // Add Pack for Drinks  // Make one for each store and make call in FromJson() with switch on storeid?
        {
            var product = new ScrapedProduct
            {
                // StoreName = storeName,
                // CategoryId = category,
                ScrapedAt = DateTime.UtcNow
            };

            try
            {
                switch (storeName)
                {
                    case "ica":

                        product = await ExtractProductFromIca(element);

                        break;
                    case "coop":

                        product = await ExtractProductFromCoop(element);

                        break;
                    case "hemkop":

                        product = await ExtractProductFromHemkop(element);

                        break;
                    case "willys":

                        product = await ExtractProductFromWillys(element);

                        break;
                    case "city gross":

                        product = await ExtractProductFromCityGross(element);

                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product from JSON element: {ex.Message}");
                return null;
            }

            return await Task.FromResult(product);
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
        private async Task<ScrapedProduct> ExtractProductFromIca(JsonElement element)
        {
            var product = new ScrapedProduct();

            product.RawName = GetStringProperty(element, "name");
            product.RawBrand = GetStringProperty(element, "brand");
            product.CountryOfOrigin = GetStringProperty(element, "countryOfOrigin");

            if (element.TryGetProperty("size", out var size) && size.ValueKind == JsonValueKind.Object)
            {
                product.Size = ParseDecimal(Regex.Replace(GetStringProperty(size, "value"), "[a-zA-Z]", ""));
            }
            if (element.TryGetProperty("packSizeDescription", out var sizeDesc) && sizeDesc.ValueKind == JsonValueKind.String && !sizeDesc.ToString().Contains("-") && product.Size == 0)
            {
                // if tvättmedel change

                product.Size = ParseDecimal(Regex.Replace(sizeDesc.ToString(), "[a-zA-Z]", ""));
            }

            decimal icaOriginalPrice = 0m;
            decimal icaOriginalJmfPrice = 0m;
            decimal icaCurrentPrice = 0m;
            decimal icaCurrentJmfPrice = 0m;
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
                        .Replace("without.liquid", "")
                        .Replace("drinkable.", "")
                        .Replace("litre.without.deposit", "l")
                        .Replace("fop.price.per.litre", "l")
                        .Replace("fop.price.per.drinkable.", "l")
                        .Replace("fop.price.per.wash", "")
                        .Replace("fop.price.per.", "")
                        .Replace("each", "st");
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
                            .Replace("without.liquid", "")
                            .Replace("drinkable.", "")
                            .Replace("litre.without.deposit", "l")
                            .Replace("fop.price.per.litre", "l")
                            .Replace("fop.price.per.drinkable.", "l")
                            .Replace("fop.price.per.wash", "")
                            .Replace("fop.price.per.", "")
                            .Replace("each", "st");
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

            if (product.Size == 0)
            {
                if (element.TryGetProperty("categoryPath", out var categoryPath) && categoryPath.ValueKind == JsonValueKind.Array && categoryPath.GetArrayLength() > 0)
                {
                    string checkTobak = categoryPath[0].ToString();
                    string checkSnus = categoryPath[1].ToString();
                    if (checkTobak == "Tobak")
                    {
                        if (checkSnus.Contains("Snus"))
                        {
                            if (product.RawName.Contains("mg") || product.RawName.Contains("S1") || product.RawName.Contains("S2") || product.RawName.Contains("S3") || product.RawName.Contains("S4") || product.RawName.Contains("S5"))
                            {
                                var match = Regex.Match(product.RawName, @"\d{1,2}\.\d{1,2}");
                                if (match.Success)
                                {
                                    product.Size = ParseDecimal(match.Value) / 1000m;
                                    product.OrdJmfPrice = Math.Round(product.OrdJmfPrice / product.Size, 2);
                                    product.RawUnit = "kg";
                                }
                                else
                                {
                                    var secondMatch = Regex.Match(product.RawName, @"\d+(?=\s*Gram)");
                                    if (secondMatch.Success)
                                    {
                                        product.Size = ParseDecimal(secondMatch.Value) / 1000m;
                                        product.OrdJmfPrice = Math.Round(product.OrdJmfPrice / product.Size, 2);
                                        product.RawUnit = "kg";
                                    }
                                }
                            }
                            else
                            {
                                product.Size = ParseDecimal(Regex.Replace(product.RawName, "[^0-9 , .]", "")) / 1000m;
                                if (product.Size > 0m)
                                {
                                    product.OrdJmfPrice = Math.Round(product.OrdJmfPrice / product.Size, 2);
                                    product.RawUnit = "kg";     // Rounding up on weight when in kg, change to g to handle
                                }
                            }
                        }
                        else
                        {
                            string cigSize = product.RawName.Replace("100", "");
                            product.Size = ParseDecimal(Regex.Replace(cigSize, "[^0-9 , .]", ""));
                            if (product.RawName.Contains("Limpa"))
                            {
                                product.TotalPrice = product.RawOrdPrice;
                                product.Size = 200;
                                product.RawOrdPrice /= 10;
                                product.OrdJmfPrice = Math.Round(product.TotalPrice / product.Size, 2);
                            }
                            else
                            {
                                product.OrdJmfPrice = Math.Round(product.RawOrdPrice / product.Size, 2);
                            }
                            product.RawUnit = "st";
                        }
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
            if (string.IsNullOrEmpty(product.MaxQuantity))
            {
                product.MaxQuantity = "Inget max antal";
            }
            // product.ImageSrc = GetStringProperty(element, "image", "imageUrl", "imageSrc", "productImage");

            product.ProdCode = GetStringProperty(element, "retailerProductId");

            return await Task.FromResult(product);
        }
        private async Task<ScrapedProduct> ExtractProductFromWillys(JsonElement element)
        {
            var product = new ScrapedProduct();

            product.RawName = GetStringProperty(element, "name");
            product.RawBrand = GetStringProperty(element, "manufacturer");
            product.RawUnit = GetStringProperty(element, "comparePriceUnit").Replace("kr/", "");

            if (element.TryGetProperty("labels", out var tags) &&
                tags.ValueKind == JsonValueKind.Array &&
                tags.GetArrayLength() > 0)
            {
                var a = tags[0];
                for (int i = 0; i < tags.GetArrayLength(); i++)
                {
                    if (tags[i].ToString() == "swedish_flag")
                    {
                        product.CountryOfOrigin = "Sverige";
                    }
                }
            }
            if (string.IsNullOrEmpty(product.CountryOfOrigin) && product.RawName.Contains("Irland"))
            {
                product.CountryOfOrigin = "Irland";
            }
            if (string.IsNullOrEmpty(product.CountryOfOrigin) && product.RawName.Contains("Sverige"))
            {
                product.CountryOfOrigin = "Sverige";
            }

            product.RawOrdPrice = ParseDecimal(GetStringProperty(element, "priceValue"));
            if (!string.IsNullOrEmpty(GetStringProperty(element, "depositPrice")))
            {
                byte.TryParse(Regex.Replace(GetStringProperty(element, "depositPrice"), "[^0-9 .]", ""), out byte deposit);
                product.DepositPrice = deposit;
            }
            string displayVolume = GetStringProperty(element, "displayVolume"); // Sort out this whole size chaos,
            if (displayVolume.Contains("p") || displayVolume.Contains("x") && !displayVolume.Contains("gx") && !displayVolume.Contains("stx"))
            {
                string[] checkedForPack = displayVolume.Contains("p") ? displayVolume.Split("p", 2) : displayVolume.Split("x", 2);
                decimal sizeWeight;
                if (displayVolume.Contains("p"))
                {
                    if (checkedForPack[0].Contains("+"))
                    {
                        checkedForPack[0] = checkedForPack[0].Substring(1, checkedForPack[0].Length - 1);
                    }
                    sizeWeight = decimal.Parse(Regex.Replace(checkedForPack[0], "[a-zA-Z /]", ""));
                }
                else
                {
                    sizeWeight = decimal.Parse(Regex.Replace(checkedForPack[1], "[a-zA-Z /]", ""));
                }

                if (displayVolume.Contains("x"))
                {
                    product.Size = Math.Round(sizeWeight * decimal.Parse(Regex.Replace(checkedForPack[0], "[a-zA-Z /]", "")), 2);
                    if (product.RawName.Contains("Snus"))
                    {
                        product.RawOrdPrice /= sizeWeight;
                        product.Size /= 1000;
                    }
                }
                else
                {
                    product.Size = sizeWeight;
                }
            }
            else if (displayVolume.Contains("gx") || displayVolume.Contains("stx"))
            {
                string[] checkPack = displayVolume.Contains("gx") ? displayVolume.Split("gx", 2) : displayVolume.Split("stx", 2);

                decimal.TryParse(Regex.Replace(checkPack[0], "[^0-9 , .]", ""), out decimal sizeOfPack);
                decimal.TryParse(Regex.Replace(checkPack[1], "[^0-9 , .]", ""), out decimal numberOfPacks);
                product.TotalPrice = product.RawOrdPrice;
                product.RawOrdPrice /= numberOfPacks;
                product.Size = sizeOfPack * numberOfPacks;
            }
            else if (displayVolume.Contains("l/"))
            {
                string[] checkedForMixingDrink = displayVolume.Split("/", 2);
                product.Size = decimal.Parse(Regex.Replace(checkedForMixingDrink[0], "[a-zA-Z /]", ""));
            }
            else if (!string.IsNullOrEmpty(displayVolume))
            {
                if (displayVolume.Contains("/"))
                {
                    string[] parts = displayVolume.Split("/", 2);
                    displayVolume = parts[0];
                }
                decimal.TryParse(Regex.Replace(displayVolume, "[^0-9 , .]", ""), out decimal volume);
                product.Size = volume != 0m ? volume : ParseDecimal(Regex.Replace(displayVolume, "[^0-9 , .]", ""));
            }
            if (product.Size > 0 && displayVolume.Contains("cl"))
            {
                product.Size /= 100m;
            }
            if (product.Size > 0 && displayVolume.Contains('g') && !displayVolume.Contains("kg") || displayVolume.Contains("ml"))
            {
                product.Size /= 1000m;
            }

            // if (element.TryGetProperty("image", out var willysImageObj) && willysImageObj.ValueKind == JsonValueKind.Object)
            // {
            //     product.ImageSrc = GetStringProperty(willysImageObj, "url");
            // }

            string comparePrice = GetStringProperty(element, "comparePrice");
            if (!string.IsNullOrEmpty(comparePrice))
            {
                product.OrdJmfPrice = decimal.Parse(Regex.Replace(comparePrice, "[a-zA-z /]", ""), CultureInfo.GetCultureInfo("de-DE"));
            }
            else if (string.IsNullOrEmpty(comparePrice) && product.Size > 0m)
            {
                product.OrdJmfPrice = Math.Round(product.RawOrdPrice / product.Size, 2);
                // product.RawUnit = "kg";
            }
            if (product.RawOrdPrice == product.OrdJmfPrice && product.Size > 0m && !product.RawName.Contains("Damastduk") && !product.RawUnit.Contains("st"))
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
                    product.RawDiscountPrice = Math.Round(ParseDecimal(GetStringProperty(willysPromoPrice, "value")), 2);
                    Console.WriteLine(Math.Round(ParseDecimal(GetStringProperty(willysPromoPrice, "value")), 2));
                }
                string discComparePrice = GetStringProperty(firstPromo, "comparePrice");
                if (!string.IsNullOrEmpty(discComparePrice))
                {
                    product.DiscountJmfPrice = decimal.Parse(Regex.Replace(discComparePrice, "[a-zA-Z /]", ""), CultureInfo.GetCultureInfo("de-DE"));
                }
                else if (string.IsNullOrEmpty(discComparePrice) && product.Size > 0m)
                { 
                
                    product.DiscountJmfPrice = Math.Round(product.RawDiscountPrice / product.Size, 2);
                }
                if (product.RawDiscountPrice == product.DiscountJmfPrice && product.Size > 0m && !product.RawUnit.Contains("st"))
                {
                    product.RawDiscountPrice = Math.Round(product.DiscountJmfPrice * product.Size, 2);
                }

                string conditionLabel = !string.IsNullOrEmpty(GetStringProperty(firstPromo, "conditionLabelFormatted")) ? GetStringProperty(firstPromo, "conditionLabelFormatted") : GetStringProperty(firstPromo, "conditionLabel");
                decimal savings = Math.Round(ParseDecimal(GetStringProperty(element, "savingsAmount")), 2);

                if (conditionLabel.Contains("för") && !conditionLabel.Contains("Handla"))
                {
                    var culture = CultureInfo.GetCultureInfo("de-DE");
                    decimal.TryParse(GetStringProperty(firstPromo, "rewardLabel"), culture, out decimal tp);
                    decimal totalPrice = tp;
                    product.RawDiscount = savings;
                    product.MinQuantity = conditionLabel;
                    product.TotalPrice = totalPrice;
                    string[] minQint = conditionLabel.Split(" ", 2);
                    // product.RawDiscountPrice = Math.Round(product.TotalPrice / int.Parse(minQint[0]), 2);
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

        

            if (string.IsNullOrEmpty(product.MaxQuantity))
            {
                product.MaxQuantity = "Inget max antal";
            }

            if (product.RawDiscount != 0m)
            {
                product.HasDiscount = true;
            }

            product.ProdCode = GetStringProperty(element, "code");

            return await Task.FromResult(product);
        }
        private async Task<ScrapedProduct> ExtractProductFromCityGross(JsonElement element)
        {
            var product = new ScrapedProduct();

            product.RawName = GetStringProperty(element, "name");
            product.RawBrand = GetStringProperty(element, "brand");
            product.CountryOfOrigin = GetStringProperty(element, "countryOfOrigin");
            string descriptiveSize = GetStringProperty(element, "descriptiveSize");

            if (product.RawBrand.Contains("BUTIKSSTYCKAT"))
            {
                product.CountryOfOrigin = "Sverige";
            }
            // if (element.TryGetProperty("image", out var citygrossImageObj) && citygrossImageObj.ValueKind == JsonValueKind.Object)
            // {
            //     product.ImageSrc = GetStringProperty(citygrossImageObj, "url");
            // }

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

            }

            if (product.RawDiscount != 0)
            {
                product.HasDiscount = true;
            }

            if (string.IsNullOrEmpty(product.MaxQuantity))
            {
                product.MaxQuantity = "Inget max antal";
            }

            product.ProdCode = GetStringProperty(element, "id");

            return await Task.FromResult(product);
        }
        private async Task<ScrapedProduct> ExtractProductFromCoop(JsonElement element)
        {
            var product = new ScrapedProduct();

            product.RawName = GetStringProperty(element, "name");
            product.RawBrand = GetStringProperty(element, "manufacturerName");
            product.Size = ParseDecimal(GetStringProperty(element, "packageSize"));

            if (element.TryGetProperty("countryOfOriginCodes", out var origin) && origin.ValueKind == JsonValueKind.Array) // Check this, no country of origin saved to db
            {
                product.CountryOfOrigin = GetStringProperty(origin[0], "value");
            }

            string packageSizeUnit = GetStringProperty(element, "packageSizeUnit");
            if (!string.IsNullOrEmpty(packageSizeUnit) && product.Size != 0)
            {
                if (packageSizeUnit.ToLower().Contains("cl"))
                {
                    product.Size /= 100m;
                }
                else if (packageSizeUnit.ToLower().Contains("gr") || packageSizeUnit.ToLower().Contains("milli") || packageSizeUnit.Contains("ml") || packageSizeUnit.Contains("MMT") || packageSizeUnit.Contains("MLT"))
                {
                    product.Size /= 1000m;
                }
            }

            // if (element.TryGetProperty("image", out var imageObj) && imageObj.ValueKind == JsonValueKind.Object)
            // {
            //     product.ImageSrc = GetStringProperty(imageObj, "url");
            // }

            if (element.TryGetProperty("piecePriceData", out var coopOrdPrice) && coopOrdPrice.ValueKind == JsonValueKind.Object)
            {
                product.RawOrdPrice = Math.Round(ParseDecimal(GetStringProperty(coopOrdPrice, "b2cPrice")), 2);
            }
            string coopDiscPrice = "";
            if (element.TryGetProperty("promotionPriceData", out var coopPromoPrice) && coopPromoPrice.ValueKind == JsonValueKind.Object)
            {
                coopDiscPrice = GetStringProperty(coopPromoPrice, "b2cPrice");
            }
            string coopOrdComparePrice = "";
            if (element.TryGetProperty("comparativePriceData", out var coopCompPrice) && coopCompPrice.ValueKind == JsonValueKind.Object)
            {
                coopOrdComparePrice = GetStringProperty(coopCompPrice, "b2cPrice");
            }
            string coopComparePriceUnit = GetStringProperty(element, "comparativePriceText")?.Replace("kr/", "").Replace("utan sås/spad", "").Replace("lit drickfärdig", "l");
            if (element.TryGetProperty("comparativePriceUnit", out var coopUnit) && coopUnit.ValueKind == JsonValueKind.Object)
            {
                coopComparePriceUnit = GetStringProperty(coopUnit, "unit")
                    .Replace("liter", "l")
                    .Replace("meter", "m")
                    .Replace("styck", "st");
            }

            product.RawUnit = coopComparePriceUnit;

            // comparativePriceData
            // : 
            // {b2cPrice: 49.94, b2bPrice: 44.59}
            // comparativePriceText
            // : 
            // "kr/kg"
            // comparativePriceUnit
            // : 
            // {unit: "kg", text: "kr/kg"}

            // declarationOfOrigin
            // : 
            // "Svensk köttråvara"
            // depositData
            // : 
            // {b2cPrice: 0, b2bPrice: 0}
            // fromSweden
            // : 
            // true
            // manufacturerName
            // : 
            // "Scan"
            // packageSize
            // : 
            // 800
            // packageSizeInformation
            // : 
            // "800 g"
            // packageSizeUnit
            // : 
            // "Gram"
            // piecePriceData
            // : 
            // {b2cPrice: 39.95, b2bPrice: 35.67}
            // salesPriceData
            // : 
            // {b2cPrice: 39.95, b2bPrice: 35.67}
            // salesUnit
            // : 
            // "Styck"



            if (!string.IsNullOrEmpty(coopOrdComparePrice) && !string.IsNullOrEmpty(coopComparePriceUnit))
            {
                product.OrdJmfPrice = Math.Round(ParseDecimal(coopOrdComparePrice), 2);
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

                if (string.IsNullOrEmpty(coopDiscPrice)) // Check this, might never enter here
                {
                    if (firstPromo.TryGetProperty("priceData", out var promoPrice) && promoPrice.ValueKind == JsonValueKind.Object)
                    {
                        product.RawDiscountPrice = Math.Round(ParseDecimal(GetStringProperty(promoPrice, "b2cPrice")), 2);
                    }
                }
                else
                {
                    product.RawDiscountPrice = Math.Round(ParseDecimal(coopDiscPrice), 2);
                }

                if (firstPromo.TryGetProperty("comparativePrice", out var discJmfPrice) &&
                    discJmfPrice.ValueKind == JsonValueKind.Object)
                {
                    product.DiscountJmfPrice = Math.Round(ParseDecimal(GetStringProperty(discJmfPrice, "b2cPrice")), 2);
                }

                var requiredAmount = GetStringProperty(firstPromo, "numberOfProductRequired");

                decimal savingsPer = Math.Round(product.OrdJmfPrice - product.DiscountJmfPrice, 2);
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
                    product.TotalPrice = Math.Round(ParseDecimal(GetStringProperty(firstPromo, "price")), 2);
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

            product.ProdCode = GetStringProperty(element, "id");

            return await Task.FromResult(product);
        }
        private async Task<ScrapedProduct> ExtractProductFromHemkop(JsonElement element)
        {
            var product = new ScrapedProduct();

            product.RawName = GetStringProperty(element, "name");
            product.RawBrand = GetStringProperty(element, "manufacturer");
            product.Size = ParseDecimal(Regex.Replace(GetStringProperty(element, "displayVolume"), "[^0-9 .]", ""));
            if (product.Size > 9)
            {
                product.Size /= 1000m;
            }

            // if (element.TryGetProperty("image", out var hemkopImageObj) && hemkopImageObj.ValueKind == JsonValueKind.Object)
            // {
            //     product.ImageSrc = GetStringProperty(hemkopImageObj, "url");
            // }

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

            product.ProdCode = GetStringProperty(element, "code");

            return await Task.FromResult(product);
        }
    }
}