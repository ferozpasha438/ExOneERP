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

namespace CIN.Application.SchoolMgtQuery
{
    #region Get Lesson Plan Details List

    public class GetLessonPlanDetailList : IRequest<List<TblLessonPlanDetailsDto>>
    {
        public UserIdentityDto User { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Grade { get; set; }

        public string Section { get; set; }

        public string Branch { get; set; }
    }

    public class GetLessonPlanDetailListHandler : IRequestHandler<GetLessonPlanDetailList, List<TblLessonPlanDetailsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetLessonPlanDetailListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<TblLessonPlanDetailsDto>> Handle(GetLessonPlanDetailList requset, CancellationToken cancellationToken)
        {
            var lessonPlanDetaillist = await _context.LessonPlanDetails.AsNoTracking()
                                        .ProjectTo<TblLessonPlanDetailsDto>(_mapper.ConfigurationProvider)
                                   .Where(e => ((e.EstStartDate >= requset.FromDate
                                             && e.EstStartDate <= requset.ToDate)
                                              && e.GradeCode == requset.Grade
                                              && e.SectionCode == requset.Section))
                                   .OrderBy(e => e.EstStartDate).ToListAsync();

            if (lessonPlanDetaillist.Count > 0)
            {
                lessonPlanDetaillist = lessonPlanDetaillist.Select(x => new TblLessonPlanDetailsDto
                {
                    ActEndDate = x.ActEndDate,
                    ActHrs = x.ActHrs,
                    ActStartDate = x.ActStartDate,
                    ActualTecherCode = x.ActualTecherCode,
                    AssignTeacherCode = x.AssignTeacherCode,
                    Chapter = x.Chapter,
                    EndTime = x.EndTime,
                    EstDays = x.EstDays,
                    EstHrs = x.EstHrs,
                    StartTime = x.StartTime,
                    EstEndDate = x.EstEndDate.Value.Date.Add(x.EndTime.Value),
                    EstStartDate = x.EstStartDate.Value.Date.Add(x.StartTime.Value),
                    GradeCode = x.GradeCode,
                    LessonName = x.LessonName,
                    LessonName2 = x.LessonName2,
                    LessonPlanCode = x.LessonPlanCode,
                    NumOfSessions = x.NumOfSessions,
                    SectionCode = x.SectionCode,
                    SubCodes = x.SubCodes,
                    Topics = x.Topics,
                    Topics2 = x.Topics2,
                    Id = x.Id
                }).ToList();
            }
            return lessonPlanDetaillist;
        }
    }

    #endregion
}
