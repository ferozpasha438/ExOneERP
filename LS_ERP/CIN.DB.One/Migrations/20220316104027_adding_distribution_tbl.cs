using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_distribution_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblFinTrnDistribution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    FinAcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Gl = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnDistribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnDistribution_tblFinDefMainAccounts_FinAcCode",
                        column: x => x.FinAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnDistribution_FinAcCode",
                table: "tblFinTrnDistribution",
                column: "FinAcCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnDistribution");
        }
    }
}
