using AutoMapper;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.SystemSetupDtos
{


    [AutoMap(typeof(TblErpSysLogin))]
    public class TblErpSysLoginDto : PrimaryKeyDto<int>
    {
        [StringLength(128)]
        [Required]
        public string LoginId { get; set; }
        [StringLength(128)]
        [Required]
        public string Password { get; set; }
        [StringLength(100)]
        [Required]
        public string UserName { get; set; }
        [StringLength(15)]
        [Required]
        public string UserType { get; set; } //Admin, Manager, etc
        [StringLength(256)]
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [StringLength(256)]
        [Required]
        public string SwpireCardId { get; set; }


        [StringLength(20)]
        [Required]
        public string PrimaryBranch { get; set; } // Ref branchCode
        [StringLength(128)]
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public string[] BranchCodes { get; set; }
        public string LoginType { get; set; }
    }


    #region For PermissionPage 


    public class ChildLink
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public List<SubChildLink> Children { get; set; }
    }
    public class SubChildLink
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
    }

    public class RootLink
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<ChildLink> Children { get; set; }
        public bool Checked { get; set; }
        public bool Collapsed { get; set; }
        public bool Disabled { get; set; }
    }



    #endregion

    public class MenuItemFlatNodeListDto
    {
        public int UserId { get; set; }
        public List<string> Nodes { get; set; }
    }

    public class MenuPermissionNodeDto
    {
        public ADMINISTRATIONMANAGEMENT ADMINISTRATIONMANAGEMENT { get; set; }
        public FINANCEMANAGEMENT FINANCEMANAGEMENT { get; set; }
    }
    public class ADMINISTRATIONMANAGEMENT
    {
        public List<string> System { get; set; }
    }
    public class FINANCEMANAGEMENT
    {
        public List<string> Finance { get; set; }
    }


    // public class MenuPermissionNodeTwoDto
    //{
    //    public string Level1 { get; set; }
    //    public string Level2 { get; set; }
    //    public string Level3 { get; set; }
    //}

    public class MenuItemFlatNodeDto
    {
        public string Item { get; set; }
        public int Level { get; set; }
        public bool Expandable { get; set; }
    }


    public class GetSideMenuOptionListDto
    {
        public string ModuleEn { get; set; }
        public string ModuleAr { get; set; }
        public string SubModuleEn { get; set; }
        public string SubModuleAr { get; set; }
        public bool HasMultiItems { get; set; }
        public MainModuleMenuOptionDto MainModule { get; set; }
        public List<SubModuleMenuOptionDto> SubModules { get; set; }
        public List<MultiListMenuOptionDto> MItems { get; set; }
        public List<TblErpSysMenuOptionDto> Items { get; set; }
        //public List<TblErpSysMenuOptionDto> ArItems { get; set; }
    }

    public class MultiListMenuOptionDto
    {
        public MainModuleMenuOptionDto SubModule { get; set; }
        public List<SubModuleMenuOptionDto> Links { get; set; }
    }

    public class MainModuleMenuOptionDto
    {
        public string ModuleEn { get; set; }
        public string ModuleAr { get; set; }
        public string Module { get; set; }

    }
    public class SubModuleMenuOptionDto
    {
        [StringLength(10)]
        public string MenuNameEng { get; set; }
        [StringLength(10)]
        public string MenuNameArb { get; set; }
        public bool IsForm { get; set; }
        [StringLength(40)]
        public string Path { get; set; }
    }

    [AutoMap(typeof(TblErpSysMenuOption))]
    public class TblErpSysMenuOptionDto : PrimaryKeyDto<int>
    {
        [StringLength(10)]
        public string MenuCode { get; set; }
        public sbyte Level1 { get; set; }
        public sbyte Level2 { get; set; }
        public sbyte Level3 { get; set; }
        [StringLength(10)]
        public string MenuNameEng { get; set; }
        [StringLength(10)]
        public string MenuNameArb { get; set; }
        public bool IsForm { get; set; }
        [StringLength(40)]
        public string Path { get; set; }
    }

    [AutoMap(typeof(TblErpSysCompany))]
    public class TblErpSysCompanyDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        [Required]
        public string CompanyNameAr { get; set; }

        [Required]
        [StringLength(500)]
        public string CompanyAddress { get; set; }

        [StringLength(500)]
        [Required]
        public string CompanyAddressAr { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        [Required]
        public string VATNumber { get; set; }
        [StringLength(12)]
        [Required]
        public string DateFormat { get; set; }
        [StringLength(15)]
        [Required]
        public string GeoLocLatitude { get; set; }
        [StringLength(15)]
        [Required]
        public string GeoLocLongitude { get; set; }
        [StringLength(100)]
        [Required]
        public string LogoURL { get; set; }
        [StringLength(1)]
        [Required]
        public string PriceDecimal { get; set; }
        [StringLength(1)]
        [Required]
        public string QuantityDecimal { get; set; }

        [StringLength(20)]
        [Required]
        public string City { get; set; }

        [StringLength(20)]
        [Required]
        public string State { get; set; }

        [StringLength(50)]
        [Required]
        public string Country { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [StringLength(100)]
        [Required]
        public string Website { get; set; }
        [StringLength(200)]
        [Required]
        public string LogoImagePath { get; set; }
        [StringLength(50)]
        public string CrNumber { get; set; }
        [StringLength(80)]
        public string CcNumber { get; set; }
        [StringLength(200)]
        public string HeaderLogo { get; set; }
        [StringLength(200)]
        public string FooterLogo { get; set; }
        public string Csr { get; set; }
        [StringLength(256)]
        public string DevRequestId { get; set; }
        public string DevCsid { get; set; }
        [StringLength(256)]
        public string DevSecret { get; set; }


        [StringLength(256)]
        public string ProdRequestId { get; set; }
        public string ProdCsid { get; set; }
        [StringLength(256)]
        public string ProdSecret { get; set; }

    }

    public class ErpSysCompanyDto : PrimaryKeyDto<int>
    {
        public string CompanyName { get; set; }

        public string CompanyNameAr { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyAddressAr { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string VATNumber { get; set; }
        public string DateFormat { get; set; }
        public string GeoLocLatitude { get; set; }
        public string GeoLocLongitude { get; set; }
        public string LogoURL { get; set; }
        public string PriceDecimal { get; set; }
        public string QuantityDecimal { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string LogoImagePath { get; set; }
        public string CrNumber { get; set; }
        public string CcNumber { get; set; }
        public string HeaderLogo { get; set; }
        public string FooterLogo { get; set; }
        public string Csr { get; set; }
        public string DevRequestId { get; set; }
        public string DevCsid { get; set; }
        public string DevSecret { get; set; }
        public string ProdRequestId { get; set; }
        public string ProdCsid { get; set; }
        public string ProdSecret { get; set; }

    }

    [AutoMap(typeof(TblErpSysCompanyBranch))]
    public class TblErpSysCompanyBranchDto : AuditableActiveEntityDto<int>
    {
        //[ForeignKey(nameof(CompanyId))]
        //public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }
        [Required]
        public int? ZoneId { get; set; }
        public string CompanyName { get; set; }

        [StringLength(20)]
        [Required]
        public string BranchCode { get; set; }

        [StringLength(150)]
        [Required]
        public string BankName { get; set; }

        [StringLength(150)]
        [Required]
        public string BankNameAr { get; set; }

        [StringLength(100)]
        [Required]
        public string BranchName { get; set; }
        [StringLength(500)]
        [Required]
        public string BranchAddress { get; set; }
        [StringLength(500)]
        public string BranchAddressAr { get; set; }
        [StringLength(80)]
        [Required]
        public string AccountNumber { get; set; }
        [StringLength(20)]
        [Required]
        public string Phone { get; set; }
        [StringLength(20)]
        [Required]
        public string Mobile { get; set; }
        [StringLength(20)]
        [Required]
        public string City { get; set; }
        [StringLength(20)]
        [Required]
        public string State { get; set; }
        [StringLength(100)]
        [Required]
        public string AuthorityName { get; set; }
        [StringLength(15)]
        public string GeoLocLatitude { get; set; }
        [StringLength(15)]
        public string GeoLocLongitude { get; set; }
        [StringLength(512)]
        public string Remarks { get; set; }
        [StringLength(80)]
        public string Iban { get; set; }


    }

    public class ErpSysCompanyBranchDto : AuditableActiveEntityDto<int>
    {
        public int CompanyId { get; set; }
        public int? ZoneId { get; set; }
        public string CompanyName { get; set; }

        public string BranchCode { get; set; }

        public string BankName { get; set; }

        public string BankNameAr { get; set; }

        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchAddressAr { get; set; }
        public string AccountNumber { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AuthorityName { get; set; }
        public string GeoLocLatitude { get; set; }
        public string GeoLocLongitude { get; set; }
        public string Remarks { get; set; }
        public string Iban { get; set; }


    }

    [AutoMap(typeof(TblErpSysCityCode))]
    public class TblErpSysCityCodeDto : AuditableActiveEntityDto<int>
    {
        [StringLength(10)]
        public string CountryCode { get; set; }
        [StringLength(10)]
        public string CityCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CityName { get; set; }
        [StringLength(100)]
        public string CityNameAr { get; set; }

        public string StateCode { get; set; }
    }

    [AutoMap(typeof(TblErpSysCurrencyCode))]
    public class TblErpSysCurrencyCodeDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string CurrencyName { get; set; }
        [StringLength(10)]
        [Required]
        public string CountryCode { get; set; }
        //[Column(TypeName = "decimal(10, 5)")]
        [Required]
        public float BuyingRate { get; set; }
        //[Column(TypeName = "decimal(10, 5)")]
        [Required]
        public float SellingRate { get; set; }
        public DateTime? Lastupdated { get; set; }
    }


}
