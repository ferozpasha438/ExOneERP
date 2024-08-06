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

namespace CIN.Application.OperationsMgtQuery
{
   public class SalesTermsCodesQuery
    {
    }
    #region GetCustomerCategorySelectList
    public class GetSelectSalesTermsCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSalesTermsCodeListHandler : IRequestHandler<GetSelectSalesTermsCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSalesTermsCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSalesTermsCodeList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.SndSalesTermsCodes.AsNoTracking()
                .Where(e => e.SalesTermsCode.Contains(search)||search==null)
              .Select(e => new CustomSelectListItem { Text = e.SalesTermsCode, Value = e.SalesTermsCode.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion


    #region GetSalesTermsByTermsCode

    public class GetSalesTermsByTermsCode : IRequest<TblSndDefSalesTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public string SalesTermsCode { get; set; }
    }

    public class GetSalesTermsByTermsCodeHandler : IRequestHandler<GetSalesTermsByTermsCode, TblSndDefSalesTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesTermsByTermsCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSalesTermsCodeDto> Handle(GetSalesTermsByTermsCode request, CancellationToken cancellationToken)
        {
            TblSndDefSalesTermsCodeDto obj = new();
            var SalesTerms = await _context.SndSalesTermsCodes.AsNoTracking().FirstOrDefaultAsync(e => e.SalesTermsCode == request.SalesTermsCode);
            if (SalesTerms is not null)
            {
                obj.SalesTermDiscDays = SalesTerms.SalesTermDiscDays;
                obj.SalesTermsCode = SalesTerms.SalesTermsCode;
                obj.SalesTermsName = SalesTerms.SalesTermsName;
                obj.SalesTermsDueDays = SalesTerms.SalesTermsDueDays;
                obj.SalesTermsDesc = SalesTerms.SalesTermsDesc;
                

                return obj;
            }
            else
                return null;
        }
    }



    #endregion
}
