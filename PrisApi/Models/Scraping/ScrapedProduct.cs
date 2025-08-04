namespace PrisApi.Models.Scraping
{
    public class ScrapedProduct
    {
        public string ID { get; set; }
        public string RawName { get; set; }
        public string RawBrand { get; set; }
        public decimal RawOrdPrice { get; set; }
        public decimal RawDiscountPrice { get; set; }
        public decimal RawDiscount { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountJmfPrice { get; set; }
        public decimal OrdJmfPrice { get; set; }
        public bool MemberDiscount { get; set; } = false;
        public bool HasDiscount { get; set; } = false;
        public string RawUnit { get; set; }
        public decimal Size { get; set; }
        public byte DepositPrice { get; set; }
        public string ImageSrc { get; set; }
        public string ProductUrl { get; set; }
        public string StoreId { get; set; }
        public string StoreLocation { get; set; }
        public string MaxQuantity { get; set; }
        public string MinQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
    }
}