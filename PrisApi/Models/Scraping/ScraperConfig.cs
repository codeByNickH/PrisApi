using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models.Scraping
{
    public class ScraperConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string StoreName { get; set; }
        [MaxLength(150)]
        public string BaseUrl { get; set; }
        public int RequestDelayMs { get; set; } = 50;
        public bool UseJavaScript { get; set; } = false;
        [Required]
        public int ScraperSelectorId { get; set; }
        [ForeignKey("ScraperSelectorId")]
        public ScraperSelector ScraperSelector { get; set; }
        [Required]
        public int ScraperNavigationId { get; set; }
        [ForeignKey("ScraperNavigationId")]
        public ScraperNavigation ScraperNavigation { get; set; }
    }
}