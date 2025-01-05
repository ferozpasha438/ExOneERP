using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.SchoolMgt;
using CIN.Application.SchoolMgtDto;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetSchoolStudentMasterDetailsByMobile
    public class GetSchoolStudentMasterDetailsByMobile : IRequest<List<SchoolStudentMasterGradeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Mobile { get; set; }

    }

    public class GetSchoolStudentMasterDetailsByMobileHandler : IRequestHandler<GetSchoolStudentMasterDetailsByMobile, List<SchoolStudentMasterGradeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentMasterDetailsByMobileHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SchoolStudentMasterGradeDto>> Handle(GetSchoolStudentMasterDetailsByMobile request, CancellationToken cancellationToken)
        {

            #region OldCode

            //List<TblSysSchoolAcedemicClassGradeDto> result = new();
            //var gradeList = await _context.DefSchoolTeacherClassMapping.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).Select(x => x.GradeCode).Distinct().ToListAsync();
            //var stuResulAdmNum = await _context.SchoolStudentResultHeader.AsNoTracking()
            //                           .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade })
            //                           .Distinct()
            //                  .FirstOrDefaultAsync(x => x.GradeCode == examSingleData.GradeCode && x.ExamId == examSingleData.Id && x.StudentAdmNumber == request.AdmissionNumber);



            // var parentStudentDetails = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).Where(e => e.Mobile == request.Mobile).ToListAsync();

            // return parentStudentDetails;


            //SchoolStudentMasterGradeDto result = new SchoolStudentMasterGradeDto();
            //var student = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Mobile == request.Mobile);
            //if (student != null)
            //{
            //    result.StuAdmNum = student.StuAdmNum;
            //    result.StuAdmDate = student.StuAdmDate;
            //    result.StuName = student.StuName;
            //    result.StuName2 = student.StuName2;
            //    result.DateofBirth = student.DateofBirth;
            //    result.Alias = student.Alias;
            //    result.GenderCode = student.GenderCode;
            //    result.Age = student.Age;
            //    result.BranchCode = student.BranchCode;
            //    result.GradeCode = student.GradeCode;
            //    result.PTGroupCode = student.PTGroupCode;
            //    result.GradeSectionCode = student.GradeSectionCode;
            //    result.LangCode = student.LangCode;
            //    result.NatCode = student.NatCode;
            //    result.ReligionCode = student.ReligionCode;
            //    result.StuIDNumber = student.StuIDNumber;
            //    result.IDNumber = student.IDNumber;
            //    result.MotherToungue = student.MotherToungue;
            //    result.RegisteredPhone = student.RegisteredPhone;
            //    result.RegisteredEmail = student.RegisteredEmail;
            //    result.IsActive = student.IsActive;
            //    result.StudentImageFileName = student.Image1Path;

            //    result.TaxApplicable = student.TaxApplicable;
            //    result.TransportationRequired = student.TransportationRequired;
            //    result.PickNDropZone = student.PickNDropZone;
            //    result.TransportationFee = student.TransportationFee;
            //    result.VehicleTransport = student.VehicleTransport;
            //    result.PAddress1 = student.PAddress1;
            //    result.BuildingName = student.BuildingName;
            //    result.City = student.City;
            //    result.Phone = student.Phone;
            //    result.ZipCode = student.ZipCode;
            //    result.Mobile = student.Mobile;
            //    result.FatherName = student.FatherName;
            //    result.MotherName = student.MotherName;
            //    result.FatherSignatureFileName = student.SignatureImage1;
            //    result.MotherSignatureFileName = student.SignatureImage2;
            //    result.BloodGroup = student.BloodGroup;
            //    result.Height = student.Height;
            //    result.Weight = student.Weight;
            //    result.SpecialAssistance = student.SpecialAssistance;
            //    result.SpecialAssistanceNotes = student.SpecialAssistanceNotes;
            //    result.PhysicalDisability = student.PhysicalDisability;
            //    result.PhysicalDisabilityNotes = student.PhysicalDisabilityNotes;
            //    result.AcademicsScale = student.AcademicsScale;
            //    result.AttentivenessScale = student.AttentivenessScale;
            //    result.HomeWorkScale = student.HomeWorkScale;
            //    result.ProjectWorkScale = student.ProjectWorkScale;
            //    result.SportsPhysicalScale = student.SportsPhysicalScale;
            //    result.DiciplineAttitude = student.DiciplineAttitude;
            //    result.AcademicsScale = student.AcademicsScale;
            //    result.FeeStructCode = student.FeeStructCode;
            //    TblSysSchoolAcedemicClassGrade gradeDetails = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(e => e.GradeCode == student.GradeCode );
            //    result.FileName = gradeDetails.FileName;
            //}

            //return result;

            #endregion

            var studentStuAdmList = await _context.DefStudentGuardiansSiblings.AsNoTracking().ProjectTo<TblDefStudentGuardiansSiblingsDto>(_mapper.ConfigurationProvider).Where(e => e.Mobile1 == request.Mobile).Select(x=>x.StuAdmNum).Distinct().ToListAsync();

            List<SchoolStudentMasterGradeDto> studentDetails = new List<SchoolStudentMasterGradeDto>();
            studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).
                                                      Where(e => e.Mobile == request.Mobile || e.RegisteredPhone==request.Mobile || e.Phone == request.Mobile || studentStuAdmList.Contains(e.StuAdmNum)
                                                        ).
                                                      Select(x => new SchoolStudentMasterGradeDto
                                                      {
                                                          Id = x.Id,
                                                          StuAdmNum = x.StuAdmNum,
                                                          StuAdmDate = x.StuAdmDate,
                                                          StuName = x.StuName,
                                                          StuName2 = x.StuName2,
                                                          DateofBirth = x.DateofBirth,
                                                          Alias = x.Alias,
                                                          GenderCode = x.GenderCode,
                                                          Age = x.Age,
                                                          BranchCode = x.BranchCode,
                                                          GradeCode = x.GradeCode,
                                                          PTGroupCode = x.PTGroupCode,
                                                          GradeSectionCode = x.GradeSectionCode,
                                                          LangCode = x.LangCode,
                                                          NatCode = x.NatCode,
                                                          ReligionCode = x.ReligionCode,
                                                          StuIDNumber = x.StuIDNumber,
                                                          IDNumber = x.IDNumber,
                                                          MotherToungue = x.MotherToungue,
                                                          RegisteredPhone = x.RegisteredPhone,
                                                          RegisteredEmail = x.RegisteredEmail,
                                                          IsActive = x.IsActive,
                                                          Image1Path = x.Image1Path,
                                                          TaxApplicable = x.TaxApplicable,
                                                          TransportationRequired = x.TransportationRequired,
                                                          PickNDropZone = x.PickNDropZone,
                                                          TransportationFee = x.TransportationFee,
                                                          VehicleTransport = x.VehicleTransport,
                                                          PAddress1 = x.PAddress1,
                                                          BuildingName = x.BuildingName,
                                                          City = x.City,
                                                          Phone = x.Phone,
                                                          ZipCode = x.ZipCode,
                                                          Country = x.Country,
                                                          Mobile = x.Mobile,
                                                          FatherName = x.FatherName,
                                                          MotherName = x.MotherName,
                                                          FatherSignatureFileName = x.SignatureImage1,
                                                          MotherSignatureFileName = x.SignatureImage2,
                                                          BloodGroup = x.BloodGroup,
                                                          Height = x.Height,
                                                          Weight = x.Weight,
                                                          SpecialAssistance = x.SpecialAssistance,
                                                          SpecialAssistanceNotes = x.SpecialAssistanceNotes,
                                                          PhysicalDisability = x.PhysicalDisability,
                                                          PhysicalDisabilityNotes = x.PhysicalDisabilityNotes,
                                                          AcademicsScale = x.AcademicsScale,
                                                          AttentivenessScale = x.AttentivenessScale,
                                                          HomeWorkScale = x.HomeWorkScale,
                                                          ProjectWorkScale = x.ProjectWorkScale,
                                                          SportsPhysicalScale = x.SportsPhysicalScale,
                                                          DiciplineAttitude = x.DiciplineAttitude,
                                                          FeeStructCode = x.FeeStructCode
                                                      }).
                                                       OrderByDescending(e => e.FatherName).ToListAsync();
            foreach (var item in studentDetails)
            {
                var schoolGradeSection = await _context.SchoolGradeSectionMapping.AsNoTracking().ProjectTo<TblSysSchoolGradeSectionMapping1Dto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.GradeCode == item.GradeCode && e.SectionCode == item.GradeSectionCode);

                if (schoolGradeSection != null)
                {
                    item.FileName = schoolGradeSection.FileName;

                }
            }
            return studentDetails;

        }


    }

    #endregion

    #region GetSchoolStudentFeeHeaderByStuAdmNum
    public class GetStudentFeeHeaderByStuAdmNum : IRequest<List<TblDefStudentFeeHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }

    }

    public class GetStudentFeeHeaderByStuAdmNumHandler : IRequestHandler<GetStudentFeeHeaderByStuAdmNum, List<TblDefStudentFeeHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentFeeHeaderByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefStudentFeeHeaderDto>> Handle(GetStudentFeeHeaderByStuAdmNum request, CancellationToken cancellationToken)
        {
            try
            {
                var studentFeeHeader = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider).Where(e => e.StuAdmNum == request.StuAdmNum).ToListAsync();
                return studentFeeHeader;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }

    #endregion

    #region GetSchoolStudentFeeDetailsByStuAdmNum
    public class GetSchoolStudentFeeDetailsByStuAdmNum : IRequest<List<TblDefStudentFeeDetailsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }
    }

    public class GetSchoolStudentFeeDetailsByStuAdmNumHandler : IRequestHandler<GetSchoolStudentFeeDetailsByStuAdmNum, List<TblDefStudentFeeDetailsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentFeeDetailsByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefStudentFeeDetailsDto>> Handle(GetSchoolStudentFeeDetailsByStuAdmNum request, CancellationToken cancellationToken)
        {

            var studentFeeDetails = await _context.DefStudentFeeDetails.AsNoTracking().ProjectTo<TblDefStudentFeeDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.StuAdmNum == request.StuAdmNum).OrderByDescending(e => e.TermCode).ToListAsync();


            return studentFeeDetails;
        }
    }

    #endregion

    #region GetSchoolStudentById
    public class GetSchoolStudentById : IRequest<SchoolStudentMasterGradeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetSchoolStudentByIdHandler : IRequestHandler<GetSchoolStudentById, SchoolStudentMasterGradeDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetSchoolStudentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolStudentMasterGradeDto> Handle(GetSchoolStudentById request, CancellationToken cancellationToken)
        {
            //var student = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            //return student;


            SchoolStudentMasterGradeDto result = new SchoolStudentMasterGradeDto();
            var student = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (student != null)
            {
                result.Id = student.Id;
                result.StuAdmNum = student.StuAdmNum;
                result.StuAdmDate = student.StuAdmDate;
                result.StuName = student.StuName;
                result.StuName2 = student.StuName2;
                result.DateofBirth = student.DateofBirth;
                result.Alias = student.Alias;
                result.GenderCode = student.GenderCode;
                result.Age = student.Age;
                result.BranchCode = student.BranchCode;
                result.GradeCode = student.GradeCode;
                result.PTGroupCode = student.PTGroupCode;
                result.GradeSectionCode = student.GradeSectionCode;
                result.LangCode = student.LangCode;
                result.NatCode = student.NatCode;
                result.ReligionCode = student.ReligionCode;
                result.StuIDNumber = student.StuIDNumber;
                result.IDNumber = student.IDNumber;
                result.MotherToungue = student.MotherToungue;
                result.RegisteredPhone = student.RegisteredPhone;
                result.RegisteredEmail = student.RegisteredEmail;
                result.IsActive = student.IsActive;
                result.Image1Path = student.Image1Path;

                result.TaxApplicable = student.TaxApplicable;
                result.TransportationRequired = student.TransportationRequired;
                result.PickNDropZone = student.PickNDropZone;
                result.TransportationFee = student.TransportationFee;
                result.VehicleTransport = student.VehicleTransport;
                result.PAddress1 = student.PAddress1;
                result.BuildingName = student.BuildingName;
                result.City = student.City;
                result.Phone = student.Phone;
                result.ZipCode = student.ZipCode;
                result.Country = student.Country;
                result.Mobile = student.Mobile;
                result.FatherName = student.FatherName;
                result.MotherName = student.MotherName;
                result.FatherSignatureFileName = student.SignatureImage1;
                result.MotherSignatureFileName = student.SignatureImage2;
                result.BloodGroup = student.BloodGroup;
                result.Height = student.Height;
                result.Weight = student.Weight;
                result.SpecialAssistance = student.SpecialAssistance;
                result.SpecialAssistanceNotes = student.SpecialAssistanceNotes;
                result.PhysicalDisability = student.PhysicalDisability;
                result.PhysicalDisabilityNotes = student.PhysicalDisabilityNotes;
                result.AcademicsScale = student.AcademicsScale;
                result.AttentivenessScale = student.AttentivenessScale;
                result.HomeWorkScale = student.HomeWorkScale;
                result.ProjectWorkScale = student.ProjectWorkScale;
                result.SportsPhysicalScale = student.SportsPhysicalScale;
                result.DiciplineAttitude = student.DiciplineAttitude;
                result.AcademicsScale = student.AcademicsScale;
                result.FeeStructCode = student.FeeStructCode;
                TblSysSchoolGradeSectionMapping gradeSectionDetails = await _context.SchoolGradeSectionMapping.AsNoTracking().FirstOrDefaultAsync(e => e.GradeCode == student.GradeCode && e.SectionCode == student.GradeSectionCode);
                result.FileName = gradeSectionDetails.FileName;
            }

            return result;

        }
    }

    #endregion

    #region GetStudentMasterFormDataById
    public class GetStudentMasterFormDataById : IRequest<AllStudentMasterDataDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetStudentMasterFormDataByIdIdHandler : IRequestHandler<GetStudentMasterFormDataById, AllStudentMasterDataDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetStudentMasterFormDataByIdIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AllStudentMasterDataDto> Handle(GetStudentMasterFormDataById request, CancellationToken cancellationToken)
        {
            AllStudentMasterDataDto result = new AllStudentMasterDataDto();
            var student = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (student != null)
            {
                result.StuAdmNum = student.StuAdmNum;
                result.StuAdmDate = student.StuAdmDate;
                result.StuName = student.StuName;
                result.StuName2 = student.StuName2;
                result.DateofBirth = student.DateofBirth;
                result.Alias = student.Alias;
                result.GenderCode = student.GenderCode;
                result.Age = student.Age;
                result.BranchCode = student.BranchCode;
                result.GradeCode = student.GradeCode;
                result.PTGroupCode = student.PTGroupCode;
                result.GradeSectionCode = student.GradeSectionCode;
                result.LangCode = student.LangCode;
                result.NatCode = student.NatCode;
                result.ReligionCode = student.ReligionCode;
                result.StuIDNumber = student.StuIDNumber;
                result.IDNumber = student.IDNumber;
                result.MotherToungue = student.MotherToungue;
                result.RegisteredPhone = student.RegisteredPhone;
                result.RegisteredEmail = student.RegisteredEmail;
                result.IsActive = student.IsActive;
                result.StudentImageFileName = student.Image1Path;

                result.TaxApplicable = student.TaxApplicable;
                result.TransportationRequired = student.TransportationRequired;
                result.PickNDropZone = student.PickNDropZone;
                result.TransportationFee = student.TransportationFee;
                result.VehicleTransport = student.VehicleTransport;
                result.PAddress1 = student.PAddress1;
                result.BuildingName = student.BuildingName;
                result.City = student.City;
                result.Phone = student.Phone;
                result.ZipCode = student.ZipCode;
                result.Mobile = student.Mobile;
                result.FatherName = student.FatherName;
                result.MotherName = student.MotherName;
                result.FatherSignatureFileName = student.SignatureImage1;
                result.MotherSignatureFileName = student.SignatureImage2;
                result.BloodGroup = student.BloodGroup;
                result.Height = student.Height;
                result.Weight = student.Weight;
                result.SpecialAssistance = student.SpecialAssistance;
                result.SpecialAssistanceNotes = student.SpecialAssistanceNotes;
                result.PhysicalDisability = student.PhysicalDisability;
                result.PhysicalDisabilityNotes = student.PhysicalDisabilityNotes;
                result.AcademicsScale = student.AcademicsScale;
                result.AttentivenessScale = student.AttentivenessScale;
                result.HomeWorkScale = student.HomeWorkScale;
                result.ProjectWorkScale = student.ProjectWorkScale;
                result.SportsPhysicalScale = student.SportsPhysicalScale;
                result.DiciplineAttitude = student.DiciplineAttitude;
                result.AcademicsScale = student.AcademicsScale;
                result.FeeStructCode = student.FeeStructCode;
                TblDefStudentGuardiansSiblings fatherDetails = await _context.DefStudentGuardiansSiblings.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == student.StuAdmNum && e.Remarks == "Student_Master_Father");
                if (fatherDetails != null)
                {
                    result.FatherMobile = fatherDetails.Mobile1;
                    result.FatherEmail = fatherDetails.email;
                    result.FatherOccupation = fatherDetails.Occupation;
                    result.FatherDesignation = fatherDetails.Deisgnation;
                }
                TblDefStudentGuardiansSiblings motherDetails = await _context.DefStudentGuardiansSiblings.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == student.StuAdmNum && e.Remarks == "Student_Master_Mother");
                if (motherDetails != null)
                {
                    result.MotherMobile = motherDetails.Mobile1;
                    result.MotherEmail = motherDetails.email;
                    result.MotherOccupation = motherDetails.Occupation;
                    result.MotherDesignation = motherDetails.Deisgnation;
                }
                result.TotFeeAmount = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == student.StuAdmNum && e.FeeStructCode == student.FeeStructCode && e.IsPaid == false).SumAsync(x => x.FeeAmount);
                result.PaidFees = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == student.StuAdmNum && e.FeeStructCode == student.FeeStructCode && e.IsPaid == true).SumAsync(x => x.FeeAmount);
                result.NetFeeAmount = result.TotFeeAmount - result.PaidFees;
            }
            return result;
        }
    }

    #endregion

    #region GetSchoolStudentFeeDetailsByStuAdmNum
    public class GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode : IRequest<List<StudentFeeDetailsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }
        public string TermCode { get; set; }
    }

    public class GetSchoolStudentFeeDetailsByStuAdmNumANDTermCodeHandler : IRequestHandler<GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode, List<StudentFeeDetailsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentFeeDetailsByStuAdmNumANDTermCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<StudentFeeDetailsDto>> Handle(GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode request, CancellationToken cancellationToken)
        {
            List<StudentFeeDetailsDto> studentFeeDetails = new List<StudentFeeDetailsDto>();
            studentFeeDetails = await _context.DefStudentFeeDetails.AsNoTracking().ProjectTo<TblDefStudentFeeDetailsDto>(_mapper.ConfigurationProvider).
                                                      Where(e => e.StuAdmNum == request.StuAdmNum && e.TermCode == request.TermCode).
                                                      Select(x => new StudentFeeDetailsDto
                                                      {
                                                          AcademicYear = x.AcademicYear,
                                                          AddedBy = x.AddedBy,
                                                          AddedOn = x.AddedOn,
                                                          DiscPer = x.DiscPer,
                                                          FeeAmount = x.FeeAmount,
                                                          FeeCode = x.FeeCode,
                                                          FeeStructCode = x.FeeStructCode,
                                                          Id = x.Id,
                                                          IsAddedManaully = x.IsAddedManaully,
                                                          IsLateFee = x.IsLateFee,
                                                          IsPaid = x.IsPaid,
                                                          IsVoided = x.IsVoided,
                                                          MaxDiscPer = x.MaxDiscPer,
                                                          NetDiscAmt = x.NetDiscAmt,
                                                          NetFeeAmount = x.NetFeeAmount,
                                                          StuAdmNum = x.StuAdmNum,
                                                          TermCode = x.TermCode,
                                                          VoidedBy = x.VoidedBy,
                                                          VoidReason = x.VoidReason,
                                                          TaxAmount = x.TaxAmount
                                                      }).
                                                       OrderByDescending(e => e.Id).ToListAsync();
            foreach (var item in studentFeeDetails)
            {
                var schoolFeeType = await _context.SysSchoolFeeType.AsNoTracking().ProjectTo<TblSysSchoolFeeTypeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.FeeCode == item.FeeCode);
                if (schoolFeeType != null)
                {
                    item.FeeName = schoolFeeType.FeesName;
                    item.FeeName2 = schoolFeeType.FeeName2;
                }
            }
            return studentFeeDetails;
        }
    }

    #endregion

    #region GetSchoolStudentFeeHeaderByStuAdmNum
    public class GetStudentDetailsByStuAdmNum : IRequest<StudentDetailsForFeeTransactionDto>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }

    }

    public class GetStudentDetailsByStuAdmNumHandler : IRequestHandler<GetStudentDetailsByStuAdmNum, StudentDetailsForFeeTransactionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentDetailsByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StudentDetailsForFeeTransactionDto> Handle(GetStudentDetailsByStuAdmNum request, CancellationToken cancellationToken)
        {
            try
            {
                StudentDetailsForFeeTransactionDto studentDetails = new();
                var result = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider)
                       .FirstOrDefaultAsync(e => e.StuAdmNum == request.StuAdmNum);
                if (result != null)
                {
                    studentDetails.StuAdmNum = result.StuAdmNum;
                    studentDetails.AdmissionDate = result.StuAdmDate;
                    studentDetails.StudentName = result.StuName;
                    studentDetails.StudentNameAR = result.StuName2;
                    studentDetails.BranchCode = result.BranchCode;
                    studentDetails.FeeAmount = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider)
                       .Where(e => e.StuAdmNum == result.StuAdmNum && e.FeeStructCode == result.FeeStructCode && e.IsPaid == false).SumAsync(x => x.NetFeeAmount);
                }
                return studentDetails;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }

    #endregion

    #region UpdateStatus 
    public class UpdateStatus : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }
        public bool IsChecked { get; set; }
    }

    public class UpdateStatusHandler : IRequestHandler<UpdateStatus, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public UpdateStatusHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            bool result = false;
            var studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == request.StuAdmNum);
            if (studentDetails != null)
            {
                studentDetails.IsActive = request.IsChecked;
                _context.DefSchoolStudentMaster.Update(studentDetails);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }
    }

    #endregion

    #region GetStudentAttandace
    public class GetStudentAttandace : IRequest<StudentAttandanceResultDto>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class GetStudentAttandaceHandler : IRequestHandler<GetStudentAttandace, StudentAttandanceResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentAttandaceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StudentAttandanceResultDto> Handle(GetStudentAttandace request, CancellationToken cancellationToken)
        {
            StudentAttandanceResultDto data = new();
            StudentAttandanceDataDto result = new();
            DateTime presentDate = DateTime.Now;
            result.FromDate = new DateTime(request.Year, request.Month, 1);
            result.ToDate = result.FromDate.AddMonths(1).AddDays(-1);
            if (result.ToDate > presentDate)
                result.ToDate = presentDate;
            result.NoOfDays = Convert.ToInt32((result.ToDate - result.FromDate).TotalDays + 1);
            var studentAttData = await _context.StudentAttendance.AsNoTracking().ProjectTo<TblDefStudentAttendanceDto>(_mapper.ConfigurationProvider).
                                                      Where(e => e.StuAdmNum == request.StuAdmNum
                                                        && e.AtnDate.Date >= result.FromDate.Date && e.AtnDate.Date <= result.ToDate.Date)
                                                      .ToListAsync();
            result.PresentDays = studentAttData.Count(x => x.AtnFlag == "P");
            var studentLeaveData = await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider).
                                                      Where(e => e.StuAdmNum == request.StuAdmNum
                                                        && (e.LeaveStartDate.Date >= result.FromDate.Date && e.LeaveStartDate.Date <= result.ToDate.Date
                                                        || e.LeaveEndDate.Date >= result.FromDate.Date && e.LeaveEndDate.Date <= result.ToDate.Date))
                                                      .ToListAsync();
            List<StudentLeaveDataDto> studentLeaveDates = new();
            foreach (var item in studentLeaveData)
            {
                for (var hDate = item.LeaveStartDate.Date; hDate <= item.LeaveEndDate.Date; hDate = hDate.AddDays(1))
                {
                    if (hDate >= result.FromDate && hDate <= result.ToDate && !studentLeaveDates.Any(x => x.LeaveDate == hDate))
                        studentLeaveDates.Add(new StudentLeaveDataDto() { LeaveDate = hDate, LeaveCode = item.LeaveCode });
                }
            }
            result.LeaveDays = studentLeaveDates.Count;
            var holidayData = await _context.StudentHolidayClaender.AsNoTracking().ProjectTo<TblSysSchoolHolidayCalanderStudentDto>(_mapper.ConfigurationProvider).
                                                      Where(e => e.HDate.Date >= result.FromDate.Date && e.HDate.Date <= result.ToDate.Date)
                                                      .ToListAsync();
            result.HolidayDays = holidayData.Count;
            result.AbsentDays = result.NoOfDays - (result.PresentDays + result.LeaveDays + result.HolidayDays);
            data.StudentAttandanceData = result;
            for (var sDate = result.FromDate; sDate <= result.ToDate; sDate = sDate.AddDays(1))
            {
                var dayData = studentAttData.FirstOrDefault(x => x.AtnDate.Date == sDate.Date);
                if (dayData != null)
                    data.AttnDaywiseDataList.Add(new StudentAttandanceDatewiseDto() { AttDate = sDate, Flag = dayData.AtnFlag, InTime = dayData.AtnTimeIn.TimeOfDay.ToString().Substring(0, 5), OutTime = dayData.AtnTimeOut == null ? "" : dayData.AtnTimeOut?.TimeOfDay.ToString().Substring(0, 5), LeaveCode = "-" });
                else if (studentLeaveDates.Any(x => x.LeaveDate.Date == sDate.Date))
                    data.AttnDaywiseDataList.Add(new StudentAttandanceDatewiseDto() { AttDate = sDate, Flag = "L", InTime = "-", OutTime = "-", LeaveCode = studentLeaveDates.FirstOrDefault(x => x.LeaveDate.Date == sDate.Date).LeaveCode });
                else if (holidayData.Any(x => x.HDate.Date == sDate.Date))
                    data.AttnDaywiseDataList.Add(new StudentAttandanceDatewiseDto() { AttDate = sDate, Flag = "H", InTime = "-", OutTime = "-", LeaveCode = "-" });
                else
                    data.AttnDaywiseDataList.Add(new StudentAttandanceDatewiseDto() { AttDate = sDate, Flag = "A", InTime = "-", OutTime = "-", LeaveCode = "-" });

            }
            return data;
        }
    }

    #endregion

    #region GetStudentFeeDetails
    public class GetStudentFeeDetails : IRequest<List<StudentFeePayDetailsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }
    }

    public class GetStudentFeeDetailsHandler : IRequestHandler<GetStudentFeeDetails, List<StudentFeePayDetailsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentFeeDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<StudentFeePayDetailsDto>> Handle(GetStudentFeeDetails request, CancellationToken cancellationToken)
        {
            List<StudentFeePayDetailsDto> resultData = new();
            try
            {
                var taxData = await _context.SystemTaxes.AsNoTracking().FirstOrDefaultAsync();
                var academicDetails = await _context.DefStudentAcademics
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync();
                if (academicDetails != null && taxData != null)
                {
                    resultData = await _context.DefStudentFeeHeader
                        .AsNoTracking()
                        .Where(e => e.StuAdmNum == request.StuAdmNum
                        && e.AcademicYear == academicDetails.AcademicYear)
                        .Select(x => new StudentFeePayDetailsDto
                        {
                            StuAdmNum = x.StuAdmNum,
                            TermCode = x.TermCode,
                            FeeDueDate = x.FeeDueDate,
                            TotFeeAmount = x.TotFeeAmount,
                            TaxAmount = x.TaxAmount,
                            NetFeeAmount = x.NetFeeAmount,
                            IsPaid = x.IsPaid,
                            PaidOn = x.PaidOn,
                            FeeStructCode = x.FeeStructCode
                        })
                        .ToListAsync();
                    foreach (var item in resultData)
                    {
                        var stuctDetails = await _context.SchoolFeeStructureHeader
                                                  .AsNoTracking()
                                                .FirstOrDefaultAsync(e => e.FeeStructCode == item.FeeStructCode);
                        if (stuctDetails != null && stuctDetails.ApplyLateFee)
                        {
                            item.LateFee = stuctDetails.ActualFeePayable;
                            item.TaxAmount = ((item.TotFeeAmount + stuctDetails.ActualFeePayable) / 100) * taxData.Taxper01;
                            item.NetFeeAmount = item.TotFeeAmount + item.LateFee + item.TaxAmount;
                        }
                    }
                }

                return resultData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }

    #endregion


    #region GetStudentFeeHeaderWithVatByStuAdmNum
    public class GetStudentFeeHeaderWithVatByStuAdmNum : IRequest<List<StudentFeeHeaderPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }

    }

    public class GetStudentFeeHeaderWithVatByStuAdmNumHandler : IRequestHandler<GetStudentFeeHeaderWithVatByStuAdmNum, List<StudentFeeHeaderPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentFeeHeaderWithVatByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<StudentFeeHeaderPaymentDto>> Handle(GetStudentFeeHeaderWithVatByStuAdmNum request, CancellationToken cancellationToken)
        {
            try
            {
                List<StudentFeeHeaderPaymentDto> result = new();
                result = await _context.DefStudentFeeHeader.AsNoTracking()
                         .Where(e => e.StuAdmNum == request.StuAdmNum)
                         .Select(x => new StudentFeeHeaderPaymentDto
                         {
                             AcademicYear = x.AcademicYear,
                             DiscAmount = x.DiscAmount,
                             DiscReason = x.DiscReason,
                             FeeDueDate = x.FeeDueDate,
                             FeeStructCode = x.FeeStructCode,
                             Id = x.Id,
                             IsCompletelyPaid = x.IsCompletelyPaid,
                             IsPaid = x.IsPaid,
                             JvNumber = x.JvNumber,
                             NetFeeAmount = x.NetFeeAmount - x.PaidAmount,
                             PaidOn = x.PaidOn,
                             PaidRemarks1 = x.PaidRemarks1,
                             PaidRemarks2 = x.PaidRemarks2,
                             PaidTransNum = x.PaidTransNum,
                             StuAdmNum = x.StuAdmNum,
                             TaxAmount = x.TaxAmount,
                             TermCode = x.TermCode,
                             TotFeeAmount = x.TotFeeAmount,
                             PaidAmount = x.PaidAmount,
                             IsPartiallyPaid = x.IsPartiallyPaid
                         }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }



    #endregion

}
