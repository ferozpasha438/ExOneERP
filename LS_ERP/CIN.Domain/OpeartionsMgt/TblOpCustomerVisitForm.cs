using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpCustomerVisitForm")]
    public class TblOpCustomerVisitForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
         [StringLength(20)]
        public string ProjectCode { get; set; }

        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }

        [ForeignKey(nameof(ReasonCode))]
        public TblOpReasonCode SysReasonCode { get; set; }
        [StringLength(20)]
        public string ReasonCode { get; set; }
        [StringLength(500)]
        public string SupervisorRemarks { get; set; }
        [StringLength(500)]
        public string CustomerRemarks { get; set; }
        [StringLength(500)]
        public string ActionTerms { get; set; }
        [StringLength(500)]
        public string CustomerNotes { get; set; }
        [StringLength(10)]
        public string ContactNumber { get; set; }
        [ForeignKey(nameof(SupervisorId))]
        public TblErpSysLogin SysSuperviserId { get; set; }
        public int SupervisorId { get; set; }
        public int? VisitedBy { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        
        public DateTime ScheduleDateTime { get; set; }
        public DateTime? VisitedDateTime { get; set; }

       
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }
        public bool IsOpen { get; set; } =true;
        public bool IsClosed { get; set; } = false;
        public bool IsInprogress { get; set; } = false;

    }
}
