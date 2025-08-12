using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class StoreLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required, MaxLength(255)]
        public string Address { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [Required, Range(10000, 99999, ErrorMessage = "Postal code must be a 5-digit number.")]
        public int PostalCode { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}