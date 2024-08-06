using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invoice_salescode_rel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentTerms",
                table: "tblTranInvoice",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblTranInvoice_PaymentTerms",
                table: "tblTranInvoice",
                column: "PaymentTerms");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTranInvoice_tblSndDefSalesTermsCode_PaymentTerms",
                table: "tblTranInvoice",
                column: "PaymentTerms",
                principalTable: "tblSndDefSalesTermsCode",
                principalColumn: "SalesTermsCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTranInvoice_tblSndDefSalesTermsCode_PaymentTerms",
                table: "tblTranInvoice");

            migrationBuilder.DropIndex(
                name: "IX_tblTranInvoice_PaymentTerms",
                table: "tblTranInvoice");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentTerms",
                table: "tblTranInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);
        }
    }
}
