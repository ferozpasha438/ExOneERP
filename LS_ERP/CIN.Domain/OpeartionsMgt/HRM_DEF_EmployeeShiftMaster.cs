//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("HRM_DEF_EmployeeShiftMaster")]
//    public class HRM_DEF_EmployeeShiftMaster 
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public long ShiftId { get; set; }
//        [StringLength(100)]
//        public string ShiftCode { get; set; }
//        [StringLength(100)]
//        public string ShiftName_EN { get; set; }
//        [StringLength(100)]
//        public string ShiftName_AR { get; set; }
//        public TimeSpan? InTime { get; set; }
//        public TimeSpan? OutTime { get; set; }
//        public TimeSpan? BreakTime { get; set; }
//        public TimeSpan? InGrace { get; set; }
//        public TimeSpan? OutGrace { get; set; }
//        public TimeSpan? WorkingTime { get; set; }
//        public TimeSpan? NetWorkingTime { get; set; }
//        public bool IsOff { get; set; }
//    }
//}
