using AutoMapper;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.GeneralLedgerQuery
{

    #region BRS Methods


    #region GetPagedList

    public class GetBRSVoucherList : IRequest<PaginatedList<TblFinTrnBankReconciliationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetBRSVoucherListHandler : IRequestHandler<GetBRSVoucherList, PaginatedList<TblFinTrnBankReconciliationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBRSVoucherListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnBankReconciliationDto>> Handle(GetBRSVoucherList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.BankReconciliations.AsNoTracking()
              .Where(e =>
              e.BranchCode.Contains(search) ||
               e.SysCompanyBranch.BranchName.Contains(search) ||
               e.DocNum.Contains(search) ||
               e.Remarks.Contains(search) ||
               e.VoucherNumber.Contains(search) || e.SpVoucherNumber.Contains(search)
              )
               .OrderBy(request.Input.OrderBy)
                .Select(d => new TblFinTrnBankReconciliationDto
                {
                    Id = d.Id,
                    VoucherNumber = d.VoucherNumber == string.Empty ? d.SpVoucherNumber : d.VoucherNumber,
                    Date = d.Date,
                    BranchCode = d.BranchCode,
                    BranchName = d.SysCompanyBranch.BranchName,
                    Amount = d.Amount,
                    Source = d.Source,
                    DocNum = d.DocNum,
                    Remarks = d.Remarks,
                    Posted = d.Posted,
                    Void = d.Void,
                    PostedDate = d.PostedDate,
                    Approved = d.Approved,
                    PayCode = d.PayCode,
                })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region SingleItem

    public class GetBankReconciliation : IRequest<CreateBankReconciliationDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBankReconciliationsHandler : IRequestHandler<GetBankReconciliation, CreateBankReconciliationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankReconciliationsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreateBankReconciliationDto> Handle(GetBankReconciliation request, CancellationToken cancellationToken)
        {
            CreateBankReconciliationDto obj = new();
            var jv = await _context.BankReconciliations.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (jv is not null)
            {
                bool isArab = request.User.Culture.IsArab();
                var segmentTwoSetups = _context.SegmentTwoSetups.AsNoTracking();
                var itemList = await _context.BankReconciliationItems.AsNoTracking()
                    .Where(e => e.BankRecId == jv.Id)
                    .Select(obj1 => new TblFinTrnBankReconciliationItemDto
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

                obj.Date = jv.Date;
                obj.BranchCode = jv.BranchCode;
                obj.Remarks = jv.Remarks;
                obj.Narration = jv.Narration;
                obj.Amount = jv.Amount ?? 0;
                obj.DocNum = jv.DocNum;
                obj.ItemList = itemList;
            }
            return obj;
        }
    }

    #endregion


    #region GetAllBankReconcialationStatement

    public class GetAllBankReconcialationStatement : IRequest<BankReconciliationStatementListDto>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllBankReconcialationStatementHandler : IRequestHandler<GetAllBankReconcialationStatement, BankReconciliationStatementListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllBankReconcialationStatementHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BankReconciliationStatementListDto> Handle(GetAllBankReconcialationStatement request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now;

            var opmPaymentList = _context.OpmCustPaymentHeaders.Where(e => e.PayType == ((short)PayCodeTypeEnum.Bank).ToString() &&
              EF.Functions.DateFromParts(e.Checkdate.Value.Year, e.Checkdate.Value.Month, e.Checkdate.Value.Day) < currentDate);
            var pendingList = opmPaymentList.Where(e => !e.IsPosted);

            if (await pendingList.AnyAsync())
            {
                //  return new() { HasPending = true, Vouchers = string.Join(", ", pendingList.Select(e => e.PaymentNumber).ToArray()) };
            }

            return new() { };

            //var bankReconciliations = await opmPaymentList.Select(e => new BankReconciliationStatementDto
            //{
            //    Checkdate = e.Checkdate,
            //    CheckNumber = e.CheckNumber,
            //    Amount = e.Amount,
            //    PostedDate = e.PostedDate,
            //    PayCode = e.PayCode,

            //}).ToListAsync();

            ////if (request.SiteCode.HasValue())
            ////    custWallets = custWallets.Where(e => e.SiteCode == request.SiteCode);

            //return new() { List = bankReconciliations };

        }
    }

    #endregion


    #region CreateUpdateBankReconciliation

    public class CreateUpdateBankReconciliation : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateBankReconciliationDto Input { get; set; }
    }
    public class CreateUpdateBankReconciliationQueryHandler : IRequestHandler<CreateUpdateBankReconciliation, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBankReconciliationQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateBankReconciliation request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateBankReconciliation method start----");

                    string spBRSVoucherNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                    //JournalVoucherNumber = await _context.TranJournalVouchers.CountAsync();
                    //JournalVoucherNumber += 1;
                    TblFinTrnBankReconciliation BRS = new();
                    var obj = request.Input;

                    if (request.Input.Id > 0)
                    {
                        BRS = await _context.BankReconciliations.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        spBRSVoucherNumber = BRS.SpVoucherNumber;
                        BRS.BranchCode = obj.BranchCode;
                        BRS.PayCode = obj.PayCode;
                        BRS.Remarks = obj.Remarks;
                        BRS.Narration = obj.Narration;
                        BRS.Date = obj.Date;
                        BRS.Amount = obj.Amount.DecValue();
                        BRS.DocNum = obj.DocNum;

                        var items = _context.JournalVoucherItems.Where(e => e.JournalVoucherId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.BankReconciliations.Update(BRS);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        BRS = new()
                        {
                            SpVoucherNumber = spBRSVoucherNumber,
                            BranchCode = obj.BranchCode,
                            PayCode = obj.PayCode,
                            Source = "GL",
                            Remarks = obj.Remarks,
                            Narration = obj.Narration,
                            Date = obj.Date,
                            Amount = obj.Amount.DecValue(),
                            DocNum = obj.DocNum,
                            VoucherNumber = string.Empty,
                            CDate = DateTime.Now,
                        };

                        await _context.BankReconciliations.AddAsync(BRS);
                        await _context.SaveChangesAsync();
                    }
                    //var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.CompanyId);
                    //if (company is not null)
                    //{
                    //    company.BankReconciliationseq++;
                    //    await _context.SaveChangesAsync();
                    //}

                    Log.Info("----Info CreateUpdateBankReconciliation method Exit----");

                    var jvId = BRS.Id;
                    var BRSVoucherItems = request.Input.ItemList;
                    if (BRSVoucherItems.Count > 0)
                    {
                        List<TblFinTrnBankReconciliationItem> BRSVoucherItemsList = new();

                        foreach (var obj1 in BRSVoucherItems)
                        {
                            var JournalVoucherItem = new TblFinTrnBankReconciliationItem
                            {
                                BankRecId = jvId,
                                BranchCode = obj1.BranchCode,
                                Batch = obj1.Batch,
                                Batch2 = obj1.Batch2,
                                CostAllocation = obj1.CostAllocation,
                                CostSegCode = obj1.CostSegCode,
                                Remarks = obj1.Remarks,
                                CrAmount = obj1.CrAmount.DecValue(),
                                DrAmount = obj1.DrAmount.DecValue(),
                                FinAcCode = obj1.FinAcCode,
                                Description = obj1.Description,

                            };
                            BRSVoucherItemsList.Add(JournalVoucherItem);
                        }
                        if (BRSVoucherItemsList.Count > 0)
                        {
                            await _context.BankReconciliationItems.AddRangeAsync(BRSVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, jvId);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateBankReconciliation Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region CreateBankReconciliationVoucherApproval
    public class CreateBankReconciliationVoucherApproval : UserIdentityDto, IRequest<BankReconciliationStatementListDto>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceApprovalDto Input { get; set; }
    }
    public class CreateBankReconciliationVoucherApprovalQueryHandler : IRequestHandler<CreateBankReconciliationVoucherApproval, BankReconciliationStatementListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateBankReconciliationVoucherApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BankReconciliationStatementListDto> Handle(CreateBankReconciliationVoucherApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateBankReconciliationVoucherApproval method start----");

                    var obj = await _context.BankReconciliations.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                    if (obj.Approved)
                        return new() { IsSuccess = true };

                    var brsDate = obj.Date;

                    #region Check From Customer Payments

                    var opmCustPaymentList = _context.OpmCustPaymentHeaders.Where(e => e.PayType == ((short)PayCodeTypeEnum.Bank).ToString() &&
                            EF.Functions.DateFromParts(e.Checkdate.Value.Year, e.Checkdate.Value.Month, e.Checkdate.Value.Day) < brsDate);

                    var pendingList = opmCustPaymentList.Where(e => !e.IsPosted);
                    if (await pendingList.AnyAsync(e => e.Id > 0))
                    {
                        return new() { Status = 1, Vouchers = string.Join(", ", pendingList.Select(e => e.PaymentNumber).ToArray()) };
                    }



                    var pendingPDCList = _context.OpmCustPaymentHeaders.Where(e => e.IsPdcCleared != null && e.IsPdcCleared == false);
                    if (await pendingPDCList.AnyAsync(e => e.Id > 0))
                    {
                        return new() { Status = 4, Vouchers = string.Join(", ", pendingPDCList.Select(e => e.PaymentNumber).ToArray()) };
                    }


                    #endregion

                    #region Check From Vendor Payments

                    var opmVendPaymentList = _context.OpmVendPaymentHeaders.Where(e => e.PayType == ((short)PayCodeTypeEnum.Bank).ToString() &&
                            EF.Functions.DateFromParts(e.Checkdate.Value.Year, e.Checkdate.Value.Month, e.Checkdate.Value.Day) < brsDate);
                    var pendingVendList = opmVendPaymentList.Where(e => !e.IsPosted);

                    if (await pendingVendList.AnyAsync(e => e.Id > 0))
                    {
                        return new() { Status = 2, Vouchers = string.Join(", ", pendingVendList.Select(e => e.PaymentNumber).ToArray()) };
                    }

                    var pendingVendPDCList = _context.OpmVendPaymentHeaders.Where(e => e.IsPdcCleared != null && e.IsPdcCleared == false);
                    if (await pendingVendPDCList.AnyAsync(e => e.Id > 0))
                    {
                        return new() { Status = 5, Vouchers = string.Join(", ", pendingVendPDCList.Select(e => e.PaymentNumber).ToArray()) };
                    }


                    #endregion


                    #region Check From Bank Voucher

                    var opmAnkPaymentList = _context.BankVouchers.Where(e => EF.Functions.DateFromParts(e.ChequeDate.Value.Year, e.ChequeDate.Value.Month, e.ChequeDate.Value.Day) < brsDate);
                    var pendingBankList = opmAnkPaymentList.Where(e => !e.Posted && e.Void == false);

                    if (await pendingBankList.AnyAsync(e => e.Id > 0))
                    {
                        return new() { Status = 3, Vouchers = string.Join(", ", pendingBankList.Select(e => !string.IsNullOrEmpty(e.VoucherNumber) ? e.VoucherNumber : e.SpVoucherNumber).ToArray()) };
                    }

                    #endregion


                    if (!obj.VoucherNumber.HasValue())
                    {
                        int sequence = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            sequence = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                BrsVoucherSeq = sequence
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            sequence = invSeq.BrsVoucherSeq + 1;
                            invSeq.BrsVoucherSeq = sequence;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        obj.VoucherNumber = sequence.ToString();
                        obj.SpVoucherNumber = string.Empty;
                        obj.Approved = true;
                        obj.ApprovedDate = DateTime.Now;

                        _context.BankReconciliations.Update(obj);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return new() { IsSuccess = true };

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateBankReconciliationVoucherApproval Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new();
                    //return new() { Vouchers = ex.Message + " " + ex.StackTrace};
                }
            }
        }
    }
    #endregion


    #region CreateBankReconciliationVoucherPosting
    public class CreateBankReconciliationVoucherPosting : UserIdentityDto, IRequest<short>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceSettlementDto Input { get; set; }
    }
    public class CreateBankReconciliationVoucherPostingQueryHandler : IRequestHandler<CreateBankReconciliationVoucherPosting, short>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateBankReconciliationVoucherPostingQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<short> Handle(CreateBankReconciliationVoucherPosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateBankReconciliationVoucherPosting method start----");

                    var input = request.Input;
                    bool isPosted = input.PaymentType == "posting" ? true : false;

                    var obj = await _context.BankReconciliations.FirstOrDefaultAsync(e => e.Id == input.Id);
                    if (obj.Posted || obj.Void)
                        return 1;

                    if (!obj.VoucherNumber.HasValue())
                    {
                        int sequence = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            sequence = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                BrsVoucherSeq = sequence
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            sequence = invSeq.BrsVoucherSeq + 1;
                            invSeq.BrsVoucherSeq = sequence;
                            _context.Sequences.Update(invSeq);
                        }
                        obj.VoucherNumber = sequence.ToString();
                        obj.SpVoucherNumber = string.Empty;
                    }

                    obj.Posted = isPosted;
                    obj.PostedDate = DateTime.Now;
                    obj.Void = !isPosted;

                    _context.BankReconciliations.Update(obj);
                    await _context.SaveChangesAsync();


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


                        #region Adding to Distribution

                        List<TblFinTrnDistribution> distributionsList = new();
                        var finPayCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.PayCode);

                        foreach (var cItem in await _context.BankReconciliationItems.Where(e => e.BankRecId == obj.Id).ToListAsync()) //distributionsList
                        {
                            var mainAccount = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == cItem.FinAcCode);
                            distributionsList.Add(new()
                            {
                                //InvoiceId = input.Id,
                                FinAcCode = cItem.FinAcCode,
                                DrAmount = cItem.DrAmount,
                                CrAmount = cItem.CrAmount,
                                Source = "GL",
                                Gl = string.Empty,
                                Type = mainAccount.Fintype,
                                CreatedOn = DateTime.Now
                            });

                            distributionsList.Add(new()
                            {
                                // InvoiceId = input.Id,
                                FinAcCode = finPayCode.FinPayAcIntgrAC,
                                CrAmount = cItem.DrAmount,
                                DrAmount = cItem.CrAmount,
                                Source = "GL",
                                Gl = string.Empty,
                                Type = "paycode",
                                CreatedOn = DateTime.Now
                            });
                        }

                        await _context.FinDistributions.AddRangeAsync(distributionsList);
                        await _context.SaveChangesAsync();


                        //Storing in  JournalVoucher tables
                        var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == obj.BranchCode);
                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = companyBranch.CompanyId,
                            BranchCode = obj.BranchCode,
                            Batch = string.Empty,
                            //Source = "GL",
                            Source = "GL",
                            Remarks = obj.Remarks,
                            Narration = obj.Narration,
                            JvDate = DateTime.Now,
                            Amount = obj.Amount.DecValue(),
                            DocNum = obj.VoucherNumber.ToString(),
                            CDate = DateTime.Now,
                            Approved = true,
                            ApprovedDate = DateTime.Now,
                            Posted = true,
                            Void = false,
                            PostedDate = DateTime.Now
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;


                        List<TblFinTrnJournalVoucherItem> JournalVoucherItemsList = new();
                        //   var costallocations = _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType });
                        foreach (var obj1 in distributionsList)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = obj.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = obj.Remarks,
                                CrAmount = obj1.CrAmount.DecValue(),
                                DrAmount = obj1.DrAmount.DecValue(),
                                FinAcCode = obj1.FinAcCode,
                                Description = obj.Remarks ?? obj.Narration,
                                // CostAllocation = costallocations.FirstOrDefaultAsync(e=>e.CostType == o),
                                // CostSegCode = vendor.VendCode,
                                SiteCode = String.Empty
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
                            Remarks1 = obj.Remarks,
                            Remarks2 = string.Empty,
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
                                CrAmount = item.CrAmount.DecValue(),
                                DrAmount = item.DrAmount.DecValue(),
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
                                Source = "GL",
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

                        #endregion

                    }

                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateBankReconciliationVoucherPosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion

    #endregion

    #region PDC Methods


    #region GetPDCCustVendPaymentList

    public class GetPDCCustVendPaymentList : IRequest<PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetOpmVendorPaymentListHandler : IRequestHandler<GetPDCCustVendPaymentList, PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmVendorPaymentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnCustomerPaymentDto>> Handle(GetPDCCustVendPaymentList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var statusId = request.Input.StatusId ?? 0;
            IQueryable<TblFinTrnCustomerPaymentDto> list = null;

            if (statusId == 0)
            {
                list = _context.OpmVendPaymentHeaders
                    .Include(e => e.SndVendorMaster)
                    .Include(e => e.SysCompanyBranch)
                   .Select(e => new TblFinTrnCustomerPaymentDto
                   {
                       Id = e.Id,
                       VoucherNumber = e.PaymentNumber,
                       CustName = request.User.Culture != "ar" ? e.SndVendorMaster.VendName : e.SndVendorMaster.VendArbName,
                       BranchName = e.SysCompanyBranch.BranchName,
                       TranDate = e.TranDate,
                       DocNum = e.DocNum,
                       CheckDate = e.Checkdate,
                       CustCode = e.VendCode,
                       CheckNumber = e.CheckNumber,
                       Amount = e.Amount,
                       IsPaid = e.IsPaid,
                       IsPosted = e.IsPosted,
                       IsPdcCleared = e.IsPdcCleared,
                       HasPdcClearance = e.HasPdcClearance,
                       Remarks = e.Remarks
                   });
            }
            else
            {
                list = _context.OpmCustPaymentHeaders
                     .Include(e => e.SndCustomerMaster)
                    .Include(e => e.SysCompanyBranch)
                   .Select(e => new TblFinTrnCustomerPaymentDto
                   {
                       Id = e.Id,
                       VoucherNumber = e.PaymentNumber,
                       CustName = request.User.Culture != "ar" ? e.SndCustomerMaster.CustName : e.SndCustomerMaster.CustArbName,
                       BranchName = e.SysCompanyBranch.BranchName,
                       TranDate = e.TranDate,
                       DocNum = e.DocNum,
                       CheckDate = e.Checkdate,
                       CustCode = e.CustCode,
                       CheckNumber = e.CheckNumber,
                       Amount = e.Amount,
                       IsPaid = e.IsPaid,
                       IsPosted = e.IsPosted,
                       HasPdcClearance = e.HasPdcClearance,
                       IsPdcCleared = e.IsPdcCleared,
                       Remarks = e.Remarks
                   });

            }

            var pdcType = request.Input.Approval;

            if (pdcType.HasValue())
            {
                return await list.Where(e => e.IsPdcCleared == true && (e.VoucherNumber.ToString().Contains(search) || EF.Functions.Like(e.CustName, "%" + search + "%")))
                .OrderByDescending(e => e.Id)
                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            }

            return await list.Where(e => e.HasPdcClearance == true && e.IsPdcCleared == false && e.IsPosted == true && (e.VoucherNumber.ToString().Contains(search) || EF.Functions.Like(e.CustName, "%" + search + "%")))
                 .OrderByDescending(e => e.Id)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

        }
    }

    #endregion


    #region GetPDCCustVendPaymentItem

    public class GetPDCCustVendPaymentItem : IRequest<TblFinTrnCustomerPaymentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class GetPDCVendorPaymentItemHandler : IRequestHandler<GetPDCCustVendPaymentItem, TblFinTrnCustomerPaymentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPDCVendorPaymentItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnCustomerPaymentDto> Handle(GetPDCCustVendPaymentItem request, CancellationToken cancellationToken)
        {
            var type = request.Type;

            if (type == "cust")
            {
                return await _context.OpmCustPaymentHeaders
                 .Include(e => e.SysCompanyBranch)
              .Where(e => e.Id == request.Id)
              .Select(e => new TblFinTrnCustomerPaymentDto
              {
                  Id = e.Id,
                  PayCode = e.PayCode,
                  PayType = e.PayType,
                  BranchName = e.SysCompanyBranch.BranchName,
                  CheckNumber = e.CheckNumber,
                  CheckDate = e.Checkdate,
                  CustCode = e.CustCode,
                  BranchCode = e.BranchCode,
                  Remarks = e.Remarks,
              }).FirstOrDefaultAsync(cancellationToken);
            }

            return await _context.OpmVendPaymentHeaders
                      .Include(e => e.SysCompanyBranch)
                   .Where(e => e.Id == request.Id)
                   .Select(e => new TblFinTrnCustomerPaymentDto
                   {
                       Id = e.Id,
                       PayCode = e.PayCode,
                       PayType = e.PayType,
                       BranchName = e.SysCompanyBranch.BranchName,
                       CheckNumber = e.CheckNumber,
                       CheckDate = e.Checkdate,
                       CustCode = e.VendCode,
                       BranchCode = e.BranchCode,
                       Remarks = e.Remarks,
                   }).FirstOrDefaultAsync(cancellationToken);

        }

    }

    #endregion




    #region ChangeCheckDate
    public class ChangeCheckDate : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnCustomerPaymentDto Input { get; set; }
    }

    public class ChangeCheckDateHandler : IRequestHandler<ChangeCheckDate, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ChangeCheckDateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(ChangeCheckDate request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info ChangeCheckDate method start----");

                var obj = request.Input;

                if (obj.PayType == "cust")
                {
                    var cvObj = await _context.OpmCustPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    cvObj.PayCode = obj.PayCode;
                    cvObj.BranchCode = obj.BranchCode;
                    cvObj.Checkdate = obj.CheckDate;
                    cvObj.CheckNumber = obj.CheckNumber;
                    cvObj.Remarks += obj.Remarks;

                    _context.OpmCustPaymentHeaders.Update(cvObj);
                    await _context.SaveChangesAsync();
                    return ApiMessageInfo.Status(1);
                }
                else
                {
                    var cvObj = await _context.OpmVendPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    cvObj.PayCode = obj.PayCode;
                    cvObj.BranchCode = obj.BranchCode;
                    cvObj.Checkdate = obj.CheckDate;
                    cvObj.CheckNumber = obj.CheckNumber;
                    cvObj.Remarks += obj.Remarks;
                    _context.OpmVendPaymentHeaders.Update(cvObj);
                    await _context.SaveChangesAsync();

                    return ApiMessageInfo.Status(1);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in ChangeCheckDate Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }

    }

    #endregion


    #region PostingPDC

    public class PostingPDC : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnCustomerPaymentDto Input { get; set; }
    }

    public class PostingPDCHandler : IRequestHandler<PostingPDC, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public PostingPDCHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(PostingPDC request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info PostingPDC method start----");

                    var obj = request.Input;

                    if (obj.PayType == "cust")
                    {
                        var header = await _context.OpmCustPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);

                        return ApiMessageInfo.Status(1, header.Id);
                    }
                    else
                    {

                        var header = await _context.OpmVendPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);
                        var payCode = await _context.FinAccountlPaycodes.Include(e => e.SysCompanyBranch).FirstOrDefaultAsync(e => e.FinPayCode == header.PayCode);

                        //Money will come into Bank PAYCODE
                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = payCode.FinPayAcIntgrAC,
                            DrAmount = 0,
                            CrAmount = header.Amount.DecValue(),
                            Source = "AP",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        //Money will Go out from PDC PAYCODE
                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = payCode.FinPayPDCClearAC,
                            DrAmount = header.Amount.DecValue(),
                            CrAmount = 0,
                            Source = "AP",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        await _context.SaveChangesAsync();

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
                            CompanyId = payCode.SysCompanyBranch.CompanyId,
                            BranchCode = header.BranchCode,
                            Batch = string.Empty,
                            //Source = "GL",
                            Source = "AP",
                            Remarks = header.Remarks,
                            Narration = header.Narration,
                            JvDate = DateTime.Now,
                            Amount = header.Amount ?? 0,
                            DocNum = header.PaymentNumber.ToString(),
                            CDate = DateTime.Now,
                            Approved = true,
                            ApprovedDate = DateTime.Now,
                            Posted = true,
                            Void = false,
                            PostedDate = DateTime.Now
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;

                        List<TblFinTrnJournalVoucherItem> JournalVoucherItemsList = new();
                        var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Vendor");
                        foreach (var obj1 in distributionsList)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = header.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = header.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = header.Remarks ?? header.Narration,
                                CostAllocation = costallocations.Id,
                                CostSegCode = header.VendCode,
                                SiteCode = String.Empty
                            };
                            JournalVoucherItemsList.Add(JournalVoucherItem);
                        }

                        if (JournalVoucherItemsList.Count > 0)
                        {
                            await _context.JournalVoucherItems.AddRangeAsync(JournalVoucherItemsList);
                            await _context.SaveChangesAsync();
                        }


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
                                CrAmount = item.CrAmount ?? 0,
                                DrAmount = item.DrAmount ?? 0,
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
                                Source = "PD",
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

                        header.IsPdcCleared = true;
                        header.PdcClearedDate = DateTime.Now;
                        header.PdcClearedBy = obj.PdcClearedBy;
                        _context.OpmVendPaymentHeaders.Update(header);

                        await _context.SaveChangesAsync();
                        Log.Info("----Info PostingPDC method ends----");
                        await transaction.CommitAsync();

                        return ApiMessageInfo.Status(1, header.Id);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in PostingPDC Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }

    }

    #endregion

    #endregion

}
