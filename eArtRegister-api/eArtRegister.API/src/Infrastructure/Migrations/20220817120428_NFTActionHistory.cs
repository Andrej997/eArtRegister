using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTActionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nft_action_history",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    wallet = table.Column<string>(type: "text", nullable: true),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_action = table.Column<string>(type: "text", nullable: true),
                    event_timestamp = table.Column<long>(type: "bigint", nullable: false),
                    transaction_hash = table.Column<string>(type: "text", nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_action_history_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_action_history_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nft_action_history_nft_id",
                schema: "eart",
                table: "nft_action_history",
                column: "nft_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nft_action_history",
                schema: "eart");
        }
    }
}
