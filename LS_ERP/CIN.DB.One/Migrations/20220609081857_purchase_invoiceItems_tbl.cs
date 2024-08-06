using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class purchase_invoiceItems_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblTranPurcInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpCreditNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: true),
                    InvoiceDueDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    LpoContract = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvoiceStatusId = table.Column<int>(type: "int", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountBeforeTax = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalPayment = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsCreditConverted = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    InvoiceModule = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InvoiceNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceDate1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTranPurcInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblTranPurcInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblTranPurcInvoice_tblPopDefVendorPOTermsCode_PaymentTerms",
                        column: x => x.PaymentTerms,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblTranPurcInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditId = table.Column<long>(type: "bigint", nullable: true),
                    CreditMemoId = table.Column<long>(type: "bigint", nullable: true),
                    DebitMemoId = table.Column<long>(type: "bigint", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountBeforeTax = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaxTariffPercentage = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Discount = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTranPurcInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblTranPurcInvoiceItem_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblTranPurcInvoiceItem_TblTranPurcInvoice_CreditId",
                        column: x => x.CreditId,
                        principalTable: "TblTranPurcInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblTranPurcInvoice_BranchCode",
                table: "TblTranPurcInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranPurcInvoice_PaymentTerms",
                table: "TblTranPurcInvoice",
                column: "PaymentTerms");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranPurcInvoiceItem_CreditId",
                table: "TblTranPurcInvoiceItem",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranPurcInvoiceItem_ItemCode",
                table: "TblTranPurcInvoiceItem",
                column: "ItemCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblTranPurcInvoiceItem");

            migrationBuilder.DropTable(
                name: "TblTranPurcInvoice");
        }
    }
}
