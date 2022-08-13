using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTSalesTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nft_sales",
                schema: "eart");

            migrationBuilder.CreateTable(
                name: "nft_sale",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    wallet_set = table.Column<string>(type: "text", nullable: true),
                    wallet_bought = table.Column<string>(type: "text", nullable: true),
                    date_of_set = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_hash_set = table.Column<string>(type: "text", nullable: true),
                    date_of_purchase = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_hash_purchase = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "ix_nft_sale_nft_id",
                schema: "eart",
                table: "nft_sale",
                column: "nft_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nft_sale",
                schema: "eart");

            migrationBuilder.CreateTable(
                name: "nft_sales",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_purchase = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_of_set = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_hash_purchase = table.Column<string>(type: "text", nullable: true),
                    transaction_hash_set = table.Column<string>(type: "text", nullable: true),
                    wallet_bought = table.Column<string>(type: "text", nullable: true),
                    wallet_set = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_sales_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_sales_nft_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nft_sales_nft_id",
                schema: "eart",
                table: "nft_sales",
                column: "nft_id");
        }
    }
}
