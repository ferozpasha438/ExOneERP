using AutoMapper;
using CIN.Domain.HumanResource.ServiceRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeVacationDateLog))]
    public class TblHRMTrnEmployeeVacationDateLogDto : AuditableEntityDto<int>
    {
        [Required]
        public int EmployeeServiceRequestID { get; set; }
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [StringLength(20)]
        public string ServiceRequestTypeCode { get; set; }
    }
}
