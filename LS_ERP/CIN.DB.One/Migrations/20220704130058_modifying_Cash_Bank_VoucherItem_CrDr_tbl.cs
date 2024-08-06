using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_Cash_Bank_VoucherItem_CrDr_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CrAmount",
                table: "tblFinTrnCashVoucherItem",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DrAmount",
                table: "tblFinTrnCashVoucherItem",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CrAmount",
                table: "tblFinTrnBankVoucherItem",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DrAmount",
                table: "tblFinTrnBankVoucherItem",
                type: "decimal(18,3)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrAmount",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "DrAmount",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "CrAmount",
                table: "tblFinTrnBankVoucherItem");

            migrationBuilder.DropColumn(
                name: "DrAmount",
                table: "tblFinTrnBankVoucherItem");
        }
    }
}
