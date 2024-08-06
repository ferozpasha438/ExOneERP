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
    #region GetAll
    public class GetSchoolDisciplinaryActionList : IRequest<List<TblDefStudentNoticesDto>>
    {
        public UserIdentityDto User { get; set; }

        public string StuAdmNum { get; set; }
       
    }

    public class GetSchoolDisciplinaryListHandler : IRequestHandler<GetSchoolDisciplinaryActionList, List<TblDefStudentNoticesDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetSchoolDisciplinaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentNoticesDto>> Handle(GetSchoolDisciplinaryActionList request, CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentNotices.AsNoTracking().ProjectTo<TblDefStudentNoticesDto>(_mapper.ConfigurationProvider).Where(e=>e.StuAdmNum == request.StuAdmNum).ToListAsync();
            
            return list;
        }
    }
    #endregion
}
