using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrisApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentComparePrice",
                table: "Products",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProdCode",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasDiscount",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentComparePrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProdCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WasDiscount",
                table: "Products");
        }
    }
}
