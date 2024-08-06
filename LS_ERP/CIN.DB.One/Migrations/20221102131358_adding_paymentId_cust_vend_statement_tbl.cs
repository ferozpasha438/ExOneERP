using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_paymentId_cust_vend_statement_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "tblFinTrnVendorStatement",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "tblFinTrnCustomerStatement",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "tblFinTrnVendorStatement");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "tblFinTrnCustomerStatement");
        }
    }
}
