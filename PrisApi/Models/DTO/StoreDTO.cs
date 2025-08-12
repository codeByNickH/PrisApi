namespace PrisApi.Models.DTO
{
    public class StoreDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }

        public ICollection<ProductDto> Products { get; set; }
    }
}