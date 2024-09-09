using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.Setup;
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
    [Table("tblHRMTrnEmployeeExitReEntryInfo")]
    public class TblHRMTrnEmployeeExitReEntryInfo : AuditableEntity<int>
    {
        [ForeignKey(nameof(EmployeeServiceRequestID))]
        public TblHRMTrnEmployeeServiceRequest TrnEmployeeServiceRequest { get; set; }
        [Required]
        public int EmployeeServiceRequestID { get; set; }
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
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
        [ForeignKey(nameof(FlightClassCode))]
        public TblHRMSysFlightClass SysFlightClass { get; set; }
        public int FlightClassCode { get; set; }
        [StringLength(200)]
        public string Airlines { get; set; }
        [ForeignKey(nameof(BoardingCityCode))]
        public TblErpSysCityCode SysBoardingCityCode { get; set; }
        [StringLength(20)]
        public int BoardingCityCode { get; set; }
        [ForeignKey(nameof(DestinationCityCode))]
        public TblErpSysCityCode SysDestinationCityCodeCityCode { get; set; }
        [StringLength(20)]
        public int DestinationCityCode { get; set; }
        public bool IsReplacementRequired { get; set; }
        [ForeignKey(nameof(ReplacementEmployeeID))]
        public TblHRMTrnPersonalInformation TrnReplacementEmployeeIDPersonalInformation { get; set; }
        public int ReplacementEmployeeID { get; set; }
        [StringLength(500)]
        public string ReplacementRemarks { get; set; }
    }
}
