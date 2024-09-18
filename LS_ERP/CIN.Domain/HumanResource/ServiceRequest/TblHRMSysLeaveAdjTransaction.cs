//using CIN.Domain.HumanResource.EmployeeMgt;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.HumanResource.ServiceRequest
//{
//    [Table("tblHRMSysLeaveAdjTransaction")]
//    public class TblHRMSysLeaveAdjTransaction : AuditableEntity<int>
//    {
//        //EmployeeID
//        [ForeignKey(nameof(EmployeeID))]
//        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
//        [Required]
//        public int EmployeeID { get; set; }

//        [Required]
//        public int NumberOfDays { get; set; }
//        [Required]
//        public DateTime LeaveDate { get; set; }
//        [StringLength(120)]
//        public string ApprovalAuthority { get; set; }
//        [StringLength(500)]
//        public string ReplacementRemarks { get; set; }
//    }
//}

