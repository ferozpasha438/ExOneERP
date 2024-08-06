using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_operations_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSndDefServiceEnquiryHeader",
                columns: table => new
                {
                    EnquiryNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DateOfEnquiry = table.Column<DateTime>(type: "date", nullable: false),
                    EstimateClosingDate = table.Column<DateTime>(type: "date", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalEstPrice = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StusEnquiryHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefServiceEnquiryHeader", x => x.EnquiryNumber);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceEnquiryHeader_tblSndDefCustomerMaster_CustomerCode",
                        column: x => x.CustomerCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSiteMaster",
                columns: table => new
                {
                    SiteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SiteArbName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SiteAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SiteCityCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SiteGeoLatitude = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    SiteGeoLongitude = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    SiteGeoGain = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSiteMaster", x => x.SiteCode);
                    table.ForeignKey(
                        name: "FK_tblSndDefSiteMaster_tblErpSysCityCode_SiteCityCode",
                        column: x => x.SiteCityCode,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefSiteMaster_tblSndDefCustomerMaster_CustomerCode",
                        column: x => x.CustomerCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSurveyFormElement",
                columns: table => new
                {
                    FormElementCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ElementEngName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ElementArbName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ElementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ListValueString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinValue = table.Column<int>(type: "int", nullable: true),
                    MaxValue = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSurveyFormElement", x => x.FormElementCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSurveyFormHead",
                columns: table => new
                {
                    SurveyFormCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SurveyFormNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SurveyFormNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSurveyFormHead", x => x.SurveyFormCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSurveyor",
                columns: table => new
                {
                    SurveyorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SurveyorNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SurveyorNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSurveyor", x => x.SurveyorCode);
                    table.ForeignKey(
                        name: "FK_tblSndDefSurveyor_tblErpSysCompanyBranches_Branch",
                        column: x => x.Branch,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefUnitMaster",
                columns: table => new
                {
                    UnitCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefUnitMaster", x => x.UnitCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefVendorMaster",
                columns: table => new
                {
                    VendCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VendName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VendArbName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendType = table.Column<short>(type: "smallint", nullable: false),
                    VendCatCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    VendRating = table.Column<short>(type: "smallint", nullable: false),
                    PoTermsCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    VendDiscount = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    VendCrLimit = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    VendPoRep = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VendPoArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VendARAc = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    VendLastPaidDate = table.Column<DateTime>(type: "date", nullable: false),
                    VendLastPoDate = table.Column<DateTime>(type: "date", nullable: false),
                    VendLastPayAmt = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    VendAddress1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendCityCode1 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    VendMobile1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendPhone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendEmail1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendContact1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendAddress2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendCityCode2 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    VendMobile2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendPhone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendEmail2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendContact2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendUDF1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendUDF2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendUDF3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendAllowCrsale = table.Column<bool>(type: "bit", nullable: false),
                    VendAlloCrOverride = table.Column<bool>(type: "bit", nullable: false),
                    VendOnHold = table.Column<bool>(type: "bit", nullable: false),
                    VendAlloChkPay = table.Column<bool>(type: "bit", nullable: false),
                    VendSetPriceLevel = table.Column<bool>(type: "bit", nullable: false),
                    VendPriceLevel = table.Column<short>(type: "smallint", nullable: false),
                    VendIsVendor = table.Column<bool>(type: "bit", nullable: false),
                    VendArAcBranch = table.Column<bool>(type: "bit", nullable: false),
                    VendArAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VendDefExpAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VendARAdjAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VendARDiscAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefVendorMaster", x => x.VendCode);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblErpSysCityCode_VendCityCode1",
                        column: x => x.VendCityCode1,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblErpSysCityCode_VendCityCode2",
                        column: x => x.VendCityCode2,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblFinDefMainAccounts_VendARAc",
                        column: x => x.VendARAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblFinDefMainAccounts_VendArAcCode",
                        column: x => x.VendArAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblFinDefMainAccounts_VendARAdjAcCode",
                        column: x => x.VendARAdjAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblFinDefMainAccounts_VendARDiscAcCode",
                        column: x => x.VendARDiscAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblFinDefMainAccounts_VendDefExpAcCode",
                        column: x => x.VendDefExpAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblPopDefVendorCategory_VendCatCode",
                        column: x => x.VendCatCode,
                        principalTable: "tblPopDefVendorCategory",
                        principalColumn: "VenCatCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefVendorMaster_tblPopDefVendorPOTermsCode_PoTermsCode",
                        column: x => x.PoTermsCode,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblTranVenInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpCreditNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: true),
                    InvoiceDueDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    LpoContract = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvoiceStatusId = table.Column<int>(type: "int", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountBeforeTax = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalPayment = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsCreditConverted = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    InvoiceModule = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InvoiceNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceDate1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTranVenInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblTranVenInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblTranVenInvoice_tblPopDefVendorPOTermsCode_PaymentTerms",
                        column: x => x.PaymentTerms,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefServiceMaster",
                columns: table => new
                {
                    ServiceCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ServiceNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SurveyFormCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefServiceMaster", x => x.ServiceCode);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceMaster_tblSndDefSurveyFormHead_SurveyFormCode",
                        column: x => x.SurveyFormCode,
                        principalTable: "tblSndDefSurveyFormHead",
                        principalColumn: "SurveyFormCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSurveyFormElementsMapp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyFormCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FormElementCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSurveyFormElementsMapp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSndDefSurveyFormElementsMapp_tblSndDefSurveyFormElement_FormElementCode",
                        column: x => x.FormElementCode,
                        principalTable: "tblSndDefSurveyFormElement",
                        principalColumn: "FormElementCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefSurveyFormElementsMapp_tblSndDefSurveyFormHead_SurveyFormCode",
                        column: x => x.SurveyFormCode,
                        principalTable: "tblSndDefSurveyFormHead",
                        principalColumn: "SurveyFormCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnVendorPayment",
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
                    Preparedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnVendorPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorPayment_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorPayment_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorPayment_tblSndDefVendorMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnVendorApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    AppRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnVendorApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorApproval_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorApproval_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorApproval_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorApproval_tblSndDefVendorMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorApproval_TblTranVenInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TblTranVenInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnVendorInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditDays = table.Column<short>(type: "smallint", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Remarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnVendorInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorInvoice_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorInvoice_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorInvoice_tblSndDefVendorMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorInvoice_TblTranVenInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TblTranVenInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnVendorStatement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranSource = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PamentCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnVendorStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorStatement_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorStatement_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorStatement_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorStatement_tblSndDefVendorMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnVendorStatement_TblTranVenInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TblTranVenInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblTranVenInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditId = table.Column<long>(type: "bigint", nullable: true),
                    CreditMemoId = table.Column<long>(type: "bigint", nullable: true),
                    DebitMemoId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AmountBeforeTax = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaxTariffPercentage = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Discount = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTranVenInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblTranVenInvoiceItem_tblTranDefProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblTranDefProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblTranVenInvoiceItem_TblTranVenInvoice_CreditId",
                        column: x => x.CreditId,
                        principalTable: "TblTranVenInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefServiceEnquiries",
                columns: table => new
                {
                    EnquiryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnquiryNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ServiceCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ServiceQuantity = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    StatusEnquiry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SurveyorCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefServiceEnquiries", x => x.EnquiryID);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceEnquiries_tblSndDefServiceEnquiryHeader_EnquiryNumber",
                        column: x => x.EnquiryNumber,
                        principalTable: "tblSndDefServiceEnquiryHeader",
                        principalColumn: "EnquiryNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceEnquiries_tblSndDefServiceMaster_ServiceCode",
                        column: x => x.ServiceCode,
                        principalTable: "tblSndDefServiceMaster",
                        principalColumn: "ServiceCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceEnquiries_tblSndDefSiteMaster_SiteCode",
                        column: x => x.SiteCode,
                        principalTable: "tblSndDefSiteMaster",
                        principalColumn: "SiteCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceEnquiries_tblSndDefUnitMaster_UnitCode",
                        column: x => x.UnitCode,
                        principalTable: "tblSndDefUnitMaster",
                        principalColumn: "UnitCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefServiceUnitMap",
                columns: table => new
                {
                    ServiceCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefServiceUnitMap", x => new { x.ServiceCode, x.UnitCode });
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceUnitMap_tblSndDefServiceMaster_ServiceCode",
                        column: x => x.ServiceCode,
                        principalTable: "tblSndDefServiceMaster",
                        principalColumn: "ServiceCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefServiceUnitMap_tblSndDefUnitMaster_UnitCode",
                        column: x => x.UnitCode,
                        principalTable: "tblSndDefUnitMaster",
                        principalColumn: "UnitCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_BranchCode",
                table: "tblFinTrnVendorApproval",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_CompanyId",
                table: "tblFinTrnVendorApproval",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_CustCode",
                table: "tblFinTrnVendorApproval",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_InvoiceId",
                table: "tblFinTrnVendorApproval",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_LoginId",
                table: "tblFinTrnVendorApproval",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorApproval_TranNumber",
                table: "tblFinTrnVendorApproval",
                column: "TranNumber",
                unique: true,
                filter: "[TranNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_BranchCode",
                table: "tblFinTrnVendorInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_CompanyId",
                table: "tblFinTrnVendorInvoice",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_CustCode",
                table: "tblFinTrnVendorInvoice",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceId",
                table: "tblFinTrnVendorInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_InvoiceNumber",
                table: "tblFinTrnVendorInvoice",
                column: "InvoiceNumber",
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorInvoice_LoginId",
                table: "tblFinTrnVendorInvoice",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorPayment_BranchCode",
                table: "tblFinTrnVendorPayment",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorPayment_CompanyId",
                table: "tblFinTrnVendorPayment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorPayment_CustCode",
                table: "tblFinTrnVendorPayment",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_BranchCode",
                table: "tblFinTrnVendorStatement",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_CompanyId",
                table: "tblFinTrnVendorStatement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_CustCode",
                table: "tblFinTrnVendorStatement",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_InvoiceId",
                table: "tblFinTrnVendorStatement",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_LoginId",
                table: "tblFinTrnVendorStatement",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnVendorStatement_TranNumber",
                table: "tblFinTrnVendorStatement",
                column: "TranNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceEnquiries_EnquiryNumber",
                table: "tblSndDefServiceEnquiries",
                column: "EnquiryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceEnquiries_ServiceCode",
                table: "tblSndDefServiceEnquiries",
                column: "ServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceEnquiries_SiteCode",
                table: "tblSndDefServiceEnquiries",
                column: "SiteCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceEnquiries_UnitCode",
                table: "tblSndDefServiceEnquiries",
                column: "UnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceEnquiryHeader_CustomerCode",
                table: "tblSndDefServiceEnquiryHeader",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceMaster_SurveyFormCode",
                table: "tblSndDefServiceMaster",
                column: "SurveyFormCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefServiceUnitMap_UnitCode",
                table: "tblSndDefServiceUnitMap",
                column: "UnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSiteMaster_CustomerCode",
                table: "tblSndDefSiteMaster",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSiteMaster_SiteCityCode",
                table: "tblSndDefSiteMaster",
                column: "SiteCityCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSurveyFormElementsMapp_FormElementCode",
                table: "tblSndDefSurveyFormElementsMapp",
                column: "FormElementCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSurveyFormElementsMapp_SurveyFormCode",
                table: "tblSndDefSurveyFormElementsMapp",
                column: "SurveyFormCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefSurveyor_Branch",
                table: "tblSndDefSurveyor",
                column: "Branch");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_PoTermsCode",
                table: "tblSndDefVendorMaster",
                column: "PoTermsCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendARAc",
                table: "tblSndDefVendorMaster",
                column: "VendARAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendArAcCode",
                table: "tblSndDefVendorMaster",
                column: "VendArAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendARAdjAcCode",
                table: "tblSndDefVendorMaster",
                column: "VendARAdjAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendARDiscAcCode",
                table: "tblSndDefVendorMaster",
                column: "VendARDiscAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendCatCode",
                table: "tblSndDefVendorMaster",
                column: "VendCatCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendCityCode1",
                table: "tblSndDefVendorMaster",
                column: "VendCityCode1");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendCityCode2",
                table: "tblSndDefVendorMaster",
                column: "VendCityCode2");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefVendorMaster_VendDefExpAcCode",
                table: "tblSndDefVendorMaster",
                column: "VendDefExpAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranVenInvoice_BranchCode",
                table: "TblTranVenInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranVenInvoice_PaymentTerms",
                table: "TblTranVenInvoice",
                column: "PaymentTerms");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranVenInvoiceItem_CreditId",
                table: "TblTranVenInvoiceItem",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_TblTranVenInvoiceItem_ProductId",
                table: "TblTranVenInvoiceItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFinTrnVendorApproval");

            migrationBuilder.DropTable(
                name: "tblFinTrnVendorInvoice");

            migrationBuilder.DropTable(
                name: "tblFinTrnVendorPayment");

            migrationBuilder.DropTable(
                name: "tblFinTrnVendorStatement");

            migrationBuilder.DropTable(
                name: "tblSndDefServiceEnquiries");

            migrationBuilder.DropTable(
                name: "tblSndDefServiceUnitMap");

            migrationBuilder.DropTable(
                name: "tblSndDefSurveyFormElementsMapp");

            migrationBuilder.DropTable(
                name: "tblSndDefSurveyor");

            migrationBuilder.DropTable(
                name: "TblTranVenInvoiceItem");

            migrationBuilder.DropTable(
                name: "tblSndDefVendorMaster");

            migrationBuilder.DropTable(
                name: "tblSndDefServiceEnquiryHeader");

            migrationBuilder.DropTable(
                name: "tblSndDefSiteMaster");

            migrationBuilder.DropTable(
                name: "tblSndDefServiceMaster");

            migrationBuilder.DropTable(
                name: "tblSndDefUnitMaster");

            migrationBuilder.DropTable(
                name: "tblSndDefSurveyFormElement");

            migrationBuilder.DropTable(
                name: "TblTranVenInvoice");

            migrationBuilder.DropTable(
                name: "tblSndDefSurveyFormHead");
        }
    }
}
