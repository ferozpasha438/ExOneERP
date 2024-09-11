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

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeContractInfo")]
    public class TblHRMTrnEmployeeContractInfo : AuditableEntity<int>
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

        //Grade
        [ForeignKey(nameof(GradeCode))]
        public TblHRMSysGrade SysGrade { get; set; }
        [Required]
        [StringLength(20)]
        public string GradeCode { get; set; }

        //Position
        [ForeignKey(nameof(PositionCode))]
        public TblHRMSysPosition SysPosition { get; set; }
        [Required]
        [StringLength(20)]
        public string PositionCode { get; set; }

        //Payroll Group
        [ForeignKey(nameof(PayrollGroupCode))]
        public TblTNASysPayrollGroup SysPayrollGroup { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //HolidayCalendar
        [ForeignKey(nameof(HolidayCalendarCode))]
        public TblHRMSysHolidayCalendar SysHolidayCalendar { get; set; }
        [Required]
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }

        //VacationPolicy
        [ForeignKey(nameof(VacationPolicyCode))]
        public TblHRMSysVacationPolicy SysVacationPolicy { get; set; }
        [StringLength(20)]
        public string VacationPolicyCode { get; set; }

        //Employee Status
        [ForeignKey(nameof(EmployeeStatusCode))]
        public TblHRMSysEmployeeStatus SysEmployeeStatus { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeStatusCode { get; set; }

        //Stop Payroll
        public bool StopPayroll { get; set; }

        public DateTime? LastWorkDay { get; set; }
    }
}
