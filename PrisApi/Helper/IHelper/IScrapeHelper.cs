using System.Text.Json;
using PrisApi.Models.Scraping;

namespace PrisApi.Helper.IHelper
{
    public interface IScrapeHelper
    {
        Task<List<ScrapedProduct>> ExtractProductsFromJson(string jsonContent, string storeId);
        List<JsonElement> FindProductElements(JsonElement root);
        void SearchForProductArrays(JsonElement element, List<JsonElement> products);
        bool IsLikelyProduct(JsonElement element);
        Task<ScrapedProduct> ExtractProductFromElement(JsonElement element, string storeId);
        string GetStringProperty(JsonElement element, params string[] propertyNames);
    }
}