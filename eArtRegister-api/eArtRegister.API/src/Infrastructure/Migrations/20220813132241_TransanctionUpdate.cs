using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class TransanctionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_transaction_bundle_bundle_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropIndex(
                name: "ix_nft_transaction_bundle_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropColumn(
                name: "bundle_id",
                schema: "eart",
                table: "nft_transaction");

            migrationBuilder.DropColumn(
                name: "contact_address",
                schema: "eart",
                table: "nft_transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "bundle_id",
                schema: "eart",
                table: "nft_transaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "contact_address",
                schema: "eart",
                table: "nft_transaction",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_nft_transaction_bundle_id",
                schema: "eart",
                table: "nft_transaction",
                column: "bundle_id");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_transaction_bundle_bundle_id",
                schema: "eart",
                table: "nft_transaction",
                column: "bundle_id",
                principalSchema: "eart",
                principalTable: "bundle",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
