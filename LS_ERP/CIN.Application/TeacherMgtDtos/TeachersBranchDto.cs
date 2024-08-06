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
    [AutoMap(typeof(TblSysSchoolBranches))]
    public class TblSysSchoolBranchesDto
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public string BranchNameAr { get; set; }
        public string FinBranch { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartWeekDay { get; set; }
        public string CurrencyCode { get; set; }
        public string Website { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string NumberOfWeekDays { get; set; }
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
        public string BranchPrefix { get; set; }
        public int NextStuNum { get; set; }
        public int NextFeeVoucherNum { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }


    public class TeacherWeekOffBranchDto
    {
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }

    }


}
