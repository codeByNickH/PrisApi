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
        [MaxLength(70)]
        public string CountryOfOrigin { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal CurrentPrice { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal? OriginalPrice { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal? ComparePrice { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? DiscountPercentage { get; set; }
        public bool MemberDiscount { get; set; }
        [MaxLength(10)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(6, 3)")]
        public decimal? Size { get; set; }
        public string ImageUrl { get; set; }
        [MaxLength(70)]
        public string MaxQuantity { get; set; }
        [MaxLength(70)]
        public string MinQuantity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        public ICollection<PriceHistory> PriceHistory { get; set; }
    }
}