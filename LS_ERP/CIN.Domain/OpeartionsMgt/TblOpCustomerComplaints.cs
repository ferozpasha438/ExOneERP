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
    [Table("tblOpCustomerComplaints")]
    public class TblOpCustomerComplaint
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

        public string ComplaintBy { get; set; }
        [StringLength(500)]
        public string ComplaintDescription { get; set; }
        public string ProofForComplaint { get; set; }
        public bool IsActionRequired { get; set; }
        [Column(TypeName = "date")]
        public DateTime ComplaintDate { get; set; }
        public int BookedBy { get; set; }



        [Column(TypeName = "date")]
        public DateTime? ClosingDate { get; set; }
        public string ProofForAction { get; set; }
        [StringLength(500)]
        public string ActionDescription { get; set; }




        public int? CreatedBy { get; set; }
        public int? ClosedBy { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        public bool IsOpen { get; set; } =true;
        public bool IsClosed { get; set; } = false;
        public bool IsInprogress { get; set; } = false;

    }
}
