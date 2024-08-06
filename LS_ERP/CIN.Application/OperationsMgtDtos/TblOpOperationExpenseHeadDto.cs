using AutoMapper;
using CIN.Application;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{

    [AutoMap(typeof(TblOpOperationExpenseHead))]
    public class TblOpOperationExpenseHeadDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        public string CostHead { get; set; }
        [StringLength(50)]
        public string CostType { get; set; }
        [StringLength(200)]
        public string CostNameInEnglish { get; set; }
        [StringLength(200)]
        public string CostNameInArabic { get; set; }
        public decimal MinServiceCosttoCompany { get; set; }
        public decimal MinServicePrice { get; set; }
        public decimal GrossMargin { get; set; }
        public bool isApplicableForVehicle { get; set; }
        public bool isApplicableForSkillset { get; set; }
        public bool isApplicableForMaterial { get; set; }
        public bool isApplicableForFinancialExpense { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
