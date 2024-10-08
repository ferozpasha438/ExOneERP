//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using CIN.Application.Common;
//using CIN.DB;
//using MediatR;

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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventorymgtQuery
{
    #region GetUserSelectList

    public class GetUserSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetUserListHandler : IRequestHandler<GetUserSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUserSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetUserSelectList method start----");
            var item = await _context.SystemLogins.AsNoTracking()
                //.Where(e => e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UserName, Value = e.UserName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetUserSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetToLocationList

    public class GetToLocationList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetToLocationListHandler : IRequestHandler<GetToLocationList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetToLocationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetToLocationList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetToLocationList method start----");
            var item = await _context.InvWarehouses.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.WHName, Value = e.WHCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetToLocationList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetAccountSelectList

    public class GetAccountSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAccountsListHandler : IRequestHandler<GetAccountSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAccountSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAccountSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAccountSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetJVNumberSelectList

    public class GetJVNumberSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetJvNumberListHandler : IRequestHandler<GetJVNumberSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetJvNumberListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetJVNumberSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetJVNumberSelectList method start----");
            var item = await _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetJVNumberSelectList method Ends----");
            return item;
        }
    }

    #endregion
    #region GetBarCodeSelectList

    public class GetBarCodeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBarcodeListHandler : IRequestHandler<GetBarCodeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBarcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBarCodeSelectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBarCodeSelectList method start----");
            var item = await _context.InvItemsBarcode.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemBarcode, Value = e.ItemCode.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetBarCodeSelectList method Ends----");
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
            Log.Info("----Info GetItemCodeList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemCode, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetItemCodeList method Ends----");
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
            var item = await _context.InvItemMaster.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName, Value = e.ItemCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetItemNameList method Ends----");
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
    #region CreateIssuesRequest
    public class CreateIssuesRequest : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblInventoryReturntDto Input { get; set; }
    }

    public class CreateIssuesQueryHandler : IRequestHandler<CreateIssuesRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateIssuesQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateIssuesRequest request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateIssuesRequest method start----");
                    var transnumber = string.Empty;
                    var obj = request.Input;
                    TblIMTransactionHeader cObj = new();
                    if (obj.Id > 0)
                    {
                        cObj = await _context.IMTransactionHeader.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        transnumber = cObj.TranNumber;
                        cObj.TranNumber = transnumber;

                        //if (obj.TranType == "5")
                        //{

                        //    var IMheader = await _context.IMTransactionHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                        //    if (IMheader != null)
                        //        transnumber = Convert.ToString(int.Parse(IMheader.TranNumber) + 1);
                        //    else
                        //        transnumber = Convert.ToString(10001);
                        //}
                    }
                    else
                    {

                        var IMheader = await _context.IMTransactionHeader.OrderBy(e => e.TranNumber).LastOrDefaultAsync();
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
                        _context.IMTransactionHeader.Update(cObj).Property(x => x.Id).IsModified = false; ;
                    }
                    else
                    {
                        cObj.Id = 0;
                        cObj.IsActive = true;
                        cObj.CreatedOn = DateTime.Now;
                        await _context.IMTransactionHeader.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    if (request.Input.itemList.Count() > 0)
                    {
                        var oldAuthList = await _context.IMTransactionDetails.Where(e => e.TranNumber == transnumber).ToListAsync();
                        _context.IMTransactionDetails.RemoveRange(oldAuthList);
                        List<TblIMTransactionDetails> UOMList = new();
                        int i = 1;
                        string trans = "1";
                        var PoDetialTransNumber = await _context.IMTransactionDetails.OrderBy(e => e.Id).LastOrDefaultAsync();
                        foreach (var auth in request.Input.itemList)
                        {
                            if (PoDetialTransNumber != null)
                                trans = Convert.ToString(int.Parse(PoDetialTransNumber.SNo) + i++);
                            else
                                trans = Convert.ToString(int.Parse("0") + i++);

                            TblIMTransactionDetails UOMItem = new()
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
                        await _context.IMTransactionDetails.AddRangeAsync(UOMList);
                        await _context.SaveChangesAsync();

                    }

                    Log.Info("----Info CreateIssuesRequest method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateIssuesRequest Method");
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

    public class GetIMTransactionList : IRequest<PaginatedList<InventoryIMPaginationDto>>
    {
        public UserIdentityDto User { get; set; }

        public PaginationFilterDto Input { get; set; }
    }

    public class GetIMTransactionListHandler : IRequestHandler<GetIMTransactionList, PaginatedList<InventoryIMPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetIMTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<InventoryIMPaginationDto>> Handle(GetIMTransactionList request, CancellationToken cancellationToken)
        {
            //var search = request.Input.Query;
            //var list = await _context.IMTransactionHeader.AsNoTracking()
            //  .Where(e =>
            //                (e.TranNumber.Contains(search) || e.TranReference.Contains(search)

            //                 ))
            //   .OrderBy(request.Input.OrderBy)
            //  .ProjectTo<TblIMTransactionHeaderDto>(_mapper.ConfigurationProvider)
            //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            //return list;


            var search = request.Input.Query;
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            var oprApprvls = _context.TblPurTrnApprovalsList.Where(e => e.ServiceType == "ISSU").AsNoTracking();

            var enquiryHeads = _context.IMTransactionHeader.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.IMTransactionHeader.AsNoTracking()
              .Where(e => (e.TranNumber != (null)
                            ))
               .OrderBy(request.Input.OrderBy).Select(d => new InventoryIMPaginationDto
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
                   //ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "IS" && e.IsApproved && e.ServiceCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).TranNumber),
                   //IsApproved = enquiryHeads.Where(e => e.TranNumber == d.TranNumber).Count() == enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsApproved).Count(),
                   //HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.TranNumber == d.TranNumber).TranBranch),

                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == d.TranNumber && e.IsApproved),
                   IsApproved = oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Any(),
                   HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.TranBranch && e.AppAuthIssue),
                   CanSettle = brnApproval.Where(e => e.FinBranchCode == d.TranBranch).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceCode == d.TranNumber && e.IsApproved).Count(),
                   IsSettled = enquiryHeads.Where(e => e.TranNumber == d.TranNumber && e.IsPaid).Any(),
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;


        }

    }

    #endregion
    #region SingleItem

    public class GetIMDetails : IRequest<TblInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class GetIMDetailsHandler : IRequestHandler<GetIMDetails, TblInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetIMDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryReturntDto> Handle(GetIMDetails request, CancellationToken cancellationToken)
        {
            var transnumber = await _context.IMTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblInventoryReturntDto obj = new();
            var IMHeader = await _context.IMTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
            if (IMHeader is not null)
            {
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMTransactionDetailsDto
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
                obj.TranBranch = IMHeader.TranBranch;
                obj.itemList = iteminventory;
                obj.Id = IMHeader.Id;

            }
            return obj;
        }
    }

    #endregion
    #region Delete
    public class DeleteIMList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteIMQueryHandler : IRequestHandler<DeleteIMList, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteIMQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteIMList request, CancellationToken cancellationToken)
        {
            try
            {
                var IMde = await _context.IMTransactionHeader.FirstOrDefaultAsync(e => e.Id == request.Id);
                TblIMTransactionDetails IMdetails;
                IMdetails = _context.IMTransactionDetails.Where(d => d.TranNumber == IMde.TranNumber).First();
                _context.Entry(IMdetails).State = EntityState.Deleted;
                _context.SaveChanges();

                TblIMTransactionHeader IMHeader;
                IMHeader = _context.IMTransactionHeader.Where(d => d.TranNumber == IMde.TranNumber).First();
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

    #region Settlement


    public class IssuesSettelementList : IRequest<TblInventoryReturntDto>
    {
        public UserIdentityDto User { get; set; }
        public int id { get; set; }
    }

    public class IssuesSettelementListHandler : IRequestHandler<IssuesSettelementList, TblInventoryReturntDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IssuesSettelementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInventoryReturntDto> Handle(IssuesSettelementList request, CancellationToken cancellationToken)
        {
            var inoviceIds = 0;
            int invoiceSeq = 0;
            var transnumber = await _context.IMTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.id);
            TblInventoryReturntDto obj = new();
            var IMHeader = await _context.IMTransactionHeader.AsNoTracking().FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);

            if (IMHeader is not null)
            {
                var author = _context.InvWarehouses.Where(a => a.WHCode == IMHeader.TranLocation).Single();
                var Transid = IMHeader.TranNumber;

                var iteminventory = await _context.IMTransactionDetails.AsNoTracking()
                    .Where(e => transnumber.TranNumber == e.TranNumber)
                    .Select(e => new TblIMTransactionDetailsDto
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
                obj.TranLocation = IMHeader.TranLocation;
                #region QtyUpdate
                foreach (var invtItem in iteminventory)
                {
                    var invtUOM = await _context.InvItemsUOM.FirstOrDefaultAsync(e => e.ItemCode == invtItem.TranItemCode && e.ItemUOM == invtItem.TranItemUnit);

                    var cInvItems = _context.InvItemInventory.Where(e => e.ItemCode == invtItem.TranItemCode && e.WHCode == obj.TranLocation);
                    var cItems = cInvItems;
                    var cItemIds = await cItems.Select(e => e.ItemCode).ToListAsync();

                    var cInvItemMasters = _context.InvItemMaster.Where(e => e.ItemCode == invtItem.TranItemCode);
                    var cItemMasters = cInvItemMasters;
                    var cItemMasterIds = await cItemMasters.Select(e => e.ItemCode).ToListAsync();

                    //decimal? QtyOnPO = await cItems.SumAsync(e => e.QtyOnPO);
                    // decimal? QtyOH = await cItems.SumAsync(e => e.QtyOH);

                    decimal? QtyReserved = await cItems.SumAsync(e => e.QtyReserved);
                    //decimal? ItemAvgCost = 0;// await cInvItems.SumAsync(e => e.ItemAvgCost);
                    decimal? ItemLastPOCost = await cInvItems.SumAsync(e => e.ItemLastPOCost);
                    //var itmAvgcost = 0;
                    //var PoDetails = await _context.purchaseOrderDetails.Where(e => e.TranId != transnumber.TranNumber && e.TranItemCode == auth.TranItemCode).OrderBy(e => e.TranNumber).LastOrDefaultAsync();
                    //if (PoDetails is not null)
                    //    itmAvgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))));
                    //else
                    //    itmAvgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty))));

                    decimal ItemAvgCost = 0, originalTranItemQty = 0;
                    originalTranItemQty = invtItem.TranItemQty;

                    foreach (var invId in cItemIds)
                    {

                        var oldInventory = await cItems.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        decimal itemAvgCost = 0;
                        if (invtUOM.ItemConvFactor > 1)
                        {
                            invtItem.TranItemQty = invtItem.TranItemQty * invtUOM.ItemConvFactor;
                            //oldInventory.QtyOH = (((decimal)oldInventory.QtyOH * invtUOM.ItemConvFactor) - invtItem.TranItemQty);
                        }
                        else
                        {
                            invtItem.TranItemQty = invtItem.TranItemQty;
                            //oldInventory.QtyOH = ((decimal)oldInventory.QtyOH - invtItem.TranItemQty);
                        }

                        decimal tranItemCost = invtItem.TranItemCost;// - (invtItem.TranItemCost * invtItem.DiscPer) / 100;

                        if (invtUOM.ItemConvFactor > 1)
                        {
                            itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)(invtItem.TranItemQty / invtUOM.ItemConvFactor)))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));
                        }
                        else
                        {
                            itemAvgCost = ((((decimal)oldInventory.QtyOH) * ((decimal)oldInventory.ItemAvgCost)) + (((decimal)tranItemCost) * ((decimal)invtItem.TranItemQty))) / ((((decimal)oldInventory.QtyOH) + ((decimal)invtItem.TranItemQty)));

                        }

                        oldInventory.ItemAvgCost = itemAvgCost;
                        ItemAvgCost = oldInventory.ItemAvgCost;


                        oldInventory.QtyOH = ((decimal)oldInventory.QtyOH - invtItem.TranItemQty);

                        oldInventory.ItemLastPOCost = ((decimal)ItemLastPOCost + invtItem.TranItemCost);
                        _context.InvItemInventory.Update(oldInventory);
                        await _context.SaveChangesAsync();


                        var itemMaster = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invId);
                        if (itemMaster is not null)
                        {
                            var C1 = String.Format("{0:0.0000}", ItemAvgCost);
                            itemMaster.ItemAvgCost = Convert.ToString(C1);
                            _context.InvItemMaster.Update(itemMaster);
                            await _context.SaveChangesAsync();
                        }

                    }

                    //foreach (var invIds in cItemMasterIds)
                    //{
                    //    var cInvoice = await cItemMasters.FirstOrDefaultAsync(e => e.ItemCode == invIds);
                    //    //var itemsAVgcost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty)));
                    //    //var itemsAVgcost = 0;
                    //    //if (PoDetails is not null)
                    //    //    itemsAVgcost = (int)(((((decimal)PoDetails.TranItemQty) * ((decimal)PoDetails.TranItemCost)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)PoDetails.TranItemQty) + ((decimal)auth.TranItemQty))));
                    //    //else
                    //    //    itemsAVgcost = (int)(((((decimal)0) * ((decimal)0)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)0) + ((decimal)auth.TranItemQty))));
                    //    var itemsAVgcost = (decimal)ItemAvgCost;
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
                        TranQty = (invtItem.TranItemQty) * (-1),
                        unitConvFactor = invtItem.TranUOMFactor,
                        TranTotQty = (invtItem.TranItemQty) * (-1),
                        TranPrice = invtItem.TranItemCost,
                        //ItemAvgCost = ((((decimal)ItemLastPOCost) * ((decimal)QtyOH)) + (((decimal)auth.TranItemCost) * ((decimal)auth.TranItemQty))) / ((((decimal)QtyOH) + ((decimal)auth.TranItemQty))),
                        ItemAvgCost = (decimal)ItemAvgCost,
                        TranRemarks = "InsertedthroughIsus",
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
                var Paid = _context.IMTransactionHeader.Where(e => e.TranNumber == transnumber.TranNumber);
                var POPaid = await Paid.FirstOrDefaultAsync(e => e.TranNumber == transnumber.TranNumber);
                POPaid.IsPaid = true;
                _context.IMTransactionHeader.Update(POPaid).Property(x => x.Id).IsModified = false; ;

                await _context.SaveChangesAsync();
                #endregion
                #region FinancialTriggering
                try
                {
                    Log.Info("----Info CreateApInvoiceSettlement method start----");
                    var Finobj = await _context.IMTransactionHeader.FirstOrDefaultAsync(e => e.TranNumber == Transid);
                    //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == Finobj.VenCatCode);
                    //var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == Finobj.PaymentID);
                    var FIncode = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == author.InvDistGroup);

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
                        FinAcCode = invDistGroup.InvCashPOAC,
                        CrAmount = 0,
                        DrAmount = Finobj.TranTotalCost,
                        Source = "IS",
                        Gl = string.Empty,
                        Type = "Cogs",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution1);

                    TblFinTrnDistribution distribution2 = new()
                    {
                        InvoiceId = inoviceIds,
                        //FinAcCode = "20202001",
                        FinAcCode = invDistGroup.InvAssetAc,

                        CrAmount = Finobj.TranTotalCost,
                        DrAmount = 0,
                        Source = "IS",
                        Gl = string.Empty,
                        Type = "Income",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution2);


                    //var invoiceItem = await _context.purchaseOrderDetails.FirstOrDefaultAsync(e => e.TranId == Finobj.TranNumber);
                    //var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.ItemTaxPer).ToString());

                    //if (tax is null)
                    //    throw new NullReferenceException("Tax is empty");

                    //TblFinTrnDistribution distribution3 = new()
                    //{
                    //    InvoiceId = inoviceIds,
                    //    FinAcCode = tax?.InputAcCode01,
                    //    CrAmount = 0,
                    //    DrAmount = Finobj.Taxes,
                    //    Source = "PO",
                    //    Gl = string.Empty,
                    //    Type = "VAT",
                    //    CreatedOn = DateTime.Now
                    //};

                    //await _context.FinDistributions.AddAsync(distribution3);
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
                        //Source = "IS",
                        Source = "IN",
                        Remarks = Finobj.TranRemarks,
                        Narration = Finobj.TranRemarks,
                        JvDate = DateTime.Now,
                        //Amount = Finobj.TranTotalCost ?? 0,
                        Amount = Finobj.TranTotalCost,
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
                                //TranSource = "IS",
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
                            Source = "IS",
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

            return obj;
        }
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }

    #endregion
}
