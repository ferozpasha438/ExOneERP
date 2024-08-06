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
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{

    #region GetPagedList

    public class GetCustomerPaymentList : IRequest<PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCustomerPaymentListHandler : IRequestHandler<GetCustomerPaymentList, PaginatedList<TblFinTrnCustomerPaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerPaymentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinTrnCustomerPaymentDto>> Handle(GetCustomerPaymentList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            bool isArab = request.User.Culture.IsArab();
            var list = _context.TrnCustomerPayments.AsNoTracking()
               .OrderBy(request.Input.OrderBy)
               .Select(e => new TblFinTrnCustomerPaymentDto
               {
                   Id = e.Id,
                   VoucherNumber = e.VoucherNumber,
                   CustName = isArab ? e.SndCustomerMaster.CustArbName : e.SndCustomerMaster.CustName,
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
                                EF.Functions.Like(e.CustName, "%" + search + "%")
                             )
                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return newList;
        }
    }

    #endregion


    #region GetSingleItem

    public class GetSingleItem : IRequest<TblFinTrnCustomerPaymentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSingleItemHandler : IRequestHandler<GetSingleItem, TblFinTrnCustomerPaymentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnCustomerPaymentDto> Handle(GetSingleItem request, CancellationToken cancellationToken)
        {
            var list = await _context.TrnCustomerPayments.AsNoTracking()
               .Where(e => e.Id == request.Id)
               .Select(e => new TblFinTrnCustomerPaymentDto
               {
                   Id = e.Id,
                   VoucherNumber = e.VoucherNumber,
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

            return list;
        }
    }

    #endregion


    #region GetCustomerStatementList

    public class GetCustomerStatementList : IRequest<TblFinTrnCustomerStatementItemDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetCustomerStatementListHandler : IRequestHandler<GetCustomerStatementList, TblFinTrnCustomerStatementItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerStatementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinTrnCustomerStatementItemDto> Handle(GetCustomerStatementList request, CancellationToken cancellationToken)
        {
            var list = await _context.TrnCustomerStatements.AsNoTracking()
              .Where(e => e.CustCode == request.CustomerCode)
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
            var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == request.CustomerCode);
            bool isArab = request.User.Culture.IsArab();

            return new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                CustomerName = isArab ? customer.CustArbName : customer.CustName,
                CustCode = request.CustomerCode,
                CustAddress1 = customer.CustAddress1,
                CustAddress2 = customer.CustAddress2,

                List = list,

            };
        }
    }

    #endregion




    #region GetCustomerInvoiceStatementList

    public class GetCustomerInvoiceStatementList : IRequest<List<TblFinTrnCustomerInvoiceDto>>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetCustomerInvoiceStatementListHandler : IRequestHandler<GetCustomerInvoiceStatementList, List<TblFinTrnCustomerInvoiceDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerInvoiceStatementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinTrnCustomerInvoiceDto>> Handle(GetCustomerInvoiceStatementList request, CancellationToken cancellationToken)
        {
            var list = await _context.TrnCustomerInvoices.AsNoTracking()
               //.Where(e => e.CustCode == request.CustomerCode)
               .Where(e => e.CustCode == request.CustomerCode && (e.AppliedAmount > 0 || e.Trantype != "Advance"))
              .OrderByDescending(e => e.InvoiceId)
              .ProjectTo<TblFinTrnCustomerInvoiceDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            var payments = _context.OpmCustomerPayments.AsNoTracking();

            list.ForEach(item =>
            {
                item.AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount);
            });

            return list;
        }
    }

    #endregion


    #region GetCustomerToBePaidAmount

    public class GetCustomerToBePaidAmount : IRequest<CustomerPayAmountDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetCustomerToBePaidAmountHandler : IRequestHandler<GetCustomerToBePaidAmount, CustomerPayAmountDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerToBePaidAmountHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerPayAmountDto> Handle(GetCustomerToBePaidAmount request, CancellationToken cancellationToken)
        {
            var cInvoiceItems = _context.TrnCustomerInvoices.Where(e => e.CustCode == request.CustomerCode);
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


    #region CreateCustomerPayment

    public class CreateCustomerPayment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinTrnCustomerPaymentDto Input { get; set; }
    }

    public class CreateCustomerPaymentQueryHandler : IRequestHandler<CreateCustomerPayment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateCustomerPayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateCustomerPaymentQuery method start----");

                    var obj = request.Input;
                    TblFinTrnCustomerPayment cObj = new();
                    if (obj.Id > 0)
                        cObj = await _context.TrnCustomerPayments.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    else
                    {
                        int vouSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            vouSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                VoucherSeq = vouSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            vouSeq = invSeq.VoucherSeq + 1;
                            invSeq.VoucherSeq = vouSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();
                        cObj.VoucherNumber = vouSeq;

                    }

                    cObj.CompanyId = obj.CompanyId;
                    cObj.BranchCode = obj.BranchCode;
                    cObj.TranDate = obj.TranDate;
                    cObj.CustCode = obj.CustCode;
                    cObj.PayType = obj.PayType;
                    cObj.PayCode = obj.PayCode;
                    cObj.Remarks = obj.Remarks;
                    cObj.Amount = obj.Amount;
                    cObj.DocNum = "DocNum";
                    cObj.Narration = obj.Narration;
                    cObj.Preparedby = obj.Preparedby;
                    cObj.SiteCode= obj.SiteCode;

                    if (((int)PayCodeTypeEnum.Bank).ToString() == obj.PayType)
                    {
                        cObj.CheckNumber = obj.CheckNumber;
                        cObj.Checkdate = obj.CheckDate;
                    }

                    if (obj.Id > 0)
                    {
                        _context.TrnCustomerPayments.Update(cObj);
                    }
                    else
                    {

                        await _context.TrnCustomerPayments.AddAsync(cObj);
                    }
                    await _context.SaveChangesAsync();
                    Log.Info("----Info CreateCustomerPaymentQuery method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, cObj.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateCustomerPaymentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion


    #region CustomerPaymentApproval

    public class CustomerPaymentApproval : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerPaymentApprovalDto Input { get; set; }
    }

    public class CustomerPaymentApprovalQueryHandler : IRequestHandler<CustomerPaymentApproval, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CustomerPaymentApprovalQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CustomerPaymentApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CustomerPaymentApprovalQuery method start----");

                    string CustomerCode = request.Input.CustomerCode;
                    var cPayment = await _context.TrnCustomerPayments.FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.Id == request.Input.Id);
                    var cInvoices = await _context.TrnCustomerInvoices.FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.NetAmount != e.PaidAmount);

                    decimal? amount = cPayment.Amount;
                    bool isLeastCredit = false;
                    //var custInvoices = await _context.TrnCustomerInvoices.OrderBy(e => e.Id)
                    //    .Where(e => e.InvoiceAmount != e.PaidAmount);

                    var input = await _context.TrnCustomerStatements
                        .Where(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.DrAmount != e.CrAmount)
                        .ToListAsync();

                    foreach (var item in input)
                    {
                        if (isLeastCredit)
                            break;

                        if (item.CrAmount is not null && item.CrAmount > 0)
                        {
                            if (amount >= (item.DrAmount - item.CrAmount))
                            {
                                amount = amount - (item.DrAmount - item.CrAmount);
                                item.CrAmount = item.CrAmount + (item.DrAmount - item.CrAmount);
                            }
                            else if (amount < (item.DrAmount - item.CrAmount))
                            {
                                item.CrAmount = item.CrAmount + amount;
                                isLeastCredit = true;
                            }
                        }
                        //   1250
                        //800 = 200
                        else
                        {
                            if (amount >= item.DrAmount)
                            {
                                item.CrAmount = item.DrAmount;
                                amount = amount - item.DrAmount;
                            }
                            else
                            {
                                item.CrAmount = amount;
                                isLeastCredit = true;
                            }
                        }
                        //item.Id = 0;
                        //item.Trantype = "Payment";

                        _context.TrnCustomerStatements.Update(item);
                        await _context.SaveChangesAsync();
                    }
                    cPayment.IsPaid = true;
                    _context.TrnCustomerPayments.Update(cPayment);
                    await _context.SaveChangesAsync();

                    Log.Info("----Info CustomerPaymentApprovalQuery method ends----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CustomerPaymentApprovalQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion


    #region GetCustomerpaymentvoucher

    public class GetCustomerpaymentvoucher : IRequest<CustomerReceiptVoucherDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCustomerpaymentvoucherHandler : IRequestHandler<GetCustomerpaymentvoucher, CustomerReceiptVoucherDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerpaymentvoucherHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerReceiptVoucherDto> Handle(GetCustomerpaymentvoucher request, CancellationToken cancellationToken)
        {
            var item = await _context.TrnCustomerPayments.AsNoTracking()
              .Where(e => e.Id == request.Id)
            .Select(e => new CustomerReceiptVoucherDto
            {
                VoucherNumber = e.VoucherNumber,
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

            return item;
        }
    }

    #endregion


    #region NoUse CustomerPaymentApprovalOne

    //public class CustomerPaymentApprovalOne : IRequest<AppCtrollerDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public CustomerPaymentApprovalDto Input { get; set; }
    //}

    //public class CustomerPaymentApprovalOneQueryHandler : IRequestHandler<CustomerPaymentApprovalOne, AppCtrollerDto>
    //{
    //    //private readonly ICurrentUserService _currentUserService;
    //    //protected string UserId => _currentUserService.UserId;
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CustomerPaymentApprovalOneQueryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<AppCtrollerDto> Handle(CustomerPaymentApprovalOne request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info CustomerPaymentApprovalOneQuery method start----");

    //                string CustomerCode = request.Input.CustomerCode;
    //                var cPayment = await _context.TrnCustomerPayments.FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.Id == request.Input.Id);
    //                var cInvoices = _context.TrnCustomerInvoices.Where(e => e.CustCode == CustomerCode && e.NetAmount != e.PaidAmount);

    //                if (!(await cInvoices.AnyAsync()))
    //                    return ApiMessageInfo.Status(2);

    //                var cInvoiceIds = await cInvoices.Select(e => e.InvoiceId).ToListAsync();

    //                decimal? amount = cPayment.Amount;
    //                bool isLeastCredit = false;

    //                foreach (var invId in cInvoiceIds)
    //                {
    //                    if (isLeastCredit)
    //                        break;

    //                    var invItem = await _context.TrnCustomerStatements
    //                   .FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Invoice" && e.InvoiceId == invId);

    //                    var pItem = await _context.TrnCustomerStatements
    //                 .FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Payment" && e.InvoiceId == invId);

    //                    var pItemSum = await _context.TrnCustomerStatements
    //                   .Where(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Payment" && e.InvoiceId == invId)
    //                   .SumAsync(e => e.CrAmount);

    //                    if (pItem is not null && pItemSum < invItem.DrAmount)
    //                    {
    //                        if (amount >= (invItem.DrAmount - pItem.CrAmount))
    //                        {
    //                            amount = amount - (invItem.DrAmount - pItem.CrAmount);
    //                            pItem.CrAmount = pItem.CrAmount + (invItem.DrAmount - pItem.CrAmount);

    //                            var cInvoice = await cInvoices.FirstOrDefaultAsync(e => e.InvoiceId == invId);
    //                            cInvoice.PaidAmount = invItem.DrAmount;

    //                            _context.TrnCustomerInvoices.Update(cInvoice);
    //                            await _context.SaveChangesAsync();

    //                        }
    //                        else if (amount < (invItem.DrAmount - pItem.CrAmount))
    //                        {
    //                            pItem.CrAmount = pItem.CrAmount + amount;
    //                            isLeastCredit = true;
    //                        }
    //                        _context.TrnCustomerStatements.Update(pItem);
    //                        await _context.SaveChangesAsync();
    //                    }
    //                    else
    //                    {
    //                        TblFinTrnCustomerStatement cStatement = new()
    //                        {
    //                            CompanyId = (int)invItem.CompanyId,
    //                            BranchCode = invItem.BranchCode,
    //                            TranDate = DateTime.Now,
    //                            TranSource = invItem.TranSource,
    //                            Trantype = "Payment",
    //                            TranNumber = invItem.TranNumber,
    //                            CustCode = invItem.CustCode,
    //                            DocNum = invItem.DocNum,
    //                            ReferenceNumber = invItem.ReferenceNumber,
    //                            PaymentType = invItem.PaymentType,
    //                            PamentCode = invItem.PamentCode,
    //                            CheckNumber = invItem.CheckNumber,
    //                            Remarks1 = invItem.Remarks1,
    //                            Remarks2 = invItem.Remarks2,
    //                            LoginId = invItem.LoginId,
    //                            DrAmount = 0,
    //                            InvoiceId = invItem.InvoiceId,
    //                        };

    //                        if (amount >= invItem.DrAmount)
    //                        {
    //                            cStatement.CrAmount = invItem.DrAmount;
    //                            amount = amount - invItem.DrAmount;

    //                            var cInvoice = await cInvoices.FirstOrDefaultAsync(e => e.InvoiceId == invId);
    //                            cInvoice.PaidAmount = invItem.DrAmount;

    //                            _context.TrnCustomerInvoices.Update(cInvoice);
    //                            await _context.SaveChangesAsync();
    //                        }
    //                        else
    //                        {
    //                            cStatement.CrAmount = amount;
    //                            isLeastCredit = true;
    //                        }

    //                        await _context.TrnCustomerStatements.AddAsync(cStatement);
    //                        await _context.SaveChangesAsync();
    //                    }
    //                }


    //                cPayment.IsPaid = true;
    //                _context.TrnCustomerPayments.Update(cPayment);
    //                await _context.SaveChangesAsync();

    //                Log.Info("----Info CustomerPaymentApprovalOneQuery method ends----");
    //                await transaction.CommitAsync();
    //                return ApiMessageInfo.Status(1, 1);
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in CustomerPaymentApprovalOneQuery Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return ApiMessageInfo.Status(0);
    //            }
    //        }
    //    }
    //}

    #endregion


    #region CustomerPaymentApprovalTwo

    public class CustomerPaymentApprovalTwo : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerPaymentApprovalDto Input { get; set; }
    }

    public class CustomerPaymentApprovalTwoQueryHandler : IRequestHandler<CustomerPaymentApprovalTwo, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CustomerPaymentApprovalTwoQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CustomerPaymentApprovalTwo request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CustomerPaymentApprovalTwoQuery method start----");

                    string CustomerCode = request.Input.CustomerCode;
                    var cPayment = await _context.TrnCustomerPayments.Include(e => e.SndCustomerMaster).FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.Id == request.Input.Id);

                    var cInvoiceItems = _context.TrnCustomerInvoices.Where(e => e.CustCode == CustomerCode);
                    var cInvoices = cInvoiceItems.Where(e => e.NetAmount != e.PaidAmount);
                    var cInvoiceAppliedAmountList = await cInvoiceItems.Where(e => e.Trantype == "Advance" && e.AppliedAmount > 0).ToListAsync();


                    //if (!(await cInvoices.AnyAsync()))
                    //    return ApiMessageInfo.Status(2);

                    var cInvoiceIds = await cInvoices.Select(e => e.InvoiceId).ToListAsync();
                    var AppliedAmount = cInvoiceAppliedAmountList.Sum(e => e.AppliedAmount);
                    decimal? amount = cPayment.Amount + AppliedAmount;
                    bool isLeastCredit = false;

                    //var invcItem = await _context.TrnCustomerStatements
                    //  .FirstOrDefaultAsync(e => e.CustCode == CustomerCode && e.PaymentType.ToLower() == "credit" && e.Trantype == "Invoice");

                    TblFinTrnCustomerStatement cStatement = new()
                    {
                        CompanyId = (int)cPayment.CompanyId,
                        BranchCode = cPayment.BranchCode,
                        TranDate = DateTime.Now,
                        TranSource = "AR",
                        Trantype = "Payment",
                        TranNumber = "0",
                        CustCode = cPayment.CustCode,
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
                        PaymentId = cPayment.VoucherNumber,
                    };
                    await _context.TrnCustomerStatements.AddAsync(cStatement);
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
                        _context.TrnCustomerInvoices.Update(cInvoice);
                        await _context.SaveChangesAsync();
                    }


                    //Updating Applied amount to zero
                    foreach (var apItem in cInvoiceAppliedAmountList)
                    {
                        apItem.AppliedAmount = 0;
                        apItem.IsPaid = true;
                        _context.TrnCustomerInvoices.Update(apItem);
                        await _context.SaveChangesAsync();
                    }

                    //_context.TrnCustomerInvoices.RemoveRange(cInvoiceAppliedAmountList);
                    //await _context.SaveChangesAsync();


                    if (amount > 0)
                    {
                        var obj = await cInvoiceItems.FirstOrDefaultAsync();
                        TblFinTrnCustomerInvoice cInvoice = new()
                        {
                            CompanyId = (int)obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            InvoiceNumber = "0",
                            InvoiceDate = obj.InvoiceDate,
                            CreditDays = 0,
                            DueDate = DateTime.Now,
                            TranSource = obj.TranSource,
                            Trantype = "Advance",
                            CustCode = obj.CustCode,
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
                        await _context.TrnCustomerInvoices.AddAsync(cInvoice);
                    }

                    cPayment.IsPaid = true;
                    _context.TrnCustomerPayments.Update(cPayment);
                    await _context.SaveChangesAsync();


                    var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == cPayment.PayCode);

                    TblFinTrnDistribution distribution1 = new()
                    {
                        InvoiceId = null,
                        FinAcCode = cPayment.SndCustomerMaster.CustArAcCode,
                        CrAmount = cPayment.Amount,
                        DrAmount = 0,
                        Source = "AR",
                        Gl = string.Empty,
                        Type = "Customer",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution1);
                    await _context.SaveChangesAsync();

                    TblFinTrnDistribution distribution2 = new()
                    {
                        InvoiceId = null,
                        FinAcCode = payCode.FinPayAcIntgrAC,
                        CrAmount = 0,
                        DrAmount = cPayment.Amount,
                        Source = "AR",
                        Gl = string.Empty,
                        Type = "paycode",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution2);
                    await _context.SaveChangesAsync();




                    /*  updating last paid and payment date    */

                    var custAmt = _context.TrnCustomerStatements.Where(e => e.CustCode == CustomerCode);
                    var custInvAmt = (await custAmt.SumAsync(e => e.DrAmount) - await custAmt.SumAsync(e => e.CrAmount));
                    var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                    customer.CustOutStandBal = custInvAmt;
                    customer.CustLastPaidDate = DateTime.Now;
                    customer.CustLastPayAmt = cPayment.Amount ?? 0;
                    _context.OprCustomers.Update(customer);
                    await _context.SaveChangesAsync();

                    //var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerCode);
                    //customer.CustLastPaidDate = DateTime.Now;
                    //customer.CustLastPayAmt = cPayment.Amount ?? 0;
                    //_context.OprCustomers.Update(customer);
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
                        Source = "AR",
                        Remarks = cPayment.Remarks,
                        Narration = cPayment.Narration,
                        JvDate = DateTime.Now,
                        Amount = cPayment.Amount ?? 0,
                        DocNum = cPayment.DocNum,
                        CDate = DateTime.Now,
                        Posted = true,
                        PostedDate = DateTime.Now,
                        SiteCode = cPayment.SiteCode
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
                                //TranSource = "GL",
                                TranSource = "AR",
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
                    var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Customer");
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
                            CostSegCode = customer.CustCode,
                            SiteCode = cPayment.SiteCode
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
                            Source = "RP",
                            ExRate = 0,
                            SiteCode = cPayment.SiteCode,
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


                    Log.Info("----Info CustomerPaymentApprovalTwoQuery method ends----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CustomerPaymentApprovalTwoQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteCustomerPayment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCustomerPaymentQueryHandler : IRequestHandler<DeleteCustomerPayment, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerPaymentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCustomerPayment request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");

                if (request.Id > 0)
                {
                    var CPayment = await _context.TrnCustomerPayments.FirstOrDefaultAsync(e => e.Id == request.Id);
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
