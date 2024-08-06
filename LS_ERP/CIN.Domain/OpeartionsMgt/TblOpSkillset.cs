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
    [Table("tblOpSkillset")]
    public class TblOpSkillset : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string SkillSetCode { get; set; }
        [StringLength(200)]
        public string SkillType { get; set; }
        [StringLength(200)]
        public string NameInEnglish { get; set; }
        [StringLength(200)]
        public string NameInArabic { get; set; }
        [StringLength(500)]
        public string DetailsOfSkillSet { get; set; }
        [StringLength(200)]
        public string SkillSetType { get; set; }
        public short PrioryImportanceScale { get; set; }
        public short MinBufferResource { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MonthlyCtc { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal CostToCompanyDay { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MonthlyOverheadCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MonthlyOtherOverHeads { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal TotalSkillsetCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal TotalSkillsetCostDay { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal ServicePriceToCompany { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MinMarginRequired { get; set; }
    
        public bool OverrideMarginIfRequired { get; set; }
        [StringLength(1000)]
        public string ResponsibilitiesRoles { get; set; }
    }
}
