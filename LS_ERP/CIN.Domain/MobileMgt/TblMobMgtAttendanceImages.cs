using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.MobileMgt
{
     [Table("tblMobMgtAttendanceImages")]
    public class TblMobMgtAttendanceImages : PrimaryKey<long>
    {
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [ForeignKey(nameof(AttendanceId))]
        public TblOpEmployeeAttendance SysEmployeeAttendance { get; set; }
        public long AttendanceId { get; set; }
        public string LoginImagePath { get; set; }
        public string LogoutImagePath { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }


    }
}
