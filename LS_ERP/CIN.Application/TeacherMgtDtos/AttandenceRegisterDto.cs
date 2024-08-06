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
 
    public class TeacherAttendanceRegisterDto
    {
        public int Id { get; set; }
        public DateTime AttnDate { get; set; }
        public string TeacherCode { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public bool IsOpen { get; set; }
        public List<TeacherAttendanceDataDto> TeacherAttendanceDataList { get; set; }
    }
    public class TeacherAttendanceDataDto
    {
        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string StudentAdmNumber { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public Char AttnFlag { get; set; }
        public bool IsPresent { get; set; }
        public bool IsLeave { get; set; }
        public string Remarks { get; set; }
    }
}
