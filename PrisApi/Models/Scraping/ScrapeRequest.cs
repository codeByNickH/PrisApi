namespace PrisApi.Models.Scraping
{
    public class ScrapeRequest
    {
        public int ConfigId { get; set; }
        public string NavigationPath { get; set; }
        public int CategoryId { get; set; }
        public string[] AllowedCities { get; set; } = Array.Empty<string>();
        public string[] AllowedDistricts { get; set; } = Array.Empty<string>();
        public string ActionName { get; set; }
        public bool PersistPerJob { get; set; } = true;
    }
}