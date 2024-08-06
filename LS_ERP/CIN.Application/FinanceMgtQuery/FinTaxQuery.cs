using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{

    #region GetPagedList

    public class GetAllTaxList : IRequest<List<TblErpSysSystemTaxDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAllTaxListHandler : IRequestHandler<GetAllTaxList, List<TblErpSysSystemTaxDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllTaxListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblErpSysSystemTaxDto>> Handle(GetAllTaxList request, CancellationToken cancellationToken)
        {
            var list = await _context.SystemTaxes.AsNoTracking()
              .ProjectTo<TblErpSysSystemTaxDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(e => e.Id)
                 .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion


    #region GetTax
    public class GetFinTax : IRequest<TblErpSysSystemTaxDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetFinTaxQueryHandler : IRequestHandler<GetFinTax, TblErpSysSystemTaxDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetFinTaxQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblErpSysSystemTaxDto> Handle(GetFinTax request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAcTypeList method start----");
            var obj = await _context.SystemTaxes.AsNoTracking()
                .Where(e=>e.Id == request.Id)
               .ProjectTo<TblErpSysSystemTaxDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetAcTypeList method Ends----");
            return obj;
        }
    }

    #endregion


    #region CreateFinTax

    public class CreateFinTax : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysSystemTaxDto Input { get; set; }
    }

    public class CreateFinTaxQueryHandler : IRequestHandler<CreateFinTax, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateFinTaxQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateFinTax request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateFinTax method start----");

                var obj = request.Input;
                TblErpSysSystemTax Tax = new();
                if (obj.Id > 0)
                    Tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                Tax.TaxName = obj.TaxName;
                Tax.IsActive = obj.IsActive;
                Tax.IsInterState = obj.IsInterState;

                Tax.TaxComponent01 = obj.TaxComponent01;
                Tax.Taxper01 = obj.Taxper01;
                Tax.InputAcCode01 = obj.InputAcCode01;
                Tax.OutputAcCode01 = obj.OutputAcCode01;

                Tax.TaxComponent02 = obj.TaxComponent02;
                Tax.Taxper02 = obj.Taxper02;
                Tax.InputAcCode02 = obj.InputAcCode02;
                Tax.OutputAcCode02 = obj.OutputAcCode02;

                Tax.TaxComponent03 = obj.TaxComponent03;
                Tax.Taxper03 = obj.Taxper03 ?? 0;
                Tax.InputAcCode03 = obj.InputAcCode03;
                Tax.OutputAcCode03 = obj.OutputAcCode03;

                Tax.TaxComponent04 = obj.TaxComponent04;
                Tax.Taxper04 = obj.Taxper04 ?? 0;
                Tax.InputAcCode04 = obj.InputAcCode04;
                Tax.OutputAcCode04 = obj.OutputAcCode04;

                Tax.TaxComponent05 = obj.TaxComponent05;
                Tax.Taxper05 = obj.Taxper05 ?? 0;
                Tax.InputAcCode05 = obj.InputAcCode05;
                Tax.OutputAcCode05 = obj.OutputAcCode05;

                if (obj.Id > 0)
                {
                    _context.SystemTaxes.Update(Tax);
                }
                else
                {
                    Tax.TaxCode = obj.TaxCode.ToUpper();
                    await _context.SystemTaxes.AddAsync(Tax);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateFinTax method Exit----");
                return ApiMessageInfo.Status(1, Tax.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateFinTax Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
