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
    [AutoMap(typeof(TblPRLTrnEmployeePayrollStructure))]
    public class TblPRLTrnEmployeePayrollStructureDto : AuditableEntityDto<int>
    {
        [Required]
        public int EmployeeID { get; set; }

        [StringLength(20)]
        public string PackageCode { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }

        //Example: DA = (Basic*(10/100) + HRA*(5/100)) where HRA = ((10/100)*Basic)
        [StringLength(100)]
        public string FormulaQueryString { get; set; }

        public decimal PayValue { get; set; }

        public bool IsUsedInPayroll { get; set; }

        [StringLength(100)]
        public string PayrollComponentName { get; set; }

        public bool IsFormula { get; set; }

        public int PayrollComponentType { get; set; }
    }

    public class PayrollPackageFilterDto
    {
        public string GradeCode { get; set; }
        public string PositionCode { get; set; }
    }

    public class BaseEmployeePayrollStructureDto
    {
        [Required]
        public int Id { get; set; }
        public List<TblPRLTrnEmployeePayrollStructureDto> PackageComponents { get; set; }
    }
}
