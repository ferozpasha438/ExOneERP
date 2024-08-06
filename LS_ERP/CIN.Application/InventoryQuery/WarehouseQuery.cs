using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.InventorySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;



namespace CIN.Application.InventoryQuery
{
    #region GetPagedList

    public class GetWarehouseList : IRequest<PaginatedList<TblInvDefWarehouseDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetWarehouseListHandler : IRequestHandler<GetWarehouseList, PaginatedList<TblInvDefWarehouseDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWarehouseListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefWarehouseDto>> Handle(GetWarehouseList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvWarehouses.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.WHCode.Contains(search) || e.WHName.Contains(search) ||
                                e.WHBranchCode.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefWarehouseDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region GetSelectWarehouseList

    public class GetSelectWarehouseList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectWarehouseListHandler : IRequestHandler<GetSelectWarehouseList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectWarehouseListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectWarehouseList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectWarehouseList method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectWarehouseList method Ends----");
            return item;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreateWarehouse : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefWarehouseDto Input { get; set; }
    }

    public class CreateWarehouseQueryHandler : IRequestHandler<CreateWarehouse, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateWarehouseQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateWarehouse request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateWarehouse method start----");

                var obj = request.Input;
                TblInvDefWarehouse cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.WHName = obj.WHName;
                cObj.WHDesc = obj.WHDesc;
                cObj.WHAddress = obj.WHAddress;
                cObj.WHCity = obj.WHCity;
                cObj.WHState = obj.WHState;
                cObj.WHIncharge = obj.WHIncharge;
                cObj.WHBranchCode = obj.WHBranchCode;
                cObj.InvDistGroup = obj.InvDistGroup;
                cObj.WhAllowDirectPur = obj.WhAllowDirectPur;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvWarehouses.Update(cObj);
                }
                else
                {
                    cObj.WHCode = obj.WHCode.ToUpper();
                    cObj.IsActive = true;
                    cObj.CreatedOn = DateTime.Now;
                    await _context.InvWarehouses.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateWarehouse method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateWarehouse Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
    #region GetSelectSysBranchList

    public class GetSelectSysBranch : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSysBranchListHandler : IRequestHandler<GetSelectSysBranch, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysBranch request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSysBranchList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSysBranchList method Ends----");
            return item;
        }
    }

    #endregion
    #region getSelectDistributionGroupList

    public class getSelectDistributionGroupList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectDistributionGroupListHandler : IRequestHandler<getSelectDistributionGroupList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectDistributionGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(getSelectDistributionGroupList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info getSelectDistributionGroupList method start----");
            var item = await _context.InvDistributionGroups.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.InvDistGroup, Value = e.InvDistGroup })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info getSelectDistributionGroupList method Ends----");
            return item;
        }
    }

    #endregion

    #region SingleItem

    public class GetWareHouse : IRequest<TblInvDefWarehouseDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetWareHouseHandler : IRequestHandler<GetWareHouse, TblInvDefWarehouseDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWareHouseHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefWarehouseDto> Handle(GetWareHouse request, CancellationToken cancellationToken)
        {
            var item = await _context.InvWarehouses.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblInvDefWarehouseDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion
    #region Delete
    public class DeleteWareHouse : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteWareHouseQueryHandler : IRequestHandler<DeleteWareHouse, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteWareHouseQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteWareHouse request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Class = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Class);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in delete Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
    #region WarehouseItem

    public class GetWarehouseItem : IRequest<TblInvDefWarehouseDto>
    {
        public UserIdentityDto User { get; set; }
        public string WHCODE { get; set; }
    }

    public class GetWarehouseHandler : IRequestHandler<GetWarehouseItem, TblInvDefWarehouseDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWarehouseHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefWarehouseDto> Handle(GetWarehouseItem request, CancellationToken cancellationToken)
        {

            var item = await _context.InvWarehouses.AsNoTracking()
                    .Where(e => e.WHCode == request.WHCODE)
               .ProjectTo<TblInvDefWarehouseDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;

            //TblInvDefWarehouseDto obj = new();
            //var InvItem = await _context.InvItemMaster.AsNoTracking().FirstOrDefaultAsync(e => e.ItemCode == request.WHCODE);
            //if (InvItem is not null)
            //{
            //    var ItemCode = InvItem.ItemCode;
            //    obj.ItemCode = InvItem.ItemCode;
            //    obj.HSNCode = InvItem.HSNCode;
            //    obj.ItemDescription = InvItem.ItemDescription;
            //    obj.ItemDescriptionAr = InvItem.ItemDescriptionAr;
            //    obj.ShortName = InvItem.ShortName;
            //    obj.ShortNameAr = InvItem.ShortNameAr;
            //    obj.ItemCat = InvItem.ItemCat;
            //    obj.ItemSubCat = InvItem.ItemSubCat;
            //    obj.ItemClass = InvItem.ItemClass;
            //    obj.ItemSubClass = InvItem.ItemSubClass;
            //    obj.ItemBaseUnit = InvItem.ItemBaseUnit;
            //    obj.ItemAvgCost = InvItem.ItemAvgCost;
            //    obj.ItemStandardCost = InvItem.ItemStandardCost;
            //    obj.ItemPrimeVendor = InvItem.ItemPrimeVendor;
            //    obj.ItemStandardPrice1 = InvItem.ItemStandardPrice1;
            //    obj.ItemStandardPrice2 = InvItem.ItemStandardPrice2;
            //    obj.ItemStandardPrice3 = InvItem.ItemStandardPrice3;
            //    obj.ItemType = InvItem.ItemType;
            //    obj.ItemTaxCode = InvItem.ItemTaxCode;
            //    obj.AllowPriceOverride = InvItem.AllowPriceOverride;
            //    obj.AllowDiscounts = InvItem.AllowDiscounts;
            //    obj.AllowQuantityOverride = InvItem.AllowQuantityOverride;
            //    obj.ItemTracking = InvItem.ItemTracking;
            //    obj.ItemWeight = InvItem.ItemWeight;
            //    obj.IsActive = InvItem.IsActive;
            //    obj.Id = InvItem.Id;
                

            //}
            //return obj;
        }
    }

    #endregion 
    #region GetSelectSysTypeList

    public class GetSelectSysTypeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSysTypeListHandler : IRequestHandler<GetSelectSysTypeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysTypeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSysTypeList method start----");
            var item = await _context.TblInvDefType.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TypeName, Value = e.TypeCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSysTypeList method Ends----");
            return item;
        }
    }

    #endregion 
    #region GetSelectTypeList

    public class GetSelectTypeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
        public string Code { get; set; }
    }

    public class GetSelectTypeListHandler : IRequestHandler<GetSelectTypeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectTypeList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetSelectTypeList method start----");
            var obj = _context.TblInvDefTracking.AsNoTracking();

            obj = obj.Where(e => e.TypeCode.Contains(request.Code));

            var newObj = await obj
               .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TrName, Value = e.TrCode })
                  .ToListAsync(cancellationToken);

            Log.Info("----Info GetSelectTypeList method Ends----");
            return newObj;
        }
    }

    #endregion
    #region getwarehouseDetails


    public class GetWarehouseDetials : IRequest<List<TblErpInvItemInventoryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public string Code { get; set; }
    }

    public class GetWarehouseDetialsHandler : IRequestHandler<GetWarehouseDetials, List<TblErpInvItemInventoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWarehouseDetialsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblErpInvItemInventoryDto>> Handle(GetWarehouseDetials request, CancellationToken cancellationToken)
        {
            var item = await _context.InvItemInventory.AsNoTracking()
                    .Where(e => e.WHCode == request.Code)
               .ProjectTo<TblErpInvItemInventoryDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //var search = request.Input;
            //var list = await _context.InvItemInventory.AsNoTracking()
            //    .Where(e => e.WHCode == request.Code)
            //  .Select(cl => new TblErpInvItemInventoryDto
            //  {

            //      QtyOH = cl.QtyOH,

            //  })
            //     .ToListAsync(cancellationToken);
            //return list;
            var cStatements = _context.InvItemInventory.AsNoTracking();
            cStatements = cStatements.Where(e => e.WHCode == request.Code);
            var statements = (await cStatements.ToListAsync()).GroupBy(e => new {
                e.WHCode
            }).Select(cl => new TblErpInvItemInventoryDto
            {
                QtyOH = cl.Sum(c => c.QtyOH),
            }).ToList();

            //var Citem = (await statements.ToDynamicListAsync()).Select(cl => new TblErpInvItemInventoryDto
            //{
            //    QtyOH=cl.QtyOH
            //}).ToList();

            return statements;
        }
    }

    #endregion 



    #region GetWarehouseInfoByWarehouseCode

    public class GetWarehouseInfoByWarehouseCode : IRequest<TblErpSysCompanyBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetWarehouseInfoByWarehouseCodeHandler : IRequestHandler<GetWarehouseInfoByWarehouseCode, TblErpSysCompanyBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWarehouseInfoByWarehouseCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompanyBranchDto> Handle(GetWarehouseInfoByWarehouseCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetWarehouseInfoByWarehouseCode method start----");
            try
            {
                var Branch = _context.InvWarehouses.FirstOrDefault(e => e.WHCode == request.Input).WHBranchCode;
                var item = await _context.CompanyBranches.AsNoTracking()
                   .Where(e => e.BranchCode == Branch)
                   .OrderByDescending(e => e.Id)
                  .ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
                     .FirstOrDefaultAsync(cancellationToken);
                Log.Info("----Info GetWarehouseInfoByWarehouseCode method Ends----");
                return item;

            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                return null;
            }
        }
    }

    #endregion



    #region GetWarehouseInfoByCode

    public class GetWarehouseInfoByCode : IRequest<TblInvDefWarehouse>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetWarehouseInfoByCodeHandler : IRequestHandler<GetWarehouseInfoByCode, TblInvDefWarehouse>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWarehouseInfoByCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefWarehouse> Handle(GetWarehouseInfoByCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetWarehouseInfoByCode method start----");
            try
            {
                var Warehouse = _context.InvWarehouses.FirstOrDefault(e => e.WHCode == request.Input);
                
                Log.Info("----Info GetWarehouseInfoByCode method Ends----");
                return Warehouse;

            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                return null;
            }
        }
    }

    #endregion
}
