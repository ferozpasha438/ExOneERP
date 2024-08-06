using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefCustomerMaster))]
    public class TblSndDefCustomerMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {


        [StringLength(20)]
        public string CustCode { get; set; }
        [StringLength(200)]

        public string CustName { get; set; }
        [StringLength(200)]
        public string CustArbName { get; set; }
        [StringLength(50)]
        public string CustAlias { get; set; }
        public short CustType { get; set; }

        [StringLength(20)]
        public string CustCatCode { get; set; }

        [StringLength(50)]
        public string VATNumber { get; set; }
        public short CustRating { get; set; }

        [StringLength(20)]
        public string SalesTermsCode { get; set; }
        // [Column(TypeName = "decimal(7,2)")]
        public decimal CustDiscount { get; set; }
        //  [Column(TypeName = "decimal(12,3)")]
        public decimal CustCrLimit { get; set; }
        public decimal? CustOutStandBal { get; set; }
        public decimal? CustAvailCrLimit { get; set; }


        // [StringLength(20)]
        public string CustSalesRep { get; set; }
        [StringLength(100)]
        public string CustSalesArea { get; set; }


        [StringLength(50)]
        public string CustARAc { get; set; }
        //   [Column(TypeName = "date")]
        public DateTime CustLastPaidDate { get; set; }
        // [Column(TypeName = "date")]
        public DateTime CustLastSalesDate { get; set; }
        //   [Column(TypeName = "decimal(12,3)")]
        public decimal CustLastPayAmt { get; set; }
        [StringLength(500)]
        public string CustAddress1 { get; set; }

        [StringLength(20)]
        public string CustCityCode1 { get; set; }
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
      //  [Required]
        [StringLength(20)]
        public string CustCityCode2 { get; set; }
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
        public bool CustAllowCrsale { get; set; }
        public bool CustAlloCrOverride { get; set; }
        public bool CustOnHold { get; set; }
        public bool CustAlloChkPay { get; set; }
        public bool CustSetPriceLevel { get; set; }
        public short CustPriceLevel { get; set; }
        public bool CustIsVendor { get; set; }
        public bool CustArAcBranch { get; set; }

        [StringLength(50)]
        public string CustArAcCode { get; set; }

        [StringLength(50)]
        public string CustDefExpAcCode { get; set; }

        [StringLength(50)]
        public string CustARAdjAcCode { get; set; }

        [StringLength(50)]
        public string CustARDiscAcCode { get; set; }

        public string stateone { get; set; }
        public string countryone { get; set; }
        public string statetwo { get; set; }
        public string countrytwo { get; set; }
        [StringLength(50)]
        public string CrNumber { get; set; }
        [StringLength(200)]
        public string CustNameAliasEn { get; set; }
        [StringLength(200)]
        public string CustNameAliasAr { get; set; }

        public int NumberOfSites { get; set; }
    }


















}
