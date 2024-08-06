using AutoMapper;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.CashVoucher;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.GeneralLedgerQuery
{
    #region GetPagedList

    public class GetCashVoucherList : IRequest<PaginatedList<FinTrnCashVoucherListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCashVoucherListHandler : IRequestHandler<GetCashVoucherList, PaginatedList<FinTrnCashVoucherListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCashVoucherListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<FinTrnCashVoucherListDto>> Handle(GetCashVoucherList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            var custApproval = _context.CashVoucherApprovals.AsNoTracking();

            var cvLineItems = _context.CashVoucherItems.AsNoTracking().AsQueryable();

            var list = await _context.CashVouchers.AsNoTracking()
              .Where(e => e.BranchCode.Contains(search) || e.Batch.Contains(search) ||
               e.SysCompanyBranch.BranchName.Contains(search) ||
               e.DocNum.Contains(search) ||
               e.Remarks.Contains(search) ||
               e.VoucherNumber.Contains(search) || e.SpVoucherNumber.Contains(search)
              )
               .OrderBy(request.Input.OrderBy)
                .Select(d => new FinTrnCashVoucherListDto
                {
                    Id = d.Id,
                    VoucherNumber = d.VoucherNumber == string.Empty ? d.SpVoucherNumber : d.VoucherNumber,
                    JvDate = d.JvDate,
                    BranchCode = d.BranchCode,
                    BranchName = d.SysCompanyBranch.BranchName,
                    Amount = d.Amount,
                    Source = d.Source,
                    DocNum = d.DocNum,
                    VoucherType = d.VoucherType,
                    CBookCode = d.CBookCode,
                    Remarks = d.Remarks,
                    Posted = d.Posted,
                    PostedDate = d.PostedDate,
                    IsDrCrAmtSame = cvLineItems.Where(e => e.CashVoucherId == d.Id).Count() >= 2 && (cvLineItems.Where(e => e.CashVoucherId == d.Id).Sum(e => e.DrAmount) == cvLineItems.Where(e => e.CashVoucherId == d.Id).Sum(e => e.CrAmount) && d.Amount == cvLineItems.Where(e => e.CashVoucherId == d.Id).Sum(e => e.CrAmount)),
                    HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthCV),
                    ApprovedUser = custApproval.Any(e => e.LoginId == request.User.UserId && e.CashVoucherId == d.Id && e.IsApproved),
                    IsApproved = custApproval.Where(e => e.CashVoucherId == d.Id && e.IsApproved).Any(),
                    CanSettle = brnApproval.Where(e => e.FinBranchCode == d.BranchCode && e.AppAuthCV).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= custApproval.Where(e => e.CashVoucherId == d.Id && e.IsApproved).Count(),
                    IsSettled = d.Posted,
                    IsVoid = d.Void
                })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region SingleItem

    public class GetCashVoucher : IRequest<CreateCashVoucherDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCashVouchersHandler : IRequestHandler<GetCashVoucher, CreateCashVoucherDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCashVouchersHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreateCashVoucherDto> Handle(GetCashVoucher request, CancellationToken cancellationToken)
        {
            CreateCashVoucherDto obj = new();
            var jv = await _context.CashVouchers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            var isPayment = jv.VoucherType.ToLower() == "payment" ? true : false;

            if (jv is not null)
            {
                var itemList = await _context.CashVoucherItems.AsNoTracking()
                    .Where(e => e.CashVoucherId == jv.Id && (isPayment ? e.CrAmount == 0 : e.DrAmount == 0))
                    .Select(obj1 => new TblFinTrnCashVoucherItemDto
                    {
                        Id = obj1.Id,
                        BranchName = obj1.SysCompanyBranch.BranchName,
                        BranchCode = obj1.BranchCode,
                        FinAcCode = obj1.FinAcCode,
                        //+ obj1.FinDefMainAccounts.FinAcName,

                        Description = obj1.Description,
                        Remarks = obj1.Remarks,
                        Batch = obj1.Batch,
                        Batch2 = obj1.Batch2,
                        CostAllocation = obj1.CostAllocation,
                        CostSegCode = obj1.CostSegCode,
                        Payment = obj1.Payment,
                    }).ToListAsync();

                obj.JvDate = jv.JvDate;
                obj.CompanyId = jv.CompanyId;
                obj.BranchCode = jv.BranchCode;
                obj.Batch = jv.Batch;
                obj.Remarks = jv.Remarks;
                obj.Narration = jv.Narration;
                obj.Amount = jv.Amount ?? 0;
                obj.DocNum = jv.DocNum;
                obj.ItemList = itemList;
                obj.VoucherType = jv.VoucherType;
                obj.CBookCode = jv.CBookCode;
                obj.SiteCode = jv.SiteCode;

            }
            return obj;
        }
    }

    #endregion


    #region CreateUpdateCashVoucher

    public class CreateCashVoucher : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateCashVoucherDto Input { get; set; }
    }
    public class CreateCashVoucherQueryHandler : IRequestHandler<CreateCashVoucher, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCashVoucherQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateCashVoucher request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateCashVoucher method start----");

                    string spCashVoucherNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                    //CashVoucherNumber = await _context.TranCashVouchers.CountAsync();
                    //CashVoucherNumber += 1;
                    TblFinTrnCashVoucher JV = new();
                    var obj = request.Input;

                    if (request.Input.Id > 0)
                    {
                        JV = await _context.CashVouchers.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        spCashVoucherNumber = JV.SpVoucherNumber;
                        JV.CompanyId = obj.CompanyId;
                        JV.BranchCode = obj.BranchCode;
                        JV.Batch = obj.Batch;
                        JV.Remarks = obj.Remarks;
                        JV.Narration = obj.Narration;
                        JV.JvDate = obj.JvDate;
                        JV.Amount = obj.Amount ?? 0;
                        JV.DocNum = obj.DocNum;
                        JV.VoucherType = obj.VoucherType;
                        JV.CBookCode = obj.CBookCode;
                        JV.SiteCode = obj.SiteCode;

                        var items = _context.CashVoucherItems.Where(e => e.CashVoucherId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.CashVouchers.Update(JV);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        JV = new()
                        {
                            SpVoucherNumber = spCashVoucherNumber,
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            Batch = obj.Batch,
                            Source = "GL",
                            Remarks = obj.Remarks,
                            Narration = obj.Narration,
                            JvDate = obj.JvDate,
                            Amount = obj.Amount ?? 0,
                            DocNum = obj.DocNum,
                            VoucherType = obj.VoucherType,
                            CBookCode = obj.CBookCode,
                            VoucherNumber = string.Empty,
                            CDate = DateTime.Now,
                            SiteCode = obj.SiteCode
                        };

                        await _context.CashVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();
                    }
                    //var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.CompanyId);
                    //if (company is not null)
                    //{
                    //    company.CashVoucherSeq++;
                    //    await _context.SaveChangesAsync();
                    //}

                    Log.Info("----Info CreateUpdateCashVoucher method Exit----");

                    var jvId = JV.Id;
                    var CashVoucherItems = request.Input.ItemList;
                    var isPayment = obj.VoucherType.ToLower() == "payment" ? true : false;

                    if (CashVoucherItems.Count > 0)
                    {
                        List<TblFinTrnCashVoucherItem> CashVoucherItemsList = new();

                        foreach (var obj1 in CashVoucherItems)
                        {
                            var CashVoucherItem = new TblFinTrnCashVoucherItem
                            {
                                CashVoucherId = jvId,
                                BranchCode = obj1.BranchCode,
                                Batch = obj1.Batch,
                                Batch2 = obj1.Batch2,
                                CostAllocation = obj1.CostAllocation,
                                CostSegCode = obj1.CostSegCode,
                                Remarks = obj1.Remarks,
                                Payment = obj1.Payment,
                                DrAmount = isPayment ? (obj1.Payment ?? 0): 0,
                                CrAmount = isPayment ? 0 : (obj1.Payment ?? 0 ),
                                FinAcCode = obj1.FinAcCode,
                                Description = obj1.Description,
                                SiteCode = obj.SiteCode

                            };
                            CashVoucherItemsList.Add(CashVoucherItem);
                        }
                        if (CashVoucherItemsList.Count > 0)
                        {
                            var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.CBookCode);
                            if (payCode is not null)
                            {
                                var CashVoucherItem = new TblFinTrnCashVoucherItem
                                {
                                    CashVoucherId = jvId,
                                    BranchCode = obj.BranchCode,
                                    Batch = obj.Batch,
                                    Batch2 = obj.Batch,
                                    Remarks = obj.Remarks,
                                    Payment = obj.Amount,
                                    DrAmount = isPayment ? 0 : (obj.Amount ?? 0),
                                    CrAmount = isPayment ? (obj.Amount ?? 0) : 0,
                                    FinAcCode = payCode.FinPayAcIntgrAC,
                                    Description = obj.Narration,
                                    SiteCode = obj.SiteCode

                                };
                                CashVoucherItemsList.Add(CashVoucherItem);
                            }
                            await _context.CashVoucherItems.AddRangeAsync(CashVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, jvId);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateCashVoucher Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion



    #region CreateCashVoucherApproval
    public class CreateCashVoucherApproval : UserIdentityDto, IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceApprovalDto Input { get; set; }
    }
    public class CreateCashVoucherApprovalQueryHandler : IRequestHandler<CreateCashVoucherApproval, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCashVoucherApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> Handle(CreateCashVoucherApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateCashVoucherApproval method start----");

                    var obj = await _context.CashVouchers.FirstOrDefaultAsync(e => e.Id == request.Input.Id);

                    if (await _context.CashVoucherApprovals.AnyAsync(e => e.CashVoucherId == request.Input.Id && e.LoginId == request.User.UserId && e.IsApproved))
                        return true;

                    TblFinTrnCashVoucherApproval approval = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        JvDate = DateTime.Now,
                        TranSource = request.Input.TranSource,
                        Trantype = request.Input.Trantype,
                        DocNum = obj.DocNum,
                        LoginId = request.User.UserId,
                        AppRemarks = request.Input.AppRemarks,
                        CashVoucherId = request.Input.Id,
                        IsApproved = true,
                    };

                    await _context.CashVoucherApprovals.AddAsync(approval);
                    await _context.SaveChangesAsync();

                    if (!obj.VoucherNumber.HasValue())
                    {
                        int creditSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            creditSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CvVoucherSeq = creditSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            creditSeq = invSeq.CvVoucherSeq + 1;
                            invSeq.CvVoucherSeq = creditSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        obj.VoucherNumber = creditSeq.ToString();
                        obj.SpVoucherNumber = string.Empty;
                        obj.Approved = true;
                        obj.ApprovedDate = DateTime.Now;
                        _context.CashVouchers.Update(obj);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateCashVoucherApproval Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return false;
                }
            }
        }
    }
    #endregion


    #region CreateCashVoucherPosting
    public class CreateCashVoucherPosting : UserIdentityDto, IRequest<short>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceSettlementDto Input { get; set; }
    }
    public class CreateCashVoucherPostingQueryHandler : IRequestHandler<CreateCashVoucherPosting, short>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCashVoucherPostingQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<short> Handle(CreateCashVoucherPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateCashVoucherPosting method start----");

                    var input = request.Input;
                    bool isPosted = input.PaymentType == "posting" ? true : false;
                    var postingDate = request.Input.CreatedOn ?? DateTime.Now;

                    var obj = await _context.CashVouchers.FirstOrDefaultAsync(e => e.Id == input.Id);

                    if (isPosted && await _context.CashVoucherStatements.AnyAsync(e => e.CashVoucherId == input.Id && e.LoginId == request.User.UserId && e.IsPosted))
                        return -1;
                    if (!isPosted && await _context.CashVoucherStatements.AnyAsync(e => e.CashVoucherId == input.Id && e.LoginId == request.User.UserId && e.IsVoid))
                        return -2;

                    if (!obj.VoucherNumber.HasValue())
                    {
                        int invoiceSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            invoiceSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CvVoucherSeq = invoiceSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeq = invSeq.CvVoucherSeq + 1;
                            invSeq.CvVoucherSeq = invoiceSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        obj.VoucherNumber = invoiceSeq.ToString();
                        obj.SpVoucherNumber = string.Empty;
                        _context.CashVouchers.Update(obj);
                        await _context.SaveChangesAsync();

                    }

                    TblFinTrnCashVoucherStatement cStatement = new()
                    {

                        JvDate = postingDate,
                        TranNumber = obj.VoucherNumber,
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
                        LoginId = request.User.UserId,
                        CashVoucherId = input.Id,
                        IsPosted = isPosted,
                        IsVoid = !isPosted
                    };
                    await _context.CashVoucherStatements.AddAsync(cStatement);

                    obj.Posted = isPosted;
                    obj.PostedDate = postingDate;
                    obj.Void = !isPosted;

                    _context.CashVouchers.Update(obj);
                    await _context.SaveChangesAsync();


                    #region Adding CV to JV

                    if (isPosted)
                    {

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

                        bool isPayment = obj.VoucherType.ToLower() == "payment";


                        #region ADding to Distribution

                        List<TblFinTrnDistribution> distributionsList = new();
                        foreach (var cItem in await _context.CashVoucherItems.Where(e => e.CashVoucherId == obj.Id).ToListAsync()) //distributionsList
                        {
                            distributionsList.Add(new()
                            {
                                //InvoiceId = input.Id,
                                FinAcCode = cItem.FinAcCode,
                                DrAmount = cItem.DrAmount,
                                CrAmount = cItem.CrAmount,
                                Source = "CV",
                                Gl = string.Empty,
                                Type = obj.VoucherType.ToLower(),
                                CreatedOn = DateTime.Now
                            });
                        }

                        var finPayCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.CBookCode);
                        distributionsList.Add(new()
                        {
                            // InvoiceId = input.Id,
                            FinAcCode = finPayCode.FinPayAcIntgrAC,
                            CrAmount = distributionsList.Sum(e => e.DrAmount),
                            DrAmount = distributionsList.Sum(e => e.CrAmount),
                            Source = "CV",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        });

                        await _context.FinDistributions.AddRangeAsync(distributionsList);

                        await _context.SaveChangesAsync();

                        #endregion


                        #region Inserting into AccountsLedger

                        var CashVoucherItemsList = await _context.CashVoucherItems.Where(e => e.CashVoucherId == obj.Id).ToListAsync();
                        List<TblFinTrnAccountsLedger> ledgerList = new();
                        foreach (var item in CashVoucherItemsList)
                        {
                            TblFinTrnAccountsLedger ledger = new()
                            {
                                MainAcCode = item.FinAcCode,
                                AcCode = item.FinAcCode,
                                AcDesc = item.Description,
                                Batch = item.Batch,
                                BranchCode = item.BranchCode,
                                DrAmount = item.DrAmount,
                                CrAmount = item.CrAmount,
                                IsApproved = true,
                                TransDate = postingDate,
                                PostedFlag = true,
                                PostDate = postingDate,
                                Jvnum = item.CashVoucherId.ToString(),
                                GlId = item.Id,
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "CV",
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

                        //var ledgerItem = ledgerList[0];
                        //if (ledgerItem is not null)
                        //{
                        //    var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.CBookCode);
                        //    TblFinTrnAccountsLedger ledger2 = new()
                        //    {
                        //        MainAcCode = payCode.FinPayAcIntgrAC,
                        //        AcCode = payCode.FinPayAcIntgrAC,
                        //        AcDesc = obj.Remarks,
                        //        Batch = obj.Batch,
                        //        BranchCode = obj.BranchCode,
                        //        DrAmount = isPayment ? ledgerList.Sum(e => e.DrAmount) : 0,
                        //        CrAmount = isPayment ? 0 : ledgerList.Sum(e => e.CrAmount),
                        //        IsApproved = true,
                        //        TransDate = DateTime.Now,
                        //        PostedFlag = true,
                        //        PostDate = DateTime.Now,
                        //        Jvnum = obj.Id.ToString(),
                        //        Narration = obj.Narration,
                        //        Remarks = obj.Remarks,
                        //        Remarks2 = string.Empty,
                        //        ReverseFlag = false,
                        //        VoidFlag = false,
                        //        Source = "CV",
                        //        ExRate = 0,
                        //    };
                        //    await _context.AccountsLedgers.AddAsync(ledger2);
                        //    await _context.SaveChangesAsync();
                        //}

                        #endregion
                    }

                    #endregion



                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateCashVoucherPosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion


    #region CvPrint

    public class GetCashVoucherPrint : IRequest<CvPrintDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCashVoucherPrintsHandler : IRequestHandler<GetCashVoucherPrint, CvPrintDto>
    {
        private readonly DMCContext _dmcContext;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCashVoucherPrintsHandler(CINDBOneContext context, IMapper mapper, DMCContext dmcContext)
        {
            _context = context;
            _mapper = mapper;
            _dmcContext = dmcContext;
        }
        public async Task<CvPrintDto> Handle(GetCashVoucherPrint request, CancellationToken cancellationToken)
        {
            CvPrintDto obj = new();
            var jv = await _context.CashVouchers.Include(e => e.SysCompany).Include(e => e.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (jv is not null)
            {
                var costAllocation = _context.CostAllocationSetups.AsNoTracking();
                bool isArab = request.User.Culture.IsArab();

                var mainAccount = _context.FinMainAccounts.AsNoTracking();
                var itemList = await _context.CashVoucherItems.AsNoTracking()
                    .Where(e => e.CashVoucherId == jv.Id)
                    .Select(obj1 => new TblFinTrnCashVoucherItemDto
                    {
                        Id = obj1.Id,
                        BranchName = obj1.SysCompanyBranch.BranchName,
                        BranchCode = obj1.BranchCode,
                        FinAcCode = obj1.FinAcCode,
                        Description = mainAccount.FirstOrDefault(e => e.FinAcCode == obj1.FinAcCode).FinAcDesc,
                        Remarks = obj1.Remarks,
                        Batch = obj1.Batch,
                        Batch2 = obj1.Batch2,
                        CostAllocation = obj1.CostAllocation,
                        CostSegCode = obj1.CostSegCode,
                        Payment = obj1.Payment,
                        DrAmount = obj1.DrAmount,
                        CrAmount = obj1.CrAmount,

                    }).ToListAsync();

                foreach (var item in itemList)
                {
                    var costObj = await costAllocation.FirstOrDefaultAsync(e => e.Id == item.CostAllocation);
                    if (costObj is not null)
                    {
                        var costType = costObj.CostType.ToLower();

                        var name = costType switch
                        {
                            "vendor" => isArab ? (await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == item.CostSegCode)).VendArbName
                                                 : (await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == item.CostSegCode)).VendName,

                            "customer" => isArab ? (await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == item.CostSegCode)).CustArbName
                                                 : (await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == item.CostSegCode)).CustName,

                            "department" => isArab ? (await _dmcContext.HRM_DEF_Departments.Select(e => new { e.DepartmentID, e.DepartmentName_AR }).FirstOrDefaultAsync(e => e.DepartmentID.ToString() == item.CostSegCode)).DepartmentName_AR
                                                   : (await _dmcContext.HRM_DEF_Departments.Select(e => new { e.DepartmentID, e.DepartmentName_EN }).FirstOrDefaultAsync(e => e.DepartmentID.ToString() == item.CostSegCode)).DepartmentName_EN,
                            _ => string.Empty
                        };


                        item.CostAllocationName = isArab ? costObj.CostName2 : costObj.CostName;
                        item.CostSegCodeName = name;
                    }
                }

                obj.CBookCode = jv.CBookCode;
                obj.Source = jv.Source;
                obj.VoucherType = jv.VoucherType;

                obj.VoucherNumber = !string.IsNullOrEmpty(jv.VoucherNumber) ? jv.VoucherNumber : jv.SpVoucherNumber;
                obj.CompanyName = jv.SysCompany.CompanyName;
                obj.Address = jv.SysCompany.CompanyAddress;
                obj.Logo = jv.SysCompany.LogoURL;
                obj.CDate = jv.JvDate;
                obj.BranchName = jv.SysCompanyBranch.BranchName;
                obj.Batch = jv.Batch;
                obj.User = jv.Narration;
                obj.DocNum = jv.DocNum;
                obj.Remarks = jv.Remarks;
                obj.JvDate = jv.PostedDate;
                obj.TotalPayment = itemList.Sum(e => e.Payment);
                obj.TotalCrPayment = itemList.Sum(e => e.CrAmount);
                obj.TotalDrPayment = itemList.Sum(e => e.DrAmount);
                obj.Amount = jv.Amount;
                obj.VoucherType = jv.VoucherType;

                ToWord toWord = new ToWord(jv.Amount is null ? 0 : (decimal)jv.Amount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                obj.TotalAmountText = toWord.ConvertToEnglish();

                ToWord toWord1 = new ToWord((obj.TotalDrPayment is null || obj.TotalDrPayment <= 0) ? (decimal)obj.TotalCrPayment : (decimal)obj.TotalDrPayment, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                obj.TotalPaymentText = toWord1.ConvertToEnglish();
                obj.ItemList = itemList;
            }
            return obj;
        }
    }

    #endregion
}
