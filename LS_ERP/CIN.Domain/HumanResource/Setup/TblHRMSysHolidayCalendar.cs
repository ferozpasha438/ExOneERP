﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysHolidayCalendar")]
    public class TblHRMSysHolidayCalendar : AutoGeneratedIdKeyAuditableEntity<int>
    {
        [Key]
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [StringLength(100)]
        public string HolidayCalendarNameEn { get; set; }
        [StringLength(100)]
        public string HolidayCalendarNameAr { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
