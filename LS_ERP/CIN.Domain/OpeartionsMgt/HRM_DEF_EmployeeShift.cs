using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("HRM_DEF_EmployeeShift")]
    public class HRM_DEF_EmployeeShift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public long EmployeeID { get; set; }
        public short? MondayShiftId { get; set; }
        public short? TuesdayShiftId { get; set; }
        public short? WednesdayShiftId { get; set; }
        public short? ThursdayShiftId { get; set; }
        public short? FridayShiftId { get; set; }
        public short? SaturdayShiftId { get; set; }
        public short? SundayShiftId { get; set; }
    }
}
