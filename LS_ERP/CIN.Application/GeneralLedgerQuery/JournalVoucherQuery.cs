using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
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

    public class GetJournalVoucherList : IRequest<PaginatedList<FinTrnJournalVoucherListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetJournalVoucherListHandler : IRequestHandler<GetJournalVoucherList, PaginatedList<FinTrnJournalVoucherListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetJournalVoucherListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<FinTrnJournalVoucherListDto>> Handle(GetJournalVoucherList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            var custApproval = _context.JournalVoucherApprovals.AsNoTracking();

            var jvLineItems = _context.JournalVoucherItems.AsNoTracking().AsQueryable();

            var list = await _context.JournalVouchers.AsNoTracking()
              .Where(e =>
              e.BranchCode.Contains(search) || e.Batch.Contains(search) ||
               e.SysCompanyBranch.BranchName.Contains(search) ||
               e.DocNum.Contains(search) ||
               e.Remarks.Contains(search) ||
               e.VoucherNumber.Contains(search) || e.SpVoucherNumber.Contains(search)
              )
               .OrderBy(request.Input.OrderBy)
                .Select(d => new FinTrnJournalVoucherListDto
                {
                    Id = d.Id,
                    VoucherNumber = d.VoucherNumber == string.Empty ? d.SpVoucherNumber : d.VoucherNumber,
                    JvDate = d.JvDate,
                    BranchCode = d.BranchCode,
                    BranchName = d.SysCompanyBranch.BranchName,
                    Amount = d.Amount,
                    Source = d.Source,
                    DocNum = d.DocNum,
                    Remarks = d.Remarks,
                    Posted = d.Posted,
                    PostedDate = d.PostedDate,
                    IsDrCrAmtSame = jvLineItems.Where(e => e.JournalVoucherId == d.Id).Count() >= 2 && (jvLineItems.Where(e => e.JournalVoucherId == d.Id).Sum(e => e.DrAmount) == jvLineItems.Where(e => e.JournalVoucherId == d.Id).Sum(e => e.CrAmount) && d.Amount == jvLineItems.Where(e => e.JournalVoucherId == d.Id).Sum(e => e.CrAmount)),
                    HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == d.BranchCode && e.AppAuthJV),
                    ApprovedUser = d.Posted ? true : custApproval.Any(e => e.LoginId == request.User.UserId && e.JournalVoucherId == d.Id && e.IsApproved),
                    IsApproved = d.Posted ? true : custApproval.Where(e => e.JournalVoucherId == d.Id && e.IsApproved).Any(),
                    CanSettle = d.Posted ? true : brnApproval.Where(e => e.FinBranchCode == d.BranchCode && e.AppAuthJV).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= custApproval.Where(e => e.JournalVoucherId == d.Id && e.IsApproved).Count(),
                    IsSettled = d.Posted,
                    IsVoid = d.Posted ? false : d.Void
                })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region SingleItem

    public class GetJournalVoucher : IRequest<CreateJournalVoucherDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetJournalVouchersHandler : IRequestHandler<GetJournalVoucher, CreateJournalVoucherDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetJournalVouchersHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreateJournalVoucherDto> Handle(GetJournalVoucher request, CancellationToken cancellationToken)
        {
            CreateJournalVoucherDto obj = new();
            var jv = await _context.JournalVouchers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (jv is not null)
            {
                bool isArab = request.User.Culture.IsArab();
                var segmentTwoSetups = _context.SegmentTwoSetups.AsNoTracking();
                var itemList = await _context.JournalVoucherItems.AsNoTracking()
                    .Where(e => e.JournalVoucherId == jv.Id)
                    .Select(obj1 => new TblFinTrnJournalVoucherItemDto
                    {
                        Id = obj1.Id,
                        BranchName = obj1.SysCompanyBranch.BranchName,
                        BranchCode = obj1.BranchCode,
                        FinAcCode = obj1.FinAcCode,
                        Description = obj1.Description,
                        Remarks = obj1.Remarks,
                        Batch = obj1.Batch,
                        Batch2 = obj1.Batch2,
                        Batch2Name = isArab ? segmentTwoSetups.FirstOrDefault(e => e.Seg2Code == obj1.Batch2).Seg2Name2
                                                                       : segmentTwoSetups.FirstOrDefault(e => e.Seg2Code == obj1.Batch2).Seg2Name,
                        CostAllocation = obj1.CostAllocation,
                        CostSegCode = obj1.CostSegCode,
                        CrAmount = obj1.CrAmount,
                        DrAmount = obj1.DrAmount
                    }).ToListAsync();

                obj.JvDate = jv.JvDate;
                obj.CompanyId = jv.CompanyId;
                obj.BranchCode = jv.BranchCode;
                obj.Batch = jv.Batch;
                obj.Remarks = jv.Remarks;
                obj.Narration = jv.Narration;
                obj.Amount = jv.Amount ?? 0;
                obj.DocNum = jv.DocNum;
                obj.SiteCode = jv.SiteCode;                
                obj.ItemList = itemList;
            }
            return obj;
        }
    }

    #endregion


    #region CreateUpdateJournalVoucher

    public class CreateJournalVoucher : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateJournalVoucherDto Input { get; set; }
    }
    public class CreateJournalVoucherQueryHandler : IRequestHandler<CreateJournalVoucher, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateJournalVoucherQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateJournalVoucher request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateJournalVoucher method start----");

                    string spJournalVoucherNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                    //JournalVoucherNumber = await _context.TranJournalVouchers.CountAsync();
                    //JournalVoucherNumber += 1;
                    TblFinTrnJournalVoucher JV = new();
                    var obj = request.Input;

                    if (request.Input.Id > 0)
                    {
                        JV = await _context.JournalVouchers.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        spJournalVoucherNumber = JV.SpVoucherNumber;
                        JV.CompanyId = obj.CompanyId;
                        JV.BranchCode = obj.BranchCode;
                        JV.Batch = obj.Batch;
                        JV.Remarks = obj.Remarks;
                        JV.Narration = obj.Narration;
                        JV.JvDate = obj.JvDate;
                        JV.Amount = obj.Amount ?? 0;
                        JV.DocNum = obj.DocNum;
                        JV.SiteCode = obj.SiteCode;

                        var items = _context.JournalVoucherItems.Where(e => e.JournalVoucherId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.JournalVouchers.Update(JV);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        JV = new()
                        {
                            SpVoucherNumber = spJournalVoucherNumber,
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            Batch = obj.Batch,
                            Source = "GL",
                            Remarks = obj.Remarks,
                            Narration = obj.Narration,
                            JvDate = obj.JvDate,
                            Amount = obj.Amount ?? 0,
                            DocNum = obj.DocNum,
                            VoucherNumber = string.Empty,
                            CDate = DateTime.Now,
                            SiteCode = obj.SiteCode
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();
                    }
                    //var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.CompanyId);
                    //if (company is not null)
                    //{
                    //    company.JournalVoucherSeq++;
                    //    await _context.SaveChangesAsync();
                    //}

                    Log.Info("----Info CreateUpdateJournalVoucher method Exit----");

                    var jvId = JV.Id;
                    var JournalVoucherItems = request.Input.ItemList;
                    if (JournalVoucherItems.Count > 0)
                    {
                        List<TblFinTrnJournalVoucherItem> JournalVoucherItemsList = new();

                        foreach (var obj1 in JournalVoucherItems)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = obj1.BranchCode,
                                Batch = obj1.Batch,
                                Batch2 = obj1.Batch2,
                                CostAllocation = obj1.CostAllocation,
                                CostSegCode = obj1.CostSegCode,
                                Remarks = obj1.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = obj1.Description,
                                SiteCode = obj.SiteCode

                            };
                            JournalVoucherItemsList.Add(JournalVoucherItem);
                        }
                        if (JournalVoucherItemsList.Count > 0)
                        {
                            await _context.JournalVoucherItems.AddRangeAsync(JournalVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, jvId);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateJournalVoucher Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion


    #region CreateJournalVoucherApproval
    public class CreateJournalVoucherApproval : UserIdentityDto, IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceApprovalDto Input { get; set; }
    }
    public class CreateJournalVoucherApprovalQueryHandler : IRequestHandler<CreateJournalVoucherApproval, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateJournalVoucherApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> Handle(CreateJournalVoucherApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateJournalVoucherApproval method start----");

                    var obj = await _context.JournalVouchers.FirstOrDefaultAsync(e => e.Id == request.Input.Id);

                    if (await _context.JournalVoucherApprovals.AnyAsync(e => e.JournalVoucherId == request.Input.Id && e.LoginId == request.User.UserId && e.IsApproved))
                        return true;

                    TblFinTrnJournalVoucherApproval approval = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        JvDate = DateTime.Now,
                        TranSource = request.Input.TranSource,
                        Trantype = request.Input.Trantype,
                        DocNum = obj.DocNum,
                        LoginId = request.User.UserId,
                        AppRemarks = request.Input.AppRemarks,
                        JournalVoucherId = request.Input.Id,
                        IsApproved = true,
                    };

                    await _context.JournalVoucherApprovals.AddAsync(approval);
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
                                JvVoucherSeq = creditSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            creditSeq = invSeq.JvVoucherSeq + 1;
                            invSeq.JvVoucherSeq = creditSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        obj.VoucherNumber = creditSeq.ToString();
                        obj.SpVoucherNumber = string.Empty;
                        obj.Approved = true;
                        obj.ApprovedDate = DateTime.Now;

                        _context.JournalVouchers.Update(obj);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateJournalVoucherApproval Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return false;
                }
            }
        }
    }
    #endregion


    #region CreateJournalVoucherPosting
    public class CreateJournalVoucherPosting : UserIdentityDto, IRequest<short>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceSettlementDto Input { get; set; }
    }
    public class CreateJournalVoucherPostingQueryHandler : IRequestHandler<CreateJournalVoucherPosting, short>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateJournalVoucherPostingQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<short> Handle(CreateJournalVoucherPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateJournalVoucherPosting method start----");

                    var input = request.Input;
                    bool isPosted = input.PaymentType == "posting" ? true : false;
                    var postingDate = request.Input.CreatedOn ?? DateTime.Now;

                    var obj = await _context.JournalVouchers.FirstOrDefaultAsync(e => e.Id == input.Id);

                    if (isPosted && await _context.JournalVoucherStatements.AnyAsync(e => e.JournalVoucherId == input.Id && e.LoginId == request.User.UserId && e.IsPosted))
                        return -1;
                    if (!isPosted && await _context.JournalVoucherStatements.AnyAsync(e => e.JournalVoucherId == input.Id && e.LoginId == request.User.UserId && e.IsVoid))
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
                                JvVoucherSeq = invoiceSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeq = invSeq.JvVoucherSeq + 1;
                            invSeq.JvVoucherSeq = invoiceSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        obj.VoucherNumber = invoiceSeq.ToString();
                        obj.SpVoucherNumber = string.Empty;
                        _context.JournalVouchers.Update(obj);

                        await _context.SaveChangesAsync();

                    }

                    TblFinTrnJournalVoucherStatement cStatement = new()
                    {

                        JvDate = postingDate,
                        TranNumber = obj.VoucherNumber,
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
                        LoginId = request.User.UserId,
                        JournalVoucherId = input.Id,
                        IsPosted = isPosted,
                        IsVoid = !isPosted
                    };
                    await _context.JournalVoucherStatements.AddAsync(cStatement);

                    obj.Posted = isPosted;
                    obj.PostedDate = postingDate;
                    obj.Void = !isPosted;

                    _context.JournalVouchers.Update(obj);
                    await _context.SaveChangesAsync();


                    if (isPosted)
                    {

                        var JournalVoucherItemsList = await _context.JournalVoucherItems.Where(e => e.JournalVoucherId == obj.Id).ToListAsync();
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
                                TransDate = postingDate,
                                PostedFlag = true,
                                PostDate = postingDate,
                                Jvnum = item.JournalVoucherId.ToString(),
                                GlId = item.Id,
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "JV",
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

                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateJournalVoucherPosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion



    #region CreateJournalVoucherCopy
    public class CreateJournalVoucherCopy : UserIdentityDto, IRequest<short>
    {
        public UserIdentityDto User { get; set; }
        public JournalVoucherCopyDto Input { get; set; }
    }
    public class CreateJournalVoucherCopyQueryHandler : IRequestHandler<CreateJournalVoucherCopy, short>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateJournalVoucherCopyQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<short> Handle(CreateJournalVoucherCopy request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateJournalVoucherCopy method start----");

                    var input = request.Input;
                    //bool isPosted = input.PaymentType == "posting" ? true : false;

                    string spJournalVoucherNumber = $"S{new Random().Next(99, 9999999).ToString()}";
                    var obj = await _context.JournalVouchers.FirstOrDefaultAsync(e => e.Id == input.Id);
                    var jvItems = await _context.JournalVoucherItems.Where(e => e.JournalVoucherId == obj.Id).ToListAsync();

                    //int invoiceSeq = 0;
                    //var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                    //if (invSeq is null)
                    //{
                    //    invoiceSeq = 1;
                    //    TblSequenceNumberSetting setting = new()
                    //    {
                    //        JvVoucherSeq = invoiceSeq
                    //    };
                    //    await _context.Sequences.AddAsync(setting);
                    //}
                    //else
                    //{
                    //    invoiceSeq = invSeq.JvVoucherSeq + 1;
                    //    invSeq.JvVoucherSeq = invoiceSeq;
                    //    _context.Sequences.Update(invSeq);
                    //}

                    //obj.VoucherNumber = invoiceSeq.ToString();
                    //obj.SpVoucherNumber = string.Empty;
                    //_context.JournalVouchers.Update(obj);

                    //await _context.SaveChangesAsync();


                    TblFinTrnJournalVoucher JV = new()
                    {
                        SpVoucherNumber = spJournalVoucherNumber,
                        VoucherNumber = string.Empty,
                        CompanyId = obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        Batch = obj.Batch,
                        Source = "GL",
                        Remarks = obj.Remarks,
                        Narration = obj.Narration,
                        JvDate = DateTime.Now,
                        Amount = obj.Amount ?? 0,
                        DocNum = obj.DocNum,
                        CDate = DateTime.Now
                    };

                    await _context.JournalVouchers.AddAsync(JV);
                    await _context.SaveChangesAsync();


                    var jvId = JV.Id;
                    var JournalVoucherItems = jvItems;
                    if (JournalVoucherItems.Count > 0)
                    {
                        List<TblFinTrnJournalVoucherItem> JournalVoucherItemsList = new();

                        foreach (var obj1 in JournalVoucherItems)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = obj1.BranchCode,
                                Batch = obj1.Batch,
                                Batch2 = obj1.Batch2,
                                Remarks = obj1.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = obj1.Description,

                            };
                            JournalVoucherItemsList.Add(JournalVoucherItem);
                        }
                        if (JournalVoucherItemsList.Count > 0)
                        {
                            await _context.JournalVoucherItems.AddRangeAsync(JournalVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }
                    }



                    //var approval = await _context.JournalVoucherApprovals.FirstOrDefaultAsync(e => e.JournalVoucherId == obj.Id);

                    //TblFinTrnJournalVoucherApproval jvApproval = new()
                    //{
                    //    CompanyId = (int)approval.CompanyId,
                    //    BranchCode = approval.BranchCode,
                    //    JvDate = DateTime.Now,
                    //    TranSource = approval.TranSource,
                    //    Trantype = approval.Trantype,
                    //    DocNum = approval.DocNum,
                    //    LoginId = approval.LoginId,
                    //    AppRemarks = approval.AppRemarks,
                    //    JournalVoucherId = jvId,
                    //    IsApproved = false,
                    //};

                    //await _context.JournalVoucherApprovals.AddAsync(jvApproval);
                    //await _context.SaveChangesAsync();


                    //var cStatement = await _context.JournalVoucherStatements.FirstOrDefaultAsync(e => e.JournalVoucherId == obj.Id);
                    //TblFinTrnJournalVoucherStatement jvStatement = new()
                    //{

                    //    JvDate = DateTime.Now,
                    //    TranNumber = cStatement.TranNumber,
                    //    Remarks1 = cStatement.Remarks1,
                    //    Remarks2 = cStatement.Remarks2,
                    //    LoginId = cStatement.LoginId,
                    //    JournalVoucherId = jvId,
                    //    IsPosted = false,
                    //    IsVoid = false
                    //};
                    //await _context.JournalVoucherStatements.AddAsync(cStatement);

                    //obj.Posted = isPosted;
                    //obj.PostedDate = DateTime.Now;
                    //obj.Void = !isPosted;
                    // _context.JournalVouchers.Update(obj);
                    // await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return 1;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateJournalVoucherCopy Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion




    #region JvPrint

    public class GetJournalVoucherPrint : IRequest<JvPrintDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetJournalVoucherPrintsHandler : IRequestHandler<GetJournalVoucherPrint, JvPrintDto>
    {
        private readonly DMCContext _dmcContext;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetJournalVoucherPrintsHandler(CINDBOneContext context, IMapper mapper, DMCContext dmcContext)
        {
            _context = context;
            _mapper = mapper;
            _dmcContext = dmcContext;
        }
        public async Task<JvPrintDto> Handle(GetJournalVoucherPrint request, CancellationToken cancellationToken)
        {
            JvPrintDto obj = new();
            var jv = await _context.JournalVouchers.Include(e => e.SysCompany).Include(e => e.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (jv is not null)
            {

                var segmentTwoSetups = _context.SegmentTwoSetups.AsNoTracking();
                var costAllocation = _context.CostAllocationSetups.AsNoTracking();
                bool isArab = request.User.Culture.IsArab();
                var siteCods = _context.OprSites.AsNoTracking();
                var mainAccounts = _context.FinMainAccounts.AsNoTracking();
                var itemList = await _context.JournalVoucherItems.AsNoTracking()
                    .Where(e => e.JournalVoucherId == jv.Id)
                    .Select(obj1 => new TblFinTrnJournalVoucherItemDto
                    {
                        Id = obj1.Id,
                        BranchName = obj1.SysCompanyBranch.BranchName,
                        BranchCode = obj1.BranchCode,
                        FinAcCode = obj1.FinAcCode,
                        Description = mainAccounts.FirstOrDefault(e => e.FinAcCode == obj1.FinAcCode).FinAcDesc,
                        Remarks = obj1.Remarks,
                        Batch = obj1.Batch,
                        Batch2 = isArab ? segmentTwoSetups.FirstOrDefault(e => e.Seg2Code == obj1.Batch2).Seg2Name2
                         : segmentTwoSetups.FirstOrDefault(e=>e.Seg2Code == obj1.Batch2).Seg2Name,
                        CostAllocation = obj1.CostAllocation,
                        CostSegCode = obj1.CostSegCode,
                        CrAmount = obj1.CrAmount,
                        SiteCode = obj1.SiteCode,
                        DrAmount = obj1.DrAmount
                    }).ToListAsync();


                foreach (var item in itemList)//.Where(e=>e.CostAllocation == 5)
                {
                    var costObj = await costAllocation.FirstOrDefaultAsync(e => e.Id == item.CostAllocation);
                    try
                    {
                        if (costObj is not null)
                        {
                            var costType = costObj.CostType.ToLower();

                            var name = costType switch
                            {
                                "vendor" => isArab ? ((await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == item.CostSegCode))?.VendArbName ?? String.Empty)
                                                     : ((await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == item.CostSegCode))?.VendName ?? String.Empty),

                                "customer" => isArab ? ((await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == item.CostSegCode))?.CustArbName ?? String.Empty)
                                                     : ((await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == item.CostSegCode))?.CustName ?? String.Empty),

                                "department" => isArab ? ((await _dmcContext.HRM_DEF_Departments.Select(e => new { e.DepartmentID, e.DepartmentName_AR }).FirstOrDefaultAsync(e => e.DepartmentID.ToString() == item.CostSegCode))?.DepartmentName_AR ?? String.Empty)
                                                     : ((await _dmcContext.HRM_DEF_Departments.Select(e => new { e.DepartmentID, e.DepartmentName_EN }).FirstOrDefaultAsync(e => e.DepartmentID.ToString() == item.CostSegCode))?.DepartmentName_EN ?? String.Empty),

                                "employee" => isArab ? ((await _dmcContext.HRM_TRAN_Employees.Select(e => new { e.EmployeeID, e.EmployeeName_AR }).FirstOrDefaultAsync(e => e.EmployeeID.ToString() == item.CostSegCode))?.EmployeeName_AR ?? String.Empty)
                                                    : ((await _dmcContext.HRM_TRAN_Employees.Select(e => new { e.EmployeeID, e.EmployeeName }).FirstOrDefaultAsync(e => e.EmployeeID.ToString() == item.CostSegCode))?.EmployeeName ?? String.Empty),
                                _ => string.Empty
                            };

                            var site = siteCods.FirstOrDefault(e => e.SiteCode == item.SiteCode);
                            item.CostAllocationName = isArab ? costObj.CostName2 : costObj.CostName;
                            item.CostSegCodeName = name + (site is not null ? (" - " + (isArab ? site.SiteArbName : site.SiteName)) : String.Empty);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

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
                obj.Amount = jv.Amount;

                obj.TotalDrAmount = itemList.Sum(e => e.DrAmount);
                obj.TotalCrAmount = itemList.Sum(e => e.CrAmount);

                ToWord toWord = new ToWord(jv.Amount is null ? 0 : (decimal)jv.Amount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                obj.TotalAmountText = toWord.ConvertToEnglish();
                obj.ItemList = itemList;
            }
            return obj;
        }
    }

    #endregion

}
