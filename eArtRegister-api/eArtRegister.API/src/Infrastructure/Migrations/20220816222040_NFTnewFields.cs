using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTnewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "days_to_pay",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "minimum_participation",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "days_to_pay",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "minimum_participation",
                schema: "eart",
                table: "nft");
        }
    }
}
