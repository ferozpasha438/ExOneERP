using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_voucher_drcr_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrAmount",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "CrAmount",
                table: "tblFinTrnBankVoucherItem");

            migrationBuilder.RenameColumn(
                name: "DrAmount",
                table: "tblFinTrnCashVoucherItem",
                newName: "Payment");

            migrationBuilder.RenameColumn(
                name: "DrAmount",
                table: "tblFinTrnBankVoucherItem",
                newName: "Payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "tblFinTrnCashVoucherItem",
                newName: "DrAmount");

            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "tblFinTrnBankVoucherItem",
                newName: "DrAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "CrAmount",
                table: "tblFinTrnCashVoucherItem",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CrAmount",
                table: "tblFinTrnBankVoucherItem",
                type: "decimal(18,3)",
                nullable: true);
        }
    }
}
