using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_opmVendPayment_InvoiceId_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnOpmVendorPayment_tblTranInvoice_InvoiceId",
                table: "tblFinTrnOpmVendorPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnOpmVendorPayment_TblTranVenInvoice_InvoiceId",
                table: "tblFinTrnOpmVendorPayment",
                column: "InvoiceId",
                principalTable: "TblTranVenInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnOpmVendorPayment_TblTranVenInvoice_InvoiceId",
                table: "tblFinTrnOpmVendorPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnOpmVendorPayment_tblTranInvoice_InvoiceId",
                table: "tblFinTrnOpmVendorPayment",
                column: "InvoiceId",
                principalTable: "tblTranInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
