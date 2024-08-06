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
    [Table("tblOpMaterialEquipment")]
    public class TblOpMaterialEquipment : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        [StringLength(200)]
        public string NameInEnglish { get; set; }
        [StringLength(200)]
        public string NameInArabic { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal EstimatedCostValue { get; set; }
        public bool IsDepreciationApplicable { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MinimumCostPerUsage { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal? DepreciationPerYear { get; set; }
        public short UsageLifetermsYear { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }

    }
}
