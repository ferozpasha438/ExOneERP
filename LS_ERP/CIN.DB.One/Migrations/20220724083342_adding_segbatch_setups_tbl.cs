using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_segbatch_setups_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "NumOfSeg",
                table: "tblFinSysFinanialSetup",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "UserCostSeg",
                table: "tblFinSysFinanialSetup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CityNameAr",
                table: "tblErpSysCityCode",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblFinSysBatchSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysBatchSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFinSysCostAllocationSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysCostAllocationSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFinSysSegmentSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seg2Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seg2Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seg2Name2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysSegmentSetup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinSysBatchSetup");

            migrationBuilder.DropTable(
                name: "tblFinSysCostAllocationSetup");

            migrationBuilder.DropTable(
                name: "tblFinSysSegmentSetup");

            migrationBuilder.DropColumn(
                name: "NumOfSeg",
                table: "tblFinSysFinanialSetup");

            migrationBuilder.DropColumn(
                name: "UserCostSeg",
                table: "tblFinSysFinanialSetup");

            migrationBuilder.DropColumn(
                name: "CityNameAr",
                table: "tblErpSysCityCode");
        }
    }
}
