using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrisApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { 1, "Djur", "Dryck", "Bröd & Kakor", null, null, "Mejeri & Ost", "Fisk & Skaldjur", "Fryst", "Frukt & Grönt", "Apotek, Hälsa & Skönhet", "Städ, Tvätt & Papper", null, "Glass, Godis & Snacks", "Barn", "Kött, Chark & Fågel", "Skafferi", null, "Färdigmat", null, "Tobak", "Vegetariskt" },
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
                    { 4, "https://www.citygross.se/", 50, 4, 4, "city gross", false },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ScraperConfigs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScraperConfigs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScraperConfigs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScraperConfigs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScraperConfigs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ScraperNavigation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScraperNavigation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScraperNavigation",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScraperNavigation",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScraperNavigation",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScraperSelector",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScraperSelector",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScraperSelector",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScraperSelector",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScraperSelector",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
