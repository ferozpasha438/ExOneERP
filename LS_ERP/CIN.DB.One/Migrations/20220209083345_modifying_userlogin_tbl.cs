using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_userlogin_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tblErpSysLogin",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysLogin_UserName",
                table: "tblErpSysLogin",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblErpSysLogin_UserName",
                table: "tblErpSysLogin");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblErpSysLogin");
        }
    }
}
