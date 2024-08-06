using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class mdifying_sch2_branch_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_tblStudentHomeWork",
            //    table: "tblStudentHomeWork");

            //migrationBuilder.AddColumn<string>(
            //    name: "BranchCode",
            //    table: "tblDefSchoolStudentMaster",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_tblStudentHomeWork",
            //    table: "tblStudentHomeWork",
            //    column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_tblStudentHomeWork",
            //    table: "tblStudentHomeWork");

            //migrationBuilder.DropColumn(
            //    name: "BranchCode",
            //    table: "tblDefSchoolStudentMaster");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_tblStudentHomeWork",
            //    table: "tblStudentHomeWork",
            //    column: "HomeworkDate");
        }
    }
}
