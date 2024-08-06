using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_JvouchersLedger_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JvVoucherSeq",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblFinTrnJournalVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpVoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Posted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnJournalVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucher_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucher_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnJournalVoucherItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalVoucherId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    FinAcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnJournalVoucherItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherItem_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherItem_tblFinDefMainAccounts_FinAcCode",
                        column: x => x.FinAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherItem_tblFinTrnJournalVoucher_JournalVoucherId",
                        column: x => x.JournalVoucherId,
                        principalTable: "tblFinTrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucher_BranchCode",
                table: "tblFinTrnJournalVoucher",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucher_CompanyId",
                table: "tblFinTrnJournalVoucher",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucher_VoucherNumber",
                table: "tblFinTrnJournalVoucher",
                column: "VoucherNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherItem_BranchCode",
                table: "tblFinTrnJournalVoucherItem",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherItem_FinAcCode",
                table: "tblFinTrnJournalVoucherItem",
                column: "FinAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherItem_JournalVoucherId",
                table: "tblFinTrnJournalVoucherItem",
                column: "JournalVoucherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucherItem");

            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucher");

            migrationBuilder.DropColumn(
                name: "JvVoucherSeq",
                table: "tblSequenceNumberSetting");
        }
    }
}
