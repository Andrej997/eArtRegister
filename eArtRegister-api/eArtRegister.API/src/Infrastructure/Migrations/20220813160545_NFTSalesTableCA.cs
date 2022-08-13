using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTSalesTableCA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sale_contract_address",
                schema: "eart",
                table: "nft_sale",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sale_contract_address",
                schema: "eart",
                table: "nft_sale");
        }
    }
}
