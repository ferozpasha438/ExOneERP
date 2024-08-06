//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("OP_HRM_DEF_EmployeeShift")]
//    public class OP_HRM_DEF_EmployeeShift
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//        public string EmployeeCode { get; set; }
//        [StringLength(2)]
//        public string MondayShiftCode { get; set; }
//        [StringLength(2)]
//        public string TuesdayShiftCode { get; set; }
//        [StringLength(2)]
//        public string WednesdayShiftCode { get; set; }
//        [StringLength(2)]
//        public string ThursdayShiftCode { get; set; }
//        [StringLength(2)]
//        public string FridayShiftCode { get; set; }
//        [StringLength(2)]
//        public string SaturdayShiftCode { get; set; }
//        [StringLength(2)]
//        public string SundayShiftCode { get; set; }
//    }
//}
