using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class removing_index_CustVendInv_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceNumber",
                table: "tblFinTrnVendorInvoice");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceNumber",
                table: "tblFinTrnCustomerInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceNumber",
                table: "tblFinTrnVendorInvoice",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceNumber",
                table: "tblFinTrnCustomerInvoice",
                column: "InvoiceNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceNumber",
                table: "tblFinTrnVendorInvoice");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceNumber",
                table: "tblFinTrnCustomerInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceNumber",
                table: "tblFinTrnVendorInvoice",
                column: "InvoiceNumber",
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceNumber",
                table: "tblFinTrnCustomerInvoice",
                column: "InvoiceNumber",
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");
        }
    }
}
