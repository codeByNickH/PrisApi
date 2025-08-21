using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrisApi.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScraperNavigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NavMeat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavChark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavDairy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavCheese = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavFruitAndVegetables = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavFishAndSeafood = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavVegetarian = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavPantry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavFrozen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavBreadAndCookies = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavIceCreamCandyAndSnacks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavSnacks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavBeverage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavReadyMeals = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavKids = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavHomeAndCleaning = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavHealth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavHygien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavPharmacy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavAnimals = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NavTobacco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScraperNavigation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScraperSelector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CookieBannerSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RejectCookiesSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ChooseStoreSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PickupOptionSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SearchStoreSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SearchButtonSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SelectStoreSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CloseChooseTabSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CategoryNavSelector = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScraperSelector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapingJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProductsScraped = table.Column<int>(type: "int", nullable: false),
                    NewProducts = table.Column<int>(type: "int", nullable: false),
                    UpdatedProducts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapingJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScraperConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RequestDelayMs = table.Column<int>(type: "int", nullable: false),
                    UseJavaScript = table.Column<bool>(type: "bit", nullable: false),
                    ScraperSelectorId = table.Column<int>(type: "int", nullable: false),
                    ScraperNavigationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScraperConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScraperConfigs_ScraperNavigation_ScraperNavigationId",
                        column: x => x.ScraperNavigationId,
                        principalTable: "ScraperNavigation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScraperConfigs_ScraperSelector_ScraperSelectorId",
                        column: x => x.ScraperSelectorId,
                        principalTable: "ScraperSelector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "StoreLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CurrentPrice = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    ComparePrice = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MemberDiscount = table.Column<bool>(type: "bit", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Size = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxQuantity = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    MinQuantity = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    ComparePrice = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    CompareUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WasDiscount = table.Column<bool>(type: "bit", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceHistories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Kött" },
                    { 2, "Mejeri" },
                    { 3, "Frukt" },
                    { 4, "Skafferi" },
                    { 5, "Fryst" },
                    { 6, "Bröd" },
                    { 7, "Fisk" },
                    { 8, "Vegetariskt" },
                    { 9, "Snacks" },
                    { 10, "Dryck" },
                    { 11, "Färdigmat" },
                    { 12, "Barn" },
                    { 13, "Hem" },
                    { 14, "Hälsa" },
                    { 15, "Apotek" },
                    { 16, "Djur" },
                    { 17, "Tobak" },
                    { 18, "Trädgård" }
                });

            migrationBuilder.InsertData(
                table: "ScraperNavigation",
                columns: new[] { "Id", "NavAnimals", "NavBeverage", "NavBreadAndCookies", "NavChark", "NavCheese", "NavDairy", "NavFishAndSeafood", "NavFrozen", "NavFruitAndVegetables", "NavHealth", "NavHomeAndCleaning", "NavHygien", "NavIceCreamCandyAndSnacks", "NavKids", "NavMeat", "NavPantry", "NavPharmacy", "NavReadyMeals", "NavSnacks", "NavTobacco", "NavVegetarian" },
                values: new object[,]
                {
                    { 1, "Djur", "Dryck", "Bröd & Kakor", null, null, "Mejeri & Ost", "Fisk & Skaldjur", "Fyst", "Frukt & Grönt", "Apotek, Skönhet & Hälsa", "Städ, Tvätt & Papper", null, "Glass, Godis & Snacks", "Barn", "Kött, Chark & Fågel", "Skafferi", null, "Färdigmat", null, "Tobak", "Vegetariskt" },
                    { 2, "/sortiment/djur", "/sortiment/dryck", "/sortiment/brod-och-kakor", null, null, "/sortiment/mejeri-ost-och-agg", "/sortiment/fisk-och-skaldjur", "/sortiment/fryst", "/sortiment/frukt-och-gront", "/sortiment/halsa-och-skonhet", "/sortiment/hem-och-stad", null, "/sortiment/glass-godis-och-snacks", "/sortiment/barn", "/sortiment/kott-chark-och-fagel", "/sortiment/skafferi", "/sortiment/apotek", "/sortiment/fardigmat", null, "/sortiment/tobak", "/sortiment/vegetariskt" },
                    { 3, "djurmat-tillbehor", "dryck", "brod-bageri", null, "ost", "mejeri-agg", "fisk-skaldjur", "frys", "frukt-gronsaker", "halsa-tillskott", "hushall", "skonhet-hygien", "godis-glass-snacks", "barn", "kott-fagel-chark", "skafferi", null, "fardigmat-mellanmal", null, "tobak", "vegetariskt" },
                    { 4, "/matvaror/husdjur", "/matvaror/dryck", "/matvaror/brod-och-bageri", "/matvaror/chark", null, "/matvaror/mejeri-ost-och-agg", "/matvaror/fisk-och-skaldjur", "/matvaror/fryst", "/matvaror/frukt-och-gront", "/matvaror/halsa", "/matvaror/hushall", "/matvaror/skonhet-och-hygien", "/matvaror/godis", "/matvaror/barn", "/matvaror/kott-och-fagel", "/matvaror/skafferiet", "/matvaror/apotek-och-receptfria-lakemedel", "/matvaror/kyld-fardigmat", "/matvaror/snacks", "/matvaror/tobak", "/matvaror/vegetariskt" },
                    { 5, "sortiment/djur", "sortiment/dryck", "sortiment/brod-och-kakor", null, null, "sortiment/mejeri-ost-och-agg", "sortiment/fisk-och-skaldjur", "sortiment/fryst", "sortiment/frukt-och-gront", "sortiment/halsa-och-skonhet", "sortiment/hem-och-stad", null, "sortiment/glass-godis-och-snacks", "sortiment/barn", "sortiment/kott-chark-och-fagel", "sortiment/skafferi", "sortiment/apotek", "sortiment/fardigmat", null, "sortiment/tobak", "sortiment/vegetariskt" }
                });

            migrationBuilder.InsertData(
                table: "ScraperSelector",
                columns: new[] { "Id", "CategoryNavSelector", "ChooseStoreSelector", "CloseChooseTabSelector", "CookieBannerSelector", "PickupOptionSelector", "RejectCookiesSelector", "SearchButtonSelector", "SearchStoreSelector", "SelectStoreSelector" },
                values: new object[,]
                {
                    { 1, "id=\"nav-menu-button\"", null, null, "id=\"onetrust-banner-sdk\"", "data-automation-id=\"store-selector-view-pickup\"", "id=\"onetrust-reject-all-handler\"", null, "class=\"svelte-vapldk\"", "class=\"store-item__column svelte-uogeuo\"" },
                    { 2, "class=\"sc-5cf2ead7-2 eTtqdC\"", "data-testid=\"delivery-picker-toggle\"", "data-testid=\"slidein-close-button\"", "id=\"onetrust-banner-sdk\"", "data-testid=\"delivery-method-pickUpInStore\"", "id=\"onetrust-reject-all-handler\"", null, "placeholder=\"Sök på butik\"", "class=\"sc-4b41f1b4-2 cIxYss\"" },
                    { 3, null, "class=\"eJadUlKd\"", "class=\"gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt\"", "id=\"cmpbox\"", "data-key=\"pickup\"", "id=\"cmpwelcomebtnno\"", "class=\"gUGSFhfR UhM7Xoea ucdesrxw qfkHWAKt\"", "placeholder=\"Ange ditt postnummer\"", "class=\"yWvaV7fj\"" },
                    { 4, "href=\"/matvaror\"", "class=\"c-change-delivery-link\"", "//*[@id='root']/div[2]/div/div/div/div[2]/div/div[5]/button", "id=\"CybotCookiebotDialog\"", "data-automation-id=\"store-selector-view-pickup\"", "id=\"CybotCookiebotDialogBodyButtonDecline\"", null, "placeholder=\"Sök butik eller stad\"", "//*[@id='root']/div[2]/div/div/div/div[2]/div/div[4]/div" },
                    { 5, null, null, null, "id=\"onetrust-banner-sdk\"", null, "id=\"onetrust-reject-all-handler\"", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "StoreLocations",
                columns: new[] { "Id", "Address", "City", "District", "PostalCode" },
                values: new object[,]
                {
                    { 1, "Handelsgatan 9", "Bollnäs", null, 82391 },
                    { 2, "Aseavägen 1", "Bollnäs", null, 82130 },
                    { 3, "Norrlandsvägen 90", "Bollnäs", null, 82136 },
                    { 4, "Västanågatan 3", "Alfta", null, 82231 },
                    { 5, "Industrigatan 16", "Gävle", null, 80283 },
                    { 6, "Ingenjörsgatan 1", "Gävle", null, 80293 },
                    { 7, "Södra Kungsvägen 14", "Gävle", null, 80257 },
                    { 8, "Lokförargatan 5", "Gävle", null, 80322 },
                    { 9, "Valbovägen 307", "Gävle", "Valbo", 81835 },
                    { 10, "Skogmursvägen 35", "Gävle", null, 80269 },
                    { 11, "Ingenjörsgatan 15", "Gävle", null, 80293 },
                    { 12, "Fyrisparksvägen 1", "Uppsala", null, 75267 },
                    { 13, "Visthusvägen 1", "Uppsala", null, 75454 },
                    { 14, "Herrhagsvägen 17", "Uppsala", null, 75267 },
                    { 15, "Björkgatan 4", "Uppsala", null, 75327 },
                    { 16, "Rapsgatan 1", "Uppsala", null, 75323 },
                    { 17, "Stångjärnsgatan 10", "Uppsala", null, 75323 },
                    { 18, "Dragarbrunnsgatan 50", "Uppsala", "", 75321 },
                    { 19, "Stickvägen 7", "Söderhamn", null, 82640 },
                    { 20, "Norra Hamngatan 11", "Söderhamn", null, 82630 },
                    { 21, "Flöjtvägen 1", "Söderhamn", null, 82640 },
                    { 22, "Blockvägen 1", "Hudiksvall", null, 82434 },
                    { 23, "Furulundsvägen 2", "Hudiksvall", null, 82431 },
                    { 24, "Bryggeriet, Västra Tullgatan 13", "Hudiksvall", null, 82430 },
                    { 25, "Råbyvägen 97", "Uppsala", null, 75460 }
                });

            migrationBuilder.InsertData(
                table: "ScraperConfigs",
                columns: new[] { "Id", "BaseUrl", "RequestDelayMs", "ScraperNavigationId", "ScraperSelectorId", "StoreName", "UseJavaScript" },
                values: new object[,]
                {
                    { 1, "https://handlaprivatkund.ica.se", 50, 1, 1, "ica", false },
                    { 2, "https://www.willys.se/", 50, 2, 2, "willys", false },
                    { 3, "https://www.coop.se/handla/varor/", 50, 3, 3, "coop", false },
                    { 4, "https://www.citygross.se/", 50, 4, 4, "citygross", false },
                    { 5, "https://www.hemkop.se/", 50, 5, 5, "hemkop", false }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Name", "StoreLocationId" },
                values: new object[,]
                {
                    { 1, "Ica Maxi", 1 },
                    { 2, "Willys", 2 },
                    { 3, "Stora Coop", 3 },
                    { 4, "City Gross", 11 },
                    { 5, "Hemköp", 4 },
                    { 6, "Ica Kvantum", 5 },
                    { 7, "Ica Maxi", 6 },
                    { 8, "Willys", 7 },
                    { 9, "Willys", 8 },
                    { 10, "Stora Coop", 9 },
                    { 11, "Coop", 10 },
                    { 12, "Ica Maxi", 12 },
                    { 13, "Ica Maxi", 13 },
                    { 14, "Willys", 14 },
                    { 15, "Willys", 15 },
                    { 16, "Stora Coop", 16 },
                    { 17, "City Gross", 17 },
                    { 18, "Hemköp", 18 },
                    { 19, "Ica Maxi", 19 },
                    { 20, "Ica Supermarket", 20 },
                    { 21, "Willys", 21 },
                    { 22, "Ica Maxi", 22 },
                    { 23, "Stora Coop", 23 },
                    { 24, "Hemkop", 24 },
                    { 25, "Willys", 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistories_ProductId",
                table: "PriceHistories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ScraperConfigs_ScraperNavigationId",
                table: "ScraperConfigs",
                column: "ScraperNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScraperConfigs_ScraperSelectorId",
                table: "ScraperConfigs",
                column: "ScraperSelectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreLocationId",
                table: "Stores",
                column: "StoreLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistories");

            migrationBuilder.DropTable(
                name: "ScraperConfigs");

            migrationBuilder.DropTable(
                name: "ScrapingJobs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ScraperNavigation");

            migrationBuilder.DropTable(
                name: "ScraperSelector");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "StoreLocations");
        }
    }
}
