using AutoMapper;
using CIN.Application.Common;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SystemQuery
{
    #region GetZoneSelectList

    public class GetZoneSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetZoneSelectListHandler : IRequestHandler<GetZoneSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetZoneSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetZoneSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();

            var list = await _context.ZoneSettings.AsNoTracking()
                    //.Where(e => e.CompanyID == request.CompanyId || e.AccountId == request.AccountId)
                    .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.NameAR : e.Name, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion

    #region GetZoneSelectListItem

    public class GetZoneSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetZoneSelectListItemHandler : IRequestHandler<GetZoneSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetZoneSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetZoneSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.ZoneSettings.AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.NameAR : e.Name, Value = e.Code.ToString() })
                .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
