namespace PrisApi.Models
{
    public class ProductPriceChange
    {
        public string StoreName { get; set; }
        public string City { get; set;}
        public string Address { get; set; }
        public string Brand { get; set; } = "";
        public string ProductName { get; set;}
        public decimal NewPrice { get; set;}
        public decimal? Size { get; set;}
        public string Unit { get; set; }
        public decimal? NewComparePrice { get; set;}
        public decimal? OldPrice { get; set;}
        public decimal? OldComparePrice { get; set;}
        public string MultiOffer { get; set; } = "";
        public string CountryOfOrigin { get; set; } = "";
        public bool MemberDiscount { get; set; }
    }
}