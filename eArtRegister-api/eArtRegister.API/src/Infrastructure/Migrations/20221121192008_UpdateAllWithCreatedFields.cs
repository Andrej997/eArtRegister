using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class UpdateAllWithCreatedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_observable",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "order",
                schema: "eart",
                table: "bundle");

            migrationBuilder.RenameColumn(
                name: "transaction_hash",
                schema: "eart",
                table: "bundle",
                newName: "symbol");

            migrationBuilder.RenameColumn(
                name: "from",
                schema: "eart",
                table: "bundle",
                newName: "custom_root");

            migrationBuilder.RenameColumn(
                name: "contract_address",
                schema: "eart",
                table: "bundle",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "block_hash",
                schema: "eart",
                table: "bundle",
                newName: "contract");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                schema: "eart",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                schema: "eart",
                table: "nft",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "abi",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bytecode",
                schema: "eart",
                table: "bundle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                schema: "eart",
                table: "bundle",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "created_on",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "created_on",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "abi",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "address",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "bytecode",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropColumn(
                name: "created_on",
                schema: "eart",
                table: "bundle");

            migrationBuilder.RenameColumn(
                name: "symbol",
                schema: "eart",
                table: "bundle",
                newName: "transaction_hash");

            migrationBuilder.RenameColumn(
                name: "custom_root",
                schema: "eart",
                table: "bundle",
                newName: "from");

            migrationBuilder.RenameColumn(
                name: "created_by",
                schema: "eart",
                table: "bundle",
                newName: "contract_address");

            migrationBuilder.RenameColumn(
                name: "contract",
                schema: "eart",
                table: "bundle",
                newName: "block_hash");

            migrationBuilder.AddColumn<bool>(
                name: "is_observable",
                schema: "eart",
                table: "bundle",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "order",
                schema: "eart",
                table: "bundle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
