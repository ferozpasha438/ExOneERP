using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryMgtDtos;
//using CIN.Application.PurchaseSetupDtos;
//using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.GeneralLedger;
using CIN.Domain.InventoryMgt;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
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
    #region GetTransferUserSelectList

    public class GetTransferUserSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferUserListHandler : IRequestHandler<GetTransferUserSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferUserListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferUserSelectList request, CancellationToken cancellationToken)
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
    #region GetTransferToLocationList

    public class GetTransferToLocationList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferToLocationListHandler : IRequestHandler<GetTransferToLocationList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferToLocationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferToLocationList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferToLocationList method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferToLocationList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferAccountSelectList

    public class GetTransferAccountSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferAccountsListHandler : IRequestHandler<GetTransferAccountSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferAccountSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferAccountSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferAccountSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferJVNumberSelectList

    public class GetTransferJVNumberSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferJvNumberListHandler : IRequestHandler<GetTransferJVNumberSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferJvNumberListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferJVNumberSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferJVNumberSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferJVNumberSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferBarCodeSelectList

    public class GetTransferBarCodeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferBarcodeListHandler : IRequestHandler<GetTransferBarCodeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferBarcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferBarCodeSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferBarCodeSelectList method start----");
            var item = await _context.InvItemsBarcode.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemBarcode, Value = e.ItemCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferBarCodeSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferItemCodeList

    public class GetTransferItemCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferItemCodeListHandler : IRequestHandler<GetTransferItemCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferItemCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferItemCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferItemCodeList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemCode, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferItemCodeList method Ends----");
            return item;
        }
    }

    #endregion
    #region TransferProductUomtPriceItem

    public class TransferProductUomtPriceItem : IRequest<TransferProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemList { get; set; }


    }

    public class TransferProductUOMFactorItemHandler : IRequestHandler<TransferProductUomtPriceItem, TransferProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TransferProductUOMFactorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TransferProductUnitPriceDTO> Handle(TransferProductUomtPriceItem request, CancellationToken cancellationToken)
        {
            var itemcode = request.ItemList.Split('_')[0];
            var ItemUOM = request.ItemList.Split('_')[1];

            Log.Info("----Info TransferProductUomtPriceItem method start----");
            var item = await _context.InvItemsUOM.AsNoTracking()
                 .Where(e =>
                            (e.ItemCode.Contains(itemcode) && e.ItemUOM.Contains(ItemUOM)
                             ))
               .Select(Product => new TransferProductUnitPriceDTO
               {
                   tranItemCode = Product.ItemCode,
                   tranItemUomFactor = Product.ItemConvFactor,
                   ItemAvgcost = Product.ItemAvgCost,

               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info TransferProductUomtPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferUOMList

    public class GetTransferUOMSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferUOMselectListHandler : IRequestHandler<GetTransferUOMSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferUOMselectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferUOMSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferUOMList method start----");
            var item = await _context.InvUoms.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UOMName, Value = e.UOMCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferUOMList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetTransferItemNameList

    public class GetTransferItemNameList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetTransferItemNameListHandler : IRequestHandler<GetTransferItemNameList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferItemNameListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTransferItemNameList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTransferItemNameList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetTransferItemNameList method Ends----");
            return item;
        }
    }

    #endregion
    #region ProductUnitPriceItem

    public class TransferProductUnitPriceItem : IRequest<TransferProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Itemcode { get; set; }
    }

    public class TransferProductUnitPriceItemHandler : IRequestHandler<TransferProductUnitPriceItem, TransferProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TransferProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TransferProductUnitPriceDTO> Handle(TransferProductUnitPriceItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info TransferProductUnitPriceItem method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .Where(e => e.ItemCode == request.Itemcode)
                 .Include(e => e.InvUoms)
               .Select(Product => new TransferProductUnitPriceDTO
               {

                   tranItemCode = Product.ItemCode,
                   tranItemName = Product.ShortName,
                   tranItemUnitCode = Product.ItemBaseUnit,
                   tranItemCost = Product.ItemAvgCost,


               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info TransferProductUnitPriceItem method Ends----");
            return item;
        }
    }

    #endregion
    #region CreateTransferRequest
    public class TransferCreateIssuesRequest : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblTransferInventoryReturntDto Input { get; set; }
    }

    public class TransferCreateIssuesQueryHandler : IRequestHandler<TransferCreateIssuesRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public TransferCreateIssuesQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(TransferCreateIssuesRequest request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateTransferRequest method start----");
                    var transnumber = string.Empty;
                    var obj = request.Input;
                    TblIMTransferTransactionHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.IMTransferTransactionHeader.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                    }
                    else
                    {

                        var IMheader = await _context.IMTransferTransactionHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
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
                    cObj.BranchCode = obj.BranchCode;

                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        //cObj.Id = 0;
                        cObj.TranLastEditDate = DateTime.Now;
                        cObj.TranLastEditUser = request.User.UserId.ToString();
                        _context.IMTransferTransactionHeader.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.IMTransferTransactionHeader.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.IMTransferTransactionDetails.Where(e => e.TranNumber == transnumber).ToListAsync();
                        _context.IMTransferTransactionDetails.RemoveRange(oldAuthList);
                        List<TblIMTransferTransactionDetails> UOMList = new();
                        int i = 1;
                        string trans = "1";
                        var PoDetialTransNumber = await _context.IMTransferTransactionDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var auth in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.SNo) + i++);
                            else
                                trans = Convert.ToString(int.Parse("0") + i++);

                            TblIMTransferTransactionDetails UOMItem = new()
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
                        await _context.IMTransferTransactionDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();

                    }

                    Log.Info("----Info CreateAjustmentsRequest method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateTransferRequest Method");
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

    public class GetIMTransferTransactionList : IRequest<PaginatedList<TransferPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetTransferIMTransactionListHandler : IRequestHandler<GetIMTransferTransactionList, PaginatedList<TransferPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferIMTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TransferPaginationDto>> Handle(GetIMTransferTransactionList request, CancellationToken cancellationToken)
        {
            //var search = request.Input.Query;
            //var list = await _context.IMTransferTransactionHeader.AsNoTracking()
            //  .Where(e =>
            //                (e.TranNumber.Contains(search) || e.TranReference.Contains(search)

            //                 ))
            //   .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblIMTransferTransactionHeaderDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            //return list;
            var search = request.Input.Query;
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "TRNS").AsNoTracking();

            var enquiryHeads = _context.IMTransferTransactionHeader.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.IMTransferTransactionHeader.AsNoTracking()
              .Where(e => (e.TranNumber != (null)
                            ))
               .OrderBy(request.Input.OrderBy).Select(d => new TransferPaginationDto
               {
                   TranNumber = d.TranNumber,
                   TranDate = d.TranDate,
                   TranReference = d.TranReference,
                   TranLocation = d.TranLocation,
                   TranToLocation = d.TranToLocation,
                   TranDocNumber = d.TranDocNumber,
                   BranchCode = d.BranchCode,
                   Id = d.Id,
                   TranCreateUser = d.TranCreateUser,
                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "ST" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).TranNumber),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),
                   //HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).BranchCode),
                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.TranNumber && e.IsApproved),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthTrans),
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Count(),
                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;


        }

    }

    #endregion
    #region SingleItem

    public class GetTransferIMDetails : IRequest<TblTransferInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetTransferIMDetailsHandler : IRequestHandler<GetTransferIMDetails, TblTransferInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTransferIMDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblTransferInventoryReturntDto> Handle(GetTransferIMDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblTransferInventoryReturntDto obj = new();
            var IMHeader = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (IMHeader is not null)
            {
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMTransferTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMTransferTransactionDetailsDto
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
                obj.BranchCode = IMHeader.BranchCode;

            }
            return obj;
        }
    }

    #endregion
    #region TransferDelete
    public class TransferDeleteIMList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class TransferDeleteIMQueryHandler : IRequestHandler<TransferDeleteIMList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public TransferDeleteIMQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(TransferDeleteIMList request, CancellationToken cancellationToken)
        {
            try
            {
                var IMde = await _context.IMTransferTransactionHeader.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblIMTransferTransactionDetails IMdetails;
                IMdetails = _context.IMTransferTransactionDetails.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMdetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblIMTransferTransactionHeader IMHeader;
                IMHeader = _context.IMTransferTransactionHeader.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMHeader).State = EntityState.Deleted;
                _context.SaveChanges();
                return request.Id;
                //return 0;
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


    #region Settlement

    public class TransferSettelementList : IRequest<TblInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class TransferSettelementListHandler : IRequestHandler<TransferSettelementList, TblInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TransferSettelementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryReturntDto> Handle(TransferSettelementList request, CancellationToken cancellationToken)
        {
            var inoviceIds = 0;
            // int invoiceSeq = 0;
            var transnumber = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblInventoryReturntDto obj = new();
            var IMHeader = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);



            // var TranNumber = new SqlParameter("@transnumner", IMHeader.TranNumber);
            //var FInAdjvalue = _context
            //            .Adjs
            //            .FromSqlRaw("exec sp_Transfersvalues @transnumner",
            //                TranNumber)
            //            .ToList();
            //var FInAdjvalue = _context.Adjs.FromSqlRaw("sp_Transfersvalues"+ IMHeader.TranNumber).ToList();

            try
            {
                if (IMHeader is not null)
                {
                    var fromWarehouse = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranLocation).Single();
                    var toWarehouse = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranToLocation).Single();

                    var IMHeaderDetails = _context.IMTransferTransactionDetails.AsNoTracking().Where(e => e.TranNumber == IMHeader.TranNumber);

                    decimal PositiveSum = IMHeaderDetails.Where(e => e.TranTotCost > 0).Sum(e => e.TranTotCost);
                     //NegativeSum = IMHeaderDetails.Where(e => e.TranTotCost < 0).Sum(e => e.TranTotCost);

                    var Transid = IMHeader.TranNumber;

                    var iteminventory = await _context.IMTransferTransactionDetails.AsNoTracking()
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


                    #region QtyUpdate On FromWarehouse

                    foreach (var invtItem in iteminventory)
                    {
                        var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == invtItem.TranItemCode && e.WHCode == fromWarehouse.WHCode);
                        var cItems = cInvItems;
                        var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();

                        var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == invtItem.TranItemCode);
                        var cItemMasters = cInvItemMasters;
                        var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();

                        //decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOnPO);
                        //decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);

                        decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                        decimal? ItemAvgCost = 0;// await cInvItems .SumAsync(e => e.ItemAvgCost);
                        decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);

                        foreach (var invId in cItemIds)
                        {

                            var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                            decimal tranItemCost = invtItem.TranItemCost;// - (invtItem.TranItemCost * invtItem.DiscPer) / 100;
                            oldInventory.ItemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));
                            ItemAvgCost = oldInventory.ItemAvgCost;
                            oldInventory.QtyOH = ((decimal)oldInventory.QtyOH - invtItem.TranItemQty);

                            oldInventory.ItemLastPOCost = ((decimal)ItemLastPOCost + invtItem.TranItemCost);
                            _context.InvItemInventory.Update(oldInventory);
                            await _context.SaveChangesAsync();


                            //var cInvoice = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                            //cInvoice.QtyOH = ((decimal)QtyOH + invtItem.TranItemQty);
                            ////cInvoice.QtyOH = ((decimal)QtyOH);

                            ////cInvoice.QtyOnPO = ((decimal)QtyOnPO + auth.TranItemQty);
                            ////cInvoice.QtyReserved = ((decimal)QtyReserved + auth.TranItemQty);
                            ////cInvoice.QtyReserved = ((decimal)QtyReserved);
                            //cInvoice.ItemAvgCost = (decimal)ItemAvgCost;
                            ////cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));
                            ////cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                            ////cInvoice.ItemAvgCost = (((decimal)QtyReserved + auth.TranItemQty)) / (((decimal)ItemAvgCost + auth.TranItemCost));
                            ////cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost + auth.TranItemCost);
                            ////cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost);

                            //_context.InvItemInventory.Update(cInvoice);
                            //await _context.SaveChangesAsync();

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
                            WHCode = fromWarehouse.WHCode,
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
                            TranRemarks = "InsertedthroTransfer",
                            IsActive = false,
                            CreatedOn = DateTime.Now
                        };
                        _context.InvItemInventoryHistory.Add(obj1);
                        //}
                        _context.SaveChanges();
                        #endregion Inventory History

                    }
                    #endregion

                    #region QtyUpdate On TransferWarehouse
                    foreach (var invtItem in iteminventory)
                    {
                        var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == invtItem.TranItemCode && e.WHCode == toWarehouse.WHCode);
                        var cItems = cInvItems;
                        var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();

                        var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == invtItem.TranItemCode);
                        var cItemMasters = cInvItemMasters;
                        var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();

                        //decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOnPO);
                        //decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);

                        decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                        decimal? ItemAvgCost = 0;// await cInvItems .SumAsync(e => e.ItemAvgCost);
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
                            //cInvoice.QtyOH = ((decimal)QtyOH + invtItem.TranItemQty);
                            ////cInvoice.QtyOH = ((decimal)QtyOH);

                            ////cInvoice.QtyOnPO = ((decimal)QtyOnPO + auth.TranItemQty);
                            ////cInvoice.QtyReserved = ((decimal)QtyReserved + auth.TranItemQty);
                            ////cInvoice.QtyReserved = ((decimal)QtyReserved);
                            //cInvoice.ItemAvgCost = (decimal)ItemAvgCost;
                            ////cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOnPO)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOnPO) + ((decimal)auth.TranItemQty)));
                            ////cInvoice.ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                            ////cInvoice.ItemAvgCost = (((decimal)QtyReserved + auth.TranItemQty)) / (((decimal)ItemAvgCost + auth.TranItemCost));
                            ////cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost + auth.TranItemCost);
                            ////cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost);

                            //_context.InvItemInventory.Update(cInvoice);
                            //await _context.SaveChangesAsync();

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
                            WHCode = toWarehouse.WHCode,
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
                            TranRemarks = "InsertedthroTransfer",
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
                    var Paid = _context.IMTransferTransactionHeader.Where(e => e.TranNumber == transnumber.TranNumber);
                    var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                    POPaid.IsPaid = true;
                    _context.IMTransferTransactionHeader.Update(POPaid).Property(x => x.Id).IsModified = false; ;

                    await _context.SaveChangesAsync();
                    #endregion
                    #region Receipt FinancialTriggering
                    if (fromWarehouse.WHCode.HasValue())//if (PositiveSum != 0)
                    {
                        try
                        {
                            Log.Info("----Info CreateApInvoiceSettlement method start----");


                            var Finobj = await _context.IMTransferTransactionHeader.FirstOrDefaultAsync(e => e.TranNumber == Transid);
                            var FinAcccode = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == fromWarehouse.InvDistGroup);

                            //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                            //var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);


                            //var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                            var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == Finobj.BranchCode);
                            var invDistGroup = await _context.InvDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();
                            //var invDistGroup = await _context.InvPoDistributionGroups.OrderBy(e => e.Id).LastOrDefaultAsync();

                            TblFinTrnDistribution distribution1 = new()
                            {
                                InvoiceId = inoviceIds,
                                // FinAcCode = FinAcccode.invAssetAc,
                                FinAcCode = invDistGroup.InvAssetAc,
                                CrAmount = PositiveSum,
                                DrAmount = 0,
                                Source = "TR",
                                //Source = "RP",
                                Gl = string.Empty,
                                Type = IsNotCreditPay("cash") ? "paycode" : "Vendor",
                                CreatedOn = DateTime.Now
                            };
                            await _context.FinDistributions.AddAsync(distribution1);

                            TblFinTrnDistribution distribution2 = new()
                            {
                                InvoiceId = inoviceIds,
                                //FinAcCode = "40106001",
                                FinAcCode = invDistGroup.InvCOGSAc,
                                CrAmount = 0,
                                DrAmount = PositiveSum,
                                Source = "TR",
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

                                BranchCode = Finobj.BranchCode,
                                Batch = string.Empty,
                                //Source = "TR",
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
                                .Where(e => e.FinBranchCode == Finobj.BranchCode).ToListAsync();
                            if (branchAuths.Count() > 0)
                            {
                                List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                                foreach (var item in branchAuths)
                                {
                                    TblFinTrnJournalVoucherApproval approval = new()
                                    {
                                        //CompanyId = (int)Finobj.c,
                                        CompanyId = 1,
                                        BranchCode = Finobj.BranchCode,
                                        JvDate = DateTime.Now,
                                        //TranSource = "TR",
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
                                    BranchCode = Finobj.BranchCode,
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
                                    Source = "TR",
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
                    if (toWarehouse.WHCode.HasValue()) //if (NegativeSum != 0)
                    {
                        #region Issues FinancialTriggering
                        try
                        {
                            Log.Info("----Info CreateApInvoiceSettlement method start----");
                            var Finobj = await _context.IMAdjustmentsTransactionHeader.FirstOrDefaultAsync(e => e.TranNumber == Transid);
                            var FIncode = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == fromWarehouse.InvDistGroup);

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
                                FinAcCode = invDistGroup.InvCOGSAc,
                                CrAmount = PositiveSum,
                                DrAmount = 0,
                                //Source = "IS",
                                Source = "TR",
                                Gl = string.Empty,
                                Type = IsNotCreditPay("cash") ? "paycode" : "Vendor",
                                CreatedOn = DateTime.Now
                            };
                            await _context.FinDistributions.AddAsync(distribution1);

                            TblFinTrnDistribution distribution2 = new()
                            {
                                InvoiceId = inoviceIds,
                                FinAcCode = invDistGroup.InvAssetAc,
                                CrAmount = 0,
                                DrAmount = PositiveSum,
                                //Source = "IS",
                                Source = "TR",
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
                                Source = "IN",
                                //Source = "TR",
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
                                        TranSource = "IN",
                                        //TranSource = "TR",
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
                                    //Source = "IS",
                                    Source = "TR",
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
            }
            catch (Exception ex)
            {

                throw;
            }

            return obj;
        }
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }


    #endregion

    #region stockTransfer
    public class stockTransfer : IRequest<TblInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class stockTransferListHandler : IRequestHandler<stockTransfer, TblInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public stockTransferListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryReturntDto> Handle(stockTransfer request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblInventoryReturntDto obj = new();
            var IMHeader = await _context.IMTransferTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (IMHeader is not null)
            {
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMTransferTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMTransferTransactionDetailsDto
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
                foreach (var auth in iteminventory)
                {
                    var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == auth.TranItemCode && e.WHCode == IMHeader.TranLocation);
                    var cItems = cInvItems;
                    var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();
                    var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == auth.TranItemCode);
                    var cItemMasters = cInvItemMasters;
                    var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();
                    decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);
                    decimal? ItemAvgCost = await cInvItems
                                                   .SumAsync(e => e.ItemAvgCost);
                    decimal? ItemLastPOCost = await cInvItems
                                                 .SumAsync(e => e.ItemLastPOCost);

                    foreach (var invId in cItemIds)
                    {
                        var cInvoice = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        cInvoice.QtyOH = ((decimal)QtyOH - auth.TranItemQty);
                        cInvoice.ItemAvgCost = (decimal)ItemAvgCost;
                        //cInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost - auth.TranItemCost);
                        _context.InvItemInventory.Update(cInvoice);
                        await _context.SaveChangesAsync();
                    }
                    //foreach (var invIds in cItemMasterIds)
                    //{
                    //    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
                    //    var itemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                    //    var C1 = String.Format("{0:0.0000}", itemsAVgcost);
                    //    cInvoice.ItemAvgCost = Convert.ToString(C1);
                    //    _context.InvItemMaster.Update(cInvoice);
                    //    await _context.SaveChangesAsync();
                    //}

                    #region Inventory History insert
                    var author = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranLocation).Single();
                    var obj1 = new TblErpInvItemInventoryHistory()
                    {
                        ItemCode = auth.TranItemCode,
                        WHCode = author.WHCode,
                        //WHCode = cItemIds1,
                        TranDate = DateTime.Now,
                        TranType = "0",
                        //TranNumber = InvTrans,
                        TranNumber = IMHeader.TranNumber,
                        TranUnit = auth.TranItemUnit,
                        TranQty = (auth.TranItemQty),
                        unitConvFactor = auth.TranUOMFactor,
                        TranTotQty = (auth.TranItemQty),
                        TranPrice = auth.TranItemCost,
                        ItemAvgCost = (decimal)ItemAvgCost,
                        //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty))),

                        TranRemarks = "StockTransfer",
                        IsActive = false,
                        CreatedOn = DateTime.Now
                    };
                    _context.InvItemInventoryHistory.Add(obj1);
                    _context.SaveChanges();
                    #endregion Inventory History 

                    #region TOStock
                    var TOcInvItems = _context.InvItemInventory.Where(e => e.ItemCode == auth.TranItemCode && e.WHCode == IMHeader.TranToLocation);
                    var TOcItems = TOcInvItems;
                    var TOcItemIds = await TOcItems.Select(e => e.ItemCode).ToListAsync();
                    var TOcInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == auth.TranItemCode);
                    var TOcItemMasters = TOcInvItemMasters;
                    var TOcItemMasterIds = await TOcItemMasters.Select(e => e.ItemCode).ToListAsync();
                    decimal? TOQtyOH = await TOcItems.SumAsync(e => e.QtyOH);
                    decimal? TOItemAvgCost = await TOcInvItems
                                                   .SumAsync(e => e.ItemAvgCost);
                    decimal? TOItemLastPOCost = await TOcInvItems
                                                 .SumAsync(e => e.ItemLastPOCost);

                    foreach (var invId in TOcItemIds)
                    {
                        var TOcInvoice = await TOcItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        TOcInvoice.QtyOH = ((decimal)TOQtyOH + auth.TranItemQty);
                        TOcInvoice.ItemAvgCost = (decimal)ItemAvgCost;
                        //TOcInvoice.ItemLastPOCost = ((decimal)ItemLastPOCost - auth.TranItemCost);
                        _context.InvItemInventory.Update(TOcInvoice);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var invIds in TOcItemMasterIds)
                    {
                        var TOcInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
                        //var TOitemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                        var TOitemsAVgcost = (decimal)ItemAvgCost;

                        var C1 = String.Format("{0:0.0000}", TOitemsAVgcost);
                        TOcInvoice.ItemAvgCost = Convert.ToString(C1);
                        _context.InvItemMaster.Update(TOcInvoice);
                        await _context.SaveChangesAsync();
                    }

                    #region Inventory History insert
                    var TOauthor = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranLocation).Single();
                    var TOobj1 = new TblErpInvItemInventoryHistory()
                    {
                        ItemCode = auth.TranItemCode,
                        WHCode = TOauthor.WHCode,
                        //WHCode = cItemIds1,
                        TranDate = DateTime.Now,
                        TranType = "0",
                        //TranNumber = InvTrans,
                        TranNumber = IMHeader.TranNumber,
                        TranUnit = auth.TranItemUnit,
                        TranQty = (auth.TranItemQty),
                        unitConvFactor = auth.TranUOMFactor,
                        TranTotQty = (auth.TranItemQty),
                        TranPrice = auth.TranItemCost,

                        //ItemAvgCost = ((((decimal)TOItemLastPOCost) * ((decimal)TOQtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)TOQtyOH) + ((decimal)auth.TranItemQty))),
                        ItemAvgCost = (decimal)ItemAvgCost,
                        TranRemarks = "StockTransfer",
                        IsActive = false,
                        CreatedOn = DateTime.Now
                    };
                    _context.InvItemInventoryHistory.Add(TOobj1);
                    _context.SaveChanges();
                    #endregion Inventory History
                    #endregion 

                }
                #endregion
                #region Ispaid 
                var Paid = _context.IMTransferTransactionHeader.Where(e => e.TranNumber == transnumber.TranNumber);
                var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                POPaid.IsPaid = true;
                _context.IMTransferTransactionHeader.Update(POPaid).Property(x => x.Id).IsModified = false; ;

                await _context.SaveChangesAsync();
                #endregion
            }

            return obj;
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
