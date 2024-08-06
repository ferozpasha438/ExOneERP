using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_invoiceItems_typecolumn_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "InvoiceType",
                table: "TblTranVenInvoiceItem",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "InvoiceType",
                table: "tblTranInvoiceItem",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "TblTranVenInvoiceItem");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "tblTranInvoiceItem");
        }
    }
}
