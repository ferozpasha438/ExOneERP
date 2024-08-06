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
    #region GetListOfBranch
    public class GetTeacherBranchList : IRequest<List<TblSysSchoolBranchesDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetTeacherBranchListHandler : IRequestHandler<GetTeacherBranchList, List<TblSysSchoolBranchesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolBranchesDto>> Handle(GetTeacherBranchList request, CancellationToken cancellationToken)
        {
            List<TblSysSchoolBranchesDto> branchList = new();
            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

            if (teacher != null)
            {
                branchList = await _context.SchoolBranches.AsNoTracking().ProjectTo<TblSysSchoolBranchesDto>(_mapper.ConfigurationProvider).Where(e => e.BranchCode == teacher.PrimaryBranchCode).ToListAsync();
            }

            return branchList;
        }


    }


    #endregion



    #region GetWeekOfTheBranch
    public class GetWeekOffBranchList : IRequest<List<TeacherWeekOffBranchDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetWeekOffBranchListHandler : IRequestHandler<GetWeekOffBranchList, List<TeacherWeekOffBranchDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWeekOffBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TeacherWeekOffBranchDto>> Handle(GetWeekOffBranchList request, CancellationToken cancellationToken)
        {
            List<TeacherWeekOffBranchDto> WeekOffList = new();
            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

            if (teacher != null)
            {
                //WeekOffList = await _context.SchoolBranches.AsNoTracking().Select(x => new TeacherWeekOffBranchDto{
                //    WeekOff1 = x.WeekOff1,
                //    WeekOff2 = x.WeekOff2 })
                //    .Where(e=>e.BranchCode==teacher.PrimaryBranchCode).ToListAsync();

                WeekOffList = await _context.SchoolBranches.AsNoTracking()
                           .Where(e => e.BranchCode == teacher.PrimaryBranchCode)
                           .Select(e => new TeacherWeekOffBranchDto
                           { 
                               WeekOff1 = e.WeekOff1, WeekOff2 = e.WeekOff2 
                           }).ToListAsync();
            }

            return WeekOffList;
        }


    }


    #endregion
}
