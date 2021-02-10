using Microsoft.EntityFrameworkCore.Migrations;

namespace KRFTemplateApi.Infrastructure.Migrations
{
    public partial class initialize_sample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SampleTable",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TemperatureMin = table.Column<int>(type: "int", nullable: false),
                    TemperatureMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAMPLE", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "SampleTable",
                columns: new[] { "Code", "Description", "TemperatureMax", "TemperatureMin" },
                values: new object[,]
                {
                    { "Freezing", "It's freezing today", -5, -15 },
                    { "Bracing", "It's Bracing", 0, -4 },
                    { "Chilly", "It's Chilly tonight", 5, 1 },
                    { "Cool", "Cool day", 10, 6 },
                    { "Mild", "Mild mornigng", 15, 11 },
                    { "Warm", "The afternoon will be Warm", 20, 16 },
                    { "Balmy", "Balmy it is", 25, 21 },
                    { "Hot", "The day is too Hot", 30, 26 },
                    { "Sweltering", "Sweltering, i need a pool", 35, 31 },
                    { "Scorching", "It's Scorching, i'm going to the beach and swim all day", 45, 36 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleTable");
        }
    }
}
