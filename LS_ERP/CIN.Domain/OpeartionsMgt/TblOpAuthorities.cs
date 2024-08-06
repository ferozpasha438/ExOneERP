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
    [Table("tblOpAuthorities")]
    public class TblOpAuthorities : PrimaryKey<int>
    {
       
       
        public int AppAuth { get; set; }  //reference from tblErpSysLogin loginId
        [StringLength(20)]
        public string BranchCode { get; set; }
        public short AppLevel { get; set; }
        public bool CanApproveEnquiry { get; set; }
        public bool CanAddSurveyorToEnquiry { get; set; }
        //public bool CanEditSurveyForm { get; set; }
        public bool CanApproveSurvey { get; set; }
        public bool CanApproveEstimation { get; set; }
        public bool CanApproveProposal { get; set; }
        public bool CanApproveContract { get; set; }
        public bool CanModifyEstimation { get; set; }
        public bool CanConvertEnqToProject { get; set; }
        public bool CanConvertEstimationToProposal { get; set; }
        public bool CanConvertProposalToContract { get; set; }
        public bool CanCreateRoaster { get; set; }
        public bool CanEditRoaster { get; set; }
        public bool IsFinalAuthority { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }

        public bool CanEditEnquiry { get; set; }
        public bool CanEditPvReq { get; set; }
        public bool CanApprovePvReq { get; set; }
    }
}
