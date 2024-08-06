using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class TblSchoolBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblStudentHomeWork",
                table: "tblStudentHomeWork");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "tblSysSchoolBranches",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "BranchNameAr",
                table: "tblSysSchoolBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchPrefix",
                table: "tblSysSchoolBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextFeeVoucherNum",
                table: "tblSysSchoolBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NextStuNum",
                table: "tblSysSchoolBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "tblDefSchoolStudentMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblStudentHomeWork",
                table: "tblStudentHomeWork",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblStudentHomeWork",
                table: "tblStudentHomeWork");

            migrationBuilder.DropColumn(
                name: "BranchNameAr",
                table: "tblSysSchoolBranches");

            migrationBuilder.DropColumn(
                name: "BranchPrefix",
                table: "tblSysSchoolBranches");

            migrationBuilder.DropColumn(
                name: "NextFeeVoucherNum",
                table: "tblSysSchoolBranches");

            migrationBuilder.DropColumn(
                name: "NextStuNum",
                table: "tblSysSchoolBranches");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "tblDefSchoolStudentMaster");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "tblSysSchoolBranches",
                newName: "email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblStudentHomeWork",
                table: "tblStudentHomeWork",
                column: "HomeworkDate");
        }
    }
}
