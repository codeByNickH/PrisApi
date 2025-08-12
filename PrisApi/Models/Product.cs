using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Brand { get; set; }
        public string CountryOfOrigin { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal CurrentPrice { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? OriginalPrice { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ComparePrice { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DiscountPercentage { get; set; }
        public bool MemberDiscount { get; set; }
        [MaxLength(20)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Size { get; set; }
        public string ImageUrl { get; set; }
        public string MaxQuantity { get; set; }
        public string MinQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int StoreLocationId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("StoreLocationId")]
        public StoreLocation StoreLocation { get; set; }
        public ICollection<PriceHistory> PriceHistory { get; set; }
    }
}