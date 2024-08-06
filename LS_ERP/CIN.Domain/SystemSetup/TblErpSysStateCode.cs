using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysStateCode")]
    //[Index(nameof(StateCode), Name = "IX_tblErpSysStateCode_StateCode", IsUnique = true)]
    public class TblErpSysStateCode : AutoActiveGenerateIdKey<int>
    {
        [StringLength(20)]
        [Key]
        public string StateCode { get; set; }
        [Required]
        [StringLength(100)]
        public string StateName { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountryCode { get; set; }
        [Required]
        public string CountryCode { get; set; }

    }
}
