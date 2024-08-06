using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class removing_produt_relation_InvoiceItems_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTranInvoiceItem_tblTranDefProduct_ProductId",
                table: "tblTranInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TblTranVenInvoiceItem_tblTranDefProduct_ProductId",
                table: "TblTranVenInvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_TblTranVenInvoiceItem_ProductId",
                table: "TblTranVenInvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_tblTranInvoiceItem_ProductId",
                table: "tblTranInvoiceItem");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "Margin",
            //    table: "tblOpProjectResourceCosting",
            //    type: "decimal(17,3)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "Margin",
            //    table: "tblOpProjectLogisticsCosting",
            //    type: "decimal(17,3)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<int>(
            //    name: "Qty",
            //    table: "tblOpProjectLogisticsCosting",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Margin",
                table: "tblOpProjectResourceCosting");

            migrationBuilder.DropColumn(
                name: "Margin",
                table: "tblOpProjectLogisticsCosting");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "tblOpProjectLogisticsCosting");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranVenInvoiceItem_ProductId",
                table: "TblTranVenInvoiceItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranInvoiceItem_ProductId",
                table: "tblTranInvoiceItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTranInvoiceItem_tblTranDefProduct_ProductId",
                table: "tblTranInvoiceItem",
                column: "ProductId",
                principalTable: "tblTranDefProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TblTranVenInvoiceItem_tblTranDefProduct_ProductId",
                table: "TblTranVenInvoiceItem",
                column: "ProductId",
                principalTable: "tblTranDefProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
