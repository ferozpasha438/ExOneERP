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
    public class SchoolTeacherExamBranchDto
    {
        public SchoolTeacherExamBranchDto()
        {
            SchoolExamMgtDetails = new List<SchoolExamMgtDetailsDto>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeOfExaminationCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public int PreparedBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public bool IsResultDeclared { get; set; }
        public DateTime? DateOfResult { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<SchoolExamMgtDetailsDto> SchoolExamMgtDetails { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolExaminationManagementDetails))]
    public class SchoolExamMgtDetailsDto
    {
        public int Id { get; set; }
        public int ExamHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string SubjectCode { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
    }



    [AutoMap(typeof(TblSysSchoolAcademicBatches))]
    public class TblSysSchoolAcademicBatchesDto
    {

        public int Id { get; set; } //Identity

        public int AcademicYear { get; set; }  //Primary Key
        [Required]
        public DateTime AcademicStartDate { get; set; }
        [Required]
        public DateTime AcademicEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }



}
