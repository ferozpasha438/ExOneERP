using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblErpSysCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VATNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateFormat = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    GeoLocLatitude = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    GeoLocLongitude = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LogoURL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriceDecimal = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    QuantityDecimal = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogoImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysCountryCode",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysCountryCode", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysMenuOption",
                columns: table => new
                {
                    MenuCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Level1 = table.Column<short>(type: "smallint", nullable: false),
                    Level2 = table.Column<short>(type: "smallint", nullable: false),
                    Level3 = table.Column<short>(type: "smallint", nullable: false),
                    MenuNameEng = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MenuNameArb = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IsForm = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ModuleName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysMenuOption", x => x.MenuCode);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysSystemTaxes",
                columns: table => new
                {
                    TaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TaxName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsInterState = table.Column<bool>(type: "bit", nullable: false),
                    TaxComponent01 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Taxper01 = table.Column<decimal>(type: "decimal(6,3)", nullable: false),
                    InputAcCode01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OutputAcCode01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxComponent02 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Taxper02 = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    InputAcCode02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OutputAcCode02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxComponent03 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Taxper03 = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    InputAcCode03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OutputAcCode03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxComponent04 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Taxper04 = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    InputAcCode04 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OutputAcCode04 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxComponent05 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Taxper05 = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    InputAcCode05 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OutputAcCode05 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysSystemTaxes", x => x.TaxCode);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysTransactionCodes",
                columns: table => new
                {
                    TransactionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysTransactionCodes", x => x.TransactionCode);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysUserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysUserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefMainAccounts",
                columns: table => new
                {
                    FinAcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinAcName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FinAcDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FinAcAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinIsPayCode = table.Column<bool>(type: "bit", nullable: false),
                    FinPayCodetype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FinIsIntegrationAC = table.Column<bool>(type: "bit", nullable: false),
                    Fintype = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FinCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinSubCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinActLastSeq = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefMainAccounts", x => x.FinAcCode);
                });

            migrationBuilder.CreateTable(
                name: "tblFinSysAccountType",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TypeBal = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysAccountType", x => x.TypeCode);
                });

            migrationBuilder.CreateTable(
                name: "tblFinSysFinanialSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FYOpenDate = table.Column<DateTime>(type: "date", nullable: false),
                    FYClosingDate = table.Column<DateTime>(type: "date", nullable: false),
                    FYYear = table.Column<short>(type: "smallint", nullable: false),
                    FinAcCatLen = table.Column<short>(type: "smallint", nullable: false),
                    FinAcSubCatLen = table.Column<short>(type: "smallint", nullable: false),
                    FinAcLen = table.Column<short>(type: "smallint", nullable: false),
                    FinBranchPrefixLen = table.Column<short>(type: "smallint", nullable: false),
                    FinAcFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinAllowNextYearTran = table.Column<bool>(type: "bit", nullable: false),
                    FinTranDateAsPostDate = table.Column<bool>(type: "bit", nullable: false),
                    FInSysGenAcCode = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinSysFinanialSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefCategory",
                columns: table => new
                {
                    ItemCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemCatDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemCatPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefCategory", x => x.ItemCatCode);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefClass",
                columns: table => new
                {
                    ItemClassCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemClassDesce = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefClass", x => x.ItemClassCode);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefSubClass",
                columns: table => new
                {
                    ItemSubClassCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemSubClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemSubClassDesce = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefSubClass", x => x.ItemSubClassCode);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefUOM",
                columns: table => new
                {
                    UOMCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UOMName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UOMDesc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefUOM", x => x.UOMCode);
                });

            migrationBuilder.CreateTable(
                name: "tblPopDefVendorCategory",
                columns: table => new
                {
                    VenCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VenCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VenCatDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopDefVendorCategory", x => x.VenCatCode);
                });

            migrationBuilder.CreateTable(
                name: "tblPopDefVendorPOTermsCode",
                columns: table => new
                {
                    POTermsCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    POTermsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    POTermsDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    POTermsDueDays = table.Column<short>(type: "smallint", nullable: false),
                    POTermDiscDays = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopDefVendorPOTermsCode", x => x.POTermsCode);
                });

            migrationBuilder.CreateTable(
                name: "tblPopDefVendorShipment",
                columns: table => new
                {
                    ShipmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShipmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipmentDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipmentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopDefVendorShipment", x => x.ShipmentCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefCustomerCategory",
                columns: table => new
                {
                    CustCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustCatDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefCustomerCategory", x => x.CustCatCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSalesShipment",
                columns: table => new
                {
                    ShipmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShipmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipmentDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipmentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSalesShipment", x => x.ShipmentCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefSalesTermsCode",
                columns: table => new
                {
                    SalesTermsCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SalesTermsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesTermsDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesTermsDueDays = table.Column<short>(type: "smallint", nullable: false),
                    SalesTermDiscDays = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefSalesTermsCode", x => x.SalesTermsCode);
                });

            migrationBuilder.CreateTable(
                name: "tblTranDefProductType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTranDefProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTranDefTax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TaxTariffPercentage = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTranDefTax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTranDefUnitType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTranDefUnitType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysCurrencyCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BuyingRate = table.Column<float>(type: "real", nullable: false),
                    SellingRate = table.Column<float>(type: "real", nullable: false),
                    Lastupdated = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysCurrencyCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpSysCurrencyCode_tblErpSysCountryCode_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "tblErpSysCountryCode",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysStateCode",
                columns: table => new
                {
                    StateCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysStateCode", x => x.StateCode);
                    table.ForeignKey(
                        name: "FK_tblErpSysStateCode_tblErpSysCountryCode_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "tblErpSysCountryCode",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinCenterCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinCenterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinCenterType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefCenters_tblFinDefMainAccounts_FinCenterCode",
                        column: x => x.FinCenterCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefDistributionGroup",
                columns: table => new
                {
                    InvDistGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InvAssetAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvNonAssetAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvCashPOAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvCOGSAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvAdjAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvSalesAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvInTransitAc = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    InvDefaultAPAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvCostCorAc = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    InvWIPAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvWriteOffAc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefDistributionGroup", x => x.InvDistGroup);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvAdjAc",
                        column: x => x.InvAdjAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvAssetAc",
                        column: x => x.InvAssetAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvCashPOAC",
                        column: x => x.InvCashPOAC,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvCOGSAc",
                        column: x => x.InvCOGSAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvCostCorAc",
                        column: x => x.InvCostCorAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvDefaultAPAc",
                        column: x => x.InvDefaultAPAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvInTransitAc",
                        column: x => x.InvInTransitAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvNonAssetAc",
                        column: x => x.InvNonAssetAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvSalesAc",
                        column: x => x.InvSalesAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvWIPAc",
                        column: x => x.InvWIPAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefDistributionGroup_tblFinDefMainAccounts_InvWriteOffAc",
                        column: x => x.InvWriteOffAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefAccountCategory",
                columns: table => new
                {
                    FinCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FinCatLastSeq = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefAccountCategory", x => x.FinCatCode);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountCategory_tblFinSysAccountType_FinType",
                        column: x => x.FinType,
                        principalTable: "tblFinSysAccountType",
                        principalColumn: "TypeCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefSubCategory",
                columns: table => new
                {
                    SubCatKey = table.Column<string>(type: "nvarchar(41)", maxLength: 41, nullable: false),
                    ItemSubCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemSubCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemSubCatDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefSubCategory", x => x.SubCatKey);
                    table.ForeignKey(
                        name: "FK_tblInvDefSubCategory_tblInvDefCategory_ItemCatCode",
                        column: x => x.ItemCatCode,
                        principalTable: "tblInvDefCategory",
                        principalColumn: "ItemCatCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblTranDefProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NameAR = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    UnitTypeId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsDefaultConfig = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTranDefProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTranDefProduct_tblTranDefProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "tblTranDefProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblTranDefProduct_tblTranDefUnitType_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "tblTranDefUnitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysCityCode",
                columns: table => new
                {
                    CityCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysCityCode", x => x.CityCode);
                    table.ForeignKey(
                        name: "FK_tblErpSysCityCode_tblErpSysStateCode_StateCode",
                        column: x => x.StateCode,
                        principalTable: "tblErpSysStateCode",
                        principalColumn: "StateCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefAccountSubCategory",
                columns: table => new
                {
                    FinSubCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FinSubCatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinSubCatLastSeq = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefAccountSubCategory", x => x.FinSubCatCode);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountSubCategory_tblFinDefAccountCategory_FinCatCode",
                        column: x => x.FinCatCode,
                        principalTable: "tblFinDefAccountCategory",
                        principalColumn: "FinCatCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysCompanyBranches",
                columns: table => new
                {
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AuthorityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeoLocLatitude = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    GeoLocLongitude = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysCompanyBranches", x => x.BranchCode);
                    table.ForeignKey(
                        name: "FK_tblErpSysCompanyBranches_tblErpSysCityCode_City",
                        column: x => x.City,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpSysCompanyBranches_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSndDefCustomerMaster",
                columns: table => new
                {
                    CustCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustArbName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustType = table.Column<short>(type: "smallint", nullable: false),
                    CustCatCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CustRating = table.Column<short>(type: "smallint", nullable: false),
                    SalesTermsCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CustDiscount = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    CustCrLimit = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    CustSalesRep = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CustSalesArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustARAc = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CustLastPaidDate = table.Column<DateTime>(type: "date", nullable: false),
                    CustLastSalesDate = table.Column<DateTime>(type: "date", nullable: false),
                    CustLastPayAmt = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    CustAddress1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustCityCode1 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CustMobile1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustPhone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustEmail1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustContact1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustAddress2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustCityCode2 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CustMobile2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustPhone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustEmail2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustContact2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustUDF1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustUDF2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustUDF3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustAllowCrsale = table.Column<bool>(type: "bit", nullable: false),
                    CustAlloCrOverride = table.Column<bool>(type: "bit", nullable: false),
                    CustOnHold = table.Column<bool>(type: "bit", nullable: false),
                    CustAlloChkPay = table.Column<bool>(type: "bit", nullable: false),
                    CustSetPriceLevel = table.Column<bool>(type: "bit", nullable: false),
                    CustPriceLevel = table.Column<short>(type: "smallint", nullable: false),
                    CustIsVendor = table.Column<bool>(type: "bit", nullable: false),
                    CustArAcBranch = table.Column<bool>(type: "bit", nullable: false),
                    CustArAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CustDefExpAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CustARAdjAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CustARDiscAcCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSndDefCustomerMaster", x => x.CustCode);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblErpSysCityCode_CustCityCode1",
                        column: x => x.CustCityCode1,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblErpSysCityCode_CustCityCode2",
                        column: x => x.CustCityCode2,
                        principalTable: "tblErpSysCityCode",
                        principalColumn: "CityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblFinDefMainAccounts_CustARAc",
                        column: x => x.CustARAc,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblFinDefMainAccounts_CustArAcCode",
                        column: x => x.CustArAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblFinDefMainAccounts_CustARAdjAcCode",
                        column: x => x.CustARAdjAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblFinDefMainAccounts_CustARDiscAcCode",
                        column: x => x.CustARDiscAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblFinDefMainAccounts_CustDefExpAcCode",
                        column: x => x.CustDefExpAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblSndDefCustomerCategory_CustCatCode",
                        column: x => x.CustCatCode,
                        principalTable: "tblSndDefCustomerCategory",
                        principalColumn: "CustCatCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSndDefCustomerMaster_tblSndDefSalesTermsCode_SalesTermsCode",
                        column: x => x.SalesTermsCode,
                        principalTable: "tblSndDefSalesTermsCode",
                        principalColumn: "SalesTermsCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysLogin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SwpireCardId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PrimaryBranch = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpSysLogin_tblErpSysCompanyBranches_PrimaryBranch",
                        column: x => x.PrimaryBranch,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysTransactionSequence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastSeqNum = table.Column<int>(type: "int", nullable: false),
                    PrefixCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PrefixFinYear = table.Column<bool>(type: "bit", nullable: false),
                    ResetAfterFYClosing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysTransactionSequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpSysTransactionSequence_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpSysTransactionSequence_tblErpSysTransactionCodes_TransactionCode",
                        column: x => x.TransactionCode,
                        principalTable: "tblErpSysTransactionCodes",
                        principalColumn: "TransactionCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefAccountBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FinBranchPrefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinBranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinBranchDesc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FinBranchAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FinBranchType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FinBranchIsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefAccountBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountBranches_tblErpSysCompanyBranches_FinBranchCode",
                        column: x => x.FinBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefAccountlPaycodes",
                columns: table => new
                {
                    FinPayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FinBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FinPayType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FinPayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinPayAcIntgrAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinPayPDCClearAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UseByOtherBranches = table.Column<bool>(type: "bit", nullable: false),
                    SystemGenCheckBook = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefAccountlPaycodes", x => x.FinPayCode);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountlPaycodes_tblErpSysCompanyBranches_FinBranchCode",
                        column: x => x.FinBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountlPaycodes_tblFinDefMainAccounts_FinPayAcIntgrAC",
                        column: x => x.FinPayAcIntgrAC,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinDefAccountlPaycodes_tblFinDefMainAccounts_FinPayPDCClearAC",
                        column: x => x.FinPayPDCClearAC,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefBranchesAuthority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppAuth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppLevel = table.Column<short>(type: "smallint", nullable: false),
                    AppAuthBV = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthCV = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthJV = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthAP = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthAR = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthFA = table.Column<bool>(type: "bit", nullable: false),
                    AppAuthBR = table.Column<bool>(type: "bit", nullable: false),
                    IsFinalAuthority = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefBranchesAuthority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefBranchesAuthority_tblErpSysCompanyBranches_FinBranchCode",
                        column: x => x.FinBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefBranchesMainAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinAcCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefBranchesMainAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefBranchesMainAccounts_tblErpSysCompanyBranches_FinBranchCode",
                        column: x => x.FinBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinDefBranchesMainAccounts_tblFinDefMainAccounts_FinAcCode",
                        column: x => x.FinAcCode,
                        principalTable: "tblFinDefMainAccounts",
                        principalColumn: "FinAcCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefWarehouse",
                columns: table => new
                {
                    WHCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WHDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WHAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WHCity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WHState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WHIncharge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WHBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvDistGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WhAllowDirectPur = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefWarehouse", x => x.WHCode);
                    table.ForeignKey(
                        name: "FK_tblInvDefWarehouse_tblErpSysCompanyBranches_WHBranchCode",
                        column: x => x.WHBranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblInvDefWarehouse_tblInvDefDistributionGroup_InvDistGroup",
                        column: x => x.InvDistGroup,
                        principalTable: "tblInvDefDistributionGroup",
                        principalColumn: "InvDistGroup",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblTranInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceRefNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: true),
                    InvoiceDueDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    LpoContract = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_tblTranInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTranInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysMenuLoginId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysMenuLoginId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpSysMenuLoginId_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpSysMenuLoginId_tblErpSysMenuOption_MenuCode",
                        column: x => x.MenuCode,
                        principalTable: "tblErpSysMenuOption",
                        principalColumn: "MenuCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpSysUserBranch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysUserBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpSysUserBranch_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpSysUserBranch_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCustomerApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_tblFinTrnCustomerApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerApproval_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerApproval_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerApproval_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerApproval_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCustomerInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_tblFinTrnCustomerInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerInvoice_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerInvoice_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerInvoice_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerInvoice_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinTrnCustomerStatement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Remarks1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    DrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinTrnCustomerStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerStatement_tblErpSysCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerStatement_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerStatement_tblErpSysLogin_LoginId",
                        column: x => x.LoginId,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFinTrnCustomerStatement_tblSndDefCustomerMaster_CustCode",
                        column: x => x.CustCode,
                        principalTable: "tblSndDefCustomerMaster",
                        principalColumn: "CustCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFinDefBankCheckLeaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinPayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StChkNum = table.Column<int>(type: "int", nullable: false),
                    EndChkNum = table.Column<int>(type: "int", nullable: false),
                    CheckLeavePrefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    TranSource = table.Column<string>(type: "char(2)", maxLength: 2, nullable: true),
                    UsedByTranNum = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    UsedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false),
                    VoidedOn = table.Column<DateTime>(type: "date", nullable: true),
                    VoidedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsPDC = table.Column<bool>(type: "bit", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "date", nullable: true),
                    IssuedToName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsBounced = table.Column<bool>(type: "bit", nullable: false),
                    BounceReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCleared = table.Column<bool>(type: "bit", nullable: false),
                    ClearedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFinDefBankCheckLeaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFinDefBankCheckLeaves_tblFinDefAccountlPaycodes_FinPayCode",
                        column: x => x.FinPayCode,
                        principalTable: "tblFinDefAccountlPaycodes",
                        principalColumn: "FinPayCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefInventoryConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentralWHCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AutoGenItemCode = table.Column<bool>(type: "bit", nullable: false),
                    PrefixCatCode = table.Column<bool>(type: "bit", nullable: false),
                    NewItemIndicator = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ItemLength = table.Column<short>(type: "smallint", nullable: false),
                    CategoryLength = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefInventoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblInvDefInventoryConfig_tblInvDefWarehouse_CentralWHCode",
                        column: x => x.CentralWHCode,
                        principalTable: "tblInvDefWarehouse",
                        principalColumn: "WHCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblTranInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_tblTranInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTranInvoiceItem_tblTranDefProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblTranDefProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblTranInvoiceItem_tblTranInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "tblTranInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysCityCode_StateCode",
                table: "tblErpSysCityCode",
                column: "StateCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysCompanyBranches_City",
                table: "tblErpSysCompanyBranches",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysCompanyBranches_CompanyId",
                table: "tblErpSysCompanyBranches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysCurrencyCode_CountryCode",
                table: "tblErpSysCurrencyCode",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysLogin_PrimaryBranch",
                table: "tblErpSysLogin",
                column: "PrimaryBranch");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysMenuLoginId_LoginId",
                table: "tblErpSysMenuLoginId",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysMenuLoginId_MenuCode",
                table: "tblErpSysMenuLoginId",
                column: "MenuCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysStateCode_CountryCode",
                table: "tblErpSysStateCode",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysTransactionSequence_BranchCode",
                table: "tblErpSysTransactionSequence",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysTransactionSequence_TransactionCode",
                table: "tblErpSysTransactionSequence",
                column: "TransactionCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysUserBranch_BranchCode",
                table: "tblErpSysUserBranch",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysUserBranch_LoginId",
                table: "tblErpSysUserBranch",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpSysUserType_UerType",
                table: "tblErpSysUserType",
                column: "UerType",
                unique: true,
                filter: "[UerType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountBranches_FinBranchCode",
                table: "tblFinDefAccountBranches",
                column: "FinBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountCategory_FinCatCode",
                table: "tblFinDefAccountCategory",
                column: "FinCatCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountCategory_FinType",
                table: "tblFinDefAccountCategory",
                column: "FinType");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountlPaycodes_FinBranchCode",
                table: "tblFinDefAccountlPaycodes",
                column: "FinBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountlPaycodes_FinPayAcIntgrAC",
                table: "tblFinDefAccountlPaycodes",
                column: "FinPayAcIntgrAC");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountlPaycodes_FinPayPDCClearAC",
                table: "tblFinDefAccountlPaycodes",
                column: "FinPayPDCClearAC");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefAccountSubCategory_FinCatCode",
                table: "tblFinDefAccountSubCategory",
                column: "FinCatCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefBankCheckLeaves_FinPayCode",
                table: "tblFinDefBankCheckLeaves",
                column: "FinPayCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefBranchesAuthority_FinBranchCode",
                table: "tblFinDefBranchesAuthority",
                column: "FinBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefBranchesMainAccounts_FinAcCode",
                table: "tblFinDefBranchesMainAccounts",
                column: "FinAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefBranchesMainAccounts_FinBranchCode",
                table: "tblFinDefBranchesMainAccounts",
                column: "FinBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefCenters_FinCenterCode",
                table: "tblFinDefCenters",
                column: "FinCenterCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinDefMainAccounts_FinAcCode",
                table: "tblFinDefMainAccounts",
                column: "FinAcCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_BranchCode",
                table: "tblFinTrnCustomerApproval",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_CompanyId",
                table: "tblFinTrnCustomerApproval",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_CustCode",
                table: "tblFinTrnCustomerApproval",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_LoginId",
                table: "tblFinTrnCustomerApproval",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerApproval_TranNumber",
                table: "tblFinTrnCustomerApproval",
                column: "TranNumber",
                unique: true,
                filter: "[TranNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_BranchCode",
                table: "tblFinTrnCustomerInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_CompanyId",
                table: "tblFinTrnCustomerInvoice",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_CustCode",
                table: "tblFinTrnCustomerInvoice",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_InvoiceNumber",
                table: "tblFinTrnCustomerInvoice",
                column: "InvoiceNumber",
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerInvoice_LoginId",
                table: "tblFinTrnCustomerInvoice",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_BranchCode",
                table: "tblFinTrnCustomerStatement",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_CompanyId",
                table: "tblFinTrnCustomerStatement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_CustCode",
                table: "tblFinTrnCustomerStatement",
                column: "CustCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_LoginId",
                table: "tblFinTrnCustomerStatement",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFinTrnCustomerStatement_TranNumber",
                table: "tblFinTrnCustomerStatement",
                column: "TranNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvAdjAc",
                table: "tblInvDefDistributionGroup",
                column: "InvAdjAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvAssetAc",
                table: "tblInvDefDistributionGroup",
                column: "InvAssetAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvCashPOAC",
                table: "tblInvDefDistributionGroup",
                column: "InvCashPOAC");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvCOGSAc",
                table: "tblInvDefDistributionGroup",
                column: "InvCOGSAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvCostCorAc",
                table: "tblInvDefDistributionGroup",
                column: "InvCostCorAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvDefaultAPAc",
                table: "tblInvDefDistributionGroup",
                column: "InvDefaultAPAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvInTransitAc",
                table: "tblInvDefDistributionGroup",
                column: "InvInTransitAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvNonAssetAc",
                table: "tblInvDefDistributionGroup",
                column: "InvNonAssetAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvSalesAc",
                table: "tblInvDefDistributionGroup",
                column: "InvSalesAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvWIPAc",
                table: "tblInvDefDistributionGroup",
                column: "InvWIPAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefDistributionGroup_InvWriteOffAc",
                table: "tblInvDefDistributionGroup",
                column: "InvWriteOffAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefInventoryConfig_CentralWHCode",
                table: "tblInvDefInventoryConfig",
                column: "CentralWHCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefSubCategory_ItemCatCode",
                table: "tblInvDefSubCategory",
                column: "ItemCatCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefWarehouse_InvDistGroup",
                table: "tblInvDefWarehouse",
                column: "InvDistGroup");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvDefWarehouse_WHBranchCode",
                table: "tblInvDefWarehouse",
                column: "WHBranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustARAc",
                table: "tblSndDefCustomerMaster",
                column: "CustARAc");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustArAcCode",
                table: "tblSndDefCustomerMaster",
                column: "CustArAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustARAdjAcCode",
                table: "tblSndDefCustomerMaster",
                column: "CustARAdjAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustARDiscAcCode",
                table: "tblSndDefCustomerMaster",
                column: "CustARDiscAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustCatCode",
                table: "tblSndDefCustomerMaster",
                column: "CustCatCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustCityCode1",
                table: "tblSndDefCustomerMaster",
                column: "CustCityCode1");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustCityCode2",
                table: "tblSndDefCustomerMaster",
                column: "CustCityCode2");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_CustDefExpAcCode",
                table: "tblSndDefCustomerMaster",
                column: "CustDefExpAcCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSndDefCustomerMaster_SalesTermsCode",
                table: "tblSndDefCustomerMaster",
                column: "SalesTermsCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranDefProduct_ProductTypeId",
                table: "tblTranDefProduct",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranDefProduct_UnitTypeId",
                table: "tblTranDefProduct",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranInvoice_BranchCode",
                table: "tblTranInvoice",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranInvoiceItem_InvoiceId",
                table: "tblTranInvoiceItem",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTranInvoiceItem_ProductId",
                table: "tblTranInvoiceItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblErpSysCurrencyCode");

            migrationBuilder.DropTable(
                name: "tblErpSysMenuLoginId");

            migrationBuilder.DropTable(
                name: "tblErpSysSystemTaxes");

            migrationBuilder.DropTable(
                name: "tblErpSysTransactionSequence");

            migrationBuilder.DropTable(
                name: "tblErpSysUserBranch");

            migrationBuilder.DropTable(
                name: "tblErpSysUserType");

            migrationBuilder.DropTable(
                name: "tblFinDefAccountBranches");

            migrationBuilder.DropTable(
                name: "tblFinDefAccountSubCategory");

            migrationBuilder.DropTable(
                name: "tblFinDefBankCheckLeaves");

            migrationBuilder.DropTable(
                name: "tblFinDefBranchesAuthority");

            migrationBuilder.DropTable(
                name: "tblFinDefBranchesMainAccounts");

            migrationBuilder.DropTable(
                name: "tblFinDefCenters");

            migrationBuilder.DropTable(
                name: "tblFinSysFinanialSetup");

            migrationBuilder.DropTable(
                name: "tblFinTrnCustomerApproval");

            migrationBuilder.DropTable(
                name: "tblFinTrnCustomerInvoice");

            migrationBuilder.DropTable(
                name: "tblFinTrnCustomerStatement");

            migrationBuilder.DropTable(
                name: "tblInvDefClass");

            migrationBuilder.DropTable(
                name: "tblInvDefInventoryConfig");

            migrationBuilder.DropTable(
                name: "tblInvDefSubCategory");

            migrationBuilder.DropTable(
                name: "tblInvDefSubClass");

            migrationBuilder.DropTable(
                name: "tblInvDefUOM");

            migrationBuilder.DropTable(
                name: "tblPopDefVendorCategory");

            migrationBuilder.DropTable(
                name: "tblPopDefVendorPOTermsCode");

            migrationBuilder.DropTable(
                name: "tblPopDefVendorShipment");

            migrationBuilder.DropTable(
                name: "tblSndDefSalesShipment");

            migrationBuilder.DropTable(
                name: "tblTranDefTax");

            migrationBuilder.DropTable(
                name: "tblTranInvoiceItem");

            migrationBuilder.DropTable(
                name: "tblErpSysMenuOption");

            migrationBuilder.DropTable(
                name: "tblErpSysTransactionCodes");

            migrationBuilder.DropTable(
                name: "tblFinDefAccountCategory");

            migrationBuilder.DropTable(
                name: "tblFinDefAccountlPaycodes");

            migrationBuilder.DropTable(
                name: "tblErpSysLogin");

            migrationBuilder.DropTable(
                name: "tblSndDefCustomerMaster");

            migrationBuilder.DropTable(
                name: "tblInvDefWarehouse");

            migrationBuilder.DropTable(
                name: "tblInvDefCategory");

            migrationBuilder.DropTable(
                name: "tblTranDefProduct");

            migrationBuilder.DropTable(
                name: "tblTranInvoice");

            migrationBuilder.DropTable(
                name: "tblFinSysAccountType");

            migrationBuilder.DropTable(
                name: "tblSndDefCustomerCategory");

            migrationBuilder.DropTable(
                name: "tblSndDefSalesTermsCode");

            migrationBuilder.DropTable(
                name: "tblInvDefDistributionGroup");

            migrationBuilder.DropTable(
                name: "tblTranDefProductType");

            migrationBuilder.DropTable(
                name: "tblTranDefUnitType");

            migrationBuilder.DropTable(
                name: "tblErpSysCompanyBranches");

            migrationBuilder.DropTable(
                name: "tblFinDefMainAccounts");

            migrationBuilder.DropTable(
                name: "tblErpSysCityCode");

            migrationBuilder.DropTable(
                name: "tblErpSysCompany");

            migrationBuilder.DropTable(
                name: "tblErpSysStateCode");

            migrationBuilder.DropTable(
                name: "tblErpSysCountryCode");
        }
    }
}
