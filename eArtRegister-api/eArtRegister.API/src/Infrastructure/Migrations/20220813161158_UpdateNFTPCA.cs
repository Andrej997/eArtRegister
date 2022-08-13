using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class UpdateNFTPCA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "purchase_contract",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "purchase_contract",
                schema: "eart",
                table: "nft");
        }
    }
}
