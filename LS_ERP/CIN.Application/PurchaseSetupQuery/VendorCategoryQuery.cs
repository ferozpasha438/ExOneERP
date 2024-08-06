using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
using CIN.Application.PurchaseSetupDtos;

namespace CIN.Application.PurchaseSetupQuery
{
    #region GetVendorCategorySelectList
    public class GetVendorCategorySelectListQuery : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetVendorCategorySelectListQueryHandler : IRequestHandler<GetVendorCategorySelectListQuery, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorCategorySelectListQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVendorCategorySelectListQuery request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.PopVendorCategories.AsNoTracking()
                .Where(e => e.VenCatCode.Contains(search))
              .Select(e => new CustomSelectListItem { Text = e.VenCatCode, Value = e.VenCatCode.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion


    #region getCustCatByVenCatCode

    public class GetCustCatByVenCatCode : IRequest<TblPopDefVendorCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendorCatCode { get; set; }
    }

    public class GetCustCatByVenCatCodeHandler : IRequestHandler<GetCustCatByVenCatCode, TblPopDefVendorCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustCatByVenCatCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorCategoryDto> Handle(GetCustCatByVenCatCode request, CancellationToken cancellationToken)
        {
            TblPopDefVendorCategoryDto obj = new();
            var VendorCat = await _context.PopVendorCategories.AsNoTracking().FirstOrDefaultAsync(e => e.VenCatCode.Contains(request.VendorCatCode));
            if (VendorCat is not null)
            {
                obj.VenCatCode = VendorCat.VenCatCode;
                obj.VenCatDesc = VendorCat.VenCatDesc;
                obj.VenCatName = VendorCat.VenCatName;
                obj.Id = VendorCat.Id;
                return obj;
            }
            else
                return new TblPopDefVendorCategoryDto();
        }
    }



    #endregion
}
