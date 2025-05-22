namespace PrisApi.Models.DTO
{
    public class StoreDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }

        // public ICollection<string> Locations { get; set; }
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public ICollection<ProductDto> Products { get; set; }
    }
}