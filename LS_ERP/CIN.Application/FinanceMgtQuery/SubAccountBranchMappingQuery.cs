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

    #region GetSingleItem

    public class GetSubAccountBranchMapping : IRequest<TblFinDefAccountBranchMappingDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetSubAccountBranchMappingHandler : IRequestHandler<GetSubAccountBranchMapping, TblFinDefAccountBranchMappingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubAccountBranchMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinDefAccountBranchMappingDto> Handle(GetSubAccountBranchMapping request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSubAccountBranchMapping method start----");
            var item = await _context.FinAccountBranchMappings.AsNoTracking()
                .Where(e => e.FinBranchCode == request.BranchCode)
              .Select(obj => new TblFinDefAccountBranchMappingDto
              {
                  FinBranchCode = obj.FinBranchCode,
                  FinBranchName = obj.FinBranchName,
                  InventoryAccount = obj.InventoryAccount,
                  CashPurchase = obj.CashPurchase,
                  CostofSalesAccount = obj.CostofSalesAccount,
                  InventoryAdjustment = obj.InventoryAdjustment,
                  DefaultSalesAccount = obj.DefaultSalesAccount,
                  DefaultSalesReturn = obj.DefaultSalesReturn,
                  InventoryTransfer = obj.InventoryTransfer,
                  DefaultPayable = obj.DefaultPayable,
                  CostCorrection = obj.CostCorrection,
                  WIPUsageConsumption = obj.WIPUsageConsumption,
                  Reserved = obj.Reserved
              })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSubAccountBranchMapping method Ends----");
            return item;
        }
    }

    #endregion




    #region CreateSubAccountBranchMapping

    public class CreateSubAccountBranchMapping : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinDefAccountBranchMappingDto Input { get; set; }
    }

    public class CreateSubAccountBranchMappingQueryHandler : IRequestHandler<CreateSubAccountBranchMapping, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSubAccountBranchMappingQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateSubAccountBranchMapping request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateSubAccountBranchMapping method start----");

                var obj = request.Input;
                TblFinDefAccountBranchMapping Mapping = await _context.FinAccountBranchMappings.FirstOrDefaultAsync(e => e.FinBranchCode == obj.FinBranchCode) ?? new();

                Mapping.FinBranchName = obj.FinBranchName;
                Mapping.InventoryAccount = obj.InventoryAccount;
                Mapping.CashPurchase = obj.CashPurchase;
                Mapping.CostofSalesAccount = obj.CostofSalesAccount;
                Mapping.InventoryAdjustment = obj.InventoryAdjustment;
                Mapping.DefaultSalesAccount = obj.DefaultSalesAccount;
                Mapping.DefaultSalesReturn = obj.DefaultSalesReturn;
                Mapping.InventoryTransfer = obj.InventoryTransfer;
                Mapping.DefaultPayable = obj.DefaultPayable;
                Mapping.CostCorrection = obj.CostCorrection;
                Mapping.WIPUsageConsumption = obj.WIPUsageConsumption;
                Mapping.Reserved = obj.Reserved;

                if (Mapping.Id > 0)
                {
                    _context.FinAccountBranchMappings.Update(Mapping);
                }
                else
                {
                    Mapping.FinBranchCode = obj.FinBranchCode;
                    await _context.FinAccountBranchMappings.AddAsync(Mapping);
                }
                await _context.SaveChangesAsync();

                Log.Info("----Info CreateSubAccountBranchMapping method Exit----");
                return ApiMessageInfo.Status(1, Mapping.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateSubAccountBranchMapping Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
