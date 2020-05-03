using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.RssNews
{
    public partial class RssNewsWithEndUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndUrl",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndUrl",
                table: "News");
        }
    }
}
