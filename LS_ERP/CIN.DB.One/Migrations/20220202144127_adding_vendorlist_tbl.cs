using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_vendorlist_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CustLastPayAmt",
                table: "tblSndDefCustomerMaster",
                type: "decimal(17,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustDiscount",
                table: "tblSndDefCustomerMaster",
                type: "decimal(17,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustCrLimit",
                table: "tblSndDefCustomerMaster",
                type: "decimal(17,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,3)");

            migrationBuilder.CreateTable(
                name: "tblSndDefSurveyFormDataEntry",
                columns: table => new
                {
                    EntryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnquiryID = table.Column<int>(type: "int", nullable: false),
                    ElementEngName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElementArbName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ElementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ListValueString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinValue = table.Column<int>(type: "int", nullable: true),
                    MaxValue = table.Column<int>(type: "int", nullable: true),
                    EntryValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSurveyFormDataEntry", x => x.EntryID);
                    table.ForeignKey(
                        name: "FK_tblSndDefSurveyFormDataEntry_tblSndDefServiceEnquiries_EnquiryID",
                        column: x => x.EnquiryID,
                        principalTable: "tblSndDefServiceEnquiries",
                        principalColumn: "EnquiryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSurveyFormDataEntry_EnquiryID",
                table: "tblSndDefSurveyFormDataEntry",
                column: "EnquiryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSndDefSurveyFormDataEntry");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustLastPayAmt",
                table: "tblSndDefCustomerMaster",
                type: "decimal(12,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustDiscount",
                table: "tblSndDefCustomerMaster",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustCrLimit",
                table: "tblSndDefCustomerMaster",
                type: "decimal(12,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,3)");
        }
    }
}
