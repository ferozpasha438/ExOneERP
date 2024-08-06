using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CIN.Application.TeacherMgtDtos
{
    [AutoMap(typeof(TblDefSchoolStudentMaster))]
    public class TeacherStudentDataDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime? StuAdmDate { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Alias { get; set; }
        public string GenderCode { get; set; }
        public int Age { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string PTGroupCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string LangCode { get; set; }
        public string NatCode { get; set; }
        public string ReligionCode { get; set; }
        public string StuIDNumber { get; set; }
        public string IDNumber { get; set; }
        public string MotherToungue { get; set; }
        public string RegisteredPhone { get; set; }
        public string RegisteredEmail { get; set; }
        public string StudentImageFileName { get; set; }
        public IFormFile StudentImage { get; set; }
        public string FeeStructCode { get; set; }
        public decimal? TotFeeAmount { get; set; }
        public decimal? PaidFees { get; set; }
        public decimal? NetFeeAmount { get; set; }
        public bool TransportationRequired { get; set; }
        public bool TaxApplicable { get; set; }
        public string PickNDropZone { get; set; }
        public decimal? TransportationFee { get; set; }
        public string VehicleTransport { get; set; }
        public string BuildingName { get; set; }
        public string PAddress1 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string Mobile { get; set; }
        public string FatherName { get; set; }
        public string FatherMobile { get; set; }
        public string FatherEmail { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherDesignation { get; set; }
        public string FatherSignatureFileName { get; set; }
        public IFormFile FatherSignature { get; set; }
        public string MotherName { get; set; }
        public string MotherMobile { get; set; }
        public string MotherEmail { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherDesignation { get; set; }
        public string MotherSignatureFileName { get; set; }
        public IFormFile MotherSignature { get; set; }
        public string BloodGroup { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool SpecialAssistance { get; set; }
        public string SpecialAssistanceNotes { get; set; }
        public bool PhysicalDisability { get; set; }
        public string PhysicalDisabilityNotes { get; set; }
        public int AcademicsScale { get; set; }
        public int AttentivenessScale { get; set; }
        public int HomeWorkScale { get; set; }
        public int ProjectWorkScale { get; set; }
        public int SportsPhysicalScale { get; set; }
        public int DiciplineAttitude { get; set; }
        //    public string BranchCode { get; set; }
    }
}
