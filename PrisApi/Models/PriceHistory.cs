using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models;

public class PriceHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(10, 2)")]
    public decimal ComparePrice { get; set; }
    public string CompareUnit { get; set; }
    public bool WasDiscount { get; set; } = false;
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}