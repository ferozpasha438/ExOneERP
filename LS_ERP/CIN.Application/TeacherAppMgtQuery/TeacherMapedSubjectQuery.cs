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
    #region GetTeacherSubjectMappingList
    public class GetTeacherSubjectMappingList : IRequest<List<TblDefSchoolTeacherSubjectsMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
       

    }

    public class GetTeacherSubjectMappingListHandler : IRequestHandler<GetTeacherSubjectMappingList, List<TblDefSchoolTeacherSubjectsMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherSubjectMappingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherSubjectsMappingDto>> Handle(GetTeacherSubjectMappingList request, CancellationToken cancellationToken)
        {
            var TeacherSubjectMapList = await _context.DefSchoolTeacherSubjectsMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherSubjectsMappingDto>(_mapper.ConfigurationProvider).Where(e => (e.TeacherCode == request.TeacherCode )).ToListAsync();

            return TeacherSubjectMapList;
        }


    }

    #endregion
}
