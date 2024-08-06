using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_vendorlist_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorApproval_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorInvoice_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorPayment_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorStatement_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorStatement");

            migrationBuilder.RenameColumn(
                name: "CustCode",
                table: "tblFinTrnVendorStatement",
                newName: "VendCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorStatement_CustCode",
                table: "tblFinTrnVendorStatement",
                newName: "IX_tblFinTrnVendorStatement_VendCode");

            migrationBuilder.RenameColumn(
                name: "CustCode",
                table: "tblFinTrnVendorPayment",
                newName: "VendCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorPayment_CustCode",
                table: "tblFinTrnVendorPayment",
                newName: "IX_tblFinTrnVendorPayment_VendCode");

            migrationBuilder.RenameColumn(
                name: "CustCode",
                table: "tblFinTrnVendorInvoice",
                newName: "VendCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorInvoice_CustCode",
                table: "tblFinTrnVendorInvoice",
                newName: "IX_tblFinTrnVendorInvoice_VendCode");

            migrationBuilder.RenameColumn(
                name: "CustCode",
                table: "tblFinTrnVendorApproval",
                newName: "VendCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorApproval_CustCode",
                table: "tblFinTrnVendorApproval",
                newName: "IX_tblFinTrnVendorApproval_VendCode");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorApproval_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorApproval",
                column: "VendCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorInvoice_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorInvoice",
                column: "VendCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorPayment_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorPayment",
                column: "VendCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorStatement_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorStatement",
                column: "VendCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorApproval_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorInvoice_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorPayment_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFinTrnVendorStatement_tblSndDefVendorMaster_VendCode",
                table: "tblFinTrnVendorStatement");

            migrationBuilder.RenameColumn(
                name: "VendCode",
                table: "tblFinTrnVendorStatement",
                newName: "CustCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorStatement_VendCode",
                table: "tblFinTrnVendorStatement",
                newName: "IX_tblFinTrnVendorStatement_CustCode");

            migrationBuilder.RenameColumn(
                name: "VendCode",
                table: "tblFinTrnVendorPayment",
                newName: "CustCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorPayment_VendCode",
                table: "tblFinTrnVendorPayment",
                newName: "IX_tblFinTrnVendorPayment_CustCode");

            migrationBuilder.RenameColumn(
                name: "VendCode",
                table: "tblFinTrnVendorInvoice",
                newName: "CustCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorInvoice_VendCode",
                table: "tblFinTrnVendorInvoice",
                newName: "IX_tblFinTrnVendorInvoice_CustCode");

            migrationBuilder.RenameColumn(
                name: "VendCode",
                table: "tblFinTrnVendorApproval",
                newName: "CustCode");

            migrationBuilder.RenameIndex(
                name: "IX_tblFinTrnVendorApproval_VendCode",
                table: "tblFinTrnVendorApproval",
                newName: "IX_tblFinTrnVendorApproval_CustCode");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorApproval_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorApproval",
                column: "CustCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorInvoice_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorInvoice",
                column: "CustCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorPayment_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorPayment",
                column: "CustCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFinTrnVendorStatement_tblSndDefVendorMaster_CustCode",
                table: "tblFinTrnVendorStatement",
                column: "CustCode",
                principalTable: "tblSndDefVendorMaster",
                principalColumn: "VendCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
