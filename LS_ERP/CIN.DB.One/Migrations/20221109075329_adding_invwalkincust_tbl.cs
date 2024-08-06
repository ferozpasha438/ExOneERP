using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invwalkincust_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustArbName",
                table: "tblTranInvoice",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustName",
                table: "tblTranInvoice",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StuAdmDate",
                table: "tblDefSchoolStudentMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustArbName",
                table: "tblTranInvoice");

            migrationBuilder.DropColumn(
                name: "CustName",
                table: "tblTranInvoice");

            migrationBuilder.DropColumn(
                name: "StuAdmDate",
                table: "tblDefSchoolStudentMaster");
        }
    }
}
