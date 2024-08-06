//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.OpeartionsMgt
//{
//    [Table("tblOpPaymentTermsToProject")]
//    public class TblOpPaymentTermsMapToProject
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public long Id { get; set; }
//        [StringLength(20)]
//        public string CustomerCode { get; set; }
//        [StringLength(20)]
//        public string ProjectCode { get; set; }
//        [StringLength(20)]
//        public string BranchCode { get; set; }
//        public DateTime? MonthStartDate { get; set; }
//        [Column(TypeName = "date")]
//        public DateTime? MonthEndDate { get; set; }
//        [Column(TypeName = "decimal(17,3)")]
//        public decimal Amount { get; set; }

//        [Column(TypeName = "date")]
//        public DateTime? Created { get; set; }
//        public int CreatedBy { get; set; }
//    }

   
//}
