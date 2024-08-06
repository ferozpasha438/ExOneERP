using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysSystemTaxes")]
    //[Index(nameof(TaxCode), Name = "IX_tblErpSysSystemTaxes_TaxCode", IsUnique = false)]
    public class TblErpSysSystemTax : AutoActiveGenerateIdKey<int>
    {
        [StringLength(20)]
        [Key]
        public string TaxCode { get; set; }
        [StringLength(100)]
        [Required]
        public string TaxName { get; set; }
        public bool IsInterState { get; set; }

        [StringLength(10)]
        [Required]
        public string TaxComponent01 { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        [Required]
        public decimal Taxper01 { get; set; }
        [StringLength(50)]
        public string InputAcCode01 { get; set; }
        [StringLength(50)]
        public string OutputAcCode01 { get; set; }

        [StringLength(10)]
        public string TaxComponent02 { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal? Taxper02 { get; set; }
        [StringLength(50)]
        public string InputAcCode02 { get; set; }
        [StringLength(50)]
        public string OutputAcCode02 { get; set; }

        [StringLength(10)]
        public string TaxComponent03 { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal? Taxper03 { get; set; }
        [StringLength(50)]
        public string InputAcCode03 { get; set; }
        [StringLength(50)]
        public string OutputAcCode03 { get; set; }

        [StringLength(10)]
        public string TaxComponent04 { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal? Taxper04 { get; set; }
        [StringLength(50)]
        public string InputAcCode04 { get; set; }
        [StringLength(50)]
        public string OutputAcCode04 { get; set; }

        [StringLength(10)]
        public string TaxComponent05 { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal? Taxper05 { get; set; }
        [StringLength(50)]
        public string InputAcCode05 { get; set; }
        [StringLength(50)]
        public string OutputAcCode05 { get; set; }

    }
}
