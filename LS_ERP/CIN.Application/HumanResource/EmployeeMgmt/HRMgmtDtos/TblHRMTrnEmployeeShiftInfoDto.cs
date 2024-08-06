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
    [AutoMap(typeof(TblHRMTrnEmployeeShiftInfo))]
    public class TblHRMTrnEmployeeShiftInfoDto : AuditableEntityDto<int>
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string MondayShiftCode { get; set; }

        [Required]
        [StringLength(20)]
        public string TuesdayShiftCode { get; set; }

        [Required]
        [StringLength(20)]
        public string WednesdayShiftCode { get; set; }

        [Required]
        [StringLength(20)]
        public string ThursdayShiftCode { get; set; }

        [Required]
        [StringLength(20)]
        public string FridayShiftCode { get; set; }

        [Required]
        [StringLength(20)]
        public string SaturdayShiftCode { get; set; }


        [Required]
        [StringLength(20)]
        public string SundayShiftCode { get; set; }
        public string EmployeeNameEn { get; set; }
        public string EmployeeNameAr { get; set; }
    }
}
