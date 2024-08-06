using AutoMapper;
using CIN.Domain.PurchaseSetup;
using CIN.Domain.SalesSetup;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.SalesSetupDtos
{
    [AutoMap(typeof(TblSndDefCustomerCategory))]
    public class TblSndDefCustomerCategoryDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [Required]
        [StringLength(20)]
        public string CustCatCode { get; set; }
        [StringLength(50)]
        [Required]
        public string CustCatName { get; set; }
        [StringLength(50)]
        [Required]
        public string CustCatDesc { get; set; }
        [StringLength(3)]
        public string CatPrefix { get; set; }
        public int LastSeq { get; set; }

    }

    public class TblInvDefSalesPurchaseConfigDto : PrimaryKeyDto<int>
    {
        public bool AutoGenCustCode { get; set; }
        public bool PrefixCatCode { get; set; }
        [StringLength(10)]
        [Required]
        public string NewCustIndicator { get; set; }
        [Required]
        public short CategoryLength { get; set; } //3
    }

    [AutoMap(typeof(TblInvDefSalesConfig))]
    public class TblInvDefSalesConfigDto : TblInvDefSalesPurchaseConfigDto
    {
        [Required]
        public short CustLength { get; set; } //4
        public bool CanEdit { get; set; }
    }

    [AutoMap(typeof(TblInvDefPurchaseConfig))]
    public class TblInvDefPurchaseConfigDto : TblInvDefSalesPurchaseConfigDto
    {
        [Required]
        public short VendLength { get; set; } //4
    }


    [AutoMap(typeof(TblSndDefSalesShipment))]
    public class TblSndDefSalesShipmentDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string ShipmentCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ShipmentName { get; set; }
        [StringLength(50)]
        public string ShipmentDesc { get; set; }
        [StringLength(10)]
        public string ShipmentType { get; set; }


    }

    [AutoMap(typeof(TblSndDefSalesTermsCode))]
    public class TblSndDefSalesTermsCodeDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string SalesTermsCode { get; set; }
        [StringLength(50)]
        [Required]
        public string SalesTermsName { get; set; }
        [StringLength(50)]
        public string SalesTermsDesc { get; set; }
        public sbyte SalesTermsDueDays { get; set; }
        public sbyte SalesTermDiscDays { get; set; }

    }

}
