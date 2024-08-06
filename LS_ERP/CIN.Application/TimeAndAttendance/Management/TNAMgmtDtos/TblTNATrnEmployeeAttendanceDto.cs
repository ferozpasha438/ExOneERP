using AutoMapper;
using CIN.Domain.TimeAndAttendance.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos
{
    [AutoMap(typeof(TblTNATrnEmployeeAttendance))]
    public class TblTNATrnEmployeeAttendanceDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Date
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        //In Time
        [Required]
        public TimeSpan InTime { get; set; }

        //OutTime
        [Required]
        public TimeSpan OutTime { get; set; }

        //Attendance Flag
        [Required]
        public string AttnFlag { get; set; }

        //Estimated In Time
        public TimeSpan EstimatedInTime { get; set; }

        //Estimated Out Time
        public TimeSpan EstimatedOutTime { get; set; }

        //Estimated NetWorking Time
        public TimeSpan EstimatedNetWorkingTime { get; set; }

        //Late Hours
        public long LateHours { get; set; }

        //OverTime Hours
        public long OverTimeHours { get; set; }

        //Net Working Time
        public long NetWorkingTime { get; set; }

        //Shift
        [Required]
        [StringLength(50)]
        public string ShiftCode { get; set; }

        //Indicates if employee is Late.
        public bool IsLate { get; set; }

        //Indicates if employee has worked on a Holiday or on week Off.
        public bool IsSpecialDay { get; set; }

        //Indicates if employee has punched out the next day.
        public bool IsPunchedOutNextDay { get; set; }

        //Shift Number
        [Required]
        public byte ShiftNumber { get; set; }

        //Indicate if attendance is approved
        public bool IsApproved { get; set; }
    }

    public class Employee
    {
        public Employee()
        {
            AttendanceRows = new List<TblTNATrnEmployeeAttendanceDto>();
        }
        //EmployeeID
        public int EmployeeID { get; set; }

        //Employee Name
        [StringLength(100)]
        public string EmployeeName { get; set; }

        //Branch Code
        [StringLength(20)]
        public string BranchCode { get; set; }

        //Branch Name
        [StringLength(100)]
        public string BranchName { get; set; }

        //Payroll Group
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //PayrollGroup Name
        [StringLength(100)]
        public string PayrollGroupName { get; set; }

        //Date
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        //List Employee Attendance Rows 
        public List<TblTNATrnEmployeeAttendanceDto> AttendanceRows { get; set; }

        //Holiday Calendar
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }
        public bool IsRoasterApplicable { get; set; }
    }

    public class AttendanceColumn
    {
        public string AttendanceDate { get; set; }
        public string AttendanceDay { get; set; }
    }

    public class BaseEmployeeAttendanceDto
    {
        public BaseEmployeeAttendanceDto()
        {
            EmployeeList = new List<Employee>();
            AttendanceColumns = new List<AttendanceColumn>();
        }
        //List of Employees attendance Details
        public List<Employee> EmployeeList { get; set; }

        //List attendace Columns 
        public List<AttendanceColumn> AttendanceColumns { get; set; }
    }

    public class EmployeeAttendanceFilter
    {
        public string PayrollGroupCode { get; set; }
        public string BranchCode { get; set; }
        public string EmployeeName { get; set; }
    }
}
