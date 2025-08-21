using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models.Scraping
{
    public class ScraperSelector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(150)]
        public string CookieBannerSelector { get; set; }
        [MaxLength(150)]
        public string RejectCookiesSelector { get; set; }
        [MaxLength(150)]
        public string ChooseStoreSelector { get; set; }
        [MaxLength(150)]
        public string PickupOptionSelector { get; set; }
        [MaxLength(150)]
        public string SearchStoreSelector { get; set; }
        [MaxLength(150)]
        public string SearchButtonSelector { get; set; }
        [MaxLength(150)]
        public string SelectStoreSelector { get; set; }
        [MaxLength(150)]
        public string CloseChooseTabSelector { get; set; }
        [MaxLength(150)]
        public string CategoryNavSelector { get; set; }
    }
}