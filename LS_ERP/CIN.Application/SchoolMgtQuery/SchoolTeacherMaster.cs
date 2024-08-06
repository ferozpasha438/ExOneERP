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
using CIN.Domain.SystemSetup;

namespace CIN.Application.SchoolMgtQuery
{
    public class SchoolTeacherMaster
    {
    }
    #region GetTeacherCode

    public class GetTeacherCode : IRequest<string>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetTeacherCodeHandler : IRequestHandler<GetTeacherCode, string>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> Handle(GetTeacherCode request, CancellationToken cancellationToken)
        {
            var techareCode = await _context.DefSchoolTeacherMaster.AsNoTracking()
                    .Where(e => e.SysLoginId == request.Id).Select(x => x.TeacherCode)
                  .FirstOrDefaultAsync(cancellationToken);
            return techareCode;
        }
    }

    #endregion
    #region GetSchoolTeacherMasterList
    public class GetSchoolTeacherMasterList : IRequest<PaginatedList<TblDefSchoolTeacherMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolTeacherMasterListHandler : IRequestHandler<GetSchoolTeacherMasterList, PaginatedList<TblDefSchoolTeacherMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolTeacherMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefSchoolTeacherMasterDto>> Handle(GetSchoolTeacherMasterList request, CancellationToken cancellationToken)
        {
            try
            {

                var list = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider)
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return list;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion

    #region GetTeacherMasterDataById
    public class GetTeacherMasterDataById : IRequest<SchoolTeacherMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetTeacherMasterDataByIdHandler : IRequestHandler<GetTeacherMasterDataById, SchoolTeacherMasterDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetTeacherMasterDataByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolTeacherMasterDto> Handle(GetTeacherMasterDataById request, CancellationToken cancellationToken)
        {
            SchoolTeacherMasterDto result = new SchoolTeacherMasterDto();
            var teacherData = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (teacherData != null)
            {
                result.Id = teacherData.Id;
                result.TeacherCode = teacherData.TeacherCode;
                result.TeacherName1 = teacherData.TeacherName1;
                result.TeacherName2 = teacherData.TeacherName2;
                result.TeacherShortName = teacherData.TeacherShortName;
                result.FatherName = teacherData.FatherName;
                result.MaritalStatus = teacherData.MaritalStatus;
                result.PAddress = teacherData.PAddress;
                result.SpouseName = teacherData.SpouseName;
                result.Pcity = teacherData.Pcity;
                result.PPhone1 = teacherData.PPhone1;
                result.SPhone2 = teacherData.SPhone2;
                result.TeacherEmail = teacherData.TeacherEmail;
                result.PMobile1 = teacherData.PMobile1;
                result.Gender = teacherData.Gender;
                result.HiringType = teacherData.HiringType;
                result.NationalityCode = teacherData.NationalityCode;
                result.DateJoin = teacherData.DateJoin;
                result.NationalityID = teacherData.NationalityID;
                result.TotalExperience = teacherData.TotalExperience;
                result.Passport = teacherData.Passport;
                result.HighestQualification = teacherData.HighestQualification;
                result.TechnologyCompetence = teacherData.TechnologyCompetence;
                result.ComminicationSkills = teacherData.ComminicationSkills;
                result.TeachingSkills = teacherData.TeachingSkills;
                result.Subjectknowledge = teacherData.Subjectknowledge;
                result.AdministrativeSkills = teacherData.AdministrativeSkills;
                result.DisciplineSkills = teacherData.DisciplineSkills;
                result.PrimaryBranchCode = teacherData.PrimaryBranchCode;
                result.AboutTeacher = teacherData.AboutTeacher;
                result.ThumbNailImagePath = teacherData.ThumbNailImagePath;
                result.FullImageParh = teacherData.FullImageParh;
                var loginData = await _context.SystemLogins.AsNoTracking().FirstOrDefaultAsync(e => e.Id == teacherData.SysLoginId);
                if (loginData != null)
                    result.Username = loginData.UserName;
            }
            return result;
        }
    }

    #endregion

    #region All_Teacher_Master_Create_And_Update
    public class AllSchoolTeacherMasterData : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public SchoolTeacherMasterDto TeacherMasterDataDto { get; set; }
    }

    public class AllSchoolTeacherMasterDataHandler : IRequestHandler<AllSchoolTeacherMasterData, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AllSchoolTeacherMasterDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AllSchoolTeacherMasterData request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateSchoolTeacherManagement method start----");
            TblDefSchoolTeacherMaster SchoolTeacherMaster = new();
            string teacherCode = string.Empty;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int loginId = 0;
                    var obj = request.TeacherMasterDataDto;
                    TblErpSysLogin tblErpSysLogin = new();
                    tblErpSysLogin = await _context.SystemLogins.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == obj.Username);
                    if (tblErpSysLogin == null && !string.IsNullOrEmpty(obj.Username) && !string.IsNullOrEmpty(obj.Password))
                    {
                        tblErpSysLogin = new();
                        tblErpSysLogin.LoginId = obj.Username;
                        tblErpSysLogin.UserName = obj.Username;
                        tblErpSysLogin.Password = SecurePasswordHasher.EncodePassword(obj.Password);
                        tblErpSysLogin.UserType = "Teacher";
                        tblErpSysLogin.PrimaryBranch = obj.PrimaryBranchCode;
                        tblErpSysLogin.IsActive = true;
                        tblErpSysLogin.IsLoginAllow = true;
                        tblErpSysLogin.LoginType = "Teacher";
                        tblErpSysLogin.ModifiedOn = DateTime.Now;
                        await _context.SystemLogins.AddAsync(tblErpSysLogin);
                        await _context.SaveChangesAsync();
                        loginId = tblErpSysLogin.Id;
                    }
                    else if (obj.Id > 0 && !string.IsNullOrEmpty(obj.Password))
                    {
                        tblErpSysLogin.Password = SecurePasswordHasher.EncodePassword(obj.Password);
                        tblErpSysLogin.ModifiedOn = DateTime.Now;
                        _context.SystemLogins.Update(tblErpSysLogin);
                        await _context.SaveChangesAsync();
                        loginId = tblErpSysLogin.Id;
                    }
                    else if (tblErpSysLogin != null)
                        loginId = tblErpSysLogin.Id;
                    if (obj.Id > 0)
                        SchoolTeacherMaster = await _context.DefSchoolTeacherMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                    if (obj.Id == 0)
                    {
                        int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                        teacherCode = "TEACH" + acadamicYear.ToString().Substring(2, 2) + Convert.ToString(new Random().Next(1, 999)) + Convert.ToString(new Random().Next(1, 9999));
                    }
                    else
                        teacherCode = SchoolTeacherMaster.TeacherCode;
                    SchoolTeacherMaster.Id = obj.Id;
                    SchoolTeacherMaster.TeacherCode = teacherCode;
                    SchoolTeacherMaster.TeacherName1 = obj.TeacherName1;
                    SchoolTeacherMaster.TeacherName2 = obj.TeacherName2;
                    SchoolTeacherMaster.TeacherShortName = obj.TeacherShortName;
                    SchoolTeacherMaster.FatherName = obj.FatherName;
                    SchoolTeacherMaster.MaritalStatus = obj.MaritalStatus;
                    SchoolTeacherMaster.PAddress = obj.PAddress;
                    SchoolTeacherMaster.SpouseName = obj.SpouseName;
                    SchoolTeacherMaster.Pcity = obj.Pcity;
                    SchoolTeacherMaster.PPhone1 = obj.PPhone1;
                    SchoolTeacherMaster.SPhone2 = obj.SPhone2;
                    SchoolTeacherMaster.TeacherEmail = obj.TeacherEmail;
                    SchoolTeacherMaster.PMobile1 = obj.PMobile1;
                    SchoolTeacherMaster.Gender = obj.Gender;
                    SchoolTeacherMaster.HiringType = obj.HiringType;
                    SchoolTeacherMaster.NationalityCode = obj.NationalityCode;
                    SchoolTeacherMaster.DateJoin = obj.DateJoin;
                    SchoolTeacherMaster.NationalityID = obj.NationalityID;
                    SchoolTeacherMaster.TotalExperience = obj.TotalExperience;
                    SchoolTeacherMaster.Passport = obj.Passport;
                    SchoolTeacherMaster.HighestQualification = obj.HighestQualification;
                    SchoolTeacherMaster.TechnologyCompetence = obj.TechnologyCompetence;
                    SchoolTeacherMaster.ComminicationSkills = obj.ComminicationSkills;
                    SchoolTeacherMaster.TeachingSkills = obj.TeachingSkills;
                    SchoolTeacherMaster.Subjectknowledge = obj.Subjectknowledge;
                    SchoolTeacherMaster.AdministrativeSkills = obj.AdministrativeSkills;
                    SchoolTeacherMaster.DisciplineSkills = obj.DisciplineSkills;
                    SchoolTeacherMaster.PrimaryBranchCode = obj.PrimaryBranchCode;
                    SchoolTeacherMaster.AboutTeacher = obj.AboutTeacher;
                    SchoolTeacherMaster.SysLoginId = loginId;
                    if (!string.IsNullOrEmpty(obj.ThumbNailImagePath))
                        SchoolTeacherMaster.ThumbNailImagePath = obj.ThumbNailImagePath;
                    if (!string.IsNullOrEmpty(obj.FullImageParh))
                        SchoolTeacherMaster.FullImageParh = obj.FullImageParh;
                    SchoolTeacherMaster.CreatedOn = DateTime.Now;
                    SchoolTeacherMaster.CreatedBy = Convert.ToString(request.User.UserId);
                    if (obj.Id > 0)
                    {
                        _context.DefSchoolTeacherMaster.Update(SchoolTeacherMaster);
                    }
                    else
                    {
                        await _context.DefSchoolTeacherMaster.AddAsync(SchoolTeacherMaster);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateSchoolTeacherManagement method Exit----");
                    return SchoolTeacherMaster.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateSchoolTeacherManagement Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }



    #endregion

    //#region GetSchoolTeacherById
    //public class GetSchoolTeacherById : IRequest<TblDefSchoolStudentMasterDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }

    //}

    //public class GetSchoolStudentByIdHandler : IRequestHandler<GetSchoolStudentById, TblDefSchoolStudentMasterDto>
    //{
    //    private CINDBOneContext _context;
    //    private IMapper _mapper;

    //    public GetSchoolStudentByIdHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<TblDefSchoolStudentMasterDto> Handle(GetSchoolStudentById request, CancellationToken cancellationToken)
    //    {
    //        var student = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
    //        return student;
    //    }
    //}

    //#endregion

    #region ValidateUsername
    public class ValidateUsername : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Username { get; set; }
    }

    public class ValidateUsernameHandler : IRequestHandler<ValidateUsername, bool>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public ValidateUsernameHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(ValidateUsername request, CancellationToken cancellationToken)
        {
            if (_context.SystemLogins.AsNoTracking().Any(e => e.UserName == request.Username))
                return false;
            else
                return true;
        }
    }

    #endregion

    #region GetTeachersByBranchcode
    public class GetTeachersByBranchcode : IRequest<List<SchoolTeachersListDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }

    }

    public class GetTeachersByBranchcodeHandler : IRequestHandler<GetTeachersByBranchcode, List<SchoolTeachersListDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetTeachersByBranchcodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SchoolTeachersListDto>> Handle(GetTeachersByBranchcode request, CancellationToken cancellationToken)
        {
            return await _context.DefSchoolTeacherMaster.AsNoTracking()
                                  .Where(e => e.PrimaryBranchCode == request.BranchCode)
                                  .Select(x => new SchoolTeachersListDto()
                                  {
                                      TeacherCode = x.TeacherCode,
                                      TeacherName1 = x.TeacherName1,
                                      TeacherName2 = x.TeacherName2
                                  }).ToListAsync();
        }
    }

    #endregion

    #region GetBranchTeacherGrades
    public class GetBranchTeacherGrades : IRequest<List<TblSysSchoolAcedemicClassGradeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetBranchTeacherGradesHandler : IRequestHandler<GetBranchTeacherGrades, List<TblSysSchoolAcedemicClassGradeDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetBranchTeacherGradesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolAcedemicClassGradeDto>> Handle(GetBranchTeacherGrades request, CancellationToken cancellationToken)
        {
            var gradeCodes = await _context.DefSchoolTeacherClassMapping.AsNoTracking()
                                  .Where(e => e.TeacherCode == request.TeacherCode)
                                  .Select(x => x.GradeCode).ToListAsync();
            var classGrades = await _context.SchoolAcedemicClassGrade.AsNoTracking().ProjectTo<TblSysSchoolAcedemicClassGradeDto>(_mapper.ConfigurationProvider)
                               .Where(x => gradeCodes.Contains(x.GradeCode)).ToListAsync();

            return classGrades;
        }
    }

    #endregion

    #region GetTeacherGradeSubjects
    public class GetTeacherGradeSubjects : IRequest<List<TblSysSchoolAcademicsSubectsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }

    }

    public class GetTeacherGradeSubjectsHandler : IRequestHandler<GetTeacherGradeSubjects, List<TblSysSchoolAcademicsSubectsDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetTeacherGradeSubjectsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolAcademicsSubectsDto>> Handle(GetTeacherGradeSubjects request, CancellationToken cancellationToken)
        {
            var subjectCodes = await _context.DefSchoolTeacherSubjectsMapping
                                  .Where(e => e.TeacherCode == request.TeacherCode && e.GradeCode == request.GradeCode)
                                  .Select(x => x.SubjectCode).ToListAsync();
            var academicSubjects = await _context.SysSchoolAcademicsSubects.AsNoTracking().ProjectTo<TblSysSchoolAcademicsSubectsDto>(_mapper.ConfigurationProvider)
                                    .Where(x => subjectCodes.Contains(x.SubCodes)).ToListAsync();

            return academicSubjects;
        }
    }

    #endregion


    #region StudentAttendanceRegisterList
    public class StudentAttendanceRegisterList : IRequest<StudentAttendanceRegisterDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }

    }

    public class StudentAttendanceRegisterListHandler : IRequestHandler<StudentAttendanceRegisterList, StudentAttendanceRegisterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StudentAttendanceRegisterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StudentAttendanceRegisterDto> Handle(StudentAttendanceRegisterList request, CancellationToken cancellationToken)
        {
            var result = new StudentAttendanceRegisterDto();
            try
            {
                result = await _context.StudentAttnRegHeader.AsNoTracking()
                                        .Where(x => x.BranchCode == request.BranchCode
                                        && x.GradeCode == request.GradeCode
                                        && x.SectionCode == request.SectionCode
                                        && x.AttnDate.Year == DateTime.Now.Year
                                        && x.AttnDate.Month == DateTime.Now.Month
                                        && x.AttnDate.Day == DateTime.Now.Day)
                                        .Select(x => new StudentAttendanceRegisterDto()
                                        {
                                            Id = x.Id,
                                            AttnDate = x.AttnDate,
                                            BranchCode = x.BranchCode,
                                            GradeCode = x.GradeCode,
                                            IsOpen = x.IsOpen,
                                            SectionCode = x.SectionCode,
                                            TeacherCode = x.TeacherCode
                                        })
                                        .FirstOrDefaultAsync();
                if (result == null)
                {
                    result = new();
                    result.AttnDate = DateTime.Now;
                    result.BranchCode = request.BranchCode;
                    result.GradeCode = request.GradeCode;
                    result.SectionCode = request.SectionCode;
                    result.IsOpen = true;
                }
                var studentList = await _context.DefSchoolStudentMaster.AsNoTracking()
                                        .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode && x.GradeSectionCode == request.SectionCode)
                                          .ToListAsync();
                if (studentList != null && studentList.Count() > 0)
                {
                    List<StudentAttendanceDataDto> studentsAttnDataList = new();
                    for (int i = 0; i < studentList.Count(); i++)
                    {
                        StudentAttendanceDataDto studentAttendanceDataDto = new();
                        studentAttendanceDataDto.StudentName = studentList[i].StuName;
                        studentAttendanceDataDto.StudentName2 = studentList[i].StuName2;
                        studentAttendanceDataDto.StudentAdmNumber = studentList[i].StuAdmNum;
                        studentAttendanceDataDto.AttnFlag = 'P';
                        studentAttendanceDataDto.IsPresent = true;
                        studentAttendanceDataDto.InTime = DateTime.Now.TimeOfDay;
                        var leaveData = await _context.StudentApplyLeave.AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.StuAdmNum == studentList[i].StuAdmNum
                                       && x.IsApproved == true
                                       && x.LeaveStartDate <= DateTime.Now
                                       && x.LeaveEndDate >= DateTime.Now);
                        if (leaveData != null)
                        {
                            studentAttendanceDataDto.AttnFlag = 'L';
                            studentAttendanceDataDto.IsLeave = true;
                            studentAttendanceDataDto.IsPresent = false;
                        }
                        if (result != null && result.Id > 0)
                        {
                            var stuAttnData = await _context.StudentAttnRegDetails.AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.AttnRegHeaderId == result.Id && x.StudentAdmNumber == studentList[i].StuAdmNum);
                            if (stuAttnData != null)
                            {
                                studentAttendanceDataDto.AttnFlag = stuAttnData.AttnFlag;
                                studentAttendanceDataDto.InTime = stuAttnData.InTime;
                                studentAttendanceDataDto.OutTime = stuAttnData.OutTime;
                                studentAttendanceDataDto.Remarks = stuAttnData.Remarks;
                                studentAttendanceDataDto.IsLeave = stuAttnData.IsLeave;
                                studentAttendanceDataDto.IsPresent = stuAttnData.IsPresent;
                            }
                        }
                        studentsAttnDataList.Add(studentAttendanceDataDto);
                    }
                    result.StudentAttendanceDataList = studentsAttnDataList;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    #endregion

    #region CreateStudentAttendanceRegisterData
    public class CreateStuAttRegisterData : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public StudentAttendanceRegisterDto StuAttRegisterData { get; set; }
    }

    public class CreateStuAttRegisterDataHandler : IRequestHandler<CreateStuAttRegisterData, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateStuAttRegisterDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateStuAttRegisterData request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateStuAttRegisterData method start----");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.StuAttRegisterData;
                    TblStudentAttnRegHeader tblStudentAttnRegHeader = new();
                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader = await _context.StudentAttnRegHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    }
                    else
                    {
                        tblStudentAttnRegHeader.AttnDate = obj.AttnDate;
                        tblStudentAttnRegHeader.IsOpen = true;
                    }
                    tblStudentAttnRegHeader.Id = obj.Id;
                    tblStudentAttnRegHeader.TeacherCode = obj.TeacherCode;
                    tblStudentAttnRegHeader.BranchCode = obj.BranchCode;
                    tblStudentAttnRegHeader.GradeCode = obj.GradeCode;
                    tblStudentAttnRegHeader.SectionCode = obj.SectionCode;

                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader.UpdatedDate = DateTime.Now;
                        tblStudentAttnRegHeader.UpdatedBy = Convert.ToString(request.User.UserId);
                        _context.StudentAttnRegHeader.Update(tblStudentAttnRegHeader);
                    }
                    else
                    {
                        tblStudentAttnRegHeader.CreatedDate = DateTime.Now;
                        tblStudentAttnRegHeader.CreatedBy = Convert.ToString(request.User.UserId);
                        await _context.StudentAttnRegHeader.AddAsync(tblStudentAttnRegHeader);
                    }
                    await _context.SaveChangesAsync();
                    if (tblStudentAttnRegHeader.Id > 0)
                    {
                        foreach (var item in obj.StudentAttendanceDataList)
                        {
                            TblStudentAttnRegDetails tblStudentAttnRegDetails = new();
                            tblStudentAttnRegDetails = await _context.StudentAttnRegDetails.AsNoTracking().FirstOrDefaultAsync(e => e.AttnRegHeaderId == obj.Id && e.StudentAdmNumber == item.StudentAdmNumber);
                            if (tblStudentAttnRegDetails == null)
                            {
                                tblStudentAttnRegDetails = new();
                                tblStudentAttnRegDetails.AttnRegHeaderId = tblStudentAttnRegHeader.Id;
                                tblStudentAttnRegDetails.StudentAdmNumber = item.StudentAdmNumber;
                                tblStudentAttnRegDetails.InTime = DateTime.Now.TimeOfDay;
                            }
                            tblStudentAttnRegDetails.AttnFlag = item.AttnFlag.ToString().ToUpper().ToCharArray()[0];
                            tblStudentAttnRegDetails.Remarks = item.Remarks;
                            tblStudentAttnRegDetails.IsPresent = item.AttnFlag.ToString().ToUpper() == "P" ? true : false;
                            tblStudentAttnRegDetails.IsLeave = item.IsLeave;
                            if (tblStudentAttnRegDetails != null && tblStudentAttnRegDetails.Id > 0)
                                _context.StudentAttnRegDetails.Update(tblStudentAttnRegDetails);
                            else
                                await _context.StudentAttnRegDetails.AddAsync(tblStudentAttnRegDetails);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateStuAttRegisterData method Exit----");
                    return tblStudentAttnRegHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateStuAttRegisterData Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }



    #endregion

    #region CloseAttendance
    public class CloseAttendance : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public StudentAttendanceRegisterDto StuAttRegisterData { get; set; }
    }

    public class CloseAttendanceHandler : IRequestHandler<CloseAttendance, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CloseAttendanceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CloseAttendance request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CloseAttendance method start----");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var CurrentDateTime = DateTime.Now;
                    var obj = request.StuAttRegisterData;
                    TblStudentAttnRegHeader tblStudentAttnRegHeader = new();
                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader = await _context.StudentAttnRegHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        tblStudentAttnRegHeader.IsOpen = false;
                        tblStudentAttnRegHeader.UpdatedDate = CurrentDateTime;
                        tblStudentAttnRegHeader.UpdatedBy = Convert.ToString(request.User.UserId);
                        _context.StudentAttnRegHeader.Update(tblStudentAttnRegHeader);
                        await _context.SaveChangesAsync();
                        int? batchYear = await _context.SysSchoolAcademicBatches.AsNoTracking().OrderBy(x => x.Id).Select(x => x.AcademicYear).FirstOrDefaultAsync();
                        foreach (var item in obj.StudentAttendanceDataList)
                        {
                            TblStudentAttnRegDetails tblStudentAttnRegDetails = new();
                            tblStudentAttnRegDetails = await _context.StudentAttnRegDetails.AsNoTracking().FirstOrDefaultAsync(e => e.AttnRegHeaderId == obj.Id && e.StudentAdmNumber == item.StudentAdmNumber);
                            if (tblStudentAttnRegDetails != null && tblStudentAttnRegDetails.Id > 0)
                            {
                                tblStudentAttnRegDetails.OutTime = CurrentDateTime.TimeOfDay;
                                _context.StudentAttnRegDetails.Update(tblStudentAttnRegDetails);
                                await _context.SaveChangesAsync();
                            }
                            TblDefStudentAttendance tblDefStudentAttendance = new();
                            tblDefStudentAttendance.StuAdmNum = tblStudentAttnRegDetails.StudentAdmNumber;
                            tblDefStudentAttendance.AtnDate = tblStudentAttnRegHeader.AttnDate;
                            tblDefStudentAttendance.AtnTimeIn = DateTime.Now.Date + tblStudentAttnRegDetails.InTime;
                            tblDefStudentAttendance.AtnTimeOut = CurrentDateTime;
                            tblDefStudentAttendance.AtnFlag = Convert.ToString(tblStudentAttnRegDetails.AttnFlag);
                            tblDefStudentAttendance.AcademicYear = batchYear ?? 0;
                            await _context.StudentAttendance.AddAsync(tblDefStudentAttendance);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CloseAttendance method Exit----");
                    return tblStudentAttnRegHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CloseAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }



    #endregion

    #region GetTeachersList
    public class GetTeachersList : IRequest<List<TblDefSchoolTeacherMasterDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetTeachersListHandler : IRequestHandler<GetTeachersList, List<TblDefSchoolTeacherMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeachersListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherMasterDto>> Handle(GetTeachersList request, CancellationToken cancellationToken)
        {
            try
            {

                var list = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider)
                                             .ToListAsync();

                return list;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion

    #region GetTeachersList
    public class IsApprovalLoginTeacher : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
        public int TypeId { get; set; }
    }

    public class IsApprovalLoginTeacherHandler : IRequestHandler<IsApprovalLoginTeacher, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IsApprovalLoginTeacherHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(IsApprovalLoginTeacher request, CancellationToken cancellationToken)
        {
            try
            {
                bool result = false;
                if (!string.IsNullOrEmpty(request.TeacherCode))
                {
                    var teacherDetails = await _context.DefSchoolTeacherMaster.AsNoTracking()
                                            .SingleOrDefaultAsync(x => x.TeacherCode == request.TeacherCode);
                    if (teacherDetails != null)
                    {
                        if (request.TypeId == 1)//leave
                        {
                            var authority = await _context.SysSchoolBranchesAuthority.AsNoTracking()
                                                                        .FirstOrDefaultAsync(x => x.BranchCode == teacherDetails.PrimaryBranchCode && x.TeacherCode == request.TeacherCode && x.IsApproveLeave == true);
                            if (authority != null)
                            {
                                result = true;
                            }
                        }
                        else if (request.TypeId == 2)//Disc
                        {
                            var authority = await _context.SysSchoolBranchesAuthority.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.BranchCode == teacherDetails.PrimaryBranchCode && x.TeacherCode == request.TeacherCode && x.IsApproveDisciPlinaryAction == true);
                            if (authority != null)
                            {
                                result = true;
                            }
                        }
                        else if (request.TypeId == 3)//Notification
                        {
                            var authority = await _context.SysSchoolBranchesAuthority.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.BranchCode == teacherDetails.PrimaryBranchCode && x.TeacherCode == request.TeacherCode && x.IsApproveNotification == true);
                            if (authority != null)
                            {
                                result = true;
                            }
                        }
                        else if (request.TypeId == 4)//Event
                        {
                            var authority = await _context.SysSchoolBranchesAuthority.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.BranchCode == teacherDetails.PrimaryBranchCode && x.TeacherCode == request.TeacherCode && x.IsApproveEvent == true);
                            if (authority != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion


    #region GetModerators
    public class GetModerators : IRequest<List<SysLoginModeratorDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetModeratorsHandler : IRequestHandler<GetModerators, List<SysLoginModeratorDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetModeratorsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SysLoginModeratorDto>> Handle(GetModerators request, CancellationToken cancellationToken)
        {
            return await _context.SystemLogins.AsNoTracking()
                                  .Select(x => new SysLoginModeratorDto()
                                  {
                                      LoginId = x.LoginId,
                                      UserName = x.UserName
                                  }).ToListAsync();
        }
    }

    #endregion


    #region StudentList
    public class StudentList : IRequest<List<TblDefSchoolStudentMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }

    }

    public class StudentListHandler : IRequestHandler<StudentList, List<TblDefSchoolStudentMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StudentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolStudentMasterDto>> Handle(StudentList request, CancellationToken cancellationToken)
        {
            var result = new List<TblDefSchoolStudentMasterDto>();
            try
            {
                result= await _context.DefSchoolStudentMaster.AsNoTracking()
                              .ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider)
                              .Where(x => x.BranchCode==request.BranchCode 
                                    && x.GradeCode==request.GradeCode 
                                    && x.GradeSectionCode==request.SectionCode)
                              .ToListAsync();
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
