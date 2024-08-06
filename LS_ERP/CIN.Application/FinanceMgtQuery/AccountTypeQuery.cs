using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{
    #region GetAcTypeList

    public class GetAcTypeList : IRequest<List<TblFinSysAccountTypeDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAcTypeListHandler : IRequestHandler<GetAcTypeList, List<TblFinSysAccountTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinSysAccountTypeDto>> Handle(GetAcTypeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAcTypeList method start----");
            var item = await _context.FinSysAccountTypes.AsNoTracking()
                .OrderBy(e => e.Id)
               .ProjectTo<TblFinSysAccountTypeDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAcTypeList method Ends----");
            return item;
        }
    }

    #endregion
}
