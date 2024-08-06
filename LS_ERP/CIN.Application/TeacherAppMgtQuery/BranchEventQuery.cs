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
    #region GetListOfNotification
    public class GetTeacherBranchEventList : IRequest<List<TblSysSchooScheduleEventsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }

    public class GetTeacherBranchEventListHandler : IRequestHandler<GetTeacherBranchEventList, List<TblSysSchooScheduleEventsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherBranchEventListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchooScheduleEventsDto>> Handle(GetTeacherBranchEventList request, CancellationToken cancellationToken)
        {
            List<TblSysSchooScheduleEventsDto> eventList = new();
            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

            if(teacher != null) { 
            eventList = await _context.SchooScheduleEvents.AsNoTracking().ProjectTo<TblSysSchooScheduleEventsDto>(_mapper.ConfigurationProvider).Where(e => e.BranchCode == teacher.PrimaryBranchCode && e.HDate.Month == request.Month && e.HDate.Year == request.Year).ToListAsync();
                
            }

            return eventList;
        }


    }


    #endregion
}
