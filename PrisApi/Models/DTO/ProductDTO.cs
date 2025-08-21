namespace PrisApi.Models.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal OriginalComparePrice { get; set; }
        public decimal DiscountComparePrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Unit { get; set; }  // kg, each, liters
        public bool MemberDiscount { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
    }
}