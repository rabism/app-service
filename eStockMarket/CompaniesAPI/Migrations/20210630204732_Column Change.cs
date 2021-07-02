using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompaniesAPI.Migrations
{
    public partial class ColumnChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StockPrice",
                table: "Stocks",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "StockId",
                keyValue: 101,
                columns: new[] { "StockDateTime", "StockPrice" },
                values: new object[] { new DateTime(2021, 7, 1, 2, 17, 31, 644, DateTimeKind.Local).AddTicks(2799), 100.89m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "StockPrice",
                table: "Stocks",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "StockId",
                keyValue: 101,
                columns: new[] { "StockDateTime", "StockPrice" },
                values: new object[] { new DateTime(2021, 6, 28, 1, 27, 1, 896, DateTimeKind.Local).AddTicks(3800), 100.89 });
        }
    }
}
