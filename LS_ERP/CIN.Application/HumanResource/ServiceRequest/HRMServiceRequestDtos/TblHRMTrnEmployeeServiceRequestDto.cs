using AutoMapper;
using CIN.Application;
using CIN.Domain.HumanResource.EmployeeMgt;
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
    [AutoMap(typeof(TblHRMTrnEmployeeServiceRequest))]
    public class TblHRMTrnEmployeeServiceRequestDto : AuditableEntityDto<int>
    {
        //ServiceRequestRefNo
        [Required]
        [StringLength(20)]
        public string ServiceRequestRefNo { get; set; }

        //ServiceRequestTypeCode
       
        [Required]
        [StringLength(20)]
        public string ServiceRequestTypeCode { get; set; }

        //EmployeeID
      
        [Required]
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public string LastProcessedUser { get; set; }

        //ActionID(An enum with Cancel = 0, Save = 1,Submit = 2, Reject = 3, Approved = 4, Release = 5, Reporting = 6, Settlement=7, Relieve=8)
        [Required]
        public int ActionID { get; set; }

        //ServiceRequestProcessStageID(Increment by 1 at each step.)
        [Required]
        public int ServiceRequestProcessStageID { get; set; }

        //IsApproved
        public bool IsApproved { get; set; }
    }
}
