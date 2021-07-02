using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompaniesAPI.Migrations
{
    public partial class DeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyCode",
                table: "Stocks");

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "StockId",
                keyValue: 101,
                column: "StockDateTime",
                value: new DateTime(2021, 6, 28, 1, 27, 1, 896, DateTimeKind.Local).AddTicks(3800));

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyCode",
                table: "Stocks",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "CompanyCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyCode",
                table: "Stocks");

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "StockId",
                keyValue: 101,
                column: "StockDateTime",
                value: new DateTime(2021, 6, 24, 3, 13, 20, 726, DateTimeKind.Local).AddTicks(3057));

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyCode",
                table: "Stocks",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "CompanyCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
