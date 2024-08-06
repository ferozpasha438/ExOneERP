using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysCityCode")]
    //[Index(nameof(CountryCode), Name = "IX_TblErpSysCityCode_CountryCode", IsUnique = false)]
    // [Index(nameof(CityCode), Name = "IX_TblErpSysCityCode_CityCode", IsUnique = true)]
    public class TblErpSysCityCode : AutoActiveGenerateIdKey<int>
    {

        [ForeignKey(nameof(StateCode))]
        public TblErpSysStateCode SysStateCode { get; set; }
        [Required]
        [StringLength(20)]
        public string StateCode { get; set; }

        [StringLength(20)]

        [Key]
        public string CityCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CityName { get; set; }
        [StringLength(100)]
        public string CityNameAr { get; set; }
    }
}
