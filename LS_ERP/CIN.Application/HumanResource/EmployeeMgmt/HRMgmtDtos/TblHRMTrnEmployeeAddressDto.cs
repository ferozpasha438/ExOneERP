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
    [AutoMap(typeof(TblHRMTrnEmployeeAddress))]
    public class TblHRMTrnEmployeeAddressDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Address Type
        [Required]
        [StringLength(20)]
        public string AddressTypeCode { get; set; }

        //Country
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Zone
        [Required]
        [StringLength(20)]
        public string ZoneCode { get; set; }

        //State
        [Required]
        [StringLength(20)]
        public string StateCode { get; set; }

        //City
        [Required]
        [StringLength(20)]
        public string CityCode { get; set; }

        //Zip Code
        [StringLength(30)]
        public string PostCode { get; set; }

        //Building Number
        [StringLength(100)]
        public string BuildingNumber { get; set; }

        //Additional Number
        [StringLength(100)]
        public string AdditionalNumber { get; set; }

        //Unit Number
        [StringLength(100)]
        public string UnitNumber { get; set; }

        [StringLength(100)]
        public string AddressTypeName { get; set; }

        [StringLength(100)]
        public string CountryName { get; set; }

        [StringLength(50)]
        public string ZoneName { get; set; }

        [StringLength(100)]
        public string CityName { get; set; }

        [StringLength(100)]
        public string StateName { get; set; }
    }
}
