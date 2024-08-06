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
using CIN.Domain.OpeartionsMgt;
using CIN.Application.PurchaseSetupDtos;

namespace CIN.Application.FinPurchaseMgtQuery
{
    #region GetSelectPoTermsCodeList
    public class GetSelectPoTermsCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectPoTermsCodeListHandler : IRequestHandler<GetSelectPoTermsCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPoTermsCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPoTermsCodeList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = _context.PopVendorPOTermsCodes.AsNoTracking();

            if (search.HasValue())
                list = list.Where(e => e.POTermsCode.Contains(search));

            var newList = await list.OrderByDescending(e => e.Id)
              .Select(e => new CustomSelectListItem { Text = e.POTermsCode, Value = e.POTermsCode.ToString(), TextTwo = e.POTermsDueDays.ToString() })
                 .ToListAsync(cancellationToken);
            return newList;
        }
    }
    #endregion


    #region GetCustomSelectPoTermsCodeList
    public class GetCustomSelectPoTermsCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCustomSelectPoTermsCodeListHandler : IRequestHandler<GetCustomSelectPoTermsCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomSelectPoTermsCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomSelectPoTermsCodeList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = _context.PopVendorPOTermsCodes.AsNoTracking();

            if (search.HasValue())
                list = list.Where(e => e.POTermsCode.Contains(search));

            var newList = await list.OrderByDescending(e => e.Id)
              .Select(e => new CustomSelectListItem { Text = e.POTermsName, Value = e.POTermsCode, TextTwo = e.POTermsDueDays.ToString() })
                 .ToListAsync(cancellationToken);
            return newList;
        }
    }
    #endregion

    #region GetPoTermsByTermsCode

    public class GetPoTermsByTermsCode : IRequest<TblPopDefVendorPOTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public string PoTermsCode { get; set; }
    }

    public class GetPoTermsByTermsCodeHandler : IRequestHandler<GetPoTermsByTermsCode, TblPopDefVendorPOTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPoTermsByTermsCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorPOTermsCodeDto> Handle(GetPoTermsByTermsCode request, CancellationToken cancellationToken)
        {
            TblPopDefVendorPOTermsCodeDto obj = new();
            var PoTerms = await _context.PopVendorPOTermsCodes.AsNoTracking().FirstOrDefaultAsync(e => e.POTermsCode.Contains(request.PoTermsCode));
            if (PoTerms is not null)
            {
                obj.POTermDiscDays = PoTerms.POTermDiscDays;
                obj.POTermsCode = PoTerms.POTermsCode;
                obj.POTermsName = PoTerms.POTermsName;
                obj.POTermsDueDays = PoTerms.POTermsDueDays;
                obj.POTermsDesc = PoTerms.POTermsDesc;


                return obj;
            }
            else
                return null;
        }
    }



    #endregion
}
