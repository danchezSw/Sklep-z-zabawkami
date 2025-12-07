using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewProduct5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "ProductColorVariants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "ProductColorVariants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
