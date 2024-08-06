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
    #region GetCostAllocationSetupList

    public class GetCostAllocationSetupList : IRequest<List<TblFinSysCostAllocationSetupDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCostAllocationSetupListHandler : IRequestHandler<GetCostAllocationSetupList, List<TblFinSysCostAllocationSetupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCostAllocationSetupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinSysCostAllocationSetupDto>> Handle(GetCostAllocationSetupList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCostAllocationSetupList method start----");
            var item = await _context.CostAllocationSetups.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .ProjectTo<TblFinSysCostAllocationSetupDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCostAllocationSetupList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSingleCostAllocationSetup

    public class GetSingleCostAllocationSetup : IRequest<TblFinSysCostAllocationSetupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSingleCostAllocationSetupHandler : IRequestHandler<GetSingleCostAllocationSetup, TblFinSysCostAllocationSetupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleCostAllocationSetupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinSysCostAllocationSetupDto> Handle(GetSingleCostAllocationSetup request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSingleCostAllocationSetup method start----");
            var item = await _context.CostAllocationSetups.AsNoTracking()
                .Where(e => e.Id == request.Id)
               .ProjectTo<TblFinSysCostAllocationSetupDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSingleCostAllocationSetup method Ends----");
            return item;
        }
    }

    #endregion


    #region GetCostAllocationSetupSelectList
    public class GetCostAllocationSetupSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCostAllocationSetupSelectListHandler : IRequestHandler<GetCostAllocationSetupSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCostAllocationSetupSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCostAllocationSetupSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.CostAllocationSetups.AsNoTracking()
                .Where(e => e.IsActive)
              .Select(e => new CustomSelectListItem
              {
                  Text = isArab ? e.CostName2 : e.CostName,
                  TextTwo = e.CostType,
                  Value = e.Id.ToString(),
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion


    #region CreateUpdateCostAllocationSetup

    public class CreateUpdateCostAllocationSetup : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinSysCostAllocationSetupDto Input { get; set; }
    }

    public class CreateUpdateCostAllocationSetupQueryHandler : IRequestHandler<CreateUpdateCostAllocationSetup, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateCostAllocationSetupQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateCostAllocationSetup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateCostAllocationSetup method start----");

                var obj = request.Input;
                TblFinSysCostAllocationSetup seg = new();
                if (obj.Id > 0)
                    seg = await _context.CostAllocationSetups.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else if (await _context.CostAllocationSetups.AnyAsync(e => e.CostCode == obj.CostCode))
                    return ApiMessageInfo.DuplicateInfo(obj.CostCode);

                seg.CostType = obj.CostType;
                seg.CostName = obj.CostName;
                seg.CostName2 = obj.CostName2;
                seg.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.CostAllocationSetups.Update(seg);
                }
                else
                {
                    seg.CostCode = obj.CostCode.ToUpper();
                    await _context.CostAllocationSetups.AddAsync(seg);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateCostAllocationSetup method Exit----");
                return ApiMessageInfo.Status(1, seg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateCostAllocationSetup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
