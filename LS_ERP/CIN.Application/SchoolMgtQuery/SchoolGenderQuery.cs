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
    public class GetSchoolGenderList : IRequest<PaginatedList<TblSysSchoolGenderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolGenderListHandler : IRequestHandler<GetSchoolGenderList, PaginatedList<TblSysSchoolGenderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolGenderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolGenderDto>> Handle(GetSchoolGenderList request, CancellationToken cancellationToken)
        {

            var schoolGender = await _context.SysSchoolGender.AsNoTracking().ProjectTo<TblSysSchoolGenderDto>(_mapper.ConfigurationProvider)
                               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolGender;
        }


    }

    #endregion

    #region GetById

    public class GetSchoolGenderById : IRequest<TblSysSchoolGenderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolGenderByIdHandler : IRequestHandler<GetSchoolGenderById, TblSysSchoolGenderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolGenderByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolGenderDto> Handle(GetSchoolGenderById request, CancellationToken cancellationToken)
        {

            TblSysSchoolGenderDto obj = new();
            var SchoolGender = await _context.SysSchoolGender.AsNoTracking().ProjectTo<TblSysSchoolGenderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolGender;
            // throw new NotImplementedException();
        }
    }
    #endregion

    
    
}
