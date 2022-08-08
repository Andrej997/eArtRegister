using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NftTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_wallet_users_user_id",
                schema: "eart",
                table: "wallet");

            migrationBuilder.CreateTable(
                name: "nft_transaction",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    bundle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_address = table.Column<string>(type: "text", nullable: true),
                    from_wallet = table.Column<string>(type: "text", nullable: true),
                    to_wallet = table.Column<string>(type: "text", nullable: true),
                    date_of_transaction = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_transaction_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_transaction_bundles_bundle_id",
                        column: x => x.bundle_id,
                        principalSchema: "eart",
                        principalTable: "bundle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_transaction_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nft_transaction_bundle_id",
                schema: "eart",
                table: "nft_transaction",
                column: "bundle_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_transaction_nft_id",
                schema: "eart",
                table: "nft_transaction",
                column: "nft_id");

            migrationBuilder.AddForeignKey(
                name: "fk_wallet_user_user_id",
                schema: "eart",
                table: "wallet",
                column: "user_is",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_wallet_user_user_id",
                schema: "eart",
                table: "wallet");

            migrationBuilder.DropTable(
                name: "nft_transaction",
                schema: "eart");

            migrationBuilder.AddForeignKey(
                name: "fk_wallet_users_user_id",
                schema: "eart",
                table: "wallet",
                column: "user_is",
                principalSchema: "eart",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
