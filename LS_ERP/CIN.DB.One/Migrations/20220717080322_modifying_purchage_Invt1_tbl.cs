using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_purchage_Invt1_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WHCode",
                table: "tblPopTrnPurchaseReturnHeader",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblPopTrnPurchaseOrderHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WHCode",
                table: "tblPopTrnPurchaseOrderHeader",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranBranch",
                table: "tblIMTransactionHeader",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranBranch",
                table: "tblIMReceiptsTransactionHeader",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranBranch",
                table: "tblIMAdjustmentsTransactionHeader",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WHCode",
                table: "tblPopTrnPurchaseReturnHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblPopTrnPurchaseOrderHeader");

            migrationBuilder.DropColumn(
                name: "WHCode",
                table: "tblPopTrnPurchaseOrderHeader");

            migrationBuilder.DropColumn(
                name: "TranBranch",
                table: "tblIMTransactionHeader");

            migrationBuilder.DropColumn(
                name: "TranBranch",
                table: "tblIMReceiptsTransactionHeader");

            migrationBuilder.DropColumn(
                name: "TranBranch",
                table: "tblIMAdjustmentsTransactionHeader");
        }
    }
}
