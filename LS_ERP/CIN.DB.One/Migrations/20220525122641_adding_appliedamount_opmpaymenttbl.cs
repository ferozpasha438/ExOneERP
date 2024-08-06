using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_appliedamount_opmpaymenttbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "UnitQuantity",
            //    table: "tblSndDefServiceEnquiries",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AppliedAmount",
                table: "tblFinTrnOpmCustomerPayment",
                type: "decimal(18,3)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitQuantity",
                table: "tblSndDefServiceEnquiries");

            migrationBuilder.DropColumn(
                name: "AppliedAmount",
                table: "tblFinTrnOpmCustomerPayment");
        }
    }
}
