using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int StoreLocationId { get; set; }
        [ForeignKey("StoreLocationId")]
        public StoreLocation StoreLocation { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}