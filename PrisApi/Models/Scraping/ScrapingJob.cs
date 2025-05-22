namespace PrisApi.Models.Scraping;

public class ScrapingJob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StoreId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int ProductsScraped { get; set; }
    public int NewProducts { get; set; }
    public int UpdatedProducts { get; set; }
}