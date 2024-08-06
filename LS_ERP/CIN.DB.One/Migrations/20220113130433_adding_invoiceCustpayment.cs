using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invoiceCustpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherSeq",
                table: "tblSequenceNumberSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks2",
                table: "tblFinTrnCustomerStatement",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks1",
                table: "tblFinTrnCustomerStatement",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tblFinTrnCustomerPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    VoucherNumber = table.Column<int>(type: "int", nullable: false),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Checkdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preparedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnCustomerPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerPayment_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerPayment_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerPayment_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerPayment_BranchCode",
                table: "tblFinTrnCustomerPayment",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerPayment_CompanyId",
                table: "tblFinTrnCustomerPayment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerPayment_CustCode",
                table: "tblFinTrnCustomerPayment",
                column: "CustCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnCustomerPayment");

            migrationBuilder.DropColumn(
                name: "VoucherSeq",
                table: "tblSequenceNumberSetting");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks2",
                table: "tblFinTrnCustomerStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks1",
                table: "tblFinTrnCustomerStatement",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
