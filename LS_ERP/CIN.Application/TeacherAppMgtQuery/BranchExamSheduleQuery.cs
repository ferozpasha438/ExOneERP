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
    #region Get All Branch Exam Schedule List

    public class GetExamSheduleOfBranch : IRequest<List<SchoolTeacherExamBranchDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetExamSheduleOfBranchHandler : IRequestHandler<GetExamSheduleOfBranch, List<SchoolTeacherExamBranchDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetExamSheduleOfBranchHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SchoolTeacherExamBranchDto>> Handle(GetExamSheduleOfBranch request,CancellationToken cancellationToken)
        {
            List<SchoolTeacherExamBranchDto> examsList = new();
            var acadamicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                    ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                    OrderByDescending(x => x.AcademicYear).
                                                    FirstOrDefaultAsync();

            if (acadamicDetails != null)
            {
                examsList = await _context.SchoolExaminationManagementHeader.AsNoTracking()
                                        .Where(e => e.BranchCode == request.BranchCode
                                               && e.StartDate >= acadamicDetails.AcademicStartDate && e.EndDate <= acadamicDetails.AcademicEndDate)
                                        .Select(x => new SchoolTeacherExamBranchDto()
                                        {
                                            BranchCode = x.BranchCode,
                                            CreatedBy = x.CreatedBy,
                                            CreatedOn = x.CreatedOn,
                                            DateOfCompletion = x.DateOfCompletion,
                                            DateOfResult = x.DateOfResult,
                                            EndDate = x.EndDate,
                                            GradeCode = x.GradeCode,
                                            Id = x.Id,
                                            IsCompleted = x.IsCompleted,
                                            IsResultDeclared = x.IsResultDeclared,
                                            PreparedBy = x.PreparedBy,
                                            Remarks = x.Remarks,
                                            StartDate = x.StartDate,
                                            TypeOfExaminationCode = x.TypeOfExaminationCode,
                                            UpdatedBy = x.UpdatedBy,
                                            UpdatedOn = x.UpdatedOn
                                        })
                                        .ToListAsync();
            }
            foreach (var item in examsList)
            {
                var examDetails = await _context.SchoolExaminationManagementDetails.AsNoTracking().ProjectTo<SchoolExamMgtDetailsDto>(_mapper.ConfigurationProvider).Where(e=>e.ExamHeaderId== item.Id).ToListAsync();
                item.SchoolExamMgtDetails = examDetails;
            }
            return examsList;


           
        }
    }
    #endregion
}
