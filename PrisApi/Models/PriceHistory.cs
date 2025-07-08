using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models;

public class PriceHistory
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(10, 2)")]
    public decimal JmfPrice { get; set; }
    
    // Add jmfPrice is per kg/st/l

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}