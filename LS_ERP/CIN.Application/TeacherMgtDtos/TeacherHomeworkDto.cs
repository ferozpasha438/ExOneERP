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
    [AutoMap(typeof(TblStudentHomeWork))]
    public class TeacherStudentHomeWorkDto
    {
        public int Id { get; set; }
        public DateTime HomeworkDate { get; set; }
        public string GradeCode { get; set; }
        public string SubCodes { get; set; }
        public string TeacherCode { get; set; }
        public string HomeWorkDescription { get; set; }
        public string HomeWorkDescription_Ar { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
