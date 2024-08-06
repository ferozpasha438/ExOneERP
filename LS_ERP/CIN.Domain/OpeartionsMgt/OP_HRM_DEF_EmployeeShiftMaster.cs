//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("OP_HRM_DEF_EmployeeShiftMaster")]
//    public class OP_HRM_DEF_EmployeeShiftMaster : AutoActiveGenerateIdAuditableKey<int>
//    {
//        [Key]
//        [StringLength(20)]
//        [Required]
//        public string ShiftCode { get; set; }
//        [Required]
//        [StringLength(200)]
//        public string ShiftName_EN { get; set; }
//        [Required]
//        [StringLength(200)]
//        public string ShiftName_AR { get; set; }
//        [Required]
//        [StringLength(5)]
//        public string InTime { get; set; }
//        [Required]
//        [StringLength(5)]
//        public string OutTime { get; set; }
//        [Required]
        
//        public short BreakTime { get; set; }
//        [Required]
       
//        public short InGrace { get; set; }
//        [Required]
       
//        public short OutGrace { get; set; }
//        [Required]
//        [StringLength(5)]
//        public string WorkingTime { get; set; }
//        [Required]
//        [StringLength(5)]
//        public string NetWorkingTime { get; set; }
//        public bool IsOff { get; set; }
//    }
//}
