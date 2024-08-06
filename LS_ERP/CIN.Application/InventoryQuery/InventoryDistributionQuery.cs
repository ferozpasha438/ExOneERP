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
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventoryQuery
{


    #region GetPagedList

    public class GetInventoryDistributionList : IRequest<PaginatedList<TblInvDefDistributionGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInventoryDistributionListHandler : IRequestHandler<GetInventoryDistributionList, PaginatedList<TblInvDefDistributionGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryDistributionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefDistributionGroupDto>> Handle(GetInventoryDistributionList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvDistributionGroups.AsNoTracking()
              .Where(e => e.InvDistGroup.Contains(search))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefDistributionGroupDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region GetPOInvetoryPagedList

    public class GetInventoryPoDistributionList : IRequest<PaginatedList<TblInventoryDefDistributionGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInventoryPoDistributionListHandler : IRequestHandler<GetInventoryPoDistributionList, PaginatedList<TblInventoryDefDistributionGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryPoDistributionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInventoryDefDistributionGroupDto>> Handle(GetInventoryPoDistributionList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvPoDistributionGroups.AsNoTracking()
              .Where(e => e.InvDistGroup.Contains(search))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInventoryDefDistributionGroupDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region CreateUpdate


    public class CreateInventoryDistribution : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefDistributionGroupDto Input { get; set; }
    }

    public class CreateInventoryDistributionQueryHandler : IRequestHandler<CreateInventoryDistribution, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryDistributionQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInventoryDistribution request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateInventoryDistribution method start----");

                var obj = request.Input;
                TblInvDefDistributionGroup cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.InvDistributionGroups.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.InvAssetAc = obj.InvAssetAc;
                cObj.InvNonAssetAc = obj.InvNonAssetAc;
                cObj.InvCashPOAC = obj.InvCashPOAC;
                cObj.InvCOGSAc = obj.InvCOGSAc;
                cObj.InvAdjAc = obj.InvAdjAc;
                cObj.InvSalesAc = obj.InvSalesAc;
                cObj.InvDefaultAPAc = obj.InvDefaultAPAc;
                cObj.InvInTransitAc = obj.InvInTransitAc;
                cObj.InvCostCorAc = obj.InvCostCorAc;
                cObj.InvWIPAc = obj.InvWIPAc;
                cObj.InvWriteOffAc = obj.InvWriteOffAc;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvDistributionGroups.Update(cObj);
                }
                else
                {
                    cObj.InvDistGroup = obj.InvDistGroup.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    if (await _context.InvDistributionGroups.AnyAsync(e => e.InvDistGroup == obj.InvDistGroup))
                        return (ApiMessageInfo.DuplicateInfo(nameof(obj.InvDistGroup)));


                    await _context.InvDistributionGroups.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateInventoryDistribution method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInventoryDistribution Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


    #region CreateUpdateInventoryPo

    public class CreateInventoryPoDistribution : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefDistributionGroupDto Input { get; set; }
    }

    public class CreateInventoryPoDistributionQueryHandler : IRequestHandler<CreateInventoryPoDistribution, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryPoDistributionQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInventoryPoDistribution request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateInventoryPoDistribution method start----");

                var obj = request.Input;
                TblInventoryDefDistributionGroup cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.InvPoDistributionGroups.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.InvAssetAc = obj.InvAssetAc;
                cObj.InvNonAssetAc = obj.InvNonAssetAc;
                cObj.InvCashPOAC = obj.InvCashPOAC;
                cObj.InvCOGSAc = obj.InvCOGSAc;
                cObj.InvAdjAc = obj.InvAdjAc;
                cObj.InvSalesAc = obj.InvSalesAc;
                cObj.InvDefaultAPAc = obj.InvDefaultAPAc;
                cObj.InvInTransitAc = obj.InvInTransitAc;
                cObj.InvCostCorAc = obj.InvCostCorAc;
                cObj.InvWIPAc = obj.InvWIPAc;
                cObj.InvWriteOffAc = obj.InvWriteOffAc;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvPoDistributionGroups.Update(cObj);
                }
                else
                {
                    cObj.InvDistGroup = obj.InvDistGroup.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    if (await _context.InvPoDistributionGroups.AnyAsync(e => e.InvDistGroup == obj.InvDistGroup))
                        return (ApiMessageInfo.DuplicateInfo(nameof(obj.InvDistGroup)));


                    await _context.InvPoDistributionGroups.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateInventoryPoDistribution method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInventoryPoDistribution Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
