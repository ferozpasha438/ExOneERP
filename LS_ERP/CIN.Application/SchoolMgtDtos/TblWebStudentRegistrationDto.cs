using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SchoolMgtDtos
{
    [AutoMap(typeof(TblWebStudentRegistration))]
    public class TblWebStudentRegistrationDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        
        [StringLength(50)]
        public string RegNum { get; set; }
        [StringLength(250)]
        public string NameAr { get; set; }
        public string GenderName { get; set; }
        public string DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public string ReligionCode { get; set; }
        public string LangCode { get; set; }
        public string Grade { get; set; }
        public string City { get; set; }
        public string IDNumber { get; set; }
        public bool PhysicalDisability { get; set; }
        public string PhysicalDisabilityNotes { get; set; }
        public bool MedicalIssue { get; set; }
        public string MedicalIssueNotes { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherEmail { get; set; }
        public string FatherPhoneNumber { get; set; }

        public string MotherPhoneNumber { get; set; }

        public int EnglishFluencyLevel { get; set; }

        public string Remarks { get; set; }

        public Boolean IsyourchildPottytrained { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

}
