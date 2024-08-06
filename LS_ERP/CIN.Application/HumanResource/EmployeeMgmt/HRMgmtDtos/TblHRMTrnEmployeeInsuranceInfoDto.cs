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
    [AutoMap(typeof(TblHRMTrnEmployeeInsuranceInfo))]
    public class TblHRMTrnEmployeeInsuranceInfoDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Insurance Type
        [Required]
        [StringLength(20)]
        public string InsuranceTypeCode { get; set; }

        //Insurance Provider
        [Required]
        [StringLength(20)]
        public string InsuranceProviderCode { get; set; }

        //Insurance Class
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
        public DateTime PolicyStartDate { get; set; }

        //Policy Expiry Date
        [Required]
        public DateTime PolicyExpiryDate { get; set; }

        //Premium per year
        [Required]
        public decimal PremiumPerYear { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(100)]
        public string InsuranceTypeName { get; set; }

        [StringLength(100)]
        public string InsuranceProviderName { get; set; }

        [StringLength(100)]
        public string InsuranceClassName { get; set; }
    }
}
