using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysHolidayCalendarMapping")]
    public class TblHRMSysHolidayCalendarMapping : PrimaryKey<int>
    {
        [ForeignKey(nameof(HolidayCalendarCode))]
        public TblHRMSysHolidayCalendar SysHolidayCalendar { get; set; }
        [Required]
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }

        [ForeignKey(nameof(HolidayCode))]
        public TblHRMSysHoliday SysHoliday { get; set; }
        [Required]
        [StringLength(20)]
        public string HolidayCode { get; set; }
    }
}
