//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("OP_HRM_DEF_PayrollGroup")]
//    public class OP_HRM_DEF_PayrollGroup : AutoActiveGenerateIdAuditableKey<int>
//    {
//        [Key]
//        [Required]
//        [StringLength(20)]
//        public string PayrollGroupCode { get; set; }
//        [StringLength(200)]
//        public string PayrollGroupName_EN { get; set; }
//        [StringLength(200)]
//        public string PayrollGroupName_AR { get; set; }
//        [StringLength(500)]
//        public string Remarks { get; set; }
      
//        public int CreatedBy { get; set; }
       
        
//        public int ModifiedBy { get; set; }
       
//        [StringLength(20)]
//        public string CountryCode { get; set; }
//        [StringLength(20)]
//        public string CompanyCode { get; set; }
//        [StringLength(20)]
//        public string SiteCode { get; set; }
//        [StringLength(20)]
//        public string ProjectCode { get; set; }
//        public string BusinessUnitID { get; set; }
//        public string DivisionID { get; set; }
//        [StringLength(20)]
//        public string BranchID { get; set; }
//        public string DepartmentID { get; set; }
//        public bool? IsForAllEmployee { get; set; }
//        public bool? IsForFutureEmployee { get; set; }
//        [Required]
//        [Column(TypeName = "date")]
//        public DateTime StartPayRollDate { get; set; }
//        [Required]
//        public short CurrentPayRollMonth { get; set; }
//        [Required]
//        [Column(TypeName = "date")]
//        public DateTime EndPayRollDate { get; set; }
//        [Required]
//        public short CurrentPayRollYear { get; set; }

//    }
//}
