using CIN.Domain.HumanResource.Setup;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeAddress")]
    public class TblHRMTrnEmployeeAddress : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Address Type
        [ForeignKey(nameof(AddressTypeCode))]
        public TblHRMSysAddressType SysAddressType { get; set; }
        [Required]
        [StringLength(20)]
        public string AddressTypeCode { get; set; }

        //Country
        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountryCode { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Zone
        [Required]
        [StringLength(20)]
        public string ZoneCode { get; set; }

        //State
        [ForeignKey(nameof(StateCode))]
        public TblErpSysStateCode SysStateCode { get; set; }
        [Required]
        [StringLength(20)]
        public string StateCode { get; set; }

        //City
        [ForeignKey(nameof(CityCode))]
        public TblErpSysCityCode SysCityCode { get; set; }
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
    }
}
