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
    [AutoMap(typeof(TblHRMTrnEmployeeVacationServiceRequestLeaveDetails))]
    public class TblHRMTrnEmployeeVacationServiceRequestLeaveDetailsDto : PrimaryKeyDto<int>
    {
        //EmployeeServiceRequestID

       // [Required]
        public int EmployeeServiceRequestID { get; set; }

        //LeaveTypeCode

        [Required]
        [StringLength(20)]
        public string LeaveTypeCode { get; set; }

        //FromDate
        [Required]
        public DateTime FromDate { get; set; }

        //ToDate
        [Required]
        public DateTime ToDate { get; set; }

        //NoOfDays
        public int NoOfDays { get; set; }
    }
}
