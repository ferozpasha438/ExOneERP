using AutoMapper;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SalesSetupQuery
{


    #region GetCustomSelectSalesTermsCodeList

    public class GetCustomSelectSalesTermsCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCustomSelectSalesTermsCodeListHandler : IRequestHandler<GetCustomSelectSalesTermsCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomSelectSalesTermsCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomSelectSalesTermsCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCustomSelectSalesTermsCodeList method start----");
            var list = await _context.SndSalesTermsCodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.SalesTermsName, Value = e.SalesTermsCode, TextTwo = e.SalesTermsDueDays.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCustomSelectSalesTermsCodeList method Ends----");
            return list;
        }
    }

    #endregion
}
