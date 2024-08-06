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

    public class MobStudentResultData
    {

        public string SubCodes { get; set; }
        public string SubjectName { get; set; }
        public string SubjectName2 { get; set; }
        public decimal? MaximumMarks { get; set; }
        public decimal? QualifyingMarks { get; set; }
        public decimal? SubjectMarks { get; set; }
        public string QualifyingGrade { get; set; }
    }
    public class MobStudentExamResultDataDto
    {
        public MobStudentExamResultDataDto()
        {
            StudentResults = new List<MobStudentResultData>();
        }
        public int ExamHeaderID { get; set; }
        public string ExaminationCode { get; set; }
        public string StudentAdmission { get; set; }
        public string GradeCode { get; set; }
        public List<MobStudentResultData> StudentResults { get; set; }


    }

    public class ExamSelectListItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }


}
