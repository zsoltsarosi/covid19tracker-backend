using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.RssNews
{
    public partial class RssNewsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FeedId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    SourceName = table.Column<string>(nullable: true),
                    SourceUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
