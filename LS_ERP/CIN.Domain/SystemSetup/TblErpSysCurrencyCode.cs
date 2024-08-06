using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysCurrencyCode")]
    public class TblErpSysCurrencyCode : PrimaryKey<int>
    {
        [StringLength(20)]
        [Required]
        public string CurrencyName { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountryCode { get; set; }
        [StringLength(50)]
        [Required]
        public string CountryCode { get; set; }
        //[Column(TypeName = "decimal(10, 5)")]
        [Required]
        public float BuyingRate { get; set; }
        //[Column(TypeName = "decimal(10, 5)")]
        [Required]
        public float SellingRate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Lastupdated { get; set; }
    }
}
