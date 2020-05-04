using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyFetcher.Core.Migrations
{
    public partial class currency_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyBeingMeasured = table.Column<string>(maxLength: 4, nullable: false),
                    CurrencyMatched = table.Column<string>(maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(nullable: false),
                    DailyDataOfCurrency = table.Column<DateTime>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyValues_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CurrencyBeingMeasured_CurrencyMatched",
                table: "Currencies",
                columns: new[] { "CurrencyBeingMeasured", "CurrencyMatched" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyValues_CurrencyId",
                table: "CurrencyValues",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyValues");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
