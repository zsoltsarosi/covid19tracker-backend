using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.ApiKey
{
    public partial class ApiKeyInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    Owner = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Roles = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Key);
                });

            migrationBuilder.InsertData(
                table: "ApiKeys",
                columns: new[] { "Key", "Created", "Expiration", "Owner", "Roles" },
                values: new object[] { new Guid("68103797-356b-4a52-9c08-09dedccd3efe"), new DateTime(2020, 5, 21, 11, 6, 32, 542, DateTimeKind.Utc).AddTicks(1275), new DateTime(2022, 5, 21, 11, 6, 32, 542, DateTimeKind.Utc).AddTicks(1587), "mobile app", "WorldData,CountryData,News" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");
        }
    }
}
