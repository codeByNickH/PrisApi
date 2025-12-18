namespace PrisApi.Models
{
    public class ProductPriceChange
    {
        public string StoreName { get; set; }
        public string ProductName { get; set;}
        public decimal NewPrice { get; set;}
        public decimal? Size { get; set;}
        public string Unit { get; set; }
        public decimal? NewComparePrice { get; set;}
        public decimal? OldPrice { get; set;}
        public decimal? OldComparePrice { get; set;}

    }
}