using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysCountryCode")]
   // [Index(nameof(CountryCode), Name = "IX_TblErpSysCountryCode_CountryCode", IsUnique = true)]
    public class TblErpSysCountryCode : AutoActiveGenerateIdKey<int>
    {
        [StringLength(50)]
        [Key]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }
    }
}
