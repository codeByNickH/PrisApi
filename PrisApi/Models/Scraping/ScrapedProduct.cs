namespace PrisApi.Models.Scraping
{
    public class ScrapedProduct
    {
        public string ID { get; set; }
        public string RawName { get; set; }
        public string RawBrand { get; set; }
        public string RawOrdPrice { get; set; }
        public string RawDiscountPrice { get; set; }
        public string RawDiscount { get; set; }
        public string DiscountJmfPrice { get; set; }
        public string OrdJmfPrice { get; set; }
        public bool MemberDiscount { get; set; } = false;
        public string RawUnit { get; set; }
        public string ImageSrc { get; set; }
        public string ProductUrl { get; set; }
        public string StoreId { get; set; }
        public string StoreLocation { get; set; }
        public string MaxQuantity { get; set; }
        public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
    }
}