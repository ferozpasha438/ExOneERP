using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using CIN.Domain.SchoolMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.ExaminationMgmtQuery
{
    #region GetAllExaminationMgmt
    public class GetAllExaminationMgmtList : IRequest<PaginatedList<TblDefSchoolExamMgtHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllExaminationMgmtListHandler : IRequestHandler<GetAllExaminationMgmtList, PaginatedList<TblDefSchoolExamMgtHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllExaminationMgmtListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefSchoolExamMgtHeaderDto>> Handle(GetAllExaminationMgmtList request, CancellationToken cancellationToken)
        {


            var result = await _context.SchoolExaminationManagementHeader.AsNoTracking().ProjectTo<TblDefSchoolExamMgtHeaderDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return result;
        }


    }
    #endregion

    #region GetById
    public class GetSysSchoolExaminationMgmtById : IRequest<SchoolExamManagementDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetSysSchoolExaminationMgmtByIdHandler : IRequestHandler<GetSysSchoolExaminationMgmtById, SchoolExamManagementDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolExaminationMgmtByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SchoolExamManagementDto> Handle(GetSysSchoolExaminationMgmtById request, CancellationToken cancellationToken)
        {
            SchoolExamManagementDto result = new();
            result = await _context.SchoolExaminationManagementHeader.AsNoTracking()
                             .Select(x => new SchoolExamManagementDto
                             {
                                 BranchCode = x.BranchCode,
                                 DateOfCompletion = x.DateOfCompletion,
                                 DateofResult = x.DateOfResult,
                                 EndingDate = x.EndDate,
                                 GradeCode = x.GradeCode,
                                 Id = x.Id,
                                 IsCompleted = x.IsCompleted,
                                 IsResultDeclared = x.IsResultDeclared,
                                 PreparedBy = x.PreparedBy,
                                 Remarks = x.Remarks,
                                 StartingDate = x.StartDate,
                                 TypeofExamination = x.TypeOfExaminationCode
                             })
                             .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (result != null)
            {
                result.TableRows = await _context.SchoolExaminationManagementDetails.AsNoTracking()
                                       .Where(e => e.ExamHeaderId == result.Id)
                                       .Select(e => new SchoolExamManagementDetailsDto
                                       {
                                           EndingDateTime = e.EndingDateTime,
                                           StartingDateTime = e.StartingDateTime,
                                           SubjectCode = e.SubjectCode
                                       })
                                       .ToListAsync();
            }
            return result;
        }
    }
    #endregion

    #region CreateUpdateExams
    public class CreateUpdateExams : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public SchoolExamManagementDto SchoolExamMgtDto { get; set; }
    }

    public class CreateUpdateExamsHandler : IRequestHandler<CreateUpdateExams, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateExamsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateExams request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateExams method start----");

                    var obj = request.SchoolExamMgtDto;
                    TblDefSchoolExaminationManagementHeader examinationManagementHeader = new();
                    if (obj.Id > 0)
                        examinationManagementHeader = await _context.SchoolExaminationManagementHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    examinationManagementHeader.Id = obj.Id;
                    examinationManagementHeader.BranchCode = obj.BranchCode;
                    examinationManagementHeader.GradeCode = obj.GradeCode;
                    examinationManagementHeader.TypeOfExaminationCode = obj.TypeofExamination;
                    examinationManagementHeader.StartDate = obj.StartingDate;
                    examinationManagementHeader.EndDate = obj.EndingDate;
                    examinationManagementHeader.Remarks = obj.Remarks;
                    examinationManagementHeader.PreparedBy = obj.PreparedBy;
                    examinationManagementHeader.IsCompleted = obj.IsCompleted;
                    examinationManagementHeader.DateOfCompletion = obj.DateOfCompletion;
                    examinationManagementHeader.IsResultDeclared = obj.IsResultDeclared;
                    examinationManagementHeader.DateOfResult = obj.DateofResult;

                    if (obj.Id > 0)
                    {
                        examinationManagementHeader.UpdatedOn = DateTime.Now;
                        examinationManagementHeader.UpdatedBy = request.User.UserId.ToString();
                        _context.SchoolExaminationManagementHeader.Update(examinationManagementHeader);
                    }
                    else
                    {
                        examinationManagementHeader.CreatedOn = DateTime.Now;
                        examinationManagementHeader.CreatedBy = request.User.UserId.ToString();
                        await _context.SchoolExaminationManagementHeader.AddAsync(examinationManagementHeader);
                    }
                    await _context.SaveChangesAsync();

                    if (obj.Id > 0)
                    {
                        var details = await _context.SchoolExaminationManagementDetails.AsNoTracking().Where(e => e.ExamHeaderId == obj.Id).ToListAsync();
                        _context.SchoolExaminationManagementDetails.RemoveRange(details);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var item in obj.TableRows)
                    {
                        TblDefSchoolExaminationManagementDetails examinationManagementDetails = new();
                        examinationManagementDetails.ExamHeaderId = examinationManagementHeader.Id;
                        examinationManagementDetails.SubjectCode = item.SubjectCode;
                        examinationManagementDetails.StartingDateTime = item.StartingDateTime;
                        examinationManagementDetails.EndingDateTime = item.EndingDateTime;
                        await _context.SchoolExaminationManagementDetails.AddAsync(examinationManagementDetails);
                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateExams method Exit----");
                    return examinationManagementHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateExams Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }


    }



    #endregion


    #region GetAllParametersForExamsList
    public class GetAllParametersForExamsList : IRequest<PaginatedList<TblDefSchoolExamMgtHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllParametersForExamsListHandler : IRequestHandler<GetAllExaminationMgmtList, PaginatedList<TblDefSchoolExamMgtHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllParametersForExamsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefSchoolExamMgtHeaderDto>> Handle(GetAllExaminationMgmtList request, CancellationToken cancellationToken)
        {


            var result = await _context.SchoolExaminationManagementHeader.AsNoTracking().ProjectTo<TblDefSchoolExamMgtHeaderDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return result;
        }


    }
    #endregion

    #region GetAllExamTypesList

    public class GetAllExamTypesList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAllExamTypesListHandler : IRequestHandler<GetAllExamTypesList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllExamTypesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAllExamTypesList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAllExamTypesList method start----");
            var item = await _context.SchoolExaminationTypes.AsNoTracking()
                .Where(e => e.IsActive == true)
               .Select(e => new CustomSelectListItem { Text = e.ExaminationTypeName, Value = e.TypeOfExaminationCode, TextTwo = e.ExaminationTypeName2 })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAllExamTypes method Ends----");
            return item;
        }
    }

    #endregion

    #region GetAllUsersList

    public class GetAllUsersList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetAllUsersListHandler : IRequestHandler<GetAllUsersList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllUsersListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAllUsersList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAllUsersList method start----");
            var item = await _context.SystemLogins
                          .Join(_context.DefSchoolTeacherMaster, login => login.Id, teacher => teacher.SysLoginId, (login, teacher) => new { login.Id, login.UserName, login.LoginType, teacher.TeacherCode, teacher.TeacherName2, teacher.TeacherName1, teacher.PrimaryBranchCode })
                          .Join(_context.DefSchoolTeacherClassMapping, result => result.TeacherCode, tgrade => tgrade.TeacherCode, (result, tgrade) => new { result.Id, result.UserName, result.LoginType, result.TeacherCode, result.TeacherName2, result.TeacherName1, result.PrimaryBranchCode, tgrade.GradeCode })
                .AsNoTracking()
                .Where(e => e.LoginType == "Teacher" && e.GradeCode == request.GradeCode && e.PrimaryBranchCode == request.BranchCode)
               .Select(e => new CustomSelectListItem { Text = e.TeacherName1, Value = e.Id.ToString(), TextTwo = e.TeacherName2 })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAllUsersList method Ends----");
            return item;
        }
    }

    #endregion

    //#region GetById
    //public class GetSysSchoolExaminationMgmtById : IRequest<SchoolExamMgtDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }
    //}
    //public class GetSysSchoolExaminationMgmtByIdHandler : IRequestHandler<GetSysSchoolExaminationMgmtById, SchoolExamMgtDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetSysSchoolExaminationMgmtByIdHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<SchoolExamMgtDto> Handle(GetSysSchoolExaminationMgmtById request, CancellationToken cancellationToken)
    //    {
    //        SchoolExamMgtDto result = new();
    //        result = await _context.SchoolExaminationManagementHeader.AsNoTracking().ProjectTo<SchoolExamMgtDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
    //        if (result != null)
    //        {
    //            var examDetails = await _context.SchoolExaminationManagementDetails.AsNoTracking().ProjectTo<TblDefSchoolExamMgtDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.ExamHeaderId == result.Id).ToListAsync();
    //            result.SchoolExamMgtDetails = examDetails;
    //        }
    //        return result;
    //    }
    //}
    //#endregion

    //#region CreateUpdateExams
    //public class CreateUpdateExams : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public SchoolExamMgtDto SchoolExamMgtDto { get; set; }
    //}

    //public class CreateUpdateExamsHandler : IRequestHandler<CreateUpdateExams, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateUpdateExamsHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateUpdateExams request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info CreateUpdateExams method start----");

    //                var obj = request.SchoolExamMgtDto;
    //                TblDefSchoolExaminationManagementHeader examinationManagementHeader = new();
    //                if (obj.Id > 0)
    //                    examinationManagementHeader = await _context.SchoolExaminationManagementHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
    //                examinationManagementHeader.Id = obj.Id;
    //                examinationManagementHeader.BranchCode = obj.BranchCode;
    //                examinationManagementHeader.GradeCode = obj.GradeCode;
    //                examinationManagementHeader.TypeOfExaminationCode = obj.TypeOfExaminationCode;
    //                examinationManagementHeader.StartDate = obj.StartDate;
    //                examinationManagementHeader.EndDate = obj.EndDate;
    //                examinationManagementHeader.Remarks = obj.Remarks;
    //                examinationManagementHeader.PreparedBy = obj.PreparedBy;
    //                examinationManagementHeader.IsCompleted = obj.IsCompleted;
    //                examinationManagementHeader.DateOfCompletion = obj.DateOfCompletion;
    //                examinationManagementHeader.IsResultDeclared = obj.IsResultDeclared;
    //                examinationManagementHeader.DateOfResult = obj.DateOfResult;

    //                if (obj.Id > 0)
    //                {
    //                    examinationManagementHeader.UpdatedOn = DateTime.Now;
    //                    examinationManagementHeader.UpdatedBy = obj.CreatedBy;
    //                    _context.SchoolExaminationManagementHeader.Update(examinationManagementHeader);
    //                }
    //                else
    //                {
    //                    examinationManagementHeader.CreatedOn = DateTime.Now;
    //                    examinationManagementHeader.CreatedBy = obj.CreatedBy;
    //                    await _context.SchoolExaminationManagementHeader.AddAsync(examinationManagementHeader);
    //                }
    //                await _context.SaveChangesAsync();

    //                if (obj.Id > 0)
    //                {
    //                    var details = await _context.SchoolExaminationManagementDetails.AsNoTracking().Where(e => e.ExamHeaderId == obj.Id).ToListAsync();
    //                    _context.SchoolExaminationManagementDetails.RemoveRange(details);
    //                    await _context.SaveChangesAsync();
    //                }
    //                foreach (var item in obj.SchoolExamMgtDetails)
    //                {
    //                    TblDefSchoolExaminationManagementDetails examinationManagementDetails = new();
    //                    examinationManagementDetails.ExamHeaderId = obj.Id;
    //                    examinationManagementDetails.SubjectCode = item.SubjectCode;
    //                    examinationManagementDetails.StartingDateTime = item.StartingDateTime;
    //                    examinationManagementDetails.EndingDateTime = item.EndingDateTime;
    //                    await _context.SchoolExaminationManagementDetails.AddAsync(examinationManagementDetails);
    //                    await _context.SaveChangesAsync();
    //                }
    //                await transaction.CommitAsync();
    //                Log.Info("----Info CreateUpdateExams method Exit----");
    //                return examinationManagementHeader.Id;
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in CreateUpdateExams Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return 0;
    //            }
    //        }

    //    }


    //}



    //#endregion


    #region ParametersForExamsList
    public class ParametersForExamsList : IRequest<PaginatedList<ParametersForAcadamicGradeListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class ParametersForExamsListHandler : IRequestHandler<ParametersForExamsList, PaginatedList<ParametersForAcadamicGradeListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ParametersForExamsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ParametersForAcadamicGradeListDto>> Handle(ParametersForExamsList request, CancellationToken cancellationToken)
        {


            var result = await _context.SchoolAcedemicClassGrade.AsNoTracking()
                                 .Select(x => new ParametersForAcadamicGradeListDto
                                 {
                                     GradeCode = x.GradeCode,
                                     GradeName = x.GradeName,
                                     GradeName2 = x.GradeName2,
                                     IsGradeRequired = Convert.ToBoolean(x.IsGradeRequired),
                                     NoOfGrades = Convert.ToInt32(x.NoOfGrades)
                                 })
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return result;
        }


    }
    #endregion
    #region CreateParametersForExams
    public class CreateParametersForExams : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ParametersForExamsDto Dto { get; set; }
    }

    public class CreateParametersForExamsHandler : IRequestHandler<CreateParametersForExams, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateParametersForExamsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateParametersForExams request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateParametersForExams method start----");
                    var obj = request.Dto;
                    TblSysSchoolAcedemicClassGrade acedemicClassGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(e => e.GradeCode == obj.GradeCode);
                    acedemicClassGrade.IsGradeRequired = obj.IsGradeRequired;
                    acedemicClassGrade.NoOfGrades = obj.NoOfGrades;
                    _context.SchoolAcedemicClassGrade.Update(acedemicClassGrade);
                    await _context.SaveChangesAsync();
                    var list = await _context.SchoolSubjectExamsGrade.AsNoTracking().Where(e => e.GradeCode == obj.GradeCode).ToListAsync();
                    if (list.Count > 0)
                    {
                        _context.SchoolSubjectExamsGrade.RemoveRange(list);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var tableRow in obj.TableRows)
                    {
                        var subCodeData = await _context.SchoolGradeSubjectMapping.AsNoTracking().Where(e => e.GradeCode == obj.GradeCode && e.SubCodes == tableRow.SubCodes).ToListAsync();
                        foreach (var item in subCodeData)
                        {
                            item.MaximumMarks = Convert.ToDecimal(tableRow.MaximumMarks);
                            item.QualifyingMarks = Convert.ToDecimal(tableRow.QualifyingMarks);
                        }
                        if (subCodeData.Count > 0)
                        {
                            _context.SchoolGradeSubjectMapping.UpdateRange(subCodeData);
                            await _context.SaveChangesAsync();
                        }
                        foreach (var configRow in tableRow.ConfigRows)
                        {
                            TblDefSchoolSubjectExamsGrade subjectExamsGrade = new();
                            subjectExamsGrade.GradeCode = obj.GradeCode;
                            subjectExamsGrade.SubjectCode = tableRow.SubCodes;
                            subjectExamsGrade.MinimumMarks = Convert.ToDecimal(configRow.MinimumMarks);
                            subjectExamsGrade.MaximumMarks = Convert.ToDecimal(configRow.MaximumMarks);
                            subjectExamsGrade.QualifiyingGrade = configRow.QualifiyingGrade;
                            await _context.SchoolSubjectExamsGrade.AddAsync(subjectExamsGrade);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateParametersForExams method Exit----");
                    return acedemicClassGrade.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateParametersForExams Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }


    }



    #endregion

    #region LoadResultList
    public class LoadResultList : IRequest<StudentExamResultListDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string ExaminationTypeCode { get; set; }
    }

    public class LoadResultListHandler : IRequestHandler<LoadResultList, StudentExamResultListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public LoadResultListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StudentExamResultListDto> Handle(LoadResultList request, CancellationToken cancellationToken)
        {
            StudentExamResultListDto studentExamResultListDto = new();
            var examData = await _context.SchoolExaminationManagementHeader.AsNoTracking().OrderByDescending(x => x.Id)
                                   .FirstOrDefaultAsync(x => x.TypeOfExaminationCode == request.ExaminationTypeCode && x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode && x.IsCompleted == true);
            if (examData != null && examData.Id > 0)
            {
                var studentResults = await _context.DefSchoolStudentMaster.AsNoTracking()
                                       .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode)
                                       .Select(x => new StudentExamResultDataDto()
                                       {
                                           StuAdmCode = x.StuAdmNum,
                                           StudentName = x.StuName,
                                           StudentName2 = x.StuName2
                                       }).ToListAsync();
                IEnumerable<StudentResultData> subjectList = await _context.SysSchoolAcademicsSubects
                                       .Join(_context.SchoolGradeSubjectMapping, sub => sub.SubCodes, map => map.SubCodes, (sub, map) => new { sub.SubCodes, sub.SubName, sub.SubName2, map.MaximumMarks, map.QualifyingMarks, map.GradeCode })
                                       .Join(_context.SchoolExaminationManagementDetails, result => result.SubCodes, details => details.SubjectCode, (result, details) => new { result.SubCodes, result.SubName, result.SubName2, result.MaximumMarks, result.QualifyingMarks, result.GradeCode, details.ExamHeaderId })
                                       .Where(x => x.GradeCode == request.GradeCode && x.ExamHeaderId == examData.Id)
                                       .Distinct()
                                       .Select(x => new StudentResultData()
                                       {
                                           SubCodes = x.SubCodes,
                                           SubjectName = x.SubName,
                                           SubjectName2 = x.SubName2,
                                           MaximumMarks = x.MaximumMarks,
                                           QualifyingMarks = x.QualifyingMarks,
                                           SubjectMarks = 0
                                       }).ToListAsync();

                studentExamResultListDto.ExamHeaderID = examData.Id;
                studentExamResultListDto.GradeCode = request.GradeCode;
                studentExamResultListDto.BranchCode = request.BranchCode;
                var examDetails = await _context.SchoolExaminationManagementDetails.AsNoTracking()
                                   .Where(x => x.ExamHeaderId == examData.Id)
                                   .ToListAsync();
                string remarks = string.Empty;
                var studentResultsData = new List<StudentResultData>();
                var studentExamResultData = new List<StudentExamResultDataDto>();
                for (int i = 0; i < studentResults.Count(); i++)
                {
                    remarks = string.Empty;
                    studentResultsData = subjectList.Select(x => new StudentResultData() { MaximumMarks = x.MaximumMarks, QualifyingGrade = x.QualifyingGrade, QualifyingMarks = x.QualifyingMarks, SubCodes = x.SubCodes, SubjectMarks = x.SubjectMarks, SubjectName = x.SubjectName, SubjectName2 = x.SubjectName2 }).ToList();
                    for (int j = 0; j < subjectList.Count(); j++)
                    {
                        var stuSubResult = await _context.SchoolStudentResultHeader.AsNoTracking()
                                          .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade, rd.Remarks })
                                 .FirstOrDefaultAsync(x => x.ExamId == examData.Id && x.GradeCode == request.GradeCode && x.StudentAdmNumber == studentResults[i].StuAdmCode && x.SubCodes == studentResultsData[j].SubCodes);
                        if (stuSubResult != null)
                        {
                            remarks = stuSubResult.Remarks;
                            studentResultsData[j].SubjectMarks = stuSubResult.MarksObtained;
                            studentResultsData[j].QualifyingGrade = stuSubResult.QualifiyingGrade;
                        }
                    }
                    var listitem = new StudentExamResultDataDto()
                    {
                        StuAdmCode = studentResults[i].StuAdmCode,
                        StudentName = studentResults[i].StudentName,
                        StudentName2 = studentResults[i].StudentName2,
                        Remarks = remarks,
                        TotalMarks = studentResultsData.Count() > 0 ? (studentResultsData.Sum(x => x.SubjectMarks)) : 0,
                        StudentResults = studentResultsData
                    };
                    studentExamResultData.Add(listitem);
                }
                studentExamResultListDto.StudentExamResultData = studentExamResultData;
                //foreach (var item in studentResults)
                //{
                //    remarks = string.Empty;
                //    foreach (var subData in subjectList)
                //    {
                //        var stuSubResult = await _context.SchoolStudentResultHeader.AsNoTracking()
                //                            .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade,rd.Remarks })
                //                   .FirstOrDefaultAsync(x => x.ExamId == examData.Id && x.GradeCode == request.GradeCode && x.StudentAdmNumber == item.StuAdmCode);
                //        if (stuSubResult != null)
                //        {
                //            remarks = stuSubResult.Remarks;
                //            subData.SubjectMarks = stuSubResult.MarksObtained;
                //            subData.QualifyingGrade = stuSubResult.QualifiyingGrade;
                //        }
                //    }
                //    var listitem = new StudentExamResultDataDto()
                //    {
                //        StuAdmCode = item.StuAdmCode,
                //        StudentName = item.StudentName,
                //        StudentName2 = item.StudentName2,
                //        Remarks= remarks,
                //        TotalMarks= subjectList.Count()>0?(subjectList.Sum(x=>x.SubjectMarks)) :0,
                //        StudentResults = subjectList.ToList()
                //    };
                //    studentExamResultListDto.StudentExamResultData.Add(listitem);
                //}
            }
            return studentExamResultListDto;
        }


    }
    #endregion

    #region CreateStudentExamResults
    public class CreateStudentExamResults : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public StudentExamResultListDto RequestDto { get; set; }
    }

    public class CreateStudentExamResultsHandler : IRequestHandler<CreateStudentExamResults, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateStudentExamResultsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateStudentExamResults request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateStudentExamResults method start----");
                    var obj = request.RequestDto;
                    TblDefSchoolStudentResultHeader studentResultHeader = new();
                    var studentResultData = await _context.SchoolStudentResultHeader.AsNoTracking().FirstOrDefaultAsync(e => e.ExamId == obj.ExamHeaderID);
                    var examHeaderData = await _context.SchoolExaminationManagementHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.ExamHeaderID);
                    if (examHeaderData != null)
                    {
                        if (studentResultData != null)
                            studentResultHeader.Id = studentResultData.Id;
                        else
                            studentResultHeader.Id = 0;
                        studentResultHeader.ExamId = obj.ExamHeaderID;
                        studentResultHeader.GradeCode = obj.GradeCode;
                        studentResultHeader.ReleasedBy = Convert.ToString(request.User.UserId);
                        studentResultHeader.ResultDate = DateTime.Now;
                        if (studentResultHeader.Id > 0)
                        {
                            studentResultHeader.UpdatedOn = DateTime.Now;
                            studentResultHeader.UpdatedBy = Convert.ToString(request.User.UserId);
                            _context.SchoolStudentResultHeader.Update(studentResultHeader);
                        }
                        else
                        {
                            studentResultHeader.CreatedBy = Convert.ToString(request.User.UserId);
                            studentResultHeader.CreatedDate = DateTime.Now;
                            await _context.SchoolStudentResultHeader.AddAsync(studentResultHeader);
                        }
                        await _context.SaveChangesAsync();

                        if (studentResultHeader.Id > 0)
                        {
                            var details = await _context.SchoolStudentResultDetails.AsNoTracking().Where(e => e.StudentResultHeaderId == studentResultHeader.Id).ToListAsync();
                            if (details.Count() > 0)
                            {
                                _context.SchoolStudentResultDetails.RemoveRange(details);
                                await _context.SaveChangesAsync();
                            }
                        }
                        foreach (var results in obj.StudentExamResultData)
                        {
                            foreach (var item in results.StudentResults)
                            {
                                TblDefSchoolStudentResultDetails studentResultDetails = new();
                                studentResultDetails.StudentResultHeaderId = studentResultHeader.Id;
                                studentResultDetails.StudentAdmNumber = results.StuAdmCode;
                                studentResultDetails.SubCodes = item.SubCodes;
                                studentResultDetails.Remarks = results.Remarks;
                                studentResultDetails.MaximumMarks = Convert.ToDecimal(item.MaximumMarks);
                                studentResultDetails.QualifiyingMarks = Convert.ToDecimal(item.QualifyingMarks);
                                studentResultDetails.MarksObtained = Convert.ToDecimal(item.SubjectMarks);
                                var qualifyingGradeData = await _context.SchoolSubjectExamsGrade.AsNoTracking()
                                                    .FirstOrDefaultAsync(e => e.GradeCode == request.RequestDto.GradeCode && e.SubjectCode == item.SubCodes && e.MaximumMarks >= item.SubjectMarks && e.MinimumMarks <= item.SubjectMarks);
                                if (qualifyingGradeData != null)
                                {
                                    studentResultDetails.QualifiyingGrade = qualifyingGradeData.QualifiyingGrade;
                                }
                                await _context.SchoolStudentResultDetails.AddAsync(studentResultDetails);
                                await _context.SaveChangesAsync();
                            }
                        }
                        if (!examHeaderData.IsResultDeclared)
                        {
                            examHeaderData.IsResultDeclared = true;
                            examHeaderData.DateOfResult = DateTime.Now;
                            examHeaderData.UpdatedBy = Convert.ToString(request.User.UserId);
                            examHeaderData.UpdatedOn = DateTime.Now;
                            _context.SchoolExaminationManagementHeader.Update(examHeaderData);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateStudentExamResults method Exit----");
                    return studentResultHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateStudentExamResults Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }
    }
    #endregion

    #region GetParametersForExamsById
    public class GetParametersForExamsById : IRequest<ParametersForExamsDto>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
    }
    public class GetParametersForExamsByIdHandler : IRequestHandler<GetParametersForExamsById, ParametersForExamsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetParametersForExamsByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParametersForExamsDto> Handle(GetParametersForExamsById request, CancellationToken cancellationToken)
        {
            ParametersForExamsDto result = new();
            var gradeData = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(e => e.GradeCode == request.GradeCode);
            if (gradeData != null)
            {
                result.IsGradeRequired = gradeData.IsGradeRequired ?? false;
                result.NoOfGrades = gradeData.NoOfGrades ?? 0;
                var subjectsConfig= await _context.SchoolGradeSubjectMapping.AsNoTracking()
                                    .Where(e => e.GradeCode == request.GradeCode)
                                    .Select(x=>new SubjectGradesConfig {
                                        SubCodes=x.SubCodes, 
                                        QualifyingMarks= x.QualifyingMarks==null?string.Empty:x.QualifyingMarks.ToString(),
                                        MaximumMarks= x.MaximumMarks == null ? string.Empty : x.MaximumMarks.ToString()
                                    })
                                    .Distinct()
                                    .ToListAsync();
                foreach (var item in subjectsConfig)
                {
                    SubjectGradesConfig subjectGradesConfig = new();
                    subjectGradesConfig.SubCodes = item.SubCodes;
                    subjectGradesConfig.QualifyingMarks = item.QualifyingMarks;
                    subjectGradesConfig.MaximumMarks = item.MaximumMarks;
                    var gradesConfig = await _context.SchoolSubjectExamsGrade.AsNoTracking()
                                    .Where(e => e.GradeCode == request.GradeCode
                                    && e.SubjectCode==item.SubCodes)
                                    .ToListAsync();
                    foreach (var gradeConfig in gradesConfig)
                    {
                        GradeConfig gradeConfigutaion = new();
                        gradeConfigutaion.MaximumMarks = gradeConfig.MaximumMarks.ToString();
                        gradeConfigutaion.MinimumMarks = gradeConfig.MinimumMarks.ToString();
                        gradeConfigutaion.QualifiyingGrade = gradeConfig.QualifiyingGrade.ToString();
                        subjectGradesConfig.ConfigRows.Add(gradeConfigutaion);
                    }
                    result.TableRows.Add(subjectGradesConfig);
                }
            }
            return result;
        }
    }
    #endregion

}
