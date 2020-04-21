using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.LastUpdate
{
    public partial class LastUpdateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastUpdates",
                columns: table => new
                {
                    DataFeed = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastUpdates", x => x.DataFeed);
                });

            migrationBuilder.InsertData(
                table: "LastUpdates",
                columns: new[] { "DataFeed", "Date" },
                values: new object[] { "WorldAggregated", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastUpdates");
        }
    }
}
