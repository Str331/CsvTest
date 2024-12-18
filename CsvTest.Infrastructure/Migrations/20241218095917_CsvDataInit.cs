using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CsvDataInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CsvDatas",
                columns: table => new
                {
                    CsvDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PULocationID = table.Column<int>(type: "int", nullable: false),
                    DOLocationID = table.Column<int>(type: "int", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TripDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StoreFwdFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TpepPickupDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TpepDropOffDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvDatas", x => x.CsvDataID);
                });

            migrationBuilder.CreateTable(
                name: "CsvDatasJson",
                columns: table => new
                {
                    CsvDataWithJsonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvDatasJson", x => x.CsvDataWithJsonID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CsvDatas");

            migrationBuilder.DropTable(
                name: "CsvDatasJson");
        }
    }
}
