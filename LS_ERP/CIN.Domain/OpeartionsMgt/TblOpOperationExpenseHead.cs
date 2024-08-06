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
    [Table("tblOpOperationExpenseHead")]
    public class TblOpOperationExpenseHead : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string CostHead { get; set; }
        [StringLength(50)]
        public string CostType { get; set; }
        [StringLength(200)]
        public string CostNameInEnglish { get; set; }
        [StringLength(200)]
        public string CostNameInArabic { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MinServiceCosttoCompany { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MinServicePrice { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal GrossMargin { get; set; }
        public bool isApplicableForVehicle { get; set; }
        public bool isApplicableForSkillset { get; set; }
        public bool isApplicableForMaterial { get; set; }
        public bool isApplicableForFinancialExpense { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

    }
}
