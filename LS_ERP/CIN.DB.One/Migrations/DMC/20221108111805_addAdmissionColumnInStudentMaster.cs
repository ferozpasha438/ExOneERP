using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations.DMC
{
    public partial class addAdmissionColumnInStudentMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("StuAdmDate", "tblDefSchoolStudentMaster", "datetime",
         unicode: false, nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRM_DEF_Branch");

            migrationBuilder.DropTable(
                name: "HRM_DEF_Department");

            migrationBuilder.DropTable(
                name: "HRM_DEF_EmployeeOff");

            migrationBuilder.DropTable(
                name: "HRM_DEF_EmployeeShiftMaster");

            migrationBuilder.DropTable(
                name: "HRM_DEF_Project");

            migrationBuilder.DropTable(
                name: "HRM_DEF_Site");

            migrationBuilder.DropTable(
                name: "HRM_TRAN_Employee");

            migrationBuilder.DropTable(
                name: "HRM_TRAN_EmployeePrimarySites_Log");

            migrationBuilder.DropTable(
                name: "HRM_TRAN_EmployeeTimeChart");
        }
    }
}
