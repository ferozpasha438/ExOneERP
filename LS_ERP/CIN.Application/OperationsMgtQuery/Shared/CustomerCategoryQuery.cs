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


namespace CIN.Application.OperationsMgtQuery
{
   public class CustomerCategoryQuery
    {
    }
    #region GetCustomerCategorySelectList
    public class GetCustomerCategorySelectListQuery : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCustomerCategorySelectListQueryHandler : IRequestHandler<GetCustomerCategorySelectListQuery, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerCategorySelectListQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomerCategorySelectListQuery request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.SndCustomerCategories.AsNoTracking()
                .Where(e => e.CustCatCode.Contains(search)||search==null)
              .Select(e => new CustomSelectListItem { Text = e.CustCatCode, Value = e.CustCatCode.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion


    #region getCustCatByCustCatCode

    public class GetCustCatByCustCatCode : IRequest<TblSndDefCustomerCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCatCode { get; set; }
    }

    public class GetCustCatByCustCatCodeHandler : IRequestHandler<GetCustCatByCustCatCode, TblSndDefCustomerCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustCatByCustCatCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefCustomerCategoryDto> Handle(GetCustCatByCustCatCode request, CancellationToken cancellationToken)
        {
            TblSndDefCustomerCategoryDto obj = new();
                var CustomerCat = await _context.SndCustomerCategories.AsNoTracking().FirstOrDefaultAsync(e => e.CustCatCode == request.CustomerCatCode);
            if (CustomerCat is not null)
            {
                obj.CustCatCode = CustomerCat.CustCatCode;
                obj.CustCatDesc = CustomerCat.CustCatDesc;
                obj.CustCatName = CustomerCat.CustCatName;
                obj.Id = CustomerCat.Id;
                return obj;
            }
            else
                return null;
        }
    }



    #endregion
}
