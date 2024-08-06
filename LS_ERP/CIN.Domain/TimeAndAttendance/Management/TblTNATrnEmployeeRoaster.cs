using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.Setup;
using CIN.Domain.SystemSetup;
using CIN.Domain.TimeAndAttendance.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.TimeAndAttendance.Management
{
    [Table("tblTNATrnEmployeeRoaster")]
    public class TblTNATrnEmployeeRoaster : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Branch
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        //Payroll Group
        [ForeignKey(nameof(PayrollGroupCode))]
        public TblTNASysPayrollGroup SysPayrollGroup { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //Date
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        //Shift
        [ForeignKey(nameof(ShiftCode))]
        public TblHRMSysShift SysShift { get; set; }
        [Required]
        [StringLength(20)]
        public string ShiftCode { get; set; }
    }
}
