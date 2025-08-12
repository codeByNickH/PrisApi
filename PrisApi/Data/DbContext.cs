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
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<ScrapingJob> ScrapingJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StoreLocation>().HasData(
                new StoreLocation()
                {
                    Id = 1,
                    StoreId = 1,
                    Address = "Handelsgatan 9",
                    City = "Bollnäs",
                    PostalCode = 82391,
                },
                new StoreLocation()
                {
                    Id = 2,
                    StoreId = 2,
                    Address = "Aseavägen 1",
                    City = "Bollnäs",
                    PostalCode = 82130
                },
                new StoreLocation()
                {
                    Id = 3,
                    StoreId = 3,
                    Address = "Norrlandsvägen 90",
                    City = "Bollnäs",
                    PostalCode = 82136
                },
                new StoreLocation()
                {
                    Id = 4,
                    StoreId = 5,
                    Address = "Västanågatan 3",
                    City = "Alfta",
                    PostalCode = 82231
                },
                new StoreLocation()
                {
                    Id = 5,
                    StoreId = 1,
                    Address = "Industrigatan 16",
                    City = "Gävle",
                    PostalCode = 80283
                },
                new StoreLocation()
                {
                    Id = 6,
                    StoreId = 1,
                    Address = "Ingenjörsgatan 1",
                    City = "Gävle",
                    PostalCode = 80293
                },
                new StoreLocation()
                {
                    Id = 7,
                    StoreId = 2,
                    Address = "Södra Kungsvägen 14",
                    City = "Gävle",
                    PostalCode = 80257
                },
                new StoreLocation()
                {
                    Id = 8,
                    StoreId = 2,
                    Address = "Lokförargatan 5",
                    City = "Gävle",
                    PostalCode = 80322
                },
                new StoreLocation()
                {
                    Id = 9,
                    StoreId = 3,
                    Address = "Valbovägen 307",
                    City = "Gävle",
                    PostalCode = 81835
                },
                new StoreLocation()
                {
                    Id = 10,
                    StoreId = 3,
                    Address = "Skogmursvägen 35",
                    City = "Gävle",
                    PostalCode = 80269
                },
                new StoreLocation()
                {
                    Id = 11,
                    StoreId = 4,
                    Address = "Ingenjörsgatan 15",
                    City = "Gävle",
                    PostalCode = 80293
                },
                new StoreLocation()
                {
                    Id = 12,
                    StoreId = 1,
                    Address = "Fyrisparksvägen 1",
                    City = "Uppsala",
                    PostalCode = 75267
                },
                new StoreLocation()
                {
                    Id = 13,
                    StoreId = 1,
                    Address = "Visthusvägen 1",
                    City = "Uppsala",
                    PostalCode = 75454
                },
                new StoreLocation()
                {
                    Id = 14,
                    StoreId = 2,
                    Address = "Herrhagsvägen 17",
                    City = "Uppsala",
                    PostalCode = 75267
                },
                new StoreLocation()
                {
                    Id = 15,
                    StoreId = 2,
                    Address = "Björkgatan 4",
                    City = "Uppsala",
                    PostalCode = 75327
                },

                new StoreLocation()
                {
                    Id = 16,
                    StoreId = 3,
                    Address = "Rapsgatan 1",
                    City = "Uppsala",
                    PostalCode = 75323
                },
                new StoreLocation()
                {
                    Id = 17,
                    StoreId = 4,
                    Address = "Stångjärnsgatan 10",
                    City = "Uppsala",
                    PostalCode = 75323
                },
                new StoreLocation()
                {
                    Id = 18,
                    StoreId = 5,
                    Address = "Dragarbrunnsgatan 50",
                    City = "Uppsala",
                    PostalCode = 75321
                },
                new StoreLocation()
                {
                    Id = 19,
                    StoreId = 1,
                    Address = "Stickvägen 7",
                    City = "Söderhamn",
                    PostalCode = 82640
                },
                new StoreLocation()
                {
                    Id = 20,
                    StoreId = 1,
                    Address = "Norra Hamngatan 11",
                    City = "Söderhamn",
                    PostalCode = 82630
                },
                new StoreLocation()
                {
                    Id = 21,
                    StoreId = 2,
                    Address = "Flöjtvägen 1",
                    City = "Söderhamn",
                    PostalCode = 82640
                },
                new StoreLocation()
                {
                    Id = 22,
                    StoreId = 1,
                    Address = "Blockvägen 1",
                    City = "Hudiksvall",
                    PostalCode = 82434
                },
                new StoreLocation()
                {
                    Id = 23,
                    StoreId = 3,
                    Address = "Furulundsvägen 2",
                    City = "Hudiksvall",
                    PostalCode = 82431
                },
                new StoreLocation()
                {
                    Id = 24,
                    StoreId = 5,
                    Address = "Bryggeriet, Västra Tullgatan 13",
                    City = "Hudiksvall",
                    PostalCode = 82430
                }
                );
            modelBuilder.Entity<Store>().HasData(
                new Store()
                {
                    Id = 1,
                    Name = "ica",
                    Website = "https://handlaprivatkund.ica.se",
                    NavMeat = "Kött, Chark & Fågel",
                    NavDairy = "Mejeri & Ost",
                    NavFruitAndVegetables = "Frukt & Grönt",
                    NavFishAndSeafood = "Fisk & Skaldjur",
                    NavBreadAndCookies = "Bröd & Kakor",
                    NavVegetarian = "Vegetariskt",
                    NavReadyMeals = "Färdigmat",
                    NavKids = "Barn",
                    NavIceCreamCandyAndSnacks = "Glass, Godis & Snacks",
                    NavBeverage = "Dryck",
                    NavPantry = "Skafferi",
                    NavFrozen = "Fyst",
                    NavTobacco = "Tobak",
                    NavAnimals = "Djur",
                    NavHomeAndCleaning = "Städ, Tvätt & Papper",
                    NavHealth = "Apotek, Skönhet & Hälsa",
                },
                new Store()
                {
                    Id = 2,
                    Name = "willys",
                    Website = "https://www.willys.se/",
                    NavMeat = "/sortiment/kott-chark-och-fagel",
                    NavDairy = "/sortiment/mejeri-ost-och-agg",
                    NavFruitAndVegetables = "/sortiment/frukt-och-gront",
                    NavFishAndSeafood = "/sortiment/fisk-och-skaldjur",
                    NavBreadAndCookies = "/sortiment/brod-och-kakor",
                    NavVegetarian = "/sortiment/vegetariskt",
                    NavReadyMeals = "/sortiment/fardigmat",
                    NavKids = "/sortiment/barn",
                    NavIceCreamCandyAndSnacks = "/sortiment/glass-godis-och-snacks",
                    NavBeverage = "/sortiment/dryck",
                    NavPantry = "/sortiment/skafferi",
                    NavFrozen = "/sortiment/fryst",
                    NavTobacco = "/sortiment/tobak",
                    NavAnimals = "/sortiment/djur",
                    NavHomeAndCleaning = "/sortiment/hem-och-stad",
                    NavHealth = "/sortiment/halsa-och-skonhet",
                    NavPharmacy = "/sortiment/apotek"
                },
                new Store()
                {
                    Id = 3,
                    Name = "coop",
                    Website = "https://www.coop.se/handla/varor/",
                    NavMeat = "kott-fagel-chark",
                    NavDairy = "mejeri-agg",
                    NavCheese = "ost",
                    NavFruitAndVegetables = "frukt-gronsaker",
                    NavFishAndSeafood = "fisk-skaldjur",
                    NavBreadAndCookies = "brod-bageri",
                    NavVegetarian = "vegetariskt",
                    NavReadyMeals = "fardigmat-mellanmal",
                    NavKids = "barn",
                    NavIceCreamCandyAndSnacks = "godis-glass-snacks",
                    NavBeverage = "dryck",
                    NavPantry = "skafferi",
                    NavFrozen = "frys",
                    NavTobacco = "tobak",
                    NavAnimals = "djurmat-tillbehor",
                    NavHomeAndCleaning = "hushall",
                    NavHealth = "halsa-tillskott",
                    NavHygien = "skonhet-hygien"
                },
                new Store()
                {
                    Id = 4,
                    Name = "citygross",
                    Website = "https://www.citygross.se/",
                    NavMeat = "/matvaror/kott-och-fagel",
                    NavChark = "/matvaror/chark",
                    NavFruitAndVegetables = "/matvaror/frukt-och-gront",
                    NavDairy = "/matvaror/mejeri-ost-och-agg",
                    NavPantry = "/matvaror/skafferiet",
                    NavFrozen = "/matvaror/fryst",
                    NavBreadAndCookies = "/matvaror/brod-och-bageri",
                    NavHomeAndCleaning = "/matvaror/hushall",
                    NavIceCreamCandyAndSnacks = "/matvaror/godis",
                    NavSnacks = "/matvaror/snacks",
                    NavBeverage = "/matvaror/dryck",
                    NavTobacco = "/matvaror/tobak",
                    NavAnimals = "/matvaror/husdjur",
                    NavHealth = "/matvaror/halsa",
                    NavHygien = "/matvaror/skonhet-och-hygien",
                    NavFishAndSeafood = "/matvaror/fisk-och-skaldjur",
                    NavReadyMeals = "/matvaror/kyld-fardigmat",
                    NavVegetarian = "/matvaror/vegetariskt",
                    NavKids = "/matvaror/barn",
                    NavPharmacy = "/matvaror/apotek-och-receptfria-lakemedel"
                },
                new Store()
                {
                    Id = 5,
                    Name = "hemkop",
                    Website = "https://www.hemkop.se/",
                    NavMeat = "sortiment/kott-chark-och-fagel",
                    NavDairy = "sortiment/mejeri-ost-och-agg",
                    NavFruitAndVegetables = "sortiment/frukt-och-gront",
                    NavFishAndSeafood = "sortiment/fisk-och-skaldjur",
                    NavBreadAndCookies = "sortiment/brod-och-kakor",
                    NavVegetarian = "sortiment/vegetariskt",
                    NavReadyMeals = "sortiment/fardigmat",
                    NavKids = "sortiment/barn",
                    NavIceCreamCandyAndSnacks = "sortiment/glass-godis-och-snacks",
                    NavBeverage = "sortiment/dryck",
                    NavPantry = "sortiment/skafferi",
                    NavFrozen = "sortiment/fryst",
                    NavTobacco = "sortiment/tobak",
                    NavAnimals = "sortiment/djur",
                    NavHomeAndCleaning = "sortiment/hem-och-stad",
                    NavHealth = "sortiment/halsa-och-skonhet",
                    NavPharmacy = "sortiment/apotek",
                }
            );
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Kött"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Mejeri"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Frukt"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Skafferi"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Fryst"
                },
                new Category()
                {
                    Id = 6,
                    Name = "Bröd"
                },
                new Category()
                {
                    Id = 7,
                    Name = "Fisk"
                }, new Category()
                {
                    Id = 8,
                    Name = "Vegetariskt"
                }, new Category()
                {
                    Id = 9,
                    Name = "Snacks"
                }, new Category()
                {
                    Id = 10,
                    Name = "Dryck"
                }, new Category()
                {
                    Id = 11,
                    Name = "Färdigmat"
                }, new Category()
                {
                    Id = 12,
                    Name = "Barn"
                }, new Category()
                {
                    Id = 13,
                    Name = "Hem"
                }, new Category()
                {
                    Id = 14,
                    Name = "Hälsa"
                }, new Category()
                {
                    Id = 15,
                    Name = "Apotek"
                }, new Category()
                {
                    Id = 16,
                    Name = "Djur"
                }, new Category()
                {
                    Id = 17,
                    Name = "Tobak"
                }, new Category()
                {
                    Id = 18,
                    Name = "Trädgård"
                }
            );

        }
    }
}