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

namespace CIN.Application.FinPurchaseMgtQuery
{
    #region GetVendorsCustomList

    public class GetVendorsCustomList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool? IsPayment { get; set; }
    }

    public class GetVendorsCustomListHandler : IRequestHandler<GetVendorsCustomList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorsCustomListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVendorsCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetVendorsCustomList method start----");
            var list = _context.VendorMasters
               .Where(e => e.IsActive);

            if (request.IsPayment is null)
                list = list.Where(e => !e.VendOnHold);

            var newList = await list.AsNoTracking()
           .OrderBy(e => e.Id)
                 .Select(e => new CustomSelectListItem { Text = e.VendName, TextTwo = e.VendArbName, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetVendorsCustomList method Ends----");
            return newList;
        }
    }


    #endregion

    #region GetLanVendorsCustomList

    public class GetLanVendorsCustomList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool? IsPayment { get; set; }
        public string Search { get; set; }
    }

    public class GetLanVendorsCustomListHandler : IRequestHandler<GetLanVendorsCustomList, List<LanCustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLanVendorsCustomListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetLanVendorsCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetLanVendorsCustomList method start----");
            var list = _context.VendorMasters
                .Where(e => e.IsActive);

            if (request.Search.HasValue())
                list = list.Where(e => e.VendName.Contains(request.Search)
                                      || e.VendArbName.Contains(request.Search) || e.VendCode.Contains(request.Search));            

            if (request.IsPayment is null)
                list = list.Where(e => !e.VendOnHold);

            var newList = await list.AsNoTracking()
              .OrderBy(e => e.Id)
               .Select(e => new LanCustomSelectListItem { Text = e.VendName, TextAr = e.VendArbName, TextTwo = e.VendCode, Value = e.Id.ToString() })
                .ToListAsync(cancellationToken);
            Log.Info("----Info GetLanVendorsCustomList method Ends----");
            return newList;
        }
    }


    #endregion
}
