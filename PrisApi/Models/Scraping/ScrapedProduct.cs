namespace PrisApi.Models.Scraping
{
    public class ScrapedProduct
    {
        public string RawName { get; set; }
        public string RawBrand { get; set; }
        public string RawPrice { get; set; }
        public string RawDiscount { get; set; }
        public string RawUnit { get; set; }
        public string ImageSrc { get; set; }
        public string ProductUrl { get; set; }
        public string StoreId { get; set; }
        public string MaxQuantity { get; set; }
        public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
    }
}