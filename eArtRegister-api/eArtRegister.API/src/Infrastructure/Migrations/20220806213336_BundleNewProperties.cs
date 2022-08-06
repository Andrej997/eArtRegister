using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class BundleNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "block_hash",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "from",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_hash",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "block_hash",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "from",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "transaction_hash",
                schema: "eart",
                table: "bundle");
        }
    }
}
