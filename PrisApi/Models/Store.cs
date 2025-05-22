using System.ComponentModel.DataAnnotations;

namespace PrisApi.Models
{
    public class Store
    {
        [Key]
        public string Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Website { get; set; }

        public string LogoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; }

    }
}