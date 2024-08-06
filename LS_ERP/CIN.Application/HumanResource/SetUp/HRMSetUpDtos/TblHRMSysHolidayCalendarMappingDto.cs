using AutoMapper;
using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpDtos
{
    [AutoMap(typeof(TblHRMSysHolidayCalendarMapping))]
    public class TblHRMSysHolidayCalendarMappingDto : PrimaryKeyDto<int>
    {
        [Required]
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }

        [Required]
        [StringLength(20)]
        public string HolidayCode { get; set; }
        public bool Checked { get; set; }
    }
}