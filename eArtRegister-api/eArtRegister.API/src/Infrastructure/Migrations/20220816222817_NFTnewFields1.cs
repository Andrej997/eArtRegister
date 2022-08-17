using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class NFTnewFields1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "minimum_participation",
                schema: "eart",
                table: "nft",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "minimum_participation",
                schema: "eart",
                table: "nft",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.0);
        }
    }
}
