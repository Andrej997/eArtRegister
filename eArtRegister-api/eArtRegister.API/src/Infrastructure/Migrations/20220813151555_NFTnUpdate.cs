using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_users_owner_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropIndex(
                name: "ix_nft_owner_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "owner_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "eart",
                table: "nft",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_nft_user_id",
                schema: "eart",
                table: "nft",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_users_user_id",
                schema: "eart",
                table: "nft",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nft_users_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropIndex(
                name: "ix_nft_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                schema: "eart",
                table: "nft",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_nft_owner_id",
                schema: "eart",
                table: "nft",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_users_owner_id",
                schema: "eart",
                table: "nft",
                column: "owner_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
