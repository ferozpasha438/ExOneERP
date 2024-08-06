using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class mdifying_sch2_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaidOn",
                table: "tblDefStudentFeeHeader",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "tblDefSchoolStudentMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AcademicYear",
                table: "tblDefSchoolStudentMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StuIDNumber",
                table: "tblDefSchoolStudentMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblOpContractClause",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClauseTitleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseSubTitleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseDescriptionEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseTitleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseSubTitleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseDescriptionArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberArb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractClause", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpContractFormHead",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TitleOfServiceEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDetailsEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDetailsEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreambleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPartyEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleOfServiceArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDetailsArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDetailsArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreambleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPartyArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractFormHead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpContractTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleOfServiceEng = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CompanyDetailsEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDetailsEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreambleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPartyEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleOfServiceArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDetailsArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDetailsArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreambleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPartyArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblStudentHomeWork",
                columns: table => new
                {
                    HomeworkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeWorkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStudentHomeWork", x => x.HomeworkDate);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchooScheduleEvents",
                columns: table => new
                {
                    HDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchooScheduleEvents", x => x.HDate);
                });

            migrationBuilder.CreateTable(
                name: "tblTranFeeTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptVoucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeeTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeStructCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidTransNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JvNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidOnline = table.Column<bool>(type: "bit", nullable: false),
                    PaidManual = table.Column<bool>(type: "bit", nullable: false),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedByUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTranFeeTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpContractClausesToContractFormMap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractFormId = table.Column<long>(type: "bigint", nullable: false),
                    ClauseTitleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseSubTitleEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseDescriptionEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseTitleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseSubTitleArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClauseDescriptionArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractClausesToContractFormMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblOpContractClausesToContractFormMap_tblOpContractFormHead_ContractFormId",
                        column: x => x.ContractFormId,
                        principalTable: "tblOpContractFormHead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblOpContractTemplateToContractClauseMap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    ContractClauseId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractTemplateToContractClauseMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblOpContractTemplateToContractClauseMap_tblOpContractClause_ContractClauseId",
                        column: x => x.ContractClauseId,
                        principalTable: "tblOpContractClause",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblOpContractTemplateToContractClauseMap_tblOpContractTemplate_ContractTemplateId",
                        column: x => x.ContractTemplateId,
                        principalTable: "tblOpContractTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblOpContractClausesToContractFormMap_ContractFormId",
                table: "tblOpContractClausesToContractFormMap",
                column: "ContractFormId");

            migrationBuilder.CreateIndex(
                name: "IX_tblOpContractTemplateToContractClauseMap_ContractClauseId",
                table: "tblOpContractTemplateToContractClauseMap",
                column: "ContractClauseId");

            migrationBuilder.CreateIndex(
                name: "IX_tblOpContractTemplateToContractClauseMap_ContractTemplateId",
                table: "tblOpContractTemplateToContractClauseMap",
                column: "ContractTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpContractClausesToContractFormMap");

            migrationBuilder.DropTable(
                name: "tblOpContractTemplateToContractClauseMap");

            migrationBuilder.DropTable(
                name: "tblStudentHomeWork");

            migrationBuilder.DropTable(
                name: "tblSysSchooScheduleEvents");

            migrationBuilder.DropTable(
                name: "tblTranFeeTransaction");

            migrationBuilder.DropTable(
                name: "tblOpContractFormHead");

            migrationBuilder.DropTable(
                name: "tblOpContractClause");

            migrationBuilder.DropTable(
                name: "tblOpContractTemplate");

            migrationBuilder.DropColumn(
                name: "StuIDNumber",
                table: "tblDefSchoolStudentMaster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaidOn",
                table: "tblDefStudentFeeHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedOn",
                table: "tblDefSchoolStudentMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "AcademicYear",
                table: "tblDefSchoolStudentMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
