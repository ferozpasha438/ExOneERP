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
        public bool? IsActive { get; set; }
    }
}
