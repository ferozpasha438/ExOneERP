using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_VatNumber_Cust_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VATNumber",
                table: "tblSndDefVendorMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VATNumber",
                table: "tblSndDefCustomerMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VATNumber",
                table: "tblSndDefVendorMaster");

            migrationBuilder.DropColumn(
                name: "VATNumber",
                table: "tblSndDefCustomerMaster");
        }
    }
}
