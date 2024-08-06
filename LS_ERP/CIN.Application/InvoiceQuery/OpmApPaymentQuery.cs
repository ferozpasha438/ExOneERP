using AutoMapper;
using CIN.Application.Common;
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
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{
    #region GetPagedList

    public class GetOpmVendorPaymentList : IRequest<PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetOpmVendorPaymentListHandler : IRequestHandler<GetOpmVendorPaymentList, PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmVendorPaymentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnCustomerPaymentDto>> Handle(GetOpmVendorPaymentList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = _context.OpmVendPaymentHeaders
               .OrderBy(request.Input.OrderBy)
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
                   IsPaid = e.IsPaid
               });
            var newList = await list.Where(e => //e.CompanyId == request.CompanyId &&
                                                //              (e.BranchCode.Contains(search) || e.BranchName.Contains(search) ||
                                e.VoucherNumber.ToString().Contains(search) ||
                                EF.Functions.Like(e.CustName, "%" + search + "%") ||
                                e.Amount.ToString().Contains(request.Input.Query.Replace(",", ""))
                             )
                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return newList;
        }
    }

    #endregion


    #region GetOpmVendorSingleItem

    public class GetOpmVendorSingleItem : IRequest<OpmCustomerPaymentSingleItemDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetOpmVendorSingleItemHandler : IRequestHandler<GetOpmVendorSingleItem, OpmCustomerPaymentSingleItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmVendorSingleItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentSingleItemDto> Handle(GetOpmVendorSingleItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var opmCustPayments = _context.OpmVendorPayments.AsNoTracking();
            var customers = _context.VendorMasters;
            var cInvoices = _context.TranVenInvoices.AsNoTracking();

            var obj = await _context.OpmVendPaymentHeaders.AsNoTracking()
               .Where(e => e.Id == request.Id)
               .Select(e => new TblFinTrnCustomerPaymentDto
               {
                   Id = e.Id,
                   CustCode = e.VendCode,
                   CompanyId = e.CompanyId,
                   BranchCode = e.BranchCode,
                   PayCode = e.PayCode,
                   PayType = e.PayType,
                   TranDate = e.TranDate,
                   DocNum = e.DocNum,
                   CheckDate = e.Checkdate,
                   CheckNumber = e.CheckNumber,
                   Amount = e.Amount,
                   Narration = e.Narration,
                   Preparedby = e.Preparedby,
                   Remarks = e.Remarks
               })
                 .FirstOrDefaultAsync(cancellationToken);

            var opmCustPaymentList = opmCustPayments.Where(e => e.PaymentId == obj.Id);

            var payments = await (from IV in opmCustPaymentList
                                  join inv in cInvoices on IV.InvoiceId equals inv.Id
                                  select new TblTranInvoiceListDto
                                  {
                                      Id = (long)IV.InvoiceId,
                                      //SpInvoiceNumber = IV.SpInvoiceNumber,
                                      InvoiceNumber = IV.InvoiceNumber,
                                      InvoiceDate = IV.InvoiceDate,
                                      InvoiceDueDate = IV.InvoiceDueDate,
                                      //ServiceDate1 = obj.ServiceDate1,

                                      DiscountAmount = (decimal)IV.DiscountAmount,
                                      // AmountBeforeTax = (decimal)IV.AmountBeforeTax,
                                      //TaxAmount = (decimal)IV.TaxAmount,
                                      InvoiceStatus = inv.IsCreditConverted ? "Credit" : "Invoice",
                                      TotalAmount = (decimal)(inv.IsCreditConverted ? (-1) * IV.Amount : IV.Amount),
                                      BranchName = IV.SysCompanyBranch.BranchName,
                                      CustomerName = isArab ? customers.FirstOrDefault(e => e.VendCode == obj.CustCode).VendArbName : customers.FirstOrDefault(e => e.VendCode == obj.CustCode).VendName,
                                      InvoiceRefNumber = IV.InvoiceRefNumber,
                                      IsApproved = true
                                  }).ToListAsync();

            var unPaidPayment = await opmCustPaymentList.Where(e => e.Flag1).FirstOrDefaultAsync();
            foreach (var item in payments)
            {
                if (unPaidPayment is not null && item.Id == unPaidPayment.InvoiceId)
                {
                    item.TotalAmount = item.TotalAmount - unPaidPayment.BalanceAmount;
                    break;
                }
            }

            return new() { List = payments, Header = obj };
        }
    }

    #endregion

    #region GetOpmApInvoiceList

    public class GetOpmApInvoiceList : IRequest<List<TblTranInvoiceListDto>>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public bool IsEdit { get; set; }
    }

    public class GetOpmApInvoiceListHandler : IRequestHandler<GetOpmApInvoiceList, List<TblTranInvoiceListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmApInvoiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblTranInvoiceListDto>> Handle(GetOpmApInvoiceList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetOpmApInvoiceList method start----");
            var opmCustPayments = _context.OpmVendorPayments.AsNoTracking().Where(e => e.VendCode == request.CustCode);
            var cInvoices = _context.TrnVendorInvoices.AsNoTracking().Where(e => e.VendCode == request.CustCode);

            //if (request.IsEdit)
            //    opmCustPayments = opmCustPayments.Where(e => e.IsPaid);//|| !e.Flag1);
            //else
            //    opmCustPayments = opmCustPayments.Where(e => e.Flag2 || (e.IsPaid && !e.Flag1));


            var customers = _context.VendorMasters;
            int customerId = await customers.Where(e => e.VendCode == request.CustCode).Select(e => e.Id).FirstOrDefaultAsync();

            var opmInvoiceIds = opmCustPayments.Where(e => e.Flag1);

            //  Paid  Onces           
            var invoiceIds = await opmCustPayments.Select(e => e.InvoiceId).ToListAsync();

            //balancezero
            var custInvoiceIDs = await cInvoices.Where(e => e.BalanceAmount > 0).Select(e => (long)e.InvoiceId).ToListAsync();//&& !e.Flag1


            var invoices = _context.TranVenInvoices.AsNoTracking().Where(e => custInvoiceIDs.Any(eid => eid == e.Id));

            bool isArab = request.User.Culture.IsArab();

            var reports = invoices.Select(IV => new TblTranInvoiceListDto
            {
                Id = IV.Id,
                //SpInvoiceNumber = IV.SpInvoiceNumber,
                InvoiceNumber = IV.CreditNumber == string.Empty ? IV.SpCreditNumber : IV.CreditNumber,
                InvoiceDate = IV.InvoiceDate,
                CreatedOn = IV.CreatedOn,
                InvoiceDueDate = IV.InvoiceDueDate,
                CompanyId = (int)IV.CompanyId,
                CustomerId = (long)IV.CustomerId,
                SubTotal = (decimal)IV.SubTotal,
                ServiceDate1 = IV.ServiceDate1,
                DiscountAmount = (decimal)IV.DiscountAmount,
                AmountBeforeTax = (decimal)IV.AmountBeforeTax,
                TaxAmount = (decimal)IV.TaxAmount,
                TotalAmount = (decimal)(IV.IsCreditConverted ? (-1) * IV.TotalAmount : IV.TotalAmount),
                IsCreditConverted = IV.IsCreditConverted,
                InvoiceStatus = IV.IsCreditConverted ? "Credit" : "Invoice",
                TotalPayment = (decimal)IV.TotalPayment,
                AmountDue = (decimal)IV.AmountDue,
                VatPercentage = (decimal)IV.VatPercentage,
                BranchName = IV.SysCompanyBranch.BranchName,
                CustomerName = isArab ? customers.FirstOrDefault(e => e.VendCode == request.CustCode).VendArbName : customers.FirstOrDefault(e => e.VendCode == request.CustCode).VendName,
                InvoiceRefNumber = IV.InvoiceRefNumber,
                LpoContract = IV.LpoContract,
                PaymentTermId = IV.SndPoTermsCode.POTermsName,
                TaxIdNumber = IV.TaxIdNumber,
                IsSettled = cInvoices.Where(e => e.InvoiceId == IV.Id && e.IsPaid).Any(),
                AppliedAmount = 0
            })
            .Where(e => e.IsSettled && e.CustomerId == customerId)
            .OrderByDescending(e => e.TotalAmount);

            var nreports = await reports.ToListAsync();

            // var unPaidPayment = await opmCustPayments.Where(e => e.Flag1).FirstOrDefaultAsync();

            //var opmCustPayments1 = _context.OpmVendorPayments.AsNoTracking().Where(e => e.VendCode == request.CustCode);

            //if (request.IsEdit)
            //    opmCustPayments1 = opmCustPayments1.Where(e => !e.IsPaid && e.Flag2);
            //else
            //    opmCustPayments1 = opmCustPayments1.Where(e => !e.IsPaid && !e.Flag2 || e.IsPaid && e.Flag1);

            foreach (var item in nreports)
            {
                //if (request.IsEdit)
                //    item.AppliedAmount = (await opmCustPayments1.FirstOrDefaultAsync(e => e.InvoiceId == item.Id))?.AppliedAmount ?? 0;
                //else
                //{
                //    var balancePayment = (await opmCustPayments1.Where(e => e.InvoiceId == item.Id).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                //    item.AppliedAmount = balancePayment is not null ? (balancePayment.Sum(e => e.Sum(e => e.AppliedAmount))) : 0;
                //}

                var cInvoice = await cInvoices.FirstOrDefaultAsync(e => e.InvoiceId == item.Id);
                item.AppliedAmount = cInvoice.PaidAmount;

                //if (unPaidPayment is not null && item.Id == unPaidPayment.InvoiceId)
                //{
                //    item.TotalAmount = item.TotalAmount - unPaidPayment.CrAmount;
                //    break;
                //}
            }

            //var nreports = await reports.PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            Log.Info("----Info GetOpmApInvoiceList method Exit----");
            return nreports;

        }

    }

    #endregion


    #region CreateOpmVendorPayment

    public class CreateOpmVendorPayment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnOpmCustomerPaymentListHeaderDto Input { get; set; }
    }

    public class CreateOpmVendorPaymentQueryHandler : IRequestHandler<CreateOpmVendorPayment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpmVendorPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateOpmVendorPayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateOpmVendorPaymentQuery method start----");

                    var obj = request.Input;
                    var advPayment = request.Input.AdvancePayment;
                    bool hasAdvPayment = false, hasOverShortAmount = false;
                    TblFinTrnOpmVendorPaymentHeader cObj = new();
                    bool isCreating = true;

                    if (obj.Id > 0)
                    {
                        isCreating = false;
                        cObj = await _context.OpmVendPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id && !e.IsPaid);
                    }
                    else
                    {
                        int vouSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            vouSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                ApPaymentNumber = vouSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            vouSeq = invSeq.ApPaymentNumber + 1;
                            invSeq.ApPaymentNumber = vouSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();
                        cObj.PaymentNumber = vouSeq;

                    }

                    cObj.CompanyId = obj.CompanyId;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.TranDate = obj.TranDate;
                    cObj.VendCode = obj.CustCode;
                    cObj.PayType = obj.PayType;
                    cObj.PayCode = obj.PayCode;
                    cObj.Remarks = obj.Remarks;

                    cObj.Amount = obj.Amount;
                    cObj.CrAmount = 0;
                    cObj.DiscountAmount = 0;

                    //if (obj.IsPdcCleared)
                    //{
                    //    cObj.IsPdcCleared = obj.IsPdcCleared;
                    //    cObj.PdcClearedDate = obj.CheckDate;
                    //    cObj.PdcClearedBy = obj.PdcClearedBy;
                    //}

                    cObj.DocNum = obj.DocNum.HasValue() ? obj.DocNum : "DocNum";
                    // cObj.InvoiceRefNumber = obj. ?? "DocNum";
                    cObj.Narration = obj.Narration;
                    cObj.Preparedby = obj.Preparedby;

                    decimal? totalInvoiecAmount = 0; bool hasInviceIds = false;
                    if (obj.InviceIds is not null && obj.InviceIds.Count() > 0)
                    {
                        hasInviceIds = true;
                        //var venInvs = _context.TranVenInvoices.Where(e => obj.InviceIds.Any(eid => eid.Id == e.Id));
                        //totalInvoiecAmount = (await venInvs.Where(e => !e.IsCreditConverted).SumAsync(e => e.TotalAmount) - await venInvs.Where(e => e.IsCreditConverted).SumAsync(e => e.TotalAmount));
                        totalInvoiecAmount = obj.Amount;
                    }
                    cObj.InvoiceAmount = totalInvoiecAmount;


                    if (((int)PayCodeTypeEnum.Bank).ToString() == obj.PayType)
                    {
                        cObj.CheckNumber = obj.CheckNumber;
                        cObj.Checkdate = obj.CheckDate;
                        cObj.HasPdcClearance = true;
                    }
                    else
                    {
                        cObj.HasPdcClearance = false;
                    }

                    cObj.IsPdcCleared = false;


                    if (isCreating && advPayment is not null && advPayment.HasData == 1) //over payment calculation
                    {

                        TblFinTrnCustomerWallet custWallet = await _context.CustomerWallets.FirstOrDefaultAsync(e => e.CustCode == obj.CustCode && e.Source == "AP");
                        if (custWallet is not null)
                        {
                            hasAdvPayment = true;

                            custWallet.AppliedAmount = advPayment.RemainingAdvAmount switch
                            {
                                > 0 => (custWallet.AppliedAmount + advPayment.PaidAmount),
                                < 0 => custWallet.AdvAmount,
                                _ => custWallet.AdvAmount,
                            };

                            _context.CustomerWallets.Update(custWallet);
                            await _context.SaveChangesAsync();
                        }


                        if (advPayment.AmountType.HasValue() && (advPayment.AmountType == "short" || advPayment.AmountType == "over"))
                        {
                            hasOverShortAmount = true;
                        }
                    }


                    if (obj.Id > 0)
                    {
                        _context.OpmVendPaymentHeaders.Update(cObj);
                    }
                    else
                    {
                        if (hasAdvPayment)
                        {
                            cObj.HasRemaining = advPayment.RemainingAdvAmount switch
                            {
                                > 0 => 1,
                                < 0 => 2,
                                _ => 0,
                            };

                            if (cObj.HasRemaining > 0)
                                cObj.AdvAmount = advPayment.PaidAmount;

                        }

                        await _context.OpmVendPaymentHeaders.AddAsync(cObj);
                    }

                    await _context.SaveChangesAsync();


                    if (hasOverShortAmount)
                    {
                        TblFinTrnOverShortAmount overShortAmount = new()
                        {
                            PaymentId = cObj.Id,
                            Amount = advPayment.WriteOffAmount,
                            Source = "ap",
                            AmtType = advPayment.AmountType
                        };
                        await _context.OverShortAmounts.AddAsync(overShortAmount);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateOpmVendorPaymentQuery method Exit----");

                    if (hasInviceIds)
                    {
                        List<TblFinTrnOpmVendorPayment> customerPayments = new();

                        //var opmCPayments = _context.OpmVendorPayments.Where(e => e.VendCode == obj.CustCode && !e.IsPaid);

                        //var opmCPaymentIds = await opmCPayments.Select(e => e.InvoiceId).ToListAsync();

                        //obj.InviceIds = obj.InviceIds.Where(e => !opmCPaymentIds.Any(eid => eid == e)).ToList();

                        //var exstinngInviceIds = obj.InviceIds.Where(e => opmCPaymentIds.Any(eid => eid == e)).ToList();


                        //foreach (var eInviceId in exstinngInviceIds)
                        //{
                        //    var opmCPayment = await opmCPayments.FirstOrDefaultAsync(e => e.InvoiceId == eInviceId);
                        //    opmCPayment.PaymentId = cObj.Id;

                        //    _context.OpmVendorPayments.Update(opmCPayment);
                        //    await _context.SaveChangesAsync();
                        //}

                        foreach (var inviceId in obj.InviceIds)
                        {
                            var inv = await _context.TranVenInvoices.FirstOrDefaultAsync(e => e.Id == inviceId.Id);
                            var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == inv.CustomerId);

                            customerPayments.Add(new()
                            {
                                BranchCode = inv.BranchCode,
                                PaymentId = cObj.Id,
                                InvoiceId = inviceId.Id,
                                InvoiceNumber = inv.CreditNumber,
                                TranDate = inv.CreatedOn,
                                DocNum = inv.InvoiceRefNumber,
                                InvoiceDate = inv.InvoiceDate,
                                InvoiceDueDate = inv.InvoiceDueDate,
                                Remarks = inv.Remarks,
                                InvoiceRefNumber = inv.InvoiceRefNumber,
                                VendCode = customer.VendCode,

                                DiscountAmount = inv.DiscountAmount ?? 0,
                                Amount = inv.TotalAmount ?? 0,
                                CrAmount = 0,
                                BalanceAmount = !inv.IsCreditConverted ? ((inv.TotalAmount - inviceId.AppliedAmount - inviceId.Amount) ?? 0) : 0,
                                AppliedAmount = inviceId.Amount ?? 0,
                                NetAmount = 0,
                                Flag1 = !inv.IsCreditConverted ? ((inv.TotalAmount - inviceId.AppliedAmount - inviceId.Amount) <= 0 ? false : true) : false, //true for Full false for half Payment
                                Flag2 = inv.IsCreditConverted //True for Credit, false for Invoice.

                                //BalanceAmount = inv.TotalAmount,
                                //NetAmount = 0,
                                //Flag2 = inv.IsCreditConverted

                            });
                        }

                        if (customerPayments.Count() > 0)
                        {
                            var opmCustPayments = await _context.OpmVendorPayments.Where(e => e.PaymentId == cObj.Id && e.CrAmount <= 0).ToListAsync();
                            _context.OpmVendorPayments.RemoveRange(opmCustPayments);

                            await _context.OpmVendorPayments.AddRangeAsync(customerPayments);
                            await _context.SaveChangesAsync();
                        }
                    }


                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, cObj.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateOpmVendorPaymentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region OpmVendorPaymentApproval

    public class OpmVendorPaymentApproval : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerPaymentApprovalDto Input { get; set; }
    }

    public class OpmVendorPaymentApprovalQueryHandler : IRequestHandler<OpmVendorPaymentApproval, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public OpmVendorPaymentApprovalQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(OpmVendorPaymentApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info OpmVendorPaymentApprovalQuery method start----");

                    string CustomerCode = request.Input.CustomerCode;

                    var header = await _context.OpmVendPaymentHeaders.Include(e => e.SndVendorMaster).
                        FirstOrDefaultAsync(e => e.VendCode == CustomerCode && e.Id == request.Input.Id);

                    //var cPayment = await _context.OpmVendorPayments.Include(e => e.SysCompanyBranch).ThenInclude(d => d.SysCompany)
                    //    .Include(e => e.SndVendorMaster).FirstOrDefaultAsync(e => e.VendCode == CustomerCode && e.Id == request.Input.Id);

                    var vendorInvoiceItems = _context.TrnVendorInvoices.Where(e => e.VendCode == CustomerCode);
                    var custPayments = await _context.OpmVendorPayments.Where(e => e.PaymentId == header.Id && !e.IsPaid).ToListAsync();


                    var opmVendorPaymentsInvoiceIDs = vendorInvoiceItems.Select(e => e.InvoiceNumber).ToList();
                    //Credit payments
                    var custCreditPayments = custPayments.Where(e => e.Flag2).ToList();

                    //Invoice payments
                    var custInvoicePayments = custPayments.Where(e => !e.Flag2).ToList();

                    //var cInvoiceAppliedAmountList = await cInvoiceItems.Where(e => e.Trantype == "Advance" && e.AppliedAmount > 0).ToListAsync();
                    //var cInvoiceIds = await cInvoices.Select(e => e.InvoiceId).ToListAsync();
                    //var AppliedAmount = cInvoiceAppliedAmountList.Sum(e => e.AppliedAmount);

                    decimal? amount = header.Amount + custCreditPayments.Sum(e => e.BalanceAmount);
                    bool isLeastCredit = false;

                    //var invcItem = await _context.TrnCustomerStatements
                    //  .FirstOrDefaultAsync(e => e.VendCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Invoice");

                    int companyId = header.CompanyId;

                    #region Advance Amount is not Applied

                    if (header.HasRemaining <= 0 || header.HasRemaining is null)
                    {


                        TblFinTrnVendorStatement cStatement = new()
                        {
                            CompanyId = companyId,
                            BranchCode = header.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "AP",
                            Trantype = "Payment",
                            TranNumber = "0",
                            VendCode = header.VendCode,
                            DocNum = header.DocNum,
                            ReferenceNumber = string.Empty,
                            PaymentType = "Credit",
                            PamentCode = header.PayCode,
                            CheckNumber = header.CheckNumber,
                            Remarks1 = request.Input.Remarks.HasValue() ? request.Input.Remarks : header.Remarks,
                            Remarks2 = string.Empty,
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = header.Amount,
                            InvoiceId = null,
                            PaymentId = header.PaymentNumber

                        };
                        await _context.TrnVendorStatements.AddAsync(cStatement);
                        await _context.SaveChangesAsync();

                        var balancePayments = _context.OpmVendorPayments.Where(e => e.VendCode == CustomerCode).Select(e => new { e.InvoiceId, e.BalanceAmount });

                        foreach (var payment in custInvoicePayments)
                        {

                            bool hasZeroBalance = await balancePayments.AnyAsync(e => e.InvoiceId == payment.InvoiceId && e.BalanceAmount <= 0);

                            payment.CrAmount = payment.CrAmount + payment.AppliedAmount;
                            //payment.BalanceAmount = payment.BalanceAmount - payment.AppliedAmount;

                            if (hasZeroBalance)
                            {
                                payment.BalanceAmount = 0;
                                var setBalances = await _context.OpmVendorPayments.Where(e => e.VendCode == CustomerCode && e.InvoiceId == payment.InvoiceId).ToListAsync();
                                foreach (var balance in setBalances)
                                {
                                    balance.BalanceAmount = 0;
                                }

                                if (setBalances.Any())
                                {
                                    _context.OpmVendorPayments.UpdateRange(setBalances);
                                }

                            }

                            payment.NetAmount = 0;
                            payment.IsPaid = true;
                            payment.Flag1 = hasZeroBalance ? false : true;

                            //if (isLeastCredit)
                            //    break;

                            //if (amount >= (payment.Amount - payment.CrAmount))
                            //{
                            //    amount = amount - (payment.Amount - payment.CrAmount);
                            //    payment.CrAmount = payment.Amount;
                            //    payment.BalanceAmount = 0;
                            //    payment.NetAmount = 0;
                            //    payment.IsPaid = true;

                            //    payment.Flag1 = false;
                            //}
                            //else
                            //{
                            //    payment.CrAmount = amount;
                            //    payment.BalanceAmount = payment.Amount - amount;
                            //    payment.NetAmount = 0;
                            //    amount = 0;
                            //    isLeastCredit = true;
                            //    payment.Flag1 = true;
                            //}


                            _context.OpmVendorPayments.Update(payment);
                            await _context.SaveChangesAsync();

                        }

                        foreach (var payment in custCreditPayments)
                        {
                            payment.CrAmount = (-1) * payment.Amount;
                            payment.BalanceAmount = 0;
                            payment.NetAmount = 0;
                            payment.DiscountAmount = 0;

                            payment.IsPaid = true;
                            payment.Flag1 = false;


                            _context.OpmVendorPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }


                        List<TblFinTrnVendorInvoice> vendorInvoices = new();
                        foreach (var custInvoiceItem in await vendorInvoiceItems.Where(e => opmVendorPaymentsInvoiceIDs.Any(invoiceNum => e.InvoiceNumber == invoiceNum)).ToListAsync())
                        {
                            var balancePayment = (await _context.OpmVendorPayments.Where(e => e.InvoiceId == custInvoiceItem.InvoiceId).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                            custInvoiceItem.PaidAmount = balancePayment.Sum(e => e.Sum(e => e.CrAmount));
                            custInvoiceItem.BalanceAmount = custInvoiceItem.Trantype.ToLower() == "credit" ? 0 : (custInvoiceItem.InvoiceAmount - custInvoiceItem.PaidAmount);
                            custInvoiceItem.AppliedAmount = custInvoiceItem.PaidAmount;
                            //custInvoiceItem.IsPaid = custInvoiceItem.InvoiceAmount == custInvoiceItem.PaidAmount ? true : false;

                            vendorInvoices.Add(custInvoiceItem);
                        }

                        if (vendorInvoices.Any())
                        {
                            _context.TrnVendorInvoices.UpdateRange(vendorInvoices);
                            await _context.SaveChangesAsync();
                        }


                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == header.PayCode);

                        var overShortPay = await _context.OverShortAmounts.FirstOrDefaultAsync(e => e.PaymentId == header.Id && e.Source == "ap");
                        bool isThereRecord = overShortPay is not null;
                        bool isShortAmt = isThereRecord && overShortPay.AmtType == "short";

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = header.SndVendorMaster.VendArAcCode,
                            DrAmount = header.Amount,
                            CrAmount = 0,
                            Source = "AP",
                            Gl = string.Empty,
                            Type = "Vendor",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        //Money will Go out from PAYCODE
                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = header.HasPdcClearance == true ? payCode.FinPayPDCClearAC : payCode.FinPayAcIntgrAC,
                            DrAmount = 0,
                            CrAmount = isThereRecord ? (isShortAmt ? (header.Amount - overShortPay.Amount) : (header.Amount + overShortPay.Amount)) : header.Amount,
                            Source = "AP",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };


                        if (overShortPay is not null)
                        {
                            bool isShort = overShortPay.AmtType == "short";
                            TblFinTrnDistribution distribution3 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = isShort ? header.SndVendorMaster.VendARAdjAcCode : header.SndVendorMaster.VendDefExpAcCode,
                                DrAmount = isShort ? 0 : overShortPay.Amount,
                                CrAmount = isShort ? overShortPay.Amount : 0,
                                Source = "AR",
                                Gl = string.Empty,
                                Type = "Customer",
                                CreatedOn = DateTime.Now
                            };
                            await _context.FinDistributions.AddAsync(distribution3);
                            distributionsList.Add(distribution3);
                        }

                        await _context.SaveChangesAsync();


                        /*  updating last paid and payment date    */

                        var custAmt = _context.TrnVendorStatements.Where(e => e.VendCode == CustomerCode);
                        var custInvAmt = (await custAmt.SumAsync(e => e.CrAmount) - await custAmt.SumAsync(e => e.DrAmount));
                        var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == CustomerCode);
                        vendor.VendOutStandBal = custInvAmt;
                        vendor.VendAvailCrLimit = vendor.VendCrLimit - vendorInvoiceItems.Sum(e => e.PaidAmount);
                        vendor.VendLastPaidDate = DateTime.Now;
                        vendor.VendLastPayAmt = header.Amount ?? 0;

                        //_context.VendorMasters.Update(customer);
                        //await _context.SaveChangesAsync();


                        //var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == CustomerCode);
                        //customer.VendLastPaidDate = DateTime.Now;
                        //customer.VendLastPayAmt = header.Amount ?? 0;
                        //_context.VendorMasters.Update(customer);
                        //await _context.SaveChangesAsync();

                        /* Create JV Distribution data. Creata an Automatic JV voucher with next seq number*/



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
                            CompanyId = companyId,
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

                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                            .Where(e => e.FinBranchCode == header.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = companyId,
                                    BranchCode = header.BranchCode,
                                    JvDate = DateTime.Now,
                                    //TranSource = "GL",
                                    TranSource = "AP",
                                    Trantype = "Invoice",
                                    DocNum = header.DocNum,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = header.Remarks,
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
                                BranchCode = header.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = header.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = header.Remarks ?? header.Narration,
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
                            Remarks1 = header.Remarks,
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

                        header.IsPaid = true;
                        header.IsPosted = true;
                        header.PostedDate = DateTime.Now;

                        _context.OpmVendPaymentHeaders.Update(header);
                        await _context.SaveChangesAsync();

                        Log.Info("----Info OpmVendorPaymentApprovalQuery method ends----");
                        await transaction.CommitAsync();
                        return ApiMessageInfo.Status(1, 1);
                    }

                    #endregion


                    #region Advance Amount is Applied


                    else if (header.HasRemaining > 0)
                    {
                        decimal? headerAmount = header.HasRemaining == 2 ? header.AdvAmount : 0;

                        TblFinTrnVendorStatement cStatement = new()
                        {
                            CompanyId = companyId,
                            BranchCode = header.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "AP",
                            Trantype = "Payment",
                            TranNumber = "0",
                            VendCode = header.VendCode,
                            DocNum = header.DocNum,
                            ReferenceNumber = string.Empty,
                            PaymentType = "Credit",
                            PamentCode = header.PayCode,
                            CheckNumber = header.CheckNumber,
                            Remarks1 = request.Input.Remarks.HasValue() ? request.Input.Remarks : header.Remarks,
                            Remarks2 = string.Empty,
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = headerAmount,
                            InvoiceId = null,
                            PaymentId = header.PaymentNumber

                        };
                        await _context.TrnVendorStatements.AddAsync(cStatement);
                        await _context.SaveChangesAsync();


                        var balancePayments = _context.OpmVendorPayments.Where(e => e.VendCode == CustomerCode).Select(e => new { e.InvoiceId, e.BalanceAmount });

                        foreach (var payment in custInvoicePayments)
                        {

                            bool hasZeroBalance = await balancePayments.AnyAsync(e => e.InvoiceId == payment.InvoiceId && e.BalanceAmount <= 0);

                            payment.CrAmount = payment.CrAmount + payment.AppliedAmount;
                            //payment.BalanceAmount = payment.BalanceAmount - payment.AppliedAmount;

                            if (hasZeroBalance)
                            {
                                payment.BalanceAmount = 0;
                                var setBalances = await _context.OpmVendorPayments.Where(e => e.VendCode == CustomerCode && e.InvoiceId == payment.InvoiceId).ToListAsync();
                                foreach (var balance in setBalances)
                                {
                                    balance.BalanceAmount = 0;
                                }

                                if (setBalances.Any())
                                {
                                    // _context.OpmVendorPayments.UpdateRange(setBalances);
                                }

                            }

                            payment.NetAmount = 0;
                            payment.IsPaid = true;
                            payment.Flag1 = hasZeroBalance ? false : true;

                            _context.OpmVendorPayments.Update(payment);
                            await _context.SaveChangesAsync();

                        }

                        foreach (var payment in custCreditPayments)
                        {
                            payment.CrAmount = (-1) * payment.Amount;
                            payment.BalanceAmount = 0;
                            payment.NetAmount = 0;
                            payment.DiscountAmount = 0;

                            payment.IsPaid = true;
                            payment.Flag1 = false;


                            _context.OpmVendorPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }

                        List<TblFinTrnVendorInvoice> vendorInvoices = new();
                        foreach (var custInvoiceItem in await vendorInvoiceItems.Where(e => opmVendorPaymentsInvoiceIDs.Any(invoiceNum => e.InvoiceNumber == invoiceNum)).ToListAsync())
                        {
                            var balancePayment = (await _context.OpmVendorPayments.Where(e => e.InvoiceId == custInvoiceItem.InvoiceId).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                            custInvoiceItem.PaidAmount = balancePayment.Sum(e => e.Sum(e => e.CrAmount));
                            custInvoiceItem.BalanceAmount = custInvoiceItem.Trantype.ToLower() == "credit" ? 0 : (custInvoiceItem.InvoiceAmount - custInvoiceItem.PaidAmount);
                            custInvoiceItem.AppliedAmount = custInvoiceItem.PaidAmount;
                            //custInvoiceItem.IsPaid = custInvoiceItem.InvoiceAmount == custInvoiceItem.PaidAmount ? true : false;

                            vendorInvoices.Add(custInvoiceItem);
                        }

                        if (vendorInvoices.Any())
                        {
                            _context.TrnVendorInvoices.UpdateRange(vendorInvoices);
                            await _context.SaveChangesAsync();
                        }


                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == header.PayCode);

                        List<TblFinTrnDistribution> distributionsList = new();
                        if (header.HasRemaining == 2)
                        {
                            TblFinTrnDistribution distribution1 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = header.SndVendorMaster.VendArAcCode,
                                DrAmount = headerAmount,
                                CrAmount = 0,
                                Source = "AP",
                                Gl = string.Empty,
                                Type = "Vendor",
                                CreatedOn = DateTime.Now
                            };
                            await _context.FinDistributions.AddAsync(distribution1);
                            distributionsList.Add(distribution1);
                            //Money will Go out from PAYCODE
                            TblFinTrnDistribution distribution2 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = payCode.FinPayAcIntgrAC,
                                DrAmount = 0,
                                CrAmount = headerAmount,
                                Source = "AP",
                                Gl = string.Empty,
                                Type = "paycode",
                                CreatedOn = DateTime.Now
                            };
                            await _context.FinDistributions.AddAsync(distribution2);
                            distributionsList.Add(distribution2);
                        }

                        await _context.SaveChangesAsync();


                        /*  updating last paid and payment date    */
                        var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.VendCode == CustomerCode);
                        if (header.HasRemaining == 2)
                        {
                            var custAmt = _context.TrnVendorStatements.Where(e => e.VendCode == CustomerCode);
                            var custInvAmt = (await custAmt.SumAsync(e => e.CrAmount) - await custAmt.SumAsync(e => e.DrAmount));

                            vendor.VendOutStandBal = custInvAmt;
                            vendor.VendAvailCrLimit = vendor.VendCrLimit - vendorInvoiceItems.Sum(e => e.PaidAmount);
                            vendor.VendLastPaidDate = DateTime.Now;
                            vendor.VendLastPayAmt = header.Amount ?? 0;
                            _context.VendorMasters.Update(vendor);
                            await _context.SaveChangesAsync();
                        }


                        if (header.HasRemaining == 2)
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

                            TblFinTrnJournalVoucher JV = new()
                            {
                                SpVoucherNumber = string.Empty,
                                VoucherNumber = jvSeq.ToString(),
                                CompanyId = companyId,
                                BranchCode = header.BranchCode,
                                Batch = string.Empty,
                                //Source = "GL",
                                Source = "AP",
                                Remarks = header.Remarks,
                                Narration = header.Narration,
                                JvDate = DateTime.Now,
                                Amount = headerAmount ?? 0,
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

                            var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                                .Where(e => e.FinBranchCode == header.BranchCode).ToListAsync();
                            if (branchAuths.Count() > 0)
                            {
                                List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                                foreach (var item in branchAuths)
                                {
                                    TblFinTrnJournalVoucherApproval approval = new()
                                    {
                                        CompanyId = companyId,
                                        BranchCode = header.BranchCode,
                                        JvDate = DateTime.Now,
                                        //TranSource = "GL",
                                        TranSource = "AP",
                                        Trantype = "Invoice",
                                        DocNum = header.DocNum,
                                        LoginId = Convert.ToInt32(item.AppAuth),
                                        AppRemarks = header.Remarks,
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
                                    BranchCode = header.BranchCode,
                                    Batch = string.Empty,
                                    Batch2 = string.Empty,
                                    Remarks = header.Remarks,
                                    CrAmount = obj1.CrAmount ?? 0,
                                    DrAmount = obj1.DrAmount ?? 0,
                                    FinAcCode = obj1.FinAcCode,
                                    Description = header.Remarks ?? header.Narration,
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
                                Remarks1 = header.Remarks,
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

                        }

                        header.IsPaid = true;
                        header.IsPosted = true;
                        header.PostedDate = DateTime.Now;

                        _context.OpmVendPaymentHeaders.Update(header);
                        await _context.SaveChangesAsync();

                        Log.Info("----Info OpmVendorPaymentApprovalQuery method ends----");
                        await transaction.CommitAsync();
                        return ApiMessageInfo.Status(1, 1);

                    }

                    #endregion

                    return ApiMessageInfo.Status(0);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in OpmVendorPaymentApprovalQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region GetOpmVendorpaymentvoucher

    public class GetOpmVendorpaymentvoucher : IRequest<CustomerReceiptVoucherListDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetOpmVendorpaymentvoucherHandler : IRequestHandler<GetOpmVendorpaymentvoucher, CustomerReceiptVoucherListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmVendorpaymentvoucherHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerReceiptVoucherListDto> Handle(GetOpmVendorpaymentvoucher request, CancellationToken cancellationToken)
        {
            var item = await _context.OpmVendPaymentHeaders
                .Include(e => e.SndVendorMaster)
                .Include(e => e.SysCompany)
                .Include(e => e.SysCompanyBranch)
                .AsNoTracking()
              .Where(e => e.Id == request.Id)
            .Select(e => new CustomerReceiptVoucherListDto
            {
                CustomerId = e.SndVendorMaster.Id,
                VoucherNumber = e.PaymentNumber,
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


            var custAllnvoices = _context.TranVenInvoices.Where(e => e.CustomerId == item.CustomerId);

            var paidInvoices = await custAllnvoices.Where(e => e.InvoiceStatus != "Open")
                .Select(e => new TblTranInvoiceDto
                {
                    InvoiceNumber = e.CreditNumber,
                    InvoiceDate = e.InvoiceDate,
                    InvoiceDueDate = e.InvoiceDueDate,
                    TotalAmount = e.TotalAmount,
                    IsCreditConverted = e.IsCreditConverted,
                }).ToListAsync();

            var unPaidInvoices = await custAllnvoices.Where(e => e.InvoiceStatus == "Open")
               .Select(e => new TblTranInvoiceDto
               {
                   InvoiceNumber = e.CreditNumber,
                   InvoiceDate = e.InvoiceDate,
                   InvoiceDueDate = e.InvoiceDueDate,
                   TotalAmount = e.TotalAmount,
                   IsCreditConverted = e.IsCreditConverted,
               }).ToListAsync();


            item.PaidList = paidInvoices;
            item.UnPaidList = unPaidInvoices;


            return item;
        }
    }

    #endregion
}
