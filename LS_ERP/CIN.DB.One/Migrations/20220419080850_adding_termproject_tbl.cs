using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_termproject_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeID",
                table: "tblOpMonthlyRoasterForSite",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MapId",
                table: "tblOpMonthlyRoasterForSite",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeID",
                table: "tblOpEmployeesToProjectSite",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "tblOpEmployeesToProjectSite",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblOpEmployeeToResourceMap",
                columns: table => new
                {
                    MapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SkillSet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpEmployeeToResourceMap", x => x.MapId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpEmployeeToResourceMap");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "tblOpMonthlyRoasterForSite");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "tblOpMonthlyRoasterForSite");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "tblOpEmployeesToProjectSite");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeID",
                table: "tblOpEmployeesToProjectSite",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
