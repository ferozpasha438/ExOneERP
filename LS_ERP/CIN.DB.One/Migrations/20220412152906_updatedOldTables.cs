using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class updatedOldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "tblSndDefSurveyor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "tblSndDefServiceEnquiryHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConvertedToProject",
                table: "tblSndDefServiceEnquiryHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "tblSndDefServiceEnquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedSurveyor",
                table: "tblSndDefServiceEnquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurveyCompleted",
                table: "tblSndDefServiceEnquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurveyInProgress",
                table: "tblSndDefServiceEnquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "tblOpProjectBudgetEstimation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isApplicableForFinancialExpense",
                table: "tblOpOperationExpenseHead",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "tblOpMonthlyRoasterForSite",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "OP_HRM_TEMP_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConvertedToProposal",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConvrtedToContract",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEstimationCompleted",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpenceOverheadsAssigned",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLogisticsAssigned",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaterialAssigned",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsResourcesAssigned",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShiftsAssigned",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkillSetsMapped",
                table: "OP_HRM_TEMP_Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tblOpAuthorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppAuth = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppLevel = table.Column<short>(type: "smallint", nullable: false),
                    CanApproveEnquiry = table.Column<bool>(type: "bit", nullable: false),
                    CanAddSurveyorToEnquiry = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveSurvey = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveEstimation = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveProposal = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveContract = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyEstimation = table.Column<bool>(type: "bit", nullable: false),
                    CanConvertEnqToProject = table.Column<bool>(type: "bit", nullable: false),
                    CanConvertEstimationToProposal = table.Column<bool>(type: "bit", nullable: false),
                    CanConvertProposalToContract = table.Column<bool>(type: "bit", nullable: false),
                    CanCreateRoaster = table.Column<bool>(type: "bit", nullable: false),
                    CanEditRoaster = table.Column<bool>(type: "bit", nullable: false),
                    IsFinalAuthority = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpAuthorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpContractTermsMapToProject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContractTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpContractTermsMapToProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpPaymentTermsToProject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Particular = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InstDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpPaymentTermsToProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectFinancialExpenseCosting",
                columns: table => new
                {
                    FinancialExpenseCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetCostingId = table.Column<int>(type: "int", nullable: false),
                    FinancialExpenseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CostPerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectFinancialExpenseCosting", x => x.FinancialExpenseCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOprTrnApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ServiceCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppAuth = table.Column<int>(type: "int", nullable: false),
                    AppRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOprTrnApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblOprTrnApprovals_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblOprTrnApprovals_BranchCode",
                table: "tblOprTrnApprovals",
                column: "BranchCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpAuthorities");

            migrationBuilder.DropTable(
                name: "tblOpContractTermsMapToProject");

            migrationBuilder.DropTable(
                name: "tblOpPaymentTermsToProject");

            migrationBuilder.DropTable(
                name: "tblOpProjectFinancialExpenseCosting");

            migrationBuilder.DropTable(
                name: "tblOprTrnApprovals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tblSndDefSurveyor");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "tblSndDefServiceEnquiryHeader");

            migrationBuilder.DropColumn(
                name: "IsConvertedToProject",
                table: "tblSndDefServiceEnquiryHeader");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "tblSndDefServiceEnquiries");

            migrationBuilder.DropColumn(
                name: "IsAssignedSurveyor",
                table: "tblSndDefServiceEnquiries");

            migrationBuilder.DropColumn(
                name: "IsSurveyCompleted",
                table: "tblSndDefServiceEnquiries");

            migrationBuilder.DropColumn(
                name: "IsSurveyInProgress",
                table: "tblSndDefServiceEnquiries");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "tblOpProjectBudgetEstimation");

            migrationBuilder.DropColumn(
                name: "isApplicableForFinancialExpense",
                table: "tblOpOperationExpenseHead");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "tblOpMonthlyRoasterForSite");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsConvertedToProposal",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsConvrtedToContract",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsEstimationCompleted",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsExpenceOverheadsAssigned",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsLogisticsAssigned",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsMaterialAssigned",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsResourcesAssigned",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsShiftsAssigned",
                table: "OP_HRM_TEMP_Project");

            migrationBuilder.DropColumn(
                name: "IsSkillSetsMapped",
                table: "OP_HRM_TEMP_Project");
        }
    }
}
