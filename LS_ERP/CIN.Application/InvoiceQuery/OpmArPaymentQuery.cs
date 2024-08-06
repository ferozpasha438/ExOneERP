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
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{

    #region GetPagedList

    public class GetOpmCustomerPaymentList : IRequest<PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetOpmCustomerPaymentListHandler : IRequestHandler<GetOpmCustomerPaymentList, PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmCustomerPaymentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnCustomerPaymentDto>> Handle(GetOpmCustomerPaymentList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = _context.OpmCustPaymentHeaders
                .Include(e => e.SndCustomerMaster).Include(e => e.SysCompanyBranch).AsNoTracking()
               //.Where(e => //e.CompanyId == request.CompanyId &&
               //              (e.BranchCode.Contains(search) || e.BranchName.Contains(search) ||
               //                  e.VoucherNumber.Contains(search) || e.Mobile.Contains(search)
               //               ))
               .OrderBy(request.Input.OrderBy)
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


    #region GetOpmSingleItem

    public class GetOpmSingleItem : IRequest<OpmCustomerPaymentSingleItemDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetOpmSingleItemHandler : IRequestHandler<GetOpmSingleItem, OpmCustomerPaymentSingleItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmSingleItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentSingleItemDto> Handle(GetOpmSingleItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var opmCustPayments = _context.OpmCustomerPayments.AsNoTracking();
            var customers = _context.OprCustomers;
            var cInvoices = _context.TranInvoices.AsNoTracking();

            var obj = await _context.OpmCustPaymentHeaders.AsNoTracking()
               .Where(e => e.Id == request.Id)
               .Select(e => new TblFinTrnCustomerPaymentDto
               {
                   Id = e.Id,
                   CustCode = e.CustCode,
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
                                      InvoiceStatus = inv.IsCreditConverted ? "Credit" : "Invoice",
                                      DiscountAmount = (decimal)IV.DiscountAmount,
                                      // AmountBeforeTax = (decimal)IV.AmountBeforeTax,
                                      //TaxAmount = (decimal)IV.TaxAmount,
                                      //TotalAmount = (decimal)(IV.Amount),
                                      TotalAmount = (decimal)(inv.IsCreditConverted ? (-1) * IV.Amount : IV.Amount),
                                      BranchName = IV.SysCompanyBranch.BranchName,
                                      CustomerName = isArab ? customers.FirstOrDefault(e => e.CustCode == obj.CustCode).CustArbName : customers.FirstOrDefault(e => e.CustCode == obj.CustCode).CustName,
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

    #region GetOpmArInvoiceList

    public class GetOpmArInvoiceList : IRequest<List<TblTranInvoiceListDto>>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public bool IsEdit { get; set; }
    }

    public class GetOpmArInvoiceListHandler : IRequestHandler<GetOpmArInvoiceList, List<TblTranInvoiceListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmArInvoiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblTranInvoiceListDto>> Handle(GetOpmArInvoiceList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetOpmArInvoiceList method start----");
            // var opmCustPayments = _context.OpmCustomerPayments.AsNoTracking().Where(e => e.CustCode == request.CustCode);
            var cInvoices = _context.TrnCustomerInvoices.AsNoTracking().Where(e => e.CustCode == request.CustCode);

            //if (request.IsEdit)
            //    opmCustPayments = opmCustPayments.Where(e => e.IsPaid);//|| !e.Flag1);
            //else
            //    opmCustPayments = opmCustPayments.Where(e => e.Flag2 || (e.IsPaid && !e.Flag1));

            var customers = _context.OprCustomers;
            var sites = _context.OprSites;
            int customerId = await customers.Where(e => e.CustCode == request.CustCode).Select(e => e.Id).FirstOrDefaultAsync();

            // var opmInvoiceIds = opmCustPayments.Where(e => e.Flag1);
            //  Paid  Onces            
            //var invoiceIDs = await opmCustPayments.Select(e => e.InvoiceId).ToListAsync();//&& !e.Flag1

            //balancezero
            var custInvoiceIDs = await cInvoices.Where(e => e.BalanceAmount > 0).Select(e => (long)e.InvoiceId).ToListAsync();//&& !e.Flag1


            //  var invoices = _context.TranInvoices.AsNoTracking().Where(e => e.CustomerId == customerId && 
            // (!invoiceIDs.Any(eid => eid == e.Id) || opmInvoiceIds.Any(eid => eid.InvoiceId == e.Id)));

            var invoices = _context.TranInvoices.AsNoTracking().Where(e => custInvoiceIDs.Any(eid => eid == e.Id));


            //var opminvoices = _context.TranInvoices.AsNoTracking().Where(e => opmInvoiceIds.Any(eid => eid.InvoiceId == e.Id));

            // var invoices = existingInvoices.Concat(opminvoices);

            // var existingInvoices = _context.TranInvoices.AsNoTracking().Where(e => opmInvoiceIds.Any(eid => eid == e.Id));

            if (request.SiteCode.HasValue())
            {
                invoices = invoices.Where(e => e.SiteCode == request.SiteCode);
            }

            bool isArab = request.User.Culture.IsArab();

            //int isCreditType = -1;

            var reports = invoices.Select(IV => new TblTranInvoiceListDto
            {
                Id = IV.Id,
                //SpInvoiceNumber = IV.SpInvoiceNumber,
                InvoiceNumber = IV.InvoiceNumber == string.Empty ? IV.SpInvoiceNumber : IV.InvoiceNumber,
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
                TotalPayment = (decimal)IV.TotalPayment,
                AmountDue = (decimal)IV.AmountDue,
                VatPercentage = (decimal)IV.VatPercentage,
                BranchName = IV.SysCompanyBranch.BranchName,
                CustomerName = isArab ? customers.FirstOrDefault(e => e.CustCode == request.CustCode).CustArbName : customers.FirstOrDefault(e => e.CustCode == request.CustCode).CustName,
                SiteName = isArab ? sites.FirstOrDefault(e => e.SiteCode == IV.SiteCode).SiteArbName : sites.FirstOrDefault(e => e.SiteCode == IV.SiteCode).SiteName,
                InvoiceRefNumber = IV.InvoiceRefNumber,
                LpoContract = IV.LpoContract,
                PaymentTermId = IV.SndSalesTermsCode.SalesTermsName,
                TaxIdNumber = IV.TaxIdNumber,
                InvoiceStatus = IV.IsCreditConverted ? "Credit" : "Invoice",
                IsSettled = cInvoices.Where(e => e.InvoiceId == IV.Id && e.IsPaid).Any(),
                AppliedAmount = 0

            })
            .Where(e => e.IsSettled)
            .OrderByDescending(e => e.TotalAmount);

            var nreports = await reports.ToListAsync();

            //var unPaidPayments = opmCustPayments.Where(e => !e.IsPaid && !e.Flag2);
            var opmCustPayments1 = _context.OpmCustomerPayments.AsNoTracking().Where(e => e.CustCode == request.CustCode);
            var opmCustPayments2 = opmCustPayments1;

            if (request.IsEdit)
                opmCustPayments1 = opmCustPayments1.Where(e => !e.IsPaid && e.Flag2);
            else
                opmCustPayments1 = opmCustPayments1.Where(e => !e.IsPaid && !e.Flag2 || e.IsPaid && e.Flag1);



            foreach (var item in nreports)
            {
                //if (request.IsEdit)
                //    item.AppliedAmount = (await opmCustPayments1.FirstOrDefaultAsync(e => e.InvoiceId == item.Id))?.AppliedAmount ?? 0;
                //else
                //{
                //    var balancePayment = (await opmCustPayments1.Where(e => e.InvoiceId == item.Id).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                //    item.AppliedAmount = balancePayment is not null ? (balancePayment.Sum(e => e.Sum(e => e.AppliedAmount))) : 0;
                //}


                //Condition 1
                //var cInvoice = await cInvoices.FirstOrDefaultAsync(e => e.InvoiceId == item.Id);
                //item.AppliedAmount = cInvoice.PaidAmount;

                //Condition 2
                var paymentAmount = await opmCustPayments2.Where(e => e.InvoiceId == item.Id).SumAsync(e => e.AppliedAmount);
                item.AppliedAmount = paymentAmount;


                //if (unPaidPayment is not null && item.Id == unPaidPayment.InvoiceId)
                //{
                //    item.AppliedAmount = unPaidPayment.AppliedAmount;
                //    item.TotalAmount = item.TotalAmount - unPaidPayment.CrAmount;
                //    break;
                //}
            }

            //var nreports = await reports.PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            Log.Info("----Info GetOpmArInvoiceList method Exit----");
            return nreports.Where(e => e.TotalAmount != e.AppliedAmount).ToList();

        }

    }

    #endregion

    #region CreateOpmCustomerPayment

    public class CreateOpmCustomerPayment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnOpmCustomerPaymentListHeaderDto Input { get; set; }
    }

    public class CreateOpmCustomerPaymentQueryHandler : IRequestHandler<CreateOpmCustomerPayment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpmCustomerPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateOpmCustomerPayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateOpmCustomerPaymentQuery method start----");

                    var obj = request.Input;
                    var advPayment = request.Input.AdvancePayment;
                    bool hasAdvPayment = false, hasOverShortAmount = false;
                    TblFinTrnOpmCustomerPaymentHeader cObj = new();
                    bool isCreating = true;
                    if (obj.Id > 0)
                    {
                        isCreating = false;
                        cObj = await _context.OpmCustPaymentHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id && !e.IsPaid);
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
                                ArPaymentNumber = vouSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            vouSeq = invSeq.ArPaymentNumber + 1;
                            invSeq.ArPaymentNumber = vouSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();
                        cObj.PaymentNumber = vouSeq;

                    }

                    cObj.CompanyId = obj.CompanyId;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.TranDate = obj.TranDate;
                    cObj.CustCode = obj.CustCode;
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
                    cObj.SiteCode = obj.SiteCode;


                    decimal? totalInvoiecAmount = 0; bool hasInviceIds = false;
                    if (obj.InviceIds is not null && obj.InviceIds.Count() > 0)
                    {
                        hasInviceIds = true;

                        //var venInvs = _context.TranInvoices.Where(e => obj.InviceIds.Any(eid => eid.Id == e.Id));
                        //totalInvoiecAmount = (await venInvs.Where(e => !e.IsCreditConverted).SumAsync(e => e.TotalAmount) - await venInvs.Where(e => e.IsCreditConverted).SumAsync(e => e.TotalAmount));
                        totalInvoiecAmount = obj.Amount;

                        //totalInvoiecAmount = await _context.TranInvoices.Where(e => obj.InviceIds.Any(eid => eid == e.Id)).SumAsync(e => e.TotalAmount);
                    }
                    cObj.InvoiceAmount = totalInvoiecAmount;


                    if (((int)PayCodeTypeEnum.Bank).ToString() == obj.PayType)
                    {
                        cObj.CheckNumber = obj.CheckNumber;
                        cObj.HasPdcClearance = true;
                    }
                    else
                    {
                        cObj.HasPdcClearance = false;
                    }

                    cObj.IsPdcCleared = false;


                    if (isCreating && advPayment is not null && advPayment.HasData == 1) //over payment calculation
                    {

                        TblFinTrnCustomerWallet custWallet = await _context.CustomerWallets.FirstOrDefaultAsync(e => e.CustCode == obj.CustCode && e.Source == "AR");
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

                        //else if (advPayment.AmountType == "advance")
                        //{
                        //    //store in new table
                        //    TblFinTrnAdvanceWallet advWallet = new()
                        //    {
                        //        PaymentId = cObj.Id,
                        //        CompanyId = obj.CompanyId,
                        //        BranchCode = obj.BranchCode,
                        //        TranDate = obj.TranDate,
                        //        CustCode = obj.CustCode,
                        //        PayCode = obj.PayCode,
                        //        Remarks = obj.Remarks,
                        //        AdvAmount = advPayment.AdvanceAmount,
                        //        AppliedAmount = 0,
                        //        PostedAmount = 0,
                        //        PreparedBy = String.Empty,
                        //        Notes = String.Empty,
                        //        IsActive = true,
                        //        Created = DateTime.Now,
                        //        CreatedBy = request.User.UserId
                        //    };
                        //    await _context.AdvanceWallets.AddAsync(advWallet);
                        //    await _context.SaveChangesAsync();
                        //}

                    }

                    if (obj.Id > 0)
                    {
                        _context.OpmCustPaymentHeaders.Update(cObj);
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


                        await _context.OpmCustPaymentHeaders.AddAsync(cObj);


                    }

                    await _context.SaveChangesAsync();

                    if (hasOverShortAmount)
                    {
                        TblFinTrnOverShortAmount overShortAmount = new()
                        {
                            PaymentId = cObj.Id,
                            Amount = advPayment.WriteOffAmount,
                            Source = advPayment.Source,
                            AmtType = advPayment.AmountType
                        };
                        await _context.OverShortAmounts.AddAsync(overShortAmount);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateOpmCustomerPaymentQuery method Exit----");

                    if (hasInviceIds)
                    {
                        List<TblFinTrnOpmCustomerPayment> customerPayments = new();

                        //var opmCPayments = _context.OpmCustomerPayments.Where(e => e.CustCode == obj.CustCode && !e.IsPaid);

                        //var opmCPaymentIds = await opmCPayments.Select(e => e.InvoiceId).ToListAsync();

                        //obj.InviceIds = obj.InviceIds.Where(e => !opmCPaymentIds.Any(eid => eid == e)).ToList();

                        //var exstinngInviceIds = obj.InviceIds.Where(e => opmCPaymentIds.Any(eid => eid == e)).ToList();


                        //foreach (var eInviceId in exstinngInviceIds)
                        //{
                        //    var opmCPayment = await opmCPayments.FirstOrDefaultAsync(e => e.InvoiceId == eInviceId);
                        //    opmCPayment.PaymentId = cObj.Id;

                        //    _context.OpmCustomerPayments.Update(opmCPayment);
                        //    await _context.SaveChangesAsync();
                        //}

                        foreach (var inviceId in obj.InviceIds)
                        {
                            var inv = await _context.TranInvoices.FirstOrDefaultAsync(e => e.Id == inviceId.Id);
                            var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.Id == inv.CustomerId);

                            customerPayments.Add(new()
                            {
                                BranchCode = inv.BranchCode,
                                PaymentId = cObj.Id,
                                InvoiceId = inviceId.Id,
                                InvoiceNumber = inv.InvoiceNumber,
                                TranDate = inv.CreatedOn,
                                DocNum = inv.InvoiceRefNumber,
                                InvoiceDate = inv.InvoiceDate,
                                InvoiceDueDate = inv.InvoiceDueDate,
                                Remarks = inv.Remarks,
                                InvoiceRefNumber = inv.InvoiceRefNumber,
                                CustCode = customer.CustCode,
                                SiteCode = inv.SiteCode,
                                DiscountAmount = inv.DiscountAmount ?? 0,
                                Amount = inv.TotalAmount ?? 0,
                                CrAmount = 0,
                                BalanceAmount = !inv.IsCreditConverted ? ((inv.TotalAmount - inviceId.AppliedAmount - inviceId.Amount) ?? 0) : 0,
                                AppliedAmount = inviceId.Amount ?? 0,
                                NetAmount = 0,
                                Flag1 = !inv.IsCreditConverted ? ((inv.TotalAmount - inviceId.AppliedAmount - inviceId.Amount) <= 0 ? false : true) : false, //true for Full false for half Payment
                                Flag2 = inv.IsCreditConverted //True for Credit, false for Invoice.

                            });
                        }

                        if (customerPayments.Count() > 0)
                        {
                            //if (obj.Id > 0)
                            //{
                            //    var opmCustPayments = await _context.OpmCustomerPayments.Where(e => e.PaymentId == cObj.Id ).ToListAsync();
                            //    _context.OpmCustomerPayments.RemoveRange(opmCustPayments);

                            //    await _context.OpmCustomerPayments.AddRangeAsync(customerPayments);
                            //}
                            //else
                            //{
                            //}


                            var opmCustPayments = await _context.OpmCustomerPayments.Where(e => e.PaymentId == cObj.Id && e.CrAmount <= 0).ToListAsync();
                            _context.OpmCustomerPayments.RemoveRange(opmCustPayments);

                            await _context.OpmCustomerPayments.AddRangeAsync(customerPayments);

                            await _context.SaveChangesAsync();
                        }
                    }




                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, cObj.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateOpmCustomerPaymentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region OpmCustomerPaymentApproval

    public class OpmCustomerPaymentApproval : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerPaymentApprovalDto Input { get; set; }
    }

    public class OpmCustomerPaymentApprovalQueryHandler : IRequestHandler<OpmCustomerPaymentApproval, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public OpmCustomerPaymentApprovalQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(OpmCustomerPaymentApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info OpmCustomerPaymentApprovalQuery method start----");
                    var postedOn = request.Input.CreatedOn ?? DateTime.Now;

                    string CustomerCode = request.Input.CustomerCode;

                    var header = await _context.OpmCustPaymentHeaders.Include(e => e.SndCustomerMaster).
                        FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.Id == request.Input.Id);

                    //var cPayment = await _context.OpmCustomerPayments.Include(e => e.SysCompanyBranch).ThenInclude(d => d.SysCompany)
                    //    .Include(e => e.SndCustomerMaster).FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.Id == request.Input.Id);

                    // var finSetup = await _context.FinSysFinanialSetups.OrderByDescending(e => e.Id).FirstOrDefaultAsync();
                    var customerInvoiceItems = _context.TrnCustomerInvoices.Where(e => e.CustCode == CustomerCode);

                    var custStatementPayments = await _context.OpmCustomerPayments.Where(e => e.PaymentId == header.Id).ToListAsync();
                    var custPayments = custStatementPayments.Where(e => !e.IsPaid).ToList();

                    var opmCustomerPaymentsInvoiceIDs = custPayments.Select(e => e.InvoiceNumber).ToList();

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
                    //  .FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Invoice");

                    int companyId = header.CompanyId;


                    #region Advance Amount is not Applied

                    if (header.HasRemaining <= 0 || header.HasRemaining is null)
                    {
                        // if (finSetup is null || finSetup.ArDistFlag is null || finSetup.ArDistFlag == false)
                        if (header.SiteCode.HasValue())
                        {
                            TblFinTrnCustomerStatement custStmts = new()
                            {
                                CompanyId = companyId,
                                BranchCode = header.BranchCode,
                                TranDate = postedOn,
                                TranSource = "AR",
                                Trantype = "Payment",
                                TranNumber = "0",
                                CustCode = header.CustCode,
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
                                PaymentId = header.PaymentNumber,
                                SiteCode = header.SiteCode
                            };

                            await _context.TrnCustomerStatements.AddAsync(custStmts);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            List<TblFinTrnCustomerStatement> custStmtsList = new();
                            foreach (var item in custStatementPayments)
                            {
                                custStmtsList.Add(new()
                                {
                                    CompanyId = companyId,
                                    BranchCode = item.BranchCode,
                                    TranDate = postedOn,
                                    TranSource = "AR",
                                    Trantype = "Payment",
                                    TranNumber = "0",
                                    CustCode = header.CustCode,
                                    DocNum = header.DocNum,
                                    ReferenceNumber = string.Empty,
                                    PaymentType = "Credit",
                                    PamentCode = header.PayCode,
                                    CheckNumber = header.CheckNumber,
                                    Remarks1 = request.Input.Remarks.HasValue() ? request.Input.Remarks : header.Remarks,
                                    Remarks2 = string.Empty,
                                    LoginId = request.User.UserId,
                                    //DrAmount = item.Flag2 ? item.Amount : 0, //Flag2 isCreditConverted checking
                                    DrAmount = 0,
                                    CrAmount = item.Flag2 ? ((-1) * item.AppliedAmount) : item.AppliedAmount, //Flag2 isCreditConverted checking
                                    InvoiceId = null,
                                    PaymentId = header.PaymentNumber,
                                    SiteCode = item.SiteCode
                                });
                            }

                            if (custStmtsList.Count() > 0)
                            {
                                await _context.TrnCustomerStatements.AddRangeAsync(custStmtsList);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var balancePayments = _context.OpmCustomerPayments.Where(e => e.CustCode == CustomerCode).AsNoTracking().Select(e => new { e.InvoiceId, e.BalanceAmount });

                        foreach (var payment in custInvoicePayments)
                        {
                            bool hasZeroBalance = await balancePayments.AnyAsync(e => e.InvoiceId == payment.InvoiceId && e.BalanceAmount <= 0);

                            payment.CrAmount = payment.CrAmount + payment.AppliedAmount;
                            //payment.BalanceAmount = payment.BalanceAmount - payment.AppliedAmount;

                            if (hasZeroBalance)
                            {
                                payment.BalanceAmount = 0;
                                var setBalances = await _context.OpmCustomerPayments.Where(e => e.CustCode == CustomerCode && e.InvoiceId == payment.InvoiceId && e.Id != payment.Id).AsNoTracking().ToListAsync();
                                foreach (var balance in setBalances)
                                {
                                    balance.BalanceAmount = 0;
                                }

                                if (setBalances.Any())
                                {
                                    //_context.Entry(setBalances).State = EntityState.Modified;
                                    // _context.Entry(setBalances).State = EntityState.Detached;


                                    // _context.OpmCustomerPayments.UpdateRange(setBalances);
                                    // await _context.SaveChangesAsync();

                                }

                            }
                            payment.NetAmount = 0;
                            payment.IsPaid = true;
                            payment.Flag1 = hasZeroBalance ? false : true;
                            // payment.AppliedAmount = 0;

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
                            //    payment.Flag1 = true; //Not Fully Paid
                            //}

                            _context.OpmCustomerPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var payment in custCreditPayments)
                        {
                            payment.CrAmount = (-1) * payment.Amount;
                            payment.BalanceAmount = 0;
                            payment.NetAmount = 0;
                            payment.DiscountAmount = 0;
                            payment.AppliedAmount = 0;

                            payment.IsPaid = true;
                            payment.Flag1 = false;


                            _context.OpmCustomerPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }


                        List<TblFinTrnCustomerInvoice> customerInvoices = new();
                        foreach (var custInvoiceItem in await customerInvoiceItems.Where(e => opmCustomerPaymentsInvoiceIDs.Any(invoiceNum => e.InvoiceNumber == invoiceNum)).ToListAsync())
                        {
                            var balancePayment = (await _context.OpmCustomerPayments.Where(e => e.InvoiceId == custInvoiceItem.InvoiceId).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                            custInvoiceItem.PaidAmount = balancePayment.Sum(e => e.Sum(e => e.CrAmount));
                            custInvoiceItem.BalanceAmount = custInvoiceItem.Trantype.ToLower() == "credit" ? 0 : (custInvoiceItem.InvoiceAmount - custInvoiceItem.PaidAmount);
                            custInvoiceItem.AppliedAmount = custInvoiceItem.PaidAmount;
                            // custInvoiceItem.IsPaid = custInvoiceItem.InvoiceAmount == custInvoiceItem.PaidAmount ? true : false;

                            customerInvoices.Add(custInvoiceItem);
                        }

                        if (customerInvoices.Any())
                        {
                            _context.TrnCustomerInvoices.UpdateRange(customerInvoices);
                            await _context.SaveChangesAsync();
                        }

                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == header.PayCode);
                        var overShortPay = await _context.OverShortAmounts.FirstOrDefaultAsync(e => e.PaymentId == header.Id && e.Source == "ar");
                        bool isThereRecord = overShortPay is not null;
                        bool isShortAmt = isThereRecord && overShortPay.AmtType == "short";

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = header.SndCustomerMaster.CustArAcCode,
                            DrAmount = 0,
                            CrAmount = header.Amount,
                            Source = "AR",
                            Gl = string.Empty,
                            Type = "Customer",
                            CreatedOn = postedOn
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        //Money will Come To PAYCODE
                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = payCode.FinPayAcIntgrAC,
                            DrAmount = isThereRecord ? (isShortAmt ? (header.Amount - overShortPay.Amount) : (header.Amount + overShortPay.Amount)) : header.Amount,
                            CrAmount = 0,
                            Source = "AR",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = postedOn
                        };
                        await _context.FinDistributions.AddAsync(distribution2);

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        if (overShortPay is not null)
                        {
                            bool isShort = overShortPay.AmtType == "short";
                            TblFinTrnDistribution distribution3 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = isShort ? header.SndCustomerMaster.CustARAdjAcCode : header.SndCustomerMaster.CustDefExpAcCode,
                                DrAmount = isShort ? overShortPay.Amount : 0,
                                CrAmount = isShort ? 0 : overShortPay.Amount,
                                Source = "AR",
                                Gl = string.Empty,
                                Type = "Customer",
                                CreatedOn = postedOn
                            };
                            await _context.FinDistributions.AddAsync(distribution3);
                            distributionsList.Add(distribution3);
                        }

                        await _context.SaveChangesAsync();


                        /*  updating last paid and payment date    */

                        var custAmt = _context.TrnCustomerStatements.Where(e => e.CustCode == CustomerCode);
                        var custInvAmt = (await custAmt.SumAsync(e => e.DrAmount) - await custAmt.SumAsync(e => e.CrAmount));
                        var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                        customer.CustOutStandBal = custInvAmt;
                        customer.CustAvailCrLimit = customer.CustCrLimit - customerInvoiceItems.Sum(e => e.PaidAmount);

                        customer.CustLastPaidDate = DateTime.Now;
                        customer.CustLastPayAmt = header.Amount ?? 0;
                        _context.OprCustomers.Update(customer);
                        await _context.SaveChangesAsync();


                        //var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                        //customer.CustLastPaidDate = DateTime.Now;
                        //customer.CustLastPayAmt = header.Amount ?? 0;
                        //_context.OprCustomers.Update(customer);
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
                            Source = "AR",
                            Remarks = header.Remarks,
                            Narration = header.Narration,
                            JvDate = postedOn,
                            Amount = header.Amount ?? 0,
                            DocNum = header.PaymentNumber.ToString(),
                            CDate = DateTime.Now,
                            Approved = true,
                            ApprovedDate = DateTime.Now,
                            Posted = true,
                            Void = false,
                            PostedDate = postedOn,
                            SiteCode = header.SiteCode,
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
                                    JvDate = postedOn,
                                    //TranSource = "GL",
                                    TranSource = "AR",
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
                        var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Customer");
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
                                CostSegCode = customer.CustCode,
                                SiteCode = header.SiteCode
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

                            JvDate = postedOn,
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
                                TransDate = postedOn,
                                PostedFlag = true,
                                PostDate = postedOn,
                                Jvnum = item.JournalVoucherId.ToString(),
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "RP",
                                ExRate = 0,
                                SiteCode = header.SiteCode,
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
                        header.PostedDate = postedOn;

                        _context.OpmCustPaymentHeaders.Update(header);
                        await _context.SaveChangesAsync();

                        Log.Info("----Info OpmCustomerPaymentApprovalQuery method ends----");
                        await transaction.CommitAsync();
                        return ApiMessageInfo.Status(1, 1);
                    }

                    #endregion

                    #region Advance Amount is Applied

                    else if (header.HasRemaining > 0)
                    {
                        decimal? headerAmount = header.HasRemaining == 2 ? header.AdvAmount : 0;
                        // if (finSetup is null || finSetup.ArDistFlag is null || finSetup.ArDistFlag == false)
                        if (header.SiteCode.HasValue())
                        {
                            if (header.HasRemaining == 2)
                            {
                                TblFinTrnCustomerStatement custStmts = new()
                                {
                                    CompanyId = companyId,
                                    BranchCode = header.BranchCode,
                                    TranDate = postedOn,
                                    TranSource = "AR",
                                    Trantype = "Payment",
                                    TranNumber = "0",
                                    CustCode = header.CustCode,
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
                                    PaymentId = header.PaymentNumber,
                                    SiteCode = header.SiteCode
                                };

                                await _context.TrnCustomerStatements.AddAsync(custStmts);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var balancePayments = _context.OpmCustomerPayments.Where(e => e.CustCode == CustomerCode).AsNoTracking().Select(e => new { e.InvoiceId, e.BalanceAmount });

                        foreach (var payment in custInvoicePayments)
                        {
                            bool hasZeroBalance = await balancePayments.AnyAsync(e => e.InvoiceId == payment.InvoiceId && e.BalanceAmount <= 0);

                            payment.CrAmount = payment.CrAmount + payment.AppliedAmount;
                            //payment.BalanceAmount = payment.BalanceAmount - payment.AppliedAmount;

                            if (hasZeroBalance)
                            {
                                payment.BalanceAmount = 0;
                                var setBalances = await _context.OpmCustomerPayments.Where(e => e.CustCode == CustomerCode && e.InvoiceId == payment.InvoiceId && e.Id != payment.Id).AsNoTracking().ToListAsync();
                                foreach (var balance in setBalances)
                                {
                                    balance.BalanceAmount = 0;
                                }

                                if (setBalances.Any())
                                {
                                    //_context.Entry(setBalances).State = EntityState.Modified;
                                    // _context.Entry(setBalances).State = EntityState.Detached;


                                    // _context.OpmCustomerPayments.UpdateRange(setBalances);
                                    // await _context.SaveChangesAsync();

                                }

                            }
                            payment.NetAmount = 0;
                            payment.IsPaid = true;
                            payment.Flag1 = hasZeroBalance ? false : true;
                            // payment.AppliedAmount = 0;

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
                            //    payment.Flag1 = true; //Not Fully Paid
                            //}

                            _context.OpmCustomerPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var payment in custCreditPayments)
                        {
                            payment.CrAmount = (-1) * payment.Amount;
                            payment.BalanceAmount = 0;
                            payment.NetAmount = 0;
                            payment.DiscountAmount = 0;
                            payment.AppliedAmount = 0;

                            payment.IsPaid = true;
                            payment.Flag1 = false;


                            _context.OpmCustomerPayments.Update(payment);
                            await _context.SaveChangesAsync();
                        }


                        List<TblFinTrnCustomerInvoice> customerInvoices = new();
                        foreach (var custInvoiceItem in await customerInvoiceItems.Where(e => opmCustomerPaymentsInvoiceIDs.Any(invoiceNum => e.InvoiceNumber == invoiceNum)).ToListAsync())
                        {
                            var balancePayment = (await _context.OpmCustomerPayments.Where(e => e.InvoiceId == custInvoiceItem.InvoiceId).ToListAsync()).GroupBy(e => e.InvoiceId).ToList();

                            custInvoiceItem.PaidAmount = balancePayment.Sum(e => e.Sum(e => e.CrAmount));
                            custInvoiceItem.BalanceAmount = custInvoiceItem.Trantype.ToLower() == "credit" ? 0 : (custInvoiceItem.InvoiceAmount - custInvoiceItem.PaidAmount);
                            custInvoiceItem.AppliedAmount = custInvoiceItem.PaidAmount;
                            // custInvoiceItem.IsPaid = custInvoiceItem.InvoiceAmount == custInvoiceItem.PaidAmount ? true : false;

                            customerInvoices.Add(custInvoiceItem);
                        }

                        if (customerInvoices.Any())
                        {
                            _context.TrnCustomerInvoices.UpdateRange(customerInvoices);
                            await _context.SaveChangesAsync();
                        }

                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == header.PayCode);

                        //var overShortPay = await _context.OverShortAmounts.FirstOrDefaultAsync(e => e.PaymentId == header.Id);
                        //bool isThereRecord = overShortPay is not null;
                        //bool isShortAmt = isThereRecord && overShortPay.AmtType == "short";

                        List<TblFinTrnDistribution> distributionsList = new();
                        if (header.HasRemaining == 2)
                        {
                            TblFinTrnDistribution distribution1 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = header.SndCustomerMaster.CustArAcCode,
                                DrAmount = 0,
                                CrAmount = headerAmount,
                                Source = "AR",
                                Gl = string.Empty,
                                Type = "Customer",
                                CreatedOn = postedOn
                            };
                            await _context.FinDistributions.AddAsync(distribution1);
                            distributionsList.Add(distribution1);
                            //Money will Come To PAYCODE
                            TblFinTrnDistribution distribution2 = new()
                            {
                                InvoiceId = null,
                                FinAcCode = payCode.FinPayAcIntgrAC,
                                DrAmount = headerAmount,
                                CrAmount = 0,
                                Source = "AR",
                                Gl = string.Empty,
                                Type = "paycode",
                                CreatedOn = postedOn
                            };
                            await _context.FinDistributions.AddAsync(distribution2);
                            distributionsList.Add(distribution2);
                        }

                        //if (overShortPay is not null)
                        //{
                        //    bool isShort = overShortPay.AmtType == "short";
                        //    TblFinTrnDistribution distribution3 = new()
                        //    {
                        //        InvoiceId = null,
                        //        FinAcCode = isShort ? header.SndCustomerMaster.CustARAdjAcCode : header.SndCustomerMaster.CustDefExpAcCode,
                        //        DrAmount = isShort ? overShortPay.Amount : 0,
                        //        CrAmount = isShort ? 0 : overShortPay.Amount,
                        //        Source = "AR",
                        //        Gl = string.Empty,
                        //        Type = "Customer",
                        //        CreatedOn = DateTime.Now
                        //    };
                        //    await _context.FinDistributions.AddAsync(distribution3);
                        //    distributionsList.Add(distribution3);
                        //}

                        await _context.SaveChangesAsync();



                        /*  updating last paid and payment date    */
                        var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                        if (header.HasRemaining == 2)
                        {
                            var custAmt = _context.TrnCustomerStatements.Where(e => e.CustCode == CustomerCode);
                            var custInvAmt = (await custAmt.SumAsync(e => e.DrAmount) - await custAmt.SumAsync(e => e.CrAmount));
                            customer.CustOutStandBal = custInvAmt;
                            customer.CustAvailCrLimit = customer.CustCrLimit - customerInvoiceItems.Sum(e => e.PaidAmount);

                            customer.CustLastPaidDate = DateTime.Now;
                            customer.CustLastPayAmt = header.Amount ?? 0;
                            _context.OprCustomers.Update(customer);
                            await _context.SaveChangesAsync();
                        }

                        //var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                        //customer.CustLastPaidDate = DateTime.Now;
                        //customer.CustLastPayAmt = header.Amount ?? 0;
                        //_context.OprCustomers.Update(customer);
                        //await _context.SaveChangesAsync();


                        /* Create JV Distribution data. Creata an Automatic JV voucher with next seq number*/


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
                                Source = "AR",
                                Remarks = header.Remarks,
                                Narration = header.Narration,
                                JvDate = postedOn,
                                Amount = headerAmount ?? 0,
                                DocNum = header.PaymentNumber.ToString(),
                                CDate = DateTime.Now,
                                Approved = true,
                                ApprovedDate = DateTime.Now,
                                Posted = true,
                                Void = false,
                                PostedDate = postedOn,
                                SiteCode = header.SiteCode,
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
                                        JvDate = postedOn,
                                        //TranSource = "GL",
                                        TranSource = "AR",
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
                            var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Customer");
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
                                    CostSegCode = customer.CustCode,
                                    SiteCode = header.SiteCode
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

                                JvDate = postedOn,
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
                                    TransDate = postedOn,
                                    PostedFlag = true,
                                    PostDate = postedOn,
                                    Jvnum = item.JournalVoucherId.ToString(),
                                    Narration = item.Description,
                                    Remarks = item.Remarks,
                                    Remarks2 = string.Empty,
                                    ReverseFlag = false,
                                    VoidFlag = false,
                                    Source = "RP",
                                    ExRate = 0,
                                    SiteCode = header.SiteCode,
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
                        header.PostedDate = postedOn;

                        _context.OpmCustPaymentHeaders.Update(header);
                        await _context.SaveChangesAsync();

                        Log.Info("----Info OpmCustomerPaymentApprovalQuery method ends----");
                        await transaction.CommitAsync();
                        return ApiMessageInfo.Status(1, 1);

                    }

                    #endregion

                    return ApiMessageInfo.Status(0);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in OpmCustomerPaymentApprovalQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region GetOpmCustomerpaymentvoucher

    public class GetOpmCustomerpaymentvoucher : IRequest<CustomerReceiptVoucherListDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetOpmCustomerpaymentvoucherHandler : IRequestHandler<GetOpmCustomerpaymentvoucher, CustomerReceiptVoucherListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmCustomerpaymentvoucherHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerReceiptVoucherListDto> Handle(GetOpmCustomerpaymentvoucher request, CancellationToken cancellationToken)
        {
            var item = await _context.OpmCustPaymentHeaders
                .Include(e => e.SndCustomerMaster)
                .Include(e => e.SysCompany)
                .Include(e => e.SysCompanyBranch)
                .AsNoTracking()
              .Where(e => e.Id == request.Id)
            .Select(e => new CustomerReceiptVoucherListDto
            {
                CustomerId = e.SndCustomerMaster.Id,
                CustCode = e.SndCustomerMaster.CustCode,
                VoucherNumber = e.PaymentNumber,
                CustomerName = request.User.Culture == "ar" ? e.SndCustomerMaster.CustArbName : e.SndCustomerMaster.CustName,
                Company = e.SysCompany.CompanyName,
                Address = e.SndCustomerMaster.CustAddress1,
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


            var paymentInvoiceIds = _context.OpmCustomerPayments.Where(e => e.PaymentId == request.Id).Select(e => e.InvoiceId);
            var opnCustPayments = _context.TrnCustomerInvoices.AsNoTracking().Where(e => e.CustCode == item.CustCode && paymentInvoiceIds.Any(pId => pId == e.InvoiceId));

            var unPaidInvoiceIds = await opnCustPayments.Where(e => e.BalanceAmount > 0).Select(e => e.InvoiceId).ToListAsync();
            var paidInvoiceIds = await opnCustPayments.Where(e => e.BalanceAmount == 0).Select(e => e.InvoiceId).ToListAsync();

            var custUnPaidAllnvoices = _context.TranInvoices.AsNoTracking().Where(e => unPaidInvoiceIds.Any(eid => eid == e.Id) && e.CustomerId == item.CustomerId);
            var custPaidAllnvoices = _context.TranInvoices.AsNoTracking().Where(e => paidInvoiceIds.Any(eid => eid == e.Id) && e.CustomerId == item.CustomerId);


            // var custAllnvoices = _context.TranInvoices.Where(e => e.CustomerId == item.CustomerId);

            var paidInvoices = await custPaidAllnvoices//.Where(e => e.InvoiceStatus != "Open")
                .Select(e => new TblTranInvoiceDto
                {
                    InvoiceNumber = e.InvoiceNumber,
                    InvoiceDate = e.InvoiceDate,
                    InvoiceDueDate = e.InvoiceDueDate,
                    TotalAmount = e.TotalAmount,
                    IsCreditConverted = e.IsCreditConverted,
                }).ToListAsync();

            var unPaidInvoices = await custUnPaidAllnvoices//.Where(e => e.InvoiceStatus == "Open")
               .Select(e => new TblTranInvoiceDto
               {
                   InvoiceNumber = e.InvoiceNumber,
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


    #region GetCustomerAdvancePaymentPazedList

    public class GetCustomerAdvancePaymentPazedList : IRequest<PaginatedList<TblFinTrnAdvanceWalletDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCustomerAdvancePaymentPazedListHandler : IRequestHandler<GetCustomerAdvancePaymentPazedList, PaginatedList<TblFinTrnAdvanceWalletDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerAdvancePaymentPazedListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnAdvanceWalletDto>> Handle(GetCustomerAdvancePaymentPazedList request, CancellationToken cancellationToken)
        {
            bool isArabic = request.User.Culture.IsArab();
            var search = request.Input.Query;
            var statusId = request.Input.StatusId ?? 0;
            var customer = statusId == 0 ? _context.OprCustomers
                .Select(e => new { CustCode = e.CustCode, CustName = e.CustName, CustArbName = e.CustArbName }).AsNoTracking()
                : _context.VendorMasters
                .Select(e => new { CustCode = e.VendCode, CustName = e.VendName, CustArbName = e.VendArbName }).AsNoTracking();

            var list = _context.AdvanceWallets.AsNoTracking()
               //.Where(e => //e.CompanyId == request.CompanyId &&
               //              (e.BranchCode.Contains(search) || e.BranchName.Contains(search) ||
               //                  e.VoucherNumber.Contains(search) || e.Mobile.Contains(search)
               //               ))
               .Where(e => e.SNDInvNum == statusId)
               .OrderBy(request.Input.OrderBy)
               .Select(e => new TblFinTrnAdvanceWalletDto
               {
                   Id = e.Id,
                   CustCode = e.CustCode,
                   CustomerName = isArabic ? customer.FirstOrDefault(cust => cust.CustCode == e.CustCode).CustArbName :
                                              customer.FirstOrDefault(cust => cust.CustCode == e.CustCode).CustName,
                   TranDate = e.TranDate,
                   PayCode = e.PayCode,
                   AdvAmount = e.AdvAmount
               });

            var newList = await list.Where(e => EF.Functions.Like(e.CustomerName, "%" + search + "%"))
               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return newList;
        }
    }

    #endregion

    #region CreateOpmCustomerAdvancePayment
    public class CreateOpmCustomerAdvancePayment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnAdvanceWalletDto Input { get; set; }
    }

    public class CreateOpmCustomerAdvancePaymentQueryHandler : IRequestHandler<CreateOpmCustomerAdvancePayment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpmCustomerAdvancePaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateOpmCustomerAdvancePayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateOpmCustomerAdvancePaymentQuery method start----");

                    var obj = request.Input;
                    TblFinTrnAdvanceWallet advWallet = new();
                    if (obj.Id > 0)
                        advWallet = await _context.AdvanceWallets.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    else
                    {
                        advWallet.AppliedAmount = 0;
                        advWallet.PostedAmount = 0;
                        advWallet.IsActive = true;
                        advWallet.Created = DateTime.Now;
                        advWallet.CreatedBy = request.User.UserId;
                    }

                    advWallet.CustCode = obj.CustCode;
                    advWallet.SiteCode = obj.SiteCode;
                    advWallet.CompanyId = obj.CompanyId;
                    advWallet.BranchCode = obj.BranchCode;
                    advWallet.TranDate = obj.TranDate;
                    advWallet.PayCode = obj.PayCode;
                    advWallet.AdvAmount = obj.AdvAmount;
                    advWallet.InvoiceNumber = obj.InvoiceNumber;
                    advWallet.PreparedBy = obj.PreparedBy;
                    advWallet.Remarks = obj.Remarks;
                    advWallet.Notes = obj.Notes;
                    advWallet.DocNum = obj.DocNum;
                    advWallet.SNDInvNum = obj.SNDInvNum; //0 for Customer , 1 for Vendor

                    if (obj.Id > 0)
                        _context.AdvanceWallets.Update(advWallet);
                    else
                        await _context.AdvanceWallets.AddAsync(advWallet);

                    await _context.SaveChangesAsync();

                    if (obj.SNDInvNum == 0)
                    {
                        #region JV Storing

                        TblFinTrnCustomerStatement custStmts = new()
                        {
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "AR",
                            Trantype = "Adv",
                            TranNumber = "0",
                            CustCode = obj.CustCode,
                            DocNum = obj.DocNum,
                            ReferenceNumber = string.Empty,
                            PaymentType = "Credit",
                            PamentCode = obj.PayCode,
                            Remarks1 = obj.Remarks,
                            Remarks2 = string.Empty,
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = obj.AdvAmount,
                            InvoiceId = null,
                            //PaymentId = obj.PaymentNumber,
                            SiteCode = obj.SiteCode
                        };

                        await _context.TrnCustomerStatements.AddAsync(custStmts);
                        await _context.SaveChangesAsync();

                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.PayCode);
                        var customer = await _context.OprCustomers.Where(e => e.CustCode == obj.CustCode).Select(e => new { e.CustArAcCode })
                                               .FirstOrDefaultAsync();
                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = customer.CustArAcCode,
                            DrAmount = 0,
                            CrAmount = obj.AdvAmount,
                            Source = "ADV",
                            Gl = string.Empty,
                            Type = "Customer",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        //Money will Come To PAYCODE
                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = payCode.FinPayAcIntgrAC,
                            DrAmount = obj.AdvAmount,
                            CrAmount = 0,
                            Source = "ADV",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);
                        await _context.SaveChangesAsync();

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        int jvSeq = 0;
                        var seqquence = await _context.Sequences.FirstOrDefaultAsync();
                        jvSeq = seqquence.JvVoucherSeq + 1;
                        seqquence.JvVoucherSeq = jvSeq;
                        _context.Sequences.Update(seqquence);
                        await _context.SaveChangesAsync();


                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            Batch = string.Empty,
                            //Source = "GL",
                            Source = "ADV",
                            Remarks = obj.Remarks,
                            Narration = obj.Notes,
                            JvDate = DateTime.Now,
                            Amount = obj.AdvAmount,
                            DocNum = advWallet.Id.ToString(),
                            CDate = DateTime.Now,
                            Approved = true,
                            ApprovedDate = DateTime.Now,
                            Posted = true,
                            Void = false,
                            PostedDate = DateTime.Now,
                            SiteCode = obj.SiteCode,
                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;

                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                                                .Where(e => e.FinBranchCode == obj.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = obj.CompanyId,
                                    BranchCode = obj.BranchCode,
                                    JvDate = DateTime.Now,
                                    //TranSource = "GL",
                                    TranSource = "AD",
                                    Trantype = "Invoice",
                                    DocNum = obj.DocNum,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = obj.Remarks,
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
                        var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Customer");
                        foreach (var obj1 in distributionsList)
                        {
                            var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                            {
                                JournalVoucherId = jvId,
                                BranchCode = obj.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = obj.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = obj.Remarks ?? obj.Notes,
                                CostAllocation = costallocations.Id,
                                CostSegCode = obj.CustCode,
                                SiteCode = obj.SiteCode
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
                                Source = "AD",
                                ExRate = 0,
                                SiteCode = obj.SiteCode,
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
                    else if (obj.SNDInvNum == 1)
                    {
                        #region JV Storing

                        TblFinTrnVendorStatement custStmts = new()
                        {
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = "AP",
                            Trantype = "Adv",
                            TranNumber = "0",
                            VendCode = obj.CustCode,
                            DocNum = obj.DocNum,
                            ReferenceNumber = string.Empty,
                            PaymentType = "Credit",
                            PamentCode = obj.PayCode,
                            Remarks1 = obj.Remarks,
                            Remarks2 = string.Empty,
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = obj.AdvAmount,
                            InvoiceId = null,
                        };

                        await _context.TrnVendorStatements.AddAsync(custStmts);
                        await _context.SaveChangesAsync();

                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == obj.PayCode);
                        var customer = await _context.VendorMasters.Where(e => e.VendCode == obj.CustCode).Select(e => new { e.VendArAcCode })
                                               .FirstOrDefaultAsync();
                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = customer.VendArAcCode,
                            DrAmount = obj.AdvAmount,
                            CrAmount = 0,
                            Source = "ADV",
                            Gl = string.Empty,
                            Type = "Vendor",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution1);

                        //Money will Come To PAYCODE
                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = null,
                            FinAcCode = payCode.FinPayAcIntgrAC,
                            DrAmount = 0,
                            CrAmount = obj.AdvAmount,
                            Source = "ADV",
                            Gl = string.Empty,
                            Type = "paycode",
                            CreatedOn = DateTime.Now
                        };
                        await _context.FinDistributions.AddAsync(distribution2);
                        await _context.SaveChangesAsync();

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        int jvSeq = 0;
                        var seqquence = await _context.Sequences.FirstOrDefaultAsync();
                        jvSeq = seqquence.JvVoucherSeq + 1;
                        seqquence.JvVoucherSeq = jvSeq;
                        _context.Sequences.Update(seqquence);
                        await _context.SaveChangesAsync();


                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            Batch = string.Empty,
                            //Source = "GL",
                            Source = "ADV",
                            Remarks = obj.Remarks,
                            Narration = obj.Notes,
                            JvDate = DateTime.Now,
                            Amount = obj.AdvAmount,
                            DocNum = advWallet.Id.ToString(),
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
                                                .Where(e => e.FinBranchCode == obj.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = obj.CompanyId,
                                    BranchCode = obj.BranchCode,
                                    JvDate = DateTime.Now,
                                    //TranSource = "GL",
                                    TranSource = "AD",
                                    Trantype = "Invoice",
                                    DocNum = obj.DocNum,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = obj.Remarks,
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
                                BranchCode = obj.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = obj.Remarks,
                                CrAmount = obj1.CrAmount ?? 0,
                                DrAmount = obj1.DrAmount ?? 0,
                                FinAcCode = obj1.FinAcCode,
                                Description = obj.Remarks ?? obj.Notes,
                                CostAllocation = costallocations.Id,
                                CostSegCode = obj.CustCode,
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
                                Source = "AD",
                                ExRate = 0,
                                SiteCode = obj.SiteCode,
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

                    #region Customer Advance Payment Storing

                    var source = obj.SNDInvNum == 0 ? "AR" : "AP";
                    TblFinTrnCustomerWallet custWallet = await _context.CustomerWallets
                                                       .FirstOrDefaultAsync(e => e.CustCode == obj.CustCode && e.Source == source) ?? new();

                    if (custWallet.CustCode.HasValue())
                    {
                        custWallet.AdvAmount = custWallet.AdvAmount + obj.AdvAmount;
                    }
                    else
                    {
                        custWallet.CustCode = obj.CustCode;
                        custWallet.Source = obj.SNDInvNum == 0 ? "AR" : "AP";
                        custWallet.AdvAmount = obj.AdvAmount;
                        custWallet.AppliedAmount = 0;
                        custWallet.PostedAmount = 0;

                    }
                    custWallet.CreatedBy = request.User.UserId;

                    if (custWallet.CustCode.HasValue())
                        _context.CustomerWallets.Update(custWallet);
                    else
                        await _context.CustomerWallets.AddAsync(custWallet);

                    await _context.SaveChangesAsync();

                    #endregion

                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, advWallet.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateOpmCustomerAdvancePaymentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion


    #region GetOpmCustomerwallet
    public class GetOpmCustomerwallet : IRequest<TblFinTrnAdvanceWalletDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public string Source { get; set; } = "AR"; //default is customer
    }

    public class GetOpmCustomerwalletHandler : IRequestHandler<GetOpmCustomerwallet, TblFinTrnAdvanceWalletDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmCustomerwalletHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnAdvanceWalletDto> Handle(GetOpmCustomerwallet request, CancellationToken cancellationToken)
        {
            return await _context.CustomerWallets.Where(e => e.CustCode == request.CustCode && e.Source == request.Source && e.AdvAmount != e.AppliedAmount).Select(e => new TblFinTrnAdvanceWalletDto
            {
                AdvAmount = (e.AdvAmount - e.AppliedAmount),
                AppliedAmount = e.AppliedAmount,
                PostedAmount = e.PostedAmount
            }).FirstOrDefaultAsync() ?? new();
        }
    }
    #endregion


    #region GetOpmAdvancepaymentWallet

    public class GetOpmAdvancepaymentWallet : IRequest<TblFinTrnAdvanceWalletDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetOpmAdvancepaymentWalletHandler : IRequestHandler<GetOpmAdvancepaymentWallet, TblFinTrnAdvanceWalletDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmAdvancepaymentWalletHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnAdvanceWalletDto> Handle(GetOpmAdvancepaymentWallet request, CancellationToken cancellationToken)
        {
            var custWallets = _context.AdvanceWallets.Where(e => e.CustCode == request.CustCode).Select(e => new TblFinTrnAdvanceWalletDto
            {
                AdvAmount = e.AdvAmount,
                AppliedAmount = e.AppliedAmount,
                PostedAmount = e.PostedAmount
            });

            if (request.SiteCode.HasValue())
                custWallets = custWallets.Where(e => e.SiteCode == request.SiteCode);

            var custWalletList = await custWallets.ToListAsync();


            return new TblFinTrnAdvanceWalletDto
            {
                AdvAmount = custWalletList.Sum(e => e.AdvAmount),
                AppliedAmount = custWalletList.Sum(e => e.AppliedAmount),
                PostedAmount = custWalletList.Sum(e => e.PostedAmount),
            };
        }
    }
    #endregion



}
