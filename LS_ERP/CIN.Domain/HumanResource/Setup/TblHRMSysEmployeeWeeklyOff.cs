using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysEmployeeWeeklyOff")]
    public class TblHRMSysEmployeeWeeklyOff : AuditableEntity<int>
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

        //Day of the Week
        [StringLength(15)]
        public string DayOfWeek { get; set; }
    }
}
