using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTNewFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_system_users_creator_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_system_users_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropIndex(
                name: "ix_nft_creator_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropIndex(
                name: "ix_nft_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "creator_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "creator_royality",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "minted_at",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.RenameColumn(
                name: "purchase_contract",
                schema: "eart",
                table: "nft",
                newName: "ipfs_nft_size");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "eart",
                table: "nft",
                newName: "ipfs_nft_hash");

            migrationBuilder.RenameColumn(
                name: "ipfs_id",
                schema: "eart",
                table: "nft",
                newName: "ipfs_image_size");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "eart",
                table: "nft",
                newName: "ipfs_image_hash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ipfs_nft_size",
                schema: "eart",
                table: "nft",
                newName: "purchase_contract");

            migrationBuilder.RenameColumn(
                name: "ipfs_nft_hash",
                schema: "eart",
                table: "nft",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "ipfs_image_size",
                schema: "eart",
                table: "nft",
                newName: "ipfs_id");

            migrationBuilder.RenameColumn(
                name: "ipfs_image_hash",
                schema: "eart",
                table: "nft",
                newName: "description");

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                schema: "eart",
                table: "nft",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "creator_royality",
                schema: "eart",
                table: "nft",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "minted_at",
                schema: "eart",
                table: "nft",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "eart",
                table: "nft",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_nft_creator_id",
                schema: "eart",
                table: "nft",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_user_id",
                schema: "eart",
                table: "nft",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_system_users_creator_id",
                schema: "eart",
                table: "nft",
                column: "creator_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_system_users_user_id",
                schema: "eart",
                table: "nft",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
