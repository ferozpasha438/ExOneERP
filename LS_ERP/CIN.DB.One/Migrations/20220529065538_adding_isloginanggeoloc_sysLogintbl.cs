using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_isloginanggeoloc_sysLogintbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasGoeSiteId",
                table: "tblErpSysLogin",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLoginAllow",
                table: "tblErpSysLogin",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasGoeSiteId",
                table: "tblErpSysLogin");

            migrationBuilder.DropColumn(
                name: "IsLoginAllow",
                table: "tblErpSysLogin");
        }
    }
}
