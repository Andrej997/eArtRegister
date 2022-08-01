using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "eart",
                table: "user_role",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { 1L, new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6") });

            migrationBuilder.DeleteData(
                schema: "eart",
                table: "user",
                keyColumn: "id",
                keyValue: new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6"));

            migrationBuilder.DropColumn(
                name: "email",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "name",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "password",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "surname",
                schema: "eart",
                table: "user");

            migrationBuilder.DropColumn(
                name: "username",
                schema: "eart",
                table: "user");

            migrationBuilder.AddColumn<bool>(
                name: "fixed_price",
                schema: "eart",
                table: "nft",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
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

            migrationBuilder.AddColumn<string>(
                name: "modified_by",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_on",
                schema: "eart",
                table: "nft",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bid_price",
                schema: "eart",
                columns: table => new
                {
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bid = table.Column<double>(type: "double precision", nullable: false),
                    bid_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bid_price_pkey", x => new { x.user_id, x.nft_id });
                    table.ForeignKey(
                        name: "fk_bid_price_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bid_price_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bundle_rate",
                schema: "eart",
                columns: table => new
                {
                    bundle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<double>(type: "double precision", nullable: false),
                    date_of_rate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bundle_rate_pkey", x => new { x.user_id, x.bundle_id });
                    table.ForeignKey(
                        name: "fk_bundle_rate_bundles_bundle_id",
                        column: x => x.bundle_id,
                        principalSchema: "eart",
                        principalTable: "bundle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bundle_rate_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nft_rate",
                schema: "eart",
                columns: table => new
                {
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<double>(type: "double precision", nullable: false),
                    date_of_rate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_rate_pkey", x => new { x.user_id, x.nft_id });
                    table.ForeignKey(
                        name: "fk_nft_rate_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_rate_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "price_offer",
                schema: "eart",
                columns: table => new
                {
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    offer = table.Column<double>(type: "double precision", nullable: false),
                    date_of_offer = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("price_offer_pkey", x => new { x.user_id, x.nft_id });
                    table.ForeignKey(
                        name: "fk_price_offer_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_price_offer_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bid_price_nft_id",
                schema: "eart",
                table: "bid_price",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_bundle_rate_bundle_id",
                schema: "eart",
                table: "bundle_rate",
                column: "bundle_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_rate_nft_id",
                schema: "eart",
                table: "nft_rate",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_price_offer_nft_id",
                schema: "eart",
                table: "price_offer",
                column: "nft_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bid_price",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "bundle_rate",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_rate",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "price_offer",
                schema: "eart");

            migrationBuilder.DropColumn(
                name: "fixed_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "min_bid_price",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "modified_by",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropColumn(
                name: "modified_on",
                schema: "eart",
                table: "nft");

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "surname",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                schema: "eart",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "eart",
                table: "user",
                columns: new[] { "id", "date_added_wallet", "email", "email_notification", "is_deleted", "metamask_wallet", "modified_by", "modified_on", "name", "password", "surname", "username" },
                values: new object[] { new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6"), null, "andrej.km997@gmail.com", true, false, null, null, null, "Admin", "$2a$11$T0F92aa6MyQHNYJERrUzge4Teh5QsO9GljSREDDuW/Y8p3YHX02Ra", "Admin", "admin" });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { 1L, new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6") });
        }
    }
}
