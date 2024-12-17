using AutoMapper;
using CIN.Domain.TimeAndAttendance.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos
{
    [AutoMap(typeof(TblTNATrnConsolidatedEmployeeAttendance))]
    public class TblTNATrnConsolidatedEmployeeAttendanceDto : PrimaryKeyDto<int>
    {
        [Required]
        public int EmployeeID { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }
        [Required]
        [StringLength(50)]
        public string PayrollPeriodCode { get; set; }
        public int? TotalDays { get; set; }
        public int? TotalPresentDays { get; set; }
        public int? TotalOffDays { get; set; }
        public int? TotalLeaves { get; set; }
        public int? TotalVacations { get; set; }
        public int? TotalHolidays { get; set; }
        public int? TotalAbsents { get; set; }
        public int? NetWorkingDays { get; set; }
        public int? TotalLateDays { get; set; }
        public long? TotalLateHours { get; set; }
        public long? NormalOTHours { get; set; }
        public long? SpecialOTHours { get; set; }
        [Required]
        public byte ShiftNumber { get; set; }
    }
}
