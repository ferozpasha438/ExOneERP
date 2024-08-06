using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_customerOutstanding_Bal_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VendOutStandBal",
                table: "tblSndDefVendorMaster",
                type: "decimal(17,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustOutStandBal",
                table: "tblSndDefCustomerMaster",
                type: "decimal(17,3)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendOutStandBal",
                table: "tblSndDefVendorMaster");

            migrationBuilder.DropColumn(
                name: "CustOutStandBal",
                table: "tblSndDefCustomerMaster");
        }
    }
}
