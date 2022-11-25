using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTPurchaseUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_in_eth",
                schema: "eart",
                table: "nft_purchase");

            migrationBuilder.DropColumn(
                name: "days_on_sale",
                schema: "eart",
                table: "nft_purchase");

            migrationBuilder.DropColumn(
                name: "min_participation",
                schema: "eart",
                table: "nft_purchase");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "amount_in_eth",
                schema: "eart",
                table: "nft_purchase",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "days_on_sale",
                schema: "eart",
                table: "nft_purchase",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<double>(
                name: "min_participation",
                schema: "eart",
                table: "nft_purchase",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
