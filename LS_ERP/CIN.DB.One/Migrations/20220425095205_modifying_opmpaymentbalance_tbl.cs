using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_opmpaymentbalance_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "tblFinTrnOpmVendorPayment",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "tblFinTrnOpmCustomerPayment",
                type: "decimal(18,3)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "tblFinTrnOpmVendorPayment");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "tblFinTrnOpmCustomerPayment");
        }
    }
}
