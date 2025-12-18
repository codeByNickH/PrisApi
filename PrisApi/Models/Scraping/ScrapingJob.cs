using System.ComponentModel.DataAnnotations;

namespace PrisApi.Models.Scraping;

public class ScrapingJob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(50)]
    public string StoreName { get; set; }
    [MaxLength(100)]
    public string StoreLocation { get; set; } // city/district
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool Success { get; set; } = false;
    [MaxLength(255)]
    public string ErrorMessage { get; set; }
    [Range(1, 100000)]
    public int ProductsScraped { get; set; }
    [Range(1, 100000)]
    public int NewProducts { get; set; }
    [Range(1, 100000)]
    public int UpdatedProducts { get; set; }
}