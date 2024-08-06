using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_cvbvvouchers_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CDate",
                table: "tblFinTrnJournalVoucher",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CDate",
                table: "tblFinTrnCashVoucher",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CDate",
                table: "tblFinTrnBankVoucher",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CDate",
                table: "tblFinTrnJournalVoucher");

            migrationBuilder.DropColumn(
                name: "CDate",
                table: "tblFinTrnCashVoucher");

            migrationBuilder.DropColumn(
                name: "CDate",
                table: "tblFinTrnBankVoucher");
        }
    }
}
