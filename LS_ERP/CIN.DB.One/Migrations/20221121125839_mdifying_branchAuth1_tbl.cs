using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class mdifying_branchAuth1_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppAuthAdj",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthIssue",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthRect",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppAuthTrans",
                table: "tblFinDefBranchesAuthority",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppAuthAdj",
                table: "tblFinDefBranchesAuthority");

            migrationBuilder.DropColumn(
                name: "AppAuthIssue",
                table: "tblFinDefBranchesAuthority");

            migrationBuilder.DropColumn(
                name: "AppAuthRect",
                table: "tblFinDefBranchesAuthority");

            migrationBuilder.DropColumn(
                name: "AppAuthTrans",
                table: "tblFinDefBranchesAuthority");
        }
    }
}
