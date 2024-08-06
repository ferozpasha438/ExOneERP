using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.TimeAndAttendance.Management
{
    [Table("tblTNATrnConsolidatedEmployeeAttendance")]
    public class TblTNATrnConsolidatedEmployeeAttendance : PrimaryKey<int>
    {
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollPeriodCode { get; set; }
        public int TotalDays { get; set; }
        public int TotalPresentDays { get; set; }
        public int TotalOffDays { get; set; }
        public int TotalLeaves { get; set; }
        public int TotalHolidays { get; set; }
        public int TotalAbsents { get; set; }
        public int NetWorkingDays { get; set; }
        public int TotalLateDays { get; set; }
        public long TotalLateHours { get; set; }
        public long NormalOTHours { get; set; }
        public long SpecialOTHours { get; set; }
        [Required]
        public byte ShiftNumber { get; set; }
    }
}
