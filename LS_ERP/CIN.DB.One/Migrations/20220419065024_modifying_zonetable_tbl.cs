using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_zonetable_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "tblErpSysCompanyBranches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysCompanyBranches_ZoneId",
                table: "tblErpSysCompanyBranches",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblErpSysCompanyBranches_tblErpSysZoneSetting_ZoneId",
                table: "tblErpSysCompanyBranches",
                column: "ZoneId",
                principalTable: "tblErpSysZoneSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblErpSysCompanyBranches_tblErpSysZoneSetting_ZoneId",
                table: "tblErpSysCompanyBranches");

            migrationBuilder.DropIndex(
                name: "IX_tblErpSysCompanyBranches_ZoneId",
                table: "tblErpSysCompanyBranches");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "tblErpSysCompanyBranches");
        }
    }
}
