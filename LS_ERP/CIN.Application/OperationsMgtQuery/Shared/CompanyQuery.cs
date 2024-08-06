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
using CIN.Domain.SystemSetup;

namespace CIN.Application.OperationsMgtQuery
{
   public class CompanyQuery
    {
    }
    #region getCompanyByCityCode

    public class GetCompanyByCityCode : IRequest<TblErpSysCompany>
    {
        public UserIdentityDto User { get; set; }
        public string CityCode { get; set; }            //Branch in HRM
    }

    public class GetCompanyByCityCodeHandler : IRequestHandler<GetCompanyByCityCode, TblErpSysCompany>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanyByCityCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompany> Handle(GetCompanyByCityCode request, CancellationToken cancellationToken)
        {
                  

            var company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(e => e.City == request.CityCode);
            if (company is null)
            {
                var UserBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == UserBranch.CompanyId);
            }
            return company;
        }
    }



    #endregion

}
