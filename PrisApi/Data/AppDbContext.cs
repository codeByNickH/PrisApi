using PrisApi.Models;
using Microsoft.EntityFrameworkCore;
using PrisApi.Models.Scraping;

namespace PrisApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<ScrapingJob> ScrapingJobs { get; set; }
        public DbSet<ScraperConfig> ScraperConfigs { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     modelBuilder.Entity<StoreLocation>().HasData(
        //         new StoreLocation()
        //         {
        //             Id = 1,
        //             Address = "Handelsgatan 9",
        //             City = "Bollnäs",
        //             PostalCode = 82391,
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 2,
        //             Address = "Aseavägen 1",
        //             City = "Bollnäs",
        //             PostalCode = 82130
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 3,
        //             Address = "Norrlandsvägen 90",
        //             City = "Bollnäs",
        //             PostalCode = 82136
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 4,
        //             Address = "Västanågatan 3",
        //             City = "Alfta",
        //             PostalCode = 82231
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 5,
        //             Address = "Industrigatan 16",
        //             City = "Gävle",
        //             PostalCode = 80283
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 6,
        //             Address = "Ingenjörsgatan 1",
        //             City = "Gävle",
        //             PostalCode = 80293
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 7,
        //             Address = "Södra Kungsvägen 14",
        //             City = "Gävle",
        //             PostalCode = 80257
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 8,
        //             Address = "Lokförargatan 5",
        //             City = "Gävle",
        //             PostalCode = 80322
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 9,
        //             Address = "Valbovägen 307",
        //             City = "Gävle",
        //             District = "Valbo",
        //             PostalCode = 81835
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 10,
        //             Address = "Skogmursvägen 35",
        //             City = "Gävle",
        //             PostalCode = 80269
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 11,
        //             Address = "Ingenjörsgatan 15",
        //             City = "Gävle",
        //             PostalCode = 80293
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 12,
        //             Address = "Fyrisparksvägen 1",
        //             City = "Uppsala",
        //             PostalCode = 75267
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 13,
        //             Address = "Visthusvägen 1",
        //             City = "Uppsala",
        //             PostalCode = 75454
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 14,
        //             Address = "Herrhagsvägen 17",
        //             City = "Uppsala",
        //             PostalCode = 75267
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 15,
        //             Address = "Björkgatan 4",
        //             City = "Uppsala",
        //             PostalCode = 75327
        //         },

        //         new StoreLocation()
        //         {
        //             Id = 16,
        //             Address = "Rapsgatan 1",
        //             City = "Uppsala",
        //             PostalCode = 75323
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 17,
        //             Address = "Stångjärnsgatan 10",
        //             City = "Uppsala",
        //             PostalCode = 75323
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 18,
        //             Address = "Dragarbrunnsgatan 50",
        //             City = "Uppsala",
        //             District = "",
        //             PostalCode = 75321
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 19,
        //             Address = "Stickvägen 7",
        //             City = "Söderhamn",
        //             PostalCode = 82640
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 20,
        //             Address = "Norra Hamngatan 11",
        //             City = "Söderhamn",
        //             PostalCode = 82630
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 21,
        //             Address = "Flöjtvägen 1",
        //             City = "Söderhamn",
        //             PostalCode = 82640
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 22,
        //             Address = "Blockvägen 1",
        //             City = "Hudiksvall",
        //             PostalCode = 82434
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 23,
        //             Address = "Furulundsvägen 2",
        //             City = "Hudiksvall",
        //             PostalCode = 82431
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 24,
        //             Address = "Bryggeriet, Västra Tullgatan 13",
        //             City = "Hudiksvall",
        //             PostalCode = 82430,
        //         },
        //         new StoreLocation()
        //         {
        //             Id = 25,
        //             Address = "Råbyvägen 97",
        //             City = "Uppsala",
        //             PostalCode = 75460
        //         }
        //         );
        //     modelBuilder.Entity<Store>().HasData(
        //         new Store()
        //         {
        //             Id = 1,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 1
        //         },
        //         new Store()
        //         {
        //             Id = 2,
        //             Name = "Willys",
        //             StoreLocationId = 2
        //         },
        //         new Store()
        //         {
        //             Id = 3,
        //             Name = "Stora Coop",
        //             StoreLocationId = 3
        //         },
        //         new Store()
        //         {
        //             Id = 4,
        //             Name = "City Gross",
        //             StoreLocationId = 11
        //         },
        //         new Store()
        //         {
        //             Id = 5,
        //             Name = "Hemköp",
        //             StoreLocationId = 4
        //         },
        //         new Store()
        //         {
        //             Id = 6,
        //             Name = "Ica Kvantum",
        //             StoreLocationId = 5
        //         },
        //         new Store()
        //         {
        //             Id = 7,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 6
        //         },
        //         new Store()
        //         {
        //             Id = 8,
        //             Name = "Willys",
        //             StoreLocationId = 7
        //         },
        //         new Store()
        //         {
        //             Id = 9,
        //             Name = "Willys",
        //             StoreLocationId = 8
        //         },
        //         new Store()
        //         {
        //             Id = 10,
        //             Name = "Stora Coop",
        //             StoreLocationId = 9
        //         },
        //         new Store()
        //         {
        //             Id = 11,
        //             Name = "Coop",
        //             StoreLocationId = 10
        //         },
        //         new Store()
        //         {
        //             Id = 12,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 12
        //         },
        //         new Store()
        //         {
        //             Id = 13,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 13
        //         },
        //         new Store()
        //         {
        //             Id = 14,
        //             Name = "Willys",
        //             StoreLocationId = 14
        //         },
        //         new Store()
        //         {
        //             Id = 15,
        //             Name = "Willys",
        //             StoreLocationId = 15
        //         },
        //         new Store()
        //         {
        //             Id = 16,
        //             Name = "Stora Coop",
        //             StoreLocationId = 16
        //         },
        //         new Store()
        //         {
        //             Id = 17,
        //             Name = "City Gross",
        //             StoreLocationId = 17
        //         },
        //         new Store()
        //         {
        //             Id = 18,
        //             Name = "Hemköp",
        //             StoreLocationId = 18
        //         },
        //         new Store()
        //         {
        //             Id = 19,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 19
        //         },
        //         new Store()
        //         {
        //             Id = 20,
        //             Name = "Ica Supermarket",
        //             StoreLocationId = 20
        //         },
        //         new Store()
        //         {
        //             Id = 21,
        //             Name = "Willys",
        //             StoreLocationId = 21
        //         },
        //         new Store()
        //         {
        //             Id = 22,
        //             Name = "Ica Maxi",
        //             StoreLocationId = 22
        //         },
        //         new Store()
        //         {
        //             Id = 23,
        //             Name = "Stora Coop",
        //             StoreLocationId = 23
        //         },
        //         new Store()
        //         {
        //             Id = 24,
        //             Name = "Hemkop",
        //             StoreLocationId = 24
        //         },
        //         new Store()
        //         {
        //             Id = 25,
        //             Name = "Willys",
        //             StoreLocationId = 25
        //         }
        //     );
        //     modelBuilder.Entity<Category>().HasData(
        //         new Category()
        //         {
        //             Id = 1,
        //             Name = "Kött"
        //         },
        //         new Category()
        //         {
        //             Id = 2,
        //             Name = "Mejeri"
        //         },
        //         new Category()
        //         {
        //             Id = 3,
        //             Name = "Frukt"
        //         },
        //         new Category()
        //         {
        //             Id = 4,
        //             Name = "Skafferi"
        //         },
        //         new Category()
        //         {
        //             Id = 5,
        //             Name = "Fryst"
        //         },
        //         new Category()
        //         {
        //             Id = 6,
        //             Name = "Bröd"
        //         },
        //         new Category()
        //         {
        //             Id = 7,
        //             Name = "Fisk"
        //         }, new Category()
        //         {
        //             Id = 8,
        //             Name = "Vegetariskt"
        //         }, new Category()
        //         {
        //             Id = 9,
        //             Name = "Snacks"
        //         }, new Category()
        //         {
        //             Id = 10,
        //             Name = "Dryck"
        //         }, new Category()
        //         {
        //             Id = 11,
        //             Name = "Färdigmat"
        //         }, new Category()
        //         {
        //             Id = 12,
        //             Name = "Barn"
        //         }, new Category()
        //         {
        //             Id = 13,
        //             Name = "Hem"
        //         }, new Category()
        //         {
        //             Id = 14,
        //             Name = "Hälsa"
        //         }, new Category()
        //         {
        //             Id = 15,
        //             Name = "Apotek"
        //         }, new Category()
        //         {
        //             Id = 16,
        //             Name = "Djur"
        //         }, new Category()
        //         {
        //             Id = 17,
        //             Name = "Tobak"
        //         }, new Category()
        //         {
        //             Id = 18,
        //             Name = "Trädgård"
        //         }
        //     );
        //     modelBuilder.Entity<ScraperConfig>().HasData(
        //         new ScraperConfig()
        //         {
        //             Id = 1,
        //             StoreName = "ica",
        //             BaseUrl = "https://handlaprivatkund.ica.se",
        //             RequestDelayMs = 50,
        //             UseJavaScript = false,
        //             ScraperSelectorId = 1,
        //             ScraperNavigationId = 1
        //         },
        //         new ScraperConfig()
        //         {
        //             Id = 2,
        //             StoreName = "willys",
        //             BaseUrl = "https://www.willys.se/",
        //             RequestDelayMs = 50,
        //             UseJavaScript = false,
        //             ScraperSelectorId = 2,
        //             ScraperNavigationId = 2
        //         },
        //         new ScraperConfig()
        //         {
        //             Id = 3,
        //             StoreName = "coop",
        //             BaseUrl = "https://www.coop.se/handla/varor/",
        //             RequestDelayMs = 50,
        //             UseJavaScript = false,
        //             ScraperSelectorId = 3,
        //             ScraperNavigationId = 3
        //         },
        //         new ScraperConfig()
        //         {
        //             Id = 4,
        //             StoreName = "citygross",
        //             BaseUrl = "https://www.citygross.se/",
        //             RequestDelayMs = 50,
        //             UseJavaScript = false,
        //             ScraperSelectorId = 4,
        //             ScraperNavigationId = 4
        //         },
        //         new ScraperConfig()
        //         {
        //             Id = 5,
        //             StoreName = "hemkop",
        //             BaseUrl = "https://www.hemkop.se/",
        //             RequestDelayMs = 50,
        //             UseJavaScript = false,
        //             ScraperSelectorId = 5,
        //             ScraperNavigationId = 5
        //         }
        //     );
        //     modelBuilder.Entity<ScraperNavigation>().HasData(
        //         new ScraperNavigation()
        //         {
        //             Id = 1,
        //             NavMeat = "Kött, Chark & Fågel",
        //             NavDairy = "Mejeri & Ost",
        //             NavFruitAndVegetables = "Frukt & Grönt",
        //             NavFishAndSeafood = "Fisk & Skaldjur",
        //             NavBreadAndCookies = "Bröd & Kakor",
        //             NavVegetarian = "Vegetariskt",
        //             NavReadyMeals = "Färdigmat",
        //             NavKids = "Barn",
        //             NavIceCreamCandyAndSnacks = "Glass, Godis & Snacks",
        //             NavBeverage = "Dryck",
        //             NavPantry = "Skafferi",
        //             NavFrozen = "Fyst",
        //             NavTobacco = "Tobak",
        //             NavAnimals = "Djur",
        //             NavHomeAndCleaning = "Städ, Tvätt & Papper",
        //             NavHealth = "Apotek, Skönhet & Hälsa",
        //         },
        //         new ScraperNavigation()
        //         {
        //             Id = 2,
        //             NavMeat = "/sortiment/kott-chark-och-fagel",
        //             NavDairy = "/sortiment/mejeri-ost-och-agg",
        //             NavFruitAndVegetables = "/sortiment/frukt-och-gront",
        //             NavFishAndSeafood = "/sortiment/fisk-och-skaldjur",
        //             NavBreadAndCookies = "/sortiment/brod-och-kakor",
        //             NavVegetarian = "/sortiment/vegetariskt",
        //             NavReadyMeals = "/sortiment/fardigmat",
        //             NavKids = "/sortiment/barn",
        //             NavIceCreamCandyAndSnacks = "/sortiment/glass-godis-och-snacks",
        //             NavBeverage = "/sortiment/dryck",
        //             NavPantry = "/sortiment/skafferi",
        //             NavFrozen = "/sortiment/fryst",
        //             NavTobacco = "/sortiment/tobak",
        //             NavAnimals = "/sortiment/djur",
        //             NavHomeAndCleaning = "/sortiment/hem-och-stad",
        //             NavHealth = "/sortiment/halsa-och-skonhet",
        //             NavPharmacy = "/sortiment/apotek"
        //         },
        //         new ScraperNavigation()
        //         {
        //             Id = 3,
        //             NavMeat = "kott-fagel-chark",
        //             NavDairy = "mejeri-agg",
        //             NavCheese = "ost",
        //             NavFruitAndVegetables = "frukt-gronsaker",
        //             NavFishAndSeafood = "fisk-skaldjur",
        //             NavBreadAndCookies = "brod-bageri",
        //             NavVegetarian = "vegetariskt",
        //             NavReadyMeals = "fardigmat-mellanmal",
        //             NavKids = "barn",
        //             NavIceCreamCandyAndSnacks = "godis-glass-snacks",
        //             NavBeverage = "dryck",
        //             NavPantry = "skafferi",
        //             NavFrozen = "frys",
        //             NavTobacco = "tobak",
        //             NavAnimals = "djurmat-tillbehor",
        //             NavHomeAndCleaning = "hushall",
        //             NavHealth = "halsa-tillskott",
        //             NavHygien = "skonhet-hygien"
        //         },
        //         new ScraperNavigation()
        //         {
        //             Id = 4,
        //             NavMeat = "/matvaror/kott-och-fagel",
        //             NavChark = "/matvaror/chark",
        //             NavFruitAndVegetables = "/matvaror/frukt-och-gront",
        //             NavDairy = "/matvaror/mejeri-ost-och-agg",
        //             NavPantry = "/matvaror/skafferiet",
        //             NavFrozen = "/matvaror/fryst",
        //             NavBreadAndCookies = "/matvaror/brod-och-bageri",
        //             NavHomeAndCleaning = "/matvaror/hushall",
        //             NavIceCreamCandyAndSnacks = "/matvaror/godis",
        //             NavSnacks = "/matvaror/snacks",
        //             NavBeverage = "/matvaror/dryck",
        //             NavTobacco = "/matvaror/tobak",
        //             NavAnimals = "/matvaror/husdjur",
        //             NavHealth = "/matvaror/halsa",
        //             NavHygien = "/matvaror/skonhet-och-hygien",
        //             NavFishAndSeafood = "/matvaror/fisk-och-skaldjur",
        //             NavReadyMeals = "/matvaror/kyld-fardigmat",
        //             NavVegetarian = "/matvaror/vegetariskt",
        //             NavKids = "/matvaror/barn",
        //             NavPharmacy = "/matvaror/apotek-och-receptfria-lakemedel"
        //         },
        //         new ScraperNavigation()
        //         {
        //             Id = 5,
        //             NavMeat = "sortiment/kott-chark-och-fagel",
        //             NavDairy = "sortiment/mejeri-ost-och-agg",
        //             NavFruitAndVegetables = "sortiment/frukt-och-gront",
        //             NavFishAndSeafood = "sortiment/fisk-och-skaldjur",
        //             NavBreadAndCookies = "sortiment/brod-och-kakor",
        //             NavVegetarian = "sortiment/vegetariskt",
        //             NavReadyMeals = "sortiment/fardigmat",
        //             NavKids = "sortiment/barn",
        //             NavIceCreamCandyAndSnacks = "sortiment/glass-godis-och-snacks",
        //             NavBeverage = "sortiment/dryck",
        //             NavPantry = "sortiment/skafferi",
        //             NavFrozen = "sortiment/fryst",
        //             NavTobacco = "sortiment/tobak",
        //             NavAnimals = "sortiment/djur",
        //             NavHomeAndCleaning = "sortiment/hem-och-stad",
        //             NavHealth = "sortiment/halsa-och-skonhet",
        //             NavPharmacy = "sortiment/apotek",
        //         }
        //     );
        //     modelBuilder.Entity<ScraperSelector>().HasData(
        //         new ScraperSelector()
        //         {
        //             Id = 1,
        //             CookieBannerSelector = "id=\"onetrust-banner-sdk\"",
        //             RejectCookiesSelector = "id=\"onetrust-reject-all-handler\"",
        //             SearchStoreSelector = "class=\"svelte-vapldk\"",
        //             SelectStoreSelector = "class=\"store-item__column svelte-uogeuo\"",
        //             PickupOptionSelector = "data-automation-id=\"store-selector-view-pickup\"",
        //             CategoryNavSelector = "id=\"nav-menu-button\"",
        //         },
        //         new ScraperSelector()
        //         {
        //             Id = 2,
        //             CookieBannerSelector = "id=\"onetrust-banner-sdk\"",
        //             RejectCookiesSelector = "id=\"onetrust-reject-all-handler\"",
        //             ChooseStoreSelector = "data-testid=\"delivery-picker-toggle\"",
        //             SearchStoreSelector = "placeholder=\"Sök på butik\"",
        //             SelectStoreSelector = "class=\"sc-4b41f1b4-2 cIxYss\"",
        //             PickupOptionSelector = "data-testid=\"delivery-method-pickUpInStore\"",
        //             CloseChooseTabSelector = "data-testid=\"slidein-close-button\"",
        //             CategoryNavSelector = "class=\"sc-5cf2ead7-2 eTtqdC\"",
        //         },
        //         new ScraperSelector()
        //         {
        //             Id = 3,
        //             CookieBannerSelector = "id=\"cmpbox\"",
        //             RejectCookiesSelector = "id=\"cmpwelcomebtnno\"",
        //             ChooseStoreSelector = "class=\"eJadUlKd\"",
        //             SearchStoreSelector = "placeholder=\"Ange ditt postnummer\"",
        //             SearchButtonSelector = "class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"",
        //             SelectStoreSelector = "class=\"yWvaV7fj\"",
        //             PickupOptionSelector = "data-key=\"pickup\"",
        //             CloseChooseTabSelector = "class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"",
        //         },
        //         new ScraperSelector()
        //         {
        //             Id = 4,
        //             CookieBannerSelector = "id=\"CybotCookiebotDialog\"",
        //             RejectCookiesSelector = "id=\"CybotCookiebotDialogBodyButtonDecline\"",
        //             ChooseStoreSelector = "class=\"c-change-delivery-link\"",
        //             SearchStoreSelector = "placeholder=\"Sök butik eller stad\"",
        //             SelectStoreSelector = "//*[@id='root']/div[2]/div/div/div/div[2]/div/div[4]/div",
        //             PickupOptionSelector = "data-automation-id=\"store-selector-view-pickup\"",
        //             CloseChooseTabSelector = "//*[@id='root']/div[2]/div/div/div/div[2]/div/div[5]/button",
        //             CategoryNavSelector = "href=\"/matvaror\"",
        //         },
        //         new ScraperSelector()
        //         {
        //             Id = 5,
        //             CookieBannerSelector = "id=\"onetrust-banner-sdk\"",
        //             RejectCookiesSelector = "id=\"onetrust-reject-all-handler\"",
        //         }
        //     );

        // }
    }
}