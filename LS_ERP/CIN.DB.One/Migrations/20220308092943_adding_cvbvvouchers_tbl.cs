using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_cvbvvouchers_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BvVoucherSeq",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CvVoucherSeq",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoid",
                table: "tblFinTrnJournalVoucherStatement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Void",
                table: "tblFinTrnJournalVoucher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tblFinTrnBankVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpVoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrnMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChequeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountPayee = table.Column<bool>(type: "bit", nullable: false),
                    PDC = table.Column<bool>(type: "bit", nullable: false),
                    VoucherType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Posted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Void = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnBankVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucher_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucher_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCashVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpVoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CBookCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VoucherType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Posted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Void = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnCashVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucher_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucher_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnBankVoucherApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankVoucherId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    AppRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnBankVoucherApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherApproval_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherApproval_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherApproval_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherApproval_tblFinTrnBankVoucher_BankVoucherId",
                        column: x => x.BankVoucherId,
                        principalTable: "tblFinTrnBankVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnBankVoucherItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankVoucherId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_tblFinTrnBankVoucherItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherItem_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherItem_tblFinDefMainAccounts_FinAcCode",
                        column: x => x.FinAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherItem_tblFinTrnBankVoucher_BankVoucherId",
                        column: x => x.BankVoucherId,
                        principalTable: "tblFinTrnBankVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnBankVoucherStatement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankVoucherId = table.Column<int>(type: "int", nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    IsVoid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnBankVoucherStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherStatement_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnBankVoucherStatement_tblFinTrnBankVoucher_BankVoucherId",
                        column: x => x.BankVoucherId,
                        principalTable: "tblFinTrnBankVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCashVoucherApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashVoucherId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    AppRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnCashVoucherApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherApproval_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherApproval_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherApproval_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherApproval_tblFinTrnCashVoucher_CashVoucherId",
                        column: x => x.CashVoucherId,
                        principalTable: "tblFinTrnCashVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCashVoucherItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashVoucherId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_tblFinTrnCashVoucherItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherItem_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherItem_tblFinDefMainAccounts_FinAcCode",
                        column: x => x.FinAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherItem_tblFinTrnCashVoucher_CashVoucherId",
                        column: x => x.CashVoucherId,
                        principalTable: "tblFinTrnCashVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCashVoucherStatement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashVoucherId = table.Column<int>(type: "int", nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    IsVoid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnCashVoucherStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherStatement_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCashVoucherStatement_tblFinTrnCashVoucher_CashVoucherId",
                        column: x => x.CashVoucherId,
                        principalTable: "tblFinTrnCashVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucher_BranchCode",
                table: "tblFinTrnBankVoucher",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucher_CompanyId",
                table: "tblFinTrnBankVoucher",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucher_VoucherNumber",
                table: "tblFinTrnBankVoucher",
                column: "VoucherNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherApproval_BankVoucherId",
                table: "tblFinTrnBankVoucherApproval",
                column: "BankVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherApproval_BranchCode",
                table: "tblFinTrnBankVoucherApproval",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherApproval_CompanyId",
                table: "tblFinTrnBankVoucherApproval",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherApproval_LoginId",
                table: "tblFinTrnBankVoucherApproval",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherApproval_TranNumber",
                table: "tblFinTrnBankVoucherApproval",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherItem_BankVoucherId",
                table: "tblFinTrnBankVoucherItem",
                column: "BankVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherItem_BranchCode",
                table: "tblFinTrnBankVoucherItem",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherItem_FinAcCode",
                table: "tblFinTrnBankVoucherItem",
                column: "FinAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherStatement_BankVoucherId",
                table: "tblFinTrnBankVoucherStatement",
                column: "BankVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherStatement_LoginId",
                table: "tblFinTrnBankVoucherStatement",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnBankVoucherStatement_TranNumber",
                table: "tblFinTrnBankVoucherStatement",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucher_BranchCode",
                table: "tblFinTrnCashVoucher",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucher_CompanyId",
                table: "tblFinTrnCashVoucher",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucher_VoucherNumber",
                table: "tblFinTrnCashVoucher",
                column: "VoucherNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherApproval_BranchCode",
                table: "tblFinTrnCashVoucherApproval",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherApproval_CashVoucherId",
                table: "tblFinTrnCashVoucherApproval",
                column: "CashVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherApproval_CompanyId",
                table: "tblFinTrnCashVoucherApproval",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherApproval_LoginId",
                table: "tblFinTrnCashVoucherApproval",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherApproval_TranNumber",
                table: "tblFinTrnCashVoucherApproval",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherItem_BranchCode",
                table: "tblFinTrnCashVoucherItem",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherItem_CashVoucherId",
                table: "tblFinTrnCashVoucherItem",
                column: "CashVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherItem_FinAcCode",
                table: "tblFinTrnCashVoucherItem",
                column: "FinAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherStatement_CashVoucherId",
                table: "tblFinTrnCashVoucherStatement",
                column: "CashVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherStatement_LoginId",
                table: "tblFinTrnCashVoucherStatement",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCashVoucherStatement_TranNumber",
                table: "tblFinTrnCashVoucherStatement",
                column: "TranNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnBankVoucherApproval");

            migrationBuilder.DropTable(
                name: "tblFinTrnBankVoucherItem");

            migrationBuilder.DropTable(
                name: "tblFinTrnBankVoucherStatement");

            migrationBuilder.DropTable(
                name: "tblFinTrnCashVoucherApproval");

            migrationBuilder.DropTable(
                name: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropTable(
                name: "tblFinTrnCashVoucherStatement");

            migrationBuilder.DropTable(
                name: "tblFinTrnBankVoucher");

            migrationBuilder.DropTable(
                name: "tblFinTrnCashVoucher");

            migrationBuilder.DropColumn(
                name: "BvVoucherSeq",
                table: "tblSequenceNumberSetting");

            migrationBuilder.DropColumn(
                name: "CvVoucherSeq",
                table: "tblSequenceNumberSetting");

            migrationBuilder.DropColumn(
                name: "IsVoid",
                table: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropColumn(
                name: "Void",
                table: "tblFinTrnJournalVoucher");
        }
    }
}
