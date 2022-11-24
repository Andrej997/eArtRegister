using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class DepositData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_purchase_nf_ts_nft_id",
                schema: "eart",
                table: "nft_purchase");

            migrationBuilder.RenameColumn(
                name: "date_wallet_added",
                schema: "eart",
                table: "user",
                newName: "deposit_created");

            migrationBuilder.AddColumn<string>(
                name: "deposit_abi",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deposit_address",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deposit_bytecode",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_purchase_nft_nft_id",
                schema: "eart",
                table: "nft_purchase",
                column: "nft_id",
                principalSchema: "eart",
                principalTable: "nft",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_purchase_nft_nft_id",
                schema: "eart",
                table: "nft_purchase");

            migrationBuilder.DropColumn(
                name: "deposit_abi",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "deposit_address",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "deposit_bytecode",
                schema: "eart",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "deposit_created",
                schema: "eart",
                table: "user",
                newName: "date_wallet_added");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_purchase_nf_ts_nft_id",
                schema: "eart",
                table: "nft_purchase",
                column: "nft_id",
                principalSchema: "eart",
                principalTable: "nft",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
