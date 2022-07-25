using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "eart");

            migrationBuilder.AlterDatabase()
                //.Annotation("Npgsql:PostgresExtension:dblink", ",,")
                //.Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "bundle",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bundle_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                schema: "eart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nft_status",
                schema: "eart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_status_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "eart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    metamask_wallet = table.Column<string>(type: "text", nullable: true),
                    date_added_wallet = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    email_notification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nft",
                schema: "eart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<long>(type: "bigint", nullable: false),
                    status_id = table.Column<string>(type: "text", nullable: true),
                    bundle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    minted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    current_price = table.Column<double>(type: "double precision", nullable: false),
                    current_price_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creator_royality = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nft_bundle_bundle_id",
                        column: x => x.bundle_id,
                        principalSchema: "eart",
                        principalTable: "bundle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nft_nft_status_status_id",
                        column: x => x.status_id,
                        principalSchema: "eart",
                        principalTable: "nft_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nft_user_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nft_user_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_user_role_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "eart",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nft_category",
                schema: "eart",
                columns: table => new
                {
                    nft_id = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nft_category", x => new { x.nft_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_nft_category_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "eart",
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nft_category_nft_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "nft_status",
                column: "Id",
                values: new object[]
                {
                    "MINTED",
                    "ON_SALE",
                    "NOT_ON_SALE",
                    "PENDING",
                    "SOLD"
                });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "roles",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1L, "Administrator" },
                    { 2L, "Observer" },
                    { 3L, "Buyer" },
                    { 4L, "Seller" }
                });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "user",
                columns: new[] { "id", "date_added_wallet", "email", "email_notification", "IsDeleted", "metamask_wallet", "ModifiedBy", "ModifiedOn", "name", "password", "surname", "username" },
                values: new object[] { new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6"), null, "andrej.km997@gmail.com", true, false, null, null, null, "Admin", "$2a$11$T0F92aa6MyQHNYJERrUzge4Teh5QsO9GljSREDDuW/Y8p3YHX02Ra", "Admin", "admin" });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { 1L, new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6") });

            migrationBuilder.CreateIndex(
                name: "IX_nft_bundle_id",
                schema: "eart",
                table: "nft",
                column: "bundle_id");

            migrationBuilder.CreateIndex(
                name: "IX_nft_creator_id",
                schema: "eart",
                table: "nft",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_nft_owner_id",
                schema: "eart",
                table: "nft",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_nft_status_id",
                schema: "eart",
                table: "nft",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_nft_category_category_id",
                schema: "eart",
                table: "nft_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                schema: "eart",
                table: "user_role",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nft_category",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "category",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "bundle",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_status",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "user",
                schema: "eart");
        }
    }
}
