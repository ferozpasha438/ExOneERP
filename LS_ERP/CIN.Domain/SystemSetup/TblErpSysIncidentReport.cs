using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysIncidentReport")]
    public class TblErpSysIncidentReport : PrimaryKey<int>
    {
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(128)]
        public string ImagePath { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SiteGeoLatitude { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SiteGeoLongitude { get; set; } = 0;
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }        
        public int? CreatedBy { get; set; }
    }
}
