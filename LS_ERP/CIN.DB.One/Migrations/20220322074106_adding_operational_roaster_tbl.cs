using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_operational_roaster_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                table: "tblFinTrnDistribution",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "HRM_DEF_EmployeeShift",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false),
                    MondayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    TuesdayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    WednesdayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    ThursdayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    FridayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    SaturdayShiftId = table.Column<short>(type: "smallint", nullable: true),
                    SundayShiftId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_DEF_EmployeeShift", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HRM_DEF_EmployeeShiftMaster",
                columns: table => new
                {
                    ShiftId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShiftName_EN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShiftName_AR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InGrace = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutGrace = table.Column<TimeSpan>(type: "time", nullable: true),
                    WorkingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    NetWorkingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsOff = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_DEF_EmployeeShiftMaster", x => x.ShiftId);
                });

            migrationBuilder.CreateTable(
                name: "HRM_DEF_PayrollGroup",
                columns: table => new
                {
                    PayrollGroupID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayrollGroupName_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PayrollGroupName_AR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountryID = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: true),
                    SiteID = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessUnitID = table.Column<long>(type: "bigint", nullable: true),
                    DivisionID = table.Column<long>(type: "bigint", nullable: true),
                    BranchID = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentID = table.Column<long>(type: "bigint", nullable: true),
                    IsForAllEmployee = table.Column<bool>(type: "bit", nullable: true),
                    IsForFutureEmployee = table.Column<bool>(type: "bit", nullable: true),
                    StartPayRollDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrentPayRollMonth = table.Column<short>(type: "smallint", nullable: true),
                    EndPayRollDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrentPayRollYear = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_DEF_PayrollGroup", x => x.PayrollGroupID);
                });

            migrationBuilder.CreateTable(
                name: "HRM_TRAN_Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_TRAN_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "OP_HRM_TEMP_Project",
                columns: table => new
                {
                    ProjectCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProjectNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OP_HRM_TEMP_Project", x => x.ProjectCode);
                });

            migrationBuilder.CreateTable(
                name: "tblOpLogisticsandvehicle",
                columns: table => new
                {
                    VehicleNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleNameInEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VehicleNameInArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DailyFuelCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    DailyMiscCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    EstimatedDailyMaintenanceCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    OtherDailyOperationCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    TotalDailyServiceCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    DailyServicePrice = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    ValueofVehicle = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Vehicletype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinMargin = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpLogisticsandvehicle", x => x.VehicleNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblOpMaterialEquipment",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameInEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameInArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EstimatedCostValue = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    IsDepreciationApplicable = table.Column<bool>(type: "bit", nullable: false),
                    MinimumCostPerUsage = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    DepreciationPerYear = table.Column<decimal>(type: "decimal(17,3)", nullable: true),
                    UsageLifetermsYear = table.Column<short>(type: "smallint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpMaterialEquipment", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "tblOpMonthlyRoaster",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    S1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S4 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S5 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S6 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S7 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S8 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S9 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S10 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S11 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S12 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S13 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S14 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S15 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S16 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S17 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S18 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S19 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S20 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S21 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S22 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S23 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S24 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S25 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S26 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S27 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S28 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S29 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S30 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S31 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpMonthlyRoaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpMonthlyRoasterForSite",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    SkillsetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SkillsetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    S1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S4 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S5 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S6 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S7 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S8 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S9 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S10 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S11 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S12 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S13 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S14 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S15 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S16 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S17 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S18 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S19 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S20 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S21 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S22 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S23 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S24 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S25 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S26 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S27 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S28 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S29 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S30 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    S31 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MonthStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    MonthEndDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpMonthlyRoasterForSite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpOperationExpenseHead",
                columns: table => new
                {
                    CostHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostNameInEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CostNameInArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MinServiceCosttoCompany = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    MinServicePrice = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    GrossMargin = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    isApplicableForVehicle = table.Column<bool>(type: "bit", nullable: false),
                    isApplicableForSkillset = table.Column<bool>(type: "bit", nullable: false),
                    isApplicableForMaterial = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpOperationExpenseHead", x => x.CostHead);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectBudgetCosting",
                columns: table => new
                {
                    ProjectBudgetCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetEstimationId = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectBudgetCosting", x => x.ProjectBudgetCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectBudgetEstimation",
                columns: table => new
                {
                    ProjectBudgetEstimationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PreviousEstimatonId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectBudgetEstimation", x => x.ProjectBudgetEstimationId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectLogisticsCosting",
                columns: table => new
                {
                    LogisticsCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetCostingId = table.Column<int>(type: "int", nullable: false),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VehicleNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CostPerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectLogisticsCosting", x => x.LogisticsCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectLogisticsSubCosting",
                columns: table => new
                {
                    LogisticsSubCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogisticsCostingId = table.Column<int>(type: "int", nullable: false),
                    CostHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectLogisticsSubCosting", x => x.LogisticsSubCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectMaterialEquipmentCosting",
                columns: table => new
                {
                    MaterialEquipmentCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetCostingId = table.Column<int>(type: "int", nullable: false),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaterialEquipmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CostPerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectMaterialEquipmentCosting", x => x.MaterialEquipmentCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectMaterialEquipmentSubCosting",
                columns: table => new
                {
                    MaterialEquipmentSubCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialEquipmentCostingId = table.Column<int>(type: "int", nullable: false),
                    CostHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectMaterialEquipmentSubCosting", x => x.MaterialEquipmentSubCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectResourceCosting",
                columns: table => new
                {
                    ResourceCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetCostingId = table.Column<int>(type: "int", nullable: false),
                    SkillsetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectResourceCosting", x => x.ResourceCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProjectResourceSubCosting",
                columns: table => new
                {
                    ResourceSubCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceCostingId = table.Column<int>(type: "int", nullable: false),
                    CostHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProjectResourceSubCosting", x => x.ResourceSubCostingId);
                });

            migrationBuilder.CreateTable(
                name: "tblOpShiftSiteMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShiftCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpShiftSiteMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpShiftsPlanForProject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShiftCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpShiftsPlanForProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpSkillset",
                columns: table => new
                {
                    SkillSetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SkillType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameInEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameInArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DetailsOfSkillSet = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SkillSetType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrioryImportanceScale = table.Column<short>(type: "smallint", nullable: false),
                    MinBufferResource = table.Column<short>(type: "smallint", nullable: false),
                    MonthlyCtc = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    CostToCompanyDay = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    MonthlyOverheadCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    MonthlyOtherOverHeads = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    TotalSkillsetCost = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    TotalSkillsetCostDay = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    ServicePriceToCompany = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    MinMarginRequired = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    OverrideMarginIfRequired = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibilitiesRoles = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpSkillset", x => x.SkillSetCode);
                });

            migrationBuilder.CreateTable(
                name: "tblOpSkillsetPlanForProject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SkillsetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Quantity = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpSkillsetPlanForProject", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRM_DEF_EmployeeShift");

            migrationBuilder.DropTable(
                name: "HRM_DEF_EmployeeShiftMaster");

            migrationBuilder.DropTable(
                name: "HRM_DEF_PayrollGroup");

            migrationBuilder.DropTable(
                name: "HRM_TRAN_Employee");

            migrationBuilder.DropTable(
                name: "OP_HRM_TEMP_Project");

            migrationBuilder.DropTable(
                name: "tblOpLogisticsandvehicle");

            migrationBuilder.DropTable(
                name: "tblOpMaterialEquipment");

            migrationBuilder.DropTable(
                name: "tblOpMonthlyRoaster");

            migrationBuilder.DropTable(
                name: "tblOpMonthlyRoasterForSite");

            migrationBuilder.DropTable(
                name: "tblOpOperationExpenseHead");

            migrationBuilder.DropTable(
                name: "tblOpProjectBudgetCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectBudgetEstimation");

            migrationBuilder.DropTable(
                name: "tblOpProjectLogisticsCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectLogisticsSubCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectMaterialEquipmentCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectMaterialEquipmentSubCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectResourceCosting");

            migrationBuilder.DropTable(
                name: "tblOpProjectResourceSubCosting");

            migrationBuilder.DropTable(
                name: "tblOpShiftSiteMap");

            migrationBuilder.DropTable(
                name: "tblOpShiftsPlanForProject");

            migrationBuilder.DropTable(
                name: "tblOpSkillset");

            migrationBuilder.DropTable(
                name: "tblOpSkillsetPlanForProject");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                table: "tblFinTrnDistribution",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
