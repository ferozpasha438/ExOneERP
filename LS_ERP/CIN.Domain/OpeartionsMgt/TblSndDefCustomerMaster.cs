using CIN.Domain.FinanceMgt;

using CIN.Domain.SalesSetup;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblSndDefCustomerMaster")]
    public class TblSndDefCustomerMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string CustCode { get; set; }
        [StringLength(200)]
        [Required]
        public string CustName { get; set; }
        [StringLength(200)]
        public string CustArbName { get; set; }
        [StringLength(50)]
        public string CustAlias { get; set; }
        public short CustType { get; set; }

      
        [ForeignKey(nameof(CustCatCode))]
        public TblSndDefCustomerCategory SndDefCustomerCategory { get; set; }
        [StringLength(50)]
        public string VATNumber { get; set; }

        [Required]
        public string CustCatCode { get; set; }
        //[ForeignKey(nameof(CustCatId))]
        // public int CustCatId { get; set; }                      //reference Category Table-tblSndDefCustomerCategory
        public short CustRating { get; set; }


        [ForeignKey(nameof(SalesTermsCode))]
        public TblSndDefSalesTermsCode SndDefSalestermsCode { get; set; }
        [Required]
        public string SalesTermsCode { get; set; }
       
        //public int CustTermsid { get; set; }                    //reference terms table-tblSndDefSalesTermsCode

        [Column(TypeName = "decimal(17,3)")]
        public decimal CustDiscount { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal CustCrLimit { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal? CustOutStandBal { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal? CustAvailCrLimit { get; set; }

        [StringLength(20)]
        public string CustSalesRep { get; set; }                 //reference user code-as of now no data
        [StringLength(100)]
        public string CustSalesArea { get; set; }

        [ForeignKey(nameof(CustARAc))]
        public TblFinDefMainAccounts CustARAcMA { get; set; }
        public string CustARAc { get; set; }                        //Reference Account Code-tblFinDefMainAccounts
        [Column(TypeName = "date")]
        public DateTime CustLastPaidDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime CustLastSalesDate { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal CustLastPayAmt { get; set; }
        [StringLength(500)]
        public string CustAddress1 { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(CustCityCode1))]
        public TblErpSysCityCode sysCityCode1 { get; set; }

        [Required]
        public string CustCityCode1 { get; set; }                    
        //public int CustStateId1 { get; set; }
        //public int CustCountryId1 { get; set; }
        [StringLength(50)]
        public string CustMobile1 { get; set; }
        [StringLength(50)]
        public string CustPhone1 { get; set; }
        [StringLength(500)]
        public string CustEmail1 { get; set; }
        [StringLength(200)]
        public string CustContact1 { get; set; }
        [StringLength(500)]
        public string CustAddress2 { get; set; }

        [ForeignKey(nameof(CustCityCode2))]
        public TblErpSysCityCode sysCityCode2 { get; set; }
       // [Required]
        public string CustCityCode2 { get; set; }
        //public int CustStateId2 { get; set; }
        //public int CustCountryId2 { get; set; }
        [StringLength(50)]
        public string CustMobile2 { get; set; }
        [StringLength(50)]
        public string CustPhone2 { get; set; }
        [StringLength(500)]
        public string CustEmail2 { get; set; }
        [StringLength(200)]
        public string CustContact2 { get; set; }
        [StringLength(200)]
        public string CustUDF1 { get; set; }
        [StringLength(200)]
        public string CustUDF2 { get; set; }
        [StringLength(200)]
        public string CustUDF3 { get; set; }
     // public bool CustIsActive { get; set; }
        public bool CustAllowCrsale { get; set; }
        public bool CustAlloCrOverride { get; set; }
        public bool CustOnHold { get; set; }
        public bool CustAlloChkPay { get; set; }
        public bool CustSetPriceLevel { get; set; }
        public short CustPriceLevel { get; set; }
        public bool CustIsVendor { get; set; }
        public bool CustArAcBranch { get; set; }
        [ForeignKey(nameof(CustArAcCode))]
        public TblFinDefMainAccounts sysCustArAcCode { get; set; }
        [Required]
        public string CustArAcCode { get; set; }                            //Reference Account Code-tblFinDefMainAccounts
        [ForeignKey(nameof(CustDefExpAcCode))]
        public TblFinDefMainAccounts sysCustDefExpAcCode { get; set; }
        [Required]
        public string CustDefExpAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        [ForeignKey(nameof(CustARAdjAcCode))]
        public TblFinDefMainAccounts sysCustARAdjAcCode { get; set; }
        [Required]
        public string CustARAdjAcCode { get; set; }                         //Reference Account Code-tblFinDefMainAccounts
        [ForeignKey(nameof(CustARDiscAcCode))]
        public TblFinDefMainAccounts sysCustARDiscAcCode { get; set; }
        [Required]
        public string CustARDiscAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        [StringLength(50)]
        public string CrNumber { get; set; }

        [StringLength(200)]
        public string CustNameAliasEn { get; set; }
        [StringLength(200)]
        public string CustNameAliasAr { get; set; }

    }
}
