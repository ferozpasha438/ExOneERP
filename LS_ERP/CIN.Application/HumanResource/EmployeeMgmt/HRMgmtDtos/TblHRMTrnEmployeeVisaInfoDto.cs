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
    [AutoMap(typeof(TblHRMTrnEmployeeVisaInfo))]
    public class TblHRMTrnEmployeeVisaInfoDto : AuditableEntityDto<int>
    {
        //EmployeeNumber
        [Required]
        public int EmployeeID { get; set; }

        //Country
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Visa Type
        [Required]
        [StringLength(20)]
        public string VisaTypeCode { get; set; }

        //Visa Number
        [Required]
        [StringLength(30)]
        public string VisaNumber { get; set; }

        //Valid From
        [Required]
        public DateTime ValidFrom { get; set; }

        //Valid To
        [Required]
        public DateTime ValidTo { get; set; }

        //Issue Location
        [StringLength(30)]
        public string IssueLocation { get; set; }

        [StringLength(100)]
        public string CountryName { get; set; }

        [StringLength(100)]
        public string VisaTypeName { get; set; }
        public bool IsVisaValid { get; set; }
    }
}
