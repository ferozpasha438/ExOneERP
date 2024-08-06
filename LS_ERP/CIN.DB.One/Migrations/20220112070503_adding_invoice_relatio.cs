using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invoice_relatio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "tblFinTrnCustomerStatement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "tblFinTrnCustomerInvoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "tblFinTrnCustomerApproval",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_InvoiceId",
                table: "tblFinTrnCustomerStatement",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceId",
                table: "tblFinTrnCustomerInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_InvoiceId",
                table: "tblFinTrnCustomerApproval",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnCustomerApproval_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerApproval",
                column: "InvoiceId",
                principalTable: "tblTranInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnCustomerInvoice_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerInvoice",
                column: "InvoiceId",
                principalTable: "tblTranInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnCustomerStatement_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerStatement",
                column: "InvoiceId",
                principalTable: "tblTranInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnCustomerApproval_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnCustomerInvoice_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnCustomerStatement_tblTranInvoice_InvoiceId",
                table: "tblFinTrnCustomerStatement");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerStatement_InvoiceId",
                table: "tblFinTrnCustomerStatement");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceId",
                table: "tblFinTrnCustomerInvoice");

            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerApproval_InvoiceId",
                table: "tblFinTrnCustomerApproval");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "tblFinTrnCustomerStatement");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "tblFinTrnCustomerInvoice");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "tblFinTrnCustomerApproval");
        }
    }
}
