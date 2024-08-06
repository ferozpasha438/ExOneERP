using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest
{
    [Table("tblHRMDefServiceRequestApprovalAuthorityMatrix")]
    public class TblHRMDefServiceRequestApprovalAuthorityMatrix : AuditableEntity<int>
    {
        //ApprovalAuthorityCode
        [ForeignKey(nameof(ApprovalAuthorityCode))]
        public TblHRMSysServiceRequestApprovalAuthority SysApprovalAuthority { get; set; }
        [Required]
        [StringLength(20)]
        public string ApprovalAuthorityCode { get; set; }

        //ServiceRequestTypeCode
        [ForeignKey(nameof(ServiceRequestTypeCode))]
        public TblHRMSysServiceRequestType SysServiceRequestType { get; set; }
        [Required]
        [StringLength(20)]
        public string ServiceRequestTypeCode { get; set; }

        //Branch
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        //ManagerEmployeeID
        [ForeignKey(nameof(ManagerEmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int ManagerEmployeeID { get; set; }
    }
}
