using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTDeleteFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "block_hash",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "block_number",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "cumulative_gas_used",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "effective_gas_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "from",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "gas_used",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "mint_status",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "to",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "transaction_index",
                schema: "eart",
                table: "nft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "block_hash",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "block_number",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "cumulative_gas_used",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "effective_gas_price",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "from",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "gas_used",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "mint_status",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "to",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "transaction_index",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
