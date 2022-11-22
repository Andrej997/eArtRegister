using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTDeleteFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION LC_TRIGGER_AFTER_UPDATE_NFT() CASCADE;");

            migrationBuilder.DropColumn(
                name: "current_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "current_price_date",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "current_wallet",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "days_to_pay",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "fixed_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "min_bid_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "minimum_participation",
                schema: "eart",
                table: "nft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "current_price",
                schema: "eart",
                table: "nft",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "current_price_date",
                schema: "eart",
                table: "nft",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "current_wallet",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "days_to_pay",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "fixed_price",
                schema: "eart",
                table: "nft",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "min_bid_price",
                schema: "eart",
                table: "nft",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "minimum_participation",
                schema: "eart",
                table: "nft",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_NFT() RETURNS trigger as $LC_TRIGGER_AFTER_UPDATE_NFT$\r\nBEGIN\r\n  IF OLD.current_price <> NEW.current_price THEN \r\n    INSERT INTO eart.nft_price_history (\"parent_id\", \"price\", \"date_of_price\") SELECT OLD.id, \r\n    OLD.current_price, \r\n    OLD.current_price_date;\r\n  END IF;\r\nRETURN NEW;\r\nEND;\r\n$LC_TRIGGER_AFTER_UPDATE_NFT$ LANGUAGE plpgsql;\r\nCREATE TRIGGER LC_TRIGGER_AFTER_UPDATE_NFT AFTER UPDATE\r\nON eart.nft\r\nFOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_UPDATE_NFT();");
        }
    }
}
