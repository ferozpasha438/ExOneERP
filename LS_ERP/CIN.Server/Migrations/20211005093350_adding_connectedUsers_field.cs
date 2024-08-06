using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.Server.Migrations
{
    public partial class adding_connectedUsers_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ConnectedUsers",
                table: "CINServerMetaData",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedUsers",
                table: "CINServerMetaData");
        }
    }
}
