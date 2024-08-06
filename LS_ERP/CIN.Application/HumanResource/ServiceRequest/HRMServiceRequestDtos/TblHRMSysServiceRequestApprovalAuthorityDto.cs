using AutoMapper;
using CIN.Application;
using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMSysServiceRequestApprovalAuthority))]
    public class TblHRMSysServiceRequestApprovalAuthorityDto : AutoGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        public string ApprovalAuthorityCode { get; set; }
        [Required]
        [StringLength(100)]
        public string ApprovalAuthorityNameEn { get; set; }
        [StringLength(100)]
        public string ApprovalAuthorityNameAr { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
    }
}
