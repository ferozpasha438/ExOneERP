using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_JvouchersLedgerApproval_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnVendorApproval_TranNumber",
                table: "tblFinTrnVendorApproval");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerApproval_TranNumber",
                table: "tblFinTrnCustomerApproval");

            migrationBuilder.CreateTable(
                name: "tblFinTrnJournalVoucherApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalVoucherId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_tblFinTrnJournalVoucherApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherApproval_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherApproval_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherApproval_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherApproval_tblFinTrnJournalVoucher_JournalVoucherId",
                        column: x => x.JournalVoucherId,
                        principalTable: "tblFinTrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnJournalVoucherStatement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalVoucherId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    JvDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PamentCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnJournalVoucherStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherStatement_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnJournalVoucherStatement_tblFinTrnJournalVoucher_JournalVoucherId",
                        column: x => x.JournalVoucherId,
                        principalTable: "tblFinTrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_TranNumber",
                table: "tblFinTrnVendorApproval",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_TranNumber",
                table: "tblFinTrnCustomerApproval",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherApproval_BranchCode",
                table: "tblFinTrnJournalVoucherApproval",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherApproval_CompanyId",
                table: "tblFinTrnJournalVoucherApproval",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherApproval_JournalVoucherId",
                table: "tblFinTrnJournalVoucherApproval",
                column: "JournalVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherApproval_LoginId",
                table: "tblFinTrnJournalVoucherApproval",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherApproval_TranNumber",
                table: "tblFinTrnJournalVoucherApproval",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_BranchCode",
                table: "tblFinTrnJournalVoucherStatement",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_CompanyId",
                table: "tblFinTrnJournalVoucherStatement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_JournalVoucherId",
                table: "tblFinTrnJournalVoucherStatement",
                column: "JournalVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_LoginId",
                table: "tblFinTrnJournalVoucherStatement",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnJournalVoucherStatement_TranNumber",
                table: "tblFinTrnJournalVoucherStatement",
                column: "TranNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucherApproval");

            migrationBuilder.DropTable(
                name: "tblFinTrnJournalVoucherStatement");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnVendorApproval_TranNumber",
                table: "tblFinTrnVendorApproval");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerApproval_TranNumber",
                table: "tblFinTrnCustomerApproval");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_TranNumber",
                table: "tblFinTrnVendorApproval",
                column: "TranNumber",
                unique: true,
                filter: "[TranNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_TranNumber",
                table: "tblFinTrnCustomerApproval",
                column: "TranNumber",
                unique: true,
                filter: "[TranNumber] IS NOT NULL");
        }
    }
}
