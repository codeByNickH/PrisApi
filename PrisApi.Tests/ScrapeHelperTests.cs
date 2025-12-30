using Xunit;
using PrisApi.Helper;
using PrisApi.Models.Scraping;
using System.Threading.Tasks;
using System.Linq;
using PrisApi.Models;

namespace PrisApi.Tests
{
    public class ScrapeHelperTests
    {
        [Fact]
        public async Task ExtractProductsFromJson_ShouldReturnProducts_WhenJsonIsValid()
        {
            var helper = new ScrapeHelper();
            string json = """
                {
                    "entities": {
                        "product": {
                            "901656b9-9c14-4894-ae1a-5d225b8bcd14": {
                                "productId": "901656b9-9c14-4894-ae1a-5d225b8bcd14",
                                "retailerProductId": "2130056",
                                "name": "Skinka Basturökt 105g Lönneberga",
                                "available": true,
                                "maxQuantityReached": false,
                                "alternatives": [],
                                "price": {
                                    "current": {
                                        "amount": "29.95",
                                        "currency": "SEK"
                                    },
                                    "unit": {
                                        "label": "fop.price.per.kg",
                                        "current": {
                                            "amount": "285.24",
                                            "currency": "SEK"
                                        }
                                    }
                                },
                                "isInCurrentCatalog": true,
                                "isInProductList": false,
                                "categoryPath": [
                                    "Kött, Chark & Fågel",
                                    "Pålägg",
                                    "Skinka"
                                ],
                                "isNew": false,
                                "brand": "Lönneberga",
                                "taxCodesDisplayNames": [],
                                "retailerFinancingPlanIds": [],
                                "image": {
                                    "src": "https://handlaprivatkund.ica.se/images-v3/bf7a00ca-390e-4769-865f-dc369586872e/9ea81e85-c00f-4108-be6f-9829f9fa608a/300x300.jpg",
                                    "description": "Skinka Basturökt 105g Lönneberga",
                                    "fopSrcset": "",
                                    "bopSrcset": ""
                                },
                                "images": [
                                    {
                                        "src": "https://handlaprivatkund.ica.se/images-v3/bf7a00ca-390e-4769-865f-dc369586872e/9ea81e85-c00f-4108-be6f-9829f9fa608a/500x500.jpg",
                                        "description": "Skinka Basturökt 105g Lönneberga",
                                        "fopSrcset": "",
                                        "bopSrcset": ""
                                    }
                                ],
                                "icons": {
                                    "certification": [],
                                    "legal": []
                                },
                                "offers": [
                                    {
                                        "id": "97c73ecd-6c04-4b91-8f39-ab34fa697b8b",
                                        "retailerPromotionId": "5003739180-1764589227",
                                        "description": "2 för 35 kr",
                                        "type": "OFFER",
                                        "presentationMode": "DEFAULT",
                                        "requiredProductQuantity": 2,
                                        "limitReached": false
                                    }
                                ],
                                "offer": {
                                    "id": "97c73ecd-6c04-4b91-8f39-ab34fa697b8b",
                                    "description": "2 för 35 kr",
                                    "type": "OFFER",
                                    "retailerPromotionId": "5003739180-1764589227",
                                    "requiredProductQuantity": 2,
                                    "presentationMode": "DEFAULT"
                                },
                                "attributes": [
                                    {
                                        "icon": "sesameFree",
                                        "label": "Kött från Sverige"
                                    }
                                ],
                                "size": {
                                    "value": "0.105kg"
                                }
                            }
                        }
                    }
                }
            """;

            string storeName = "ica";
            
            var expectedValue = new ScrapedProduct()
            {
                RawName = "Skinka Basturökt 105g Lönneberga",
                RawBrand = "Lönneberga",
                CountryOfOrigin = null,
                DiscountJmfPrice = 166.67m,
                CategoryId = 0,
                DepositPrice = 0,
                DiscountPer = 118.57m,
                HasDiscount = true,
                ImageSrc = null,
                MaxQuantity = "Inget max antal",
                MemberDiscount = false,
                MinQuantity = "2 för",
                OrdJmfPrice = 285.24m,
                ProdCode = "2130056",
                RawDiscount = 12.45m,
                RawDiscountPrice = 17.5m,
                RawOrdPrice = 29.95m,
                RawUnit = "kg",
                Size = 0.105m,
                StoreId = 0,
                TotalPrice = 35,
            };

            var result = await helper.ExtractProductsFromJson(json, storeName);
            
            Assert.NotEmpty(result);
            
            var actualValue = result.First();
            expectedValue.ScrapedAt = actualValue.ScrapedAt;
            
            Assert.Equal(expectedValue, actualValue);
        }
    }
}