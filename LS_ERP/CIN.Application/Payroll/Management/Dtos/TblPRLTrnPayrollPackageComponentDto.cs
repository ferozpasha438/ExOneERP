using AutoMapper;
using CIN.Domain.Payroll.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.Management.Dtos
{
    [AutoMap(typeof(TblPRLTrnPayrollPackageComponent))]
    public class TblPRLTrnPayrollPackageComponentDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(20)]
        public string PackageCode { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }
        [StringLength(100)]
        public string PayrollComponentName { get; set; }
        public bool IsFormula { get; set; }
        public decimal PayValue { get; set; }
        public int PayrollComponentType { get; set; }

        [StringLength(100)]
        public string FormulaQueryString { get; set; }
    }
}
