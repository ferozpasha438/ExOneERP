using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeInsuranceInfo")]
    public class TblHRMTrnEmployeeInsuranceInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Insurance Type
        [ForeignKey(nameof(InsuranceTypeCode))]
        public TblHRMSysInsuranceType SysInsuranceType { get; set; }
        [Required]
        [StringLength(20)]
        public string InsuranceTypeCode { get; set; }

        //Insurance Provider
        [ForeignKey(nameof(InsuranceProviderCode))]
        public TblHRMSysInsuranceProvider SysInsuranceProvider { get; set; }
        [Required]
        [StringLength(20)]
        public string InsuranceProviderCode { get; set; }

        //Insurance Class
        [ForeignKey(nameof(InsuranceClassCode))]
        public TblHRMSysInsuranceClass SysInsuranceClass { get; set; }
        [Required]
        [StringLength(20)]
        public string InsuranceClassCode { get; set; }

        //Policy Name
        [Required]
        [StringLength(100)]
        public string PolicyName { get; set; }

        //Policy Number
        [Required]
        [StringLength(30)]
        public string PolicyNumber { get; set; }

        //Policy Holder
        [Required]
        [StringLength(100)]
        public string PolicyHolder { get; set; }

        //Policy Start Date
        [Required]
        [Column(TypeName = "date")]
        public DateTime PolicyStartDate { get; set; }

        //Policy Expiry Date
        [Required]
        [Column(TypeName = "date")]
        public DateTime PolicyExpiryDate { get; set; }

        //Premium per year
        [Required]
        [Column(TypeName = "decimal(17,3)")]
        public decimal PremiumPerYear { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
