using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompaniesAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCEO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyTurnOver = table.Column<int>(type: "int", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyCode);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockPrice = table.Column<double>(type: "float", nullable: false),
                    StockDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stocks_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "CompanyCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyCode", "CompanyCEO", "CompanyName", "CompanyTurnOver", "Website" },
                values: new object[] { "Code1", "XYZ", "ABC", 100, "website1" });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "StockId", "CompanyCode", "StockDateTime", "StockPrice" },
                values: new object[] { 101, "Code1", new DateTime(2021, 6, 24, 3, 13, 20, 726, DateTimeKind.Local).AddTicks(3057), 100.89 });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CompanyCode",
                table: "Stocks",
                column: "CompanyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
