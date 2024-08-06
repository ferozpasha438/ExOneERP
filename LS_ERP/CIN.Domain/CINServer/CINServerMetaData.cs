using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain
{
    [Table("CINServerMetaData")]
    [Index(propertyNames: nameof(CINNumber), IsUnique = true)]
    public class CINServerMetaData
    {
        [Key]
        public long Id { get; set; }
        [StringLength(20)]
        public string CINNumber { get; set; }
        [StringLength(1000)]
        public string ModueCodes { get; set; }
        public short ConcurrentUsers { get; set; }
        public short ConnectedUsers { get; set; }
        public DateTime ValidDate { get; set; }

        [StringLength(100)]
        public string APIEndpoint { get; set; }
        [StringLength(250)]
        public string DBConnectionString { get; set; }
        public DateTime PaymentDate { get; set; }
        [StringLength(500)]
        public string AdmUrl { get; set; }
        [StringLength(500)]
        public string FinUrl { get; set; }
        [StringLength(500)]
        public string OpmUrl { get; set; }
        [StringLength(500)]
        public string InvUrl { get; set; }
        [StringLength(500)]
        public string SndUrl { get; set; }
        [StringLength(500)]
        public string PopUrl { get; set; }
        [StringLength(500)]
        public string HrmUrl { get; set; }
        [StringLength(500)]
        public string HraUrl { get; set; }
        [StringLength(500)]
        public string HrsUrl { get; set; }
        [StringLength(500)]
        public string FltUrl { get; set; }
        [StringLength(500)]
        public string SchUrl { get; set; }
        [StringLength(500)]
        public string ScpUrl { get; set; }
        [StringLength(500)]
        public string SctUrl { get; set; }
        [StringLength(500)]
        public string PosUrl { get; set; }
        [StringLength(500)]
        public string MfgUrl { get; set; }
        [StringLength(500)]
        public string CrmUrl { get; set; }
        [StringLength(500)]
        public string UtlUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}
