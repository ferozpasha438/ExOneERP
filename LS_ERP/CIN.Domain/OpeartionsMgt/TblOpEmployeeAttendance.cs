using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpEmployeeAttendance")]
    public class TblOpEmployeeAttendance : AuditableEntity<long>
    {
        [Column(TypeName = "date")]
        public DateTime? AttnDate { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        public string ShiftCode { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public short? ShiftNumber { get; set; }
       
        public bool isDefaultEmployee { get; set; }
        public bool isPrimarySite { get; set; }
        public bool isDefShiftOff { get; set; }
        public bool isPosted { get; set; }
        [StringLength(5)]
        public string Attendance { get; set; }
        [StringLength(20)]
        public string AltEmployeeNumber { get; set; }
        [StringLength(20)]
        public string AltShiftCode { get; set; }
        public long? RefIdForAlt { get; set; }


        //for new Dashboard
        public bool? IsLate { get; set; } = false;
        public bool? IsLoginToShift { get; set; }
        public bool? IsLogoutFromShift { get; set; } = false;
        public bool? IsOnBreak { get; set; } = false;
        public bool? IsGeofenseOut { get; set; } = false;
        public short? GeofenseOutCount { get; set; } = 0;
        public string SkillsetCode { get; set; } 
    }
}
