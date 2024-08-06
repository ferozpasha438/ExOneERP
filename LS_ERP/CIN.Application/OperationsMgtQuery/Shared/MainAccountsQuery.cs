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
   public class MainAccountsQueryQuery
    {
    }
    #region GetSelectMainAccountsList
    public class GetSelectMainAccountsList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectMainAccountsListHandler : IRequestHandler<GetSelectMainAccountsList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectMainAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectMainAccountsList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .Where(e => e.FinAcCode.Contains(search) || e.FinAcName.Contains(search)||search==null )
              .Select(e => new CustomSelectListItem {

                  Text = "(" + e.FinAcCode + ") " + e.FinAcName,
                  TextTwo = e.FinAcCode,
                  Value = e.FinAcCode,
                  //Value = "(" + e.FinAcCode + ") " + e.FinAcName,
                  
                  /*Text = e.FinAcCode, Value = e.FinAcCode.ToString()*/ })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion


    #region GetAccountByAccountCode

    public class GetAccountByAccountCode : IRequest<TblFinDefMainAccountsDto>
    {
        public UserIdentityDto User { get; set; }
        public string AccountCode { get; set; }
    }

    public class GetAccountByAccountCodeHandler : IRequestHandler<GetAccountByAccountCode, TblFinDefMainAccountsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountByAccountCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinDefMainAccountsDto> Handle(GetAccountByAccountCode request, CancellationToken cancellationToken)
        {
            TblFinDefMainAccountsDto obj = new();
            var Account = await _context.FinMainAccounts.AsNoTracking().FirstOrDefaultAsync(e => e.FinAcCode== request.AccountCode);
            if (Account is not null)
            {
                obj.FinAcCode = Account.FinAcCode;
                obj.FinAcName = Account.FinAcName;
                obj.FinAcDesc = Account.FinAcDesc;
                obj.FinAcAlias = Account.FinAcAlias;
                obj.FinIsPayCode = Account.FinIsPayCode;
                obj.FinPayCodetype = Account.FinPayCodetype;
                obj.FinIsIntegrationAC = Account.FinIsIntegrationAC;
                obj.Fintype = Account.Fintype;
                obj.FinCatCode = Account.FinCatCode;
                obj.FinSubCatCode = Account.FinSubCatCode;
                obj.FinActLastSeq = Account.FinActLastSeq;
                obj.CreatedOn = Account.CreatedOn;
                obj.ModifiedOn = Account.ModifiedOn;


                return obj;
            }
            else
                return null;
        }
    }


    #endregion
}
