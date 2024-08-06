using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_ledgerbalance_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblFinTrnAccountsLedger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jvnum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AcDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MainAcCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostedFlag = table.Column<bool>(type: "bit", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExRate = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    VoidFlag = table.Column<bool>(type: "bit", nullable: false),
                    ReverseFlag = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Batch = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnAccountsLedger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnAccountsLedger_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnTrialBalance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AcDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrBalance = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnTrialBalance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnAccountsLedger_BranchCode",
                table: "tblFinTrnAccountsLedger",
                column: "BranchCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnAccountsLedger");

            migrationBuilder.DropTable(
                name: "tblFinTrnTrialBalance");
        }
    }
}
