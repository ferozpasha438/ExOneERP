using AutoMapper;
using CIN.Domain;
using CIN.Domain.PurchaseSetup;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.PurchaseSetupDtos
{
    [AutoMap(typeof(TblPopDefVendorCategory))]
    public class TblPopDefVendorCategoryDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string VenCatCode { get; set; }
        [StringLength(50)]
        [Required]
        public string VenCatName { get; set; }
        [StringLength(50)]
        public string VenCatDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    [AutoMap(typeof(TblPopDefVendorPOTermsCode))]
    public class TblPopDefVendorPOTermsCodeDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string POTermsCode { get; set; }
        [StringLength(50)]
        [Required]
        public string POTermsName { get; set; }
        [StringLength(50)]
        public string POTermsDesc { get; set; }
        public sbyte POTermsDueDays { get; set; }
        public sbyte POTermDiscDays { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    [AutoMap(typeof(TblPopDefVendorShipment))]
    public class TblPopDefVendorShipmentDto : AutoActiveGenerateIdKeyDto<int>
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
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
