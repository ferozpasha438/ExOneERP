using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_JvouchersLedger_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Trantype",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "tblFinTrnJournalVoucherInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalVoucherId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditDays = table.Column<short>(type: "smallint", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Remarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnJournalVoucherInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherInvoice_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherInvoice_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherInvoice_tblFinTrnJournalVoucher_JournalVoucherId",
                        column: x => x.JournalVoucherId,
                        principalTable: "tblFinTrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherInvoice_BranchCode",
                table: "tblFinTrnJournalVoucherInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherInvoice_CompanyId",
                table: "tblFinTrnJournalVoucherInvoice",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherInvoice_JournalVoucherId",
                table: "tblFinTrnJournalVoucherInvoice",
                column: "JournalVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherInvoice_LoginId",
                table: "tblFinTrnJournalVoucherInvoice",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherInvoice_VoucherNumber",
                table: "tblFinTrnJournalVoucherInvoice",
                column: "VoucherNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucherInvoice");

            migrationBuilder.DropColumn(
                name: "Trantype",
                table: "tblFinTrnJournalVoucherStatement");
        }
    }
}
