using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "eart");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "category",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "keys",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    use = table.Column<string>(type: "text", nullable: true),
                    algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_x509certificate = table.Column<bool>(type: "boolean", nullable: false),
                    data_protected = table.Column<bool>(type: "boolean", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nft_price_history",
                schema: "eart",
                columns: table => new
                {
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    date_of_price = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "nft_status",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_status_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id);
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
                    date_added_wallet = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email_notification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bundle",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<long>(type: "bigint", nullable: false),
                    is_observable = table.Column<bool>(type: "boolean", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bundle_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_bundle_users_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follow_user",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followed_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    follow_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("follow_user_pkey", x => new { x.user_id, x.followed_user_id });
                    table.ForeignKey(
                        name: "fk_follow_user_users_followed_user_id",
                        column: x => x.followed_user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_follow_user_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_portal_notification",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    date_of_notification = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    seen = table.Column<bool>(type: "boolean", nullable: false),
                    seen_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_portal_notification_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_portal_notification_user_user_id",
                        column: x => x.user_id,
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
                    table.PrimaryKey("pk_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_role_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "eart",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "follow_bundle",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bundle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    follow_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("follow_bundle_pkey", x => new { x.user_id, x.bundle_id });
                    table.ForeignKey(
                        name: "fk_follow_bundle_bundle_bundle_id",
                        column: x => x.bundle_id,
                        principalSchema: "eart",
                        principalTable: "bundle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_follow_bundle_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nft",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ipfs_id = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<long>(type: "bigint", nullable: false),
                    status_id = table.Column<string>(type: "text", nullable: true),
                    bundle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    minted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    current_price = table.Column<double>(type: "double precision", nullable: false),
                    current_price_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_royality = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_nft_bundle_bundle_id",
                        column: x => x.bundle_id,
                        principalSchema: "eart",
                        principalTable: "bundle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_nft_statuses_status_id",
                        column: x => x.status_id,
                        principalSchema: "eart",
                        principalTable: "nft_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_users_creator_id",
                        column: x => x.creator_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_users_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "follow_nft",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    follow_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("follow_nft_pkey", x => new { x.user_id, x.nft_id });
                    table.ForeignKey(
                        name: "fk_follow_nft_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_follow_nft_users_user_id",
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
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nft_category", x => new { x.nft_id, x.category_id });
                    table.ForeignKey(
                        name: "fk_nft_category_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "eart",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_category_nf_ts_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nft_like",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    liked_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nft_like_pkey", x => new { x.user_id, x.nft_id });
                    table.ForeignKey(
                        name: "fk_nft_like_nft_nft_id",
                        column: x => x.nft_id,
                        principalSchema: "eart",
                        principalTable: "nft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_nft_like_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "eart",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "nft_status",
                column: "id",
                values: new object[]
                {
                    "MINTED",
                    "NOT_ON_SALE",
                    "ON_SALE",
                    "PENDING",
                    "SOLD",
                    "APPROVED",
                    "WAITING_FOR_APPROVAL"
                });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "roles",
                columns: new[] { "id", "name" },
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
                columns: new[] { "id", "date_added_wallet", "email", "email_notification", "is_deleted", "metamask_wallet", "modified_by", "modified_on", "name", "password", "surname", "username" },
                values: new object[] { new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6"), null, "andrej.km997@gmail.com", true, false, null, null, null, "Admin", "$2a$11$T0F92aa6MyQHNYJERrUzge4Teh5QsO9GljSREDDuW/Y8p3YHX02Ra", "Admin", "admin" });

            migrationBuilder.InsertData(
                schema: "eart",
                table: "user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { 1L, new Guid("09ff8406-6775-4bf9-a5b1-d37510cc65e6") });

            migrationBuilder.CreateIndex(
                name: "ix_bundle_owner_id",
                schema: "eart",
                table: "bundle",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_follow_bundle_bundle_id",
                schema: "eart",
                table: "follow_bundle",
                column: "bundle_id");

            migrationBuilder.CreateIndex(
                name: "ix_follow_nft_nft_id",
                schema: "eart",
                table: "follow_nft",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_follow_user_followed_user_id",
                schema: "eart",
                table: "follow_user",
                column: "followed_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_keys_use",
                table: "keys",
                column: "use");

            migrationBuilder.CreateIndex(
                name: "ix_nft_bundle_id",
                schema: "eart",
                table: "nft",
                column: "bundle_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_creator_id",
                schema: "eart",
                table: "nft",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_owner_id",
                schema: "eart",
                table: "nft",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_status_id",
                schema: "eart",
                table: "nft",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_category_category_id",
                schema: "eart",
                table: "nft_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_nft_like_nft_id",
                schema: "eart",
                table: "nft_like",
                column: "nft_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_portal_notification_user_id",
                schema: "eart",
                table: "user_portal_notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_role_id",
                schema: "eart",
                table: "user_role",
                column: "role_id");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_NFT() RETURNS trigger as $LC_TRIGGER_AFTER_UPDATE_NFT$\r\nBEGIN\r\n  IF OLD.current_price <> NEW.current_price THEN \r\n    INSERT INTO eart.nft_price_history (\"parent_id\", \"price\", \"date_of_price\") SELECT OLD.id, \r\n    OLD.current_price, \r\n    OLD.current_price_date;\r\n  END IF;\r\nRETURN NEW;\r\nEND;\r\n$LC_TRIGGER_AFTER_UPDATE_NFT$ LANGUAGE plpgsql;\r\nCREATE TRIGGER LC_TRIGGER_AFTER_UPDATE_NFT AFTER UPDATE\r\nON eart.nft\r\nFOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_UPDATE_NFT();");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION LC_TRIGGER_AFTER_UPDATE_NFT() CASCADE;");

            migrationBuilder.DropTable(
                name: "follow_bundle",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "follow_nft",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "follow_user",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "keys");

            migrationBuilder.DropTable(
                name: "nft_category",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_like",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "nft_price_history",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "user_portal_notification",
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
