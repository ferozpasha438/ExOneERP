using CIN.Domain.SystemSetup;
using CIN.Domain.TimeAndAttendance.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.Payroll.Management
{
    [Table("tblPRLTrnPayrollProcessFiltersLog")]
    public class TblPRLTrnPayrollProcessFiltersLog : AuditableEntity<int>
    {
        //Payroll Group
        [ForeignKey(nameof(PayrollGroupCode))]
        public TblTNASysPayrollGroup SysPayrollGroup { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //Branch
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollMonth { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReleased { get; set; }
    }
}
