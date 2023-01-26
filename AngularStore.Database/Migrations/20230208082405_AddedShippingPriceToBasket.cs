using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStore.Database.Migrations
{
    public partial class AddedShippingPriceToBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ShippingPrice",
                table: "Baskets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingPrice",
                table: "Baskets");
        }
    }
}
