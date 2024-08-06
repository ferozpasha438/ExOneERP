using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.PurchaseMgt;
using CIN.Domain.PurchaseSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.PurchasemgtQuery
{

    #region GetCurrencyList

    public class GetPRShipmentList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRShipmentListHandler : IRequestHandler<GetPRShipmentList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRShipmentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRShipmentList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetShipmentList method start----");
            var item = await _context.PopVendorShipments.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShipmentName, Value = e.ShipmentCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetShipmentList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetCurrencyList

    public class GetPRCurrencyList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRCurrencyListHandler : IRequestHandler<GetPRCurrencyList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRCurrencyListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRCurrencyList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCurrencyList method start----");
            var item = await _context.CurrencyCodes.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.CurrencyName, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCurrencyList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTaxList

    public class GetPRTaxList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRTaxListHandler : IRequestHandler<GetPRTaxList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRTaxListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRTaxList request, CancellationToken cancellationToken)
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
    #region GetCompanyList

    public class GetPRCompanyList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRCompanyListHandler : IRequestHandler<GetPRCompanyList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRCompanyListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRCompanyList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCompanyList method start----");
            var item = await _context.Companies.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.CompanyName, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCompanyList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetBranchList

    public class GetPRBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRBranchListHandler : IRequestHandler<GetPRBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRBranchList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetBranchList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetVendorCodeList

    public class GetPRVendorCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRVendorCodeListHandler : IRequestHandler<GetPRVendorCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRVendorCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRVendorCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetVendorCodeList method start----");
            var item = await _context.VendorMasters.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.VendCode, Value = e.VendCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetVendorCodeList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetVendorNameList

    public class GetPRVendorNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRVendorNameListHandler : IRequestHandler<GetPRVendorNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRVendorNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRVendorNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetVendorCodeList method start----");
            var item = await _context.VendorMasters.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.VendName, Value = e.VendCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetVendorCodeList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetPaymentTermList

    public class GetPRPaymentTermList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRPaymentTermListHandler : IRequestHandler<GetPRPaymentTermList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRPaymentTermListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRPaymentTermList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPaymentTermList method start----");
            var item = await _context.PopVendorPOTermsCodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.POTermsName, Value = e.POTermsCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetPaymentTermList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetItemCodeList

    public class GetPRItemCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRItemCodeListHandler : IRequestHandler<GetPRItemCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRItemCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRItemCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetItemCodeList method start----");
            bool isArab = request.User.Culture.IsArab();
            var item = _context.InvItemMaster.AsNoTracking();

            if (request.Input.HasValue())
            {
                item = item.Where(e => e.ItemCode.Contains(request.Input) || e.ShortName.Contains(request.Input)
                || e.ShortNameAr.Contains(request.Input));
            }
            var itemList = await item.OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemCode, Value = e.ItemCode, TextTwo = isArab ? e.ShortNameAr : e.ShortName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetItemCodeList method Ends----");
            return itemList;
        }
    }

    #endregion
    #region GetItemNameList

    public class GetPRItemNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRItemNameListHandler : IRequestHandler<GetPRItemNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRItemNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRItemNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetItemNameList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetItemNameList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetUOMList

    public class GetPRUOMSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRUOMselectListHandler : IRequestHandler<GetPRUOMSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRUOMselectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPRUOMSelectList request, CancellationToken cancellationToken)
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
    #region ProductUnitPriceItem

    public class PRProductUnitPriceItem : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Itemcode { get; set; }
    }

    public class PRProductUnitPriceItemHandler : IRequestHandler<PRProductUnitPriceItem, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public PRProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(PRProductUnitPriceItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info ProductUnitPriceItem method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .Where(e => e.ItemCode == request.Itemcode)
                 .Include(e => e.InvUoms)
               .Select(Product => new ProductUnitPriceDTO
               {

                   tranItemCode = Product.ItemCode,
                   tranItemName = Product.ShortName,
                   tranItemUnitCode = Product.ItemBaseUnit,
                   tranItemCost = Product.ItemAvgCost,


               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info ProductUnitPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region ProductUomtPriceItem

    public class PRProductUomtPriceItem : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class PRProductUOMFactorItemHandler : IRequestHandler<PRProductUomtPriceItem, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public PRProductUOMFactorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(PRProductUomtPriceItem request, CancellationToken cancellationToken)
        {
            var itemcode = request.ItemList.Split('_')[0];
            var ItemUOM = request.ItemList.Split('_')[1];

            Log.Info("----Info ProductUomtPriceItem method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                 .Where(e =>
                            (e.ItemCode.Contains(itemcode) && e.ItemUOM.Contains(ItemUOM)
                             ))
               .Select(Product => new ProductUnitPriceDTO
               {
                   tranItemCode = Product.ItemCode,
                   tranItemUomFactor = Product.ItemConvFactor,
                   ItemAvgcost = Product.ItemAvgCost,

               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info ProductUomtPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region CreatePurchaseReturn
    public class CreatePurchaseReturn : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPurchaseReturntDto Input { get; set; }
    }

    public class CreatePurchaseReturnQueryHandler : IRequestHandler<CreatePurchaseReturn, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePurchaseReturnQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreatePurchaseReturn request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {


                    Log.Info("----Info CreatePurchaseReturn method start----");
                    var transnumber = string.Empty;

                    var obj = request.Input;
                    TblPopTrnPurchaseReturnHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.purchaseReturnHeader.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                    }

                    else
                    {
                        if (obj.Trantype == "3")
                        {

                            var poheader = await _context.purchaseReturnHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                            if (poheader != null)
                            {
                                transnumber = Convert.ToString(int.Parse(poheader.TranNumber) + 1);

                            }
                            else
                            {
                                transnumber = Convert.ToString(10001);
                            }

                            cObj.TranNumber = transnumber;
                        }
                    }

                    cObj.VenCatCode = obj.VenCatCode;
                    cObj.Trantype = obj.Trantype;
                    cObj.TranDate = DateTime.Now;
                    cObj.DeliveryDate = obj.DeliveryDate ?? DateTime.Now;
                    cObj.CompCode = obj.CompCode;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.InvRefNumber = obj.InvRefNumber;
                    cObj.VendCode = obj.VendCode;
                    cObj.DocNumber = obj.DocNumber;
                    cObj.PaymentID = obj.PaymentID;
                    cObj.Remarks = obj.Remarks;
                    cObj.TAXId = obj.TAXId;
                    cObj.TaxInclusive = obj.TaxInclusive;
                    cObj.PONotes = obj.PONotes;
                    cObj.IsApproved = false;
                    cObj.TranCreateUserDate = DateTime.Now;
                    cObj.TranCreateUser = request.User.UserId;
                    cObj.CreatedOn = DateTime.Now;
                    cObj.IsActive = true;
                    cObj.TranCurrencyCode = obj.TranCurrencyCode;
                    cObj.ClosedBy = request.User.UserId;
                    cObj.TranCreateUser = request.User.UserId;
                    cObj.TranLastEditUser = request.User.UserId;
                    cObj.TranpostUser = request.User.UserId;
                    cObj.TranvoidDate = request.User.UserId;
                    cObj.TranvoidDate = request.User.UserId;


                    if (!obj.TranShipMode.HasValue())
                    {

                        var shCodeCount = await _context.PopVendorShipments.CountAsync();
                        var shCode = $"SH{shCodeCount:d5}";

                        TblPopDefVendorShipment vendShipment = new()
                        {
                            ShipmentCode = shCode,
                            ShipmentName = obj.ShipmentMode,
                            ShipmentDesc = obj.ShipmentMode,
                            ShipmentType = "Other",
                            //IsActive = true,
                            CreatedOn = DateTime.Now
                        };

                        await _context.PopVendorShipments.AddAsync(vendShipment);
                        await _context.SaveChangesAsync();

                        cObj.TranShipMode = shCode;
                    }
                    else
                        cObj.TranShipMode = obj.TranShipMode;


                    cObj.WHCode = obj.WHCode;
                    cObj.IsPaid = false;
                    //cObj.PurchaseReturnNO = obj.PurchaseRequestNO;

                    cObj.TranTotalCost = obj.TranTotalCost;
                    cObj.TranDiscPer = obj.TranDiscPer;
                    cObj.TranDiscAmount = obj.TranDiscAmount;
                    cObj.Taxes = obj.Taxes;







                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        //cObj.Id = 0;

                        _context.purchaseReturnHeader.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.purchaseReturnHeader.AddAsync(cObj);
                    }


                    await _context.SaveChangesAsync();

                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.purchaseReturnDetails.Where(e => e.TranId == transnumber).ToListAsync();
                        _context.purchaseReturnDetails.RemoveRange(oldAuthList);

                        List<TblPopTrnPurchaseReturnDetails> UOMList = new();
                        int i = 1;
                        string trans = "";
                        var PoDetialTransNumber = await _context.purchaseReturnDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var rItem in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.TranNumber) + i++);
                            else
                                trans = Convert.ToString(int.Parse(transnumber) + i++);

                            TblPopTrnPurchaseReturnDetails UOMItem = new()
                            {

                                TranNumber = trans,
                                TranId = transnumber,
                                TranDate = DateTime.UtcNow,
                                VendCode = obj.VendCode,
                                CompCode = obj.CompCode,
                                BranchCode = obj.BranchCode,
                                TranVendorCode = obj.VendCode,
                                ItemTracking = 0,
                                TranItemCode = rItem.TranItemCode,
                                TranItemName = rItem.TranItemName,
                                TranItemName2 = rItem.TranItemName2,
                                TranItemQty = rItem.TranItemQty,
                                TranItemUnitCode = rItem.TranItemUnitCode,
                                TranUOMFactor = rItem.TranUOMFactor,
                                TranItemCost = rItem.TranItemCost,
                                TranTotCost = rItem.TranTotCost,
                                DiscPer = rItem.DiscPer,
                                DiscAmt = rItem.DiscAmt,
                                ItemTax = rItem.ItemTax,
                                ItemTaxPer = rItem.ItemTaxPer,
                                TaxAmount = rItem.TaxAmount,
                                ReceivedQty = rItem.ReceivedQty ?? 0,
                                ReturnedQty = rItem.ReturnedQty ?? 0,
                                BalQty = rItem.BalQty,
                                IsActive = true,
                                CreatedOn = DateTime.UtcNow,

                            };
                            UOMList.Add(UOMItem);

                            ////#region Inventory History insert
                            ////   // var WareHouse = _context.InvWarehouses.Where(c => c.IsActive == Convert.ToBoolean(true));

                            //////foreach (var item in WareHouse)
                            //////{

                            ////var obj1 = new TblErpInvItemInventoryHistory()
                            ////{
                            ////    ItemCode = rItem.TranItemCode,
                            ////    WHCode = obj.WHCode,
                            ////    TranDate = DateTime.Now,
                            ////    TranType = "3",
                            ////    //TranNumber = InvTrans,
                            ////    TranNumber = transnumber,
                            ////    TranUnit = rItem.TranItemUnitCode,
                            ////    TranQty = rItem.TranItemQty,
                            ////    unitConvFactor = rItem.TranUOMFactor,
                            ////    TranTotQty = rItem.TranItemQty,
                            ////    TranPrice = rItem.TranItemCost,
                            ////    ItemAvgCost = (rItem.TranItemQty) / (rItem.TranItemCost),
                            ////    TranRemarks = "Inserted through PRT",
                            ////    IsActive = false,
                            ////    CreatedOn = DateTime.Now
                            ////};
                            ////_context.InvItemInventoryHistory.Add(obj1);
                            //////}
                            ////_context.SaveChanges();
                            ////#endregion Inventory History


                        }
                        await _context.purchaseReturnDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();



                    }

                    Log.Info("----Info CreatePurchaseReturn method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreatePurchaseReturn Method");
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

    public class GetPurchaseReturnList : IRequest<PaginatedList<PurchaseReturnPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetPurchasereturnListHandler : IRequestHandler<GetPurchaseReturnList, PaginatedList<PurchaseReturnPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchasereturnListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PurchaseReturnPaginationDto>> Handle(GetPurchaseReturnList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            //var list = await _context.purchaseReturnHeader.AsNoTracking()
            //  .Where(e =>
            //                (e.TranNumber.Contains(search) || e.InvRefNumber.Contains(search) ||
            //                    e.VendCode.Contains(search)
            //                 ))
            //   .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblPopTrnPurchaseReturnHeaderDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            // var brnApproval = _context.PurAuthorities.AsNoTracking();
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            var PRT = _context.purchaseReturnHeader.AsNoTracking();

            var isArab = request.User.Culture.IsArab();
            var enquiryDetails = _context.purchaseOrderDetails.AsNoTracking();
            var itemsList = _context.InvItemMaster.AsNoTracking().Select(e => new { e.ItemCode, e.ShortName, e.ShortNameAr });

            //  var oprAuths = _context.PurAuthorities.AsNoTracking();
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "PRT").AsNoTracking();

            var enquiryHeads = _context.purchaseReturnHeader.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.purchaseReturnHeader.AsNoTracking()
              .Where(e =>
                            (e.TranNumber.Contains(search) || e.InvRefNumber.Contains(search) ||
                                e.VendCode.Contains(search)
                            ))
               .OrderBy(request.Input.OrderBy).Select(d => new PurchaseReturnPaginationDto
               {
                   TranNumber = d.TranNumber,
                   TranDate = d.TranDate,
                   InvRefNumber = d.InvRefNumber,
                   BranchCode = d.BranchCode,
                   VendCode = d.VendCode,
                   PaymentID = d.PaymentID,
                   TAXId = d.TAXId,
                   Id = d.Id,
                   //IsApproved = d.IsApproved,
                   TranCreateUser = d.TranCreateUser,
                   TranTotalCost = d.TranTotalCost,

                   ItemNames = itemsList.Where(item => enquiryDetails.Where(e => e.TranId == d.TranNumber).Select(e => e.TranItemCode).Any(e => e == item.ItemCode))
                                         .Select(item => isArab ? item.ShortNameAr : item.ShortName).ToList(),

                   //HasAuthority=true,
                   //ApprovedUser = vendApproval.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PRT" && e.ServiceCode == d.TranNumber && e.IsApproved),
                   //IsApproved = vendApproval.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),

                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PRT" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).TranNumber),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),                   
                   //HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode && e.AppAuthPurcReturn),

                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.TranNumber && e.IsApproved),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthPurcReturn),
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Count(),

                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),

               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;

        }

    }

    #endregion
    #region SingleItem

    public class GetPRsDetails : IRequest<TblPurchaseReturListnDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetPRsDetailsHandler : IRequestHandler<GetPRsDetails, TblPurchaseReturListnDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRsDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturListnDto> Handle(GetPRsDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblPurchaseReturListnDto obj = new();
            var PoHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (PoHeader is not null)
            {
                var Transid = PoHeader.TranNumber;

                var iteminventory = await _context.purchaseReturnDetails.AsNoTracking()
                    .Where(e => Transid == e.TranId)
                    .Select(e => new TblPopTrnPurchaseReturnDetailsDto
                    {
                        TranItemCode = e.TranItemCode,
                        TranItemName = e.TranItemName,
                        TranItemName2 = e.TranItemName2,
                        TranItemQty = e.TranItemQty,
                        TranItemUnitCode = e.TranItemUnitCode,
                        TranUOMFactor = e.TranUOMFactor,
                        TranItemCost = e.TranItemCost,
                        TranTotCost = e.TranTotCost,
                        DiscPer = e.DiscPer,
                        DiscAmt = e.DiscAmt,
                        ItemTax = e.ItemTax,
                        ItemTaxPer = e.ItemTaxPer,
                        TaxAmount = e.TaxAmount,
                        ItemTracking = e.ItemTracking


                    }).ToListAsync();


                obj.VenCatCode = PoHeader.VenCatCode;
                obj.TranNumber = PoHeader.TranNumber;
                obj.Trantype = PoHeader.Trantype;
                obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                obj.DeliveryDate = PoHeader.DeliveryDate;
                obj.CompCode = PoHeader.CompCode;
                obj.BranchCode = PoHeader.BranchCode;
                obj.InvRefNumber = PoHeader.InvRefNumber;
                obj.VendCode = PoHeader.VendCode;
                obj.DocNumber = PoHeader.DocNumber;
                obj.PaymentID = PoHeader.PaymentID;
                obj.Remarks = PoHeader.Remarks;
                obj.TAXId = PoHeader.TAXId;
                obj.TaxInclusive = PoHeader.TaxInclusive;
                obj.PONotes = PoHeader.PONotes;
                obj.itemList = iteminventory;
                obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                obj.TranShipMode = PoHeader.TranShipMode;
                obj.WHCode = PoHeader.WHCode;


                //obj.trantotalcost = iteminventory;
                //obj.trandiscamount = iteminventory;
                //obj.itemList = iteminventory;

            }
            return obj;
        }
    }

    #endregion

    #region SingleItem

    public class GetPReturnDetails : IRequest<TblPurchaseReturListnDto>
    {
        public UserIdentityDto User { get; set; }
        public string TranNumber { get; set; }
    }

    public class GetPReturnDetailsHandler : IRequestHandler<GetPReturnDetails, TblPurchaseReturListnDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPReturnDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturListnDto> Handle(GetPReturnDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == request.TranNumber);
            TblPurchaseReturListnDto obj = new();
            var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (PoHeader is not null)
            {
                var Transid = PoHeader.TranNumber;

                var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
                    .Where(e => Transid == e.TranId)
                    .Select(e => new TblPopTrnPurchaseReturnDetailsDto
                    {
                        TranItemCode = e.TranItemCode,
                        TranItemName = e.TranItemName,
                        TranItemName2 = e.TranItemName2,
                        TranItemQty = e.TranItemQty,
                        TranItemUnitCode = e.TranItemUnitCode,
                        TranUOMFactor = e.TranUOMFactor,
                        TranItemCost = e.TranItemCost,
                        TranTotCost = e.TranTotCost,
                        DiscPer = e.DiscPer,
                        DiscAmt = e.DiscAmt,
                        ItemTax = e.ItemTax,
                        ItemTaxPer = e.ItemTaxPer,
                        TaxAmount = e.TaxAmount,
                        ItemTracking = e.ItemTracking


                    }).ToListAsync();

                obj.Id = PoHeader.Id;
                obj.VenCatCode = PoHeader.VenCatCode;
                obj.TranNumber = PoHeader.TranNumber;
                obj.Trantype = PoHeader.Trantype;
                //obj.TranDate = PoHeader.TranDate is not null ? Convert.ToDateTime(PoHeader.TranDate.Value.ToShortDateString()) : null;
                obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                obj.DeliveryDate = PoHeader.DeliveryDate;
                obj.CompCode = PoHeader.CompCode;
                obj.BranchCode = PoHeader.BranchCode;
                obj.InvRefNumber = PoHeader.InvRefNumber;
                obj.VendCode = PoHeader.VendCode;
                obj.DocNumber = PoHeader.DocNumber;
                obj.PaymentID = PoHeader.PaymentID;
                obj.Remarks = PoHeader.Remarks;
                obj.TAXId = PoHeader.TAXId;
                obj.TaxInclusive = PoHeader.TaxInclusive;
                obj.PONotes = PoHeader.PONotes;
                obj.itemList = iteminventory;
                obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                obj.TranShipMode = PoHeader.TranShipMode;



            }
            return obj;
        }
    }

    #endregion

    #region Get PRList

    public class GetPurReturnList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPeturnListHandler : IRequestHandler<GetPurReturnList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPeturnListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPurReturnList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPurchaseRequestList method start----");
            var item = await _context.purchaseReturnHeader.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TranNumber, Value = e.TranNumber })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetPurchaseRequestList method Ends----");
            return item;
        }
    }

    #endregion


    #region Delete
    public class DeletePRList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePRQueryHandler : IRequestHandler<DeletePRList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePRQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePRList request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var PRHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(d => d.Id == request.Id);
                    if (PRHeader is not null)
                    {

                        var prDetails = _context.purchaseReturnDetails.Where(e => e.TranId == PRHeader.TranNumber);
                        _context.purchaseReturnDetails.RemoveRange(prDetails);
                        _context.SaveChanges();
                        _context.purchaseReturnHeader.Remove(PRHeader);
                        _context.SaveChanges();

                        await transaction.CommitAsync();
                        return request.Id;
                    }
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
    }

    #endregion

    #region Settlement

    public class PRTAccountsPosting : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetPRTAccountsPostHandler : IRequestHandler<PRTAccountsPosting, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRTAccountsPostHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(PRTAccountsPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var PurchaseRequestNO = string.Empty;
                var PurchaseOrderNO = string.Empty;
                string InvTrans = string.Empty;
                string FinTrans = string.Empty, invoiceSeq = String.Empty;
                var inoviceIds = 0;
                int invoiceSeqId = 0;

                var transnumber = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                TblPurchaseReturListnDto obj = new();
                var PoHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.purchaseReturnDetails.AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseReturnDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranItemName = e.TranItemName,
                            TranItemName2 = e.TranItemName2,
                            TranItemQty = e.TranItemQty,
                            TranItemUnitCode = e.TranItemUnitCode,
                            TranUOMFactor = e.TranUOMFactor,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            DiscPer = e.DiscPer,
                            DiscAmt = e.DiscAmt,
                            ItemTax = e.ItemTax,
                            ItemTaxPer = e.ItemTaxPer,
                            TaxAmount = e.TaxAmount,
                            ItemTracking = e.ItemTracking


                        }).ToListAsync();
                    FinTrans = PoHeader.TranNumber;


                    obj.VenCatCode = PoHeader.VenCatCode;
                    obj.TranNumber = PoHeader.TranNumber;
                    obj.Trantype = PoHeader.Trantype;
                    obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = PoHeader.DeliveryDate;
                    obj.CompCode = PoHeader.CompCode;
                    obj.BranchCode = PoHeader.BranchCode;
                    obj.InvRefNumber = PoHeader.InvRefNumber;
                    obj.VendCode = PoHeader.VendCode;
                    obj.DocNumber = PoHeader.DocNumber;
                    obj.PaymentID = PoHeader.PaymentID;
                    obj.Remarks = PoHeader.Remarks;
                    obj.TAXId = PoHeader.TAXId;
                    obj.TaxInclusive = PoHeader.TaxInclusive;
                    obj.PONotes = PoHeader.PONotes;
                    obj.itemList = iteminventory;
                    obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                    obj.TranShipMode = PoHeader.TranShipMode;
                    obj.TranTotalCost = PoHeader.TranTotalCost;
                    obj.WHCode = PoHeader.WHCode;

                    #region Ispaid 
                    var Paid = _context.purchaseReturnHeader.Where(e => e.TranNumber == FinTrans);
                    var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                    POPaid.IsPaid = true;
                    _context.purchaseReturnHeader.Update(POPaid).Property(x => x.Id).IsModified = false; ;

                    await _context.SaveChangesAsync();
                    #endregion

                    #region QtyUpdate
                    foreach (var invtItem in iteminventory)
                    {
                        var invtUOM = await _context.InvItemsUOM.FirstOrDefaultAsync(e => e.ItemCode == invtItem.TranItemCode && e.ItemUOM == invtItem.TranItemUnitCode);
                        var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == invtItem.TranItemCode && e.WHCode == obj.WHCode);
                        var cItems = cInvItems;
                        var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();

                        var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == invtItem.TranItemCode);
                        var cItemMasters = cInvItemMasters;
                        var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();

                        // decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOH);
                        decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                        decimal? ItemAvgCost = await cInvItems.SumAsync(e => e.ItemAvgCost);
                        decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);
                        //var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == auth.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                        decimal itmAvgcost = 0;

                        var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == invtItem.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                        //if (PoDetails is not null)
                        //    itmAvgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))));
                        //else
                        //    itmAvgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty))));


                        foreach (var itemCode in cItemIds)
                        {
                            var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == itemCode);
                            decimal originalQty = 0;
                            decimal itemAvgCost = 0;
                            if (invtUOM.ItemConvFactor > 1)
                            {
                                originalQty = invtItem.TranItemQty;
                                invtItem.TranItemQty = invtItem.TranItemQty * invtUOM.ItemConvFactor;
                            }
                            else
                            {
                                originalQty = invtItem.TranItemQty;
                                invtItem.TranItemQty = invtItem.TranItemQty;
                            }

                            decimal tranItemCost = invtItem.TranItemCost - (invtItem.TranItemCost * invtItem.DiscPer) / 100;

                            //cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));
                            //cInvoice.ItemAvgCost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty)));

                            decimal newTranItemQty =  invtItem.TranItemQty;

                            if (invtUOM.ItemConvFactor > 1)
                            {
                                itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)(newTranItemQty / invtUOM.ItemConvFactor)))) / ((((decimal)oldInventory.QtyOH) + ((decimal)newTranItemQty)));
                            }
                            else
                            {
                                itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)newTranItemQty))) / ((((decimal)oldInventory.QtyOH) + ((decimal)newTranItemQty)));

                            }

                            oldInventory.ItemAvgCost = itemAvgCost;

                            // oldInventory.ItemAvgCost = ((oldInventory.QtyOH * oldInventory.ItemAvgCost) + (tranItemCost * newTranItemQty)) / (oldInventory.QtyOH + newTranItemQty);

                            //if (PoDetails is not null)
                            //else
                            //    oldInventory.ItemAvgCost = ((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty)));

                            //cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost - auth.TranItemCost);

                            itmAvgcost = oldInventory.ItemAvgCost;
                            oldInventory.QtyOH = ((decimal)oldInventory.QtyOH - invtItem.TranItemQty);

                            _context.InvItemInventory.Update(oldInventory);
                            await _context.SaveChangesAsync();


                            var itemMaster = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == itemCode);
                            if (itemMaster is not null)
                            {
                                var C1 = String.Format("{0:0.0000}", itmAvgcost);
                                itemMaster.ItemAvgCost = Convert.ToString(C1);
                                _context.InvItemMaster.Update(itemMaster);
                                await _context.SaveChangesAsync();
                            }

                            #region Inventory History insert
                            //var WareHouse = _context.InvWarehouses.Where(c => c.WHBranchCode == obj.BranchCode);
                            //var author = _context.InvWarehouses.Where(a => a.WHBranchCode == obj.BranchCode).Single();
                            //foreach (var item in WareHouse)
                            //{


                            var obj1 = new TblErpInvItemInventoryHistory()
                            {
                                ItemCode = invtItem.TranItemCode,
                                WHCode = obj.WHCode,
                                //WHCode = cItemIds1,
                                TranDate = DateTime.Now,
                                TranType = "3",
                                //TranNumber = InvTrans,
                                TranNumber = FinTrans,
                                TranUnit = invtItem.TranItemUnitCode,
                                TranQty = originalQty,
                                unitConvFactor = invtItem.TranUOMFactor,
                                TranTotQty = invtItem.TranItemQty,
                                //TranTotQty = oldInventory.QtyOH == 0 ? 1 : oldInventory.QtyOH,
                                TranPrice = invtItem.TranItemCost,
                                //ItemAvgCost = (auth.TranItemQty) / (auth.TranItemCost),
                                //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty))),
                                ItemAvgCost = itmAvgcost,
                                TranRemarks = "Inserted through PRT",
                                IsActive = false,
                                CreatedOn = DateTime.Now
                            };

                            _context.InvItemInventoryHistory.Add(obj1);
                            _context.SaveChanges();

                            #endregion Inventory History

                        }

                        ////foreach (var invIds in cItemMasterIds)
                        ////{
                        ////    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
                        ////    //var itemsAVgcost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty)));
                        ////    //var itemsAVgcost = (((decimal)QtyReserved + auth.TranItemQty)) / (((decimal)ItemAvgCost + auth.TranItemCost));
                        ////    var itemsAVgcost = 0;
                        ////    if (PoDetails is not null)
                        ////        itemsAVgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)invtItem.TranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)invtItem.TranItemQty))));
                        ////    else
                        ////        itemsAVgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)invtItem.TranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)0) + ((decimal)invtItem.TranItemQty))));

                        ////    var C1 = String.Format("{0:0.0000}", itemsAVgcost);
                        ////    cInvoice.ItemAvgCost = Convert.ToString(C1);
                        ////    _context.InvItemMaster.Update(cInvoice);
                        ////    await _context.SaveChangesAsync();
                        ////}





                    }
                    #endregion

                    try
                    {
                        Log.Info("----Info CreateApInvoiceSettlement method start----");

                        //var input = request.Input;
                        var Finobj = await _context.purchaseReturnHeader.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                        var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);


                        //if (await _context.TrnVendorInvoices.AnyAsync(e => e.InvoiceId == request.id && e.LoginId == request.User.UserId && e.IsPaid))
                        //    return ApiMessageInfo.Status(1);


                        //Finobj.TranNumber = invoiceSeq.ToString();
                        //_context.purchaseOrderHeaders.Update(Finobj);
                        //await _context.SaveChangesAsync();

                        var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        // var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.BranchCode);
                        var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = inoviceIds,
                            //FinAcCode = IsNotCreditPay(request.Input.PaymentType) ? payCode.FinPayAcIntgrAC : vendor.VendArAcCode,
                            FinAcCode = IsNotCreditPay("cash") ? invDistGroup.InvDefaultAPAc : vendor.VendArAcCode,

                            CrAmount = 0,
                            DrAmount = Finobj.TranTotalCost,
                            Source = "PR",
                            Gl = string.Empty,
                            Type = IsNotCreditPay("cash") ? "paycode" : "Vendor",
                            //Type = IsNotCreditPay(request.Input.PaymentType) ? "paycode" : "Vendor",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = inoviceIds,
                            //FinAcCode = vendor.VendDefExpAcCode,
                            FinAcCode = invDistGroup.InvAssetAc,//InventoryAccount 10201001
                                                                // FinAcCode = vendor.VendDefExpAcCode,//InventoryAccount 10201001

                            CrAmount = Finobj.TranTotalCost - Finobj.Taxes,
                            DrAmount = 0,
                            Source = "PR",
                            Gl = string.Empty,
                            Type = "Cost",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);


                        var invoiceItem = await _context.purchaseReturnDetails.FirstOrDefaultAsync(e => e.TranId == Finobj.TranNumber);
                        var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.ItemTaxPer).ToString());

                        if (tax is null)
                            throw new NullReferenceException("Tax is empty");

                        TblFinTrnDistribution distribution3 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = tax?.InputAcCode01,
                            CrAmount = Finobj.Taxes,
                            DrAmount = 0,
                            Source = "PR",
                            Gl = string.Empty,
                            Type = "VAT",
                            CreatedOn = DateTime.Now
                        };

                        await _context.FinDistributions.AddAsync(distribution3);
                        await _context.SaveChangesAsync();

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2, distribution3 };

                        int jvSeq = 0;
                        var seqquence = await _context.Sequences.FirstOrDefaultAsync();
                        if (seqquence is null)
                        {
                            jvSeq = 1;
                            TblSequenceNumberSetting setting1 = new()
                            {
                                JvVoucherSeq = jvSeq
                            };
                            await _context.Sequences.AddAsync(setting1);
                        }
                        else
                        {
                            jvSeq = seqquence.JvVoucherSeq + 1;
                            seqquence.JvVoucherSeq = jvSeq;
                            _context.Sequences.Update(seqquence);
                        }
                        await _context.SaveChangesAsync();


                        #region Purchase Invoice Inserting  and Storing in  JournalVoucher tables

                        string SpCreditNumber = $"PRT";
                        //string SpCreditNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        if (invSeq is null)
                        {
                            invoiceSeqId = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                VendCreditSeq = invoiceSeqId
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeqId = invSeq.VendCreditSeq + 1;
                            invSeq.VendCreditSeq = invoiceSeqId;

                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        invoiceSeq = $"C{invoiceSeqId}";


                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)Finobj.CompCode,
                            BranchCode = Finobj.BranchCode,
                            Batch = string.Empty,
                            Source = "PR",
                            Remarks = Finobj.Remarks,
                            Narration = Finobj.PONotes ?? Finobj.Remarks,
                            JvDate = DateTime.Now,
                            //Amount = Finobj.TranTotalCost ?? 0,
                            Amount = Finobj.TranTotalCost,
                            DocNum = invoiceSeq.ToString(),
                            CDate = DateTime.Now,
                            Posted = true,
                            PostedDate = DateTime.Now
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;

                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                            .Where(e => e.FinBranchCode == Finobj.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval vapproval = new()
                                {
                                    CompanyId = (int)Finobj.CompCode,
                                    BranchCode = Finobj.BranchCode,
                                    JvDate = DateTime.Now,
                                    TranSource = "PR",
                                    Trantype = "Invoice",
                                    DocNum = Finobj.InvRefNumber,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = Finobj.Remarks,
                                    JournalVoucherId = jvId,
                                    IsApproved = true,
                                };
                                jvApprovalList.Add(vapproval);
                            }

                            if (jvApprovalList.Count > 0)
                            {
                                await _context.JournalVoucherApprovals.AddRangeAsync(jvApprovalList);
                                await _context.SaveChangesAsync();
                            }
                        }


                        List<TblFinTrnJournalVoucherItem> JournalVoucherItemsList = new();

                        foreach (var obj1 in distributionsList)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = Finobj.BranchCode,
                                Batch = string.Empty,
                                Remarks = Finobj.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = Finobj.PONotes,

                            };
                            JournalVoucherItemsList.Add(JournalVoucherItem);
                        }

                        if (JournalVoucherItemsList.Count > 0)
                        {
                            await _context.JournalVoucherItems.AddRangeAsync(JournalVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }


                        TblFinTrnJournalVoucherStatement jvStatement = new()
                        {

                            JvDate = DateTime.Now,
                            TranNumber = jvSeq.ToString(),
                            //Remarks1 = input.Remarks1,
                            //Remarks2 = input.Remarks2,
                            Remarks1 = Finobj.Remarks,
                            Remarks2 = Finobj.PONotes,
                            LoginId = request.User.UserId,
                            JournalVoucherId = jvId,
                            IsPosted = true,
                            IsVoid = false
                        };
                        await _context.JournalVoucherStatements.AddAsync(jvStatement);
                        await _context.SaveChangesAsync();

                        List<TblFinTrnAccountsLedger> ledgerList = new();
                        foreach (var item in JournalVoucherItemsList)
                        {
                            TblFinTrnAccountsLedger ledger = new()
                            {
                                MainAcCode = item.FinAcCode,
                                AcCode = item.FinAcCode,
                                AcDesc = item.Description,
                                Batch = item.Batch,
                                BranchCode = item.BranchCode,
                                CrAmount = item.CrAmount,
                                DrAmount = item.DrAmount,
                                IsApproved = true,
                                TransDate = DateTime.Now,
                                PostedFlag = true,
                                PostDate = DateTime.Now,
                                Jvnum = item.JournalVoucherId.ToString(),
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "PR",
                                ExRate = 0,
                                CostAllocation = item.CostAllocation,
                                CostSegCode = item.CostSegCode
                            };
                            ledgerList.Add(ledger);
                        }
                        if (ledgerList.Count > 0)
                        {
                            await _context.AccountsLedgers.AddRangeAsync(ledgerList);
                            await _context.SaveChangesAsync();
                        }

                        //await transaction.CommitAsync();
                        //return ApiMessageInfo.Status(1);
                        //return true;


                        var VendorID = _context.VendorMasters.Where(c => c.VendCode == obj.VendCode).Single();
                        TblTranVenInvoice Invoice = new();
                        Invoice = new()
                        {
                            SpCreditNumber = string.Empty,
                            CreditNumber = invoiceSeq.ToString(),
                            InvoiceDate = Convert.ToDateTime(PoHeader.TranDate),
                            InvoiceDueDate = Convert.ToDateTime(PoHeader.DeliveryDate),
                            CompanyId = PoHeader.CompCode,



                            SubTotal = PoHeader.TranTotalCost,
                            DiscountAmount = 0,
                            AmountBeforeTax = 0,
                            TaxAmount = 0,
                            TotalAmount = PoHeader.TranTotalCost,
                            TotalPayment = 0,
                            AmountDue = 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(PoHeader.TranDate),
                            CreatedBy = request.User.UserId,

                            CustomerId = VendorID.Id,

                            InvoiceStatus = "Open",
                            TaxIdNumber = PoHeader.TAXId,

                            InvoiceModule = "",
                            InvoiceNotes = PoHeader.PONotes,
                            Remarks = obj.Remarks,
                            InvoiceRefNumber = PoHeader.InvRefNumber,

                            LpoContract = "",
                            VatPercentage = Convert.ToDecimal(0),
                            PaymentTerms = PoHeader.PaymentID,
                            BranchCode = PoHeader.BranchCode,
                            ServiceDate1 = PoHeader.DeliveryDate.ToString(),

                            IsCreditConverted = true,
                            InvoiceStatusId = 2

                        };

                        await _context.TranVenInvoices.AddAsync(Invoice);
                        await _context.SaveChangesAsync();

                        TblFinTrnVendorApproval approval = new()
                        {
                            CompanyId = request.User.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "PR",
                            Trantype = "Invoice",
                            VendCode = PoHeader.VenCatCode,
                            DocNum = "DocNum",
                            LoginId = request.User.UserId,
                            AppRemarks = "POApproval",
                            InvoiceId = Invoice.Id,
                            IsApproved = true,
                        };

                        await _context.TrnVendorApprovals.AddAsync(approval);
                        await _context.SaveChangesAsync();
                        var paymentTerms1 = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == obj.PaymentID);
                        TblFinTrnVendorInvoice cInvoice = new()
                        {
                            CompanyId = (int)obj.CompCode,
                            BranchCode = obj.BranchCode,
                            InvoiceNumber = invoiceSeq.ToString(),
                            InvoiceDate = obj.TranDate,
                            CreditDays = paymentTerms1.POTermsDueDays,
                            DueDate = obj.DeliveryDate,
                            TranSource = "PR",
                            Trantype = "Credit",
                            //Trantype = "Invoice",
                            VendCode = obj.VenCatCode,
                            DocNum = obj.DocNumber,
                            LoginId = request.User.UserId,
                            ReferenceNumber = obj.InvRefNumber,
                            InvoiceAmount = obj.TranTotalCost,
                            DiscountAmount = 0,
                            NetAmount = obj.TranTotalCost,
                            PaidAmount = 0,
                            BalanceAmount = obj.TranTotalCost,
                            AppliedAmount = 0,
                            Remarks1 = obj.Remarks,
                            Remarks2 = obj.Remarks,
                            InvoiceId = Invoice.Id,
                            IsPaid = true,
                        };
                        await _context.TrnVendorInvoices.AddAsync(cInvoice);

                        TblFinTrnVendorStatement cStatement = new()
                        {
                            CompanyId = PoHeader.CompCode,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "PR",
                            Trantype = "Credit",
                            TranNumber = PoHeader.TranNumber,
                            VendCode = obj.VendCode,
                            DocNum = "DocNum",
                            ReferenceNumber = obj.InvRefNumber,
                            PaymentType = "check",
                            PamentCode = "paycode",
                            CheckNumber = invoiceSeq.ToString(),
                            Remarks1 = obj.Remarks,
                            Remarks2 = obj.Remarks,
                            LoginId = request.User.UserId,
                            DrAmount = obj.TranTotalCost,
                            CrAmount = obj.TranTotalCost,
                            InvoiceId = Invoice.Id,
                        };
                        await _context.TrnVendorStatements.AddAsync(cStatement);


                        await _context.SaveChangesAsync();


                        Log.Info("----Info CreateUpdateInvoice method Exit----");

                        var inoviceId = Invoice.Id;
                        var invoiceItems = iteminventory;
                        if (invoiceItems.Count > 0)
                        {
                            List<TblTranVenInvoiceItem> invoiceItemsList = new();

                            foreach (var obj1 in invoiceItems)
                            {
                                var ProductID = _context.InvItemMaster.Where(c => c.ItemCode == obj1.TranItemCode).Single();
                                var InvoiceItem = new TblTranVenInvoiceItem
                                {

                                    CreditId = inoviceId,
                                    CreditNumber = SpCreditNumber,
                                    ProductId = ProductID.Id,
                                    Quantity = Convert.ToInt32(obj1.TranItemQty),

                                    UnitPrice = obj1.TranItemCost,
                                    SubTotal = obj1.TranTotCost,
                                    DiscountAmount = obj1.DiscAmt,
                                    AmountBeforeTax = obj1.TranItemCost,
                                    TaxAmount = obj1.TaxAmount,
                                    TotalAmount = obj1.TranTotCost,
                                    IsDefaultConfig = true,
                                    CreatedOn = Convert.ToDateTime(obj.TranDate),

                                    CreatedBy = 1,
                                    Description = obj1.TranItemName,
                                    TaxTariffPercentage = obj1.ItemTaxPer,
                                    InvoiceType = 4,

                                    Discount = 1
                                };
                                invoiceItemsList.Add(InvoiceItem);
                            }
                            if (invoiceItemsList.Count > 0)
                            {
                                await _context.TranVenInvoiceItems.AddRangeAsync(invoiceItemsList);
                                await _context.SaveChangesAsync();
                            }
                        }

                        #endregion

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in CreateUpdateInvoice Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return ApiMessageInfo.Status(0);
                    }

                }
                Log.Info("----Info CreatePurchaseRequest method Exit----");
                await transaction.CommitAsync();
                //return ApiMessageInfo.Status(1, 0);
                return ApiMessageInfo.Status(1, request.id);
                //return obj;
            }
        }
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }

    #endregion

    #region GetGRNReturnDetails

    public class GetGRNReturnDetails : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public string PONO { get; set; }
    }

    public class GetGRNReturnDetailListsHandler : IRequestHandler<GetGRNReturnDetails, TblPurchaseReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGRNReturnDetailListsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetGRNReturnDetails request, CancellationToken cancellationToken)
        {
            var bqty = 0;

            TblPurchaseReturntDto obj = new();
            var PoHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == request.PONO);
            // var GRNH = await _context.GRNHeaders.Where(e => e.PurchaseOrderNO == request.PONO).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
            //if (GRNH is not null)
            //{
            //    var bqty1 = await _context.GRNDetails.Where(e => e.TranId == GRNH.TranNumber).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
            //    bqty = (int)bqty1.BalQty;
            //}


            //var GRNHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == PoHeader.PurchaseOrderNO);

            if (PoHeader is not null)
            {
                var Transid = PoHeader.TranNumber;

                List<TblPopTrnPurchaseOrderDetailsDto> iteminventory = new();

                var iteminventoryItems = _context.GRNDetails.AsNoTracking()
                    .Where(e => Transid == e.TranId)
                    .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                    {
                        TranItemCode = e.TranItemCode,
                        TranItemName = e.TranItemName,
                        TranItemName2 = e.TranItemName2,
                        TranItemQty = e.TranItemQty,
                        TranItemUnitCode = e.TranItemUnitCode,
                        TranUOMFactor = e.TranUOMFactor,
                        TranItemCost = e.TranItemCost,
                        TranTotCost = e.TranTotCost,
                        DiscPer = e.DiscPer,
                        DiscAmt = e.DiscAmt,
                        ItemTax = e.ItemTax,
                        ItemTaxPer = e.ItemTaxPer,
                        TaxAmount = e.TaxAmount,
                        ItemTracking = e.ItemTracking,
                        ReceivedQty = 0,
                        ReturnedQty = 0,
                        BalQty = (int)e.ReceivingQty,
                        PreBalQty = (int)e.ReceivingQty,
                    });



                //var poReturnHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseReturnNO == PoHeader.PurchaseOrderNO);
                //if (poReturnHeader is not null)
                //{
                //    iteminventory = await _context.purchaseReturnDetails.AsNoTracking()
                //                                      .Where(e => e.TranId == poReturnHeader.TranNumber)
                //                                      .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                //                                      {
                //                                          TranItemCode = e.TranItemCode,
                //                                          TranItemName = e.TranItemName,
                //                                          TranItemName2 = e.TranItemName2,
                //                                          TranItemQty = e.TranItemQty,
                //                                          TranItemUnitCode = e.TranItemUnitCode,
                //                                          TranUOMFactor = e.TranUOMFactor,
                //                                          TranItemCost = e.TranItemCost,
                //                                          TranTotCost = e.TranTotCost,
                //                                          DiscPer = e.DiscPer,
                //                                          DiscAmt = e.DiscAmt,
                //                                          ItemTax = e.ItemTax,
                //                                          ItemTaxPer = e.ItemTaxPer,
                //                                          TaxAmount = e.TaxAmount,
                //                                          ItemTracking = e.ItemTracking,
                //                                          ReceivedQty = e.ReturnedQty,
                //                                          ReturnedQty = 0,
                //                                          BalQty = (int)e.BalQty,
                //                                          PreBalQty = (int)e.BalQty
                //                                      }).ToListAsync();
                //}
                //else
                //{
                //    iteminventory = await iteminventoryItems.ToListAsync();

                //}


                iteminventory = await iteminventoryItems.ToListAsync();

                foreach (var item in iteminventory)
                {
                    var returnItem = _context.purchaseReturnDetails.AsNoTracking().Where(e => e.TranItemCode == item.TranItemCode);
                    if (returnItem is not null)
                    {
                        item.ReceivedQty = returnItem.Sum(e => e.ReturnedQty);
                        item.ReturnedQty = 0;
                        item.BalQty = (int)returnItem.Sum(e => e.BalQty);
                        item.PreBalQty = (int)returnItem.Sum(e => e.BalQty);
                    }
                }

                iteminventory = iteminventory.Where(e => e.TranItemQty != e.ReceivedQty).ToList();

                obj.Id = PoHeader.Id;
                obj.VenCatCode = PoHeader.VenCatCode;
                obj.TranNumber = PoHeader.TranNumber;
                obj.Trantype = PoHeader.Trantype;
                //obj.TranDate = PoHeader.TranDate is not null ? Convert.ToDateTime(PoHeader.TranDate.Value.ToShortDateString()) : null;
                obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                obj.DeliveryDate = PoHeader.DeliveryDate;
                obj.CompCode = PoHeader.CompCode;
                obj.BranchCode = PoHeader.BranchCode;
                obj.InvRefNumber = PoHeader.InvRefNumber;
                obj.VendCode = PoHeader.VendCode;
                obj.DocNumber = PoHeader.DocNumber;
                obj.PaymentID = PoHeader.PaymentID;
                obj.Remarks = PoHeader.Remarks;
                obj.TAXId = PoHeader.TAXId;
                obj.TaxInclusive = PoHeader.TaxInclusive;
                obj.PONotes = PoHeader.PONotes;
                obj.itemList = iteminventory;
                obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                obj.TranShipMode = PoHeader.TranShipMode;
                obj.TranTotalCost = PoHeader.TranTotalCost;
                obj.Taxes = PoHeader.Taxes;
                obj.TranDiscPer = PoHeader.TranDiscPer;
                obj.TranDiscAmount = PoHeader.TranDiscAmount;
                obj.WHCode = PoHeader.WHCode;


            }
            //}

            return obj;
        }
    }

    #endregion


    #region GetItemsForWarehouseSelectList
    public class GetItemsForWarehouseSelectList : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string WhCode { get; set; }
        public string ItemCode { get; set; }
        public decimal ItemCount { get; set; }

    }

    public class GetItemsForWarehouseSelectListHandler : IRequestHandler<GetItemsForWarehouseSelectList, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemsForWarehouseSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetItemsForWarehouseSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPurchaseRequestList method start----");
            var item = await _context.InvItemInventory.AsNoTracking()
                                     .Where(e => e.WHCode == request.WhCode && e.ItemCode == request.ItemCode)
                                     .Select(e => new CustomSelectListItem { Text = e.QtyOH.ToString(), Value = "0" })
                                     .FirstOrDefaultAsync(cancellationToken);
            if (request.ItemCount > decimal.Parse(item.Text))
                item.Value = "1";

            Log.Info("----Info GetPurchaseRequestList method Ends----");
            return item;
        }
    }

    #endregion
}
