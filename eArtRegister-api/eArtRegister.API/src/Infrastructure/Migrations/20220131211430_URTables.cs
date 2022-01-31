using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace eArtRegister.API.Infrastructure.Migrations
{
    public partial class URTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "eart");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "eart",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.user_id, x.role_id });
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "eart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            var sql = @"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"" WITH SCHEMA public;

                ALTER TABLE IF EXISTS eart.user_roles
                    ADD CONSTRAINT fk_user_id FOREIGN KEY (user_id)
                    REFERENCES eart.users (id) MATCH SIMPLE
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
                    NOT VALID;

                ALTER TABLE IF EXISTS eart.user_roles
                    ADD CONSTRAINT fk_role_id FOREIGN KEY (role_id)
                    REFERENCES eart.roles (id) MATCH SIMPLE
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
                    NOT VALID;

                INSERT INTO eart.users (username, password_hash, name, surname) VALUES ('admin', 'admin', 'admin', 'admin');

                INSERT INTO eart.roles (name) VALUES ('admin');
                INSERT INTO eart.roles (name) VALUES ('artist');
                INSERT INTO eart.roles (name) VALUES ('user');
            ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roles",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "eart");

            migrationBuilder.DropTable(
                name: "users",
                schema: "eart");
        }
    }
}
