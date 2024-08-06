using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_segtwo_setups_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostAllocation",
                table: "tblFinTrnJournalVoucherItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostSegCode",
                table: "tblFinTrnJournalVoucherItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostAllocation",
                table: "tblFinTrnCashVoucherItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostSegCode",
                table: "tblFinTrnCashVoucherItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostAllocation",
                table: "tblFinTrnBankVoucherItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostSegCode",
                table: "tblFinTrnBankVoucherItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblFinSysSegmentTwoSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seg2Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Seg2Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Seg2Name2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysSegmentTwoSetup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinSysSegmentTwoSetup");

            migrationBuilder.DropColumn(
                name: "CostAllocation",
                table: "tblFinTrnJournalVoucherItem");

            migrationBuilder.DropColumn(
                name: "CostSegCode",
                table: "tblFinTrnJournalVoucherItem");

            migrationBuilder.DropColumn(
                name: "CostAllocation",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "CostSegCode",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "CostAllocation",
                table: "tblFinTrnBankVoucherItem");

            migrationBuilder.DropColumn(
                name: "CostSegCode",
                table: "tblFinTrnBankVoucherItem");
        }
    }
}
