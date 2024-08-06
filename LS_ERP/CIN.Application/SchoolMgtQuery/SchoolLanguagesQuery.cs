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
    public class GetSchoolLanguagesList : IRequest<PaginatedList<TblSysSchoolLanguagesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolLanguagesListHandler : IRequestHandler<GetSchoolLanguagesList, PaginatedList<TblSysSchoolLanguagesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolLanguagesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolLanguagesDto>> Handle(GetSchoolLanguagesList request, CancellationToken cancellationToken)
        {

          
            var schoolLanguages = await _context.SysSchoolLanguages.AsNoTracking().ProjectTo<TblSysSchoolLanguagesDto>(_mapper.ConfigurationProvider)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken); 

            return schoolLanguages;
        }


    }

    #endregion

    #region GetById

    public class GetSchoolLanguagesById : IRequest<TblSysSchoolLanguagesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolLanguagesByIdHandler : IRequestHandler<GetSchoolLanguagesById, TblSysSchoolLanguagesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolLanguagesByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolLanguagesDto> Handle(GetSchoolLanguagesById request, CancellationToken cancellationToken)
        {

            TblSysSchoolReligionDto obj = new();
            var SchoolLanguages = await _context.SysSchoolLanguages.AsNoTracking().ProjectTo<TblSysSchoolLanguagesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolLanguages;
            // throw new NotImplementedException();
        }
    }
    #endregion
}
