using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.MobileMgt
{
     [Table("tblMobMgtGeoLocationLogs")]
    public class TblMobMgtGeoLocationLogs : PrimaryKey<long>
    {
        public long AttnId { get; set; }
        [ForeignKey(nameof(AttnId))]
        public TblOpEmployeeAttendance SysAttendance { get; set; }
        public bool IsGeofenceOut { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Remarks { get; set; }


    }
}
