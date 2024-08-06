using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.DB;
using CIN.Domain.InventorySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventoryQuery
{

    #region GetInvtConfig
    public class GetInvtConfig : IRequest<TblInvDefInventoryConfigDto>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetInvtConfigQueryHandler : IRequestHandler<GetInvtConfig, TblInvDefInventoryConfigDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetInvtConfigQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblInvDefInventoryConfigDto> Handle(GetInvtConfig request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetInvtConfig method start----");

            var item = await _context.InvInventoryConfigs.OrderBy(e => e.Id)
                .ProjectTo<TblInvDefInventoryConfigDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(cancellationToken) ?? new();
            item.CanEdit = await _context.InvItemMaster.AnyAsync(e=>e.Id > 0);
            return item;

        }
    }

    #endregion

    #region CreateUpdate

    public class CreateInvtConfig : IRequest<(string msg, int InvconfigId)>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefInventoryConfigDto Input { get; set; }
    }

    public class CreateInvtConfigQueryHandler : IRequestHandler<CreateInvtConfig, (string msg, int InvconfigId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInvtConfigQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int InvconfigId)> Handle(CreateInvtConfig request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateInvtConfig method start----");

                var obj = request.Input;
                TblInvDefInventoryConfig cObj = await _context.InvInventoryConfigs.OrderBy(e => e.Id).FirstOrDefaultAsync() ?? new();

                //if (obj.Id > 0)
                //{
                //    // cObj = await _context.InvInventoryConfigs.FirstOrDefaultAsync(e => e.Id == obj.Id);
                //    cObj = await _context.InvInventoryConfigs.OrderBy(e => e.Id).FirstOrDefaultAsync();
                //}

                cObj.AutoGenItemCode = obj.AutoGenItemCode;
                cObj.PrefixCatCode = obj.PrefixCatCode;
                cObj.NewItemIndicator = obj.NewItemIndicator;
                cObj.ItemLength = obj.ItemLength;
                cObj.CategoryLength = obj.CategoryLength;

                if (cObj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvInventoryConfigs.Update(cObj);
                }
                else
                {
                    cObj.CentralWHCode = obj.CentralWHCode;
                    cObj.CreatedOn = DateTime.Now;

                    cObj.CentralWHCode = obj.CentralWHCode.ToUpper();
                    if (await _context.InvInventoryConfigs.AnyAsync(e => e.CentralWHCode == obj.CentralWHCode))
                        return (ApiMessageInfo.Duplicate(nameof(obj.CentralWHCode)), 0);

                    await _context.InvInventoryConfigs.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateInvtConfig method Exit----");
                return (string.Empty, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInvtConfig Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return (ApiMessageInfo.Failed, 0);
            }
        }
    }
    #endregion
}
