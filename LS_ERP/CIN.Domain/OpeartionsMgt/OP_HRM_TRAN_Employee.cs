//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("OP_HRM_TRAN_Employee")]
//    public class OP_HRM_TRAN_Employee : AutoActiveGenerateIdAuditableKey<int>
//    {
//        [Key]
//        [Required]
//        public string EmployeeCode { get; set; }
//        [Required]
//        [StringLength(200)]
//        public string EmployeeNameEng { get; set; }
//        [Required]
//        [StringLength(200)]
//        public string EmployeeNameArb { get; set; }
//        public int ModifiedBy { get; set; }
//        public int CreatedBy { get; set; }
//    }
//}
