using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTNewFields4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "token_standard",
                schema: "eart",
                table: "nft",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "nft_purchase",
                schema: "eart",
                columns: table => new
                {
                    address = table.Column<string>(type: "text", nullable: false),
                    contract = table.Column<string>(type: "text", nullable: true),
                    abi = table.Column<string>(type: "text", nullable: true),
                    bytecode = table.Column<string>(type: "text", nullable: true),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entire_amount = table.Column<bool>(type: "boolean", nullable: false),
                    repayment_in_installments = table.Column<bool>(type: "boolean", nullable: false),
                    auction = table.Column<bool>(type: "boolean", nullable: false),
                    amount_in_eth = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    days_on_sale = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    min_participation = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_purchase_pkey", x => x.address);
                    table.ForeignKey(
                        name: "fk_nft_purchase_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nft_purchase_nft_id",
                schema: "eart",
                table: "nft_purchase",
                column: "nft_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nft_purchase",
                schema: "eart");

            migrationBuilder.DropColumn(
                name: "token_standard",
                schema: "eart",
                table: "nft");
        }
    }
}
