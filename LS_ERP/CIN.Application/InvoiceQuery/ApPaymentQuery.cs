using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{
    #region GetPagedList

    public class GetVendorPaymentList : IRequest<PaginatedList<TblFinTrnVendorPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetVendorPaymentListHandler : IRequestHandler<GetVendorPaymentList, PaginatedList<TblFinTrnVendorPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorPaymentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnVendorPaymentDto>> Handle(GetVendorPaymentList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            bool isArab = request.User.Culture.IsArab();
            var list = _context.TrnVendorPayments.AsNoTracking()
               .OrderBy(request.Input.OrderBy)
               .Select(e => new TblFinTrnVendorPaymentDto
               {
                   Id = e.Id,
                   VoucherNumber = e.VoucherNumber,
                   CustName = isArab ? e.SndVendorMaster.VendArbName : e.SndVendorMaster.VendName,
                   BranchName = e.SysCompanyBranch.BranchName,
                   TranDate = e.TranDate,
                   DocNum = e.DocNum,
                   Checkdate = e.Checkdate,
                   VendCode = e.VendCode,
                   CheckNumber = e.CheckNumber,
                   Amount = e.Amount,
                   IsPaid = e.IsPaid
               });

            var newList = await list.Where(e => //e.CompanyId == request.CompanyId &&
                                                //              (e.BranchCode.Contains(search) || e.BranchName.Contains(search) ||
                                 e.VoucherNumber.ToString().Contains(search) ||
                                 EF.Functions.Like(e.CustName, "%" + search + "%")
                              )
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return newList;
        }
    }

    #endregion


    #region GetVendorSingleItem

    public class GetVendorSingleItem : IRequest<TblFinTrnVendorPaymentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVendorSingleItemHandler : IRequestHandler<GetVendorSingleItem, TblFinTrnVendorPaymentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorSingleItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnVendorPaymentDto> Handle(GetVendorSingleItem request, CancellationToken cancellationToken)
        {
            var list = await _context.TrnVendorPayments.AsNoTracking()
               .Where(e => e.Id == request.Id)
               .Select(e => new TblFinTrnVendorPaymentDto
               {
                   Id = e.Id,
                   VoucherNumber = e.VoucherNumber,
                   VendCode = e.VendCode,
                   CompanyId = e.CompanyId,
                   BranchCode = e.BranchCode,
                   PayCode = e.PayCode,
                   PayType = e.PayType,
                   TranDate = e.TranDate,
                   DocNum = e.DocNum,
                   Checkdate = e.Checkdate,
                   CheckNumber = e.CheckNumber,
                   Amount = e.Amount,
                   Narration = e.Narration,
                   Preparedby = e.Preparedby,
                   Remarks = e.Remarks
               })
                 .FirstOrDefaultAsync(cancellationToken);

            return list;
        }
    }

    #endregion


    #region GetVendorStatementList

    public class GetVendorStatementList : IRequest<TblFinTrnCustomerStatementItemDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetVendorStatementListHandler : IRequestHandler<GetVendorStatementList, TblFinTrnCustomerStatementItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorStatementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnCustomerStatementItemDto> Handle(GetVendorStatementList request, CancellationToken cancellationToken)
        {
            var list = await _context.TrnVendorStatements.AsNoTracking()
              .Where(e => e.VendCode == request.CustomerCode)
              .OrderByDescending(e => e.InvoiceId).ThenBy(e => e.CrAmount)
              .Select(e => new TblFinTrnCustomerStatementDto
              {
                  PaymentType = e.PaymentType,
                  Remarks1 = e.Remarks1,
                  TranDate = e.TranDate,
                  TranSource = e.TranSource,
                  Trantype = e.Trantype,
                  DrAmount = e.DrAmount,
                  CrAmount = e.CrAmount

              }).ToListAsync(cancellationToken);


            int count = 0;
            decimal? balance = 0, oldBalance = 0;
            foreach (var item in list)
            {
                count++;
                if (count > 1)
                    item.TranNumber = ((oldBalance + item.DrAmount) - item.CrAmount).ToString();
                else
                {
                    balance = (item.DrAmount - item.CrAmount);
                    item.TranNumber = balance.ToString();
                }
                oldBalance = Convert.ToDecimal(item.TranNumber);
            }

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == request.CustomerCode);
            bool isArab = request.User.Culture.IsArab();

            return new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                CustomerName = isArab ? customer.VendArbName : customer.VendName,
                CustCode = request.CustomerCode,
                CustAddress1 = customer.VendAddress1,
                CustAddress2 = customer.VendAddress2,

                List = list,

            };
        }
    }

    #endregion



    #region GetVendorInvoiceStatementList

    public class GetVendorInvoiceStatementList : IRequest<List<TblFinTrnVendorInvoiceDto>>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetVendorInvoiceStatementListHandler : IRequestHandler<GetVendorInvoiceStatementList, List<TblFinTrnVendorInvoiceDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorInvoiceStatementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinTrnVendorInvoiceDto>> Handle(GetVendorInvoiceStatementList request, CancellationToken cancellationToken)
        {

            var list = await _context.TrnVendorInvoices.AsNoTracking()
              .Where(e => e.VendCode == request.CustomerCode && (e.AppliedAmount > 0 || e.Trantype != "Advance"))
              .OrderByDescending(e => e.InvoiceId)
              .ProjectTo<TblFinTrnVendorInvoiceDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            var payments = _context.OpmVendorPayments.AsNoTracking();

            list.ForEach(item =>
            {
                item.AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount);
            });

            return list;
        }
    }

    #endregion 


    #region GetVendorToBePaidAmount

    public class GetVendorToBePaidAmount : IRequest<CustomerPayAmountDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetVendorToBePaidAmountHandler : IRequestHandler<GetVendorToBePaidAmount, CustomerPayAmountDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorToBePaidAmountHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerPayAmountDto> Handle(GetVendorToBePaidAmount request, CancellationToken cancellationToken)
        {
            var cInvoiceItems = _context.TrnVendorInvoices.Where(e => e.VendCode == request.CustomerCode);
            var cInvoices = cInvoiceItems.Where(e => e.NetAmount != e.PaidAmount);

            decimal? NetAmount = await cInvoices.SumAsync(e => e.NetAmount);
            decimal? PaidAmount = await cInvoices.SumAsync(e => e.PaidAmount);
            decimal? AppliedAmount = await cInvoiceItems.Where(e => e.AppliedAmount > 0)
                                           .SumAsync(e => e.AppliedAmount);

            var item = new CustomerPayAmountDto
            {
                NetAmount = NetAmount,
                PaidAmount = PaidAmount,
                AppliedAmount = AppliedAmount,
                //TobePaidAmount = NetAmount - PaidAmount,
                TobePaidAmount = NetAmount - (PaidAmount + AppliedAmount),
            };
            //item.IsPaid = item.TobePaidAmount == 0;
            return item;
        }
    }

    #endregion


    #region CreateVendorPayment

    public class CreateVendorPayment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnVendorPaymentDto Input { get; set; }
    }

    public class CreateVendorPaymentQueryHandler : IRequestHandler<CreateVendorPayment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateVendorPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateVendorPayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateVendorPaymentQuery method start----");

                    var obj = request.Input;
                    TblFinTrnVendorPayment cObj = new();
                    if (obj.Id > 0)
                        cObj = await _context.TrnVendorPayments.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    else
                    {
                        int vouSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            vouSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                ApVoucherSeq = vouSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            vouSeq = invSeq.ApVoucherSeq + 1;
                            invSeq.ApVoucherSeq = vouSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();
                        cObj.VoucherNumber = vouSeq;

                    }

                    cObj.CompanyId = obj.CompanyId;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.TranDate = obj.TranDate;
                    cObj.VendCode = obj.VendCode;
                    cObj.PayType = obj.PayType;
                    cObj.PayCode = obj.PayCode;
                    cObj.Remarks = obj.Remarks;
                    cObj.Amount = obj.Amount;
                    cObj.DocNum = "DocNum";
                    cObj.Narration = obj.Narration;
                    cObj.Preparedby = obj.Preparedby;

                    if (((int)PayCodeTypeEnum.Bank).ToString() == obj.PayType)
                    {
                        cObj.CheckNumber = obj.CheckNumber;
                        cObj.Checkdate = obj.Checkdate;
                    }

                    if (obj.Id > 0)
                    {
                        _context.TrnVendorPayments.Update(cObj);
                    }
                    else
                    {

                        await _context.TrnVendorPayments.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    Log.Info("----Info CreateVendorPaymentQuery method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, cObj.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateVendorPaymentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion



    #region VendorPaymentApprovalTwo

    public class VendorPaymentApprovalTwo : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerPaymentApprovalDto Input { get; set; }
    }

    public class VendorPaymentApprovalTwoQueryHandler : IRequestHandler<VendorPaymentApprovalTwo, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public VendorPaymentApprovalTwoQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(VendorPaymentApprovalTwo request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info VendorPaymentApprovalTwoQuery method start----");

                    string CustomerCode = request.Input.CustomerCode;
                    var cPayment = await _context.TrnVendorPayments.Include(e => e.SndVendorMaster).FirstOrDefaultAsync(e => e.VendCode == CustomerCode && e.Id == request.Input.Id);

                    var cInvoiceItems = _context.TrnVendorInvoices.Where(e => e.VendCode == CustomerCode);
                    var cInvoices = cInvoiceItems.Where(e => e.NetAmount != e.PaidAmount);
                    var cInvoiceAppliedAmountList = await cInvoiceItems.Where(e => e.Trantype == "Advance" && e.AppliedAmount > 0).ToListAsync();


                    //if (!(await cInvoices.AnyAsync()))
                    //    return ApiMessageInfo.Status(2);

                    var cInvoiceIds = await cInvoices.Select(e => e.InvoiceId).ToListAsync();
                    var AppliedAmount = cInvoiceAppliedAmountList.Sum(e => e.AppliedAmount);
                    decimal? amount = cPayment.Amount + AppliedAmount;
                    bool isLeastCredit = false;

                    //var invcItem = await _context.TrnVendorStatements
                    //  .FirstOrDefaultAsync(e => e.VendCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Invoice");

                    TblFinTrnVendorStatement cStatement = new()
                    {
                        CompanyId = (int)cPayment.CompanyId,
                        BranchCode = cPayment.BranchCode,
                        TranDate = DateTime.Now,
                        TranSource = "AP",
                        Trantype = "Payment",
                        TranNumber = "0",
                        VendCode = cPayment.VendCode,
                        DocNum = cPayment.DocNum,
                        ReferenceNumber = string.Empty,
                        PaymentType = "Credit",
                        PamentCode = "Paycode",
                        CheckNumber = cPayment.CheckNumber,
                        Remarks1 = cPayment.Remarks,
                        Remarks2 = string.Empty,
                        LoginId = request.User.UserId,
                        DrAmount = 0,
                        CrAmount = cPayment.Amount,
                        InvoiceId = null,
                        PaymentId = cPayment.VoucherNumber
                    };
                    await _context.TrnVendorStatements.AddAsync(cStatement);
                    await _context.SaveChangesAsync();

                    foreach (var invId in cInvoiceIds)
                    {
                        if (isLeastCredit)
                            break;

                        var cInvoice = await cInvoices.FirstOrDefaultAsync(e => e.InvoiceId == invId);

                        if (amount >= (cInvoice.NetAmount - cInvoice.PaidAmount))
                        {
                            amount = amount - (cInvoice.NetAmount - cInvoice.PaidAmount);
                            decimal? totalPaid = cInvoice.PaidAmount + (cInvoice.NetAmount - cInvoice.PaidAmount);
                            cInvoice.PaidAmount = totalPaid;
                            cInvoice.BalanceAmount = cInvoice.NetAmount - totalPaid;
                            cInvoice.IsPaid = true;
                        }
                        else if (amount < (cInvoice.NetAmount - cInvoice.PaidAmount))
                        {
                            decimal? remaining = cInvoice.PaidAmount + amount;
                            cInvoice.PaidAmount = remaining;
                            cInvoice.BalanceAmount = cInvoice.NetAmount - remaining;
                            amount = 0;
                            isLeastCredit = true;
                        }
                        _context.TrnVendorInvoices.Update(cInvoice);
                        await _context.SaveChangesAsync();
                    }


                    //Updating Applied amount to zero
                    foreach (var apItem in cInvoiceAppliedAmountList)
                    {
                        apItem.AppliedAmount = 0;
                        apItem.IsPaid = true;
                        _context.TrnVendorInvoices.Update(apItem);
                        await _context.SaveChangesAsync();
                    }

                    //_context.TrnVendorInvoices.RemoveRange(cInvoiceAppliedAmountList);
                    //await _context.SaveChangesAsync();

                    if (amount > 0)
                    {
                        var obj = await cInvoiceItems.FirstOrDefaultAsync();
                        TblFinTrnVendorInvoice cInvoice = new()
                        {
                            CompanyId = (int)obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            InvoiceNumber = "0",
                            InvoiceDate = obj.InvoiceDate,
                            CreditDays = 0,
                            DueDate = DateTime.Now,
                            TranSource = obj.TranSource,
                            Trantype = "Advance",
                            VendCode = obj.VendCode,
                            DocNum = "DocNum",
                            LoginId = request.User.UserId,
                            ReferenceNumber = obj.ReferenceNumber,
                            InvoiceAmount = 0,
                            DiscountAmount = 0,
                            NetAmount = 0,
                            PaidAmount = 0,
                            AppliedAmount = amount,
                            Remarks1 = string.Empty,
                            Remarks2 = string.Empty,
                            InvoiceId = null,
                            IsPaid = false,
                        };
                        cInvoice.BalanceAmount = cInvoice.NetAmount - cInvoice.PaidAmount;
                        await _context.TrnVendorInvoices.AddAsync(cInvoice);
                    }

                    cPayment.IsPaid = true;
                    _context.TrnVendorPayments.Update(cPayment);
                    await _context.SaveChangesAsync();


                    var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == cPayment.PayCode);

                    TblFinTrnDistribution distribution1 = new()
                    {
                        InvoiceId = null,
                        FinAcCode = cPayment.SndVendorMaster.VendArAcCode,
                        CrAmount = 0,
                        DrAmount = cPayment.Amount,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = "Vendor",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution1);
                    await _context.SaveChangesAsync();

                    TblFinTrnDistribution distribution2 = new()
                    {
                        InvoiceId = null,
                        FinAcCode = payCode.FinPayAcIntgrAC,
                        CrAmount = cPayment.Amount,
                        DrAmount = 0,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = "paycode",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution2);
                    await _context.SaveChangesAsync();


                    /*  updating last paid and payment date    */


                    var custAmt = _context.TrnVendorStatements.Where(e => e.VendCode == CustomerCode);
                    var custInvAmt = (await custAmt.SumAsync(e => e.CrAmount) - await custAmt.SumAsync(e => e.DrAmount));
                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == CustomerCode);
                    vendor.VendOutStandBal = custInvAmt;
                    vendor.VendLastPaidDate = DateTime.Now;
                    vendor.VendLastPayAmt = cPayment.Amount ?? 0;
                    _context.VendorMasters.Update(vendor);
                    await _context.SaveChangesAsync();



                    //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == CustomerCode);
                    //customer.VendLastPaidDate = DateTime.Now;
                    //customer.VendLastPayAmt = cPayment.Amount ?? 0;                    
                    //_context.VendorMasters.Update(customer);
                    //await _context.SaveChangesAsync();

                    /* Create JV Distribution data. Creata an Automatic JV voucher with next seq number*/

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
                        CompanyId = (int)cPayment.CompanyId,
                        BranchCode = cPayment.BranchCode,
                        Batch = string.Empty,
                        //Source = "GL",
                        Source = "AP",
                        Remarks = cPayment.Remarks,
                        Narration = cPayment.Narration,
                        JvDate = DateTime.Now,
                        Amount = cPayment.Amount ?? 0,
                        DocNum = cPayment.DocNum,
                        CDate = DateTime.Now,
                        Posted = true,
                        PostedDate = DateTime.Now
                    };

                    await _context.JournalVouchers.AddAsync(JV);
                    await _context.SaveChangesAsync();

                    var jvId = JV.Id;

                    var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                        .Where(e => e.FinBranchCode == cPayment.BranchCode).ToListAsync();
                    if (branchAuths.Count() > 0)
                    {
                        List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                        foreach (var item in branchAuths)
                        {
                            TblFinTrnJournalVoucherApproval approval = new()
                            {
                                CompanyId = (int)cPayment.CompanyId,
                                BranchCode = cPayment.BranchCode,
                                JvDate = DateTime.Now,
                                TranSource = "AP",
                                //TranSource = "GL",
                                Trantype = "Invoice",
                                DocNum = cPayment.DocNum,
                                LoginId = Convert.ToInt32(item.AppAuth),
                                AppRemarks = cPayment.Remarks,
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
                    var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Vendor");
                    foreach (var obj1 in distributionsList)
                    {
                        var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                        {
                            JournalVoucherId = jvId,
                            BranchCode = cPayment.BranchCode,
                            Batch = string.Empty,
                            Batch2 = string.Empty,
                            Remarks = cPayment.Remarks,
                            CrAmount = obj1.CrAmount ?? 0,
                            DrAmount = obj1.DrAmount ?? 0,
                            FinAcCode = obj1.FinAcCode,
                            Description = cPayment.Remarks ?? cPayment.Narration,
                            CostAllocation = costallocations.Id,
                            CostSegCode = vendor.VendCode,
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
                        Remarks1 = cPayment.Remarks,
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
                            Source = "PP",
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

                    await transaction.CommitAsync();

                    Log.Info("----Info VendorPaymentApprovalTwoQuery method ends----");
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in VendorPaymentApprovalTwoQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }
    #endregion


    #region GetVendorpaymentvoucher

    public class GetVendorpaymentvoucher : IRequest<VendorReceiptVoucherDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVendorpaymentvoucherHandler : IRequestHandler<GetVendorpaymentvoucher, VendorReceiptVoucherDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorpaymentvoucherHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<VendorReceiptVoucherDto> Handle(GetVendorpaymentvoucher request, CancellationToken cancellationToken)
        {
            var item = await _context.TrnVendorPayments.AsNoTracking()
              .Where(e => e.Id == request.Id)
            .Select(e => new VendorReceiptVoucherDto
            {
                VoucherNumber = e.VoucherNumber,
                CustomerName = request.User.Culture == "ar" ? e.SndVendorMaster.VendArbName : e.SndVendorMaster.VendName,
                Company = e.SysCompany.CompanyName,
                Address = e.SndVendorMaster.VendAddress1,
                DocNum = e.DocNum,
                Date = e.TranDate,
                Logo = e.SysCompany.LogoURL,


                BranchName = e.SysCompanyBranch.BranchName,
                CheckNumber = e.CheckNumber,
                Checkdate = e.Checkdate,
                ReceivedFrom = e.Preparedby,
                VoucherDate = e.TranDate,
                PayType = e.PayType,
                Amount = e.Amount

            })
            .FirstOrDefaultAsync();

            if (item is not null)
                item.PayType = EnumData.GetPayCodeTypeEnum().FirstOrDefault(e => e.Value == item.PayType).Text;

            return item;
        }
    }

    #endregion

    #region Delete
    public class DeleteVendorPayment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVendorPaymentQueryHandler : IRequestHandler<DeleteVendorPayment, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVendorPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVendorPayment request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");

                if (request.Id > 0)
                {
                    var CPayment = await _context.TrnVendorPayments.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(CPayment);
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
}
