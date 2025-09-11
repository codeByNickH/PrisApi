using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string ProdCode { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Brand { get; set; }
        [MaxLength(70)]
        public string CountryOfOrigin { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal CurrentPrice { get; set; } // Add TotalPrice, DepositPrice
        [Column(TypeName = "decimal(9, 2)")]
        public decimal? OriginalPrice { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal? ComparePrice { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal CurrentComparePrice { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? DiscountPercentage { get; set; }
        public bool MemberDiscount { get; set; }
        public bool WasDiscount { get; set; }
        [MaxLength(10)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(7, 3)")] // change to 7,2 if change to g/ml
        public decimal? Size { get; set; }    // Change to show in grams/ml? if so add extra unit, one for size and one for comparePrice unit
        public string ImageUrl { get; set; }  // Add ImageUrl class to manage stock images for different products?
        [MaxLength(70)]
        public string MaxQuantity { get; set; }
        [MaxLength(70)]
        public string MinQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<CategoryList> Categories { get; set; }
        // [Required]
        // public int CategoryId { get; set; } // Change to CategoryList to manage products like frozen chicken which match with category meat and frozen
        // [ForeignKey("CategoryId")]
        // public Category Category { get; set; }
        [Required]
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        public ICollection<PriceHistory> PriceHistory { get; set; }
    }
}