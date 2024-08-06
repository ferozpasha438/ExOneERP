using AutoMapper;
using CIN.Application.Common;
//using CIN.Application.InvoiceDtos;
using CIN.Application.PurchaseMgtDtos;
//using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.PurchaseMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.PurchasemgtQuery
{
    #region GetPagedList

    public class GetApSalesInvoiceList : IRequest<PaginatedList<TblTranVenInvoiceListDto>>
    {
        public UserIdentityDto User { get; set; }
        public int InvoiceStatusId { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetApSalesInvoiceListHandler : IRequestHandler<GetApSalesInvoiceList, PaginatedList<TblTranVenInvoiceListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetApSalesInvoiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblTranVenInvoiceListDto>> Handle(GetApSalesInvoiceList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetApSalesInvoiceList method start----");
            var invoices = _context.TranPurcInvoices.AsNoTracking();//.Where(e => e.BranchCode == request.User.BranchCode);
            var companies = _context.Companies.AsNoTracking();
            var vendors = _context.VendorMasters.AsNoTracking();
            var vendApproval = _context.TrnVendorApprovals.AsNoTracking();
            var cInvoices = _context.TrnVendorInvoices.AsNoTracking();
            // var users = _context.SystemLogins.Select(e => new { e.Id, e.PrimaryBranch });
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();

            //var customers3 = customers.Where(c => EF.Functions.Like(c.NameAR, "%" + request.Input.Query + "%"));                    

            bool isDate = DateTime.TryParse(request.Input.Query, out DateTime invoiceDate);
            var reports = from IV in invoices
                          join CM in companies
                          on IV.CompanyId equals CM.Id
                          join CS in vendors
                          on IV.CustomerId equals CS.Id

                          //into AP_Left
                          //from AP in AP_Left.DefaultIfEmpty()
                          //join AP in approvals
                          //on IV.Id equals AP.CreditId
                          where //CM.Id == request.User.CompanyId &&
                          IV.InvoiceStatusId == request.InvoiceStatusId &&
                          (
                     CS.VendName.Contains(request.Input.Query) ||
                     EF.Functions.Like(CS.VendArbName, "%" + request.Input.Query + "%")
                     ||
                     IV.CreditNumber.Contains(request.Input.Query) || (isDate && IV.InvoiceDate == invoiceDate)
                          )
                          select new TblTranVenInvoiceListDto
                          {
                              Id = IV.Id,
                              //SpCreditNumber = IV.SpCreditNumber,
                              CreditNumber = IV.CreditNumber == string.Empty ? IV.SpCreditNumber : IV.CreditNumber,
                              InvoiceDate = IV.InvoiceDate,
                              CreatedOn = IV.CreatedOn,
                              InvoiceDueDate = IV.InvoiceDueDate,
                              CompanyId = (int)IV.CompanyId,
                              CustomerId = (long)IV.CustomerId,
                              SubTotal = (decimal)IV.SubTotal,
                              DiscountAmount = (decimal)IV.DiscountAmount,
                              AmountBeforeTax = (decimal)IV.AmountBeforeTax,
                              TaxAmount = (decimal)IV.TaxAmount,
                              TotalAmount = (decimal)IV.TotalAmount,
                              TotalPayment = (decimal)IV.TotalPayment,
                              AmountDue = (decimal)IV.AmountDue,
                              VatPercentage = (decimal)IV.VatPercentage,
                              CompanyName = CM.CompanyName,
                              BranchName = IV.SysCompanyBranch.BranchName,
                              CustomerName = CS.VendName,
                              InvoiceRefNumber = IV.InvoiceRefNumber,
                              LpoContract = IV.LpoContract,
                              PaymentTermId = IV.SndPoTermsCode.POTermsName,
                              TaxIdNumber = IV.TaxIdNumber,
                              InvoiceStatus = IV.IsCreditConverted ? "Credit" : "Invoice",
                              ApprovedUser = vendApproval.Any(e => e.LoginId == request.User.UserId && e.InvoiceId == IV.Id && e.IsApproved),
                              IsApproved = vendApproval.Where(e => e.InvoiceId == IV.Id && e.IsApproved).Any(),
                              //CanSettle = brnApproval.Where(e => e.FinBranchCode == IV.BranchCode).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= vendApproval.Where(e => e.InvoiceId == IV.Id && e.IsApproved).Count(),
                              IsSettled = cInvoices.Where(e => e.InvoiceId == IV.Id && e.IsPaid).Any(),
                              HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == IV.BranchCode && e.AppAuthAP),
                              //HasAuthority=true,
                              CanSettle = true,
                              //IsSettled = false,

                          };


            if (!string.IsNullOrEmpty(request.Input.Approval))
            {
                string aprv = request.Input.Approval;

                reports = aprv switch
                {
                    "settled" => reports.Where(e => e.IsSettled),
                    "unsettled" => reports.Where(e => !e.IsSettled),
                    "approval" => reports.Where(e => e.IsApproved),
                    "unapproval" => reports.Where(e => !e.IsApproved),
                    _ => reports
                };
            }

            //.OrderByDescending(t => t.CustomerId)
            // .ProjectTo<TblTranVenInvoiceDto>(_mapper.ConfigurationProvider)
            var nreports = await reports.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            Log.Info("----Info GetApSalesInvoiceList method Exit----");
            return nreports;
        }

    }

    #endregion


    #region GetSingleCreditApInvoiceById

    public class GetSingleCreditApInvoiceById : IRequest<TblTranPurcInvoiceDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int InvoiceStatusId { get; set; }
    }

    public class GetSingleCreditApInvoiceByIdHandler : IRequestHandler<GetSingleCreditApInvoiceById, TblTranPurcInvoiceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleCreditApInvoiceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblTranPurcInvoiceDto> Handle(GetSingleCreditApInvoiceById request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetSingleCreditApInvoiceById method start----");
            var invoice = await _context.TranPurcInvoices.Include(e => e.SysCompanyBranch).ThenInclude(e => e.SysCompany)
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            var items = await _context.TranPurcInvoiceItems
                .Where(e => e.CreditId == request.Id)
                .Include(e => e.ItemCode)
                .ToListAsync();



            //var products = _context.TranProducts.Include(e=>e.UnitType).AsNoTracking();//.Where(e=> items.Any(ivt=>ivt.ProductId == e.ProductTypeId)).ToListAsync();
            //var unitTypes = _context.TranUnitTypes.AsNoTracking();

            //var ivItems = await (from iv in items
            //                     join pd in products
            //                     on iv.ProductId equals pd.Id
            //                     into P_Left
            //                     from PL in P_Left.DefaultIfEmpty()
            //                     join ut in unitTypes
            //                     on PL.UnitTypeId equals (int)ut.Id
            //                     select new { iv, PL, ut }).ToListAsync();

            int invoiceStatus = 1;

            if (request.InvoiceStatusId == (int)InvoiceStatusIdType.Credit)
                invoiceStatus = -1;

            TblTranPurcInvoiceDto invoiceDto = new()
            {
                CustomerId = invoice.CustomerId,
                InvoiceDate = invoice.InvoiceDate,
                InvoiceDueDate = invoice.InvoiceDueDate,
                CompanyId = invoice.CompanyId,
                BranchCode = invoice.BranchCode,
                InvoiceRefNumber = invoice.InvoiceRefNumber,
                LpoContract = invoice.LpoContract,
                PaymentTerms = invoice.PaymentTerms,
                PaymentTermId = invoice.PaymentTerms,
                CreditNumber = invoice.CreditNumber,
                ServiceDate1 = invoice.ServiceDate1,
                IsCreditConverted = invoice.IsCreditConverted,
                InvoiceStatusId = invoice.InvoiceStatusId,

                SubTotal = invoice.SubTotal,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                DiscountAmount = invoice.DiscountAmount,

                CreatedOn = invoice.CreatedOn,
                TaxIdNumber = invoice.TaxIdNumber,
                InvoiceNotes = invoice.InvoiceNotes,
                Remarks = invoice.Remarks,
                //LogoImagePath = invoice.SysCompanyBranch.SysCompany.LogoURL
            };

            List<TblTranPurcInvoiceItemDto> itemList = new();

            items.ForEach(item =>
            {
                TblTranPurcInvoiceItemDto itemDto = new()
                {
                    ItemCode = item.ItemCode,
                    ProductName = item.InvItemMaster.ShortName,
                    Description = item.Description,
                    Quantity = invoiceStatus * item.Quantity,
                    UnitType = item.InvItemMaster.ItemBaseUnit,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    DiscountAmount = item.DiscountAmount,

                    TaxTariffPercentage = item.TaxTariffPercentage,
                    TaxAmount = invoiceStatus * item.TaxAmount,
                    TotalAmount = invoiceStatus * item.TotalAmount
                };

                itemList.Add(itemDto);
            });

            //items.ForEach(item =>
            //{
            //    TblTranVenInvoiceItemDto itemDto = new()
            //    {
            //        ProductId = item.iv.ProductId,
            //        ProductName = item.PL.NameEN,
            //        Description = item.iv.Description,
            //        Quantity = invoiceStatus * item.iv.Quantity,
            //        UnitType = item.ut.NameEN,
            //        UnitPrice = item.iv.UnitPrice,
            //        TaxTariffPercentage = item.iv.TaxTariffPercentage,
            //        TaxAmount = invoiceStatus * item.iv.TaxAmount,
            //        TotalAmount = invoiceStatus * item.iv.TotalAmount
            //    };

            //    itemList.Add(itemDto);
            //});


            invoiceDto.ItemList = itemList;

            Log.Info("----Info GetSingleCreditApInvoiceById method Exit----");
            return invoiceDto;
        }

    }

    #endregion



    #region CreateUpdateInvoice

    public class CreateApInvoice : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblTranPurcInvoiceDto Input { get; set; }


    }
    public class CreateApInvoiceQueryHandler : IRequestHandler<CreateApInvoice, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateApInvoiceQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateApInvoice request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateInvoice method start----");

                    string SpCreditNumber = $"S{new Random().Next(99, 9999999).ToString()}";

                    //invoiceNumber = await _context.TranVenInvoices.CountAsync();
                    //invoiceNumber += 1;
                    TblTranPurcInvoice Invoice = new();
                    var obj = request.Input;

                    if (request.Input.Id > 0)
                    {
                        Invoice = await _context.TranPurcInvoices.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        SpCreditNumber = Invoice.SpCreditNumber;

                        Invoice.CustomerId = obj.CustomerId;
                        Invoice.InvoiceDate = Convert.ToDateTime(obj.InvoiceDate);
                        Invoice.InvoiceDueDate = Convert.ToDateTime(obj.InvoiceDueDate);
                        Invoice.ServiceDate1 = obj.ServiceDate1;
                        Invoice.CompanyId = obj.CompanyId;
                        Invoice.BranchCode = obj.BranchCode;
                        Invoice.InvoiceRefNumber = obj.InvoiceRefNumber;


                        Invoice.LpoContract = obj.LpoContract;
                        Invoice.PaymentTerms = obj.PaymentTermId;
                        Invoice.TaxIdNumber = obj.TaxIdNumber;
                        Invoice.InvoiceNotes = obj.InvoiceNotes;
                        Invoice.Remarks = obj.Remarks;


                        Invoice.SubTotal = obj.SubTotal;
                        Invoice.DiscountAmount = obj.DiscountAmount ?? 0;
                        Invoice.AmountBeforeTax = obj.AmountBeforeTax ?? 0;
                        Invoice.TaxAmount = obj.TaxAmount;
                        Invoice.TotalAmount = obj.TotalAmount;
                        Invoice.TotalPayment = obj.TotalPayment ?? 0;
                        Invoice.AmountDue = obj.AmountDue ?? 0;


                        var items = _context.TranPurcInvoiceItems.Where(e => e.CreditId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.TranPurcInvoices.Update(Invoice);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (obj.TotalAmount is null)
                            return ApiMessageInfo.Status(0);

                        Invoice = new()
                        {
                            SpCreditNumber = SpCreditNumber,
                            CreditNumber = string.Empty,
                            InvoiceDate = Convert.ToDateTime(obj.InvoiceDate),
                            InvoiceDueDate = Convert.ToDateTime(obj.InvoiceDueDate),
                            CompanyId = obj.CompanyId,


                            SubTotal = obj.SubTotal,
                            DiscountAmount = obj.DiscountAmount ?? 0,
                            AmountBeforeTax = obj.AmountBeforeTax ?? 0,
                            TaxAmount = obj.TaxAmount,
                            TotalAmount = obj.TotalAmount,
                            TotalPayment = obj.TotalPayment ?? 0,
                            AmountDue = obj.AmountDue ?? 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(obj.InvoiceDate),
                            CreatedBy = request.User.UserId,
                            CustomerId = obj.CustomerId,
                            InvoiceStatus = "Open",
                            TaxIdNumber = obj.TaxIdNumber,
                            InvoiceModule = obj.InvoiceModule,
                            InvoiceNotes = obj.InvoiceNotes,
                            Remarks = obj.Remarks,
                            InvoiceRefNumber = obj.InvoiceRefNumber,
                            LpoContract = obj.LpoContract,
                            VatPercentage = obj.VatPercentage ?? 0,
                            PaymentTerms = obj.PaymentTermId,
                            BranchCode = obj.BranchCode,
                            ServiceDate1 = obj.ServiceDate1,
                            IsCreditConverted = obj.IsCreditConverted,
                            InvoiceStatusId = obj.InvoiceStatusId
                        };

                        await _context.TranPurcInvoices.AddAsync(Invoice);
                        await _context.SaveChangesAsync();
                    }
                    //var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.CompanyId);
                    //if (company is not null)
                    //{
                    //    company.InvoiceSeq++;
                    //    await _context.SaveChangesAsync();
                    //}

                    Log.Info("----Info CreateUpdateInvoice method Exit----");

                    var inoviceId = Invoice.Id;
                    var invoiceItems = request.Input.ItemList;
                    if (invoiceItems.Count > 0)
                    {
                        List<TblTranPurcInvoiceItem> invoiceItemsList = new();

                        foreach (var obj1 in invoiceItems)
                        {
                            var InvoiceItem = new TblTranPurcInvoiceItem
                            {
                                CreditId = inoviceId,
                                CreditNumber = SpCreditNumber,
                                ItemCode = obj1.ItemCode,
                                Quantity = obj1.Quantity,
                                UnitPrice = obj1.UnitPrice,
                                SubTotal = obj1.SubTotal,
                                DiscountAmount = obj1.DiscountAmount,
                                AmountBeforeTax = obj1.AmountBeforeTax,
                                TaxAmount = obj1.TaxAmount,
                                TotalAmount = obj1.TotalAmount,
                                IsDefaultConfig = true,
                                CreatedOn = Convert.ToDateTime(obj.InvoiceDate),
                                CreatedBy = (int)request.UserId,
                                Description = obj1.Description,
                                TaxTariffPercentage = obj1.TaxTariffPercentage,
                                Discount = obj1.Discount ?? 0
                            };
                            invoiceItemsList.Add(InvoiceItem);
                        }
                        if (invoiceItemsList.Count > 0)
                        {
                            await _context.TranPurcInvoiceItems.AddRangeAsync(invoiceItemsList);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, inoviceId);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateInvoice Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region CreateApInvoiceApproval
    public class CreateApInvoiceApproval : UserIdentityDto, IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceApprovalDto Input { get; set; }
    }
    public class CreateApInvoiceApprovalQueryHandler : IRequestHandler<CreateApInvoiceApproval, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateApInvoiceApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> Handle(CreateApInvoiceApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateApInvoiceApproval method start----");

                    var obj = await _context.TranPurcInvoices.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                    var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);

                    if (await _context.TrnVendorApprovals.AnyAsync(e => e.InvoiceId == request.Input.Id && e.LoginId == request.User.UserId && e.IsApproved))
                        return true;

                    TblFinTrnVendorApproval approval = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        TranDate = DateTime.Now,
                        TranSource = request.Input.TranSource,
                        Trantype = request.Input.Trantype,
                        VendCode = customer.VendCode,
                        DocNum = "DocNum",
                        LoginId = request.User.UserId,
                        AppRemarks = request.Input.AppRemarks,
                        InvoiceId = request.Input.Id,
                        IsApproved = true,
                    };

                    await _context.TrnVendorApprovals.AddAsync(approval);
                    await _context.SaveChangesAsync();

                    if (!obj.CreditNumber.HasValue())
                    {
                        int creditSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                        if (invSeq is null)
                        {
                            creditSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CreditSeq = creditSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            creditSeq = invSeq.CreditSeq + 1;
                            invSeq.CreditSeq = creditSeq;
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        obj.CreditNumber = creditSeq.ToString();
                        obj.SpCreditNumber = string.Empty;
                        _context.TranPurcInvoices.Update(obj);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateApInvoiceApproval Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return false;
                }
            }
        }
    }
    #endregion


    #region CreateApInvoiceSettlement
    public class CreateApInvoiceSettlement : UserIdentityDto, IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public TblTranInvoiceSettlementDto Input { get; set; }
    }
    public class CreateApInvoiceSettlementQueryHandler : IRequestHandler<CreateApInvoiceSettlement, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateApInvoiceSettlementQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> Handle(CreateApInvoiceSettlement request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateApInvoiceSettlement method start----");

                    var input = request.Input;
                    var obj = await _context.TranPurcInvoices.FirstOrDefaultAsync(e => e.Id == input.Id);
                    var customer = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);
                    var paymentTerms = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.POTermsCode == obj.PaymentTerms);

                    if (await _context.TrnVendorInvoices.AnyAsync(e => e.InvoiceId == input.Id && e.LoginId == request.User.UserId && e.IsPaid))
                        return true;


                    if (!obj.CreditNumber.HasValue())
                    {
                        int invoiceSeq = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync();

                        if (invSeq is null)
                        {
                            invoiceSeq = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                CreditSeq = invoiceSeq
                            };
                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            invoiceSeq = invSeq.CreditSeq + 1;
                            invSeq.CreditSeq = invoiceSeq;
                            _context.Sequences.Update(invSeq);
                        }

                        obj.CreditNumber = invoiceSeq.ToString();
                        obj.SpCreditNumber = string.Empty;

                    }

                    obj.InvoiceStatus = "Closed";
                    _context.TranPurcInvoices.Update(obj);

                    await _context.SaveChangesAsync();


                    TblFinTrnVendorInvoice cInvoice = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        InvoiceNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                        InvoiceDate = obj.InvoiceDate,
                        CreditDays = paymentTerms.POTermsDueDays,
                        DueDate = obj.InvoiceDueDate,
                        TranSource = input.TranSource,
                        Trantype = input.Trantype,
                        VendCode = customer.VendCode,
                        DocNum = obj.InvoiceRefNumber,
                        LoginId = request.User.UserId,
                        ReferenceNumber = obj.InvoiceRefNumber,
                        InvoiceAmount = obj.TotalAmount,
                        DiscountAmount = obj.DiscountAmount ?? 0,
                        NetAmount = obj.TotalAmount - obj.DiscountAmount ?? 0,
                        PaidAmount = Utility.IsNotCreditPay(request.Input.PaymentType) ? obj.TotalAmount : 0,
                        AppliedAmount = 0,
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
                        InvoiceId = input.Id,
                        IsPaid = true,
                    };
                    cInvoice.BalanceAmount = cInvoice.NetAmount - cInvoice.PaidAmount;
                    await _context.TrnVendorInvoices.AddAsync(cInvoice);

                    TblFinTrnVendorStatement cStatement = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        TranDate = DateTime.Now,
                        TranSource = input.TranSource,
                        Trantype = input.Trantype,
                        TranNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                        VendCode = customer.VendCode,
                        DocNum = "DocNum",
                        ReferenceNumber = obj.InvoiceRefNumber,
                        PaymentType = input.PaymentType,
                        PamentCode = "paycode",
                        CheckNumber = input.CheckNumber,
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
                        LoginId = request.User.UserId,
                        DrAmount = !obj.IsCreditConverted ? obj.TotalAmount : 0,
                        CrAmount = obj.IsCreditConverted ? obj.TotalAmount : 0,
                        InvoiceId = input.Id,
                    };
                    await _context.TrnVendorStatements.AddAsync(cStatement);


                    if (IsNotCreditPay(request.Input.PaymentType))
                    {
                        TblFinTrnVendorStatement cPaymentStatement = new()
                        {
                            CompanyId = (int)obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = DateTime.Now,
                            TranSource = input.TranSource,
                            Trantype = "Payment",
                            TranNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                            VendCode = customer.VendCode,
                            DocNum = "DocNum",
                            ReferenceNumber = obj.InvoiceRefNumber,
                            PaymentType = input.PaymentType,
                            PamentCode = "Paycode",
                            CheckNumber = input.CheckNumber,
                            Remarks1 = input.Remarks1,
                            Remarks2 = input.Remarks2,
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = obj.TotalAmount,
                            InvoiceId = input.Id,
                        };
                        await _context.TrnVendorStatements.AddAsync(cPaymentStatement);
                    }

                    //obj.CreditNumber = invoiceSeq.ToString();
                    //obj.SpCreditNumber = string.Empty;
                    //_context.TranVenInvoices.Update(obj);



                    //storing in TblFinTrnDistribution tables

                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);
                    var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinBranchCode == obj.BranchCode);

                    TblFinTrnDistribution distribution1 = new()
                    {
                        InvoiceId = input.Id,
                        FinAcCode = IsNotCreditPay(request.Input.PaymentType) ? payCode.FinPayAcIntgrAC : vendor.VendArAcCode,
                        CrAmount = !obj.IsCreditConverted ? obj.TotalAmount : 0,
                        DrAmount = obj.IsCreditConverted ? obj.TotalAmount : 0,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = IsNotCreditPay(request.Input.PaymentType) ? "paycode" : "Vendor",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution1);

                    TblFinTrnDistribution distribution2 = new()
                    {
                        InvoiceId = input.Id,
                        FinAcCode = vendor.VendDefExpAcCode,
                        CrAmount = obj.IsCreditConverted ? (obj.TotalAmount - obj.TaxAmount) : 0,
                        DrAmount = !obj.IsCreditConverted ? (obj.TotalAmount - obj.TaxAmount) : 0,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = "Income",
                        CreatedOn = DateTime.Now
                    };
                    await _context.FinDistributions.AddAsync(distribution2);


                    var invoiceItem = await _context.TranPurcInvoiceItems.FirstOrDefaultAsync(e => e.CreditId == obj.Id);
                    var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.TaxTariffPercentage).ToString());

                    if (tax is null)
                        throw new NullReferenceException("Tax is empty");

                    TblFinTrnDistribution distribution3 = new()
                    {
                        InvoiceId = input.Id,
                        FinAcCode = tax?.InputAcCode01,
                        CrAmount = obj.IsCreditConverted ? obj.TaxAmount : 0,
                        DrAmount = !obj.IsCreditConverted ? obj.TaxAmount : 0,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = "VAT",
                        CreatedOn = DateTime.Now
                    };

                    await _context.FinDistributions.AddAsync(distribution3);
                    await _context.SaveChangesAsync();



                    /* updateing out standing balance*/

                    var custAmt = _context.TrnVendorStatements.Where(e => e.VendCode == customer.VendCode);
                    var custInvAmt = (await custAmt.SumAsync(e => e.CrAmount) - await custAmt.SumAsync(e => e.DrAmount));

                    customer.VendOutStandBal = custInvAmt;
                    _context.VendorMasters.Update(customer);
                    await _context.SaveChangesAsync();


                    //var custAmt = _context.TranVenInvoices.Where(e => e.CustomerId == customer.Id && e.InvoiceStatus == "Open");
                    //var custInvAmt = await custAmt.Where(e => !e.IsCreditConverted).ToListAsync();
                    //var custCreditAmt = await custAmt.Where(e => e.IsCreditConverted).ToListAsync();

                    //customer.VendOutStandBal = (custInvAmt.Sum(e => e.TotalAmount) - custCreditAmt.Sum(e => e.TotalAmount));
                    //_context.VendorMasters.Update(customer);
                    //await _context.SaveChangesAsync();



                    List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2, distribution3 };

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
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        Batch = string.Empty,
                        Source = "AP",
                        Remarks = obj.Remarks,
                        Narration = obj.InvoiceNotes ?? obj.Remarks,
                        JvDate = DateTime.Now,
                        Amount = obj.TotalAmount ?? 0,
                        DocNum = obj.InvoiceRefNumber,
                        CDate = DateTime.Now,
                        Posted = true,
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
                                CompanyId = (int)obj.CompanyId,
                                BranchCode = obj.BranchCode,
                                JvDate = DateTime.Now,
                                TranSource = "AP",
                                Trantype = obj.IsCreditConverted ? "Credit" : "Invoice",
                                DocNum = obj.InvoiceRefNumber,
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

                    foreach (var obj1 in distributionsList)
                    {
                        var JournalVoucherItem = new TblFinTrnJournalVoucherItem
                        {
                            JournalVoucherId = jvId,
                            BranchCode = obj.BranchCode,
                            Batch = string.Empty,
                            Remarks = obj.Remarks,
                            CrAmount = obj1.CrAmount ?? 0,
                            DrAmount = obj1.DrAmount ?? 0,
                            FinAcCode = obj1.FinAcCode,
                            Description = obj.InvoiceNotes,

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
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
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
                            Source = "AP",
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
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateApInvoiceSettlement Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return false;
                }
            }
        }
        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);

    }

    #endregion
}
