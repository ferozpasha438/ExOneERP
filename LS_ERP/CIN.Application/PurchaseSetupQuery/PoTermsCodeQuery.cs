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

namespace CIN.Application.PurchaseSetupQuery
{
    #region GetSelectPoTermsCodeListOne

    public class GetSelectPoTermsCodeListOne : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectPoTermsCodeListOneHandler : IRequestHandler<GetSelectPoTermsCodeListOne, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPoTermsCodeListOneHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPoTermsCodeListOne request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectPoTermsCodeListOne method start----");
            var list = await _context.PopVendorPOTermsCodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.POTermsName, Value = e.POTermsCode, TextTwo = e.POTermsDueDays.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectPoTermsCodeListOne method Ends----");
            return list;
        }
    }

    #endregion
}
