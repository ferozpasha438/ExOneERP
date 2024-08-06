using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.Server.Migrations
{
    public partial class updating_metadata_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ConcurrentUsers",
                table: "CINServerMetaData",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ModueCodes",
                table: "CINServerMetaData",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrentUsers",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "ModueCodes",
                table: "CINServerMetaData");
        }
    }
}
