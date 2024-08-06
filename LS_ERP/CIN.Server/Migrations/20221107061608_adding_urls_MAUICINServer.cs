using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.Server.Migrations
{
    public partial class adding_urls_MAUICINServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdmUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrmUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FltUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HraUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrmUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrsUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CINServerMetaData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MfgUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpmUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PopUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScpUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SctUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SndUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtlUrl",
                table: "CINServerMetaData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "CrmUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "FinUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "FltUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "HraUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "HrmUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "HrsUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "InvUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "MfgUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "OpmUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "PopUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "PosUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "SchUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "ScpUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "SctUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "SndUrl",
                table: "CINServerMetaData");

            migrationBuilder.DropColumn(
                name: "UtlUrl",
                table: "CINServerMetaData");
        }
    }
}
