using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class StoreLocation
    {
        [Key]
        public int Id { get; set; }
        
        public string StoreId { get; set; }
        
        [Required, MaxLength(255)]
        public string Address { get; set; }
        
        [MaxLength(100)]
        public string City { get; set; }
        
        [MaxLength(20)]
        public string PostalCode { get; set; }
        
        // Navigation property
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
    }
}