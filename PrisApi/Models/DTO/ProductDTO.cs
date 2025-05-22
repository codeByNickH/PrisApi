namespace PrisApi.Models.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }  // If discounted
        public decimal DiscountPercentage { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }  // kg, each, liters
        public string ImageUrl { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
    }
}