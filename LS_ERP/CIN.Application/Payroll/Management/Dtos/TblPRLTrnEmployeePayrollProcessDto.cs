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
    [AutoMap(typeof(TblPRLTrnEmployeePayrollProcess))]
    public class TblPRLTrnEmployeePayrollProcessDto : AuditableEntityDto<int>
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollMonth { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }

        [StringLength(100)]
        public string PayrollComponentName { get; set; }
        
        public bool IsFormula { get; set; }

        //Example: DA = (Basic*(10/100) + HRA*(5/100)) where HRA = ((10/100)*Basic)
        [StringLength(100)]
        public string FormulaQueryString { get; set; }

        public int PayrollComponentType { get; set; }

        [Required]
        public decimal PayValue { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReleased { get; set; }
    }

    public class EmployeePaySlipHeader
    {
        public int EmployeeID { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [StringLength(5)]
        public string EmployeeNumber { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }
        [StringLength(100)]
        public string GradeName { get; set; }
        [StringLength(100)]
        public string PositionName { get; set; }
        public string PayrollMonth { get; set; }
        public int CalandarDays { get; set; }
        public int TotalOffDays { get; set; }
        public int TotalHolidays { get; set; }
        public int TotalLeaves { get; set; }
        public int TotalAbsents { get; set; }
        public int NetWorkingDays { get; set; }
        public decimal Earnings { get; set; }
        public decimal Deductions { get; set; }
        public decimal Netpay { get; set; }
    }

    public class BaseEmployeePaySlipDto
    {
        public BaseEmployeePaySlipDto()
        {
            PaySlipHeader = new EmployeePaySlipHeader();
            PaySlipDetails = new List<TblPRLTrnEmployeePayrollProcessDto>();
        }
        public EmployeePaySlipHeader PaySlipHeader { get; set; }
        public List<TblPRLTrnEmployeePayrollProcessDto> PaySlipDetails { get; set; }
        public string StatusMessage { get; set; }
    }
}
