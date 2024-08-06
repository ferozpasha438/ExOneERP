using AutoMapper;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.FinanceMgtDtos
{
    [AutoMap(typeof(TblFinSysBatchSetup))]
    public class TblFinSysBatchSetupDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(50)]
        public string BatchCode { get; set; }
        [Required]
        [StringLength(150)]
        public string BatchName { get; set; }
        [StringLength(150)]
        [Required]
        public string BatchName2 { get; set; }
        public bool IsActive { get; set; }
    }

    [AutoMap(typeof(TblFinSysSegmentTwoSetup))]
    public class TblFinSysSegmentTwoSetupDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(50)]
        public string Seg2Code { get; set; }
        [Required]
        [StringLength(150)]
        public string Seg2Name { get; set; }
        [StringLength(150)]
        [Required]
        public string Seg2Name2 { get; set; }
        public bool IsActive { get; set; }

    }
    [AutoMap(typeof(TblFinSysSegmentSetup))]
    public class TblFinSysSegmentSetupDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(50)]
        public string Seg2Code { get; set; }
        [Required]
        [StringLength(150)]
        public string Seg2Name { get; set; }
        [StringLength(150)]
        [Required]
        public string Seg2Name2 { get; set; }
        public bool IsActive { get; set; }

    }

    [AutoMap(typeof(TblFinSysCostAllocationSetup))]
    public class TblFinSysCostAllocationSetupDto : PrimaryKeyDto<int>
    {
        [StringLength(100)]
        [Required]
        public string CostType { get; set; }
        [Required]
        [StringLength(50)]
        public string CostCode { get; set; }
        [Required]
        [StringLength(150)]
        public string CostName { get; set; }
        [StringLength(150)]
        [Required]
        public string CostName2 { get; set; }
        public bool IsActive { get; set; }

    }

    public class TblFinDefAccountBranchMappingDto : PrimaryKeyDto<int>
    {      
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [Required]
        [StringLength(50)]
        public string FinBranchName { get; set; }
        [StringLength(50)]
        public string InventoryAccount { get; set; }
        [StringLength(50)]
        public string CashPurchase { get; set; }
        [StringLength(50)]
        public string CostofSalesAccount { get; set; }
        [StringLength(50)]
        public string InventoryAdjustment { get; set; }
        [StringLength(50)]
        public string DefaultSalesAccount { get; set; }
        [StringLength(50)]
        public string DefaultSalesReturn { get; set; }
        [StringLength(50)]
        public string InventoryTransfer { get; set; }
        [StringLength(50)]
        public string DefaultPayable { get; set; }
        [StringLength(50)]
        public string CostCorrection { get; set; }
        [StringLength(50)]
        public string WIPUsageConsumption { get; set; }
        [StringLength(50)]
        public string Reserved { get; set; }
    }

    public class TblErpSysAcCodeSegmentDto : PrimaryKeyDto<int>
    {
        
        [StringLength(50)]
        public string CodeType1 { get; set; }
        public short Segment1 { get; set; }
        [StringLength(50)]
        public string CodeType2 { get; set; }
        public short Segment2 { get; set; }
        [StringLength(50)]
        public string CodeType3 { get; set; }
        public short Segment3 { get; set; }
        
    }

    #region For Mapping BranchesMainAccounts

    public class TblFinDefBranchesMainAccountsDto : AuditableActiveEntityDto<int>
    {
        [StringLength(50)]
        public string FinAcCode { get; set; } //Reference FinAcCode        
        public string FinAcDesc { get; set; } //Reference  BranchCode       

    }
    public class CreateBranchesMainAccountsDto
    {
        public List<string> AcCodeList { get; set; }
        public string FinBranchCode { get; set; }

    }

    #endregion

    [AutoMap(typeof(TblErpSysSystemTax))]
    public class TblErpSysSystemTaxDto : AuditableActiveEntityDto<int>
    {
        [StringLength(20)]
        [Required]
        public string TaxCode { get; set; }
        [StringLength(100)]
        [Required]
        public string TaxName { get; set; }
        public bool IsInterState { get; set; }


        [StringLength(10)]
        [Required]
        public string TaxComponent01 { get; set; }
        [Required]
        public decimal Taxper01 { get; set; }
        [StringLength(50)]
        [Required]
        public string InputAcCode01 { get; set; }
        [StringLength(50)]
        [Required]
        public string OutputAcCode01 { get; set; }


        [StringLength(10)]
        public string TaxComponent02 { get; set; }
        public decimal? Taxper02 { get; set; }
        [StringLength(50)]
        public string InputAcCode02 { get; set; }
        [StringLength(50)]
        public string OutputAcCode02 { get; set; }

        [StringLength(10)]
        public string TaxComponent03 { get; set; }
        public decimal? Taxper03 { get; set; }
        [StringLength(50)]
        public string InputAcCode03 { get; set; }
        [StringLength(50)]
        public string OutputAcCode03 { get; set; }

        [StringLength(10)]
        public string TaxComponent04 { get; set; }
        public decimal? Taxper04 { get; set; }
        [StringLength(50)]
        public string InputAcCode04 { get; set; }
        [StringLength(50)]
        public string OutputAcCode04 { get; set; }

        [StringLength(10)]
        public string TaxComponent05 { get; set; }
        public decimal? Taxper05 { get; set; }
        [StringLength(50)]
        public string InputAcCode05 { get; set; }
        [StringLength(50)]
        public string OutputAcCode05 { get; set; }

    }



    #region For Binding To PayCode & CheckLeave

    [AutoMap(typeof(TblFinDefAccountlPaycodes))]
    public class FinDefAccountlPaycodesDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string FinPayCode { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; }//Reference  BranchCode
        [Required]
        [StringLength(10)]
        public string FinPayType { get; set; }
        //  [Required]
        [StringLength(50)]
        [Required]
        public string FinPayName { get; set; }
        [StringLength(50)]
        public string FinPayAcIntgrAC { get; set; }//Reference FinAcCode
        [StringLength(50)]
        public string FinPayPDCClearAC { get; set; }//Reference FinAcCode
        public bool IsActive { get; set; }
        public bool UseByOtherBranches { get; set; }
        public bool SystemGenCheckBook { get; set; }


        [Required]
        public int StChkNum { get; set; }
        [Required]
        public int EndChkNum { get; set; }
        [StringLength(10)]
       // [Required]
        public string CheckLeavePrefix { get; set; }

    }


    public class FinDefBankPayCodeCheckLeavesDto : FinDefAccountlPaycodesDto
    {
        //public int Id { get; set; }
        //[StringLength(20)]
        //public string FinPayCode { get; set; }
        //[StringLength(20)]
        //public string FinBranchCode { get; set; }//Reference  BranchCode
        //[Required]
        //[StringLength(10)]
        //public string FinPayType { get; set; }
        ////  [Required]
        //[StringLength(50)]
        //public string FinPayName { get; set; }
        //[StringLength(50)]
        //public string FinPayAcIntgrAC { get; set; }//Reference FinAcCode
        //[StringLength(50)]
        //public string FinPayPDCClearAC { get; set; }//Reference FinAcCode
        //public bool IsActive { get; set; }
        //public bool UseByOtherBranches { get; set; }
        //public bool SystemGenCheckBook { get; set; }


        //For CheckLeave
        //[Required]
        //public int StChkNum { get; set; }
        //[Required]
        //public int EndChkNum { get; set; }
        //[StringLength(10)]
        //public string CheckLeavePrefix { get; set; }


    }
    #endregion

    #region For Binding To AccountCategoty Page

    public class AccountAccountCodeDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string FinAcCode { get; set; }
        [Required]
        [StringLength(200)]
        public string FinAcName { get; set; }
        [StringLength(200)]
        public string FinAcDesc { get; set; }
        [StringLength(50)]
        public string FinAcAlias { get; set; }
    }
    public class AccountSubCategoryDto
    {
        [StringLength(20)]
        public string FinCatCode { get; set; } //Reference FinCatCode
        [StringLength(20)]
        public string FinSubCatCode { get; set; }
        [StringLength(50)]
        public string FinSubCatName { get; set; }
        public List<AccountAccountCodeDto> List { get; set; }

    }
    public class AccountCategoryDto
    {
        [StringLength(20)]
        public string FinCatCode { get; set; }
        [StringLength(50)]
        public string FinCatName { get; set; }

        public List<AccountSubCategoryDto> List { get; set; }
    }

    public class AccountRootCategoryDto
    {
        //public int Sequence { get; set; }
        public string Name { get; set; }
        public List<AccountCategoryDto> List { get; set; }

    }

    public class AccountRootCategoryListDto
    {
        public bool FInSysGenAcCode { get; set; }
        public List<AccountRootCategoryDto> List { get; set; }
    }

    #endregion


    [AutoMap(typeof(TblFinDefMainAccounts))]
    public class TblFinDefMainAccountsDto : PrimaryKeyDto<int>
    {
        [StringLength(50)]
        public string FinAcCode { get; set; }
        [Required]
        [StringLength(200)]
        public string FinAcName { get; set; }
        [StringLength(200)]
        [Required]
        public string FinAcDesc { get; set; }
        [StringLength(50)]
        [Required]
        public string FinAcAlias { get; set; }
        public bool FinIsPayCode { get; set; }

        [StringLength(10)]
        public string FinPayCodetype { get; set; }
        public bool FinIsIntegrationAC { get; set; }
        [StringLength(15)]
        public string Fintype { get; set; } //Reference  TypeCode
        [StringLength(20)]
        public string FinCatCode { get; set; } //Reference  tblFinDefAccountCategory FinCatCode
        [StringLength(20)]
        public string FinSubCatCode { get; set; } //Reference  tblFinDefAccountSubCategory FinSubCatCode        
        public bool FinIsRevenue { get; set; }        
        [StringLength(15)]
        public string FinIsRevenuetype { get; set; } //Reference  TypeCode
        public short FinActLastSeq { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? Status { get; set; }

    }

    [AutoMap(typeof(TblFinDefAccountSubCategory))]
    public class TblFinDefAccountSubCategoryDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string FinCatCode { get; set; } //Reference FinCatCode
        [StringLength(20)]
        [Required]
        public string FinSubCatCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FinSubCatName { get; set; }
        //[Required]
        public short FinSubCatLastSeq { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }

    [AutoMap(typeof(TblFinDefAccountCategory))]
    public class TblFinDefAccountCategoryDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string FinCatCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FinCatName { get; set; }
        [StringLength(15)]
        [Required]
        public string FinType { get; set; } //Reference  TypeCode
        //[Required]
        public short FinCatLastSeq { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    [AutoMap(typeof(TblFinSysAccountType))]
    public class TblFinSysAccountTypeDto : PrimaryKeyDto<int>
    {
        [StringLength(15)]
        public string TypeCode { get; set; }
        [StringLength(2)]
        public string TypeBal { get; set; }

    }

    [AutoMap(typeof(TblFinSysFinanialSetup))]
    public class TblFinSysFinanialSetupDto : PrimaryKeyDto<int>
    {
        [Required]
        public DateTime FYOpenDate { get; set; }
        [Required]
        public DateTime FYClosingDate { get; set; }
        [Required]
        public short FYYear { get; set; }
        [Required]
        public sbyte FinAcCatLen { get; set; }
        [Required]
        public sbyte FinAcSubCatLen { get; set; }
        [Required]
        public sbyte FinAcLen { get; set; }
        public sbyte FinBranchPrefixLen { get; set; }
        [StringLength(50)]
        public string FinAcFormat { get; set; }
        public bool FinAllowNextYearTran { get; set; }
        public bool FinTranDateAsPostDate { get; set; }
        public bool FInSysGenAcCode { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public short NumOfSeg { get; set; }
        public bool UserCostSeg { get; set; }
        [Required]
        public decimal? MinCutOffShortAmt { get; set; } = 0; 
        [Required]
        public decimal? MaxCutOffOverAmr { get; set; } = 0;
        public bool? ArDistFlag { get; set; } = false;
    }

    #region Adding AccountBranches

    public class TblFinDistributionItemDto
    {
        public string FinBranchCode { get; set; }
        public string FinBranchName { get; set; }

        public List<CustomSelectListItem> List1 { get; set; }
        public List<CustomSelectListItem> List2 { get; set; }
        public List<CustomSelectListItem> List3 { get; set; }
        public List<CustomSelectListItem> List4 { get; set; }
        public List<CustomSelectListItem> List5 { get; set; }
        public List<CustomSelectListItem> List6 { get; set; }
        public List<CustomSelectListItem> List7 { get; set; }
        public List<CustomSelectListItem> List8 { get; set; }
        public List<CustomSelectListItem> List9 { get; set; }
        public List<CustomSelectListItem> List10 { get; set; }
        public List<CustomSelectListItem> List11 { get; set; }
    }

    public class TblFinDefAccountAuthorityBranchesDto : TblFinDefAccountBranchesDto
    {
        public List<TblFinDefBranchesAuthorityDto> AuthList { get; set; }
    }

    [AutoMap(typeof(TblFinDefAccountBranches))]
    public class TblFinDefAccountBranchesDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [Required]
        [StringLength(20)]
        public string FinBranchPrefix { get; set; }
        [Required]
        [StringLength(50)]
        public string FinBranchName { get; set; }
        [Required]
        [StringLength(150)]
        public string FinBranchDesc { get; set; }
        [Required]
        [StringLength(500)]
        public string FinBranchAddress { get; set; }
        [Required]
        [StringLength(10)]
        public string FinBranchType { get; set; }
        public bool FinBranchIsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    [AutoMap(typeof(TblFinDefBranchesAuthority))]
    public class TblFinDefBranchesAuthorityDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [StringLength(20)]
        public string AppAuth { get; set; } //Reference User Login Table
        public short AppLevel { get; set; }
        public bool AppAuthBV { get; set; }
        public bool AppAuthCV { get; set; }
        public bool AppAuthJV { get; set; }
        public bool AppAuthAP { get; set; }
        public bool AppAuthAR { get; set; }
        public bool AppAuthFA { get; set; }
        public bool AppAuthBR { get; set; }
        public bool AppAuthPurcOrder { get; set; }
        public bool AppAuthPurcRequest { get; set; }
        public bool AppAuthPurcReturn { get; set; }
        public bool AppAuthTrans { get; set; }
        public bool AppAuthAdj { get; set; }
        public bool AppAuthIssue { get; set; }
        public bool AppAuthRect { get; set; }
        public bool IsFinalAuthority { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    #endregion
}
