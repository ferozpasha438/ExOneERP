using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("HRM_DEF_HolidayMaster")]
    public class HRM_DEF_HolidayMaster
    {
        [Key]
        public int HolidayCalanderID { get; set; }
        [StringLength(100)]
        public string CalanderName_EN { get; set; }
        [StringLength(100)]
        public string CalanderName_AR { get; set; }
        [StringLength(100)]
        public string HolidayReason { get; set; }
        public bool? IsHoliday { get; set; }
        public DateTime? HolidayDate { get; set; }
    }
    
}
