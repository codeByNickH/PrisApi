using System.Text.Json;
using PrisApi.Models.Scraping;

namespace PrisApi.Helper.IHelper
{
    public interface IScrapeHelper
    {
        Task<List<ScrapedProduct>> ExtractProductsFromJson(string jsonContent, string storeName, string category);
        List<JsonElement> FindProductElements(JsonElement root);
        void SearchForProductArrays(JsonElement element, List<JsonElement> products);
        bool IsLikelyProduct(JsonElement element);
        Task<ScrapedProduct> ExtractProductFromElement(JsonElement element, string storeName, string category);
        string GetStringProperty(JsonElement element, params string[] propertyNames);
    }
}