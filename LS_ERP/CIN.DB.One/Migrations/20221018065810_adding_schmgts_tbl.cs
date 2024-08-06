using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_schmgts_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblDefSchoolStudentMaster",
                columns: table => new
                {
                    StuRegNum = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuRegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeSectionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PTGroupCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReligionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherToungue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeStructCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportationRequired = table.Column<bool>(type: "bit", nullable: false),
                    PickNDropZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleTransport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortListDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortListedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofAdmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuConvDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuConvBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhysicalDisability = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalDisabilityNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WearSpects = table.Column<bool>(type: "bit", nullable: false),
                    SpecialAssistance = table.Column<bool>(type: "bit", nullable: false),
                    SpecialAssistanceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicsScale = table.Column<int>(type: "int", nullable: false),
                    AttentivenessScale = table.Column<int>(type: "int", nullable: false),
                    HomeWorkScale = table.Column<int>(type: "int", nullable: false),
                    ProjectWorkScale = table.Column<int>(type: "int", nullable: false),
                    SportsPhysicalScale = table.Column<int>(type: "int", nullable: false),
                    DiciplineAttitude = table.Column<int>(type: "int", nullable: false),
                    SignatureImage1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureImage2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefSchoolStudentMaster", x => x.StuRegNum);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentAcademics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassPercent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentAcademics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile1 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentApplyLeave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisteredPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcademicYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentApplyLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AtnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtnTimeIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtnTimeOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtnFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLeave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentAttendance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentFeeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeStructCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetDiscAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsLateFee = table.Column<bool>(type: "bit", nullable: false),
                    IsAddedManaully = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false),
                    VoidedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoidReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentFeeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentFeeHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeStructCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidTransNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JvNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<int>(type: "int", nullable: false),
                    IsCompletelyPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentFeeHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentGuardiansSiblings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deisgnation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentGuardiansSiblings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosNeg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoticeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    AprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsClosed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentNotices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentNoticesReasonCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequireAction = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentNoticesReasonCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblDefStudentPrevEducation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuAdmNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfInstitute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassStudied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageMedium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearofPassing = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDefStudentPrevEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSchoolMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mobile = table.Column<int>(type: "int", nullable: false),
                    SentBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadFlag = table.Column<bool>(type: "bit", nullable: false),
                    ReadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSchoolMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolHolidayCalanderStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HName2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolHolidayCalanderStudent", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDefSchoolStudentMaster");

            migrationBuilder.DropTable(
                name: "tblDefStudentAcademics");

            migrationBuilder.DropTable(
                name: "tblDefStudentAddress");

            migrationBuilder.DropTable(
                name: "tblDefStudentApplyLeave");

            migrationBuilder.DropTable(
                name: "tblDefStudentAttendance");

            migrationBuilder.DropTable(
                name: "tblDefStudentFeeDetails");

            migrationBuilder.DropTable(
                name: "tblDefStudentFeeHeader");

            migrationBuilder.DropTable(
                name: "tblDefStudentGuardiansSiblings");

            migrationBuilder.DropTable(
                name: "tblDefStudentNotices");

            migrationBuilder.DropTable(
                name: "tblDefStudentNoticesReasonCode");

            migrationBuilder.DropTable(
                name: "tblDefStudentPrevEducation");

            migrationBuilder.DropTable(
                name: "tblSchoolMessages");

            migrationBuilder.DropTable(
                name: "tblSysSchoolHolidayCalanderStudent");
        }
    }
}
