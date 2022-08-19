using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class DeletedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bundle_users_owner_id",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropForeignKey(
                name: "fk_bundle_rate_users_user_id",
                schema: "eart",
                table: "bundle_rate");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_bundle_users_user_id",
                schema: "eart",
                table: "follow_bundle");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_nft_users_user_id",
                schema: "eart",
                table: "follow_nft");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_user_users_followed_user_id",
                schema: "eart",
                table: "follow_user");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_user_users_user_id",
                schema: "eart",
                table: "follow_user");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_users_creator_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_users_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_like_users_user_id",
                schema: "eart",
                table: "nft_like");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_rate_users_user_id",
                schema: "eart",
                table: "nft_rate");

            migrationBuilder.DropForeignKey(
                name: "fk_price_offer_users_user_id",
                schema: "eart",
                table: "price_offer");

            migrationBuilder.DropTable(
                name: "bid_price",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_sale",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_transaction",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "user_deposit",
                schema: "eart");

            migrationBuilder.DropColumn(
                name: "server_balance",
                schema: "eart",
                table: "user");

            migrationBuilder.AddForeignKey(
                name: "fk_bundle_system_users_owner_id",
                schema: "eart",
                table: "bundle",
                column: "owner_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bundle_rate_system_users_user_id",
                schema: "eart",
                table: "bundle_rate",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_bundle_system_users_user_id",
                schema: "eart",
                table: "follow_bundle",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_nft_system_users_user_id",
                schema: "eart",
                table: "follow_nft",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_user_system_users_followed_user_id",
                schema: "eart",
                table: "follow_user",
                column: "followed_user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_user_system_users_user_id",
                schema: "eart",
                table: "follow_user",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "fk_nft_like_system_users_user_id",
                schema: "eart",
                table: "nft_like",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_rate_system_users_user_id",
                schema: "eart",
                table: "nft_rate",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_price_offer_system_users_user_id",
                schema: "eart",
                table: "price_offer",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bundle_system_users_owner_id",
                schema: "eart",
                table: "bundle");

            migrationBuilder.DropForeignKey(
                name: "fk_bundle_rate_system_users_user_id",
                schema: "eart",
                table: "bundle_rate");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_bundle_system_users_user_id",
                schema: "eart",
                table: "follow_bundle");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_nft_system_users_user_id",
                schema: "eart",
                table: "follow_nft");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_user_system_users_followed_user_id",
                schema: "eart",
                table: "follow_user");

            migrationBuilder.DropForeignKey(
                name: "fk_follow_user_system_users_user_id",
                schema: "eart",
                table: "follow_user");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_system_users_creator_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_system_users_user_id",
                schema: "eart",
                table: "nft");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_like_system_users_user_id",
                schema: "eart",
                table: "nft_like");

            migrationBuilder.DropForeignKey(
                name: "fk_nft_rate_system_users_user_id",
                schema: "eart",
                table: "nft_rate");

            migrationBuilder.DropForeignKey(
                name: "fk_price_offer_system_users_user_id",
                schema: "eart",
                table: "price_offer");

            migrationBuilder.AddColumn<long>(
                name: "server_balance",
                schema: "eart",
                table: "user",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "bid_price",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "nft_sale",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_purchase = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_of_set = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sale_contract_address = table.Column<string>(type: "text", nullable: true),
                    transaction_hash_purchase = table.Column<string>(type: "text", nullable: true),
                    transaction_hash_set = table.Column<string>(type: "text", nullable: true),
                    wallet_bought = table.Column<string>(type: "text", nullable: true),
                    wallet_set = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_sale_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_sale_nft_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nft_transaction",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_transaction = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    from_wallet = table.Column<string>(type: "text", nullable: true),
                    to_wallet = table.Column<string>(type: "text", nullable: true),
                    transaction_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_transaction_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_transaction_nft_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_deposit",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deposit_value = table.Column<long>(type: "bigint", nullable: false),
                    deposit_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_deposit_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_deposit_user_user_id",
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
                name: "ix_nft_sale_nft_id",
                schema: "eart",
                table: "nft_sale",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_transaction_nft_id",
                schema: "eart",
                table: "nft_transaction",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_deposit_user_id",
                schema: "eart",
                table: "user_deposit",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_bundle_users_owner_id",
                schema: "eart",
                table: "bundle",
                column: "owner_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bundle_rate_users_user_id",
                schema: "eart",
                table: "bundle_rate",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_bundle_users_user_id",
                schema: "eart",
                table: "follow_bundle",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_nft_users_user_id",
                schema: "eart",
                table: "follow_nft",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_user_users_followed_user_id",
                schema: "eart",
                table: "follow_user",
                column: "followed_user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_follow_user_users_user_id",
                schema: "eart",
                table: "follow_user",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_users_creator_id",
                schema: "eart",
                table: "nft",
                column: "creator_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_users_user_id",
                schema: "eart",
                table: "nft",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_nft_like_users_user_id",
                schema: "eart",
                table: "nft_like",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_nft_rate_users_user_id",
                schema: "eart",
                table: "nft_rate",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_price_offer_users_user_id",
                schema: "eart",
                table: "price_offer",
                column: "user_id",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
