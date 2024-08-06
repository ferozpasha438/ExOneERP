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
    [Table("tblSndDefSiteMaster")]
    public class TblSndDefSiteMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(50)]
        public string SiteCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SiteName { get; set; }
        [StringLength(200)]
        public string SiteArbName { get; set; }
        [ForeignKey(nameof(CustomerCode))]
        public TblSndDefCustomerMaster SysCustomerCode { get; set; }
        [Required]
        public string CustomerCode { get; set; }
        [StringLength(500)]
        [Required]
        public string SiteAddress { get; set; }
        [StringLength(20)]
        [Required]
        [ForeignKey(nameof(SiteCityCode))]
        public TblErpSysCityCode SysSiteCityCode { get; set; }
        public string SiteCityCode{ get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal SiteGeoLatitude { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal SiteGeoLongitude { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal SiteGeoGain { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal SiteGeoLatitudeMeter { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal SiteGeoLongitudeMeter { get; set; }

        public bool IsChildCustomer { get; set; }
        [StringLength(50)]
        public string VATNumber { get; set; }
        public int StandardDeviation { get; set; } = 0;  //in meters

    }
}
