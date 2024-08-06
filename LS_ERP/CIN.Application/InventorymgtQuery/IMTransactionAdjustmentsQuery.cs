//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using CIN.Application.Common;
//using CIN.Application.InventoryMgtDtos;
//using CIN.DB;
//using CIN.Domain.InventoryMgt;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryMgtDtos;
//using CIN.Application.PurchaseSetupDtos;
//using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InventoryMgt;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
//using CIN.Domain.PurchaseMgt;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventorymgtQuery
{
    #region GetAdjustmentsUserSelectList

    public class GetAdjustmentsUserSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentsUserListHandler : IRequestHandler<GetAdjustmentsUserSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentsUserListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentsUserSelectList request, CancellationToken cancellationToken)
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
    #region GetAdjustmentToLocationList

    public class GetAdjustmentToLocationList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentToLocationListHandler : IRequestHandler<GetAdjustmentToLocationList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentToLocationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentToLocationList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentToLocationList method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentToLocationList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentAccountSelectList

    public class GetAdjustmentAccountSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentAccountsListHandler : IRequestHandler<GetAdjustmentAccountSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentAccountSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentAccountSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentAccountSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentJVNumberSelectList

    public class GetAdjustmentJVNumberSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentJvNumberListHandler : IRequestHandler<GetAdjustmentJVNumberSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentJvNumberListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentJVNumberSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentJVNumberSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentJVNumberSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentBarCodeSelectList

    public class GetAdjustmentBarCodeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentBarcodeListHandler : IRequestHandler<GetAdjustmentBarCodeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentBarcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentBarCodeSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentBarCodeSelectList method start----");
            var item = await _context.InvItemsBarcode.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemBarcode, Value = e.ItemCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentBarCodeSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentItemCodeList

    public class GetAdjustmentItemCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentItemCodeListHandler : IRequestHandler<GetAdjustmentItemCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentItemCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentItemCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentItemCodeList method start----");
            var item = _context.InvItemMaster.AsNoTracking();

            if (request.Input.HasValue())
            {
                item = item.Where(e => e.ItemCode.Contains(request.Input) || e.ShortName.Contains(request.Input)
                || e.ShortNameAr.Contains(request.Input));
            }
            bool isArab = request.User.Culture.IsArab();
            var itemList = await item.OrderByDescending(e => e.Id)
           .Select(e => new CustomSelectListItem { Text = e.ItemCode, Value = e.ItemCode, TextTwo = isArab ? e.ShortNameAr : e.ShortName })
              .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentItemCodeList method Ends----");
            return itemList;
        }
    }

    #endregion
    #region AdjustmentProductUomtPriceItem

    public class AdjustmentProductUomtPriceItem : IRequest<AdjustmentProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class AdjustmentProductUOMFactorItemHandler : IRequestHandler<AdjustmentProductUomtPriceItem, AdjustmentProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AdjustmentProductUOMFactorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AdjustmentProductUnitPriceDTO> Handle(AdjustmentProductUomtPriceItem request, CancellationToken cancellationToken)
        {
            var itemcode = request.ItemList.Split('_')[0];
            var ItemUOM = request.ItemList.Split('_')[1];

            Log.Info("----Info AdjustmentProductUomtPriceItem method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                 .Where(e =>
                            (e.ItemCode.Contains(itemcode) && e.ItemUOM.Contains(ItemUOM)
                             ))
               .Select(Product => new AdjustmentProductUnitPriceDTO
               {
                   tranItemCode = Product.ItemCode,
                   tranItemUomFactor = Product.ItemConvFactor,
                   ItemAvgcost = Product.ItemAvgCost,

               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info AdjustmentProductUomtPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentUOMList

    public class GetAdjustmentUOMSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentUOMselectListHandler : IRequestHandler<GetAdjustmentUOMSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentUOMselectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentUOMSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentUOMList method start----");
            var item = await _context.InvUoms.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UOMName, Value = e.UOMCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentUOMList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAdjustmentItemNameList

    public class GetAdjustmentItemNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAdjustmentItemNameListHandler : IRequestHandler<GetAdjustmentItemNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentItemNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAdjustmentItemNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAdjustmentItemNameList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAdjustmentItemNameList method Ends----");
            return item;
        }
    }

    #endregion
    #region ProductUnitPriceItem

    public class AdjustmentProductUnitPriceItem : IRequest<AdjustmentProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Itemcode { get; set; }
    }

    public class AdjustmentProductUnitPriceItemHandler : IRequestHandler<AdjustmentProductUnitPriceItem, AdjustmentProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AdjustmentProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AdjustmentProductUnitPriceDTO> Handle(AdjustmentProductUnitPriceItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info AdjustmentProductUnitPriceItem method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .Where(e => e.ItemCode == request.Itemcode)
                 .Include(e => e.InvUoms)
               .Select(Product => new AdjustmentProductUnitPriceDTO
               {

                   tranItemCode = Product.ItemCode,
                   tranItemName = Product.ShortName,
                   tranItemUnitCode = Product.ItemBaseUnit,
                   tranItemCost = Product.ItemAvgCost,


               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info AdjustmentProductUnitPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region CreateAdjustmentsRequest
    public class AdjustmentCreateIssuesRequest : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblAdjustmentsInventoryReturntDto Input { get; set; }
    }

    public class AdjustmentCreateIssuesQueryHandler : IRequestHandler<AdjustmentCreateIssuesRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AdjustmentCreateIssuesQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(AdjustmentCreateIssuesRequest request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateAdjustmentsRequest method start----");
                    var transnumber = string.Empty;
                    var obj = request.Input;
                    TblIMAdjustmentsTransactionHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.IMAdjustmentsTransactionHeader.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                    }
                    else
                    {

                        var IMheader = await _context.IMAdjustmentsTransactionHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
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
                    cObj.TranBranch = obj.TranBranch;


                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        //cObj.Id = 0;
                        cObj.TranLastEditDate = DateTime.Now;
                        cObj.TranLastEditUser = request.User.UserId.ToString();
                        _context.IMAdjustmentsTransactionHeader.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.IMAdjustmentsTransactionHeader.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.IMAdjustmentsTransactionDetails.Where(e => e.TranNumber == transnumber).ToListAsync();
                        _context.IMAdjustmentsTransactionDetails.RemoveRange(oldAuthList);
                        List<TblIMAdjustmentsTransactionDetails> UOMList = new();
                        int i = 1;
                        string trans = "1";
                        var PoDetialTransNumber = await _context.IMAdjustmentsTransactionDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var auth in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.SNo) + i++);
                            else
                                trans = Convert.ToString(int.Parse("0") + i++);

                            TblIMAdjustmentsTransactionDetails UOMItem = new()
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
                        await _context.IMAdjustmentsTransactionDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();

                    }

                    Log.Info("----Info CreateAjustmentsRequest method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateAdjustmentsRequest Method");
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

    public class GetIMAdjustmentsTransactionList : IRequest<PaginatedList<AdjacementrPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetAdjIMTransactionListHandler : IRequestHandler<GetIMAdjustmentsTransactionList, PaginatedList<AdjacementrPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjIMTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AdjacementrPaginationDto>> Handle(GetIMAdjustmentsTransactionList request, CancellationToken cancellationToken)
        {
            //var search = request.Input.Query;
            //var list = await _context.IMAdjustmentsTransactionHeader.AsNoTracking()
            //  .Where(e =>
            //                (e.TranNumber.Contains(search) || e.TranReference.Contains(search)

            //                 ))
            //   .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblIMAdjustmentsTransactionHeaderDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            //return list;
            var search = request.Input.Query;
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            //var oprAuths = _context.PurAuthorities.AsNoTracking();
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "ADJS").AsNoTracking();

            var enquiryHeads = _context.IMAdjustmentsTransactionHeader.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.IMAdjustmentsTransactionHeader.AsNoTracking()
              .Where(e => (e.TranNumber != (null)
                            ))
               .OrderBy(request.Input.OrderBy).Select(d => new AdjacementrPaginationDto
               {
                   TranNumber = d.TranNumber,
                   TranDate = d.TranDate,
                   TranReference = d.TranReference,
                   TranLocation = d.TranLocation,
                   TranToLocation = d.TranToLocation,
                   TranDocNumber = d.TranDocNumber,
                   TranBranch = d.TranBranch,
                   Id = d.Id,
                   TranCreateUser = d.TranCreateUser,
                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.TranNumber && e.IsApproved),
                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "AD" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).TranNumber),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.TranBranch && e.AppAuthAdj),
                   //HasAuthority = true,
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.TranBranch).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Count(),
                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;




        }

    }

    #endregion
    #region SingleItem

    public class GetAdjustmentIMDetails : IRequest<TblAdjustmentsInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetAdjustmentIMDetailsHandler : IRequestHandler<GetAdjustmentIMDetails, TblAdjustmentsInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAdjustmentIMDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblAdjustmentsInventoryReturntDto> Handle(GetAdjustmentIMDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.IMAdjustmentsTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblAdjustmentsInventoryReturntDto obj = new();
            var IMHeader = await _context.IMAdjustmentsTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (IMHeader is not null)
            {
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMAdjustmentsTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMAdjustmentsTransactionDetailsDto
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
                obj.itemList = iteminventory;
                obj.Id = IMHeader.Id;
                obj.TranBranch = IMHeader.TranBranch;



            }
            return obj;
        }
    }

    #endregion
    #region AdjustmentDelete
    public class AdjustmentDeleteIMList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class AdjustmentDeleteIMQueryHandler : IRequestHandler<AdjustmentDeleteIMList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AdjustmentDeleteIMQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AdjustmentDeleteIMList request, CancellationToken cancellationToken)
        {
            try
            {
                var IMde = await _context.IMAdjustmentsTransactionHeader.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblIMAdjustmentsTransactionDetails IMdetails;
                IMdetails = _context.IMAdjustmentsTransactionDetails.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMdetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblIMAdjustmentsTransactionHeader IMHeader;
                IMHeader = _context.IMAdjustmentsTransactionHeader.Where(d => d.TranNumber == IMde.TranNumber).First();
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
    //#region SingleItem

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
    //#endregion 

    #region Settlement


    public class AdjustmentSettelementList : IRequest<TblInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class AdjustmentSettelementListHandler : IRequestHandler<AdjustmentSettelementList, TblInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AdjustmentSettelementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryReturntDto> Handle(AdjustmentSettelementList request, CancellationToken cancellationToken)
        {
            var inoviceIds = 0;
            int invoiceSeq = 0;
            var transnumber = await _context.IMAdjustmentsTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblInventoryReturntDto obj = new();
            var IMHeader = await _context.IMAdjustmentsTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);

            // var TranNumber = new SqlParameter("@transnumner", IMHeader.TranNumber);
            //var FInAdjvalue = _context
            //            .Adjs
            //            .FromSqlRaw("exec sp_Adjustmentsvalues @transnumner",
            //                TranNumber)
            //            .ToList();
            //var FInAdjvalue = _context.Adjs.FromSqlRaw("sp_Adjustmentsvalues"+ IMHeader.TranNumber).ToList();

            if (IMHeader is not null)
            {
                var author = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranLocation).Single();
                var IMHeaderDetails = _context.IMAdjustmentsTransactionDetails.AsNoTracking().Where(e => e.TranNumber == IMHeader.TranNumber);
                decimal PositiveSum = IMHeaderDetails.Where(e => e.TranTotCost > 0).Sum(e => e.TranTotCost),
                        NegativeSum = IMHeaderDetails.Where(e => e.TranTotCost < 0).Sum(e => e.TranTotCost);
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMAdjustmentsTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMAdjustmentsTransactionDetailsDto
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
                #region QtyUpdate
                foreach (var invtItem in iteminventory)
                {
                    var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == invtItem.TranItemCode && e.WHCode == author.WHCode);
                    var cItems = cInvItems;
                    var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();

                    var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == invtItem.TranItemCode);
                    var cItemMasters = cInvItemMasters;
                    var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();

                    //decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOnPO);
                    // decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);

                    decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                    decimal? ItemAvgCost = 0;// await cInvItems.SumAsync(e => e.ItemAvgCost);

                    decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);

                    foreach (var invId in cItemIds)
                    {
                        var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        decimal tranItemCost = invtItem.TranItemCost;// - (invtItem.TranItemCost * invtItem.DiscPer) / 100;
                        oldInventory.ItemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));
                        ItemAvgCost = oldInventory.ItemAvgCost;
                        oldInventory.QtyOH = ((decimal)oldInventory.QtyOH + invtItem.TranItemQty);

                        oldInventory.ItemLastPOCost = ((decimal)ItemLastPOCost + invtItem.TranItemCost);
                        _context.InvItemInventory.Update(oldInventory);
                        await _context.SaveChangesAsync();

                        //var cInvoice = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        //cInvoice.QtyOH = ((decimal)QtyOH + auth.TranItemQty);
                        //cInvoice.ItemAvgCost = (decimal)ItemAvgCost;

                        //cInvoice.QtyOH = ((decimal)QtyOH);

                        //cInvoice.QtyOnPO = ((decimal)QtyOnPO + auth.TranItemQty);
                        //cInvoice.QtyReserved = ((decimal)QtyReserved + auth.TranItemQty);
                        //cInvoice.QtyReserved = ((decimal)QtyReserved);
                        //cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));
                        //cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                        //cInvoice.ItemAvgCost = (((decimal)QtyReserved + auth.TranItemQty)) / (((decimal)ItemAvgCost + auth.TranItemCost));
                        //cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost + auth.TranItemCost);
                        //cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost);

                        //  _context.InvItemInventory.Update(cInvoice);
                        // await _context.SaveChangesAsync();

                        var itemMaster = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        if (itemMaster is not null)
                        {
                            var C1 = String.Format("{0:0.0000}", ItemAvgCost ?? 0);
                            itemMaster.ItemAvgCost = Convert.ToString(C1);
                            _context.InvItemMaster.Update(itemMaster);
                            await _context.SaveChangesAsync();
                        }

                    }

                    //foreach (var invIds in cItemMasterIds)
                    //{
                    //    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
                    //    //var itemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));
                    //    //var itemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                    //    var itemsAVgcost = (decimal)ItemAvgCost;
                    //    //var itemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                    //    //var itemsAVgcost = (((decimal)QtyReserved + auth.TranItemQty)) / (((decimal)ItemAvgCost + auth.TranItemCost));
                    //    var C1 = String.Format("{0:0.0000}", itemsAVgcost);
                    //    cInvoice.ItemAvgCost = Convert.ToString(C1);
                    //    _context.InvItemMaster.Update(cInvoice);
                    //    await _context.SaveChangesAsync();
                    //}

                    #region Inventory History insert
                    //var WareHouse = _context.InvWarehouses.Where(c => c.WHCode == IMHeader.TranLocation);

                    //foreach (var item in WareHouse)
                    //{
                    var obj1 = new TblErpInvItemInventoryHistory()
                    {
                        ItemCode = invtItem.TranItemCode,
                        WHCode = author.WHCode,
                        //WHCode = cItemIds1,
                        TranDate = DateTime.Now,
                        TranType = "0",
                        //TranNumber = InvTrans,
                        TranNumber = IMHeader.TranNumber,
                        TranUnit = invtItem.TranItemUnit,
                        TranQty = (invtItem.TranItemQty),
                        unitConvFactor = invtItem.TranUOMFactor,
                        TranTotQty = (invtItem.TranItemQty),
                        TranPrice = invtItem.TranItemCost,
                        //ItemAvgCost = (auth.TranItemQty) / (auth.TranItemCost),
                        //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty))),
                        ItemAvgCost = (decimal)ItemAvgCost,
                        //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty))),
                        TranRemarks = "InsertedthroughAdjst",
                        IsActive = false,
                        CreatedOn = DateTime.Now
                    };
                    _context.InvItemInventoryHistory.Add(obj1);
                    //}
                    _context.SaveChanges();
                    #endregion Inventory History

                }
                #endregion
                #region Ispaid 
                var Paid = _context.IMAdjustmentsTransactionHeader.Where(e => e.TranNumber == transnumber.TranNumber);
                var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                POPaid.IsPaid = true;
                _context.IMAdjustmentsTransactionHeader.Update(POPaid).Property(x => x.Id).IsModified = false; ;

                await _context.SaveChangesAsync();
                #endregion
                #region Receipt FinancialTriggering
                if (PositiveSum != 0)
                {
                    try
                    {
                        Log.Info("----Info CreateApInvoiceSettlement method start----");


                        var Finobj = await _context.IMAdjustmentsTransactionHeader.FirstOrDefaultAsync(e => e.TranNumber == Transid);
                        var FinAcccode = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == author.InvDistGroup);

                        //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        //var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);



                        //var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        //if (invSeq is null)
                        //{
                        //    invoiceSeq = 1;
                        //    TblSequenceNumberSetting setting = new()
                        //    {
                        //        CreditSeq = invoiceSeq
                        //    };
                        //    await _context.Sequences.AddAsync(setting);
                        //}
                        //else
                        //{
                        //    invoiceSeq = invSeq.CreditSeq + 1;
                        //    invSeq.CreditSeq = invoiceSeq;
                        //    _context.Sequences.Update(invSeq);
                        //}



                        //var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.TranBranch);
                        var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();
                        //var invDistGroup = await _context.InvPoDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = invDistGroup.InvAssetAc,
                            //FinAcCode =  invDistGroup.InvAssetAc ,
                            CrAmount = 0,
                            DrAmount = PositiveSum,
                            //Source = "RP",
                            Source = "AD",
                            Gl = string.Empty,
                            Type = "NonAsset",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = inoviceIds,
                            //FinAcCode = "40106001",
                            FinAcCode = invDistGroup.InvNonAssetAc,
                            CrAmount = PositiveSum,
                            DrAmount = 0,
                            Source = "AD",
                            Gl = string.Empty,
                            Type = "Cash",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);
                        await _context.SaveChangesAsync();

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

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
                            //CompanyId = (int)Finobj.CompCode,
                            CompanyId = 1,

                            BranchCode = Finobj.TranBranch,
                            Batch = string.Empty,
                            //Source = "AD",
                            Source = "IN",
                            Remarks = Finobj.TranRemarks,
                            Narration = Finobj.TranRemarks,
                            JvDate = DateTime.Now,
                            //Amount = Finobj.TranTotalCost ?? 0,
                            Amount = PositiveSum,
                            DocNum = Finobj.TranReference,
                            CDate = DateTime.Now,
                            Posted = true,
                            PostedDate = DateTime.Now
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;

                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                            .Where(e => e.FinBranchCode == Finobj.TranBranch).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    //CompanyId = (int)Finobj.c,
                                    CompanyId = 1,
                                    BranchCode = Finobj.TranBranch,
                                    JvDate = DateTime.Now,
                                    //TranSource = "AD",
                                    TranSource = "IN",
                                    Trantype = "Invoice",
                                    DocNum = Finobj.TranReference,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = Finobj.TranRemarks,
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
                                BranchCode = Finobj.TranBranch,
                                Batch = string.Empty,
                                Remarks = Finobj.TranRemarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = Finobj.TranRemarks,

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

                            Remarks1 = Finobj.TranRemarks,
                            Remarks2 = Finobj.TranRemarks,
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
                                Source = "AD",
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
                        //await transaction.RollbackAsync();
                        Log.Error("Error in CreateApInvoiceSettlement Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        //return ApiMessageInfo.Status(0);
                        //return false;
                    }
                }
                if (NegativeSum != 0)
                {
                    int positiveJV = -1;
                    #region Issues FinancialTriggering
                    try
                    {
                        Log.Info("----Info CreateApInvoiceSettlement method start----");
                        var Finobj = await _context.IMAdjustmentsTransactionHeader.FirstOrDefaultAsync(e => e.TranNumber == Transid);
                        var FIncode = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == author.InvDistGroup);

                        //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        //var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);



                        //var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        //if (invSeq is null)
                        //{
                        //    invoiceSeq = 1;
                        //    TblSequenceNumberSetting setting = new()
                        //    {
                        //        CreditSeq = invoiceSeq
                        //    };
                        //    await _context.Sequences.AddAsync(setting);
                        //}
                        //else
                        //{
                        //    invoiceSeq = invSeq.CreditSeq + 1;
                        //    invSeq.CreditSeq = invoiceSeq;
                        //    _context.Sequences.Update(invSeq);
                        //}



                        //var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.TranBranch);
                        var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();
                        //var invDistGroup = await _context.InvPoDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = invDistGroup.InvNonAssetAc,
                            CrAmount = 0,
                            DrAmount = NegativeSum * positiveJV,
                            //Source = "IS",
                            Source = "AD",
                            Gl = string.Empty,
                            Type = "Cogs",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = inoviceIds,
                            FinAcCode = invDistGroup.InvAssetAc,
                            CrAmount = NegativeSum * positiveJV,
                            DrAmount = 0,
                            Source = "AD",
                            Gl = string.Empty,
                            Type = "Income",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);


                        await _context.SaveChangesAsync();

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

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
                            //CompanyId = (int)Finobj.CompCode,
                            CompanyId = 1,

                            BranchCode = Finobj.TranBranch,
                            Batch = string.Empty,
                            //Source = "AD",
                            Source = "IN",
                            Remarks = Finobj.TranRemarks,
                            Narration = Finobj.TranRemarks,
                            JvDate = DateTime.Now,
                            //Amount = Finobj.TranTotalCost ?? 0,
                            Amount = NegativeSum * positiveJV,
                            DocNum = Finobj.TranReference,
                            CDate = DateTime.Now,
                            Posted = true,
                            PostedDate = DateTime.Now
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;

                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                            .Where(e => e.FinBranchCode == Finobj.TranBranch).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    //CompanyId = (int)Finobj.c,
                                    CompanyId = 1,
                                    BranchCode = Finobj.TranBranch,
                                    JvDate = DateTime.Now,
                                    //TranSource = "AD",
                                    TranSource = "IN",
                                    Trantype = "Invoice",
                                    DocNum = Finobj.TranReference,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = Finobj.TranRemarks,
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
                                BranchCode = Finobj.TranBranch,
                                Batch = string.Empty,
                                Remarks = Finobj.TranRemarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = Finobj.TranRemarks,

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

                            Remarks1 = Finobj.TranRemarks,
                            Remarks2 = Finobj.TranRemarks,
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
                                Source = "AD",
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
                        //await transaction.RollbackAsync();
                        Log.Error("Error in CreateApInvoiceSettlement Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        //return ApiMessageInfo.Status(0);
                        //return false;
                    }
                    #endregion
                }
                #endregion
            }

            return obj;
        }
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }

    #endregion
}
