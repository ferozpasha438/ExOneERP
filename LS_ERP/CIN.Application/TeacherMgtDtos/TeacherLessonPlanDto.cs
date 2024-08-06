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
    public class TeacherLessonPlanInfoDto
    {
        public TeacherLessonPlanInfoDto()
        {
            LessonPlanDetailsRows = new();
        }
        public int Id { get; set; }
        public string LessonPlanCode { get; set; }
        public string BranchCode { get; set; }
        public string TeacherCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string SubCodes { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public int NumOfDays { get; set; }
        public int NumOfLessons { get; set; }
        public List<LessonPlanDetailsDto> LessonPlanDetailsRows { get; set; }

    }
    [AutoMap(typeof(TblLessonPlanDetails))]
    public class LessonPlanDetailsDto
    {
        public int Id { get; set; }
        public string LessonPlanCode { get; set; }
        public string Chapter { get; set; }
        public string LessonName { get; set; }
        public string LessonName2 { get; set; }
        public string Topics { get; set; }
        public string Topics2 { get; set; }
        public string NumOfSessions { get; set; }
        public DateTime TopicDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    
}
