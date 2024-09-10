using AutoMapper;
using CIN.Domain.HumanResource.ServiceRequest;
using System;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeExitReEntryInfo))]
    public class TblHRMTrnEmployeeExitReEntryInfoDto : AuditableEntityDto<int>
    {

        [Required]
        public int EmployeeServiceRequestID { get; set; }

        [Required]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(30)]
        public string ExitReEntryNumber { get; set; }
        [Required]
        public DateTime ExitEffectiveFromDate { get; set; }
        [Required]
        public DateTime ExitEffectiveToDate { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        [Required]
        public DateTime ExpectedDateOfReporting { get; set; }
        public bool IsVacationExtensionAllowed { get; set; }
        [StringLength(15)]
        public string TicketNumber { get; set; }

        public string FlightClassCode { get; set; }
        [StringLength(200)]
        public string Airlines { get; set; }

        [StringLength(20)]
        public string BoardingCityCode { get; set; }

        [StringLength(20)]
        public string DestinationCityCode { get; set; }
        public bool IsReplacementRequired { get; set; }

        public int ReplacementEmployeeID { get; set; }
        [StringLength(500)]
        public string ReplacementRemarks { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Replacementemployee { get; set; }
        public string NameOfTheReplacementEmployee { get; set; }
    }
}
