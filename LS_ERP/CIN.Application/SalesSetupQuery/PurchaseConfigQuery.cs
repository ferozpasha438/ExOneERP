using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SalesSetupDtos;
using CIN.DB;
using CIN.Domain.PurchaseSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SalesSetupQuery
{
    #region GetSinglePurchaseConfig

    public class GetSinglePurchaseConfig : IRequest<TblInvDefPurchaseConfigDto>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSinglePurchaseConfigHandler : IRequestHandler<GetSinglePurchaseConfig, TblInvDefPurchaseConfigDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSinglePurchaseConfigHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefPurchaseConfigDto> Handle(GetSinglePurchaseConfig request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSinglePurchaseConfig method start----");
            var item = await _context.PurchaseConfigs.AsNoTracking()
               .ProjectTo<TblInvDefPurchaseConfigDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSinglePurchaseConfig method Ends----");
            return item ?? new();
        }
    }

    #endregion

    #region CreateUpdatePurchaseConfig

    public class CreateUpdatePurchaseConfig : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefPurchaseConfigDto Input { get; set; }
    }

    public class CreateUpdatePurchaseConfigHandler : IRequestHandler<CreateUpdatePurchaseConfig, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateUpdatePurchaseConfigHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(CreateUpdatePurchaseConfig request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdatePurchaseConfig method start----");
                var obj = request.Input;

                var PurchaseConfig = await _context.PurchaseConfigs.FirstOrDefaultAsync();

                TblInvDefPurchaseConfig CustCateg = PurchaseConfig ?? new();

                CustCateg.AutoGenCustCode = obj.AutoGenCustCode;
                CustCateg.PrefixCatCode = obj.PrefixCatCode;
                CustCateg.VendLength = obj.VendLength;
                CustCateg.CategoryLength = obj.CategoryLength;
                CustCateg.NewCustIndicator = obj.NewCustIndicator;

                if (PurchaseConfig is null)
                {
                    await _context.PurchaseConfigs.AddAsync(CustCateg);
                }
                else
                    _context.PurchaseConfigs.Update(CustCateg);

                await _context.SaveChangesAsync();
                return ApiMessageInfo.Status(1, CustCateg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdatePurchaseConfig Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion

    #region CanAutoGenerateVendCode

    public class CanAutoGenerateVendCode : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }

    }

    public class CanAutoGenerateVendCodeHandler : IRequestHandler<CanAutoGenerateVendCode, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CanAutoGenerateVendCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CanAutoGenerateVendCode request, CancellationToken cancellationToken)
        {
            var item = await _context.PurchaseConfigs.AsNoTracking()
                .FirstOrDefaultAsync();

            return item?.AutoGenCustCode ?? false;
        }
    }

    #endregion
}
