using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.DB;
using CIN.Domain.InventorySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventoryQuery
{
    #region GetPagedList

    public class GetInventoryManagementList : IRequest<PaginatedList<TblErpInvItemMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInventoryManagementListHandler : IRequestHandler<GetInventoryManagementList, PaginatedList<TblErpInvItemMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryManagementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpInvItemMasterDto>> Handle(GetInventoryManagementList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvWarehouses.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.WHCode.Contains(search) || e.WHName.Contains(search) ||
                                e.WHBranchCode.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region GetSelectSubCategoryList

    public class GetSelectSubCategoryList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
        public string Code { get; set; }
    }

    public class GetSelectSubCategoryListHandler : IRequestHandler<GetSelectSubCategoryList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSubCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSubCategoryList request, CancellationToken cancellationToken)
        {
            //Log.Info("----Info GetSelectSubCategoryList method start----");
            //var item = await _context.InvSubCategories.AsNoTracking()
            //    .OrderByDescending(e => e.Id)
            //   .Select(e => new CustomSelectListItem { Text = e.ItemSubCatName, Value = e.ItemSubCatCode })
            //      .ToListAsync(cancellationToken);
            //Log.Info("----Info GetSelectSubCategoryList method Ends----");
            //return item;

            Log.Info("----Info GetSelectSubCategoryList method start----");
            var obj = _context.InvSubCategories.AsNoTracking();

            obj = obj.Where(e => e.ItemCatCode.Contains(request.Code));

            var newObj = await obj
               .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemSubCatName, Value = e.ItemSubCatCode })
                  .ToListAsync(cancellationToken);

            Log.Info("----Info GetSelectSubCategoryList method Ends----");
            return newObj;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreateInventoryManagementList : IRequest<AppCtrollerStringDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpInvItemMasterDto Input { get; set; }
    }

    public class CreateInventoryManagementQueryHandler : IRequestHandler<CreateInventoryManagementList, AppCtrollerStringDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryManagementQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerStringDto> Handle(CreateInventoryManagementList request, CancellationToken cancellationToken)
        {

            try
            {

                Log.Info("----Info CreateItemMaster method start----");

                var obj = request.Input;
                TblErpInvItemMaster cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.InvItemMaster.FirstOrDefaultAsync(e => e.Id == obj.Id);

                var invSetUp = await _context.InvInventoryConfigs.OrderBy(e => e.Id).FirstOrDefaultAsync();
                //var invSetUp = await _context.InvInventoryConfigs.OrderBy(e => e.Id).LastOrDefaultAsync();

                //var accCodeCount = await _context.FinMainAccounts.CountAsync(e => e.FinSubCatCode == obj.FinSubCatCode);
                var UomCode = await _context.InvUoms
                  .Select(p => p).FirstOrDefaultAsync(e => e.UOMCode.Equals("EACH"));
                var invSubCategory = await _context.InvSubCategories.FirstOrDefaultAsync(e => e.ItemSubCatCode == obj.ItemSubCat);
                //var itemCat = await _context.InvItemMaster.LastOrDefaultAsync(e => e.ItemCat == invSubCategory.ItemCatCode);
                var itemCat = await _context.InvItemMaster
                  .OrderByDescending(a => a.Id)
                  .Select(p => p).FirstOrDefaultAsync(e => e.ItemCat == invSubCategory.ItemCatCode);

                if (obj.Id > 0)
                {
                    cObj.ItemCode = obj.ItemCode;
                }
                else
                {
                    string itemCode = obj.ItemCode;

                    if (invSetUp is not null && invSetUp.AutoGenItemCode)
                    {
                        var lastCustSeq = await _context.InvCategories.OrderByDescending(e => e.LastSeq).Select(e => e.LastSeq).FirstOrDefaultAsync();
                        var invCategory = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == invSubCategory.ItemCatCode);
                        int LastSeq = lastCustSeq;

                        LastSeq = LastSeq + 1;
                        string custSeq = LastSeq.ToString(GetPrefixLen(invSetUp.ItemLength));

                        if (invSetUp.PrefixCatCode)
                            itemCode = (invCategory.ItemCatPrefix ?? string.Empty) + "" + custSeq;
                        else
                            itemCode = custSeq;

                        invCategory.LastSeq = LastSeq;
                        _context.InvCategories.Update(invCategory);
                        await _context.SaveChangesAsync();

                    }

                    cObj.ItemCode = itemCode.ToUpper();
                    cObj.CreatedOn = DateTime.Now;
                }

                //cObj.ItemCode = obj.ItemCode;
                cObj.HSNCode = obj.HSNCode;
                cObj.ItemDescription = obj.ItemDescription;
                cObj.ItemDescriptionAr = obj.ItemDescriptionAr;
                cObj.ShortName = obj.ShortName;
                cObj.ShortNameAr = obj.ShortNameAr;
                cObj.ItemCat = obj.ItemCat;
                cObj.ItemSubCat = obj.ItemSubCat;
                cObj.ItemClass = obj.ItemClass;
                cObj.ItemSubClass = obj.ItemSubClass;
                cObj.ItemBaseUnit = obj.ItemBaseUnit;
                cObj.ItemAvgCost = obj.ItemAvgCost;
                cObj.ItemStandardCost = obj.ItemStandardCost;
                cObj.ItemPrimeVendor = obj.ItemPrimeVendor;
                cObj.ItemStandardPrice1 = obj.ItemStandardPrice1;
                cObj.ItemStandardPrice2 = obj.ItemStandardPrice2;
                cObj.ItemStandardPrice3 = obj.ItemStandardPrice3;
                cObj.ItemType = obj.ItemType;
                cObj.ItemTracking = obj.ItemTracking;
                cObj.ItemWeight = obj.ItemWeight;
                cObj.ItemTaxCode = obj.ItemTaxCode;
                cObj.AllowPriceOverride = obj.AllowPriceOverride;
                cObj.AllowDiscounts = obj.AllowDiscounts;
                cObj.AllowQuantityOverride = obj.AllowQuantityOverride;
                cObj.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvItemMaster.Update(cObj);
                }
                else
                {
                    cObj.CreatedOn = DateTime.Now;
                    await _context.InvItemMaster.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                if (obj.Id == 0)
                {
                    if (UomCode != null)
                    {
                        #region UOM INSERT
                        var uoMobj = request.Input;
                        TblErpInvItemsUOM UOMcObj = new();
                        UOMcObj.ItemCode = cObj.ItemCode;
                        UOMcObj.ItemUOMFlag = 0;
                        UOMcObj.ItemUOM = UomCode.UOMCode;
                        UOMcObj.ItemConvFactor = 1;
                        UOMcObj.ItemUOMPrice1 = 0;
                        UOMcObj.ItemUOMPrice2 = 0;
                        UOMcObj.ItemUOMPrice3 = 0;
                        UOMcObj.ItemUOMDiscPer = 0;
                        UOMcObj.ItemUOMPrice4 = 0;
                        UOMcObj.ItemAvgCost = 0;
                        UOMcObj.ItemLastPOCost = 0;
                        UOMcObj.ItemLandedCost = 0;
                        UOMcObj.CreatedOn = DateTime.Now;
                        UOMcObj.IsActive = true;
                        await _context.InvItemsUOM.AddAsync(UOMcObj);
                        await _context.SaveChangesAsync();

                        #endregion
                    }
                    #region WareHouse insert
                    var WareHouse = _context.InvWarehouses.Where(c => c.IsActive == Convert.ToBoolean(true));

                    foreach (var item in WareHouse)
                    {
                        var obj1 = new TblErpInvItemInventory()
                        {
                            ItemCode = cObj.ItemCode,
                            WHCode = item.WHCode,
                            QtyOH = 0,
                            QtyOnSalesOrder = 0,
                            QtyOnPO = 0,
                            QtyReserved = 0,
                            ItemAvgCost = 0,
                            ItemLastPOCost = 0,
                            ItemLandedCost = 0,
                            MinQty = 0,
                            MaxQty = 0,
                            EOQ = 0,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        _context.InvItemInventory.Add(obj1);
                    }
                    _context.SaveChanges();
                    #endregion WareHouse
                    if (UomCode != null)
                    {
                        #region Barcode INSERT

                        var barobj = request.Input;
                        TblErpInvItemsBarcode barcodeobj = new();
                        barcodeobj.ItemCode = cObj.ItemCode;
                        barcodeobj.ItemUOMFlag = 0;
                        barcodeobj.ItemBarcode = "";
                        barcodeobj.ItemUOM = UomCode.UOMCode;
                        barcodeobj.CreatedOn = DateTime.Now;
                        barcodeobj.IsActive = true;
                        await _context.InvItemsBarcode.AddAsync(barcodeobj);
                        await _context.SaveChangesAsync();

                        #endregion
                    }

                }


                Log.Info("----Info CreateItemMaster method Exit----");
                //return ApiMessageInfo.Status(1, cObj.ItemCode);

                return ApiMessageInfo.Statusstring(1, cObj.ItemCode);


            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInventoryManagementList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Statusstring(0);
                //return (ApiMessageInfo.Failed, "0");
            }
        }
        private string GetPrefixLen(int len) => "0000000000".Substring(0, len);
    }
    #endregion
    #region GetSelectClassList

    public class GetSelectClass : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectClassListHandler : IRequestHandler<GetSelectClass, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectClassListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectClass request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectClass method start----");
            var item = await _context.InvClasses.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemClassName, Value = e.ItemClassCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectClass method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTaxList

    public class GetTaxList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTaxListHandler : IRequestHandler<GetTaxList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTaxListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTaxList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTaxList method start----");
            var item = await _context.SystemTaxes.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TaxName, Value = e.TaxCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTaxList method Ends----");
            return item;
        }
    }

    #endregion
    #region Get UOMList

    public class GetUOMList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetUOMListHandler : IRequestHandler<GetUOMList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUOMListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUOMList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetUOMList method start----");
            var item = await _context.InvUoms.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UOMName, Value = e.UOMCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetUOMList method Ends----");
            return item;
        }
    }

    #endregion
    #region getSelectSubClassList

    public class getSelectSubClassList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSubClassListHandler : IRequestHandler<getSelectSubClassList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSubClassListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(getSelectSubClassList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info getSelectSubClassList method start----");
            var item = await _context.InvSubClasses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemSubClassName, Value = e.ItemSubClassCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info getSelectSubClassList method Ends----");
            return item;
        }
    }

    #endregion

    #region SingleItem

    public class GetInventoryManagement : IRequest<TblErpInvItemMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetInventoryManagementHandler : IRequestHandler<GetInventoryManagement, TblErpInvItemMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryManagementHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpInvItemMasterDto> Handle(GetInventoryManagement request, CancellationToken cancellationToken)
        {
            var item = await _context.InvWarehouses.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion
    #region Delete
    public class DeleteInventoryManagementList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteInventoryManagementQueryHandler : IRequestHandler<DeleteInventoryManagementList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteInventoryManagementQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteInventoryManagementList request, CancellationToken cancellationToken)
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
    #region UOMTab

    public class GetUOMListByID : IRequest<TblINVTblErpInvItemsUOMDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public string Code { get; set; }
    }

    public class GetUOMListIDHandler : IRequestHandler<GetUOMListByID, TblINVTblErpInvItemsUOMDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUOMListIDHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblINVTblErpInvItemsUOMDto> Handle(GetUOMListByID request, CancellationToken cancellationToken)
        {
            TblINVTblErpInvItemsUOMDto obj = new();
            var INVUOMLISt = await _context.InvItemsUOM.AsNoTracking()
                .Where(e => request.Code == e.ItemCode)
                .Select(e => new TblErpInvItemsUOMDto
                {
                    ItemUOM = e.ItemUOM,
                    ItemConvFactor = e.ItemConvFactor,
                    ItemUOMPrice1 = e.ItemUOMPrice1,
                    ItemUOMPrice2 = e.ItemUOMPrice2,
                    ItemUOMPrice3 = e.ItemUOMPrice3,
                    ItemUOMDiscPer = e.ItemUOMDiscPer,
                    ItemUOMPrice4 = e.ItemUOMPrice4,
                    //ItemAvgCost = e.ItemAvgCost,
                    //ItemLastPOCost = e.ItemLastPOCost,
                    //ItemLandedCost = e.ItemLandedCost
                    ItemUomAvgCost = e.ItemAvgCost,
                    ItemUomLastPOCost = e.ItemLastPOCost,
                    ItemUomLandedCost = e.ItemLandedCost

                }).ToListAsync();
            obj.AuthList = INVUOMLISt;
            return obj;
        }
    }


    public class CreateUOMItem : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblINVTblErpInvItemsUOMDto Input { get; set; }
    }

    public class CreateUOMItemQueryHandler : IRequestHandler<CreateUOMItem, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUOMItemQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUOMItem request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUOMItem method start----");
                    var obj = request.Input;
                    if (request.Input.AuthList.Count() > 0)
                    {
                        var oldAuthList = await _context.InvItemsUOM.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemsUOM.RemoveRange(oldAuthList);

                        List<TblErpInvItemsUOM> UOMList = new();
                        foreach (var auth in request.Input.AuthList)
                        {
                            TblErpInvItemsUOM UOMItem = new()
                            {
                                ItemCode = obj.ItemCode.ToUpper(),
                                ItemUOMFlag = 1,
                                ItemUOM = auth.ItemUOM,
                                ItemConvFactor = auth.ItemConvFactor,
                                ItemUOMPrice1 = auth.ItemUOMPrice1,
                                ItemUOMPrice2 = auth.ItemUOMPrice2,
                                ItemUOMPrice3 = auth.ItemUOMPrice3,
                                ItemUOMDiscPer = auth.ItemUOMDiscPer,
                                ItemUOMPrice4 = auth.ItemUOMPrice4,
                                //ItemAvgCost = auth.ItemAvgCost,
                                //ItemLastPOCost = auth.ItemLastPOCost,
                                //ItemLandedCost = auth.ItemLandedCost,
                                ItemAvgCost = auth.ItemUomAvgCost,
                                ItemLastPOCost = auth.ItemUomLastPOCost,
                                ItemLandedCost = auth.ItemUomLandedCost,
                                CreatedOn = DateTime.UtcNow
                            };
                            UOMList.Add(UOMItem);
                        }
                        await _context.InvItemsUOM.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();

                        #region Barcodeinsert
                        var UomList = _context.InvItemsUOM.Where(e => e.ItemCode == obj.ItemCode);
                        var OldBarcodeList = await _context.InvItemsBarcode.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemsBarcode.RemoveRange(OldBarcodeList);
                        foreach (var item in UomList)
                        {
                            var obj1 = new TblErpInvItemsBarcode()
                            {
                                ItemCode = obj.ItemCode,
                                ItemUOMFlag = 0,
                                ItemBarcode = "",
                                ItemUOM = item.ItemUOM,
                                IsActive = true,
                                CreatedOn = DateTime.Now
                            };
                            _context.InvItemsBarcode.Add(obj1);
                        }
                        _context.SaveChanges();
                        #endregion Barcode
                    }

                    Log.Info("----Info CreateUOMItem method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUOMItem Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
    #region InventoryItemTab

    public class GetInventoryListByID : IRequest<TblInventoryItemsDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public string Code { get; set; }
    }

    public class GetInventoryListIDHandler : IRequestHandler<GetInventoryListByID, TblInventoryItemsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryListIDHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryItemsDto> Handle(GetInventoryListByID request, CancellationToken cancellationToken)
        {
            TblInventoryItemsDto obj = new();
            var LISt = await _context.InvItemInventory.AsNoTracking()
                .Where(e => request.Code == e.ItemCode)
                .Select(e => new TblErpInvItemInventoryDto
                {
                    WHCode = e.WHCode,
                    QtyOH = e.QtyOH,
                    QtyOnSalesOrder = e.QtyOnSalesOrder,
                    QtyOnPO = e.QtyOnPO,
                    QtyReserved = e.QtyReserved,
                    //ItemAvgCost = e.ItemAvgCost,
                    InvItemAvgCost = e.ItemAvgCost,
                    ItemLastPOCost = e.ItemLastPOCost,
                    ItemLandedCost = e.ItemLandedCost,
                    MinQty = e.MinQty,
                    MaxQty = e.MaxQty,
                    EOQ = e.EOQ


                }).ToListAsync();
            obj.InventoryList = LISt;
            return obj;
        }
    }

    public class CreateInventoryItem : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInventoryItemsDto Input { get; set; }
    }

    public class CreateInventoryItemQueryHandler : IRequestHandler<CreateInventoryItem, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryItemQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInventoryItem request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateInventoryItem method start----");
                    var obj = request.Input;
                    if (request.Input.InventoryList.Count() > 0)
                    {
                        var oldAuthList = await _context.InvItemInventory.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemInventory.RemoveRange(oldAuthList);

                        List<TblErpInvItemInventory> InventoryList = new();
                        foreach (var auth in request.Input.InventoryList)
                        {
                            TblErpInvItemInventory InvItem = new()
                            {
                                ItemCode = obj.ItemCode.ToUpper(),
                                WHCode = auth.WHCode,
                                QtyOH = auth.QtyOH,
                                QtyOnSalesOrder = auth.QtyOnSalesOrder,
                                QtyOnPO = auth.QtyOnPO,
                                QtyReserved = auth.QtyReserved,
                                //ItemAvgCost = auth.ItemAvgCost,
                                ItemAvgCost = auth.InvItemAvgCost,
                                ItemLastPOCost = auth.ItemLastPOCost,
                                ItemLandedCost = auth.ItemLandedCost,
                                MinQty = auth.MinQty,
                                MaxQty = auth.MaxQty,
                                EOQ = auth.EOQ,

                                CreatedOn = DateTime.UtcNow
                            };
                            InventoryList.Add(InvItem);
                        }
                        await _context.InvItemInventory.AddRangeAsync(InventoryList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateInventoryItem method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateInventoryItem Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
    #region BarcodeTab
    public class CreateBarcodeItem : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblBarcodeItemsDto Input { get; set; }
    }

    public class CreateBarcodeItemQueryHandler : IRequestHandler<CreateBarcodeItem, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateBarcodeItemQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateBarcodeItem request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateBarcodeItem method start----");
                    var obj = request.Input;
                    if (request.Input.BarCodeList.Count() > 0)
                    {
                        var oldAuthList = await _context.InvItemsBarcode.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemsBarcode.RemoveRange(oldAuthList);

                        List<TblErpInvItemsBarcode> BarCodeList = new();
                        foreach (var auth in request.Input.BarCodeList)
                        {
                            TblErpInvItemsBarcode BarCodeItem = new()
                            {
                                ItemCode = obj.ItemCode.ToUpper(),
                                ItemUOMFlag = 1,
                                //ItemUOM = auth.ItemUOM,
                                ItemUOM = auth.ItemBarUOM,
                                ItemBarcode = auth.ItemBarcode,
                                CreatedOn = DateTime.UtcNow,
                                IsActive = Convert.ToBoolean(1)
                            };
                            BarCodeList.Add(BarCodeItem);
                        }
                        await _context.InvItemsBarcode.AddRangeAsync(BarCodeList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateBarcodeItem method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateBarcodeItem Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    public class GetBarcodeListByID : IRequest<TblBarcodeItemsDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public string Code { get; set; }
    }

    public class GetBarcodeListIDHandler : IRequestHandler<GetBarcodeListByID, TblBarcodeItemsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBarcodeListIDHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblBarcodeItemsDto> Handle(GetBarcodeListByID request, CancellationToken cancellationToken)
        {
            TblBarcodeItemsDto obj = new();
            var LISt = await _context.InvItemsBarcode.AsNoTracking()
                .Where(e => request.Code == e.ItemCode)
                .Select(e => new TblErpInvItemsBarcodeDto
                {
                    ItemCode = e.ItemCode,
                    ItemUOMFlag = e.ItemUOMFlag,
                    ItemBarcode = e.ItemBarcode,
                    //ItemUOM = e.ItemUOM
                    ItemBarUOM = e.ItemUOM

                }).ToListAsync();
            obj.BarCodeList = LISt;
            return obj;
        }
    }
    public class GetBarcode : IRequest<TblBarcodeItemsDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public string Code { get; set; }
    }

    public class GetBarcodeListHandler : IRequestHandler<GetBarcode, TblBarcodeItemsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBarcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblBarcodeItemsDto> Handle(GetBarcode request, CancellationToken cancellationToken)
        {
            var item = await _context.InvItemsBarcode.AsNoTracking()
                    .Where(e => e.ItemBarcode == request.Code)
               .ProjectTo<TblBarcodeItemsDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }
    #endregion
    #region NotesTab
    public class CreateNotesItem : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblNotesItemsDto Input { get; set; }
    }

    public class CreateNotesItemQueryHandler : IRequestHandler<CreateNotesItem, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateNotesItemQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateNotesItem request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateNotesItem method start----");
                    var obj = request.Input;
                    if (request.Input.NotesList.Count() > 0)
                    {
                        var oldAuthList = await _context.InvItemNotes.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemNotes.RemoveRange(oldAuthList);

                        List<TblErpInvItemNotes> NotesList = new();
                        foreach (var auth in request.Input.NotesList)
                        {
                            TblErpInvItemNotes NotesItem = new()
                            {
                                ItemCode = obj.ItemCode.ToUpper(),
                                Name = auth.Name,
                                NoteDates = auth.NoteDates,
                                Notes = auth.Notes,
                                CreatedOn = DateTime.UtcNow,
                                IsActive = Convert.ToBoolean(1)
                            };
                            NotesList.Add(NotesItem);
                        }
                        await _context.InvItemNotes.AddRangeAsync(NotesList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateNotesItem method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateNotesItem Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
    #region ItemHistoryTab
    public class CreateItemHistory : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHistoryItemsDto Input { get; set; }
    }

    public class CreateItemHistoryQueryHandler : IRequestHandler<CreateItemHistory, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateItemHistoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateItemHistory request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateItemHistory method start----");
                    var obj = request.Input;
                    if (request.Input.HistoryList.Count() > 0)
                    {
                        var oldAuthList = await _context.InvItemInventoryHistory.Where(e => e.ItemCode == obj.ItemCode).ToListAsync();
                        _context.InvItemInventoryHistory.RemoveRange(oldAuthList);

                        List<TblErpInvItemInventoryHistory> HistoryList = new();
                        foreach (var auth in request.Input.HistoryList)
                        {
                            TblErpInvItemInventoryHistory HistoryItem = new()
                            {
                                ItemCode = obj.ItemCode.ToUpper(),
                                WHCode = auth.WHCode,
                                TranDate = auth.TranDate,
                                TranType = auth.TranType,
                                TranNumber = auth.TranNumber,
                                TranUnit = auth.TranUnit,
                                TranQty = auth.TranQty,
                                unitConvFactor = auth.unitConvFactor,
                                TranTotQty = auth.TranTotQty,
                                TranPrice = auth.TranPrice,
                                ItemAvgCost = auth.ItemAvgCost,
                                TranRemarks = auth.TranRemarks,
                                CreatedOn = DateTime.UtcNow,
                                IsActive = Convert.ToBoolean(1)
                            };
                            HistoryList.Add(HistoryItem);
                        }
                        await _context.InvItemInventoryHistory.AddRangeAsync(HistoryList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateItemHistory method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateItemHistory Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
    #region SingleItem

    public class GetItemGenerate : IRequest<ItemGenerateNumberDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GettemGerateHandler : IRequestHandler<GetItemGenerate, ItemGenerateNumberDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GettemGerateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ItemGenerateNumberDto> Handle(GetItemGenerate request, CancellationToken cancellationToken)
        {
            var invSetUp = await _context.InvInventoryConfigs
                  .FirstOrDefaultAsync();
            //var invCategory = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == request.CatCode);
            var invItem = await _context.InvItemMaster
                  .OrderByDescending(a => a.Id)
                  .Select(p => p).FirstOrDefaultAsync();
            //var invItem = await _context.InvItemMaster.AsNoTracking().ToListAsync();
            int ItemNo = 0000;
            List<ItemGenerateNumberDto> ItemList = new();
            if (invSetUp != null)
            {
                if (invSetUp.AutoGenItemCode)
                {
                    if (invItem != null)
                    {
                        ItemNo = Convert.ToInt32(invItem.ItemCode) + Convert.ToInt32(1);
                        ItemList.Add(new ItemGenerateNumberDto { ItemCode = ItemNo });
                    }
                    else
                    {
                        int lens = Convert.ToInt32(invSetUp.ItemLength) - Convert.ToInt32(1);
                        ItemNo = Convert.ToInt32(GetPrefixLen(lens)) + Convert.ToInt32(1);
                        ItemList.Add(new ItemGenerateNumberDto { ItemCode = ItemNo });
                    }




                }
            }
            //var currentUserId = user.Claims.ToList().FirstOrDefault(x => x.Type == "id").Value;
            //var userProfile = (await _context.GetUserByIds.FromSqlInterpolated($"Exec sp_InvDefSubClass @userId = {currentUserId}").ToListAsync()).FirstOrDefault();
            //var userProfile = (await _context.InvSubClasses.FromSqlInterpolated($"Exec Sp_GenerateItemNO").ToListAsync()).FirstOrDefault();
            //var itemNo = (await _context.InvSubClasses.FromSqlInterpolated($"Exec Sp_GenerateItemNO").ToListAsync()).FirstOrDefault();

            return new ItemGenerateNumberDto { ItemCode = ItemNo }; ;
        }
        private string GetPrefixLen(int len) => "0000000000".Substring(0, len);
    }

    #endregion
    #region GetPagedList

    public class GetInventoryItemList : IRequest<PaginatedList<TblErpInvItemMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetINventoryItemListHandler : IRequestHandler<GetInventoryItemList, PaginatedList<TblErpInvItemMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetINventoryItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpInvItemMasterDto>> Handle(GetInventoryItemList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            //var invItem = await _context.InvItemMaster
            //     .Select(p => p)
            //     .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            var list = await _context.InvItemMaster.AsNoTracking()
              .Where(e =>
                            (e.ItemCode.Contains(search) || e.HSNCode.Contains(search)
                                || e.ItemDescription.Contains(search) || e.ItemDescriptionAr.Contains(search) ||
                                e.ShortName.Contains(search) || e.ShortNameAr.Contains(search) ||
                                e.ItemCat.Contains(search) || e.ItemSubCat.Contains(search) ||
                                e.ItemClass.Contains(search) || e.ItemSubClass.Contains(search) ||
                                e.ItemType.Contains(search) || e.ItemTracking.Contains(search) ||
                                e.ItemWeight.Contains(search) || e.ItemTaxCode.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region SingleItem

    public class GetItemMaster : IRequest<TblEditInventoryItemDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetItemMasterHandler : IRequestHandler<GetItemMaster, TblEditInventoryItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblEditInventoryItemDto> Handle(GetItemMaster request, CancellationToken cancellationToken)
        {
            //var item = await _context.InvItemMaster.AsNoTracking()
            //        .Where(e => e.Id == request.Id)
            //   .ProjectTo<TblInventoryItemsDto>(_mapper.ConfigurationProvider)
            //      .FirstOrDefaultAsync(cancellationToken);
            //return item;
            TblEditInventoryItemDto obj = new();
            var InvItem = await _context.InvItemMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (InvItem is not null)
            {
                var ItemCode = InvItem.ItemCode;

                var iteminventory = await _context.InvItemInventory.AsNoTracking()
                    .Where(e => ItemCode == e.ItemCode)
                    .Select(e => new TblErpInvItemInventoryDto
                    {
                        WHCode = e.WHCode,
                        QtyOH = e.QtyOH,
                        QtyOnSalesOrder = e.QtyOnSalesOrder,
                        QtyOnPO = e.QtyOnPO,
                        QtyReserved = e.QtyReserved,
                        //ItemAvgCost = e.ItemAvgCost,
                        InvItemAvgCost = e.ItemAvgCost,
                        ItemLastPOCost = e.ItemLastPOCost,
                        ItemLandedCost = e.ItemLandedCost,
                        MinQty = e.MinQty,
                        MaxQty = e.MaxQty,
                        EOQ = e.EOQ

                    }).ToListAsync();

                var itemuom = await _context.InvItemsUOM.AsNoTracking()
                  .Where(e => ItemCode == e.ItemCode)
                  .Select(e => new TblErpInvItemsUOMDto
                  {
                      ItemUOMFlag = e.ItemUOMFlag,
                      ItemUOM = e.ItemUOM,
                      ItemConvFactor = e.ItemConvFactor,
                      ItemUOMPrice1 = e.ItemUOMPrice1,
                      ItemUOMPrice2 = e.ItemUOMPrice2,
                      ItemUOMPrice3 = e.ItemUOMPrice3,
                      ItemUOMDiscPer = e.ItemUOMDiscPer,
                      ItemUOMPrice4 = e.ItemUOMPrice4,
                      //ItemAvgCost = e.ItemLastPOCost,
                      //ItemLastPOCost = e.ItemLastPOCost,
                      //ItemLandedCost = e.ItemLandedCost
                      ItemUomAvgCost = e.ItemLastPOCost,
                      ItemUomLastPOCost = e.ItemLastPOCost,
                      ItemUomLandedCost = e.ItemLandedCost

                  }).ToListAsync();

                var itembarcode = await _context.InvItemsBarcode.AsNoTracking()
                 .Where(e => ItemCode == e.ItemCode)
                 .Select(e => new TblErpInvItemsBarcodeDto
                 {
                     ItemUOMFlag = e.ItemUOMFlag,
                     ItemBarcode = e.ItemBarcode,
                     //ItemUOM = e.ItemUOM
                     ItemBarUOM = e.ItemUOM

                 }).ToListAsync();

                var itemnotes = await _context.InvItemNotes.AsNoTracking()
                .Where(e => ItemCode == e.ItemCode)
                .Select(e => new TblErpInvItemNotesDto
                {
                    Name = e.Name,
                    Notes = e.Notes,
                    NoteDates = e.NoteDates


                }).ToListAsync();

                var cStatements = _context.InvItemInventoryHistory.AsNoTracking();
                cStatements = cStatements.Where(e => e.ItemCode == ItemCode && e.TranDate == DateTime.Now.AddMonths(-1));
                //cStatements = cStatements.Where(e => DateTime.Parse(e.TranDate).ToString("dd-MM-yyyy") == DateTime.Now.AddMonths(-1));
                var statements = (await cStatements.ToListAsync()).GroupBy(e => new
                {
                    e.WHCode
                }).Select(cl => new TblErpInvItemInventoryHistoryDto
                {
                    WHCode = cl.First().WHCode,
                    //TranDate=DateTime.Now,
                    TranType = "",
                    TranNumber = "",
                    TranUnit = "",
                    TranQty = cl.Sum(c => c.TranQty),
                    unitConvFactor = cl.Sum(c => c.unitConvFactor),
                    TranTotQty = cl.Sum(c => c.TranTotQty),
                    TranPrice = cl.Sum(c => c.TranPrice),
                    ItemAvgCost = cl.Sum(c => c.ItemAvgCost),
                    TranRemarks = "Opening Balance",
                }).ToList();





                var itemhstry = await _context.InvItemInventoryHistory.AsNoTracking()
          .Where(e => ItemCode == e.ItemCode)
          .Select(e => new TblErpInvItemInventoryHistoryDto
          {
              WHCode = e.WHCode,
              TranDate = e.TranDate,
              TranType = e.TranType,
              TranNumber = e.TranNumber,
              TranUnit = e.TranUnit,
              TranQty = e.TranQty,
              unitConvFactor = e.unitConvFactor,
              TranTotQty = e.TranTotQty,
              TranPrice = e.TranPrice,
              ItemAvgCost = e.ItemAvgCost,
              TranRemarks = e.TranRemarks,
          }).ToListAsync();

                var itemhistory = statements.Union(itemhstry);


                var Citem = (await itemhistory.ToDynamicListAsync()).Select(cl => new TblErpInvItemInventoryHistoryDto
                {
                    WHCode = cl.WHCode,
                    TranDate = cl.TranDate,
                    TranType = cl.TranType,
                    TranNumber = cl.TranNumber,
                    TranUnit = cl.TranUnit,
                    TranQty = cl.TranQty,
                    unitConvFactor = cl.unitConvFactor,
                    TranTotQty = cl.TranTotQty,
                    TranPrice = cl.TranPrice,
                    ItemAvgCost = cl.ItemAvgCost,
                    TranRemarks = cl.TranRemarks,
                }).ToList();


                obj.ItemCode = InvItem.ItemCode;
                obj.HSNCode = InvItem.HSNCode;
                obj.ItemDescription = InvItem.ItemDescription;
                obj.ItemDescriptionAr = InvItem.ItemDescriptionAr;
                obj.ShortName = InvItem.ShortName;
                obj.ShortNameAr = InvItem.ShortNameAr;
                obj.ItemCat = InvItem.ItemCat;
                obj.ItemSubCat = InvItem.ItemSubCat;
                obj.ItemClass = InvItem.ItemClass;
                obj.ItemSubClass = InvItem.ItemSubClass;
                obj.ItemBaseUnit = InvItem.ItemBaseUnit;
                obj.ItemAvgCost = InvItem.ItemAvgCost;
                obj.ItemStandardCost = InvItem.ItemStandardCost;
                obj.ItemPrimeVendor = InvItem.ItemPrimeVendor;
                obj.ItemStandardPrice1 = InvItem.ItemStandardPrice1;
                obj.ItemStandardPrice2 = InvItem.ItemStandardPrice2;
                obj.ItemStandardPrice3 = InvItem.ItemStandardPrice3;
                obj.ItemType = InvItem.ItemType;
                obj.ItemTaxCode = InvItem.ItemTaxCode;
                obj.AllowPriceOverride = InvItem.AllowPriceOverride;
                obj.AllowDiscounts = InvItem.AllowDiscounts;
                obj.AllowQuantityOverride = InvItem.AllowQuantityOverride;
                obj.ItemTracking = InvItem.ItemTracking;
                obj.ItemWeight = InvItem.ItemWeight;
                obj.IsActive = InvItem.IsActive;
                obj.Id = InvItem.Id;
                obj.InventoryList = iteminventory;
                obj.AuthList = itemuom;
                obj.BarcodeList = itembarcode;
                obj.NotesList = itemnotes;
                //obj.HistoryList = itemhistory;
                obj.HistoryList = Citem;

            }
            return obj;
        }
    }

    #endregion
    #region Delete
    public class DeleteItemMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteItemQueryHandler : IRequestHandler<DeleteItemMaster, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteItemQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteItemMaster request, CancellationToken cancellationToken)
        {
            //try
            //{
            //    Log.Info("----Info delte method start----");
            //    Log.Info("----Info delete method end----");

            //    if (request.Id > 0)
            //    {
            //        var Class = await _context.InvItemMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
            //        _context.Remove(Class);
            //        await _context.SaveChangesAsync();
            //        return request.Id;
            //    }
            //    return 0;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Error in delete Method");
            //    Log.Error("Error occured time : " + DateTime.UtcNow);
            //    Log.Error("Error message : " + ex.Message);
            //    Log.Error("Error StackTrace : " + ex.StackTrace);
            //    return 0;
            //}
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info delte method start----");

                    if (request.Id > 0)
                    {
                        var Inv = await _context.InvItemMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.Remove(Inv);
                        var ItemInventory = await _context.InvItemInventory.Where(e => e.ItemCode == Inv.ItemCode).ToListAsync();
                        _context.InvItemInventory.RemoveRange(ItemInventory);

                        var ItemBarcode = await _context.InvItemsBarcode.Where(e => e.ItemCode == Inv.ItemCode).ToListAsync();
                        _context.InvItemsBarcode.RemoveRange(ItemBarcode);

                        var ItemNotes = await _context.InvItemNotes.Where(e => e.ItemCode == Inv.ItemCode).ToListAsync();
                        _context.InvItemNotes.RemoveRange(ItemNotes);

                        var ItemInvHistory = await _context.InvItemInventoryHistory.Where(e => e.ItemCode == Inv.ItemCode).ToListAsync();
                        _context.InvItemInventoryHistory.RemoveRange(ItemInvHistory);

                        var ItemUOM = await _context.InvItemsUOM.Where(e => e.ItemCode == Inv.ItemCode).ToListAsync();
                        _context.InvItemsUOM.RemoveRange(ItemUOM);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return request.Id;

                        //var Branch = await _context.FinAccountBranches.FirstOrDefaultAsync(e => e.Id == request.Id);
                        //_context.Remove(Branch);
                        //var oldAuthList = await _context.FinBranchesAuthorities.Where(e => e.FinBranchCode == Branch.FinBranchCode).ToListAsync();
                        //_context.FinBranchesAuthorities.RemoveRange(oldAuthList);
                        //await _context.SaveChangesAsync();
                        //await transaction.CommitAsync();
                        //return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in delete Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }
    }

    #endregion
    #region GetInventoryItem

    public class GetInventoryItem : IRequest<TblEditInventoryItemDto>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }
        public string CheckCode { get; set; }
    }

    public class GetInvItemHandler : IRequestHandler<GetInventoryItem, TblEditInventoryItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInvItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblEditInventoryItemDto> Handle(GetInventoryItem request, CancellationToken cancellationToken)
        {
            TblEditInventoryItemDto obj = new();
            var InvItem = await _context.InvItemMaster.AsNoTracking().FirstOrDefaultAsync(e => e.ItemCode == request.ItemCode);
            if (InvItem is not null)
            {
                if (request.CheckCode.HasValue())
                {
                    obj.Id = 1;
                    return obj;
                }

                var ItemCode = InvItem.ItemCode;

                var iteminventory = await _context.InvItemInventory.AsNoTracking()
                    .Where(e => ItemCode == e.ItemCode)
                    .Select(e => new TblErpInvItemInventoryDto
                    {
                        WHCode = e.WHCode,
                        QtyOH = e.QtyOH,
                        QtyOnSalesOrder = e.QtyOnSalesOrder,
                        QtyOnPO = e.QtyOnPO,
                        QtyReserved = e.QtyReserved,
                        //ItemAvgCost = e.ItemAvgCost,
                        InvItemAvgCost = e.ItemAvgCost,
                        ItemLastPOCost = e.ItemLastPOCost,
                        ItemLandedCost = e.ItemLandedCost,
                        MinQty = e.MinQty,
                        MaxQty = e.MaxQty,
                        EOQ = e.EOQ

                    }).ToListAsync();

                var itemuom = await _context.InvItemsUOM.AsNoTracking()
                  .Where(e => ItemCode == e.ItemCode)
                  .Select(e => new TblErpInvItemsUOMDto
                  {
                      ItemUOMFlag = e.ItemUOMFlag,
                      ItemUOM = e.ItemUOM,
                      ItemConvFactor = e.ItemConvFactor,
                      ItemUOMPrice1 = e.ItemUOMPrice1,
                      ItemUOMPrice2 = e.ItemUOMPrice2,
                      ItemUOMPrice3 = e.ItemUOMPrice3,
                      ItemUOMDiscPer = e.ItemUOMDiscPer,
                      ItemUOMPrice4 = e.ItemUOMPrice4,
                      //ItemAvgCost = e.ItemLastPOCost,
                      //ItemLastPOCost = e.ItemLastPOCost,
                      //ItemLandedCost = e.ItemLandedCost
                      ItemUomAvgCost = e.ItemLastPOCost,
                      ItemUomLastPOCost = e.ItemLastPOCost,
                      ItemUomLandedCost = e.ItemLandedCost

                  }).ToListAsync();

                var itembarcode = await _context.InvItemsBarcode.AsNoTracking()
                 .Where(e => ItemCode == e.ItemCode)
                 .Select(e => new TblErpInvItemsBarcodeDto
                 {
                     ItemUOMFlag = e.ItemUOMFlag,
                     ItemBarcode = e.ItemBarcode,
                     //ItemUOM = e.ItemUOM
                     ItemBarUOM = e.ItemUOM

                 }).ToListAsync();

                var itemnotes = await _context.InvItemNotes.AsNoTracking()
                .Where(e => ItemCode == e.ItemCode)
                .Select(e => new TblErpInvItemNotesDto
                {
                    Name = e.Name,
                    Notes = e.Notes,
                    NoteDates = e.NoteDates
                }).ToListAsync();

                var itemhistory = await _context.InvItemInventoryHistory.AsNoTracking()
          .Where(e => ItemCode == e.ItemCode)
          .Select(e => new TblErpInvItemInventoryHistoryDto
          {
              WHCode = e.WHCode,
              TranDate = e.TranDate,
              TranType = e.TranType,
              TranNumber = e.TranNumber,
              TranUnit = e.TranUnit,
              TranQty = e.TranQty,
              unitConvFactor = e.unitConvFactor,
              TranTotQty = e.TranTotQty,
              TranPrice = e.TranPrice,
              ItemAvgCost = e.ItemAvgCost,
              TranRemarks = e.TranRemarks,


          }).ToListAsync();

                obj.ItemCode = InvItem.ItemCode;
                obj.HSNCode = InvItem.HSNCode;
                obj.ItemDescription = InvItem.ItemDescription;
                obj.ItemDescriptionAr = InvItem.ItemDescriptionAr;
                obj.ShortName = InvItem.ShortName;
                obj.ShortNameAr = InvItem.ShortNameAr;
                obj.ItemCat = InvItem.ItemCat;
                obj.ItemSubCat = InvItem.ItemSubCat;
                obj.ItemClass = InvItem.ItemClass;
                obj.ItemSubClass = InvItem.ItemSubClass;
                obj.ItemBaseUnit = InvItem.ItemBaseUnit;
                obj.ItemAvgCost = InvItem.ItemAvgCost;
                obj.ItemStandardCost = InvItem.ItemStandardCost;
                obj.ItemPrimeVendor = InvItem.ItemPrimeVendor;
                obj.ItemStandardPrice1 = InvItem.ItemStandardPrice1;
                obj.ItemStandardPrice2 = InvItem.ItemStandardPrice2;
                obj.ItemStandardPrice3 = InvItem.ItemStandardPrice3;
                obj.ItemType = InvItem.ItemType;
                obj.ItemTaxCode = InvItem.ItemTaxCode;
                obj.AllowPriceOverride = InvItem.AllowPriceOverride;
                obj.AllowDiscounts = InvItem.AllowDiscounts;
                obj.AllowQuantityOverride = InvItem.AllowQuantityOverride;
                obj.ItemTracking = InvItem.ItemTracking;
                obj.ItemWeight = InvItem.ItemWeight;
                obj.IsActive = InvItem.IsActive;
                obj.Id = InvItem.Id;
                obj.InventoryList = iteminventory;
                obj.AuthList = itemuom;
                obj.BarcodeList = itembarcode;
                obj.NotesList = itemnotes;
                obj.HistoryList = itemhistory;

            }
            return obj;
        }
    }

    #endregion

    #region InventoryHistory

    public class InventoryHistory : IRequest<TblHistoryItemsDto>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCodes { get; set; }
    }

    public class InventoryHistoryHandler : IRequestHandler<InventoryHistory, TblHistoryItemsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public InventoryHistoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblHistoryItemsDto> Handle(InventoryHistory request, CancellationToken cancellationToken)
        {
            TblHistoryItemsDto obj = new();
            var itemcode = await _context.InvItemMaster.AsNoTracking().FirstOrDefaultAsync(e => e.ItemCode == request.ItemCodes);
            //TblErpInvItemInventoryHistoryDto obj = new();


            var iteminventory = await _context.InvItemInventoryHistory.AsNoTracking()
                    .Where(e => itemcode.ItemCode == e.ItemCode)
                    .Select(e => new TblErpInvItemInventoryHistoryDto
                    {
                        ItemCode = e.ItemCode,
                        WHCode = e.WHCode,
                        TranDate = e.TranDate,
                        TranNumber = e.TranNumber,
                        TranUnit = e.TranUnit,
                        TranQty = e.TranQty,
                        unitConvFactor = e.unitConvFactor,
                        TranTotQty = e.TranTotQty,
                        TranPrice = e.TranPrice,
                        ItemAvgCost = e.ItemAvgCost,
                        TranRemarks = e.TranRemarks
                    }).ToListAsync();

            obj.HistoryList = iteminventory;

            return obj;
        }
    }

    #endregion
}
