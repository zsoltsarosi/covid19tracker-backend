using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace covid19tracker.Migrations.WorldAggregated
{
    public partial class WorldWideSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorldData",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    Confirmed = table.Column<int>(nullable: false),
                    Recovered = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false),
                    IncreaseRate = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldData", x => x.Date);
                });

            migrationBuilder.InsertData(
                table: "WorldData",
                columns: new[] { "Date", "Confirmed", "Deaths", "IncreaseRate", "Recovered" },
                values: new object[,]
                {
                    { new DateTime(2020, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 555, 17, null, 28 },
                    { new DateTime(2020, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 467653, 21181, 11.867736f, 113787 },
                    { new DateTime(2020, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 418041, 18625, 10.525314f, 108000 },
                    { new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 378231, 16505, 12.250373f, 98351 },
                    { new DateTime(2020, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 336953, 14651, 10.655256f, 97899 },
                    { new DateTime(2020, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 304507, 12973, 11.865559f, 91692 },
                    { new DateTime(2020, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 272208, 11299, 12.218328f, 87420 },
                    { new DateTime(2020, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 242570, 9867, 12.917266f, 84975 },
                    { new DateTime(2020, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 214821, 8733, 8.989761f, 83312 },
                    { new DateTime(2020, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 529591, 23970, 13.244435f, 122150 },
                    { new DateTime(2020, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 197102, 7905, 8.551886f, 80840 },
                    { new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 167454, 6440, 7.2728553f, 76034 },
                    { new DateTime(2020, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 156101, 5819, 7.503874f, 72624 },
                    { new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 145205, 5404, 13.130298f, 70251 },
                    { new DateTime(2020, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 128352, 4720, 1.9678252f, 68324 },
                    { new DateTime(2020, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 125875, 4615, 6.1161695f, 67003 },
                    { new DateTime(2020, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 118620, 4262, 4.428207f, 64404 },
                    { new DateTime(2020, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 113590, 3988, 3.4319484f, 62494 },
                    { new DateTime(2020, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 109821, 3802, 3.7544758f, 60694 },
                    { new DateTime(2020, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 181574, 7126, 8.432166f, 78088 },
                    { new DateTime(2020, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 593291, 27198, 12.02815f, 130915 },
                    { new DateTime(2020, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 660693, 30652, 11.360698f, 139415 },
                    { new DateTime(2020, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 720140, 33925, 8.997674f, 149082 },
                    { new DateTime(2020, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2240190, 153821, 4.076914f, 568343 },
                    { new DateTime(2020, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2152437, 143800, 4.687766f, 542107 },
                    { new DateTime(2020, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2056054, 134176, 4.0412593f, 511019 },
                    { new DateTime(2020, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1976191, 125983, 3.7458827f, 474261 },
                    { new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1904838, 119481, 3.821671f, 448655 },
                    { new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1834721, 114090, 5.7080054f, 421722 },
                    { new DateTime(2020, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1735650, 108502, 4.7132897f, 402110 },
                    { new DateTime(2020, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1657526, 102525, 5.893394f, 376096 },
                    { new DateTime(2020, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1565278, 95521, 5.747594f, 353975 },
                    { new DateTime(2020, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1480202, 88338, 5.99552f, 328661 },
                    { new DateTime(2020, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1396476, 81937, 5.675072f, 300054 },
                    { new DateTime(2020, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1321481, 74565, 5.7392893f, 276515 },
                    { new DateTime(2020, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1249754, 69374, 6.2661767f, 260012 },
                    { new DateTime(2020, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1176060, 64606, 7.3128715f, 246152 },
                    { new DateTime(2020, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1095917, 58787, 8.135547f, 225796 },
                    { new DateTime(2020, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1013466, 52983, 8.6704445f, 210263 },
                    { new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 932605, 47180, 8.760249f, 193177 },
                    { new DateTime(2020, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 857487, 42107, 9.59855f, 178034 },
                    { new DateTime(2020, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 782389, 37582, 8.644013f, 164566 },
                    { new DateTime(2020, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 105847, 3558, 3.9744208f, 58358 },
                    { new DateTime(2020, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2317758, 159509, 3.4625635f, 592319 },
                    { new DateTime(2020, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 101801, 3460, 3.9995506f, 55865 },
                    { new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 95120, 3254, 2.455838f, 51170 },
                    { new DateTime(2020, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 40150, 906, 8.162716f, 3244 },
                    { new DateTime(2020, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 37120, 806, 7.9352155f, 2616 },
                    { new DateTime(2020, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 34391, 719, 11.680847f, 2011 },
                    { new DateTime(2020, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 30794, 634, 11.431156f, 1487 },
                    { new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 27635, 564, 15.666332f, 1124 },
                    { new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 23892, 492, 20.175041f, 852 },
                    { new DateTime(2020, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 19881, 426, 18.43093f, 623 },
                    { new DateTime(2020, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 16787, 362, 39.450073f, 472 },
                    { new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 42762, 1013, 6.505604f, 3946 },
                    { new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12038, 259, 21.265236f, 284 },
                    { new DateTime(2020, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8234, 171, 33.53876f, 143 },
                    { new DateTime(2020, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 6166, 133, 10.541412f, 126 },
                    { new DateTime(2020, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 5578, 131, 90.57055f, 107 },
                    { new DateTime(2020, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 2927, 82, 38.19641f, 61 },
                    { new DateTime(2020, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2118, 56, 47.698746f, 52 },
                    { new DateTime(2020, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1434, 42, 52.391075f, 39 },
                    { new DateTime(2020, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 941, 26, 43.883793f, 36 },
                    { new DateTime(2020, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 654, 18, 17.837837f, 30 },
                    { new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 9927, 213, 20.561089f, 222 },
                    { new DateTime(2020, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 44802, 1113, 4.770591f, 4683 },
                    { new DateTime(2020, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 45221, 1118, 0.9352261f, 5150 },
                    { new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 60368, 1371, 33.4955f, 6295 },
                    { new DateTime(2020, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 92840, 3160, 2.806015f, 48228 },
                    { new DateTime(2020, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 90306, 3085, 2.191945f, 45602 },
                    { new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 88369, 2996, 2.74151f, 42716 },
                    { new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 86011, 2941, 2.257704f, 39782 },
                    { new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 84112, 2872, 1.650835f, 36711 },
                    { new DateTime(2020, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 82746, 2814, 1.6685506f, 33277 },
                    { new DateTime(2020, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 81388, 2770, 1.2213019f, 30384 },
                    { new DateTime(2020, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 80406, 2708, 1.0620781f, 27905 },
                    { new DateTime(2020, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 79561, 2629, 0.76369715f, 25227 },
                    { new DateTime(2020, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 78958, 2469, 0.49126914f, 23394 },
                    { new DateTime(2020, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 78572, 2458, 2.2819874f, 22886 },
                    { new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 76819, 2251, 0.8163051f, 18890 },
                    { new DateTime(2020, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 76197, 2247, 0.73771465f, 18177 },
                    { new DateTime(2020, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 75639, 2122, 0.6694527f, 16121 },
                    { new DateTime(2020, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 75136, 2007, 2.5635426f, 14352 },
                    { new DateTime(2020, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 73258, 1868, 2.855779f, 12583 },
                    { new DateTime(2020, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 71224, 1770, 3.1783283f, 10865 },
                    { new DateTime(2020, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69030, 1666, 3.2069972f, 9395 },
                    { new DateTime(2020, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 66885, 1523, 10.795455f, 8058 },
                    { new DateTime(2020, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 97886, 3348, 2.9079058f, 53796 },
                    { new DateTime(2020, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2401378, 165043, 3.607797f, 623903 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldData");
        }
    }
}
