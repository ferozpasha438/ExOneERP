using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
    public class GetSchoolReligionList : IRequest<PaginatedList<TblSysSchoolReligionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolReligionListHandler : IRequestHandler<GetSchoolReligionList, PaginatedList<TblSysSchoolReligionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolReligionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolReligionDto>> Handle(GetSchoolReligionList request, CancellationToken cancellationToken)
        {

            
            var schoolReligion = await _context.SysSchoolReligion.AsNoTracking().ProjectTo<TblSysSchoolReligionDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolReligion;
        }


    }

    #endregion

    #region GetById

    public class GetSchoolReligionById : IRequest<TblSysSchoolReligionDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolReligionByIdHandler : IRequestHandler<GetSchoolReligionById, TblSysSchoolReligionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolReligionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolReligionDto> Handle(GetSchoolReligionById request, CancellationToken cancellationToken)
        {

            TblSysSchoolReligionDto obj = new();
            var SchoolReligion = await _context.SysSchoolReligion.AsNoTracking().ProjectTo<TblSysSchoolReligionDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolReligion;
            // throw new NotImplementedException();
        }
    }
    #endregion
}
