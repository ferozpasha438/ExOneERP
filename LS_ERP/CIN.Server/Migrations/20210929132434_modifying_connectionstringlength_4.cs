using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.Server.Migrations
{
    public partial class modifying_connectionstringlength_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CINServerMetaData_CINNumber",
                table: "CINServerMetaData",
                column: "CINNumber",
                unique: true,
                filter: "[CINNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CINServerMetaData_CINNumber",
                table: "CINServerMetaData");
        }
    }
}
