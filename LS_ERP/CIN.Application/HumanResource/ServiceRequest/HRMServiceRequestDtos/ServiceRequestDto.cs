using CIN.Domain.HumanResource.ServiceRequest;
using CIN.Domain.HumanResource.ServiceRequest.HRMServiceRequestDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    public class ApprovalListDto
    {
        public int ActionType { get; set; }
        public string Remarks { get; set; }
        public List<int> Ids { get; set; }
    }
    public class ServiceRequestDataDto
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        [StringLength(20)]
        public string ServiceRequestTypeCode { get; set; }

        [StringLength(20)]
        public string ServiceRequestRefNo { get; set; }
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        public short Sequence { get; set; }
    }

    public class VacationServiceRequestDto : ServiceRequestDataDto
    {
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string DocumentType { get; set; }
        public CustomSelectListItem EmployeeInfo { get; set; }
        public List<TblHRMTrnEmployeeVacationServiceRequestLeaveDetailsDto> List { get; set; }
        public List<TblHRMTrnEmployeeServiceRequestAuditDto> Audits { get; set; }
    }

    public class CreateVacationReleaseExitDto
    {

    }
    public class CreateVacationReportEntryDto
    {

    }
}
