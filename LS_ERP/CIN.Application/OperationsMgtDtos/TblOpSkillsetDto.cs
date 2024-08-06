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

    [AutoMap(typeof(TblOpSkillset))]
    public class TblOpSkillsetDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
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
        public decimal MonthlyCtc { get; set; }
        public decimal CostToCompanyDay { get; set; }
        public decimal MonthlyOverheadCost { get; set; }
        public decimal MonthlyOtherOverHeads { get; set; }
        public decimal TotalSkillsetCost { get; set; }
        public decimal TotalSkillsetCostDay { get; set; }
        public decimal ServicePriceToCompany { get; set; }
        public decimal MinMarginRequired { get; set; }
        public bool OverrideMarginIfRequired { get; set; }
        [StringLength(1000)]
        public string ResponsibilitiesRoles { get; set; }
    }


    public class OpSkillsetDto : TblOpSkillsetDto
    {
        public int Quantity { get; set; }
    }
}
