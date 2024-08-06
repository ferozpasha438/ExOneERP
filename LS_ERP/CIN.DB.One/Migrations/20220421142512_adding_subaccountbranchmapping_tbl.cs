using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_subaccountbranchmapping_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApPaymentNumber",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArPaymentNumber",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblFinDefAccountBranchMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FinBranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InventoryAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CashPurchase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostofSalesAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InventoryAdjustment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultSalesAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultSalesReturn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InventoryTransfer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultPayable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostCorrection = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WIPUsageConsumption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reserved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefAccountBranchMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountBranchMapping_tblErpSysCompanyBranches_FinBranchCode",
                        column: x => x.FinBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnOpmCustomerPaymentHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PaymentNumber = table.Column<int>(type: "int", nullable: false),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Checkdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preparedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Flag1 = table.Column<bool>(type: "bit", nullable: false),
                    Flag2 = table.Column<bool>(type: "bit", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVoid = table.Column<bool>(type: "bit", nullable: false),
                    VoidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnOpmCustomerPaymentHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPaymentHeader_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPaymentHeader_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPaymentHeader_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnOpmVendorPaymentHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PaymentNumber = table.Column<int>(type: "int", nullable: false),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Checkdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preparedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Flag1 = table.Column<bool>(type: "bit", nullable: false),
                    Flag2 = table.Column<bool>(type: "bit", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVoid = table.Column<bool>(type: "bit", nullable: false),
                    VoidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnOpmVendorPaymentHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPaymentHeader_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPaymentHeader_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPaymentHeader_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnOpmCustomerPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: true),
                    InvoiceDueDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Flag1 = table.Column<bool>(type: "bit", nullable: false),
                    Flag2 = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnOpmCustomerPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPayment_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPayment_tblFinTrnOpmCustomerPaymentHeader_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "tblFinTrnOpmCustomerPaymentHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPayment_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmCustomerPayment_tblTranInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "tblTranInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnOpmVendorPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: true),
                    InvoiceDueDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Flag1 = table.Column<bool>(type: "bit", nullable: false),
                    Flag2 = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnOpmVendorPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPayment_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPayment_tblFinTrnOpmVendorPaymentHeader_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "tblFinTrnOpmVendorPaymentHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPayment_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnOpmVendorPayment_tblTranInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "tblTranInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountBranchMapping_FinBranchCode",
                table: "tblFinDefAccountBranchMapping",
                column: "FinBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPayment_BranchCode",
                table: "tblFinTrnOpmCustomerPayment",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPayment_CustCode",
                table: "tblFinTrnOpmCustomerPayment",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPayment_InvoiceId",
                table: "tblFinTrnOpmCustomerPayment",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPayment_PaymentId",
                table: "tblFinTrnOpmCustomerPayment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPaymentHeader_BranchCode",
                table: "tblFinTrnOpmCustomerPaymentHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPaymentHeader_CompanyId",
                table: "tblFinTrnOpmCustomerPaymentHeader",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmCustomerPaymentHeader_CustCode",
                table: "tblFinTrnOpmCustomerPaymentHeader",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPayment_BranchCode",
                table: "tblFinTrnOpmVendorPayment",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPayment_InvoiceId",
                table: "tblFinTrnOpmVendorPayment",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPayment_PaymentId",
                table: "tblFinTrnOpmVendorPayment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPayment_VendCode",
                table: "tblFinTrnOpmVendorPayment",
                column: "VendCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPaymentHeader_BranchCode",
                table: "tblFinTrnOpmVendorPaymentHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPaymentHeader_CompanyId",
                table: "tblFinTrnOpmVendorPaymentHeader",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnOpmVendorPaymentHeader_VendCode",
                table: "tblFinTrnOpmVendorPaymentHeader",
                column: "VendCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinDefAccountBranchMapping");

            migrationBuilder.DropTable(
                name: "tblFinTrnOpmCustomerPayment");

            migrationBuilder.DropTable(
                name: "tblFinTrnOpmVendorPayment");

            migrationBuilder.DropTable(
                name: "tblFinTrnOpmCustomerPaymentHeader");

            migrationBuilder.DropTable(
                name: "tblFinTrnOpmVendorPaymentHeader");

            migrationBuilder.DropColumn(
                name: "ApPaymentNumber",
                table: "tblSequenceNumberSetting");

            migrationBuilder.DropColumn(
                name: "ArPaymentNumber",
                table: "tblSequenceNumberSetting");
        }
    }
}
