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
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
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
            var invoices = _context.TranVenInvoices.AsNoTracking();//.Where(e => e.BranchCode == request.User.BranchCode);
            var companies = _context.Companies.AsNoTracking();
            var vendors = _context.VendorMasters.AsNoTracking();
            var vendApproval = _context.TrnVendorApprovals.AsNoTracking();
            var cInvoices = _context.TrnVendorInvoices.AsNoTracking();
            // var users = _context.SystemLogins.Select(e => new { e.Id, e.PrimaryBranch });
            var brnApproval = _context.FinBranchesAuthorities.AsNoTracking();
            bool isArab = request.User.Culture.IsArab();
            //var customers3 = customers.Where(c => EF.Functions.Like(c.NameAR, "%" + request.Input.Query + "%"));                    

            if (request.Input.Id > 0)
            {
                var invoiceItems = _context.TranVenInvoiceItems.AsNoTracking();
                var invoiceIds = invoiceItems.Where(e => e.ProductId == request.Input.Id).Select(e => e.CreditId);

                invoices = invoices.Where(e => invoiceIds.Any(id => id == e.Id));
            }

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
                     CS.VendCode.Contains(request.Input.Query) ||
                     EF.Functions.Like(CS.VendArbName, "%" + request.Input.Query + "%") ||
                     IV.CreditNumber.Contains(request.Input.Query) ||
                     IV.SpCreditNumber.Contains(request.Input.Query) ||
                         IV.TotalAmount.ToString().Contains(request.Input.Query.Replace(",", "")) ||
                     (isDate && IV.InvoiceDate == invoiceDate)
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
                              CustomerName = isArab ? CS.VendArbName : CS.VendName,
                              Code = CS.VendCode,
                              InvoiceRefNumber = IV.InvoiceRefNumber,
                              LpoContract = IV.LpoContract,
                              PaymentTermId = IV.SndPoTermsCode.POTermsName,
                              TaxIdNumber = IV.TaxIdNumber,
                              Remarks=IV.Remarks,
                              InvoiceStatus = IV.IsCreditConverted ? "Credit" : "Invoice",
                              HasAuthority = brnApproval.Any(e => e.AppAuth == request.User.UserId.ToString() && e.FinBranchCode == IV.BranchCode && e.AppAuthAP),
                              ApprovedUser = vendApproval.Any(e => e.LoginId == request.User.UserId && e.InvoiceId == IV.Id && e.IsApproved),
                              IsApproved = vendApproval.Where(e => e.InvoiceId == IV.Id && e.IsApproved).Any(),
                              CanSettle = brnApproval.Where(e => e.FinBranchCode == IV.BranchCode && e.AppAuthAP).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= vendApproval.Where(e => e.InvoiceId == IV.Id && e.IsApproved).Count(),
                              IsSettled = cInvoices.Where(e => e.InvoiceId == IV.Id && e.IsPaid).Any(),
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

    public class GetSingleCreditApInvoiceById : IRequest<TblTranVenInvoiceDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int InvoiceStatusId { get; set; }
    }

    public class GetSingleCreditApInvoiceByIdHandler : IRequestHandler<GetSingleCreditApInvoiceById, TblTranVenInvoiceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleCreditApInvoiceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblTranVenInvoiceDto> Handle(GetSingleCreditApInvoiceById request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            Log.Info("----Info GetSingleCreditApInvoiceById method start----");
            var invoice = await _context.TranVenInvoices.Include(e => e.SysCompanyBranch).ThenInclude(e => e.SysCompany)
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            var vendInvoice = await _context.TrnVendorInvoices.AsNoTracking().FirstOrDefaultAsync(e => e.InvoiceId == invoice.Id);

            var items = await _context.TranVenInvoiceItems
                .Where(e => e.CreditId == request.Id)
                // .Include(e => e.Product)
                //.ThenInclude(e => e.UnitType)                 
                .ToListAsync();


            var productUnitTypes = _context.TranProducts.Include(e => e.UnitType).AsNoTracking()
                .Select(e => new { e.Id, pNameEN = isArab ? e.NameAR : e.NameEN, uNameEN = isArab ? e.UnitType.NameAR : e.UnitType.NameEN });

            var itemUnitCodes = _context.InvItemMaster.Include(e => e.InvUoms).AsNoTracking()
              .Select(e => new { e.Id, pNameEN = isArab ? e.ShortNameAr : e.ShortName, uNameEN = e.InvUoms.UOMName });

            //.Where(e=> items.Any(ivt=>ivt.ProductId == e.ProductTypeId)).ToListAsync();
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

            TblTranVenInvoiceDto invoiceDto = new()
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
               // PaidAmount = invoice.IsCreditConverted ? (-1) * vendInvoice?.PaidAmount ?? 0,
                PaidAmount = (invoice.IsCreditConverted ? (-1) * vendInvoice?.PaidAmount : vendInvoice?.PaidAmount) ?? 0,
                CreatedOn = invoice.CreatedOn,
                TaxIdNumber = invoice.TaxIdNumber,
                InvoiceNotes = invoice.InvoiceNotes,
                Remarks = invoice.Remarks,
                //LogoImagePath = invoice.SysCompanyBranch.SysCompany.LogoURL
            };

            List<TblTranVenInvoiceItemDto> itemList = new();


            foreach (var item in items)
            {
                string pNameEN = string.Empty, uNameEN = string.Empty;
                if (item.InvoiceType is null)
                {
                    var punitType = await productUnitTypes.FirstOrDefaultAsync(e => e.Id == item.ProductId);
                    if (punitType is not null)
                    {
                        pNameEN = punitType.pNameEN;
                        uNameEN = punitType.uNameEN;
                    }
                }
                else
                {
                    var itemCode = await itemUnitCodes.FirstOrDefaultAsync(e => e.Id == item.ProductId);
                    if (itemCode is not null)
                    {
                        pNameEN = itemCode.pNameEN;
                        uNameEN = itemCode.uNameEN;
                    }
                }

                TblTranVenInvoiceItemDto itemDto = new()
                {
                    ProductId = item.ProductId,
                    ProductName = pNameEN,
                    Description = item.Description,
                    Quantity = invoiceStatus * item.Quantity,
                    UnitType = uNameEN,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    DiscountAmount = item.DiscountAmount,

                    TaxTariffPercentage = item.TaxTariffPercentage,
                    TaxAmount = invoiceStatus * item.TaxAmount,
                    TotalAmount = invoiceStatus * item.TotalAmount
                };

                itemList.Add(itemDto);
            }

            //    items.ForEach(async item =>
            //{
            //});

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
        public TblTranVenInvoiceDto Input { get; set; }
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

                    var obj = request.Input;
                    if (obj.TotalAmount <= 0)
                        return ApiMessageInfo.Status(" Total Amount is >= 0 ", 0);

                    string SpCreditNumber = $"{(obj.IsCreditConverted ? "C" : "S") + new Random().Next(99, 9999999).ToString()}";

                    //invoiceNumber = await _context.TranVenInvoices.CountAsync();
                    //invoiceNumber += 1;
                    TblTranVenInvoice Invoice = new();

                    if (request.Input.Id > 0)
                    {
                        Invoice = await _context.TranVenInvoices.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
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


                        var items = _context.TranVenInvoiceItems.Where(e => e.CreditId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.TranVenInvoices.Update(Invoice);
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

                        await _context.TranVenInvoices.AddAsync(Invoice);
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
                        List<TblTranVenInvoiceItem> invoiceItemsList = new();

                        foreach (var obj1 in invoiceItems)
                        {
                            var InvoiceItem = new TblTranVenInvoiceItem
                            {
                                CreditId = inoviceId,
                                CreditNumber = SpCreditNumber,
                                // InvoiceType = obj1.InvoiceType,
                                ProductId = obj1.ProductId,
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
                            await _context.TranVenInvoiceItems.AddRangeAsync(invoiceItemsList);
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

                    var obj = await _context.TranVenInvoices.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
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
                            TblSequenceNumberSetting setting = new();

                            if (obj.IsCreditConverted)
                                setting.VendCreditSeq = creditSeq;
                            else
                                setting.CreditSeq = creditSeq;

                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            if (obj.IsCreditConverted)
                            {
                                creditSeq = invSeq.VendCreditSeq + 1;
                                invSeq.VendCreditSeq = creditSeq;
                            }
                            else
                            {
                                creditSeq = invSeq.CreditSeq + 1;
                                invSeq.CreditSeq = creditSeq;
                            }
                            _context.Sequences.Update(invSeq);
                        }
                        await _context.SaveChangesAsync();

                        obj.CreditNumber = obj.IsCreditConverted ? $"C{creditSeq}" : creditSeq.ToString(); ;
                        obj.SpCreditNumber = string.Empty;
                        _context.TranVenInvoices.Update(obj);
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
                    var postedOn = input.CreatedOn ?? DateTime.Now;
                    var createdOn = DateTime.Now;

                    var obj = await _context.TranVenInvoices.FirstOrDefaultAsync(e => e.Id == input.Id);
                    var vendorMaster = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);
                    var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);
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
                            TblSequenceNumberSetting setting = new();
                            if (obj.IsCreditConverted)
                                setting.VendCreditSeq = invoiceSeq;
                            else
                                setting.CreditSeq = invoiceSeq;

                            await _context.Sequences.AddAsync(setting);
                        }
                        else
                        {
                            if (obj.IsCreditConverted)
                            {
                                invoiceSeq = invSeq.VendCreditSeq + 1;
                                invSeq.VendCreditSeq = invoiceSeq;
                            }
                            else
                            {
                                invoiceSeq = invSeq.CreditSeq + 1;
                                invSeq.CreditSeq = invoiceSeq;
                            }
                                _context.Sequences.Update(invSeq);
                        }

                        obj.CreditNumber = obj.IsCreditConverted ? $"C{invoiceSeq}" : invoiceSeq.ToString();
                        obj.SpCreditNumber = string.Empty;

                    }

                    obj.InvoiceStatus = "Closed";
                    _context.TranVenInvoices.Update(obj);

                    await _context.SaveChangesAsync();

                    obj.TotalAmount = obj.TotalAmount ?? 0;

                    TblFinTrnVendorInvoice cInvoice = new()
                    {
                        CompanyId = (int)obj.CompanyId,
                        BranchCode = obj.BranchCode,
                        InvoiceNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                        InvoiceDate = obj.InvoiceDate,
                        CreditDays = paymentTerms.POTermsDueDays,
                        DueDate = obj.InvoiceDueDate,
                        TranSource = input.TranSource,
                        //Trantype = input.Trantype,
                        Trantype = obj.IsCreditConverted ? "Credit" : "Invoice",
                        VendCode = vendor.VendCode,
                        DocNum = obj.InvoiceRefNumber,
                        LoginId = request.User.UserId,
                        ReferenceNumber = obj.InvoiceRefNumber,
                        InvoiceAmount = obj.TotalAmount,
                        DiscountAmount = obj.DiscountAmount ?? 0,
                        NetAmount = obj.TotalAmount,
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
                        TranDate = postedOn,
                        TranSource = input.TranSource,
                        //Trantype = input.Trantype,
                        Trantype = obj.IsCreditConverted ? "Credit" : "Invoice",
                        TranNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                        VendCode = vendor.VendCode,
                        DocNum = "DocNum",
                        ReferenceNumber = obj.InvoiceRefNumber,
                        PaymentType = input.PaymentType,
                        PamentCode = "paycode",
                        CheckNumber = input.CheckNumber,
                        Remarks1 = input.Remarks1,
                        Remarks2 = input.Remarks2,
                        LoginId = request.User.UserId,
                        DrAmount = !obj.IsCreditConverted ? obj.TotalAmount : 0,
                        CrAmount = obj.IsCreditConverted ? (-1) * obj.TotalAmount : 0,
                        InvoiceId = input.Id,
                    };
                    await _context.TrnVendorStatements.AddAsync(cStatement);


                    if (IsNotCreditPay(request.Input.PaymentType))
                    {
                        TblFinTrnVendorStatement cPaymentStatement = new()
                        {
                            CompanyId = (int)obj.CompanyId,
                            BranchCode = obj.BranchCode,
                            TranDate = postedOn,
                            TranSource = input.TranSource,
                            Trantype = "Payment",
                            TranNumber = obj.CreditNumber,// invoiceSeq.ToString(),
                            VendCode = vendor.VendCode,
                            DocNum = "DocNum",
                            ReferenceNumber = obj.InvoiceRefNumber,
                            PaymentType = input.PaymentType,
                            PamentCode = "Paycode",
                            CheckNumber = input.CheckNumber,
                            Remarks1 = input.Remarks1,
                            Remarks2 = input.Remarks2,
                            LoginId = request.User.UserId,
                            DrAmount = obj.TotalAmount,
                            CrAmount = 0,
                            InvoiceId = input.Id,
                        };
                        await _context.TrnVendorStatements.AddAsync(cPaymentStatement);
                    }

                    //obj.CreditNumber = invoiceSeq.ToString();
                    //obj.SpCreditNumber = string.Empty;
                    //_context.TranVenInvoices.Update(obj);



                    //storing in TblFinTrnDistribution tables

                    //  var vendor = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == obj.CustomerId);

                    string accountCode = string.Empty;
                    if (IsNotCreditPay(request.Input.PaymentType))
                    {

                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == input.PayCode);
                        accountCode = payCode.FinPayAcIntgrAC;
                    }


                    TblFinTrnDistribution distribution1 = new()
                    {
                        InvoiceId = input.Id,
                        FinAcCode = IsNotCreditPay(request.Input.PaymentType) ? accountCode : vendor.VendArAcCode,
                        CrAmount = !obj.IsCreditConverted ? obj.TotalAmount : 0,
                        DrAmount = obj.IsCreditConverted ? obj.TotalAmount : 0,
                        Source = "AP",
                        Gl = string.Empty,
                        Type = IsNotCreditPay(request.Input.PaymentType) ? "paycode" : "Vendor",
                        CreatedOn = postedOn
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
                        CreatedOn = postedOn
                    };
                    await _context.FinDistributions.AddAsync(distribution2);


                    var invoiceItem = await _context.TranVenInvoiceItems.FirstOrDefaultAsync(e => e.CreditId == obj.Id);
                    var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.TaxTariffPercentage).ToString());

                    List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };
                    if (tax is not null)
                    {
                        //throw new NullReferenceException("Tax is empty");

                        TblFinTrnDistribution distribution3 = new()
                        {
                            InvoiceId = input.Id,
                            FinAcCode = tax?.InputAcCode01,
                            CrAmount = obj.IsCreditConverted ? obj.TaxAmount : 0,
                            DrAmount = !obj.IsCreditConverted ? obj.TaxAmount : 0,
                            Source = "AP",
                            Gl = string.Empty,
                            Type = "VAT",
                            CreatedOn = postedOn
                        };

                        await _context.FinDistributions.AddAsync(distribution3);
                        distributionsList.Add(distribution3);
                    }

                    await _context.SaveChangesAsync();


                    /* updateing out standing balance */

                    var custAmt = _context.TrnVendorStatements.Where(e => e.VendCode == vendor.VendCode);
                    var custInvAmt = (await custAmt.SumAsync(e => e.CrAmount) - await custAmt.SumAsync(e => e.DrAmount));

                    vendor.VendOutStandBal = custInvAmt;
                    // _context.VendorMasters.Update(vendor);
                    //  await _context.SaveChangesAsync();


                    //var custAmt = _context.TranVenInvoices.Where(e => e.CustomerId == customer.Id && e.InvoiceStatus == "Open");
                    //var custInvAmt = await custAmt.Where(e => !e.IsCreditConverted).ToListAsync();
                    //var custCreditAmt = await custAmt.Where(e => e.IsCreditConverted).ToListAsync();

                    //vendor.VendOutStandBal = (custInvAmt.Sum(e => e.TotalAmount) - custCreditAmt.Sum(e => e.TotalAmount));
                    //_context.VendorMasters.Update(vendor);
                    //await _context.SaveChangesAsync();





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
                        JvDate = postedOn,
                        Amount = obj.TotalAmount ?? 0,
                        DocNum = obj.CreditNumber,
                        CDate = createdOn,
                        Approved = true,
                        ApprovedDate = DateTime.Now,
                        Posted = true,
                        Void = false,
                        PostedDate = postedOn
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
                                JvDate = postedOn,
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
                            Description = obj.InvoiceNotes,

                            CostAllocation = costallocations.Id,
                            CostSegCode = vendorMaster.VendCode,
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

                        JvDate = postedOn,
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

    #region PurchaseInvoiceList Printing
    public class GetPurchaseInvoicePrintingList : IRequest<PurchaseItemsReportingPrintListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public int? Id { get; set; }
    }
    public class GetPurchaseInvoicePrintListHandler : IRequestHandler<GetPurchaseInvoicePrintingList, PurchaseItemsReportingPrintListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseInvoicePrintListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PurchaseItemsReportingPrintListDto> Handle(GetPurchaseInvoicePrintingList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();

            var venInvoices = _context.TranVenInvoices.AsNoTracking();

            var vendors = _context.VendorMasters.AsNoTracking().Select(e => new { e.VendCode, e.VendArbName, e.VendName, e.Id });

            string branchName = string.Empty;

            if (request.BranchCode.HasValue())
            {
                var cBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode);
                branchName = cBranch.BranchName;

                venInvoices = venInvoices.Where(e => e.BranchCode == request.BranchCode);
            }

            if (request.Id is not null && request.Id > 0)
            {
                venInvoices = venInvoices.Where(e => e.CustomerId == request.Id);
            }

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                venInvoices = venInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            //var grpList = await venInvoices.GroupBy(e => e.CustomerId).ToListAsync();

            //foreach (var e in grpList)
            //{
            //  var item = e.ToList()  new TaxReportingPrintDto
            //    {
            //        TranNumber = e..CreditNumber,
            //        TaxIdNumber = e.TaxIdNumber,
            //        Code = vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendCode,
            //        TaxCode = string.Empty,
            //        Date = e.InvoiceDate,
            //        Name = isArab ? vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendArbName : vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendName,
            //        Source = "Purchase",
            //        Type = "",
            //        InputTaxAmount = e.DiscountAmount,
            //        OutputTaxAmount = e.TaxAmount,
            //        Amount = e.SubTotal,
            //        TotalAmount = e.TotalAmount

            //    }
            //}

            var list = await venInvoices.Select(e => new TaxReportingPrintDto
            {
                TranNumber = e.CreditNumber,
                TaxIdNumber = e.TaxIdNumber,
                Code = vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendCode,
                IsCredit = e.IsCreditConverted,
                TaxCode = string.Empty,
                Date = e.InvoiceDate,
                Name = isArab ? vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendArbName : vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendName,
                Source = "AP",
                Type = "",
                InputTaxAmount = e.DiscountAmount,
                OutputTaxAmount = e.TaxAmount,
                Amount = e.SubTotal,
                TotalAmount = e.TotalAmount

            }).ToListAsync();

            var grpList = list.GroupBy(e => e.Code).OrderByDescending(e => e.Key).ToList();

            List<PurchaseReportingPrintListDto> pRPintingList = new();



            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
            .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;
            var companyDto = new CommonDataLedgerDto();

            if (company is not null)
            {
                companyDto = new()
                {
                    CompanyName = company.CompanyName,
                    CompanyAddress = company.CompanyAddress,
                    Phone = company.Phone,
                    LogoURL = company.LogoURL,
                    BranchName = branchName.HasValue() ? branchName : branch.BranchName,
                    //ledger.Fax = company.;
                    //ledger.PoBox = company.;
                };
            }


            foreach (var item in grpList)
            {
                var invoiceList = item.Where(e => e.IsCredit == false).ToList();
                var creditList = item.Where(e => e.IsCredit == true).ToList();

                List<PurchaseInvoiceReportingPrintListDto> tList = new();
                List<TaxPricingDto> taxPricings = new();

                PurchaseInvoiceReportingPrintListDto calItem = new()
                {
                    List = invoiceList,
                    CreditList = creditList,

                    InvoicePrice = new()
                    {
                        TotalInputTaxAomunt = invoiceList.Sum(e => e.InputTaxAmount),
                        TotalOutputTaxAomunt = invoiceList.Sum(e => e.OutputTaxAmount),
                        TotalPurchaseAmount = invoiceList.Sum(e => e.Amount),
                        TotalSaleAmount = invoiceList.Sum(e => e.TotalAmount),
                    },

                    CreditPrice = new()
                    {
                        TotalInputTaxAomunt = creditList.Sum(e => e.InputTaxAmount),
                        TotalOutputTaxAomunt = creditList.Sum(e => e.OutputTaxAmount),
                        TotalPurchaseAmount = creditList.Sum(e => e.Amount),
                        TotalSaleAmount = creditList.Sum(e => e.TotalAmount),
                    },
                };

                //  calItem.SummaryList = new() { calItem.InvoicePrice, calItem.CreditPrice };

                tList.Add(calItem);

                pRPintingList.Add(new()
                {
                    ItemList = tList
                });
            }

            return new() { ItemPrintingList = pRPintingList, Company = companyDto };
        }
    }

    #endregion

}

