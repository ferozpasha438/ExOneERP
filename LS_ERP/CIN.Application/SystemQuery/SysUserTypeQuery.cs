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
    #region GetUserTypeSelectList

    public class GetUserTypeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetUserTypeSelectListHandler : IRequestHandler<GetUserTypeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserTypeSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUserTypeSelectList request, CancellationToken cancellationToken)
        {
            var list = await _context.UserTypes.AsNoTracking()
                    //.Where(e => e.CompanyID == request.CompanyId || e.AccountId == request.AccountId)
                    .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UerType, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
