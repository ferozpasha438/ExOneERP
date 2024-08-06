using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
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

namespace CIN.Application.TeacherAppMgtQuery
{
    #region Get Result Of Student
    public class StudentExamResult : IRequest<List<MobStudentExamResultDataDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public int AcademicYear { get; set; }
        public string Grade { get; set; }
        public string ExaminationType { get; set; }
        public string AdmissionNumber { get; set; }

    }

    public class StudentExamResultHandler : IRequestHandler<StudentExamResult, List<MobStudentExamResultDataDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StudentExamResultHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<MobStudentExamResultDataDto>> Handle(StudentExamResult request, CancellationToken cancellationToken)
        {
            List<MobStudentExamResultDataDto> studentExamResultListDto = new();
            var acadamicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking()
                                                    .Where(x => x.AcademicYear == request.AcademicYear).
                                                    ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                    FirstOrDefaultAsync();


            var examData = await _context.SchoolExaminationManagementHeader.AsNoTracking()
                                   .Where(x => x.BranchCode == request.BranchCode && (x.StartDate >= acadamicDetails.AcademicStartDate && x.EndDate <= acadamicDetails.AcademicEndDate) && x.TypeOfExaminationCode == request.ExaminationType && x.GradeCode == request.Grade)
                                     .ToListAsync();
            var studentList = await _context.DefSchoolStudentMaster.AsNoTracking().Where(x => x.BranchCode == request.BranchCode && x.AcademicYear == request.AcademicYear).ToListAsync();



            foreach (var examSingleData in examData)
            {
                MobStudentExamResultDataDto mobStudentExamResultDataDto = new();
                if (examSingleData != null && examSingleData.Id > 0)
                {

                    var subjectList = await _context.SysSchoolAcademicsSubects
                                           .Join(_context.SchoolGradeSubjectMapping, sub => sub.SubCodes, map => map.SubCodes, (sub, map) => new { sub.SubCodes, sub.SubName, sub.SubName2, map.MaximumMarks, map.QualifyingMarks, map.GradeCode })
                                           .Join(_context.SchoolExaminationManagementDetails, result => result.SubCodes, details => details.SubjectCode, (result, details) => new { result.SubCodes, result.SubName, result.SubName2, result.MaximumMarks, result.QualifyingMarks, result.GradeCode, details.ExamHeaderId })
                                           .Join(_context.SchoolExaminationManagementHeader, detail => detail.ExamHeaderId, head => head.Id, (detail, head) => new { detail.SubCodes, detail.SubName, detail.SubName2, detail.MaximumMarks, detail.QualifyingMarks, detail.GradeCode, detail.ExamHeaderId, head.TypeOfExaminationCode })
                                           .Where(x => x.GradeCode == examSingleData.GradeCode && x.ExamHeaderId == examSingleData.Id && x.TypeOfExaminationCode == request.ExaminationType)
                                           .Distinct()
                                           .Select(x => new MobStudentResultData()
                                           {

                                               SubCodes = x.SubCodes,
                                               SubjectName = x.SubName,
                                               SubjectName2 = x.SubName2,
                                               MaximumMarks = x.MaximumMarks,
                                               QualifyingMarks = x.QualifyingMarks
                                           }).ToListAsync();

                    var stuResulAdmNum = await _context.SchoolStudentResultHeader.AsNoTracking()
                                         .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade })
                                         .Distinct()
                                .FirstOrDefaultAsync(x => x.GradeCode == examSingleData.GradeCode && x.ExamId == examSingleData.Id && x.StudentAdmNumber == request.AdmissionNumber);

                    foreach (var subData in subjectList)
                    {
                        var stuSubResult = await _context.SchoolStudentResultHeader.AsNoTracking()
                                            .Join(_context.SchoolStudentResultDetails, rh => rh.Id, rd => rd.StudentResultHeaderId, (rh, rd) => new { rh.ExamId, rh.GradeCode, rd.StudentAdmNumber, rd.SubCodes, rd.MarksObtained, rd.QualifiyingGrade })
                                            .Distinct()
                                   .FirstOrDefaultAsync(x => x.GradeCode == examSingleData.GradeCode && x.ExamId == examSingleData.Id && x.StudentAdmNumber == request.AdmissionNumber);
                        if (stuSubResult != null)
                        {
                            //subData.StudentAdmissionNum = stuSubResult.StudentAdmNumber;
                            subData.SubjectMarks = stuSubResult.MarksObtained;
                            subData.QualifyingGrade = stuSubResult.QualifiyingGrade;
                        }

                    }
                    mobStudentExamResultDataDto.GradeCode = stuResulAdmNum.GradeCode;
                    mobStudentExamResultDataDto.StudentAdmission = stuResulAdmNum.StudentAdmNumber;
                    mobStudentExamResultDataDto.ExaminationCode = examSingleData.TypeOfExaminationCode;
                    mobStudentExamResultDataDto.ExamHeaderID = examSingleData.Id;
                    mobStudentExamResultDataDto.StudentResults = subjectList;
                    studentExamResultListDto.Add(mobStudentExamResultDataDto);
                }
            }


            return studentExamResultListDto;

        }




    }

    #endregion


    #region ExamTypeList
    public class GetExamTypeList : IRequest<List<ExamSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetExamTypeListHandler : IRequestHandler<GetExamTypeList, List<ExamSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetExamTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ExamSelectListItem>> Handle(GetExamTypeList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.SchoolExaminationManagementHeader.AsNoTracking().Where(e => e.GradeCode == request.Input)
                               .Select(e => new ExamSelectListItem
                               {
                                   Text = e.TypeOfExaminationCode,
                                   Value = e.Id

                               }).ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion




}
