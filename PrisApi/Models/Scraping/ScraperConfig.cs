namespace PrisApi.Models.Scraping
{
    public class ScraperConfig
    {
        public string StoreId { get; set; }
        public string BaseUrl { get; set; }
                                                        // Maybe add all Selectors for all playwright navigation?
        public string ProductListSelector { get; set; }
        public string ProductNameSelector { get; set; }
        public string ProductPriceSelector { get; set; }
        public string ProductImageSelector { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
        public int RequestDelayMs { get; set; } = 1000;
        public bool UseJavaScript { get; set; } = false;
    }
}