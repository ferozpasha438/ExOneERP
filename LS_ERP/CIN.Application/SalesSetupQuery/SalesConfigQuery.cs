using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SalesSetupDtos;
using CIN.DB;
using CIN.Domain.SalesSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SalesSetupQuery
{

    #region GetSingleSalesConfig

    public class GetSingleSalesConfig : IRequest<TblInvDefSalesConfigDto>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSingleSalesConfigHandler : IRequestHandler<GetSingleSalesConfig, TblInvDefSalesConfigDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSalesConfigHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefSalesConfigDto> Handle(GetSingleSalesConfig request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSingleSalesConfig method start----");
            var item = await _context.SalesConfigs.AsNoTracking()
               .ProjectTo<TblInvDefSalesConfigDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);

            item.CanEdit = await _context.OprCustomers.AnyAsync(e => e.Id > 0);

            Log.Info("----Info GetSingleSalesConfig method Ends----");
            return item ?? new();
        }
    }

    #endregion

    #region CreateUpdateSalesConfig

    public class CreateUpdateSalesConfig : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefSalesConfigDto Input { get; set; }
    }

    public class CreateUpdateSalesConfigHandler : IRequestHandler<CreateUpdateSalesConfig, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateUpdateSalesConfigHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(CreateUpdateSalesConfig request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSalesConfig method start----");
                var obj = request.Input;

                var salesConfig = await _context.SalesConfigs.FirstOrDefaultAsync();

                TblInvDefSalesConfig CustCateg = salesConfig ?? new();

                CustCateg.AutoGenCustCode = obj.AutoGenCustCode;
                CustCateg.PrefixCatCode = obj.PrefixCatCode;
                CustCateg.CustLength = obj.CustLength;
                CustCateg.CategoryLength = obj.CategoryLength;
                CustCateg.NewCustIndicator = obj.NewCustIndicator;

                if (salesConfig is null)
                {
                    await _context.SalesConfigs.AddAsync(CustCateg);
                }
                else
                    _context.SalesConfigs.Update(CustCateg);

                await _context.SaveChangesAsync();
                return ApiMessageInfo.Status(1, CustCateg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSalesConfig Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion
}
