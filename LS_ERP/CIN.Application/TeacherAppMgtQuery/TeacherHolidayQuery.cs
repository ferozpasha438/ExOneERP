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
    #region GetListOfBranchHoliday
    public class GetTeacherBranchHolidayList : IRequest<List<TeacherHolidayCalanderDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetTeacherBranchHolidayListHandler : IRequestHandler<GetTeacherBranchHolidayList,List<TeacherHolidayCalanderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherBranchHolidayListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TeacherHolidayCalanderDto>> Handle(GetTeacherBranchHolidayList request, CancellationToken cancellationToken)
        {
            List<TeacherHolidayCalanderDto> holidayList = new();

            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

            if(teacher != null)
            {
                holidayList = await _context.StudentHolidayClaender.AsNoTracking().ProjectTo<TeacherHolidayCalanderDto>(_mapper.ConfigurationProvider).Where(e => e.BranchCode == teacher.PrimaryBranchCode).ToListAsync();
            }

            return holidayList;

        }


    }


    #endregion
}
