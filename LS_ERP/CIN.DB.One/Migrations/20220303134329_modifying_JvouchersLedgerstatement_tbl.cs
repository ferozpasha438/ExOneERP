using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_JvouchersLedgerstatement_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompany_CompanyId",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompanyBranches_BranchCode",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucherInvoice");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_BranchCode",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_CompanyId",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "CheckNumber",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "CrAmount",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "DocNum",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "DrAmount",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "PamentCode",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "TranSource",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "Trantype",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "tblFinTrnJournalVoucherStatement",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckNumber",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "tblFinTrnJournalVoucherStatement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CrAmount",
                table: "tblFinTrnJournalVoucherStatement",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocNum",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DrAmount",
                table: "tblFinTrnJournalVoucherStatement",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PamentCode",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranSource",
                table: "tblFinTrnJournalVoucherStatement",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

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
                    AppliedAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreditDays = table.Column<short>(type: "smallint", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    JournalVoucherId = table.Column<int>(type: "int", nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                name: "IX_tblFinTrnJournalVoucherStatement_BranchCode",
                table: "tblFinTrnJournalVoucherStatement",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_CompanyId",
                table: "tblFinTrnJournalVoucherStatement",
                column: "CompanyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompany_CompanyId",
                table: "tblFinTrnJournalVoucherStatement",
                column: "CompanyId",
                principalTable: "tblErpSysCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompanyBranches_BranchCode",
                table: "tblFinTrnJournalVoucherStatement",
                column: "BranchCode",
                principalTable: "tblErpSysCompanyBranches",
                principalColumn: "BranchCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
