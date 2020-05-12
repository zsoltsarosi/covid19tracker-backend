using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations
{
    public partial class CountryDataSeedUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "TW",
                column: "Name",
                value: "Taiwan");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Iso2", "Iso3", "Lat", "Long", "Name", "Population" },
                values: new object[,]
                {
                    { "TJ", "TJK", 38.861f, 71.2761f, "Tajikistan", 9537642 },
                    { "MO", "MAC", 22.1667f, 113.55f, "Macau", 649342 },
                    { "HK", "HKG", 22.3f, 114.2f, "Hong Kong", 7496988 },
                    { "GL", "GRL", 71.7069f, -42.6043f, "Greenland", 56772 },
                    { "GI", "GIB", 36.1408f, -5.3536f, "Gibraltar", 33691 },
                    { "FO", "FRO", 61.8926f, -6.9118f, "Faroe Islands", 48865 },
                    { "US", "USA", 40f, -100f, "US", 329466283 },
                    { "FK", "FLK", -51.7963f, -59.5236f, "Falkland Islands", 3483 },
                    { "CN", "CHN", 30.5928f, 114.3055f, "China", 1404676330 },
                    { "KY", "CYM", 19.3133f, -81.2546f, "Cayman Islands", 65720 },
                    { "CA", "CAN", 60f, -95f, "Canada", 37855702 },
                    { "VG", "VGB", 18.4207f, -64.64f, "British Virgin Islands", 30237 },
                    { "BM", "BMU", 32.3078f, -64.7505f, "Bermuda", 62273 },
                    { "AU", "AUS", -25f, 133f, "Australia", 25459700 },
                    { "KM", "COM", -11.6455f, 43.3333f, "Comoros", 869595 },
                    { "AI", "AIA", 18.2206f, -63.0686f, "Anguilla", 15002 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "AI");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "AU");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "BM");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "CA");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "CN");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "FK");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "FO");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "GI");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "GL");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "HK");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "KM");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "KY");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "MO");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "TJ");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "US");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "VG");

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Iso2",
                keyValue: "TW",
                column: "Name",
                value: "Taiwan*");
        }
    }
}
