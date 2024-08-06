using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CIN.DB.One.Migrations.DMC
{
    public partial class stuAdmDateNotNULLToNULL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
               name: "StuAdmDate",
               table: "tblDefSchoolStudentMaster",
               type: "DateTime",
               nullable: true,
               oldClrType: typeof(DateTime),
               oldType: "DateTime");
            migrationBuilder.AlterColumn<DateTime>(
               name: "StuRegDate",
               table: "tblDefSchoolStudentMaster",
               type: "DateTime",
               nullable: true,
               oldClrType: typeof(DateTime),
               oldType: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
