using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class UpdateRemovedKC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallet",
                schema: "eart");

            migrationBuilder.DropColumn(
                name: "email_notification",
                schema: "eart",
                table: "user");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_wallet_added",
                schema: "eart",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "wallet",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_wallet_added",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "wallet",
                schema: "eart",
                table: "user");

            migrationBuilder.AddColumn<bool>(
                name: "email_notification",
                schema: "eart",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "wallet",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_is = table.Column<Guid>(type: "uuid", nullable: false),
                    date_added_wallet = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    metamask_wallet = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("wallet_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_wallet_user_user_id",
                        column: x => x.user_is,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_wallet_user_id",
                schema: "eart",
                table: "wallet",
                column: "user_is");
        }
    }
}
