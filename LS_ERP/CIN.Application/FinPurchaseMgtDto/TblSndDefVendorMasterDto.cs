using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.FinPurchaseMgtDto
{
    [AutoMap(typeof(TblSndDefVendorMaster))]
    public class TblSndDefVendorMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
       // [StringLength(20)]
        
        public string VendCode { get; set; }
        [StringLength(200)]
      //  [Required]
        public string VendName { get; set; }
        [StringLength(200)]
        public string VendArbName { get; set; }
        [StringLength(50)]
        public string VendAlias { get; set; }
        public short VendType { get; set; }

        [StringLength(50)]
        public string VATNumber { get; set; }

        //  [Required]
        public string VendCatCode { get; set; }
        //[ForeignKey(nameof(VendCatId))]
        // public int VendCatId { get; set; }                      //reference Category Table-tblSndDefVendorCategory
        public short VendRating { get; set; }


       
  //      [Required]
        public string PoTermsCode { get; set; }

        //public int VendTermsid { get; set; }                    //reference terms table-tblSndDefPoTermsCode

        
        public decimal VendDiscount { get; set; }
      
        public decimal VendCrLimit { get; set; }        
        [StringLength(20)]
        public string VendPoRep { get; set; }                 //reference user code-as of now no data
        [StringLength(100)]
        public string VendPoArea { get; set; }

      
        public string VendARAc { get; set; }                        //Reference Account Code-tblFinDefMainAccounts
       
        public DateTime VendLastPaidDate { get; set; }
    
        public DateTime VendLastPoDate { get; set; }
       
        public decimal VendLastPayAmt { get; set; }
        [StringLength(500)]
        public string VendAddress1 { get; set; }
        [StringLength(20)]
      

     //   [Required]
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

        
     //   [Required]
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
       
    //    [Required]
        public string VendArAcCode { get; set; }                            //Reference Account Code-tblFinDefMainAccounts
       
     //   [Required]
        public string VendDefExpAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

       
     //   [Required]
        public string VendARAdjAcCode { get; set; }                         //Reference Account Code-tblFinDefMainAccounts
      
      //  [Required]
        public string VendARDiscAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        public string stateone { get; set; }
        public string countryone { get; set; }
        public string statetwo { get; set; }
        public string countrytwo { get; set; }

        public decimal? VendOutStandBal { get; set; }
        public decimal? VendAvailCrLimit { get; set; }
        public string Iban { get; set; }
        public string CrNumber { get; set; }
    }
}
