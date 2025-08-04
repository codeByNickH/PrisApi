using PrisApi.Models;
using Microsoft.EntityFrameworkCore;
using PrisApi.Models.Scraping;

namespace PrisApi.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; } // StoreLocation will get Store on postal code and Store will get Products and Product will get PriceHistory
        public DbSet<ScrapingJob> ScrapingJobs { get; set; }
        
    }
}