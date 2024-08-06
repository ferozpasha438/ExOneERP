using CIN.Domain.FinanceMgt;
using CIN.Domain.PurchaseSetup;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblSndDefVendorMaster")]
    public class TblSndDefVendorMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string VendCode { get; set; }
        [StringLength(200)]
        [Required]
        public string VendName { get; set; }
        [StringLength(200)]
        public string VendArbName { get; set; }
        [StringLength(50)]
        public string VendAlias { get; set; }
        public short VendType { get; set; }

        [StringLength(50)]
        public string VATNumber { get; set; }

        [ForeignKey(nameof(VendCatCode))]
        public TblPopDefVendorCategory SndDefVendorCategory { get; set; }
        [Required]
        public string VendCatCode { get; set; }
        //[ForeignKey(nameof(VendCatId))]
        // public int VendCatId { get; set; }                      //reference Category Table-tblSndDefVendorCategory
        public short VendRating { get; set; }


        [ForeignKey(nameof(PoTermsCode))]
        public TblPopDefVendorPOTermsCode PopDefVendorPOTerms { get; set; }
        [Required]
        public string PoTermsCode { get; set; }

        //public int VendTermsid { get; set; }                    //reference terms table-tblSndDefPoTermsCode

        [Column(TypeName = "decimal(7,2)")]
        public decimal VendDiscount { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal VendCrLimit { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal? VendOutStandBal { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal? VendAvailCrLimit { get; set; }
        [StringLength(20)]
        public string VendPoRep { get; set; }                 //reference user code-as of now no data
        [StringLength(100)]
        public string VendPoArea { get; set; }


        [ForeignKey(nameof(VendARAc))]
        public TblFinDefMainAccounts VendARAcMA { get; set; }
        public string VendARAc { get; set; }                        //Reference Account Code-tblFinDefMainAccounts
        [Column(TypeName = "date")]
        public DateTime VendLastPaidDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime VendLastPoDate { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal VendLastPayAmt { get; set; }
        [StringLength(500)]
        public string VendAddress1 { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(VendCityCode1))]
        public TblErpSysCityCode sysCityCode1 { get; set; }

        [Required]
        public string VendCityCode1 { get; set; }
        //public int VendStateId1 { get; set; }
        //public int VendCountryId1 { get; set; }
        [StringLength(50)]
        public string VendMobile1 { get; set; }
        [StringLength(50)]
        public string VendPhone1 { get; set; }
        [StringLength(500)]
        public string VendEmail1 { get; set; }
        [StringLength(200)]
        public string VendContact1 { get; set; }
        [StringLength(500)]
        public string VendAddress2 { get; set; }

        [ForeignKey(nameof(VendCityCode2))]
        public TblErpSysCityCode sysCityCode2 { get; set; }
       // [Required]
        public string VendCityCode2 { get; set; }


        //public int VendStateId2 { get; set; }
        //public int VendCountryId2 { get; set; }
        [StringLength(50)]
        public string VendMobile2 { get; set; }
        [StringLength(50)]
        public string VendPhone2 { get; set; }
        [StringLength(500)]
        public string VendEmail2 { get; set; }
        [StringLength(200)]
        public string VendContact2 { get; set; }
        [StringLength(200)]
        public string VendUDF1 { get; set; }
        [StringLength(200)]
        public string VendUDF2 { get; set; }
        [StringLength(200)]
        public string VendUDF3 { get; set; }
        // public bool VendIsActive { get; set; }
        public bool VendAllowCrsale { get; set; }
        public bool VendAlloCrOverride { get; set; }
        public bool VendOnHold { get; set; }
        public bool VendAlloChkPay { get; set; }
        public bool VendSetPriceLevel { get; set; }
        public short VendPriceLevel { get; set; }
        public bool VendIsVendor { get; set; }
        public bool VendArAcBranch { get; set; }
        [ForeignKey(nameof(VendArAcCode))]
        public TblFinDefMainAccounts sysVendArAcCode { get; set; }
        [Required]
        public string VendArAcCode { get; set; }                            //Reference Account Code-tblFinDefMainAccounts
        [ForeignKey(nameof(VendDefExpAcCode))]
        public TblFinDefMainAccounts sysVendDefExpAcCode { get; set; }
        [Required]
        public string VendDefExpAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        [ForeignKey(nameof(VendARAdjAcCode))]
        public TblFinDefMainAccounts sysVendARAdjAcCode { get; set; }
        [Required]
        public string VendARAdjAcCode { get; set; }                         //Reference Account Code-tblFinDefMainAccounts
        [ForeignKey(nameof(VendARDiscAcCode))]
        public TblFinDefMainAccounts sysVendARDiscAcCode { get; set; }
        [Required]
        public string VendARDiscAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        [StringLength(80)]
        public string Iban { get; set; }
        [StringLength(50)]
        public string CrNumber { get; set; }
        
    }
}
