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
    [AutoMap(typeof(TblHRMTrnEmployeeLanguageInfo))]
    public class TblHRMTrnEmployeeLanguageInfoDto : PrimaryKeyDto<int>
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string LanguageCode { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSpeak { get; set; }
    }
}
