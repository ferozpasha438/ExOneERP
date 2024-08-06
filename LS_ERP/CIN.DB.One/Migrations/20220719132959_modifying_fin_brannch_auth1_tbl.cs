using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_fin_brannch_auth1_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "EmployeeNameAr",
            //    table: "tblOpEmployeesToProjectSite",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthPurcOrder",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthPurcRequest",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthPurcReturn",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNameAr",
                table: "tblOpEmployeesToProjectSite");

            migrationBuilder.DropColumn(
                name: "AppAuthPurcOrder",
                table: "tblFinDefBranchesAuthority");

            migrationBuilder.DropColumn(
                name: "AppAuthPurcRequest",
                table: "tblFinDefBranchesAuthority");

            migrationBuilder.DropColumn(
                name: "AppAuthPurcReturn",
                table: "tblFinDefBranchesAuthority");
        }
    }
}
