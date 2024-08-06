using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefSiteMaster))]
    public class TblSndDefSiteMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
       
        [StringLength(50)]
        public string SiteCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SiteName { get; set; }
      

        [StringLength(200)]
        public string SiteArbName { get; set; }
       
        public string CustomerCode { get; set; }
      

        [StringLength(500)]
        public string SiteAddress { get; set; }
        [StringLength(20)]
        
        public string SiteCityCode { get; set; }
        
        [Column(TypeName = "decimal(7,2)")]
        public decimal SiteGeoLatitude { get; set; }
       
        [Column(TypeName = "decimal(7,2)")]
        public decimal SiteGeoLongitude { get; set; }
        
        [Column(TypeName = "decimal(7,2)")]
        public decimal SiteGeoGain { get; set; }
        public decimal SiteGeoLatitudeMeter { get; set; }
        public decimal SiteGeoLongitudeMeter { get; set; }
        public bool IsChildCustomer { get; set; }
        [StringLength(50)]
        public string VATNumber { get; set; }
        public int StandardDeviation { get; set; } = 0;  //in meters

    }
}