using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invoice_nvoiceTranNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerStatement_TranNumber",
                table: "tblFinTrnCustomerStatement");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_TranNumber",
                table: "tblFinTrnCustomerStatement",
                column: "TranNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblFinTrnCustomerStatement_TranNumber",
                table: "tblFinTrnCustomerStatement");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_TranNumber",
                table: "tblFinTrnCustomerStatement",
                column: "TranNumber",
                unique: true);
        }
    }
}
