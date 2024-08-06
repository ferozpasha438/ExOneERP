using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.TeacherMgtDtos
{
    [AutoMap(typeof(TblSysSchoolHolidayCalanderStudent))]
    public class TeacherHolidayCalanderDto
    {

        public int Id { get; set; }
        public DateTime HDate { get; set; }
        public string HName { get; set; }
        public string HName2 { get; set; }
        [StringLength(200)]
        public string BranchCode { get; set; }

    }
}
