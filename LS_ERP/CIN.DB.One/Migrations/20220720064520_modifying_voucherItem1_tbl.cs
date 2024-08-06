using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_voucherItem1_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Batch2",
                table: "tblFinTrnJournalVoucherItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Batch2",
                table: "tblFinTrnCashVoucherItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Batch2",
                table: "tblFinTrnBankVoucherItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Batch2",
                table: "tblFinTrnJournalVoucherItem");

            migrationBuilder.DropColumn(
                name: "Batch2",
                table: "tblFinTrnCashVoucherItem");

            migrationBuilder.DropColumn(
                name: "Batch2",
                table: "tblFinTrnBankVoucherItem");
        }
    }
}
