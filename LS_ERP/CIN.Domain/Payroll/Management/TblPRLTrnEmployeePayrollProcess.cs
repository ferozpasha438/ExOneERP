using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.Payroll.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.Payroll.Management
{
    [Table("tblPRLTrnEmployeePayrollProcess")]
    public class TblPRLTrnEmployeePayrollProcess : AuditableEntity<int>
    {
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollMonth { get; set; }

        [ForeignKey(nameof(PayrollComponentCode))]
        public TblPRLSysPayrollComponent SysPayrollComponent { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }

        //Example: DA = (Basic*(10/100) + HRA*(5/100)) where HRA = ((10/100)*Basic)
        [StringLength(100)]
        public string FormulaQueryString { get; set; }

        [Required]
        public decimal PayValue { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReleased { get; set; }
    }
}
