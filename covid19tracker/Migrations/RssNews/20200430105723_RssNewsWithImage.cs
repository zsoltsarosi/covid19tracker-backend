using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.RssNews
{
    public partial class RssNewsWithImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "News");
        }
    }
}
