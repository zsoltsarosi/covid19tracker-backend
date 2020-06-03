using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.VersionUpdate
{
    public partial class VersionUpdateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VersionUpdate",
                columns: table => new
                {
                    Version = table.Column<string>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionUpdate", x => x.Version);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VersionUpdate");
        }
    }
}
