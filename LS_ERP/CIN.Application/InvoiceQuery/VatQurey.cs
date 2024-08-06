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

namespace CIN.Application.InvoiceQuery
{
    #region GetSelectVatList

    public class GetSelectVatList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectVatListHandler : IRequestHandler<GetSelectVatList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectVatListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectVatList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectVatList method start----");
            var item = await _context.TranTaxes.AsNoTracking()
               // .Where(e => e.CompanyId == request.User.CompanyId)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = request.User.Culture != "ar" ? e.NameEN  : e.NameAR, Value = e.TaxTariffPercentage.Value.ToString("N0") })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectVatList method Ends----");
            return item;
        }
    }

    #endregion
}
