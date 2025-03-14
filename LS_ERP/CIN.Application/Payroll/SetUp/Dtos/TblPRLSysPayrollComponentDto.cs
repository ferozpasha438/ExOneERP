﻿using AutoMapper;
using CIN.Domain.Payroll.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.SetUp.Dtos
{
    [AutoMap(typeof(TblPRLSysPayrollComponent))]
    public class TblPRLSysPayrollComponentDto : AutoGeneratedIdKeyAuditableEntityDto<int>
    {
        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }
        [Required]
        [StringLength(100)]
        public string PayrollComponentNameEn { get; set; }
        [StringLength(100)]
        public string PayrollComponentNameAr { get; set; }
        [Required]
        public int PayrollComponentType { get; set; }
        /*if PayrollComponent is an earning calculation should done only on Basic else calculation can be done either on Basic/Gross Salary.*/
        [Required]
        public bool IsFormula { get; set; }
        //This flag will be used to indicate if the component can be used as part formulea for PayrollComponent.
        [Required]
        public bool IsUsedForOtherPayrollComponent { get; set; }
        //This flag will be used to indicate if the component needs to be considered while calculating the gross Salary.
        [Required]
        public bool IsApplicableForDeduction { get; set; }
        [StringLength(100)]
        public string FormulaQueryString { get; set; }
        public string PayrollComponentTypeName { get; set; }
    }

}
