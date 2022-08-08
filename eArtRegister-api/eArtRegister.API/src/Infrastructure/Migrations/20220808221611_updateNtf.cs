using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class updateNtf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_transaction_bundles_bundle_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_transaction_nf_ts_nft_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.RenameColumn(
                name: "order",
                schema: "eart",
                table: "nft",
                newName: "token_id");

            migrationBuilder.AddColumn<string>(
                name: "current_wallet",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_transaction_bundle_bundle_id",
                schema: "eart",
                table: "nft_transaction",
                column: "bundle_id",
                principalSchema: "eart",
                principalTable: "bundle",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_transaction_nft_nft_id",
                schema: "eart",
                table: "nft_transaction",
                column: "nft_id",
                principalSchema: "eart",
                principalTable: "nft",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_transaction_bundle_bundle_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_transaction_nft_nft_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropColumn(
                name: "current_wallet",
                schema: "eart",
                table: "nft");

            migrationBuilder.RenameColumn(
                name: "token_id",
                schema: "eart",
                table: "nft",
                newName: "order");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_transaction_bundles_bundle_id",
                schema: "eart",
                table: "nft_transaction",
                column: "bundle_id",
                principalSchema: "eart",
                principalTable: "bundle",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_transaction_nf_ts_nft_id",
                schema: "eart",
                table: "nft_transaction",
                column: "nft_id",
                principalSchema: "eart",
                principalTable: "nft",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
