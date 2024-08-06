using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.ServiceRequest;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CIN.Domain;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMDefServiceRequestApprovalAuthorityMatrix))]
    public class TblHRMDefServiceRequestApprovalAuthorityMatrixDto : AuditableEntityDto<int>
    {
        //ApprovalAuthorityCode

        [Required]
        [StringLength(20)]
        public string ApprovalAuthorityCode { get; set; }

        //ServiceRequestTypeCode

        [Required]
        [StringLength(20)]
        public string ServiceRequestTypeCode { get; set; }

        //Branch

        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        //ManagerEmployeeID        
        [Required]
        public int ManagerEmployeeID { get; set; }
    }
}
