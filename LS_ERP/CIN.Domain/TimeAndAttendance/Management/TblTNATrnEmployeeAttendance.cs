using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.TimeAndAttendance.Management
{
    [Table("tblTNATrnEmployeeAttendance")]
    public class TblTNATrnEmployeeAttendance : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
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
        public char AttnFlag { get; set; }

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
        [ForeignKey(nameof(ShiftCode))]
        public TblHRMSysShift SysShift { get; set; }
        [Required]
        [StringLength(20)]
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
}
