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
using FluentValidation.Validators;

//using LinqToDB;


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

    public class GetShipmentList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetShipmentListHandler : IRequestHandler<GetShipmentList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShipmentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetShipmentList request, CancellationToken cancellationToken)
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

    public class GetCurrencyList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCurrencyListHandler : IRequestHandler<GetCurrencyList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCurrencyListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCurrencyList request, CancellationToken cancellationToken)
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
    #region GetCompanyList

    public class GetCompanyList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCompanyListHandler : IRequestHandler<GetCompanyList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanyListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCompanyList request, CancellationToken cancellationToken)
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

    public class GetBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchListHandler : IRequestHandler<GetBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBranchList request, CancellationToken cancellationToken)
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

    public class GetVendorCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetVendorCodeListHandler : IRequestHandler<GetVendorCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVendorCodeList request, CancellationToken cancellationToken)
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

    public class GetVendorNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetVendorNameListHandler : IRequestHandler<GetVendorNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVendorNameList request, CancellationToken cancellationToken)
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

    public class GetPaymentTermList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPaymentTermListHandler : IRequestHandler<GetPaymentTermList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPaymentTermListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPaymentTermList request, CancellationToken cancellationToken)
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

    public class GetItemCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetItemCodeListHandler : IRequestHandler<GetItemCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetItemCodeList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            Log.Info("----Info GetItemCodeList method start----");
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

    public class GetItemNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetItemNameListHandler : IRequestHandler<GetItemNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetItemNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetItemNameList method start----");
            var items = _context.InvItemMaster.AsNoTracking();
            bool isArab = request.User.Culture.IsArab();
            if (request.Input.HasValue())
                items = items.Where(e => e.IsActive && (e.ShortName.Contains(request.Input) || e.ItemCode.Contains(request.Input)));

            var newItems = await items.OrderByDescending(e => e.Id)
            .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
            //.Select(e => new CustomSelectListItem { Text = isArab ? e.ShortNameAr : e.ShortName, Value = e.ItemCode })
               .ToListAsync(cancellationToken);
            Log.Info("----Info GetItemNameList method Ends----");
            return newItems;
        }
    }

    #endregion
    #region GetUOMList

    public class GetUOMSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetUOMselectListHandler : IRequestHandler<GetUOMSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUOMselectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUOMSelectList request, CancellationToken cancellationToken)
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

    public class ProductUnitPriceItem : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Itemcode { get; set; }
    }

    public class ProductUnitPriceItemHandler : IRequestHandler<ProductUnitPriceItem, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(ProductUnitPriceItem request, CancellationToken cancellationToken)
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

    public class ProductUomtPriceItem : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class ProductUOMFactorItemHandler : IRequestHandler<ProductUomtPriceItem, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductUOMFactorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(ProductUomtPriceItem request, CancellationToken cancellationToken)
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
    #region CreatePurchaseRequest
    public class CreatePurchaseRequest : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPurchaseReturntDto Input { get; set; }
    }

    public class CreatePurchaseRequestQueryHandler : IRequestHandler<CreatePurchaseRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePurchaseRequestQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreatePurchaseRequest request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {


                    Log.Info("----Info CreatePurchaseRequest method start----");
                    var transnumber = string.Empty;
                    var PurchaseRequestNO = string.Empty;
                    var PurchaseOrderNO = string.Empty;
                    string InvTrans = string.Empty;
                    string FinTrans = string.Empty;
                    var inoviceIds = 0;
                    var obj = request.Input;

                    int poSeq = 0;
                    var sequence = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == obj.BranchCode);

                    TblPopTrnPurchaseOrderHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.purchaseOrderHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                        //var Pode = await _context.purchaseOrderHeaders.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        //TblPopTrnPurchaseOrderDetails podetails;
                        //podetails = _context.purchaseOrderDetails.Where(d => d.TranId == Pode.TranNumber).First();
                        //_context.Entry(podetails).State = EntityState.Deleted;
                        //_context.SaveChanges();

                        //TblPopTrnPurchaseOrderHeader department;
                        //department = _context.purchaseOrderHeaders.Where(d => d.TranNumber == Pode.TranNumber).First();
                        //_context.Entry(department).State = EntityState.Deleted;
                        //_context.SaveChanges();


                        //obj.Id = 0;
                        //if (obj.PurchaseRequestNO == "" || obj.PurchaseRequestNO == "null")
                        //{
                        if (obj.Trantype == "1")
                        {

                            //var poheader = await _context.purchaseOrderHeaders.Where(e => e.PurchaseRequestNO != null).OrderBy(e => e.PurchaseRequestNO).LastOrDefaultAsync();
                            //if (poheader != null)
                            //    PurchaseRequestNO = Convert.ToString(int.Parse(poheader.PurchaseRequestNO.HasValue() ? poheader.PurchaseRequestNO : "10001") + 1);
                            //else
                            //    PurchaseRequestNO = Convert.ToString(10001);

                            PurchaseRequestNO = cObj.PurchaseRequestNO;

                        }
                        //}
                        //if (obj.PurchaseOrderNO == "" || obj.PurchaseOrderNO == "null")
                        ////if (cObj.PurchaseOrderNO == "null")
                        //{
                        if (obj.Trantype == "2")
                        {
                            //var poheader = await _context.purchaseOrderHeaders.Where(e => e.PurchaseOrderNO != null).OrderByDescending(e => e.PurchaseOrderNO).FirstOrDefaultAsync();
                            var poheadertransnumber = await _context.purchaseOrderHeaders.OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                            //if (poheader.PurchaseOrderNO != null)
                            //{
                            //    PurchaseOrderNO = Convert.ToString(int.Parse(poheader.PurchaseOrderNO) + 1);
                            //}

                            // if (obj.FromPr.HasValue() && obj.FromPr == "FPR")
                            if (cObj.PurchaseOrderNO is null)
                            {

                                poSeq = sequence.PONumber + 1;
                                sequence.PONumber = poSeq;
                                PurchaseOrderNO = $"{sequence.Id}-{poSeq.ToString()}";
                                FinTrans = (int.Parse(poheadertransnumber.TranNumber) + 1).ToString();
                            }
                            else
                            {
                                PurchaseOrderNO = cObj.PurchaseOrderNO;
                                FinTrans = poheadertransnumber.TranNumber;
                            }

                            //PurchaseOrderNO = cObj.PurchaseOrderNO;
                            //FinTrans = poheadertransnumber.TranNumber;


                        }
                        //}

                    }

                    else
                    {

                        if (obj.Trantype == "1")
                        {


                            var poheader = await _context.purchaseOrderHeaders.OrderBy(e => e.Id).LastOrDefaultAsync();
                            // var poheaderSeq = await _context.purchaseOrderHeaders.Where(e => e.PurchaseRequestNO != null).OrderByDescending(e => e.PurchaseRequestNO).FirstOrDefaultAsync();
                            //var poheader = await _context.purchaseOrderHeaders.OrderBy(e => e.PurchaseRequestNO).LastOrDefaultAsync();
                            var poheadertransnumber = await _context.purchaseOrderHeaders.OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                            //if (poheaderSeq is not null)
                            //    PurchaseRequestNO = Convert.ToString(int.Parse(poheaderSeq.PurchaseRequestNO.HasValue() ? poheaderSeq.PurchaseRequestNO : "10001") + 1);
                            //else
                            //    PurchaseRequestNO = Convert.ToString(10001);

                            poSeq = sequence.PRNumber + 1;
                            sequence.PRNumber = poSeq;
                            PurchaseRequestNO = $"{sequence.Id}-{poSeq.ToString()}";

                            if (poheader != null)
                            {
                                //PurchaseRequestNO = Convert.ToString(int.Parse(poheaderSeq.PurchaseRequestNO.HasValue() ? poheaderSeq.PurchaseRequestNO : "10001") + 1);
                                transnumber = Convert.ToString(int.Parse(poheadertransnumber.TranNumber) + 1);
                            }
                            else
                            {
                                transnumber = Convert.ToString(10001);
                            }

                            cObj.TranNumber = transnumber;
                        }
                        if (obj.Trantype == "2")
                        {
                            var poheader = await _context.purchaseOrderHeaders.OrderBy(e => e.PurchaseOrderNO).LastOrDefaultAsync();
                            //   var poheaderSeq = await _context.purchaseOrderHeaders.Where(e => e.PurchaseOrderNO != null).OrderByDescending(e => e.PurchaseOrderNO).FirstOrDefaultAsync();
                            var poheadertransnumber = await _context.purchaseOrderHeaders.OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                            //if (poheaderSeq is not null)
                            //    PurchaseOrderNO = Convert.ToString(int.Parse(poheaderSeq.PurchaseOrderNO.HasValue() ? poheaderSeq.PurchaseOrderNO : "10001") + 1);
                            //else
                            //    PurchaseOrderNO = Convert.ToString(10001);

                            poSeq = sequence.PONumber + 1;
                            sequence.PONumber = poSeq;
                            PurchaseOrderNO = $"{sequence.Id}-{poSeq.ToString()}";


                            if (poheader != null)
                            {
                                transnumber = Convert.ToString(int.Parse(poheadertransnumber.TranNumber) + 1);
                                FinTrans = poheadertransnumber.TranNumber;
                            }
                            else
                            {
                                transnumber = Convert.ToString(10001);

                            }

                            cObj.TranNumber = transnumber;
                        }

                        _context.Sequences.Update(sequence);
                        await _context.SaveChangesAsync();

                    }

                    if (obj.Trantype == "1")
                        cObj.PurchaseRequestNO = PurchaseRequestNO;
                    if (obj.Trantype == "2")
                        cObj.PurchaseOrderNO = PurchaseOrderNO;

                    cObj.VenCatCode = obj.VenCatCode;
                    cObj.Trantype = obj.Trantype;
                    //cObj.TranDate = DateTime.Now;
                    cObj.TranDate = obj.TranDate;
                    cObj.DeliveryDate = obj.DeliveryDate;
                    cObj.InvRefNumber = obj.InvRefNumber;
                    //cObj.CancelDate = obj.CancelDate;
                    cObj.CompCode = obj.CompCode;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.VendCode = obj.VendCode;
                    //cObj.RFQContractNum = obj.RFQContractNum;
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

                    if (!obj.TranShipMode.HasValue() && !obj.ShipmentMode.HasValue())
                    {
                        cObj.TranShipMode = null;
                    }
                    else if (!obj.TranShipMode.HasValue())
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


                    cObj.TranTotalCost = obj.TranTotalCost;
                    cObj.TranDiscPer = obj.TranDiscPer;
                    cObj.TranDiscAmount = obj.TranDiscAmount;
                    cObj.Taxes = obj.Taxes;
                    cObj.WHCode = obj.WHCode;
                    cObj.IsPaid = false;
                    cObj.ISGRN = false;



                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        //cObj.Id = 0;

                        _context.purchaseOrderHeaders.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.purchaseOrderHeaders.AddAsync(cObj);
                    }


                    await _context.SaveChangesAsync();

                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.purchaseOrderDetails.Where(e => e.TranId == transnumber).ToListAsync();
                        _context.purchaseOrderDetails.RemoveRange(oldAuthList);

                        List<TblPopTrnPurchaseOrderDetails> UOMList = new();
                        int i = 1;
                        string trans = "";
                        var PoDetialTransNumber = await _context.purchaseOrderDetails.OrderByDescending(e => e.TranNumber).FirstOrDefaultAsync();
                        //var PoDetialTransNumber = await _context.purchaseOrderDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var auth in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.TranNumber) + i++);
                            else
                                trans = Convert.ToString(int.Parse(transnumber) + i++);

                            InvTrans = trans;
                            //FinTrans = transnumber;
                            TblPopTrnPurchaseOrderDetails UOMItem = new()
                            {

                                TranNumber = trans,
                                TranId = transnumber,
                                TranDate = DateTime.UtcNow,
                                VendCode = obj.VendCode,
                                CompCode = obj.CompCode,
                                BranchCode = obj.BranchCode,
                                TranVendorCode = obj.VendCode,
                                ItemTracking = 0,
                                TranItemCode = auth.TranItemCode,
                                TranItemName = auth.TranItemName,
                                TranItemName2 = auth.TranItemName2,
                                TranItemQty = auth.TranItemQty,
                                TranItemUnitCode = auth.TranItemUnitCode,
                                TranUOMFactor = auth.TranUOMFactor,
                                TranItemCost = auth.TranItemCost,
                                TranTotCost = auth.TranTotCost,
                                DiscPer = auth.DiscPer,
                                DiscAmt = auth.DiscAmt,
                                ItemTax = auth.ItemTax,
                                ItemTaxPer = auth.ItemTaxPer,
                                TaxAmount = auth.TaxAmount,
                                IsActive = true,
                                CreatedOn = DateTime.UtcNow
                            };
                            UOMList.Add(UOMItem);
                        }
                        await _context.purchaseOrderDetails.AddRangeAsync(UOMList);

                        await _context.SaveChangesAsync();




                    }
                    if (obj.Trantype == "2")
                    {

                    }

                    #region GRNINsert

                    #endregion

                    Log.Info("----Info CreatePurchaseRequest method Exit----");
                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, cObj.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreatePurchaseRequest Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }

        }
        private string GetPrefixLen(int len) => "0000000000".Substring(0, len);
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }

    #endregion
    #region GetPagedList

    public class GetPurchaseRequestList : IRequest<PaginatedList<PurchaseRequestPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetPurchaserequestListHandler : IRequestHandler<GetPurchaseRequestList, PaginatedList<PurchaseRequestPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaserequestListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PurchaseRequestPaginationDto>> Handle(GetPurchaseRequestList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var code = request.Input.Code;
            var isArab = request.User.Culture.IsArab();

            //var oprAuths = _context.PurAuthorities.AsNoTracking();
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "PR").AsNoTracking();
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            var enquiryHeads = _context.purchaseOrderHeaders.AsNoTracking();
            var enquiryDetails = _context.purchaseOrderDetails.AsNoTracking();
            var itemsList = _context.InvItemMaster.AsNoTracking().Select(e => new { e.ItemCode, e.ShortName, e.ShortNameAr });

            var surveyors = _context.OprSurveyors.AsNoTracking();

            var list = _context.purchaseOrderHeaders.AsNoTracking();


            if (code.HasValue())
            {
                var tranIdList = _context.purchaseOrderDetails.Where(e => e.TranItemCode == code).Select(e => e.TranId);
                list = list.Where(e => tranIdList.Any(tid => tid == e.TranNumber));
            }


            var requestList = await list.Where(e => (e.PurchaseRequestNO != (null) &&
                 (
                   e.VendCode.Contains(search) ||
                   e.PurchaseRequestNO.Contains(search) ||
                   e.Vendor.VendName.Contains(search) ||
                   EF.Functions.Like(e.Vendor.VendArbName, "%" + search + "%"))
                 ))
                  .OrderBy(request.Input.OrderBy).Select(d => new PurchaseRequestPaginationDto
                  {
                      Id = d.Id,
                      PurchaseRequestNO = d.PurchaseRequestNO,
                      TranDate = d.TranDate,
                      InvRefNumber = d.InvRefNumber,
                      BranchCode = d.BranchCode,
                      VendCode = d.VendCode,
                      VendName = isArab ? d.Vendor.VendArbName : d.Vendor.VendName,
                      PaymentID = d.PaymentID,
                      TAXId = d.TAXId,
                      TotalAmount = d.TranTotalCost + d.Taxes,
                      Remarks = d.Remarks,
                      TranCreateUser = d.TranCreateUser,

                      ItemNames = itemsList.Where(item => enquiryDetails.Where(e => e.TranId == d.TranNumber).Select(e => e.TranItemCode).Any(e => e == item.ItemCode))
                                         .Select(item => isArab ? item.ShortNameAr : item.ShortName).ToList(),

                      //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PR" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).PurchaseRequestNO),
                      //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),                   
                      //HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode && e.AppAuthPurcRequest),
                      //IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),

                      ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.PurchaseRequestNO && e.IsApproved),
                      IsApproved = oprApprvls.Where(e => e.ServiceCode == d.PurchaseRequestNO && e.IsApproved).Any(),
                      HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthPurcRequest),
                      //CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.PurchaseRequestNO && e.IsApproved).Count(),
                      IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),

                  })
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return requestList;
        }

    }

    #endregion
    #region GetPagedPurchaseOrderList

    public class GetPagedPurchaseOrderList : IRequest<PaginatedList<PurchaseOrderPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetPurchaseorderListHandler : IRequestHandler<GetPagedPurchaseOrderList, PaginatedList<PurchaseOrderPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseorderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PurchaseOrderPaginationDto>> Handle(GetPagedPurchaseOrderList request, CancellationToken cancellationToken)
        {
            //var search = request.Input.Query;
            //var list = await _context.purchaseOrderHeaders.AsNoTracking()
            //   .Where(e =>
            //                (e.PurchaseOrderNO != (null)
            //                 ))
            //   .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblPopTrnPurchaseOrderHeaderDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            var search = request.Input.Query;
            var code = request.Input.Code;
            var isArab = request.User.Culture.IsArab();

            //var oprAuths = _context.PurAuthorities.AsNoTracking();
            //var oprApprvls = _context.TblPurTrnApprovalsList.AsNoTracking();
            //var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "PO").AsNoTracking();
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            var enquiryHeads = _context.purchaseOrderHeaders.AsNoTracking();
            var enquiryDetails = _context.purchaseOrderDetails.AsNoTracking();
            var itemsList = _context.InvItemMaster.AsNoTracking().Select(e => new { e.ItemCode, e.ShortName, e.ShortNameAr });

            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = _context.purchaseOrderHeaders.AsNoTracking();


            if (code.HasValue())
            {
                var tranIdList = _context.purchaseOrderDetails.Where(e => e.TranItemCode == code).Select(e => e.TranId);
                list = list.Where(e => tranIdList.Any(tid => tid == e.TranNumber));
            }


            var orderList = await list.Where(e => (e.PurchaseOrderNO != (null) &&
              (
                e.VendCode.Contains(search) ||
                e.PurchaseOrderNO.Contains(search) ||
                e.Vendor.VendName.Contains(search) ||
                EF.Functions.Like(e.Vendor.VendArbName, "%" + search + "%"))
              ))
               .OrderBy(request.Input.OrderBy).Select(d => new PurchaseOrderPaginationDto
               {
                   Id = d.Id,
                   TranNumber = d.TranNumber,
                   PurchaseOrderNO = d.PurchaseOrderNO,
                   TranDate = d.TranDate,
                   InvRefNumber = d.InvRefNumber,
                   BranchCode = d.BranchCode,
                   VendCode = d.VendCode,
                   VendName = isArab ? d.Vendor.VendArbName : d.Vendor.VendName,
                   PaymentID = d.PaymentID,
                   Remarks = d.Remarks,
                   TAXId = d.TAXId,
                   TranTotalCost = d.TranTotalCost,
                   //IsApproved = d.IsApproved,
                   TranCreateUser = d.TranCreateUser,
                   TotalAmount = d.TranTotalCost + d.Taxes,

                   ItemNames = itemsList.Where(item => enquiryDetails.Where(e => e.TranId == d.TranNumber).Select(e => e.TranItemCode).Any(e => e == item.ItemCode))
                                         .Select(item => isArab ? item.ShortNameAr : item.ShortName).ToList(),

                   //Authorities = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode)
                   //? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode) : new TblPurAuthorities
                   //{
                   //    PurchaseOrder = false,
                   //    PurchaseRequest = false,
                   //    PurchaseReturn = false,
                   //}
                   //HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode),
                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PO" && e.IsApproved && e.ServiceCode == d.PurchaseOrderNO.ToString()),

                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PO" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).PurchaseOrderNO),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),
                   //HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode && e.AppAuthPurcOrder),


                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.PurchaseOrderNO && e.IsApproved),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.PurchaseOrderNO && e.IsApproved).Any(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthPurcOrder),
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.PurchaseOrderNO && e.IsApproved).Count(),
                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),
                   ISGRN = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.ISGRN).Any(),

               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);


            foreach (var item in orderList.Items)
            {
                var poItems = _context.purchaseOrderDetails.AsNoTracking()
                .Where(e => e.TranId == item.TranNumber).Select(e => e.TranItemCode);

                var items = _context.InvItemMaster.AsNoTracking()
                          .Where(e => poItems.Any(poItem => poItem == e.ItemCode));

                var expExistingItems = _context.InvItemExpiryBatches.AsNoTracking().Where(e => e.PoNumber == item.PurchaseOrderNO).Select(e => e.ItemCode);
                var serialExistingItems = _context.InvItemSerialBatches.AsNoTracking().Where(e => e.PoNumber == item.PurchaseOrderNO).Select(e => e.ItemCode);

                if (await expExistingItems.AnyAsync())
                    items = items.Where(e => !expExistingItems.Any(poItem => poItem == e.ItemCode));
                if (await serialExistingItems.AnyAsync())
                    items.Where(e => !serialExistingItems.Any(poItem => poItem == e.ItemCode));


                var expItems = await items.AnyAsync(e => e.ItemTracking.ToLower() == "exp");
                var serialItems = await items.AnyAsync(e => e.ItemTracking.ToLower() == "srl");

                if (expItems && !serialItems)
                    item.ExpSerial = 1;
                else if (!expItems && serialItems)
                    item.ExpSerial = 2;
                else if (expItems && serialItems)
                    item.ExpSerial = 3;
            }

            return orderList;

        }

    }

    #endregion

    #region SingleItem

    public class GetPoDetails : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetPoDetailsHandler : IRequestHandler<GetPoDetails, TblPurchaseReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPoDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetPoDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblPurchaseReturntDto obj = new();

            var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            var podItems = _context.purchaseOrderDetails.AsNoTracking()
                          .Where(e => e.TranId == transnumber.TranNumber)
                          .Select(e => e.TranItemCode)
                          .ToList();

            var itemTrackingData = _context.InvItemMaster.AsNoTracking()
                         .Where(e => podItems.Contains(e.ItemCode))
                         .Select(e => e.ItemTracking)
                         .FirstOrDefault();

            if (PoHeader is not null)
            {
                var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
                    .Where(e => e.TranId == PoHeader.TranNumber)
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
                        SerExpTracking = itemTrackingData


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

    public class GetPRDetails : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public string TranNumber { get; set; }
    }

    public class GetPRDetailsHandler : IRequestHandler<GetPRDetails, TblPurchaseReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetPRDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == request.TranNumber);
            TblPurchaseReturntDto obj = new();
            var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);

            var podItems = _context.purchaseOrderDetails.AsNoTracking()
                            .Where(e => e.TranId == transnumber.TranNumber)
                            .Select(e => e.TranItemCode)
                            .ToList();

            var itemTrackingData = _context.InvItemMaster.AsNoTracking()
                         .Where(e => podItems.Contains(e.ItemCode))
                         .Select(e => e.ItemTracking)
                         .FirstOrDefault();


            if (PoHeader is not null)
            {
                var Transid = PoHeader.TranNumber;

                var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
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
                        SerExpTracking = itemTrackingData
                    }).ToListAsync();

                obj.Id = PoHeader.Id;
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
                obj.Taxes = PoHeader.Taxes;
                obj.TranDiscPer = PoHeader.TranDiscPer;
                obj.TranDiscAmount = PoHeader.TranDiscAmount;
                obj.WHCode = PoHeader.WHCode;


            }
            return obj;
        }
    }

    #endregion

    #region Get PRList

    public class GetPurRequestList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetPRListHandler : IRequestHandler<GetPurRequestList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPRListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPurRequestList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPurchaseRequestList method start----");
            //var item = await _context.purchaseOrderHeaders.AsNoTracking().Where(e => (e.PurchaseRequestNO != (null)
            //                ))
            //    .OrderByDescending(e => e.Id)
            //   .Select(e => new CustomSelectListItem { Text = e.PurchaseRequestNO, Value = e.TranNumber })
            //      .ToListAsync(cancellationToken);




            var item = (from m in _context.purchaseOrderHeaders
                        join s in _context.TblPurTrnApprovalsList
                        on m.PurchaseRequestNO equals s.ServiceCode
                        where m.PurchaseRequestNO != (null)
                        group new { m, s } by new { m.PurchaseRequestNO, m.TranNumber } into grp
                        select new CustomSelectListItem
                        {
                            Text = grp.Key.PurchaseRequestNO,
                            Value = grp.Key.TranNumber,
                        });

            if (request.Input.HasValue())
                item = item.Where(e => e.Text.Contains(request.Input) || e.Value.Contains(request.Input));

            Log.Info("----Info GetPurchaseRequestList method Ends----");
            return await item.ToListAsync();
        }
    }

    #endregion


    #region GetPOPrintingOneList

    public class GetPOPrintingOneList : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
        public string Type { get; set; }
    }

    public class GetPOPrintingTwoListHandler : IRequestHandler<GetPOPrintingOneList, TblPurchaseReturntDto>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPOPrintingTwoListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetPOPrintingOneList request, CancellationToken cancellationToken)
        {
            string type = request.Type;
            bool isArab = request.User.Culture.IsArab();
            TblPurchaseReturntDto obj = new();

            if (type.HasValue() && type == "grn")
            {
                var transnumber = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);

                var grnHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (grnHeader is not null)
                {
                    var Transid = grnHeader.TranNumber;

                    var iteminventory = await _context.GRNDetails.
                        Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranNumber = e.TranNumber,
                            TranItemName = isArab ? e.InvItemMaster.ShortNameAr : e.InvItemMaster.ShortName,
                            ItemDescription = isArab ? e.InvItemMaster.ItemDescriptionAr : e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            DiscPer = e.DiscPer,
                            DiscAmt = e.DiscAmt,
                            ItemTax = e.ItemTax,
                            ItemTaxPer = e.ItemTaxPer,
                            TranItemUnitCode = e.InvUoms.UOMName,

                        }).ToListAsync();

                    var shipmentList = await _context.GRNDetails.Where(e => Transid == e.TranId)
                       .Select(e => new ShipmentQuantityDto
                       {
                           TranItemQty = e.TranItemQty,
                           TranItemCost = e.TranItemCost,
                           TranTotCost = e.TranTotCost,
                           ReceivedQty = e.ReceivedQty,
                           ReceivingQty = e.ReceivingQty,
                           BalQty = e.BalQty

                       }).ToListAsync();

                    obj.itemList = iteminventory;
                    obj.ShipmentList = shipmentList;

                    obj.TranNumber = grnHeader.TranNumber;
                    obj.PurchaseOrderNO = grnHeader.PurchaseOrderNO;
                    obj.TranDate = Convert.ToDateTime(grnHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = grnHeader.DeliveryDate;

                    obj.VendCode = grnHeader.VendCode;
                    obj.TranTotalCost = grnHeader.TranTotalCost;
                    obj.Taxes = grnHeader.Taxes;
                    obj.TranDiscAmount = grnHeader.TranDiscAmount;
                    obj.TotalAmount = (grnHeader.TranTotalCost + grnHeader.Taxes - grnHeader.TranDiscAmount);
                    obj.OHCharges = grnHeader.OHCharges;
                    obj.DocNumber = grnHeader.DocNumber;
                    obj.DeliveryDate = grnHeader.DeliveryDate;
                    obj.TranDate = grnHeader.TranDate;

                    //var toWord = new ToWord(obj.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    //obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();




                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == grnHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VendPhone1;
                        obj.WHName = vendor.VendEmail1;
                    }


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            Email = company.Email
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };

                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }
            else if (type.HasValue() && type == "prt")
            {
                var transnumber = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);

                var grnHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (grnHeader is not null)
                {
                    var Transid = grnHeader.TranNumber;

                    var iteminventory = await _context.purchaseReturnDetails.
                        Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranNumber = e.TranNumber,
                            TranItemName = isArab ? e.InvItemMaster.ShortNameAr : e.InvItemMaster.ShortName,
                            ItemDescription = isArab ? e.InvItemMaster.ItemDescriptionAr : e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            DiscPer = e.DiscPer,
                            DiscAmt = e.DiscAmt,
                            ItemTax = e.ItemTax,
                            ItemTaxPer = e.ItemTaxPer,
                            TranItemUnitCode = e.InvUoms.UOMName,

                        }).ToListAsync();

                    var shipmentList = await _context.purchaseReturnDetails.Where(e => Transid == e.TranId)
                       .Select(e => new ShipmentQuantityDto
                       {
                           TranItemQty = e.TranItemQty,
                           TranItemCost = e.TranItemCost,
                           TranTotCost = e.TranTotCost,
                           ReceivedQty = e.ReceivedQty,
                           ReceivingQty = e.ReceivingQty,
                           BalQty = e.BalQty

                       }).ToListAsync();

                    obj.itemList = iteminventory;
                    obj.ShipmentList = shipmentList;

                    obj.TranNumber = grnHeader.TranNumber;
                    obj.PurchaseOrderNO = grnHeader.TranNumber;
                    obj.TranDate = Convert.ToDateTime(grnHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = grnHeader.DeliveryDate;

                    obj.VendCode = grnHeader.VendCode;
                    obj.TranTotalCost = grnHeader.TranTotalCost;
                    obj.Taxes = grnHeader.Taxes;
                    obj.TranDiscAmount = grnHeader.TranDiscAmount;
                    obj.TotalAmount = (grnHeader.TranTotalCost + grnHeader.Taxes - grnHeader.TranDiscAmount);
                    obj.OHCharges = grnHeader.OHCharges;
                    obj.DocNumber = grnHeader.DocNumber;
                    obj.DeliveryDate = grnHeader.DeliveryDate;
                    obj.TranDate = grnHeader.TranDate;

                    //var toWord = new ToWord(obj.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    //obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();




                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == grnHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VendPhone1;
                        obj.WHName = vendor.VendEmail1;
                    }


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            Email = company.Email
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };

                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }
            else
            {
                var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);

                var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.purchaseOrderDetails.
                        Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranNumber = e.TranNumber,
                            TranItemName = isArab ? e.InvItemMaster.ShortNameAr : e.InvItemMaster.ShortName,
                            ItemDescription = isArab ? e.InvItemMaster.ItemDescriptionAr : e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            DiscPer = e.DiscPer,
                            DiscAmt = e.DiscAmt,
                            ItemTax = e.ItemTax,
                            ItemTaxPer = e.ItemTaxPer,
                            TranItemUnitCode = e.InvUoms.UOMName,
                        }).ToListAsync();

                    obj.itemList = iteminventory;

                    obj.TranNumber = PoHeader.TranNumber;
                    obj.PurchaseRequestNO = PoHeader.PurchaseRequestNO;
                    obj.PurchaseOrderNO = type == "PO" ? PoHeader.PurchaseOrderNO : PoHeader.PurchaseRequestNO;
                    obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = PoHeader.DeliveryDate;

                    obj.VendCode = PoHeader.VendCode;
                    obj.TranTotalCost = PoHeader.TranTotalCost;
                    obj.Taxes = PoHeader.Taxes;
                    obj.TranDiscAmount = PoHeader.TranDiscAmount;
                    obj.TotalAmount = (PoHeader.TranTotalCost + PoHeader.Taxes - PoHeader.TranDiscAmount);
                    obj.OHCharges = PoHeader.OHCharges;
                    obj.DocNumber = PoHeader.DocNumber;
                    obj.DeliveryDate = PoHeader.DeliveryDate;
                    obj.TranDate = PoHeader.TranDate;



                    //var toWord = new ToWord(obj.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    //obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();




                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == PoHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VendPhone1;
                        obj.WHName = vendor.VendEmail1;
                    }


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            Email = company.Email
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };

                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }

            return obj;
        }
    }

    #endregion

    #region GetPOPrintingTwoList

    public class GetPOPrintingTwoList : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
        public string Type { get; set; }
    }

    public class GetPOPrintingListHandler : IRequestHandler<GetPOPrintingTwoList, TblPurchaseReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPOPrintingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetPOPrintingTwoList request, CancellationToken cancellationToken)
        {

            string type = request.Type;
            bool isArab = request.User.Culture.IsArab();
            TblPurchaseReturntDto obj = new();
            var users = _context.SystemLogins.AsNoTracking();

            if (type.HasValue() && type == "grn")
            {
                var transnumber = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                var PoHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.GRNDetails
                        .Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranItemName = e.InvItemMaster.ShortName,
                            TranItemName2 = e.InvItemMaster.ShortNameAr,
                            ItemDescription = e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemUnitCode = e.InvUoms.UOMName,
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

                    var shipmentList = await _context.GRNDetails.Where(e => Transid == e.TranId)
                        .Select(e => new ShipmentQuantityDto
                        {
                            TranItemQty = e.TranItemQty,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            ReceivedQty = e.ReceivedQty,
                            ReceivingQty = e.ReceivingQty,
                            BalQty = e.BalQty

                        }).ToListAsync();



                    obj.itemList = iteminventory;
                    obj.ShipmentList = shipmentList;

                    obj.VenCatCode = PoHeader.VenCatCode;
                    obj.TranNumber = PoHeader.TranNumber;
                    obj.PurchaseOrderNO = PoHeader.TranNumber;
                    obj.Trantype = PoHeader.Trantype;
                    obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = PoHeader.DeliveryDate;
                    obj.CompCode = PoHeader.CompCode;
                    obj.BranchCode = PoHeader.BranchCode;
                    obj.InvRefNumber = PoHeader.InvRefNumber;
                    obj.DocNumber = PoHeader.DocNumber;
                    obj.PaymentID = PoHeader.PaymentID;
                    obj.Remarks = PoHeader.Remarks;
                    obj.TAXId = PoHeader.TAXId;
                    obj.TaxInclusive = PoHeader.TaxInclusive;
                    obj.TranTotalCost = PoHeader.TranTotalCost;
                    obj.Taxes = PoHeader.Taxes;
                    obj.TranDiscAmount = PoHeader.TranDiscAmount;
                    obj.TotalAmount = (PoHeader.TranTotalCost + PoHeader.Taxes - PoHeader.TranDiscAmount);
                    obj.PONotes = PoHeader.PONotes;
                    obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                    obj.TranShipMode = PoHeader.TranShipMode;
                    obj.VendCode = PoHeader.VendCode;
                    obj.WHCode = PoHeader.WHCode;


                    var purApprovalUser = await _context.TblPurTrnApprovalsList.OrderByDescending(e => e.Id)
                       .FirstOrDefaultAsync(e => e.ServiceType == "GN" && e.ServiceCode == obj.PurchaseOrderNO);

                    if (purApprovalUser is not null)
                    {
                        obj.Approvers = (await users.FirstOrDefaultAsync(e => e.Id == purApprovalUser.AppAuth)).UserName;
                        obj.ApproverDate = purApprovalUser.CreatedOn;
                    }
                    //var purApprovalUsers = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "GN" && e.ServiceCode == PoHeader.PurchaseOrderNO)
                    //                           .Select(e => e.AppAuth);
                    //obj.Approvers = String.Join(",", await users.Where(e => purApprovalUsers.Any(pid => pid == e.Id)).Select(e => e.UserName).ToListAsync());


                    var toWord = new ToWord(obj.TotalAmount + obj.TranDiscAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();


                    var wareHouse = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.WHCode == PoHeader.WHCode);

                    if (wareHouse is not null)
                        obj.WHName = wareHouse.WHName;

                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == PoHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VATNumber;
                    }

                    var payTearms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == PoHeader.PaymentID);

                    if (payTearms is not null)
                        obj.POTermsName = payTearms.POTermsName;


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };
                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }
            else if (type.HasValue() && type == "prt")
            {
                var transnumber = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                var PoHeader = await _context.purchaseReturnHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.purchaseReturnDetails
                        .Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranItemName = e.InvItemMaster.ShortName,
                            TranItemName2 = e.InvItemMaster.ShortNameAr,
                            ItemDescription = e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemUnitCode = e.InvUoms.UOMName,
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

                    var shipmentList = await _context.purchaseReturnDetails.Where(e => Transid == e.TranId)
                        .Select(e => new ShipmentQuantityDto
                        {
                            TranItemQty = e.TranItemQty,
                            TranItemCost = e.TranItemCost,
                            TranTotCost = e.TranTotCost,
                            ReceivedQty = e.ReceivedQty,
                            ReceivingQty = e.ReceivingQty,
                            BalQty = e.BalQty

                        }).ToListAsync();



                    obj.itemList = iteminventory;
                    obj.ShipmentList = shipmentList;

                    obj.VenCatCode = PoHeader.VenCatCode;
                    obj.TranNumber = PoHeader.TranNumber;
                    obj.PurchaseOrderNO = PoHeader.TranNumber;
                    obj.Trantype = PoHeader.Trantype;
                    obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = PoHeader.DeliveryDate;
                    obj.CompCode = PoHeader.CompCode;
                    obj.BranchCode = PoHeader.BranchCode;
                    obj.InvRefNumber = PoHeader.InvRefNumber;
                    obj.DocNumber = PoHeader.DocNumber;
                    obj.PaymentID = PoHeader.PaymentID;
                    obj.Remarks = PoHeader.Remarks;
                    obj.TAXId = PoHeader.TAXId;
                    obj.TaxInclusive = PoHeader.TaxInclusive;
                    obj.TranTotalCost = PoHeader.TranTotalCost;
                    obj.Taxes = PoHeader.Taxes;
                    obj.TranDiscAmount = PoHeader.TranDiscAmount;
                    obj.TotalAmount = (PoHeader.TranTotalCost + PoHeader.Taxes - PoHeader.TranDiscAmount);
                    obj.PONotes = PoHeader.PONotes;
                    obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                    obj.TranShipMode = PoHeader.TranShipMode;
                    obj.VendCode = PoHeader.VendCode;
                    obj.WHCode = PoHeader.WHCode;


                    var purApprovalUser = await _context.TblPurTrnApprovalsList.OrderByDescending(e => e.Id)
                       .FirstOrDefaultAsync(e => e.ServiceType == "PRT" && e.ServiceCode == PoHeader.TranNumber);

                    if (purApprovalUser is not null)
                    {
                        obj.Approvers = (await users.FirstOrDefaultAsync(e => e.Id == purApprovalUser.AppAuth)).UserName;
                        obj.ApproverDate = purApprovalUser.CreatedOn;
                    }
                    //var purApprovalUsers = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "GN" && e.ServiceCode == PoHeader.PurchaseOrderNO)
                    //                           .Select(e => e.AppAuth);
                    //obj.Approvers = String.Join(",", await users.Where(e => purApprovalUsers.Any(pid => pid == e.Id)).Select(e => e.UserName).ToListAsync());


                    var toWord = new ToWord(obj.TotalAmount + obj.TranDiscAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();


                    var wareHouse = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.WHCode == PoHeader.WHCode);

                    if (wareHouse is not null)
                        obj.WHName = wareHouse.WHName;

                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == PoHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VATNumber;
                    }

                    var payTearms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == PoHeader.PaymentID);

                    if (payTearms is not null)
                        obj.POTermsName = payTearms.POTermsName;


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };
                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }
            else
            {
                var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.purchaseOrderDetails.
                        Include(e => e.InvItemMaster)
                        .Include(e => e.InvUoms)
                        .AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnPurchaseOrderDetailsDto
                        {
                            TranItemCode = e.TranItemCode,
                            TranItemName = e.InvItemMaster.ShortName,
                            TranItemName2 = e.InvItemMaster.ShortNameAr,
                            ItemDescription = e.InvItemMaster.ItemDescription,
                            TranItemQty = e.TranItemQty,
                            TranItemUnitCode = e.InvUoms.UOMName,
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

                    obj.itemList = iteminventory;

                    obj.VenCatCode = PoHeader.VenCatCode;
                    obj.TranNumber = PoHeader.TranNumber;
                    obj.PurchaseOrderNO = type == "PO" ? PoHeader.PurchaseOrderNO : PoHeader.PurchaseRequestNO;
                    obj.Trantype = PoHeader.Trantype;
                    obj.TranDate = Convert.ToDateTime(PoHeader.TranDate.ToShortDateString());
                    obj.DeliveryDate = PoHeader.DeliveryDate;
                    obj.CompCode = PoHeader.CompCode;
                    obj.BranchCode = PoHeader.BranchCode;
                    obj.InvRefNumber = PoHeader.InvRefNumber;
                    obj.DocNumber = PoHeader.DocNumber;
                    obj.PaymentID = PoHeader.PaymentID;
                    obj.Remarks = PoHeader.Remarks;
                    obj.TAXId = PoHeader.TAXId;
                    obj.TaxInclusive = PoHeader.TaxInclusive;
                    obj.TranTotalCost = PoHeader.TranTotalCost;
                    obj.Taxes = PoHeader.Taxes;
                    obj.TranDiscAmount = PoHeader.TranDiscAmount;
                    obj.TotalAmount = (PoHeader.TranTotalCost + PoHeader.Taxes - PoHeader.TranDiscAmount);
                    obj.PONotes = PoHeader.PONotes;
                    obj.TranCurrencyCode = PoHeader.TranCurrencyCode;
                    obj.TranShipMode = PoHeader.TranShipMode;
                    obj.VendCode = PoHeader.VendCode;
                    obj.WHCode = PoHeader.WHCode;


                    string ServiceType = (type == "PO" ? "PO" : "PR");

                    var purApprovalUser = await _context.TblPurTrnApprovalsList.OrderByDescending(e => e.Id)
                        .FirstOrDefaultAsync(e => e.ServiceType == ServiceType && e.ServiceCode == obj.PurchaseOrderNO);

                    if (purApprovalUser is not null)
                    {
                        obj.Approvers = (await users.FirstOrDefaultAsync(e => e.Id == purApprovalUser.AppAuth)).UserName;
                        obj.ApproverDate = purApprovalUser.CreatedOn;
                    }
                    //obj.Approvers = String.Join(",", await users.Where(e => purApprovalUsers.Any(pid => pid == e.Id)).Select(e => e.UserName).ToListAsync());

                    var toWord = new ToWord(obj.TotalAmount + obj.TranDiscAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                    obj.TotalAmountWord = isArab ? toWord.ConvertToArabic() : toWord.ConvertToEnglish();


                    var wareHouse = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.WHCode == PoHeader.WHCode);

                    if (wareHouse is not null)
                        obj.WHName = wareHouse.WHName;

                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == PoHeader.VendCode);
                    if (vendor is not null)
                    {
                        obj.VendName = isArab ? vendor.VendArbName : vendor.VendName;
                        obj.VendAddress = vendor.VendAddress1;
                        obj.VATNumber = vendor.VATNumber;
                    }

                    var payTearms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == PoHeader.PaymentID);

                    if (payTearms is not null)
                        obj.POTermsName = payTearms.POTermsName;


                    var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                  .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                    var company = branch?.SysCompany;

                    if (company is not null)
                    {
                        obj.Company = new()
                        {
                            CompanyName = company.CompanyName,
                            CompanyAddress = company.CompanyAddress,
                            Phone = company.Phone,
                            Mobile = company.Mobile,
                            LogoURL = company.LogoURL,
                            BranchName = branch.BranchName,
                            //ledger.Fax = company.;
                            //ledger.PoBox = company.;
                        };
                    }

                    //obj.trantotalcost = iteminventory;
                    //obj.trandiscamount = iteminventory;
                    //obj.itemList = iteminventory;

                }
            }

            return obj;
        }
    }

    #endregion


    #region Delete
    public class DeletePOList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePOQueryHandler : IRequestHandler<DeletePOList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePOQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePOList request, CancellationToken cancellationToken)
        {
            try
            {
                var Pode = await _context.purchaseOrderHeaders.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblPopTrnPurchaseOrderDetails podetails;
                podetails = _context.purchaseOrderDetails.Where(d => d.TranId == Pode.TranNumber).First();
                _context.Entry(podetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblPopTrnPurchaseOrderHeader PoHeader1;
                PoHeader1 = _context.purchaseOrderHeaders.Where(d => d.TranNumber == Pode.TranNumber).First();
                _context.Entry(PoHeader1).State = EntityState.Deleted;
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
            //using (var transaction = await _context.Database.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        Log.Info("----Info delte method start----");

            //        if (request.Id > 0)
            //        {
            //            var PoID = await _context.purchaseOrderHeaders.FirstOrDefaultAsync(e => e.Id == request.Id);
            //            var PoDetails = await _context.purchaseOrderDetails.FirstOrDefaultAsync(e => e.TranId == PoID.TranNumber);
            //            _context.Remove(PoDetails);
            //            var PoHeader = await _context.purchaseOrderHeaders.Where(e => e.TranNumber == PoID.TranNumber).ToListAsync();
            //            _context.purchaseOrderHeaders.RemoveRange(PoHeader);


            //            await _context.SaveChangesAsync();
            //            await transaction.CommitAsync();



            //            var adv1 = _context.purchaseOrderDetails.Include(b => b.TranNumber)
            //.FirstOrDefault(b => b.TranNumber == PoID.TranNumber);
            //            _context.purchaseOrderDetails.Remove(adv1);
            //            var adv = _context.purchaseOrderHeaders.Include(b => b.TranNumber)
            //    .FirstOrDefault(b => b.TranNumber == PoID.TranNumber);
            //            _context.purchaseOrderHeaders.Remove(adv);


            //            return request.Id;


            //        }
            //        return 0;
            //    }
            //    catch (Exception ex)
            //    {
            //        await transaction.RollbackAsync();
            //        Log.Error("Error in delete Method");
            //        Log.Error("Error occured time : " + DateTime.UtcNow);
            //        Log.Error("Error message : " + ex.Message);
            //        Log.Error("Error StackTrace : " + ex.StackTrace);
            //        return 0;
            //    }
            //}

        }
    }

    #endregion
    #region ProductVenPriceItem

    public class ProductVenPriceItem : IRequest<ProductVendorDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Vencode { get; set; }


    }

    public class ProductVenPriceItemItemHandler : IRequestHandler<ProductVenPriceItem, ProductVendorDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductVenPriceItemItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductVendorDTO> Handle(ProductVenPriceItem request, CancellationToken cancellationToken)
        {
            //var itemcode = request.ItemList.Split('_')[0];
            //var ItemUOM = request.ItemList.Split('_')[1];

            Log.Info("----Info ProductVenPriceItem method start----");
            var item = await _context.VendorMasters.AsNoTracking()
                 .Where(e =>
                            (e.VendCode.Contains(request.Vencode)
                             ))
               .Select(Product => new ProductVendorDTO
               {
                   VendName = Product.VendName,
                   PoTermsCode = Product.PoTermsCode
               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info ProductVenPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region ProductTaxPriceItem

    public class ProductTaxPriceItem : IRequest<List<ProductTaxDTO>>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }


    }

    public class ProductTaxPriceItemItemHandler : IRequestHandler<ProductTaxPriceItem, List<ProductTaxDTO>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductTaxPriceItemItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ProductTaxDTO>> Handle(ProductTaxPriceItem request, CancellationToken cancellationToken)
        {

            Log.Info("----Info ProductTaxPriceItem method start----");
            //var item = await _context.InvItemMaster.AsNoTracking()
            //     .Where(e =>
            //                (e.ItemCode.Contains(request.ItemCode)
            //                 ))
            //   .Select(Product => new ProductTaxDTO
            //   {
            //       ItemTaxperc = Product.ItemTaxCode

            //   })
            //      .FirstOrDefaultAsync(cancellationToken);

            var item = (from m in _context.InvItemMaster
                        join s in _context.SystemTaxes
                        on m.ItemTaxCode equals s.TaxCode
                        where m.ItemCode == request.ItemCode
                        select new ProductTaxDTO
                        {
                            ItemTaxperc = s.Taxper01,
                            ShortName = m.ShortName,
                        }).ToList();

            Log.Info("----Info ProductTaxPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region ItemTaxPriceItem

    public class ItemTaxPriceItem : IRequest<List<ItemTaxDTO>>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }


    }

    public class ItemTaxPriceItemItemHandler : IRequestHandler<ItemTaxPriceItem, List<ItemTaxDTO>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ItemTaxPriceItemItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ItemTaxDTO>> Handle(ItemTaxPriceItem request, CancellationToken cancellationToken)
        {

            Log.Info("----Info ProductTaxPriceItem method start----");
            var item = (from m in _context.InvItemMaster
                        join s in _context.SystemTaxes
                        on m.ItemTaxCode equals s.TaxCode
                        where m.ItemCode == request.ItemCode
                        select new ItemTaxDTO
                        {
                            Desc = m.ItemDescription,
                            Unit = m.ItemBaseUnit,
                            price = m.ItemAvgCost

                        }).ToList();

            Log.Info("----Info ProductTaxPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region Account Posting

    public class AccountsPosting : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetAccountPostHandler : IRequestHandler<AccountsPosting, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountPostHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(AccountsPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var PurchaseRequestNO = string.Empty;
                var PurchaseOrderNO = string.Empty;
                string InvTrans = string.Empty;
                string FinTrans = string.Empty;
                var inoviceIds = 0;
                int invoiceSeq = 0;

                var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                TblPurchaseReturntDto obj = new();
                var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
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
                    obj.Taxes = PoHeader.Taxes;
                    obj.WHCode = PoHeader.WHCode;

                    #region Ispaid 
                    var Paid = _context.purchaseOrderHeaders.Where(e => e.TranNumber == FinTrans);
                    var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                    POPaid.IsPaid = true;
                    _context.purchaseOrderHeaders.Update(POPaid).Property(x => x.Id).IsModified = false; ;
                    //_context.purchaseOrderHeaders.Update(POPaid);
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

                        // decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);
                        decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                        // decimal? ItemAvgCost = await cInvItems.SumAsync(e => e.ItemAvgCost);
                        decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);

                        decimal itmAvgcost = 0, originalTranItemQty = 0;
                        // var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == invtItem.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                        //if (PoDetails is not null)
                        //    itmAvgcost = (decimal)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))));
                        //else
                        //    itmAvgcost = (decimal)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty))));


                        originalTranItemQty = invtItem.TranItemQty;
                        foreach (var itemCode in cItemIds)
                        {
                            var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == itemCode);
                            decimal itemAvgCost = 0;
                            if (invtUOM.ItemConvFactor > 1)
                            {
                                invtItem.TranItemQty = invtItem.TranItemQty * invtUOM.ItemConvFactor;
                            }
                            else
                            {
                                invtItem.TranItemQty = invtItem.TranItemQty;
                            }

                            decimal tranItemCost = invtItem.TranItemCost - (invtItem.TranItemCost * invtItem.DiscPer) / 100;
                            //cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));

                            if (invtUOM.ItemConvFactor > 1)
                            {
                                itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)(invtItem.TranItemQty / invtUOM.ItemConvFactor)))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));
                            }
                            else
                            {
                                itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));

                            }



                            oldInventory.ItemAvgCost = itemAvgCost;


                            //if (oldInventory is not null)
                            //else
                            //    oldInventory.ItemAvgCost = ((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty)));


                            itmAvgcost = oldInventory.ItemAvgCost;
                            oldInventory.QtyOH = (decimal)oldInventory.QtyOH + invtItem.TranItemQty;

                            oldInventory.ItemLastPOCost = ((decimal)ItemLastPOCost + invtItem.TranItemCost);
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
                        }

                        ////foreach (var invIds in cItemMasterIds)
                        ////{
                        ////    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
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
                            TranType = "2",
                            //TranNumber = InvTrans,
                            TranNumber = FinTrans,
                            TranUnit = invtItem.TranItemUnitCode,
                            TranQty = originalTranItemQty,
                            unitConvFactor = invtItem.TranUOMFactor,
                            TranTotQty = invtItem.TranItemQty,
                            TranPrice = invtItem.TranItemCost,
                            //ItemAvgCost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))),
                            ItemAvgCost = itmAvgcost,
                            TranRemarks = "Inserted through PO",
                            IsActive = false,
                            CreatedOn = DateTime.Now
                        };
                        _context.InvItemInventoryHistory.Add(obj1);
                        //}
                        _context.SaveChanges();
                        #endregion Inventory History

                    }
                    #endregion
                    #region FinancialTriggering
                    try
                    {
                        Log.Info("----Info CreateApInvoiceSettlement method start----");

                        //var input = request.Input;
                        var Finobj = await _context.purchaseOrderHeaders.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                        var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);


                        //if (await _context.TrnVendorInvoices.AnyAsync(e => e.InvoiceId == request.id && e.LoginId == request.User.UserId && e.IsPaid))
                        //    return ApiMessageInfo.Status(1);




                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        if (invSeq is null)
                        {
                            invoiceSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CreditSeq = invoiceSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeq = invSeq.CreditSeq + 1;
                            invSeq.CreditSeq = invoiceSeq;
                            _context.Sequences.Update(invSeq);
                        }

                        //Finobj.TranNumber = invoiceSeq.ToString();
                        //_context.purchaseOrderHeaders.Update(Finobj);
                        //await _context.SaveChangesAsync();

                        var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        //var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.BranchCode);
                        var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = inoviceIds,
                            //FinAcCode = IsNotCreditPay(request.Input.PaymentType) ? payCode.FinPayAcIntgrAC : vendor.VendArAcCode,

                            FinAcCode = IsNotCreditPay("cash") ? invDistGroup.InvDefaultAPAc : vendor.VendArAcCode,
                            //FinAcCode = IsNotCreditPay("cash") ? payCode.FinPayAcIntgrAC : vendor.VendArAcCode,
                            //FinAcCode = invDistGroup.InvAssetAc,
                            //FinAcCode = IsNotCreditPay("cash") ? invDistGroup.InvAssetAc : vendor.VendArAcCode,

                            CrAmount = Finobj.TranTotalCost + Finobj.Taxes,
                            DrAmount = 0,
                            Source = "PO",
                            Gl = string.Empty,
                            Type = IsNotCreditPay("cash") ? "paycode" : "Vendor",
                            // Type = IsNotCreditPay("cash") ? "paycode" : "Vendor",
                            //Type = IsNotCreditPay(request.Input.PaymentType) ? "paycode" : "Vendor",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = inoviceIds,
                            //FinAcCode = vendor.VendDefExpAcCode,
                            //FinAcCode = vendor.VendDefExpAcCode,//InventoryAccount 10201001
                            FinAcCode = invDistGroup.InvAssetAc,

                            CrAmount = 0,
                            DrAmount = Finobj.TranTotalCost,// - Finobj.Taxes,
                            Source = "PO",
                            Gl = string.Empty,
                            Type = "Cost",
                            //Type = "Income",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);


                        var invoiceItem = await _context.purchaseOrderDetails.FirstOrDefaultAsync(e => e.TranId == Finobj.TranNumber);
                        var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.ItemTaxPer).ToString());

                        if (tax is null)
                            throw new NullReferenceException("Tax is empty");

                        TblFinTrnDistribution distribution3 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = tax?.InputAcCode01,
                            CrAmount = 0,
                            DrAmount = Finobj.Taxes,
                            Source = "PO",
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


                        //Storing in  JournalVoucher tables

                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)Finobj.CompCode,
                            BranchCode = Finobj.BranchCode,
                            Batch = string.Empty,
                            Source = "PO",
                            Remarks = Finobj.Remarks,
                            Narration = Finobj.PONotes ?? Finobj.Remarks,
                            JvDate = DateTime.Now,
                            //Amount = Finobj.TranTotalCost ?? 0,
                            Amount = Finobj.TranTotalCost + Finobj.Taxes,
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
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = (int)Finobj.CompCode,
                                    BranchCode = Finobj.BranchCode,
                                    JvDate = DateTime.Now,
                                    TranSource = "PO",
                                    Trantype = "Invoice",
                                    DocNum = Finobj.InvRefNumber,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = Finobj.Remarks,
                                    JournalVoucherId = jvId,
                                    IsApproved = true,
                                };
                                jvApprovalList.Add(approval);
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
                                Source = "PO",
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
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in CreateApInvoiceSettlement Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return ApiMessageInfo.Status(0);
                        //return false;
                    }
                    #endregion

                    #region Purchase Invoice Inserting 
                    try
                    {
                        Log.Info("----Info CreateUpdateInvoice method start----");

                        string SpCreditNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                        //invoiceNumber = await _context.TranVenInvoices.CountAsync();
                        //invoiceNumber += 1;
                        var VendorID = _context.VendorMasters.Where(c => c.VendCode == obj.VendCode).Single();
                        TblTranVenInvoice Invoice = new();
                        Invoice = new()
                        {
                            SpCreditNumber = string.Empty,
                            CreditNumber = invoiceSeq.ToString(),
                            InvoiceDate = Convert.ToDateTime(PoHeader.TranDate),
                            InvoiceDueDate = Convert.ToDateTime(PoHeader.DeliveryDate),
                            CompanyId = PoHeader.CompCode,


                            //SubTotal = obj.SubTotal,
                            //DiscountAmount = obj.DiscountAmount ?? 0,
                            //AmountBeforeTax = obj.AmountBeforeTax ?? 0,
                            //TaxAmount = obj.TaxAmount,
                            //TotalAmount = obj.TotalAmount,
                            //TotalPayment = obj.TotalPayment ?? 0,
                            //AmountDue = obj.AmountDue ?? 0,


                            SubTotal = PoHeader.TranTotalCost,
                            DiscountAmount = PoHeader.TranDiscAmount,
                            AmountBeforeTax = 0,
                            TaxAmount = PoHeader.Taxes,
                            TotalAmount = PoHeader.TranTotalCost,
                            TotalPayment = 0,
                            AmountDue = 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(PoHeader.TranDate),
                            CreatedBy = request.User.UserId,
                            //CustomerId = PoHeader.VendCode,
                            CustomerId = VendorID.Id,

                            InvoiceStatus = "Open",
                            TaxIdNumber = PoHeader.TAXId,
                            //InvoiceModule = obj.InvoiceModule,
                            InvoiceModule = "POMgt",
                            InvoiceNotes = PoHeader.PONotes,
                            Remarks = obj.Remarks,
                            InvoiceRefNumber = PoHeader.InvRefNumber,
                            //LpoContract = obj.LpoContract,
                            //VatPercentage = obj.VatPercentage ?? 0,
                            LpoContract = "",
                            VatPercentage = Convert.ToDecimal(0),
                            PaymentTerms = PoHeader.PaymentID,
                            BranchCode = PoHeader.BranchCode,
                            ServiceDate1 = PoHeader.DeliveryDate.ToString(),
                            //IsCreditConverted = obj.IsCreditConverted,
                            //InvoiceStatusId = obj.InvoiceStatusId
                            IsCreditConverted = false,
                            InvoiceStatusId = 1

                        };

                        await _context.TranVenInvoices.AddAsync(Invoice);
                        await _context.SaveChangesAsync();
                        //var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == obj.ServiceCode);
                        TblFinTrnVendorApproval approval = new()
                        {
                            CompanyId = request.User.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "PO",
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
                        var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == obj.PaymentID);
                        TblFinTrnVendorInvoice cInvoice = new()
                        {
                            CompanyId = (int)obj.CompCode,
                            BranchCode = obj.BranchCode,
                            InvoiceNumber = invoiceSeq.ToString(),// invoiceSeq.ToString(),
                            InvoiceDate = obj.TranDate,
                            CreditDays = paymentTerms.POTermsDueDays,
                            DueDate = obj.DeliveryDate,
                            TranSource = "PO",
                            Trantype = "Invoice",
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
                            TranSource = "PO",
                            Trantype = "Invoice",
                            TranNumber = PoHeader.TranNumber,// invoiceSeq.ToString(),
                            VendCode = obj.VendCode,
                            DocNum = "DocNum",
                            ReferenceNumber = obj.InvRefNumber,
                            PaymentType = "check",
                            PamentCode = "paycode",
                            CheckNumber = invoiceSeq.ToString(),
                            Remarks1 = obj.Remarks,
                            Remarks2 = obj.Remarks,
                            LoginId = request.User.UserId,
                            DrAmount = obj.TranTotalCost + obj.Taxes,
                            CrAmount = 0,
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
                                    //UnitPrice = obj1.TranItemCost,
                                    UnitPrice = obj1.TranItemCost,
                                    SubTotal = obj1.TranTotCost,
                                    DiscountAmount = obj1.DiscAmt,
                                    AmountBeforeTax = obj1.TranItemCost,
                                    TaxAmount = obj1.TaxAmount,
                                    TotalAmount = obj1.TranTotCost + obj1.TaxAmount,
                                    IsDefaultConfig = true,
                                    CreatedOn = Convert.ToDateTime(obj.TranDate),
                                    //CreatedBy = (int),
                                    CreatedBy = 1,
                                    Description = obj1.TranItemName,
                                    TaxTariffPercentage = obj1.ItemTaxPer,
                                    InvoiceType = 2,
                                    //Discount = 1
                                    Discount = 1
                                };
                                invoiceItemsList.Add(InvoiceItem);
                            }
                            if (invoiceItemsList.Count > 0)
                            {
                                await _context.TranVenInvoiceItems.AddRangeAsync(invoiceItemsList);
                                await _context.SaveChangesAsync();

                                var editInvoice = await _context.TranVenInvoices.FirstOrDefaultAsync(e => e.Id == inoviceId);
                                editInvoice.TaxAmount = invoiceItemsList.Sum(item => item.TaxAmount);
                                editInvoice.TotalAmount = invoiceItemsList.Sum(item => item.TotalAmount);
                                await _context.SaveChangesAsync();
                            }
                        }

                        //await transaction.CommitAsync();

                        // return ApiMessageInfo.Status(1, inoviceId);
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
                    #endregion
                    #region Purchase test
                    try
                    {
                        Log.Info("----Info CreateUpdateInvoice method start----");

                        string SpCreditNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                        //invoiceNumber = await _context.TranVenInvoices.CountAsync();
                        //invoiceNumber += 1;
                        var VendorID = _context.VendorMasters.Where(c => c.VendCode == obj.VendCode).Single();
                        TblTranPurcInvoice Invoice = new();
                        Invoice = new()
                        {
                            SpCreditNumber = SpCreditNumber,
                            CreditNumber = string.Empty,
                            InvoiceDate = Convert.ToDateTime(PoHeader.TranDate),
                            InvoiceDueDate = Convert.ToDateTime(PoHeader.DeliveryDate),
                            CompanyId = PoHeader.CompCode,


                            //SubTotal = obj.SubTotal,
                            //DiscountAmount = obj.DiscountAmount ?? 0,
                            //AmountBeforeTax = obj.AmountBeforeTax ?? 0,
                            //TaxAmount = obj.TaxAmount,
                            //TotalAmount = obj.TotalAmount,
                            //TotalPayment = obj.TotalPayment ?? 0,
                            //AmountDue = obj.AmountDue ?? 0,


                            SubTotal = 0,
                            DiscountAmount = 0,
                            AmountBeforeTax = 0,
                            TaxAmount = 0,
                            TotalAmount = PoHeader.TranTotalCost,
                            TotalPayment = 0,
                            AmountDue = 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(PoHeader.TranDate),
                            CreatedBy = request.User.UserId,
                            //CustomerId = PoHeader.VendCode,
                            CustomerId = VendorID.Id,

                            InvoiceStatus = "Open",
                            TaxIdNumber = PoHeader.TAXId,
                            //InvoiceModule = obj.InvoiceModule,
                            InvoiceModule = "",
                            InvoiceNotes = PoHeader.PONotes,
                            Remarks = obj.Remarks,
                            InvoiceRefNumber = PoHeader.InvRefNumber,
                            //LpoContract = obj.LpoContract,
                            //VatPercentage = obj.VatPercentage ?? 0,
                            LpoContract = "",
                            VatPercentage = Convert.ToDecimal(0),
                            PaymentTerms = PoHeader.PaymentID,
                            BranchCode = PoHeader.BranchCode,
                            ServiceDate1 = PoHeader.DeliveryDate.ToString(),
                            //IsCreditConverted = obj.IsCreditConverted,
                            //InvoiceStatusId = obj.InvoiceStatusId
                            IsCreditConverted = true,
                            InvoiceStatusId = 1
                        };

                        await _context.TranPurcInvoices.AddAsync(Invoice);
                        await _context.SaveChangesAsync();


                        Log.Info("----Info CreateUpdateInvoice method Exit----");

                        var inoviceId = Invoice.Id;
                        var invoiceItems = iteminventory;
                        if (invoiceItems.Count > 0)
                        {
                            List<TblTranPurcInvoiceItem> invoiceItemsList = new();

                            foreach (var obj1 in invoiceItems)
                            {
                                var ProductID = _context.InvItemMaster.Where(c => c.ItemCode == obj1.TranItemCode).Single();
                                var InvoiceItem = new TblTranPurcInvoiceItem
                                {

                                    CreditId = inoviceId,
                                    CreditNumber = SpCreditNumber,
                                    ItemCode = ProductID.ItemCode,
                                    Quantity = Convert.ToInt32(obj1.TranItemQty),
                                    //UnitPrice = obj1.TranItemCost,
                                    UnitPrice = obj1.TranItemCost,
                                    SubTotal = obj1.TranTotCost,
                                    DiscountAmount = obj1.DiscAmt,
                                    AmountBeforeTax = obj1.TranItemCost,
                                    TaxAmount = obj1.TaxAmount,
                                    TotalAmount = obj1.TranTotCost,
                                    IsDefaultConfig = true,
                                    CreatedOn = Convert.ToDateTime(obj.TranDate),
                                    //CreatedBy = (int),
                                    CreatedBy = 1,
                                    Description = obj1.TranItemName,
                                    TaxTariffPercentage = obj1.ItemTaxPer,
                                    //Discount = 1
                                    Discount = 1
                                };
                                invoiceItemsList.Add(InvoiceItem);
                            }
                            if (invoiceItemsList.Count > 0)
                            {
                                await _context.TranPurcInvoiceItems.AddRangeAsync(invoiceItemsList);
                                await _context.SaveChangesAsync();
                            }
                        }

                        await transaction.CommitAsync();

                        return ApiMessageInfo.Status(1, inoviceId);
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
                    #endregion
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
    #region ItemUomList

    public class ItemUomList : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class ItemUomListHandler : IRequestHandler<ItemUomList, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ItemUomListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(ItemUomList request, CancellationToken cancellationToken)
        {
            var itemcode = request.ItemList.Split('_')[0];


            Log.Info("----Info ProductUomtPriceItem method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                 .Where(e =>
                            (e.ItemCode.Contains(itemcode)
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
    #region GetUOMItemList

    public class GetUOMItemList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetUOMItemListHandler : IRequestHandler<GetUOMItemList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUOMItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUOMItemList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetUOMItemList method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                .Where(e => e.ItemCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemUOM, Value = e.ItemUOM })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetUOMItemList method Ends----");
            return item;
        }
    }

    #endregion
    //#region ProductBranch

    //public class ProductBranchItem : IRequest<ProductVendorDTO>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string BranchCode { get; set; }


    //}

    //public class ProductBranchItemItemHandler : IRequestHandler<ProductBranchItem, ProductVendorDTO>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public ProductBranchItemItemHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<ProductVendorDTO> Handle(ProductBranchItem request, CancellationToken cancellationToken)
    //    {


    //        Log.Info("----Info ProductVenPriceItem method start----");
    //        var item = await _context.VendorMasters.AsNoTracking()
    //             .Where(e =>
    //                        (e.VendCode.Contains(request.BranchCode)
    //                         ))
    //           .Select(Product => new ProductVendorDTO
    //           {
    //               VendName = Product.VendName,
    //               PoTermsCode = Product.PoTermsCode
    //           })
    //              .FirstOrDefaultAsync(cancellationToken);

    //        Log.Info("----Info ProductVenPriceItem method Ends----");
    //        return item;
    //    }
    //}

    //#endregion
    #region ProductWarehouse

    public class ProductWarehouse : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        //public string Input { get; set; }
        public string BranchCode { get; set; }
    }

    public class ProductWarehouseHandler : IRequestHandler<ProductWarehouse, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductWarehouseHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(ProductWarehouse request, CancellationToken cancellationToken)
        {
            Log.Info("----Info ProductWarehouse method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .Where(e =>
                            (e.WHBranchCode.Contains(request.BranchCode)
                             ))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info ProductWarehouse method Ends----");
            return item;
        }
    }

    #endregion
    #region Productcompany

    public class Productcompany : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        //public string Input { get; set; }
        public string BranchCode { get; set; }
    }

    public class ProductcompanyHandler : IRequestHandler<Productcompany, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductcompanyHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(Productcompany request, CancellationToken cancellationToken)
        {
            Log.Info("----Info Productcompany method start----");
            var item = (from m in _context.Companies
                        join s in _context.CompanyBranches
                        on m.Id equals s.CompanyId
                        where s.BranchCode == request.BranchCode
                        select new CustomSelectListItem
                        {
                            Text = m.CompanyName,
                            Value = m.Id.ToString(),
                        }).ToListAsync(cancellationToken);

            //var item = await _context.InvWarehouses.AsNoTracking()
            //    .Where(e =>
            //                (e.WHBranchCode.Contains(request.BranchCode)
            //                 ))
            //    .OrderByDescending(e => e.Id)
            //   .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode })
            //      .ToListAsync(cancellationToken);
            //Log.Info("----Info Productcompany method Ends----");
            return await item;
        }
    }

    #endregion
    #region GRNSingleItem

    public class GRNDetails : IRequest<TblGRNDetailsDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetGRNDetailsHandler : IRequestHandler<GRNDetails, TblGRNDetailsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGRNDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblGRNDetailsDto> Handle(GRNDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblGRNDetailsDto obj = new();
            if (transnumber is not null)
            {
                var PoHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.GRNDetails.AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnGRNDetailsDto
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
                            ReceivingQty = e.ReceivingQty,
                            ReceivedQty = e.ReceivedQty,
                            BalQty = e.BalQty,




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
            }

            return obj;
        }
    }

    #endregion
    #region CreateGRN
    public class CreateGRN : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblGRNDetailsDto Input { get; set; }
    }

    public class CreateGRNQueryHandler : IRequestHandler<CreateGRN, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateGRNQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateGRN request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {


                    Log.Info("----Info CreateGRN method start----");
                    var transnumber = string.Empty;
                    var PurchaseRequestNO = string.Empty;
                    var PurchaseOrderNO = string.Empty;
                    string InvTrans = string.Empty;
                    string FinTrans = string.Empty;
                    var inoviceIds = 0;
                    var obj = request.Input;


                    #region GRNINsert
                    TblPopTrnGRNHeader GOBJ = new();
                    if (obj.Id > 0)
                    {
                        GOBJ = await _context.GRNHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = GOBJ.TranNumber;
                        GOBJ.TranNumber = transnumber;
                    }



                    var poheader = await _context.GRNHeaders.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                    var poheadertransnumber = await _context.GRNHeaders.OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                    if (poheader != null)
                    {
                        transnumber = Convert.ToString(int.Parse(poheadertransnumber.TranNumber) + 1);

                    }
                    else
                    {

                        transnumber = Convert.ToString(10001);
                    }

                    GOBJ.TranNumber = transnumber;





                    GOBJ.PurchaseOrderNO = obj.PurchaseOrderNO;
                    GOBJ.VenCatCode = obj.VenCatCode;
                    GOBJ.Trantype = obj.Trantype;
                    GOBJ.TranDate = DateTime.Now;
                    GOBJ.DeliveryDate = obj.DeliveryDate;
                    GOBJ.CompCode = obj.CompCode;
                    GOBJ.BranchCode = obj.BranchCode;
                    GOBJ.InvRefNumber = obj.InvRefNumber;
                    GOBJ.VendCode = obj.VendCode;
                    GOBJ.DocNumber = obj.DocNumber;
                    GOBJ.PaymentID = obj.PaymentID;
                    GOBJ.Remarks = obj.Remarks;
                    GOBJ.TAXId = obj.TAXId;
                    GOBJ.TaxInclusive = obj.TaxInclusive;
                    GOBJ.PONotes = obj.PONotes;
                    GOBJ.IsApproved = false;
                    GOBJ.TranCreateUserDate = DateTime.Now;
                    GOBJ.TranCreateUser = request.User.UserId;
                    GOBJ.CreatedOn = DateTime.Now;
                    GOBJ.IsActive = true;
                    GOBJ.TranCurrencyCode = obj.TranCurrencyCode;
                    GOBJ.ClosedBy = request.User.UserId;
                    GOBJ.TranCreateUser = request.User.UserId;
                    GOBJ.TranLastEditUser = request.User.UserId;
                    GOBJ.TranpostUser = request.User.UserId;
                    GOBJ.TranvoidDate = request.User.UserId;
                    GOBJ.TranvoidDate = request.User.UserId;
                    GOBJ.TranShipMode = obj.TranShipMode;
                    GOBJ.TranTotalCost = obj.TranTotalCost;
                    GOBJ.TranDiscPer = obj.TranDiscPer;
                    GOBJ.TranDiscAmount = obj.TranDiscAmount;
                    GOBJ.Taxes = obj.Taxes;
                    GOBJ.WHCode = obj.WHCode;
                    GOBJ.IsPaid = false;
                    GOBJ.Id = 0;
                    GOBJ.IsActive = true;
                    GOBJ.CreatedOn = DateTime.Now;
                    //await _context.GRNHeaders.AddAsync(GOBJ);
                    //await _context.SaveChangesAsync();
                    var GRNH = await _context.GRNHeaders.Where(e => e.PurchaseOrderNO == obj.PurchaseOrderNO).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                    if (obj.Id > 0)
                    {
                        GOBJ.ModifiedOn = DateTime.Now;
                        _context.GRNHeaders.Update(GOBJ).Property(x => x.Id).IsModified = false;
                    }
                    else
                    {
                        GOBJ.Id = 0;
                        GOBJ.IsActive = true;
                        GOBJ.CreatedOn = DateTime.Now;
                        await _context.GRNHeaders.AddAsync(GOBJ);
                    }


                    await _context.SaveChangesAsync();

                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.GRNDetails.Where(e => e.TranId == transnumber).ToListAsync();
                        _context.GRNDetails.RemoveRange(oldAuthList);

                        List<TblPopTrnGRNDetails> UOMList = new();
                        int i = 1;
                        string trans = "";
                        var PoDetialTransNumber = await _context.GRNDetails.OrderByDescending(e => e.TranNumber).FirstOrDefaultAsync();

                        foreach (var auth in request.Input.itemList)
                        {
                            var RQty = 0;
                            if (GRNH is not null)
                            {
                                var ReceivedQty = await _context.GRNDetails.Where(e => e.TranItemCode == auth.TranItemCode && e.TranId == GRNH.TranNumber).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                                if (ReceivedQty != null)
                                {
                                    RQty = Convert.ToInt32(ReceivedQty.ReceivingQty) + Convert.ToInt32(ReceivedQty.ReceivedQty);
                                }
                            }
                            //var cInvItems = _context.GRNDetails.Where(e => e.TranItemCode == auth.TranItemCode);
                            //var cItems = cInvItems;
                            //var cItemIds = await cItems.Select(e => e.ReceivingQty).LastOrDefaultAsync();


                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.TranNumber) + i++);
                            else
                                trans = Convert.ToString(int.Parse(transnumber) + i++);

                            InvTrans = trans;
                            TblPopTrnGRNDetails UOMItem = new()
                            {

                                TranNumber = trans,
                                TranId = transnumber,
                                TranDate = DateTime.UtcNow,
                                VendCode = obj.VendCode,
                                CompCode = obj.CompCode,
                                BranchCode = obj.BranchCode,
                                TranVendorCode = obj.VendCode,
                                ItemTracking = 0,
                                TranItemCode = auth.TranItemCode,
                                TranItemName = auth.TranItemName,
                                TranItemName2 = auth.TranItemName2,
                                TranItemQty = auth.TranItemQty,
                                TranItemUnitCode = auth.TranItemUnitCode,
                                TranUOMFactor = auth.TranUOMFactor,
                                TranItemCost = auth.TranItemCost,
                                TranTotCost = auth.TranTotCost,
                                DiscPer = auth.DiscPer,
                                DiscAmt = auth.DiscAmt,
                                ItemTax = auth.ItemTax,
                                ItemTaxPer = auth.ItemTaxPer,
                                TaxAmount = auth.TaxAmount,
                                IsActive = true,
                                CreatedOn = DateTime.UtcNow,
                                ReceivingQty = auth.ReceivingQty,
                                BalQty = auth.TranItemQty - (auth.ReceivingQty + RQty),
                                ReceivedQty = RQty

                            };
                            UOMList.Add(UOMItem);
                            #region PurchaseOrderDetails Updates
                            var Bal = auth.TranItemQty - (auth.ReceivingQty + RQty);
                            if (Bal == 0)
                            {
                                var POH = await _context.purchaseOrderHeaders.Where(e => e.PurchaseOrderNO == obj.PurchaseOrderNO).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                                var Paid = _context.purchaseOrderDetails.Where(e => e.TranId == POH.TranNumber && e.TranItemCode == auth.TranItemCode);
                                var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranId == POH.TranNumber && e.TranItemCode == auth.TranItemCode);
                                POPaid.IsGrn = true;
                                _context.purchaseOrderDetails.Update(POPaid).Property(x => x.Id).IsModified = false; ;
                                await _context.SaveChangesAsync();
                            }

                            #endregion
                        }
                        await _context.GRNDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();
                    }
                    #endregion
                    Log.Info("----Info CreateGRN method Exit----");
                    await transaction.CommitAsync();
                    //return ApiMessageInfo.Status(1, 0);
                    return ApiMessageInfo.Status(1, GOBJ.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateGRN Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }

        }
        private string GetPrefixLen(int len) => "0000000000".Substring(0, len);
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }

    #endregion
    #region GetPagedGRNList

    public class GetPagedGRNList : IRequest<PaginatedList<GrnPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetGRNListHandler : IRequestHandler<GetPagedGRNList, PaginatedList<GrnPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGRNListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GrnPaginationDto>> Handle(GetPagedGRNList request, CancellationToken cancellationToken)
        {
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "GN").AsNoTracking();
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            var isArab = request.User.Culture.IsArab();
            var enquiryDetails = _context.purchaseOrderDetails.AsNoTracking();
            var itemsList = _context.InvItemMaster.AsNoTracking().Select(e => new { e.ItemCode, e.ShortName, e.ShortNameAr });

            var enquiryHeads = _context.GRNHeaders.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.GRNHeaders.AsNoTracking()
              .Where(e => (e.PurchaseOrderNO != (null)
                            ))
               .OrderBy(request.Input.OrderBy).Select(d => new GrnPaginationDto
               {
                   PurchaseOrderNO = d.PurchaseOrderNO,
                   TranDate = d.TranDate,
                   TranNumber = d.TranNumber,
                   InvRefNumber = d.InvRefNumber,
                   BranchCode = d.BranchCode,
                   VendCode = d.VendCode,
                   PaymentID = d.PaymentID,
                   TAXId = d.TAXId,
                   Id = d.Id,
                   TranCreateUser = d.TranCreateUser,
                   TranTotalCost = d.TranTotalCost,

                   ItemNames = itemsList.Where(item => enquiryDetails.Where(e => e.TranId == d.TranNumber).Select(e => e.TranItemCode).Any(e => e == item.ItemCode))
                                         .Select(item => isArab ? item.ShortNameAr : item.ShortName).ToList(),

                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.TranNumber && e.IsApproved),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthPurcOrder),
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Count(),
                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),


                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "PO" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).PurchaseOrderNO),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),
                   //HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode),
                   //IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;

        }

    }

    #endregion

    #region GRNDelete
    public class DeleteGRNList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteGRNQueryHandler : IRequestHandler<DeleteGRNList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteGRNQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteGRNList request, CancellationToken cancellationToken)
        {
            try
            {
                var Pode = await _context.GRNHeaders.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblPopTrnGRNDetails podetails;
                podetails = _context.GRNDetails.Where(d => d.TranId == Pode.TranNumber).First();
                _context.Entry(podetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblPopTrnGRNHeader PoHeader1;
                PoHeader1 = _context.GRNHeaders.Where(d => d.TranNumber == Pode.TranNumber).First();
                _context.Entry(PoHeader1).State = EntityState.Deleted;
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


    #region GRNAccount Posting

    public class GRNAccountsPosting : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetGRNAccountPostHandler : IRequestHandler<GRNAccountsPosting, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGRNAccountPostHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(GRNAccountsPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var PurchaseRequestNO = string.Empty;
                var PurchaseOrderNO = string.Empty;
                string InvTrans = string.Empty;
                string FinTrans = string.Empty;
                var inoviceIds = 0;
                int invoiceSeq = 0;

                var transnumber = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
                TblGRNDetailsDto obj = new();
                var PoHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                if (PoHeader is not null)
                {
                    var Transid = PoHeader.TranNumber;

                    var iteminventory = await _context.GRNDetails.AsNoTracking()
                        .Where(e => Transid == e.TranId)
                        .Select(e => new TblPopTrnGRNDetailsDto
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
                            ReceivingQty = e.ReceivingQty

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
                    obj.Taxes = PoHeader.Taxes;

                    #region Ispaid 
                    var Paid = _context.GRNHeaders.Where(e => e.TranNumber == FinTrans);
                    var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                    POPaid.IsPaid = true;
                    _context.GRNHeaders.Update(POPaid).Property(x => x.Id).IsModified = false; ;
                    //_context.purchaseOrderHeaders.Update(POPaid);
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

                        //decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOnPO);
                        decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                        //decimal? ItemAvgCost = await cInvItems.SumAsync(e => e.ItemAvgCost);
                        decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);
                        //var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == auth.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                        decimal itmAvgcost = 0, originalTranItemQty = 0;
                        //var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == invtItem.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();

                        //if (PoDetails is not null)
                        //    itmAvgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))));
                        //else
                        //    itmAvgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.ReceivingQty))) / ((((decimal)0) + ((decimal)auth.ReceivingQty))));

                        originalTranItemQty = invtItem.TranItemQty;
                        foreach (var itemCode in cItemIds)
                        {

                            var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == itemCode);
                            decimal itemAvgCost = 0;
                            if (invtUOM.ItemConvFactor > 1)
                            {
                                invtItem.TranItemQty = invtItem.TranItemQty * invtUOM.ItemConvFactor;
                            }
                            else
                            {
                                invtItem.TranItemQty = invtItem.ReceivingQty;
                            }

                            decimal tranItemCost = invtItem.TranItemCost - (invtItem.TranItemCost * invtItem.DiscPer) / 100;

                            //  oldInventory.QtyOH = ((decimal)QtyOnPO + auth.TranItemQty);
                            //cInvoice.ItemAvgCost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty)));
                            //if (PoDetails is not null)
                            // oldInventory.ItemAvgCost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty)));
                            //else

                            // oldInventory.ItemAvgCost = ((((decimal)0) * ((decimal)0)) + (((decimal)invtItem.TranItemCost) * ((decimal)invtItem.ReceivingQty))) / ((((decimal)0) + ((decimal)invtItem.ReceivingQty)));

                            decimal newTranItemQty = invtItem.ReceivingQty;
                            if (invtUOM.ItemConvFactor > 1)
                            {
                                itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)(invtItem.TranItemQty / invtUOM.ItemConvFactor)))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));
                            }
                            else
                            {
                                oldInventory.ItemAvgCost = ((oldInventory.QtyOH * oldInventory.ItemAvgCost) + (tranItemCost * newTranItemQty)) / (oldInventory.QtyOH + newTranItemQty);
                            }
                            //cInvoice.ItemAvgCost = ((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty)));

                            oldInventory.ItemAvgCost = itemAvgCost;
                            itmAvgcost = oldInventory.ItemAvgCost;

                            oldInventory.QtyOH = ((decimal)oldInventory.QtyOH + invtItem.TranItemQty);
                            oldInventory.ItemLastPOCost = ((decimal)ItemLastPOCost + invtItem.TranItemCost);
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
                        }


                        //foreach (var invIds in cItemMasterIds)
                        //{
                        //    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);

                        //    //var itemsAVgcost = ((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty)));
                        //    var itemsAVgcost = 0;
                        //    if (PoDetails is not null)
                        //        itemsAVgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)invtItem.TranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)invtItem.TranItemQty))));
                        //    else
                        //        itemsAVgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)invtItem.TranItemCost) * ((decimal)invtItem.ReceivingQty))) / ((((decimal)0) + ((decimal)invtItem.ReceivingQty))));
                        //    //itemsAVgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty))));


                        //    var C1 = String.Format("{0:0.0000}", itemsAVgcost);
                        //    cInvoice.ItemAvgCost = Convert.ToString(C1);
                        //    _context.InvItemMaster.Update(cInvoice);
                        //    await _context.SaveChangesAsync();
                        //}



                        #region Inventory History insert

                        var obj1 = new TblErpInvItemInventoryHistory()
                        {
                            ItemCode = invtItem.TranItemCode,
                            WHCode = obj.WHCode,

                            TranDate = DateTime.Now,
                            TranType = "2",

                            TranNumber = FinTrans,
                            TranUnit = invtItem.TranItemUnitCode,
                            //TranQty = invtItem.TranItemQty,
                            TranQty = originalTranItemQty,//invtItem.ReceivingQty,

                            unitConvFactor = invtItem.TranUOMFactor,
                            //TranTotQty = invtItem.TranItemQty,
                            TranTotQty = invtItem.TranItemQty,

                            TranPrice = invtItem.TranItemCost,

                            //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty))),
                            ItemAvgCost = itmAvgcost,

                            TranRemarks = "Inserted through GRN",
                            IsActive = false,
                            CreatedOn = DateTime.Now
                        };
                        _context.InvItemInventoryHistory.Add(obj1);
                        //}                        

                        _context.SaveChanges();
                        #endregion Inventory History

                    }
                    #endregion
                    #region FinancialTriggering
                    try
                    {
                        Log.Info("----Info CreateApInvoiceSettlement method start----");


                        var Finobj = await _context.GRNHeaders.FirstOrDefaultAsync(e => e.TranNumber == FinTrans);
                        var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);



                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        if (invSeq is null)
                        {
                            invoiceSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CreditSeq = invoiceSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeq = invSeq.CreditSeq + 1;
                            invSeq.CreditSeq = invoiceSeq;
                            _context.Sequences.Update(invSeq);
                        }



                        var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.BranchCode);
                        var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();


                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = inoviceIds,

                            FinAcCode = invDistGroup.InvDefaultAPAc,//InventoryAccount 10201001

                            CrAmount = Finobj.TranTotalCost,
                            DrAmount = 0,
                            Source = "GN",
                            Gl = string.Empty,
                            Type = "Liability",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);


                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = inoviceIds,

                            //FinAcCode = IsNotCreditPay("cash") ? payCode.FinPayAcIntgrAC : vendor.VendArAcCode,
                            FinAcCode = invDistGroup.InvAssetAc,

                            CrAmount = 0,
                            DrAmount = Finobj.TranTotalCost - Finobj.Taxes,
                            Source = "GN",
                            Gl = string.Empty,
                            Type = "Asset",

                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);


                        var invoiceItem = await _context.GRNDetails.FirstOrDefaultAsync(e => e.TranId == Finobj.TranNumber);
                        var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.ItemTaxPer).ToString());

                        if (tax is null)
                            throw new NullReferenceException("Tax is empty");

                        TblFinTrnDistribution distribution3 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = tax?.InputAcCode01,
                            CrAmount = 0,
                            DrAmount = Finobj.Taxes,
                            Source = "GN",
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


                        //Storing in  JournalVoucher tables

                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)Finobj.CompCode,
                            BranchCode = Finobj.BranchCode,
                            Batch = string.Empty,
                            Source = "GN",
                            Remarks = Finobj.Remarks,
                            Narration = Finobj.PONotes ?? Finobj.Remarks,
                            JvDate = DateTime.Now,
                            Amount = Finobj.TranTotalCost + Finobj.Taxes,
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
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = (int)Finobj.CompCode,
                                    BranchCode = Finobj.BranchCode,
                                    JvDate = DateTime.Now,
                                    TranSource = "GN",
                                    Trantype = "Invoice",
                                    DocNum = Finobj.InvRefNumber,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = Finobj.Remarks,
                                    JournalVoucherId = jvId,
                                    IsApproved = true,
                                };
                                jvApprovalList.Add(approval);
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
                                Source = "GN",
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


                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in CreateApInvoiceSettlement Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return ApiMessageInfo.Status(0);
                        //return false;
                    }
                    #endregion

                    #region Purchase Invoice Inserting 
                    try
                    {
                        Log.Info("----Info CreateUpdateInvoice method start----");

                        string SpCreditNumber = $"S{new Random().Next(99, 9999999).ToString()}";


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
                            TaxAmount = PoHeader.Taxes,
                            TotalAmount = PoHeader.TranTotalCost + PoHeader.Taxes,
                            TotalPayment = 0,
                            AmountDue = 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(PoHeader.TranDate),
                            CreatedBy = request.User.UserId,

                            CustomerId = VendorID.Id,

                            InvoiceStatus = "Open",
                            TaxIdNumber = PoHeader.TAXId,

                            InvoiceModule = "POMgt",
                            InvoiceNotes = PoHeader.PONotes,
                            Remarks = obj.Remarks,
                            InvoiceRefNumber = PoHeader.InvRefNumber,

                            LpoContract = "",
                            VatPercentage = Convert.ToDecimal(0),
                            PaymentTerms = PoHeader.PaymentID,
                            BranchCode = PoHeader.BranchCode,
                            ServiceDate1 = PoHeader.DeliveryDate.ToString(),

                            IsCreditConverted = false,
                            InvoiceStatusId = 1

                        };

                        await _context.TranVenInvoices.AddAsync(Invoice);
                        await _context.SaveChangesAsync();

                        TblFinTrnVendorApproval approval = new()
                        {
                            CompanyId = request.User.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "GN",
                            Trantype = "Invoice",
                            VendCode = PoHeader.VenCatCode,
                            DocNum = "DocNum",
                            LoginId = request.User.UserId,
                            AppRemarks = "GNApprove",
                            InvoiceId = Invoice.Id,
                            IsApproved = true,
                        };

                        await _context.TrnVendorApprovals.AddAsync(approval);
                        await _context.SaveChangesAsync();
                        var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == obj.PaymentID);
                        TblFinTrnVendorInvoice cInvoice = new()
                        {
                            CompanyId = (int)obj.CompCode,
                            BranchCode = obj.BranchCode,
                            InvoiceNumber = invoiceSeq.ToString(),// invoiceSeq.ToString(),
                            InvoiceDate = obj.TranDate,
                            CreditDays = paymentTerms.POTermsDueDays,
                            DueDate = obj.DeliveryDate,
                            TranSource = "GN",
                            Trantype = "Invoice",
                            VendCode = obj.VenCatCode,
                            DocNum = obj.DocNumber,
                            LoginId = request.User.UserId,
                            ReferenceNumber = obj.InvRefNumber,
                            InvoiceAmount = Invoice.TotalAmount,
                            DiscountAmount = 0,
                            NetAmount = Invoice.TotalAmount,
                            PaidAmount = 0,
                            BalanceAmount = Invoice.TotalAmount,
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
                            TranSource = "GN",
                            Trantype = "Invoice",
                            TranNumber = PoHeader.TranNumber,// invoiceSeq.ToString(),
                            VendCode = obj.VendCode,
                            DocNum = "DocNum",
                            ReferenceNumber = obj.InvRefNumber,
                            PaymentType = "check",
                            PamentCode = "paycode",
                            CheckNumber = invoiceSeq.ToString(),
                            Remarks1 = obj.Remarks,
                            Remarks2 = obj.Remarks,
                            LoginId = request.User.UserId,
                            DrAmount = obj.TranTotalCost + obj.Taxes,
                            CrAmount = 0,
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
                                    //Quantity = Convert.ToInt32(obj1.TranItemQty),
                                    Quantity = Convert.ToInt32(obj1.ReceivingQty),
                                    UnitPrice = obj1.TranItemCost,
                                    SubTotal = obj1.TranTotCost,
                                    DiscountAmount = obj1.DiscAmt,
                                    AmountBeforeTax = obj1.TranItemCost,
                                    TaxAmount = obj1.TaxAmount,
                                    TotalAmount = obj1.TranTotCost,
                                    IsDefaultConfig = true,
                                    CreatedOn = Convert.ToDateTime(obj.TranDate),
                                    //CreatedBy = (int),
                                    CreatedBy = 1,
                                    Description = obj1.TranItemName,
                                    TaxTariffPercentage = obj1.ItemTaxPer,
                                    InvoiceType = 3,
                                    //Discount = 1
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
                    #endregion

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


    #region GRNDetails

    public class GetGRNDetails : IRequest<TblPurchaseReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public string PONO { get; set; }
    }

    public class GetGRNDetailListsHandler : IRequestHandler<GetGRNDetails, TblPurchaseReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGRNDetailListsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPurchaseReturntDto> Handle(GetGRNDetails request, CancellationToken cancellationToken)
        {
            var bqty = 0;
            var transnumber = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == request.PONO);
            TblPurchaseReturntDto obj = new();
            var PoHeader = await _context.purchaseOrderHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == request.PONO);
            var GRNH = await _context.GRNHeaders.Where(e => e.PurchaseOrderNO == request.PONO).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
            if (GRNH is not null)
            {
                var bqty1 = await _context.GRNDetails.Where(e => e.TranId == GRNH.TranNumber).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                bqty = (int)bqty1.BalQty;
            }


            var GRNHeader = await _context.GRNHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.PurchaseOrderNO == transnumber.PurchaseOrderNO);

            var podItems = _context.purchaseOrderDetails.AsNoTracking()
                          .Where(e => e.TranId == transnumber.TranNumber)
                          .Select(e => e.TranItemCode)
                          .ToList();

            var itemTrackingData = _context.InvItemMaster.AsNoTracking()
                         .Where(e => podItems.Contains(e.ItemCode))
                         .Select(e => e.ItemTracking)
                         .FirstOrDefault();

            if (PoHeader is not null)
            {
                var Transid = PoHeader.TranNumber;

                var iteminventory = await _context.purchaseOrderDetails.AsNoTracking()
                    .Where(e => Transid == e.TranId && e.IsGrn == false)
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
                        BalQty = bqty,
                        SerExpTracking = itemTrackingData,
                        PoNo = PoHeader.PurchaseOrderNO


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
    #region Get GRNList

    public class GetGRNList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GRNListHandler : IRequestHandler<GetGRNList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GRNListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetGRNList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetGRNList method start----");

            var item = (from m in _context.purchaseOrderHeaders
                            // where m.ISGRN == true
                        group new { m } by new { m.PurchaseOrderNO } into grp
                        select new CustomSelectListItem
                        {
                            Text = grp.Key.PurchaseOrderNO,
                            Value = grp.Key.PurchaseOrderNO,
                        });

            if (request.Input.HasValue())
                item = item.Where(e => e.Text.Contains(request.Input) || e.Value.Contains(request.Input));

            Log.Info("----Info GetPurchaseRequestList method Ends----");
            return await item.ToListAsync();
        }
    }

    #endregion
    #region GRNUpdateList
    public class GRNUpdateList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public string Id { get; set; }
    }

    public class GRNUpdateQueryHandler : IRequestHandler<GRNUpdateList, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GRNUpdateQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(GRNUpdateList request, CancellationToken cancellationToken)
        {
            try
            {
                var Paid = _context.purchaseOrderHeaders.Where(e => e.PurchaseOrderNO == request.Id);
                var POPaid = await Paid.FirstOrDefaultAsync(e => e.PurchaseOrderNO == request.Id);
                POPaid.ISGRN = true;
                POPaid.IsPaid = true;
                _context.purchaseOrderHeaders.Update(POPaid).Property(x => x.Id).IsModified = false;
                await _context.SaveChangesAsync();




                return POPaid.Id;

            }
            catch (Exception ex)
            {
                Log.Error("Error in GRNUpdateList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region Inventory Expiry Mgt and Serial Mgt

    #region GetInvItemSpecifications

    public class GetInvItemSpecifications : IRequest<List<TblErpInvItemSpecificationDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetInvItemSpecificationsHandler : IRequestHandler<GetInvItemSpecifications, List<TblErpInvItemSpecificationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetInvItemSpecificationsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblErpInvItemSpecificationDto>> Handle(GetInvItemSpecifications request, CancellationToken cancellationToken)
        {
            return await _context.InvItemSpecifications.AsNoTracking()
                .OrderByDescending(e => e.Id)
                .ProjectTo<TblErpInvItemSpecificationDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }

    #endregion






    #region Expairy Details
    public class GetExpairyDetails : IRequest<List<TblErpInvItemExpiryBatchDto>>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }
    }

    public class GetExpairyDetailsHandler : IRequestHandler<GetExpairyDetails, List<TblErpInvItemExpiryBatchDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetExpairyDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblErpInvItemExpiryBatchDto>> Handle(GetExpairyDetails request, CancellationToken cancellationToken)
        {
            List<TblErpInvItemExpiryBatchDto> ExpiryBatchList = new();
            // var Batches = await _context.InvItemExpiryBatches.AsNoTracking().Where(e => e.ItemCode == request.ItemCode).FirstOrDefaultAsync();

            if (request.ItemCode is not null)
            {
                ExpiryBatchList = await _context.InvItemExpiryBatches.AsNoTracking().ProjectTo<TblErpInvItemExpiryBatchDto>(_mapper.ConfigurationProvider).Where(e => e.ItemCode == request.ItemCode).ToListAsync();

            }

            return ExpiryBatchList;
        }


    }


    #endregion




    #region GetInvItemSelectListByPoNumber

    public class GetInvItemSelectListByPoNumber : IRequest<InvItemSelectListByPoNumberDto>
    {
        public UserIdentityDto User { get; set; }
        public string PurchaseOrderNO { get; set; }
    }

    public class GetInvItemSelectListByPoNumberHandler : IRequestHandler<GetInvItemSelectListByPoNumber, InvItemSelectListByPoNumberDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetInvItemSelectListByPoNumberHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvItemSelectListByPoNumberDto> Handle(GetInvItemSelectListByPoNumber request, CancellationToken cancellationToken)
        {

            var isArab = request.User.Culture.IsArab();
            var items = _context.InvItemMaster.AsNoTracking();
            var poHeaderTranNumber = await _context.purchaseOrderHeaders.AsNoTracking().Where(e => e.PurchaseOrderNO == request.PurchaseOrderNO).Select(e => e.TranNumber).FirstOrDefaultAsync();
            var poItems = _context.purchaseOrderDetails.AsNoTracking()
                .Where(e => e.TranId == poHeaderTranNumber).Select(e => new { e.TranItemCode, e.TranItemQty });

            items = items.Where(e => poItems.Any(poItem => poItem.TranItemCode == e.ItemCode));

            var expExistingItems = _context.InvItemExpiryBatches.AsNoTracking().Where(e => e.PoNumber == request.PurchaseOrderNO).Select(e => e.ItemCode);
            var serialExistingItems = _context.InvItemSerialBatches.AsNoTracking().Where(e => e.PoNumber == request.PurchaseOrderNO).Select(e => e.ItemCode);

            if (await expExistingItems.AnyAsync())
                items = items.Where(e => !expExistingItems.Any(poItem => poItem == e.ItemCode));
            if (await serialExistingItems.AnyAsync())
                items = items.Where(e => !serialExistingItems.Any(poItem => poItem == e.ItemCode));

            var expItems = await items.Where(e => e.ItemTracking.ToLower() == "exp").Select(e => new CustomSelectListItem { Value = e.ItemCode, Text = isArab ? e.ShortNameAr : e.ShortName }).ToListAsync();
            var serialItems = await items.Where(e => e.ItemTracking.ToLower() == "srl").Select(e => new CustomSelectListItem { Value = e.ItemCode, Text = isArab ? e.ShortNameAr : e.ShortName }).ToListAsync();

            foreach (var item in expItems)
            {
                item.DecValue = (await poItems.FirstOrDefaultAsync(e => e.TranItemCode == item.Value))?.TranItemQty ?? 0;
            }
            foreach (var item in serialItems)
            {
                item.DecValue = (await poItems.FirstOrDefaultAsync(e => e.TranItemCode == item.Value))?.TranItemQty ?? 0;
            }

            return new() { Exps = expItems, Serials = serialItems };
        }
    }

    #endregion

    //#region GetInvItemSelectListItem

    //public class GetInvItemSelectListItem : IRequest<CustomSelectListItem>
    //{
    //    public string ItemCode { get; set; }
    //}

    //public class GetInvItemSelectListItemHandler : IRequestHandler<GetInvItemSelectListItem, CustomSelectListItem>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public GetInvItemSelectListItemHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<CustomSelectListItem> Handle(GetInvItemSelectListItem request, CancellationToken cancellationToken)
    //    {
    //        return await _context.InvItemSpecifications.AsNoTracking()
    //            .OrderByDescending(e => e.Id)
    //            // .Where(e=>e.)
    //            .ProjectTo<CustomSelectListItem>(_mapper.ConfigurationProvider)
    //            .FirstOrDefaultAsync(cancellationToken);
    //    }
    //}

    //#endregion

    #region CreateInvItemExpiryBatch

    public class CreateInvItemExpiryBatch : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpInvItemExpiryBatchListDto Input { get; set; }
    }

    //public class CreateInvItemExpiryBatchHandler : IRequestHandler<CreateInvItemExpiryBatch, AppCtrollerDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateInvItemExpiryBatchHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<AppCtrollerDto> Handle(CreateInvItemExpiryBatch request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var input = request.Input;
    //            string error = string.Empty;

    //            List<TblErpInvItemExpiryBatch> InvExpBatchs = new();

    //            foreach (var obj in input.Items)
    //            {
    //                // Check if the batch number already exists
    //                var existingBatch = await _context.InvItemExpiryBatches
    //                    .FirstOrDefaultAsync(e => e.BatchNumber == obj.BatchNumber);

    //                TblErpInvItemExpiryBatch InvExpBatch;

    //                if (existingBatch != null)
    //                {
    //                    // Update the existing batch
    //                    existingBatch.ItemCode = obj.ItemCode;
    //                    existingBatch.ItemName = obj.ItemName;
    //                    existingBatch.PoNumber = obj.PoNumber;
    //                    existingBatch.MfgDate = obj.MfgDate;
    //                    existingBatch.ExpDate = obj.ExpDate;
    //                    existingBatch.Qty = obj.Qty;
    //                    existingBatch.Remarks = obj.Remarks;

    //                    _context.InvItemExpiryBatches.Update(existingBatch);
    //                    InvExpBatch = existingBatch;
    //                }
    //                else
    //                {
    //                    // Add a new batch
    //                    InvExpBatch = new TblErpInvItemExpiryBatch
    //                    {
    //                        ItemCode = obj.ItemCode,
    //                        ItemName = obj.ItemName,
    //                        PoNumber = obj.PoNumber,
    //                        BatchNumber = obj.BatchNumber,
    //                        MfgDate = obj.MfgDate,
    //                        ExpDate = obj.ExpDate,
    //                        Available = 0,
    //                        Qty = obj.Qty,
    //                        QtyCommitted = 0,
    //                        Remarks = obj.Remarks,
    //                    };

    //                    await _context.InvItemExpiryBatches.AddAsync(InvExpBatch);
    //                }


    //                //await _context.SaveChangesAsync();
    //                //return ApiMessageInfo.Status(1, InvExpBatch.Id);

    //            }

    //            if (error.HasValue())
    //                return ApiMessageInfo.Status(error);

    //            if (InvExpBatchs.Count > 0)
    //            {
    //                await _context.InvItemExpiryBatches.AddRangeAsync(InvExpBatchs);
    //                await _context.SaveChangesAsync();
    //                return ApiMessageInfo.Status(1, 1);
    //            }

    //            return ApiMessageInfo.Status(0);


    //            //var hasExpBatch = await _context.InvItemExpiryBatches.AnyAsync(e => e.BatchNumber == obj.BatchNumber);
    //            //if (hasExpBatch)
    //            //    return ApiMessageInfo.Status($"Duplicate BatchNumber {obj.BatchNumber}");

    //            //TblErpInvItemExpiryBatch InvExpBatch = await _context.InvItemExpiryBatches.FirstOrDefaultAsync(e => e.Id == obj.Id) ?? new();
    //            //InvExpBatch.PoNumber = obj.PoNumber;
    //            //InvExpBatch.BatchNumber = obj.BatchNumber;
    //            //InvExpBatch.MfgDate = obj.MfgDate;
    //            //InvExpBatch.ExpDate = obj.ExpDate;
    //            //InvExpBatch.Available = 0;
    //            //InvExpBatch.Qty = obj.Qty;
    //            //InvExpBatch.QtyCommitted = 0;
    //            //InvExpBatch.Remarks = obj.Remarks;

    //            //if (obj.Id > 0)
    //            //    _context.InvItemExpiryBatches.Update(InvExpBatch);
    //            //else
    //            //    await _context.InvItemExpiryBatches.AddAsync(InvExpBatch);

    //            //await _context.SaveChangesAsync();
    //            //return ApiMessageInfo.Status(1, InvExpBatch.Id);

    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in CreateInvItemExpiryBatch Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return ApiMessageInfo.Status(0);
    //        }
    //    }
    //}

    public class CreateInvItemExpiryBatchHandler : IRequestHandler<CreateInvItemExpiryBatch, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInvItemExpiryBatchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInvItemExpiryBatch request, CancellationToken cancellationToken)
        {
            try
            {
                var input = request.Input;
                string error = string.Empty;

                List<TblErpInvItemExpiryBatch> InvExpBatchs = new();

                foreach (var obj in input.Items)
                {
                    // Check if the batch number already exists
                    var existingBatch = await _context.InvItemExpiryBatches
                        .FirstOrDefaultAsync(e => e.BatchNumber == obj.BatchNumber && e.ItemCode == obj.ItemCode, cancellationToken);

                    TblErpInvItemExpiryBatch InvExpBatch;

                    if (existingBatch != null)
                    {
                        // Update the existing batch
                        existingBatch.ItemCode = obj.ItemCode;
                        existingBatch.ItemName = obj.ItemName;
                        existingBatch.PoNumber = obj.PoNumber;
                        existingBatch.MfgDate = obj.MfgDate;
                        existingBatch.ExpDate = obj.ExpDate;
                        existingBatch.Qty = obj.Qty;
                        existingBatch.Remarks = obj.Remarks;

                        _context.InvItemExpiryBatches.Update(existingBatch);
                        InvExpBatch = existingBatch;
                    }
                    else
                    {
                        // Add a new batch
                        InvExpBatch = new TblErpInvItemExpiryBatch
                        {
                            ItemCode = obj.ItemCode,
                            ItemName = obj.ItemName,
                            PoNumber = obj.PoNumber,
                            BatchNumber = obj.BatchNumber,
                            MfgDate = obj.MfgDate,
                            ExpDate = obj.ExpDate,
                            Available = 0,
                            Qty = obj.Qty,
                            QtyCommitted = 0,
                            Remarks = obj.Remarks,
                        };

                        await _context.InvItemExpiryBatches.AddAsync(InvExpBatch, cancellationToken);
                    }

                    InvExpBatchs.Add(InvExpBatch);
                }

                if (!string.IsNullOrEmpty(error))
                    return ApiMessageInfo.Status(error);

                if (InvExpBatchs.Count > 0)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return ApiMessageInfo.Status(1, 1); // Consider returning more relevant info if needed
                }

                return ApiMessageInfo.Status(0);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInvItemExpiryBatch Method");
                Log.Error("Error occurred time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }


    #endregion




    #region CreateInvItemSerialBatch

    public class CreateInvItemSerialBatch : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpInvItemSerialBatchListDto Input { get; set; }
    }

    public class CreateInvItemSerialBatchHandler : IRequestHandler<CreateInvItemSerialBatch, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInvItemSerialBatchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInvItemSerialBatch request, CancellationToken cancellationToken)
        {
            try
            {
                var input = request.Input;
                string error = string.Empty;

                List<TblErpInvItemSerialBatch> InvSerialBatches = new();
                foreach (var obj in input.SerialList)
                {
                    var hasSerialBatch = await _context.InvItemSerialBatches.AnyAsync(e => e.SerialNumber == obj.SerialNumber);
                    if (hasSerialBatch)
                        error += $"Duplicate {nameof(obj.SerialNumber)} {obj.SerialNumber} <br/>";

                    TblErpInvItemSerialBatch InvSerialBatch = new()
                    {
                        ItemCode = obj.ItemCode,
                        SerialNumber = obj.SerialNumber,
                        PoNumber = obj.PoNumber,
                        PoDate = obj.PoDate,
                        SalesOrdNum = string.Empty,
                        Committed = false,
                    };

                    InvSerialBatches.Add(InvSerialBatch);
                }
                if (error.HasValue())
                    return ApiMessageInfo.Status(error);

                if (InvSerialBatches.Count > 0)
                {
                    await _context.InvItemSerialBatches.AddRangeAsync(InvSerialBatches);
                    await _context.SaveChangesAsync();
                    return ApiMessageInfo.Status(1, 1);
                }

                return ApiMessageInfo.Status(0);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInvItemSerialBatch Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion


    #region CreateInvItemSpecification

    public class CreateInvItemSpecification : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpInvItemSpecificationDto Input { get; set; }
    }

    public class CreateInvItemSpecificationHandler : IRequestHandler<CreateInvItemSpecification, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateInvItemSpecificationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateInvItemSpecification request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = request.Input;
                TblErpInvItemSpecification InvSpecification = await _context.InvItemSpecifications.FirstOrDefaultAsync(e => e.Id == obj.Id) ?? new();
                InvSpecification.ItemCode = obj.ItemCode;
                InvSpecification.HasWarranty = obj.HasWarranty;
                InvSpecification.Remarks = obj.Remarks;
                InvSpecification.Specification = obj.Specification;
                InvSpecification.Unit = obj.Unit;
                InvSpecification.Value = obj.Value;
                InvSpecification.WarrTime = obj.WarrTime;

                if (obj.Id > 0)
                    _context.InvItemSpecifications.Update(InvSpecification);
                else
                    await _context.InvItemSpecifications.AddAsync(InvSpecification);

                await _context.SaveChangesAsync();
                return ApiMessageInfo.Status(1, InvSpecification.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateInvItemSpecification Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);

                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion


    #endregion
}
