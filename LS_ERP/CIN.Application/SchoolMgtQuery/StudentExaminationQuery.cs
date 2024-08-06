using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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

namespace CIN.Application.SchoolMgtQuery
{
    #region GetExaminationList by Grade
    public class GetStudentExaminationList : IRequest<List<SchoolExamMgtDto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
        public string BranchCode { get; set; }
       
    }
    public class GetSysSchoolExaminationListHandler : IRequestHandler<GetStudentExaminationList, List<SchoolExamMgtDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolExaminationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SchoolExamMgtDto>> Handle(GetStudentExaminationList request, CancellationToken cancellationToken)
        {
            List<SchoolExamMgtDto> examsList = new();
            var acadamicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                    ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                    OrderByDescending(x => x.AcademicYear).
                                                    FirstOrDefaultAsync();
            if (acadamicDetails != null)
            {
                examsList = await _context.SchoolExaminationManagementHeader.AsNoTracking()
                                        .Where(e => e.GradeCode == request.GradeCode && e.BranchCode == request.BranchCode 
                                               && e.StartDate >= acadamicDetails.AcademicStartDate && e.EndDate <= acadamicDetails.AcademicEndDate)
                                        .Select(x=>new SchoolExamMgtDto() 
                                        { 
                                            BranchCode=x.BranchCode,
                                            CreatedBy=x.CreatedBy,
                                            CreatedOn=x.CreatedOn,
                                            DateOfCompletion=x.DateOfCompletion,
                                            DateOfResult=x.DateOfResult,
                                            EndDate=x.EndDate,
                                            GradeCode=x.GradeCode,
                                            Id=x.Id,
                                            IsCompleted=x.IsCompleted,
                                            IsResultDeclared = x.IsResultDeclared,
                                            PreparedBy=x.PreparedBy,
                                            Remarks=x.Remarks,
                                            StartDate=x.StartDate,
                                            TypeOfExaminationCode=x.TypeOfExaminationCode,
                                            UpdatedBy=x.UpdatedBy,
                                            UpdatedOn=x.UpdatedOn
                                        } )
                                        .ToListAsync();
            }
            foreach (var item in examsList)
            {
                var examDetails = await _context.SchoolExaminationManagementDetails.AsNoTracking().ProjectTo<TblDefSchoolExamMgtDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.ExamHeaderId == item.Id).ToListAsync();
                item.SchoolExamMgtDetails = examDetails;
            }
            return examsList;
        }
    }
    #endregion

    #region Get Result Of Student
    public class StudentExamResult : IRequest<MobStudentExamResultListDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string StuAdmNum { get; set; }
    }

    public class StudentExamResultHandler : IRequestHandler<StudentExamResult, MobStudentExamResultListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StudentExamResultHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MobStudentExamResultListDto> Handle(StudentExamResult request, CancellationToken cancellationToken)
        {
            int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();

            var acadamicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                 ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                 OrderByDescending(x => x.AcademicYear).
                                                 FirstOrDefaultAsync();


            MobStudentExamResultListDto studentExamResultListDto = new();
            var examData = await _context.SchoolExaminationManagementHeader.AsNoTracking()
                                   .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode)
                                   .ToListAsync();
            studentExamResultListDto = await _context.DefSchoolStudentMaster.AsNoTracking()
                                           .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode && x.StuAdmNum == request.StuAdmNum)
                                           .Select(x => new MobStudentExamResultListDto()
                                           {
                                               StuAdmCode = x.StuAdmNum,
                                               StudentName = x.StuName,
                                               StudentName2 = x.StuName2
                                           }).FirstOrDefaultAsync();
            studentExamResultListDto.GradeCode = request.GradeCode;
            studentExamResultListDto.BranchCode = request.BranchCode;
            List<MobStudentExamResultDataDto> studentExamResultData = new();
            foreach (var examSingleData in examData)
            {
                MobStudentExamResultDataDto mobStudentExamResultDataDto = new();
                if (examSingleData != null && examSingleData.Id > 0)
                {
                    
                    var subjectList = await _context.SysSchoolAcademicsSubects
                                           .Join(_context.SchoolGradeSubjectMapping, sub => sub.SubCodes, map => map.SubCodes, (sub, map) => new { sub.SubCodes, sub.SubName, sub.SubName2, map.MaximumMarks, map.QualifyingMarks, map.GradeCode })
                                           .Join(_context.SchoolExaminationManagementDetails, result => result.SubCodes, details => details.SubjectCode, (result, details) => new { result.SubCodes, result.SubName, result.SubName2, result.MaximumMarks, result.QualifyingMarks, result.GradeCode, details.ExamHeaderId })
                                           .Where(x => x.GradeCode == request.GradeCode && x.ExamHeaderId == examSingleData.Id)
                                           .Distinct()
                                           .Select(x => new MobStudentResultData()
                                           {
                                               SubCodes = x.SubCodes,
                                               SubjectName = x.SubName,
                                               SubjectName2 = x.SubName2,
                                               MaximumMarks = x.MaximumMarks,
                                               QualifyingMarks = x.QualifyingMarks
                                           }).ToListAsync();
                    
                    foreach (var subData in subjectList)
                    {
                        var stuSubResult = await _context.SchoolStudentResultHeader.AsNoTracking()
                                            .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade })
                                   .FirstOrDefaultAsync(x => x.ExamId == examSingleData.Id && x.GradeCode == request.GradeCode && x.StudentAdmNumber == request.StuAdmNum);
                        if (stuSubResult != null)
                        {
                            subData.SubjectMarks = stuSubResult.MarksObtained;
                            subData.QualifyingGrade = stuSubResult.QualifiyingGrade;
                        }
                    }
                    mobStudentExamResultDataDto.ExamHeaderID = examSingleData.Id;
                    mobStudentExamResultDataDto.StudentResults = subjectList;
                    studentExamResultData.Add(mobStudentExamResultDataDto);
                }
            }
            studentExamResultListDto.StudentExamResultData = studentExamResultData;

            studentExamResultListDto.AttnPercentage = 93.50M;
            studentExamResultListDto.Totaldays = 93;
            studentExamResultListDto.TotalPresent = 66;
            studentExamResultListDto.TotalLeave = 6;
            studentExamResultListDto.TotalHoliday = 15;
            studentExamResultListDto.TotalAbsent = 6;
            studentExamResultListDto.Remarks = "Excellent Performance";

            //DateTime FromDate = acadamicDetails.AcademicStartDate;
            //DateTime ToDate = DateTime.Now;
            //TimeSpan differenceDates = ToDate - FromDate;
            //studentExamResultListDto.Totaldays = differenceDates;
            //studentExamResultListDto.TotalPresent = await _context.StudentAttendance.AsNoTracking().Where(e => e.StuAdmNum == request.StuAdmNum && e.AcademicYear == acadamicYear).CountAsync();
            //studentExamResultListDto.TotalLeave = await _context.StudentApplyLeave.AsNoTracking().Where(e => e.StuAdmNum == request.StuAdmNum && e.AcademicYear == acadamicYear).CountAsync();
            //studentExamResultListDto.TotalHoliday = await _context.StudentHolidayClaender.AsNoTracking().CountAsync();





            //DateTime date1 = acadamicDetails.AcademicStartDate;
            //DayOfWeek dw = date1.DayOfWeek;
            ////   int cw=date1.DayOfWe
            //// string dateToday = date.ToString("d");
            //DayOfWeek day = DateTime.Now.DayOfWeek;
            //string dayToday = day.ToString();
            //int num = DayOfWeek.Saturday.ToString().Count();
            ////   DayOfWeek weekend = date1.DayOfWeek.Friday.ToString();

            //DateTime startDate = acadamicDetails.AcademicStartDate;
            //DateTime endDate = DateTime.Now;

            //int daycount = 0;
            //while (startDate < endDate)
            //{
            //    startDate = startDate.AddDays(1);
            //    int DayNumInWeek = (int)startDate.DayOfWeek;
            //    if (DayNumInWeek != 0 && DayNumInWeek != 6)
            //        daycount += 1;
            //}

            //var totalweekend = daycount;

            // compare enums
            //if ((day == DayOfWeek.Monday) || (day == DayOfWeek.Sunday))
            //{
            //    Console.WriteLine(dateToday + " is a weekend");
            //}
            //else
            //{
            //    Console.WriteLine(dateToday + " is not a weekend");
            //}

            //// compare strings
            //if ((dayToday == DayOfWeek.Saturday.ToString().Count()) || (dayToday == DayOfWeek.Sunday.ToString()))
            //{
            //    Console.WriteLine(dateToday + " is a weekend");
            //}
            //else
            //{
            //    Console.WriteLine(dateToday + " is not a weekend");
            //}
            return studentExamResultListDto;
            
        }


        
        //    public async Task<StudentExamResultListDto> Handle(StudentExamResult request, CancellationToken cancellationToken)
        //    {
        //        StudentExamResultListDto studentExamResultListDto = new();
        //        var examData = await _context.SchoolExaminationManagementHeader.AsNoTracking().OrderByDescending(x => x.Id)
        //                               .FirstOrDefaultAsync(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode);
        //        if (examData != null && examData.Id > 0)
        //        {
        //            var studentResults = await _context.DefSchoolStudentMaster.AsNoTracking()
        //                                   .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode)
        //                                   .Select(x => new StudentExamResultDataDto()
        //                                   {
        //                                       StuAdmCode = x.StuAdmNum,
        //                                       StudentName = x.StuName,
        //                                       StudentName2 = x.StuName2
        //                                   }).ToListAsync();
        //            var subjectList = await _context.SysSchoolAcademicsSubects
        //                                   .Join(_context.SchoolGradeSubjectMapping, sub => sub.SubCodes, map => map.SubCodes, (sub, map) => new { sub.SubCodes, sub.SubName, sub.SubName2, map.MaximumMarks, map.QualifyingMarks, map.GradeCode })
        //                                   .Join(_context.SchoolExaminationManagementDetails, result => result.SubCodes, details => details.SubjectCode, (result, details) => new { result.SubCodes, result.SubName, result.SubName2, result.MaximumMarks, result.QualifyingMarks, result.GradeCode, details.ExamHeaderId })
        //                                   .Where(x => x.GradeCode == request.GradeCode && x.ExamHeaderId == examData.Id)
        //                                   .Select(x => new StudentResultData()
        //                                   {
        //                                       SubCodes = x.SubCodes,
        //                                       SubjectName = x.SubName,
        //                                       SubjectName2 = x.SubName2,
        //                                       MaximumMarks = x.MaximumMarks,
        //                                       QualifyingMarks = x.QualifyingMarks
        //                                   }).ToListAsync();

        //            studentExamResultListDto.ExamHeaderID = examData.Id;
        //            studentExamResultListDto.GradeCode = request.GradeCode;
        //            var examDetails = await _context.SchoolExaminationManagementDetails.AsNoTracking()
        //                               .Where(x => x.ExamHeaderId == examData.Id)
        //                               .ToListAsync();
        //            foreach (var item in studentResults)
        //            {
        //                foreach (var subData in subjectList)
        //                {
        //                    var stuSubResult = await _context.SchoolStudentResultHeader.AsNoTracking()
        //                                        .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade })
        //                               .FirstOrDefaultAsync(x => x.ExamId == examData.Id && x.GradeCode == request.GradeCode && x.StudentAdmNumber == request.StuAdmNum);
        //                    if (stuSubResult != null)
        //                    {
        //                        subData.SubjectMarks = stuSubResult.MarksObtained;
        //                        subData.QualifyingGrade = stuSubResult.QualifiyingGrade;
        //                    }
        //                }
        //                var listitem = new StudentExamResultDataDto()
        //                {
        //                    StuAdmCode = item.StuAdmCode,
        //                    StudentName = item.StudentName,
        //                    StudentName2 = item.StudentName2,
        //                    StudentResults = subjectList
        //                };
        //                studentExamResultListDto.StudentExamResultData.Add(listitem);
        //            }
        //        }
        //        return studentExamResultListDto;
        //    }
    }

    #endregion
}
