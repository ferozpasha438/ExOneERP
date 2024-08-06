using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeDependentInfo))]
    public class TblHRMTrnEmployeeDependentInfoDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Dependent Type
        [Required]
        [StringLength(20)]
        public string DependentTypeCode { get; set; }

        //Gender
        [Required]
        [StringLength(20)]
        public string GenderCode { get; set; }

        //Name as in ID in English
        [Required]
        [StringLength(100)]
        public string NameInIdEn { get; set; }

        //Name as in ID in Arabic
        [StringLength(100)]
        public string NameInIdAr { get; set; }

        //ID Number
        [StringLength(30)]
        public string IDNumber { get; set; }

        //ID Number Expiry Date
        public DateTime? IDExpiryDate { get; set; }

        //DOB
        [Required]
        public DateTime? DateOfBirth { get; set; }

        //Phone Number
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        //Is Emergency Contact Number
        public bool IsEmergencyNumber { get; set; }

        //Email
        [StringLength(30)]
        public string Email { get; set; }

        //Passport Expiry Date
        public DateTime? PassportExpiryDate { get; set; }

        //Is Eligible For Exit-ReEntry
        public bool IsEligibleForExitReEntry { get; set; }

        //Is Eligible For Ticket
        public bool IsEligibleForAirTicket { get; set; }

        //Use Employee Address
        public bool UseEmployeeAddress { get; set; }

        //Address Type
        [Required]
        [StringLength(20)]
        public string AddressTypeCode { get; set; }

        //Address        
        [StringLength(500)]
        public string Address { get; set; }

        //Is Eligible For Schooling
        public bool IsEligibleForSchooling { get; set; }

        //Is Eligible For Insurance
        public bool IsEligibleForInsurance { get; set; }

        //Insurance Class 
        [StringLength(20)]
        public string InsuranceClassCode { get; set; }

        [StringLength(100)]
        public string DependentTypeName { get; set; }

        [StringLength(100)]
        public string Gender { get; set; }

        [StringLength(100)]
        public string DependentName { get; set; }
    }
}
