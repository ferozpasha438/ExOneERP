using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryMgtDtos;
//using CIN.Application.PurchaseSetupDtos;
//using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.InventoryMgt;
using CIN.Domain.InventorySetup;
//using CIN.Domain.PurchaseMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventorymgtQuery
{
    #region GetStockReconcilationUserSelectList

    public class GetStockReconcilationUserSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationUserListHandler : IRequestHandler<GetStockReconcilationUserSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationUserListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationUserSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetUserSelectList method start----");
            var item = await _context.SystemLogins.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UserName, Value = e.UserName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetUserSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationToLocationList

    public class GetStockReconcilationToLocationList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationToLocationListHandler : IRequestHandler<GetStockReconcilationToLocationList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationToLocationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationToLocationList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationToLocationList method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationToLocationList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationAccountSelectList

    public class GetStockReconcilationAccountSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationAccountsListHandler : IRequestHandler<GetStockReconcilationAccountSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationAccountSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationAccountSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationAccountSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationJVNumberSelectList

    public class GetStockReconcilationJVNumberSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationJvNumberListHandler : IRequestHandler<GetStockReconcilationJVNumberSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationJvNumberListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationJVNumberSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationJVNumberSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationJVNumberSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationBarCodeSelectList

    public class GetStockReconcilationBarCodeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationBarcodeListHandler : IRequestHandler<GetStockReconcilationBarCodeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationBarcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationBarCodeSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationBarCodeSelectList method start----");
            var item = await _context.InvItemsBarcode.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemBarcode, Value = e.ItemCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationBarCodeSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationItemCodeList

    public class GetStockReconcilationItemCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationItemCodeListHandler : IRequestHandler<GetStockReconcilationItemCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationItemCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationItemCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationItemCodeList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemCode, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationItemCodeList method Ends----");
            return item;
        }
    }

    #endregion
    #region StockReconcilationProductUomtPriceItem

    public class StockReconcilationProductUomtPriceItem : IRequest<StockReconcilationProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class StockReconcilationProductUOMFactorItemHandler : IRequestHandler<StockReconcilationProductUomtPriceItem, StockReconcilationProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StockReconcilationProductUOMFactorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StockReconcilationProductUnitPriceDTO> Handle(StockReconcilationProductUomtPriceItem request, CancellationToken cancellationToken)
        {
            var itemcode = request.ItemList.Split('_')[0];
            var ItemUOM = request.ItemList.Split('_')[1];

            Log.Info("----Info StockReconcilationProductUomtPriceItem method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                 .Where(e =>
                            (e.ItemCode.Contains(itemcode) && e.ItemUOM.Contains(ItemUOM)
                             ))
               .Select(Product => new StockReconcilationProductUnitPriceDTO
               {
                   tranItemCode = Product.ItemCode,
                   tranItemUomFactor = Product.ItemConvFactor,
                   ItemAvgcost = Product.ItemAvgCost,

               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info StockReconcilationProductUomtPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationUOMList

    public class GetStockReconcilationUOMSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationUOMselectListHandler : IRequestHandler<GetStockReconcilationUOMSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationUOMselectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationUOMSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationUOMList method start----");
            var item = await _context.InvUoms.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UOMName, Value = e.UOMCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationUOMList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetStockReconcilationItemNameList

    public class GetStockReconcilationItemNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStockReconcilationItemNameListHandler : IRequestHandler<GetStockReconcilationItemNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationItemNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStockReconcilationItemNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStockReconcilationItemNameList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetStockReconcilationItemNameList method Ends----");
            return item;
        }
    }

    #endregion
    #region ProductUnitPriceItem

    public class StockReconcilationProductUnitPriceItem : IRequest<StockReconcilationProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Itemcode { get; set; }
    }

    public class StockReconcilationProductUnitPriceItemHandler : IRequestHandler<StockReconcilationProductUnitPriceItem, StockReconcilationProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StockReconcilationProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StockReconcilationProductUnitPriceDTO> Handle(StockReconcilationProductUnitPriceItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info StockReconcilationProductUnitPriceItem method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .Where(e => e.ItemCode == request.Itemcode)
                 .Include(e => e.InvUoms)
               .Select(Product => new StockReconcilationProductUnitPriceDTO
               {

                   tranItemCode = Product.ItemCode,
                   tranItemName = Product.ShortName,
                   tranItemUnitCode = Product.ItemBaseUnit,
                   tranItemCost = Product.ItemAvgCost,


               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info StockReconcilationProductUnitPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region StockReconcilationCreateRequest
    public class StockReconcilationCreateRequest : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblStockReconcilationInventoryReturntDto Input { get; set; }
    }

    public class StockReconcilationCreateIssuesQueryHandler : IRequestHandler<StockReconcilationCreateRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public StockReconcilationCreateIssuesQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(StockReconcilationCreateRequest request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateStockReconcilationRequest method start----");
                    var transnumber = string.Empty;
                    var obj = request.Input;
                    TblIMStockReconciliationTransactionHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.IMStockReconciliationTransactionHeader.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                    }
                    else
                    {

                        var IMheader = await _context.IMStockReconciliationTransactionHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                        if (IMheader != null)
                            transnumber = Convert.ToString(int.Parse(IMheader.TranNumber) + 1);
                        else
                            transnumber = Convert.ToString(10001);
                    }

                    cObj.TranNumber = transnumber;
                    cObj.TranDate = DateTime.Now;
                    cObj.TranUser = obj.TranUser;
                    cObj.TranLocation = obj.TranLocation;
                    cObj.TranToLocation = obj.TranToLocation;
                    cObj.TranDocNumber = obj.TranDocNumber;
                    cObj.TranReference = obj.TranReference;
                    cObj.TranType = obj.TranType;
                    cObj.TranTotalCost = obj.TranTotalCost;
                    cObj.TranTotItems = obj.TranTotItems;
                    cObj.CreatedOn = DateTime.Now;
                    cObj.IsActive = true;
                    cObj.TranCreateDate = DateTime.Now;
                    cObj.TranCreateUser = request.User.UserId.ToString();
                    cObj.TranRemarks = obj.TranRemarks;
                    cObj.TranInvAccount = obj.TranInvAccount;
                    cObj.TranInvAdjAccount = obj.TranInvAdjAccount;
                    cObj.JVNum = obj.JVNum;

                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        //cObj.Id = 0;
                        cObj.TranLastEditDate = DateTime.Now;
                        cObj.TranLastEditUser = request.User.UserId.ToString();
                        _context.IMStockReconciliationTransactionHeader.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.IMStockReconciliationTransactionHeader.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.IMStockReconciliationTransactionDetails.Where(e => e.TranNumber == transnumber).ToListAsync();
                        _context.IMStockReconciliationTransactionDetails.RemoveRange(oldAuthList);
                        List<TblIMStockReconciliationTransactionDetails> UOMList = new();
                        int i = 1;
                        string trans = "1";
                        var PoDetialTransNumber = await _context.IMStockReconciliationTransactionDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var auth in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.SNo) + i++);
                            else
                                trans = Convert.ToString(int.Parse("0") + i++);

                            TblIMStockReconciliationTransactionDetails UOMItem = new()
                            {
                                SNo = trans.ToString(),
                                TranNumber = transnumber,
                                TranDate = DateTime.UtcNow,
                                TranLocation = obj.TranLocation,
                                TranToLocation = obj.TranToLocation,
                                TranType = obj.TranType,
                                TranItemCode = auth.TranItemCode,
                                TranBarcode = auth.TranBarcode,
                                TranItemName = auth.TranItemName,
                                TranItemName2 = auth.TranItemName2,
                                TranItemQty = auth.TranItemQty,
                                TranItemUnit = auth.TranItemUnit,
                                TranUOMFactor = auth.TranUOMFactor,
                                TranItemCost = auth.TranItemCost,
                                ItemAttribute1 = auth.ItemAttribute1,
                                ItemAttribute2 = auth.ItemAttribute2,
                                Remarks = auth.Remarks,
                                INVAcc = auth.INVAcc,
                                INVADJAcc = auth.INVADJAcc,
                                IsActive = true,
                                CreatedOn = DateTime.UtcNow,
                                TranTotCost = auth.TranTotCost
                            };
                            UOMList.Add(UOMItem);
                        }
                        await _context.IMStockReconciliationTransactionDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();

                    }

                    Log.Info("----Info CreateAjustmentsRequest method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateStockReconcilationRequest Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }

        }
        private string GetPrefixLen(int len) => "0000000000".Substring(0, len);
    }

    #endregion
    #region GetPagedList

    public class GetIMStockReconcilationTransactionList : IRequest<PaginatedList<TblIMStockReconciliationTransactionHeaderDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetStockIMTransactionListHandler : IRequestHandler<GetIMStockReconcilationTransactionList, PaginatedList<TblIMStockReconciliationTransactionHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockIMTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblIMStockReconciliationTransactionHeaderDto>> Handle(GetIMStockReconcilationTransactionList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.IMStockReconciliationTransactionHeader.AsNoTracking()
              .Where(e =>
                            (e.TranNumber.Contains(search) || e.TranReference.Contains(search)

                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblIMStockReconciliationTransactionHeaderDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;


        }

    }

    #endregion
    #region SingleItem

    public class GetStockReconcilationIMDetails : IRequest<TblStockReconcilationInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetStockReconcilationIMDetailsHandler : IRequestHandler<GetStockReconcilationIMDetails, TblStockReconcilationInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStockReconcilationIMDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblStockReconcilationInventoryReturntDto> Handle(GetStockReconcilationIMDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.IMStockReconciliationTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblStockReconcilationInventoryReturntDto obj = new();
            var IMHeader = await _context.IMStockReconciliationTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (IMHeader is not null)
            {
                var Transid = IMHeader.TranNumber;

                var iteminventory1 = await _context.IMStockReconciliationTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMStockReconciliationTransactionDetailsDto
                    {
                        TranItemCode = e.TranItemCode,
                        TranBarcode = e.TranBarcode,
                        TranItemName = e.TranItemName,
                        TranItemName2 = e.TranItemName2,
                        TranItemQty = e.TranItemQty,
                        TranItemUnit = e.TranItemUnit,
                        TranUOMFactor = e.TranUOMFactor,
                        TranItemCost = e.TranItemCost,
                        TranTotCost = e.TranTotCost,
                        ItemAttribute1 = e.ItemAttribute1,
                        ItemAttribute2 = e.ItemAttribute2,
                        Remarks = e.Remarks,
                        INVAcc = e.INVAcc,
                        INVADJAcc = e.INVADJAcc


                    }).ToListAsync();

                obj.TranNumber = IMHeader.TranNumber;
                obj.TranDate = Convert.ToDateTime(IMHeader.TranDate.ToShortDateString());
                obj.TranUser = IMHeader.TranUser;
                obj.TranLocation = IMHeader.TranLocation;
                obj.TranToLocation = IMHeader.TranToLocation;
                obj.TranDocNumber = IMHeader.TranDocNumber;
                obj.TranReference = IMHeader.TranReference;
                obj.TranType = IMHeader.TranType;
                obj.TranTotalCost = IMHeader.TranTotalCost;
                obj.TranTotItems = IMHeader.TranTotItems;
                obj.TranRemarks = IMHeader.TranRemarks;
                obj.TranInvAccount = IMHeader.TranInvAccount;
                obj.TranInvAdjAccount = IMHeader.TranInvAdjAccount;
                obj.JVNum = IMHeader.JVNum;
                obj.itemList = iteminventory1;
                obj.Id = IMHeader.Id;

            }
            return obj;
        }
    }

    #endregion
   
    #region StockReconcilationDelete
    public class StockReconcilationDeleteIMList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class StockReconcilationDeleteIMQueryHandler : IRequestHandler<StockReconcilationDeleteIMList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public StockReconcilationDeleteIMQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(StockReconcilationDeleteIMList request, CancellationToken cancellationToken)
        {
            try
            {
                var IMde = await _context.IMStockReconciliationTransactionHeader.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblIMStockReconciliationTransactionDetails IMdetails;
                IMdetails = _context.IMStockReconciliationTransactionDetails.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMdetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblIMStockReconciliationTransactionHeader IMHeader;
                IMHeader = _context.IMStockReconciliationTransactionHeader.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMHeader).State = EntityState.Deleted;
                _context.SaveChanges();
                return request.Id;
                return 0;
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                Log.Error("Error in delete Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }


        }
    }

    #endregion

    #region SingleItem

    //public class GetPRDetails : IRequest<TblPurchaseReturntDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string TranNumber { get; set; }
    //}

    //public class GetPRDetailsHandler : IRequestHandler<GetPRDetails, TblPurchaseReturntDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetPRDetailsHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<TblPurchaseReturntDto> Handle(GetPRDetails request, CancellationToken cancellationToken)
    //    {
    //        var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == request.TranNumber);
    //        TblPurchaseReturntDto obj = new();
    //        var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
    //        if (PoHeader is not null)
    //        {
    //            var Transid = PoHeader.TranNumber;

    //            var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
    //                .Where(e => Transid == e.TranId)
    //                .Select(e => new TblPopTrnPurchaseOrderDetailsDto
    //                {
    //                    TranItemCode = e.TranItemCode,
    //                    TranItemName = e.TranItemName,
    //                    TranItemName2 = e.TranItemName2,
    //                    TranItemQty = e.TranItemQty,
    //                    TranItemUnitCode = e.TranItemUnitCode,
    //                    TranUOMFactor = e.TranUOMFactor,
    //                    TranItemCost = e.TranItemCost,
    //                    TranTotCost = e.TranTotCost,
    //                    DiscPer = e.DiscPer,
    //                    DiscAmt = e.DiscAmt,
    //                    ItemTax = e.ItemTax,
    //                    ItemTaxPer = e.ItemTaxPer,
    //                    TaxAmount = e.TaxAmount,
    //                    ItemTracking = e.ItemTracking


    //                }).ToListAsync();

    //            obj.Id = PoHeader.Id;
    //            obj.VenCatCode = PoHeader.VenCatCode;
    //            obj.TranNumber = PoHeader.TranNumber;
    //            obj.Trantype = PoHeader.Trantype;
    //            obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
    //            obj.DeliveryDate = PoHeader.DeliveryDate;
    //            obj.CompCode = PoHeader.CompCode;
    //            obj.BranchCode = PoHeader.BranchCode;
    //            obj.InvRefNumber = PoHeader.InvRefNumber;
    //            obj.VendCode = PoHeader.VendCode;
    //            obj.DocNumber = PoHeader.DocNumber;
    //            obj.PaymentID = PoHeader.PaymentID;
    //            obj.Remarks = PoHeader.Remarks;
    //            obj.TAXId = PoHeader.TAXId;
    //            obj.TaxInclusive = PoHeader.TaxInclusive;
    //            obj.PONotes = PoHeader.PONotes;
    //            obj.itemList = iteminventory;
    //            obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
    //            obj.TranShipMode = PoHeader.TranShipMode;



    //        }
    //        return obj;
    //    }
    //}

    //#endregion

    //#region Get PRList

    //public class GetPurRequestList : IRequest<List<CustomSelectListItem>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string Input { get; set; }
    //}

    //public class GetPRListHandler : IRequestHandler<GetPurRequestList, List<CustomSelectListItem>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetPRListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<List<CustomSelectListItem>> Handle(GetPurRequestList request, CancellationToken cancellationToken)
    //    {
    //        Log.Info("----Info GetPurchaseRequestList method start----");
    //        var item = await _context.purchaseOrderHeaders.AsNoTracking()
    //            .OrderByDescending(e => e.Id)
    //           .Select(e => new CustomSelectListItem { Text = e.TranNumber, Value = e.TranNumber })
    //              .ToListAsync(cancellationToken);
    //        Log.Info("----Info GetPurchaseRequestList method Ends----");
    //        return item;
    //    }
    //}

    //#endregion


    //#region CreateIssuesApproval
    //public class CreateIssuesApproval : UserIdentityDto, IRequest<bool>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public TblTranInvoiceApprovalDto Input { get; set; }
    //}
    //public class CreateIssuesApprovalQueryHandler : IRequestHandler<CreateIssuesApproval, bool>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateIssuesApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
    //    {
    //        _mapper = mapper;
    //        _context = context;
    //    }

    //    public async Task<bool> Handle(CreateIssuesApproval request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info CreateInvoiceApproval method start----");

    //                var obj = await _context.TranInvoices.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
    //                var customer = await _context.OprCustomerMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);

    //                if (await _context.TrnCustomerApprovals.AnyAsync(e => e.InvoiceId == request.Input.Id && e.LoginId == request.User.UserId && e.IsApproved))
    //                    return true;

    //                TblFinTrnCustomerApproval approval = new()
    //                {
    //                    CompanyId = (int)obj.CompanyId,
    //                    BranchCode = obj.BranchCode,
    //                    TranDate = DateTime.Now,
    //                    TranSource = request.Input.TranSource,
    //                    Trantype = request.Input.Trantype,
    //                    CustCode = customer.CustCode,
    //                    DocNum = "DocNum",
    //                    LoginId = request.User.UserId,
    //                    AppRemarks = request.Input.AppRemarks,
    //                    InvoiceId = request.Input.Id,
    //                    IsApproved = true,
    //                };

    //                await _context.TrnCustomerApprovals.AddAsync(approval);
    //                await _context.SaveChangesAsync();

    //                if (!obj.InvoiceNumber.HasValue())
    //                {
    //                    int invoiceSeq = 0;
    //                    var invSeq = await _context.Sequences.FirstOrDefaultAsync();
    //                    if (invSeq is null)
    //                    {
    //                        invoiceSeq = 1;
    //                        TblSequenceNumberSetting setting = new()
    //                        {
    //                            InvoiceSeq = invoiceSeq
    //                        };
    //                        await _context.Sequences.AddAsync(setting);
    //                    }
    //                    else
    //                    {
    //                        invoiceSeq = invSeq.InvoiceSeq + 1;
    //                        invSeq.InvoiceSeq = invoiceSeq;
    //                        _context.Sequences.Update(invSeq);
    //                    }
    //                    await _context.SaveChangesAsync();

    //                    obj.InvoiceNumber = invoiceSeq.ToString();
    //                    obj.SpInvoiceNumber = string.Empty;
    //                    _context.TranInvoices.Update(obj);
    //                    await _context.SaveChangesAsync();
    //                }

    //                await transaction.CommitAsync();
    //                return true;
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in CreateInvoiceApproval Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return false;
    //            }
    //        }
    //    }
    //}
    #endregion
}
