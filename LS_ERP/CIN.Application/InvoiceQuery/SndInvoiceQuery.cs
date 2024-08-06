using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.SndDtos;
using CIN.Application.SndDtos.Comman;
using CIN.Application.SNdDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SND;
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

    public class GetSndSalesInvoiceList : IRequest<PaginatedList<TblSndTranInvoicePagedListDto>>
    {
        public UserIdentityDto User { get; set; }
        public int InvoiceStatusId { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSndSalesInvoiceListHandler : IRequestHandler<GetSndSalesInvoiceList, PaginatedList<TblSndTranInvoicePagedListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndSalesInvoiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblSndTranInvoicePagedListDto>> Handle(GetSndSalesInvoiceList request, CancellationToken cancellationToken)
        {
            try
            {



                Log.Info("----Info GetSndSalesInvoiceList method start----");

                bool isAdmin = _context.SystemLogins.FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
                var invoices = _context.SndTranInvoice.Include(e => e.SysWarehouse).ThenInclude(e => e.SysCompanyBranch).AsNoTracking();//.Where(e => e.BranchCode == request.User.BranchCode);
                var companies = _context.Companies.AsNoTracking();
                var customers = _context.OprCustomers.AsNoTracking();
                var custApproval = _context.TblSndTrnApprovalsList.Where(e => e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice).AsNoTracking();

                //    var cInvoices = _context.TrnCustomerInvoices.AsNoTracking();
                var authorities = await _context.TblSndAuthoritiesList.ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).ToListAsync();
                // var users = _context.SystemLogins.Select(e => new { e.Id, e.PrimaryBranch });
                bool isArab = request.User.Culture.IsArab();
                //var customers3 = customers.Where(c => EF.Functions.Like(c.NameAR, "%" + request.Input.Query + "%"));                    


                if (request.Input.Id > 0)
                {
                    var invoiceItems = _context.SndTranInvoiceItem.AsNoTracking();
                    var invoiceIds = invoiceItems.Where(e => e.ItemCode == request.Input.Id.ToString()).Select(e => e.InvoiceId);

                    invoices = invoices.Where(e => invoiceIds.Any(id => id == e.Id));
                }

                bool isDate = DateTime.TryParse(request.Input.Query, out DateTime invoiceDate);
                var reports = from IV in invoices
                              join CM in companies
                              on IV.CompanyId equals CM.Id
                              join CS in customers
                              on IV.CustomerId equals CS.Id

                              //into AP_Left
                              //from AP in AP_Left.DefaultIfEmpty()
                              //join AP in approvals
                              //on IV.Id equals AP.InvoiceId
                              where //CM.Id == request.User.CompanyId &&
                              IV.InvoiceStatusId == request.InvoiceStatusId &&
                              (
                         CS.CustName.Contains(request.Input.Query) ||
                         EF.Functions.Like(CS.CustArbName, "%" + request.Input.Query + "%") ||
                         IV.InvoiceNumber.Contains(request.Input.Query) ||
                         IV.SpInvoiceNumber.Contains(request.Input.Query) ||
                         //IV.TotalAmount.ToString().Contains(request.Input.Query.Replace(",", "")) ||
                         (isDate && IV.InvoiceDate == invoiceDate)
                              )
                              select new TblSndTranInvoicePagedListDto
                              {
                                  IsQtyDeducted = IV.IsQtyDeducted ?? false,

                                  Id = IV.Id,
                                  //SpInvoiceNumber = IV.SpInvoiceNumber,
                                  InvoiceNumber = IV.InvoiceNumber == string.Empty ? IV.SpInvoiceNumber : IV.InvoiceNumber,
                                  SpInvoiceNumber = IV.SpInvoiceNumber,
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
                                  WarehouseName = IV.SysWarehouse.WHName,
                                  CustomerName = isArab ? CS.CustArbName : CS.CustName,
                                  InvoiceRefNumber = IV.InvoiceRefNumber,
                                  LpoContract = IV.LpoContract,
                                  PaymentTermId = IV.SndSalesTermsCode.SalesTermsCode,
                                  PaymentTermName = IV.SndSalesTermsCode.SalesTermsName,
                                  DueDays = IV.SndSalesTermsCode.SalesTermsDueDays,

                                  TaxIdNumber = IV.TaxIdNumber,
                                  InvoiceStatus = IV.IsCreditConverted ? "Credit" : "Invoice",
                                  ApprovedUser = custApproval.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == IV.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.IsApproved),



                                  IsApproved = (custApproval.Where(e => e.ServiceCode == IV.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.IsApproved).Any()) || (IV.IsApproved),
                                  IsPosted = IV.IsPosted,
                                  IsSettled = IV.IsSettled,
                                  IsPaid = IV.IsPaid,
                                  IsVoid = IV.IsVoid,

                                  HasAuthority = _context.TblSndAuthoritiesList.Where(e => e.BranchCode == IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth == request.User.UserId).ToList().Count > 0,
                                  BranchCode = IV.SysWarehouse.SysCompanyBranch.BranchCode,
                                  WarehouseCode = IV.WarehouseCode,
                                  Authority = _context.TblSndAuthoritiesList.Where(e => e.BranchCode == IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth == request.User.UserId).ToList().Count > 0 ? _context.TblSndAuthoritiesList.ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.BranchCode == IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth == request.User.UserId)
                                  : new TblSndAuthoritiesDto
                                  {
                                      AppAuth = request.User.UserId,

                                      CanCreateSndQuotation = false,
                                      CanEditSndQuotation = false,
                                      CanApproveSndQuotation = false,

                                      CanVoidSndQuotation = false,
                                      CanConvertSndQuotationToInvoice = false,
                                      CanConvertSndQuotationToOrder = false,
                                      CanReviseSndQuotation = false,
                                      CanConvertSndQuotationToDeliveryNote = false,
                                      CanApproveSndInvoice = false,
                                      CanConvertSndDeliveryNoteToInvoice = false,
                                      CanCreateSndInvoice = false,
                                      CanEditSndInvoice = false,
                                      CanSettleSndInvoice = false,
                                      CanPostSndInvoice = false,
                                      CanVoidSndInvoice = false,
                                      BranchCode = IV.SysWarehouse.SysCompanyBranch.BranchCode,




                                  }
                              };







                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    reports = aprv switch
                    {
                        "settled" => reports.Where(e => e.IsSettled && !e.IsVoid),
                        "unsettled" => reports.Where(e => e.IsApproved && !e.IsSettled && !e.IsVoid),
                        "approved" => reports.Where(e => e.IsApproved && !e.IsSettled && !e.IsPosted && !e.IsVoid),
                        "unapproved" => reports.Where(e => !e.IsApproved && !e.IsVoid),
                        "posted" => reports.Where(e => e.IsApproved && e.IsPosted && !e.IsVoid),
                        "cancelled" => reports.Where(e => e.IsVoid),
                        _ => reports
                    };
                }

                //.OrderByDescending(t => t.CustomerId)
                // .ProjectTo<TblTranInvoiceDto>(_mapper.ConfigurationProvider)
                var nreports = await reports.OrderBy(request.Input.OrderBy).Where(e => e.HasAuthority || isAdmin).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetSndSalesInvoiceList method Exit----");
                return nreports;
            }
            catch (Exception ex)
            {


                Log.Error("Error in GetSndSalesInvoiceList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }

    }

    #endregion

    #region CreateSndInvoice

    public class CreateSndInvoice : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputTblSndTranInvoiceDto Input { get; set; }
    }
    public class CreateSndInvoiceQueryHandler : IRequestHandler<CreateSndInvoice, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSndInvoiceQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CreateSndInvoice request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateInvoice method start----");

                    var obj = request.Input;
                    string spInvoiceNumber = $"{(obj.IsCreditConverted ? "C" : "S") + new Random().Next(99, 9999999).ToString()}";
                    var customer = await _context.OprCustomers.FirstOrDefaultAsync(e=>e.Id==obj.CustomerId);
                    //invoiceNumber = await _context.TranInvoices.CountAsync();
                    //invoiceNumber += 1;
                    TblSndTranInvoice Invoice = new();
                    List<TblSndTranInvoiceItem> invoiceItemsList = new();

                 
                        if (request.Input.Id > 0  && (request.Input.SaveType == EnumSaveType.SaveAndApprove || request.Input.SaveType == EnumSaveType.SaveAndSettle) )
                    {
                        Invoice = await _context.SndTranInvoice.FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        spInvoiceNumber = Invoice.SpInvoiceNumber;

                        Invoice.CustomerId = obj.CustomerId;
                        Invoice.InvoiceDate = Convert.ToDateTime(obj.InvoiceDate);
                        Invoice.InvoiceDueDate = Convert.ToDateTime(obj.InvoiceDueDate);
                        Invoice.ServiceDate1 = obj.ServiceDate1;
                        Invoice.CompanyId = obj.CompanyId;
                        Invoice.WarehouseCode = obj.WarehouseCode;
                        Invoice.InvoiceRefNumber = obj.InvoiceRefNumber;

                        Invoice.LpoContract = obj.LpoContract;
                        Invoice.PaymentTermId = obj.PaymentTermId;
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
                        Invoice.FooterDiscount = obj.FooterDiscount;
                        Invoice.CustomerCode = customer.CustCode;
                        if (obj.CustName.HasValue())
                        {
                            Invoice.CustName = obj.CustName;
                            Invoice.CustArbName = obj.CustArbName;
                        }

                        var items = _context.SndTranInvoiceItem.Where(e => e.InvoiceId == request.Input.Id);
                        _context.RemoveRange(items);
                        _context.SndTranInvoice.Update(Invoice);
                        await _context.SaveChangesAsync();
                    }
                    else if(request.Input.Id==0)
                    {
                        if (obj.TotalAmount is null)
                            return 0;

                        Invoice = new()
                        {
                           
                            SpInvoiceNumber = spInvoiceNumber,
                            InvoiceNumber = string.Empty,
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
                            PaymentTermId = obj.PaymentTermId,
                            WarehouseCode = obj.WarehouseCode,
                            ServiceDate1 = obj.ServiceDate1,
                            IsCreditConverted = obj.IsCreditConverted,
                            InvoiceStatusId = obj.InvoiceStatusId,
                            CustName = obj.CustName.HasValue() ? obj.CustName : string.Empty,
                            CustArbName = obj.CustArbName.HasValue() ? obj.CustArbName : string.Empty,
                            FooterDiscount = obj.FooterDiscount,
                            Source = request.Input.SaveType == EnumSaveType.ConvertDelNoteToInvoice ? "D"
                            : request.Input.SaveType == EnumSaveType.ConvertQuotToInvoice ? "Q"
                            : string.Empty,
                            DeleveryRefNumber = request.Input.SaveType == EnumSaveType.ConvertDelNoteToInvoice ? request.Input.DeliveryNoteId.ToString()
                            : request.Input.SaveType == EnumSaveType.ConvertQuotToInvoice ? request.Input.QuotationId.ToString()
                            : string.Empty,

                            IsQtyDeducted = false,
                            SiteCode=request.Input.SiteCode,
                            CustomerCode=customer.CustCode,
                             TotalCost=0,
                             IsApproved=false,
                             IsPaid=false,
                             IsPosted=false,
                              IsSettled=false,
                              
                        };


                        await _context.SndTranInvoice.AddAsync(Invoice);
                        await _context.SaveChangesAsync();
                    }


                    Log.Info("----Info CreateUpdateInvoice method Exit----");

                    var invoiceId = Invoice.Id;
                    var invoiceItems = request.Input.ItemList;
                 
                        if ((invoiceItems.Count > 0 && (request.Input.Id > 0 && (request.Input.SaveType == EnumSaveType.SaveAndApprove || request.Input.SaveType == EnumSaveType.SaveAndSettle)))||request.Input.Id==0)
                        {
                            decimal TotalCost = 0;
                            foreach (var obj1 in invoiceItems)
                            {
                                obj1.ItemAvgCost = _context.InvItemInventory.FirstOrDefault(e => e.ItemCode == obj1.ItemCode && e.WHCode == Invoice.WarehouseCode).ItemAvgCost;



                                var convFactor = _context.InvItemsUOM.FirstOrDefault(e => e.ItemCode == obj1.ItemCode && e.ItemUOM == obj1.UnitType).ItemConvFactor;
                                TotalCost += (decimal)obj1.ItemAvgCost * obj1.Quantity * convFactor ?? 0;

                                var InvoiceItem = new TblSndTranInvoiceItem
                                {
                                    InvoiceId = invoiceId,
                                    InvoiceNumber = spInvoiceNumber,
                                    InvoiceType = obj1.InvoiceType,
                                    ItemCode = obj1.ItemCode,
                                    UnitType = obj1.UnitType,
                                    Quantity = obj1.Quantity,
                                    NetQuantity=obj1.Quantity*convFactor,
                                    UnitPrice = obj1.UnitPrice,
                                    SubTotal = obj1.SubTotal,
                                    DiscountAmount = obj1.DiscountAmount,
                                    AmountBeforeTax = obj1.AmountBeforeTax,
                                    TaxAmount = obj1.TaxAmount,
                                    TotalAmount = obj1.TotalAmount,
                                    IsDefaultConfig = true,
                                    CreatedOn = Convert.ToDateTime(obj.InvoiceDate),
                                    CreatedBy = request.User.UserId,
                                    Description = obj1.Description,
                                    TaxTariffPercentage = obj1.TaxTariffPercentage,
                                    Discount = obj1.Discount,
                                    ItemAvgCost = obj1.ItemAvgCost,


                                };
                                invoiceItemsList.Add(InvoiceItem);
                            }
                            if (invoiceItemsList.Count > 0)
                            {
                                await _context.SndTranInvoiceItem.AddRangeAsync(invoiceItemsList);
                                await _context.SaveChangesAsync();

                                Invoice.TotalCost = TotalCost;
                                _context.SndTranInvoice.Update(Invoice);
                                await _context.SaveChangesAsync();
                            }

                        }
                  

                    if (request.Input.SaveType == EnumSaveType.SaveAndApprove || request.Input.SaveType == EnumSaveType.SaveAndSettle || request.Input.SaveType == EnumSaveType.ConvertDelNoteToInvoice || request.Input.SaveType == EnumSaveType.ConvertQuotToInvoice)
                    {

                        var warehouse = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.WHCode == request.Input.WarehouseCode);

                        TblSndTrnApprovals approval = new()
                        {

                            BranchCode = warehouse.WHBranchCode,
                            AppAuth = request.User.UserId,
                            CreatedOn = DateTime.UtcNow,
                            ServiceCode = Invoice.Id.ToString(),
                            ServiceType = (short)EnumSndApprovalServiceType.SndInvoice,
                            AppRemarks = request.Input.SaveType == EnumSaveType.SaveAndApprove ? "Save And Approve"
                            : request.Input.SaveType == EnumSaveType.SaveAndSettle ? "Save and Settle"
                            : request.Input.SaveType == EnumSaveType.ConvertDelNoteToInvoice ? "Convert Delivery Note To Invoice"
                            : request.Input.SaveType == EnumSaveType.ConvertQuotToInvoice ? "Convert Quotation To Invoice"
                            : "",
                            IsApproved = true,
                        };

                        await _context.TblSndTrnApprovalsList.AddAsync(approval);
                        await _context.SaveChangesAsync();

                        if (!Invoice.InvoiceNumber.HasValue())
                        {
                            int invoiceSeq = 0;
                            var invSeq = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == warehouse.WHBranchCode);
                            if (invSeq is null)
                            {
                                invoiceSeq = 1;
                                TblSequenceNumberSetting setting = new()
                                {
                                    SDInvoiceNumber = invoiceSeq,
                                    BranchCode = warehouse.WHBranchCode,
                                };
                                await _context.Sequences.AddAsync(setting);
                            }
                            else
                            {
                                invoiceSeq = invSeq.SDInvoiceNumber + 1;
                                invSeq.SDInvoiceNumber = invoiceSeq;
                                _context.Sequences.Update(invSeq);
                            }
                            await _context.SaveChangesAsync();

                            Invoice.InvoiceNumber = invoiceSeq.ToString();
                            Invoice.SpInvoiceNumber = string.Empty;

                            Invoice.IsApproved = true;          //extra statement with respect to normal Approval

                            _context.SndTranInvoice.Update(Invoice);
                            await _context.SaveChangesAsync();
                        }

                        if (request.Input.SaveType == EnumSaveType.SaveAndSettle)
                        {
                            if (request.Input.SettlementData.PaymentType == "Credit")   //settlement only with credit
                            {
                                if (request.Input.DueDays == 0)
                                {
                                    await transaction.RollbackAsync();

                                    return -2;

                                }

                                Invoice.IsSettled = true;
                                //Invoice.InvoiceStatusId = (int)InvoiceStatusIdType.Credit;
                                // Invoice.IsCreditConverted = true;
                                _context.SndTranInvoice.Update(Invoice);
                                await _context.SaveChangesAsync();

                            }

                            else if (request.Input.SettlementData.PaymentType == "Cash" && request.Input.SettlementData.PaymentsList.Count == 0)
                            {
                                await transaction.RollbackAsync();

                                return -3;

                            }

                            var settlements = await _context.TblSndTranInvoicePaymentSettlementsList.Where(e => e.InvoiceId == Invoice.Id).ToListAsync();
                            if (settlements.Count > 0)
                            {
                                _context.TblSndTranInvoicePaymentSettlementsList.RemoveRange(settlements);
                                await _context.SaveChangesAsync();
                            }

                            List<TblSndTranInvoicePaymentSettlements> Settlements = new();
                            for (var i = 0; i < request.Input.SettlementData.PaymentsList.Count; i++)
                            {
                                Settlements.Add(
                                    new TblSndTranInvoicePaymentSettlements
                                    {
                                        PaymentCode = request.Input.SettlementData.PaymentsList[i].PaymentCode,
                                        InvoiceId = Invoice.Id,
                                        DueDate = Invoice.InvoiceDueDate,
                                        SettledAmount = request.Input.SettlementData.PaymentsList[i].SettledAmount,
                                        SettledDate = DateTime.UtcNow,
                                        SettledBy = request.User.UserId,
                                        Remarks = request.Input.SettlementData.PaymentsList[i].Remarks,
                                        IsPaid = false
                                    }

                                    );



                            }

                            await _context.TblSndTranInvoicePaymentSettlementsList.AddRangeAsync(Settlements);

                            await _context.SaveChangesAsync();



                            Invoice.IsSettled = true;

                            _context.SndTranInvoice.Update(Invoice);
                            await _context.SaveChangesAsync();


                        }
                        else if (request.Input.SaveType == EnumSaveType.ConvertDelNoteToInvoice)
                        {
                            var DeliveryNote = await _context.DeliveryNoteHeaders.FirstOrDefaultAsync(e => e.Id == request.Input.DeliveryNoteId.Value);
                            DeliveryNote.IsConvertedDeliveryNoteToInvoice = true;
                            DeliveryNote.UpdatedBy = request.User.UserId;
                            DeliveryNote.UpdatedOn = DateTime.UtcNow;
                            _context.DeliveryNoteHeaders.Update(DeliveryNote);
                            await _context.SaveChangesAsync();


                            #region Deduct Inventory Qty --> its when converting deliver  note to invoice, same code might be useful when posting invoice

                            var DeliveryNoteLines = await _context.DeliveryNoteLines.Where(e => e.DeliveryNoteId == DeliveryNote.Id).ToListAsync();

                            if (DeliveryNoteLines.Count > 0)
                            {

                                foreach (var dnl in DeliveryNoteLines)
                                {
                                    var uom = await _context.InvItemsUOM.FirstOrDefaultAsync(e => e.ItemCode == dnl.ItemCode && e.ItemUOM == dnl.UnitType);

                                    var StockItem = await _context.InvItemInventory.FirstOrDefaultAsync(e => e.ItemCode == dnl.ItemCode && e.WHCode == DeliveryNote.WarehouseCode);

                                    bool isQtyOverrideAllowed = true;   // need to find from Inventory later



                                    decimal baseQty = (decimal)(dnl.Quantity * uom.ItemConvFactor);
                                   // decimal basePrice = (decimal)(dnl.SubTotal??(dnl.Quantity*dnl.UnitPrice) - dnl.DiscountAmount) / baseQty;
                                    if (StockItem is not null)
                                    {

                                        StockItem.QtyOH = StockItem.QtyOH - baseQty;
                                        if (isQtyOverrideAllowed || StockItem.QtyOH > 0)
                                        {
                                            _context.InvItemInventory.Update(StockItem);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            await transaction.RollbackAsync();
                                            return -1;              //Qty override Not Allowed
                                        }

                                    }
                                    else
                                    {
                                        //if (isQtyOverrideAllowed)
                                        //{
                                        //    TblErpInvItemInventory item = new();

                                        //    item.ItemCode = dnl.ItemCode;
                                        //    item.WHCode = DeliveryNote.WarehouseCode;
                                        //    item.QtyOH = 0 - (dnl.Delivery * uom.ItemConvFactor);
                                        //    item.CreatedOn = DateTime.UtcNow;
                                        //    item.EOQ = 0;
                                        //    item.IsActive = true;
                                        //    item.ItemLandedCost = 0;
                                        //    item.ItemLastPOCost = 0;
                                        //    item.MaxQty = 0;
                                        //    item.MinQty = 0;
                                        //    item.QtyOnPO = 0;
                                        //    item.QtyOnSalesOrder = 0;
                                        //    item.QtyReserved = 0;
                                        //    item.ItemAvgCost = 0;

                                        //    await _context.InvItemInventory.AddAsync(item);
                                        //    await _context.SaveChangesAsync();


                                        //}

                                        await transaction.RollbackAsync();
                                        return -2;

                                    }



                                }
                                Invoice.IsQtyDeducted = true;
                                _context.SndTranInvoice.Update(Invoice);
                                await _context.SaveChangesAsync();
                                decimal TotalCost = 0;

                                var invItems = await _context.SndTranInvoiceItem.Where(e=>e.InvoiceId==Invoice.Id).ToListAsync();
                                foreach (var invItem in invItems) {
                                    invItem.ItemAvgCost = _context.InvItemInventory.AsNoTracking().FirstOrDefault(e => e.ItemCode == invItem.ItemCode && e.WHCode==Invoice.WarehouseCode).ItemAvgCost;
                                    var item = _context.InvItemMaster.FirstOrDefault(e => e.ItemCode == invItem.ItemCode);
                                    var convFactor = _context.InvItemsUOM.FirstOrDefault(e=>e.ItemCode==invItem.ItemCode&&e.ItemUOM==invItem.UnitType).ItemConvFactor;
                                    TotalCost +=(decimal) invItem.ItemAvgCost * invItem.Quantity *convFactor??0;
                                    _context.SndTranInvoiceItem.Update(invItem);
                                    await _context.SaveChangesAsync();
                                }
                                Invoice.TotalCost = TotalCost;
                                _context.SndTranInvoice.Update(Invoice);
                                await _context.SaveChangesAsync();
                               

                                #endregion

                            }
                        }
                        else if (request.Input.SaveType == EnumSaveType.ConvertQuotToInvoice)
                        {
                            var Quotation = await _context.SndTranQuotations.FirstOrDefaultAsync(e => e.Id == request.Input.QuotationId.Value);
                            Quotation.IsConvertedSndQuotationToInvoice = true;
                            Quotation.UpdatedBy = request.User.UserId;
                            Quotation.UpdatedOn = DateTime.UtcNow;
                            _context.SndTranQuotations.Update(Quotation);
                            await _context.SaveChangesAsync();
                        }
                        
                    }

                    await transaction.CommitAsync();

                    return Invoice.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSndInvoice Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion











    #region GetCustomerSndInvoiceNumberList

    public class GetCustomerSndInvoiceNumberList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCustomerSndInvoiceNumberListQueryHandler : IRequestHandler<GetCustomerSndInvoiceNumberList, List<CustomSelectListItem>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetCustomerSndInvoiceNumberListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetCustomerSndInvoiceNumberList request, CancellationToken cancellationToken)
        {
            try
            {

                var invNumbers = await _context.SndTranInvoice.AsNoTracking()
                         .Where(e => e.CustomerId == request.Id && !string.IsNullOrEmpty(e.InvoiceNumber))
                         .OrderByDescending(e => e.Id)
                         .Select(e => new CustomSelectListItem { Text = e.InvoiceNumber, Value = e.Id.ToString() })
                        .ToListAsync(cancellationToken);
                return invNumbers;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetCustomerSndInvoiceNumberList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion






    #region GetSingleSndCreditInvoiceById

    public class GetSingleSndCreditInvoiceById : IRequest<TblSndTranInvoiceDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int InvoiceStatusId { get; set; }
    }

    public class GetSingleSndCreditInvoiceByIdHandler : IRequestHandler<GetSingleSndCreditInvoiceById, TblSndTranInvoiceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSndCreditInvoiceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSndTranInvoiceDto> Handle(GetSingleSndCreditInvoiceById request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetSingleSndCreditInvoiceById method start----");
            try
            {
                var invoice = await _context.SndTranInvoice.Include(e => e.SysWarehouse).ThenInclude(e => e.SysCompanyBranch).ThenInclude(e => e.SysCompany)
                       .FirstOrDefaultAsync(e => e.Id == request.Id);
                //  var custInvoice = await _context.SndTranInvoice.AsNoTracking().FirstOrDefaultAsync(e => e.InvoiceId == invoice.Id);

                var items = await _context.SndTranInvoiceItem
                    .Where(e => e.InvoiceId == request.Id)
                    //.Include(e => e.Product)
                    //.ThenInclude(e => e.UnitType)
                    .ToListAsync();



                var productUnitTypes = _context.TranProducts.Include(e => e.UnitType).AsNoTracking()
                    .Select(e => new { e.Id, pNameEN = e.NameEN, uNameEN = e.UnitType.NameEN });


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

                TblSndTranInvoiceDto invoiceDto = new()
                {
                    CustomerId = invoice.CustomerId,
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceDueDate = invoice.InvoiceDueDate,
                    CompanyId = invoice.CompanyId,
                    WarehouseCode = invoice.SysWarehouse.WHCode,
                    InvoiceRefNumber = invoice.InvoiceRefNumber,
                    LpoContract = invoice.LpoContract,
                    PaymentTermId = invoice.PaymentTermId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    ServiceDate1 = invoice.ServiceDate1,
                    IsCreditConverted = invoice.IsCreditConverted,
                    InvoiceStatusId = invoice.InvoiceStatusId,

                    SubTotal = invoice.SubTotal,
                    TaxAmount = invoice.TaxAmount,
                    TotalAmount = invoice.TotalAmount,
                    DiscountAmount = invoice.DiscountAmount,
                    PaidAmount = 0,

                    CreatedOn = invoice.CreatedOn,
                    TaxIdNumber = invoice.TaxIdNumber,
                    InvoiceNotes = invoice.InvoiceNotes,
                    Remarks = invoice.Remarks,
                    CustName = invoice.CustName,
                    CustArbName = invoice.CustArbName,
                    CustomerName = invoice.CustName,
                    LogoImagePath = invoice.SysWarehouse.SysCompanyBranch.SysCompany.LogoURL,
                    FooterDiscount = (short)invoice.FooterDiscount,
                      VatPercentage=invoice.VatPercentage,
                      InvoiceStatus=invoice.InvoiceStatus,
                      Source=invoice.Source,
                       

                };

                List<TblSndTranInvoiceItemDto> itemList = new();

                foreach (var item in items)
                {
                    var Item = _context.InvItemMaster.FirstOrDefault(e => e.ItemCode == item.ItemCode);
                    var punitType = _context.InvItemsUOM.FirstOrDefault(e => e.ItemCode == item.ItemCode);
                    string pNameEN = string.Empty, uNameEN = string.Empty;
                    if (punitType is not null)
                    {
                        pNameEN = Item.ShortName;
                        uNameEN = Item.ShortNameAr;
                    }

                    TblSndTranInvoiceItemDto itemDto = new()
                    {

                        ItemId = Item.Id,

                        ItemCode = item.ItemCode,
                        ItemName = pNameEN,
                        Description = item.Description,
                        Quantity = invoiceStatus * item.Quantity,
                        UnitType = item.UnitType,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        DiscountAmount = item.DiscountAmount,

                        TaxTariffPercentage = item.TaxTariffPercentage,
                        TaxAmount = invoiceStatus * item.TaxAmount,
                        TotalAmount = invoiceStatus * item.TotalAmount,
                        NetQuantity=item.NetQuantity,
                        ItemAvgCost=item.ItemAvgCost,
                        NetCost=item.NetQuantity*item.ItemAvgCost
                    };

                    itemList.Add(itemDto);
                }

                //items.ForEach(async item =>
                //{

                //});

                //items.ForEach(item =>
                //{
                //    TblTranInvoiceItemDto itemDto = new()
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

                Log.Info("----Info GetSingleSndCreditInvoiceById method Exit----");
                return invoiceDto;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSingleSndCreditInvoiceById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;

            }
        }

    }

    #endregion
    #region PostSndInvoiceByInvoiceId  //if Qty Not Deducted=> then Deduct Qty,update History and then =>generating,Aproving,Settling and posting AR Invoice

    public class PostSndInvoiceByInvoiceIdQuery : IRequest<SndResultDto>
    {
        public UserIdentityDto User { get; set; }
        public InputSndTranInvoicePostDto Input { get; set; }
    }

    public class PostSndInvoiceByInvoiceIdQueryHandler : IRequestHandler<PostSndInvoiceByInvoiceIdQuery, SndResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public PostSndInvoiceByInvoiceIdQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);

        public async Task<SndResultDto> Handle(PostSndInvoiceByInvoiceIdQuery request, CancellationToken cancellationToken)
        {

            Log.Info("----Info PostSndInvoiceByInvoiceId method start----");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var invoice = await _context.SndTranInvoice.Include(e => e.SysWarehouse).ThenInclude(e => e.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                    bool IsQtyReducted = invoice.IsQtyDeducted ?? false;


                    if (invoice is null)
                    {
                        return new() { IsSuccess = false, ErrorMsg = "Invoice:" + request.Input.Id + " Not Exists" };
                    }
                    if (!(await _context.TblSndTrnApprovalsList.AnyAsync(e => e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.ServiceCode == request.Input.Id.ToString() && e.AppAuth == request.User.UserId && e.IsApproved)))
                        return new() { IsSuccess = false, ErrorMsg = "Invoice:" + request.Input.Id + " Not Yet Approved" };

                    bool hasAuthority = await _context.TblSndAuthoritiesList.AnyAsync(e => e.AppAuth == request.User.UserId && e.CanPostSndInvoice && invoice.SysWarehouse.WHBranchCode == e.BranchCode);
                    if (!hasAuthority)
                    {
                        return new()
                        {
                            IsSuccess = false,
                            ErrorMsg = "Un Authorised Access-WareHouseBranch:" + invoice.SysWarehouse.WHBranchCode + " User:" + request.User.UserId
                        };
                    }
                 
                    var InvoiceItemLines = await _context.SndTranInvoiceItem.Where(e => e.InvoiceId == invoice.Id).ToListAsync();
                    invoice.IsPosted = true;
                    _context.SndTranInvoice.Update(invoice);
                    await _context.SaveChangesAsync();
                 


                    #region UpdateHistory,if not deducted->Deduct Qty,
                    if (InvoiceItemLines.Count() > 0)
                    {
                        if (!(IsQtyReducted))
                        {
                            decimal TotalCost = 0;
                            foreach (var invItem in InvoiceItemLines)
                            {
                                invItem.ItemAvgCost = _context.InvItemInventory.FirstOrDefault(e => e.ItemCode == invItem.ItemCode && e.WHCode == invoice.WarehouseCode).ItemAvgCost;
                                var item = _context.InvItemMaster.FirstOrDefault(e => e.ItemCode == invItem.ItemCode);
                                TotalCost += (decimal)invItem.ItemAvgCost * invItem.NetQuantity ?? 0;
                                _context.SndTranInvoiceItem.Update(invItem);
                                await _context.SaveChangesAsync();
                            }
                            invoice.TotalCost = TotalCost;
                            _context.SndTranInvoice.Update(invoice);
                            await _context.SaveChangesAsync();
                          
                        }


                        List<TblErpInvItemInventoryHistory> invHistory = new();


                        foreach (var dnl in InvoiceItemLines)
                        {
                            var uom = await _context.InvItemsUOM.FirstOrDefaultAsync(e => e.ItemCode == dnl.ItemCode && e.ItemUOM == dnl.UnitType);

                            var StockItem = await _context.InvItemInventory.Include(e => e.InvItemMaster).FirstOrDefaultAsync(e => e.ItemCode == dnl.ItemCode && e.WHCode == invoice.WarehouseCode);

                            decimal qty = dnl.Quantity ?? 0;
                            decimal baseQty = (decimal)(dnl.Quantity * uom.ItemConvFactor);
                          //  decimal basePrice = (decimal)(dnl.UnitPrice - (dnl.DiscountAmount / dnl.Quantity)) / uom.ItemConvFactor;



                            if (invoice.IsCreditConverted)
                            {
                                baseQty = -baseQty;
                                qty = -qty;
                            }
                            if (!(IsQtyReducted))
                            {
                                #region Deduct Inventory Qty--> This Code is Same in "ConvertDeliveryNoteToInvoice"      ReusableCode-->DeliveryNoteToInvoice

                                bool isQtyOverrideAllowed = true;   // need to find from Inventory later
                                if (StockItem is not null)
                                {

                                    StockItem.QtyOH = StockItem.QtyOH - baseQty;
                                    if ((isQtyOverrideAllowed || StockItem.QtyOH > 0))
                                    {
                                        _context.InvItemInventory.Update(StockItem);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        await transaction.RollbackAsync();
                                        return new() { IsSuccess = false, ErrorMsg = "Qty Override Not Allowed" };              //Qty override Not Allowed
                                    }
                                }
                                else
                                {
                                    //if (isQtyOverrideAllowed || baseQty < 0)
                                    //{
                                    //    TblErpInvItemInventory item = new();

                                    //    item.ItemCode = dnl.ItemCode;
                                    //    item.WHCode = invoice.WarehouseCode;
                                    //    item.QtyOH = 0 - baseQty;
                                    //    item.CreatedOn = DateTime.UtcNow;
                                    //    item.EOQ = 0;
                                    //    item.IsActive = true;
                                    //    item.ItemLandedCost = 0;
                                    //    item.ItemLastPOCost = 0;
                                    //    item.MaxQty = 0;
                                    //    item.MinQty = 0;
                                    //    item.QtyOnPO = 0;
                                    //    item.QtyOnSalesOrder = 0;
                                    //    item.QtyReserved = 0;
                                    //    item.ItemAvgCost = 0;

                                    //    await _context.InvItemInventory.AddAsync(item);
                                    //    await _context.SaveChangesAsync();
                                    //}



                                    await transaction.RollbackAsync();
                                    return new() { IsSuccess = false, ErrorMsg = "Item not found in inventory" };
                                }











                               
                                #endregion
                            }
                           
                          
                            invHistory.Add(new()
                            {
                                CreatedOn = DateTime.UtcNow,
                                IsActive = true,
                                ItemCode = StockItem.ItemCode,
                                WHCode = invoice.WarehouseCode,
                                TranType = "6",  //Suggested By Rakesh 12/06/2023
                                TranDate = DateTime.UtcNow,
                                TranNumber = "SD"+invoice.InvoiceNumber,
                                TranQty =-qty,
                                TranPrice = dnl.TotalAmount/ uom.ItemConvFactor ?? 0,  
                                unitConvFactor = uom.ItemConvFactor,
                                TranUnit = dnl.UnitType,
                                TranTotQty = -baseQty,
                                TranRemarks = invoice.IsCreditConverted?"Inserted through SR": "Inserted through SD",
                                ItemAvgCost = dnl.ItemAvgCost??StockItem.ItemAvgCost
                            });

                        }


                       

                        if (invHistory.Count > 0)
                        {
                            await _context.InvItemInventoryHistory.AddRangeAsync(invHistory);
                            await _context.SaveChangesAsync();
                        }
                        invoice.IsQtyDeducted = true;
                        _context.SndTranInvoice.Update(invoice);
                        await _context.SaveChangesAsync();

                    }
                    #endregion


                    #region Copy SndInvoice Into AR Invoice, SndInvoiceItems To ArInvoiceItems

                    //int invoiceSeq = 0;
                    //var invSeq = await _context.Sequences.FirstOrDefaultAsync();
                    //if (invSeq is null)
                    //{
                    //    invoiceSeq = 1;
                    //    TblSequenceNumberSetting setting = new()
                    //    {
                    //        InvoiceSeq = invoiceSeq
                    //    };
                    //    await _context.Sequences.AddAsync(setting);
                    //}
                    //else
                    //{
                    //    invoiceSeq = invSeq.InvoiceSeq + 1;
                    //    invSeq.InvoiceSeq = invoiceSeq;
                    //    _context.Sequences.Update(invSeq);
                    //}
                    //await _context.SaveChangesAsync();

                    TblTranInvoice arInvoice = new()
                    {
                        InvoiceDate = invoice.InvoiceDate,
                        AmountDue = invoice.AmountDue,
                        AmountBeforeTax = invoice.AmountBeforeTax,
                        BranchCode = invoice.SysWarehouse.SysCompanyBranch.BranchCode,
                        CompanyId = invoice.CompanyId,
                        CreatedBy = request.User.UserId,
                        CreatedOn = DateTime.UtcNow,
                        CurrencyId = null,
                        CustArbName = invoice.CustArbName,
                        CustName = invoice.CustName,
                        CustomerId = invoice.CustomerId,
                        DiscountAmount = invoice.DiscountAmount??0,
                        InvoiceDueDate = invoice.InvoiceDueDate,
                        InvoiceModule = "SD",
                        InvoiceNotes = "Generated From Sales And Distributions, Invoice Id:" + invoice.InvoiceNumber,
                        InvoiceRefNumber = "",
                        InvoiceStatus = "Open",
                        IsCreditConverted = invoice.IsCreditConverted,
                        IsDefaultConfig = invoice.IsDefaultConfig,
                        LpoContract = invoice.LpoContract,
                        Remarks = "Generated From Sales And Distributions, Invoice Id:" + invoice.InvoiceNumber,
                        ServiceDate1 = invoice.ServiceDate1,
                        SiteCode = invoice.SiteCode,
                        SubTotal = invoice.SubTotal,
                        TaxAmount = invoice.TaxAmount,
                        TotalAmount = invoice.TotalAmount,
                        VatPercentage = invoice.VatPercentage,
                        TaxIdNumber = invoice.TaxIdNumber,
                        TotalPayment = invoice.TotalPayment,
                        InvoiceNumber = "SD"+invoice.InvoiceNumber,//invoiceSeq.ToString(),
                        SpInvoiceNumber = string.Empty,
                        InvoiceStatusId = invoice.InvoiceStatusId,
                          PaymentTerms=invoice.PaymentTermId,
                    };

                    await _context.TranInvoices.AddAsync(arInvoice);
                    await _context.SaveChangesAsync();



                    if (InvoiceItemLines.Count()>0)
                    {
                        var ExistArInvoiceItems = _context.TranInvoiceItems.AsNoTracking().Where(e=>e.InvoiceId==arInvoice.Id).ToList();
                        if (ExistArInvoiceItems.Count()>0)
                        {
                             _context.TranInvoiceItems.RemoveRange(ExistArInvoiceItems);
                            await _context.SaveChangesAsync();
                        }
                        foreach(var Item in InvoiceItemLines)
                        {
                            var Product = _context.InvItemMaster.Where(c => c.ItemCode == Item.ItemCode).Single();
                            TblTranInvoiceItem ArInvItem = new()
                            {
                                AmountBeforeTax = Item.AmountBeforeTax,
                                CreatedBy = request.User.UserId,
                                CreatedOn = DateTime.UtcNow,
                                CreditMemoId = Item.CreditMemoId,
                                DebitMemoId = Item.DebitMemoId,
                                Description = Item.Description,
                                Discount = Item.Discount,
                                DiscountAmount = Item.DiscountAmount,
                                InvoiceId = arInvoice.Id,  
                                InvoiceNumber = Item.InvoiceNumber,
                                InvoiceType = 6,  //1->SND  suggested by Rakesh 21/04/2023, changed to 6-->12/06/2023
                                IsDefaultConfig = Item.IsDefaultConfig,

                                ProductId = Product.Id,    // Taken Reference From ARInvoice, suggested by Mohsin and Rakesh
                                SiteCode = Item.SiteCode,
                                 SubTotal=Item.SubTotal,
                                  Quantity=Item.Quantity,
                                   TaxAmount=Item.TaxAmount,
                                    TaxTariffPercentage=Item.TaxTariffPercentage,
                                     TotalAmount=Item.TotalAmount,
                                      UnitPrice=Item.UnitPrice,
                                       Id=0,
                            };
                            await _context.TranInvoiceItems.AddAsync(ArInvItem);
                            await _context.SaveChangesAsync();
                        }
                       
                        
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return new() { IsSuccess = false, ErrorMsg = "No Invoice Items Found" };
                    }


                    #endregion


                   





                    #region Approve,Settle and  Post AR Invoice

                    Log.Info("----Info CreateArInvoiceSettlement method start----");

                       // var input = request.Input;
                        var postedOn = arInvoice.CreatedOn ?? DateTime.Now;
                        var createdOn = DateTime.Now;

                        //var obj = await _context.TranInvoices.FirstOrDefaultAsync(e => e.Id == input.Id);  //obj Replaced By arInvoice
                        var customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.Id == arInvoice.CustomerId);
                        var paymentTerms = await _context.SndSalesTermsCodes.FirstOrDefaultAsync(e => e.SalesTermsCode == arInvoice.PaymentTerms);

                    if (await _context.TrnCustomerInvoices.AnyAsync(e => e.InvoiceId == arInvoice.Id && e.LoginId == request.User.UserId && e.IsPaid))
                    {
                        await transaction.RollbackAsync();
                        return new() { IsSuccess = false, ErrorMsg = "Invoice:" + arInvoice.Id + " is Already Paid" };
                    }

                        //if (!arInvoice.InvoiceNumber.HasValue())
                        //{
                        //    int invoiceSeq = 0;
                        //    TblSequenceNumberSetting setting = await _context.Sequences.FirstOrDefaultAsync();
                        //    if (setting is null)
                        //    {
                        //        invoiceSeq = 1;
                        //        setting = new()
                        //        {
                        //            InvoiceSeq = invoiceSeq
                        //        };
                        //        await _context.Sequences.AddAsync(setting);
                        //    }
                        //    else
                        //    {
                        //        invoiceSeq = setting.InvoiceSeq + 1;
                        //        setting.InvoiceSeq = invoiceSeq;
                        //        _context.Sequences.Update(setting);
                        //    }

                        //    obj.InvoiceNumber = invoiceSeq.ToString();
                        //    obj.SpInvoiceNumber = string.Empty;

                        //}

                        arInvoice.InvoiceStatus = "Closed";
                        _context.TranInvoices.Update(arInvoice);


                    TblFinTrnCustomerApproval approvalArInv = new()
                    {
                        CompanyId = (int)arInvoice.CompanyId,
                        BranchCode = arInvoice.BranchCode,
                        TranDate = DateTime.Now,
                        TranSource = "SD",
                        Trantype =arInvoice.IsCreditConverted ? "Credit" : "Invoice",
                        CustCode = customer.CustCode,
                        DocNum = "DocNum",
                        LoginId = request.User.UserId,
                        AppRemarks = "Automatic Approval From Snd",
                        InvoiceId = arInvoice.Id,
                        IsApproved = true,
                    };

                    await _context.TrnCustomerApprovals.AddAsync(approvalArInv);
                    await _context.SaveChangesAsync();






                    await _context.SaveChangesAsync();

                        arInvoice.TotalAmount = arInvoice.TotalAmount ?? 0;


                    string PaymentType = "Credit";
                    if (await _context.TblSndTranInvoicePaymentSettlementsList.AnyAsync(e=>e.InvoiceId==invoice.Id))
                    {
                        PaymentType = "Cash";
                    }




                        TblFinTrnCustomerInvoice cInvoice = new()
                        {
                            CompanyId = (int)arInvoice.CompanyId,
                            BranchCode = arInvoice.BranchCode,
                            InvoiceNumber = arInvoice.InvoiceNumber,// invoiceSeq.ToString(),
                            InvoiceDate = arInvoice.InvoiceDate,
                            CreditDays = paymentTerms.SalesTermsDueDays,
                            DueDate = arInvoice.InvoiceDueDate,
                            TranSource = "SD",
                            Trantype = arInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            CustCode = customer.CustCode,
                            DocNum = arInvoice.InvoiceRefNumber,
                            LoginId = request.User.UserId,
                            ReferenceNumber = arInvoice.InvoiceRefNumber,
                            InvoiceAmount = arInvoice.TotalAmount,
                            DiscountAmount = arInvoice.DiscountAmount ?? 0,
                            NetAmount = arInvoice.TotalAmount,
                         //  PaidAmount = Utility.IsNotCreditPay(request.Input.PaymentType) ? arInvoice.TotalAmount : 0,
                           PaidAmount = Utility.IsNotCreditPay(PaymentType) ? arInvoice.TotalAmount : 0,
                            AppliedAmount = 0,
                            Remarks1 = arInvoice.Remarks,
                            Remarks2 = "Settled From Snd",
                            InvoiceId = arInvoice.Id,
                            IsPaid = true,
                        };
                        cInvoice.BalanceAmount = cInvoice.NetAmount - cInvoice.PaidAmount;
                        await _context.TrnCustomerInvoices.AddAsync(cInvoice);

                        TblFinTrnCustomerStatement cStatement = new()
                        {
                            CompanyId = (int)arInvoice.CompanyId,
                            BranchCode = arInvoice.BranchCode,
                            TranDate = postedOn,
                            TranSource = "SD",
                            Trantype = arInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            TranNumber = arInvoice.InvoiceNumber,// invoiceSeq.ToString(),
                            CustCode = customer.CustCode,
                            DocNum = "DocNum",
                            ReferenceNumber = arInvoice.InvoiceRefNumber,
                           // PaymentType =input.PaymentType,
                            PaymentType = PaymentType,
                            PamentCode = "paycode",
                            CheckNumber = "",
                            Remarks1 = arInvoice.Remarks,
                            Remarks2 = "SnD Invoice",
                            LoginId = request.User.UserId,
                            DrAmount = !arInvoice.IsCreditConverted ? arInvoice.TotalAmount : 0,
                            CrAmount = arInvoice.IsCreditConverted ? arInvoice.TotalAmount : 0,
                          
                            InvoiceId = arInvoice.Id,
                        };
                        await _context.TrnCustomerStatements.AddAsync(cStatement);


                     // if (IsNotCreditPay(request.Input.PaymentType))
                      if (IsNotCreditPay(PaymentType))
                        {
                            TblFinTrnCustomerStatement cPaymentStatement = new()
                            {
                                CompanyId = (int)arInvoice.CompanyId,
                                BranchCode = arInvoice.BranchCode,
                                TranDate = postedOn,
                                TranSource = "SD",
                                Trantype = "Payment",
                                TranNumber = arInvoice.InvoiceNumber,// invoiceSeq.ToString(),
                                CustCode = customer.CustCode,
                                DocNum = "DocNum",
                                ReferenceNumber = arInvoice.InvoiceRefNumber,
                                PaymentType = PaymentType,
                                PamentCode = "Paycode",
                                CheckNumber ="",
                                Remarks1 = arInvoice.Remarks,
                                Remarks2 = "SnD Invoice",
                                LoginId = request.User.UserId,
                                DrAmount = 0,
                                CrAmount = arInvoice.TotalAmount,
                                InvoiceId = arInvoice.Id,
                            };
                            await _context.TrnCustomerStatements.AddAsync(cPaymentStatement);
                        }
                    

                        string accountCode = string.Empty;
                 //if (IsNotCreditPay(request.Input.PaymentType))
                 if (IsNotCreditPay(PaymentType))
                    {
                        var SndSettlement = await _context.TblSndTranInvoicePaymentSettlementsList.FirstOrDefaultAsync(e => e.InvoiceId == invoice.Id);

                        //var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == input.PayCode);
                        var payCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.FinPayCode == SndSettlement.PaymentCode);
                            accountCode = payCode.FinPayAcIntgrAC;
                        }

                        var customerMst = await _context.OprCustomers.FirstOrDefaultAsync(e => e.Id == arInvoice.CustomerId);

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = arInvoice.Id,
                            //FinAcCode = IsNotCreditPay(request.Input.PaymentType) ? accountCode : customerMst.CustArAcCode,
                            FinAcCode = IsNotCreditPay(PaymentType) ? accountCode : customerMst.CustArAcCode,
                            CrAmount = arInvoice.IsCreditConverted ? arInvoice.TotalAmount : 0,
                            DrAmount = !arInvoice.IsCreditConverted ? arInvoice.TotalAmount : 0,
                            Source = "SD",
                            //Type = IsNotCreditPay(request.Input.PaymentType) ? "paycode" : "Customer",
                            Type = IsNotCreditPay(PaymentType) ? "paycode" : "Customer",
                            Gl = string.Empty,
                            CreatedOn = postedOn
                        };

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = arInvoice.Id,
                            FinAcCode = customerMst.CustDefExpAcCode,
                            CrAmount = !arInvoice.IsCreditConverted ? (arInvoice.TotalAmount - arInvoice.TaxAmount) : 0,
                            DrAmount = arInvoice.IsCreditConverted ? (arInvoice.TotalAmount - arInvoice.TaxAmount) : 0,
                            Source = "SD",
                            Gl = string.Empty,
                            Type = "Expense",
                            CreatedOn = postedOn
                        };


                        await _context.FinDistributions.AddAsync(distribution1);
                        await _context.FinDistributions.AddAsync(distribution2);

                        var invoiceItem = await _context.TranInvoiceItems.FirstOrDefaultAsync(e => e.InvoiceId == arInvoice.Id);
                        var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(invoiceItem.TaxTariffPercentage).ToString());
                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        if (tax is not null)
                        {
                           
                            TblFinTrnDistribution distribution3 = new()
                            {
                                InvoiceId = arInvoice.Id,
                                FinAcCode = tax?.OutputAcCode01,
                                CrAmount = !arInvoice.IsCreditConverted ? arInvoice.TaxAmount : 0,
                                DrAmount = arInvoice.IsCreditConverted ? arInvoice.TaxAmount : 0,
                                Source = "SD",
                                Gl = string.Empty,
                                Type = "VAT",
                                CreatedOn = postedOn
                            };

                            await _context.FinDistributions.AddAsync(distribution3);
                            distributionsList.Add(distribution3);
                        }
                        await _context.SaveChangesAsync();


                      
                        var custAmt = _context.TrnCustomerStatements.Where(e => e.CustCode == customer.CustCode);
                        var custInvAmt = (await custAmt.SumAsync(e => e.DrAmount) - await custAmt.SumAsync(e => e.CrAmount));

                        customer.CustOutStandBal = custInvAmt;
                        _context.OprCustomers.Update(customer);
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



                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)arInvoice.CompanyId,
                            BranchCode = arInvoice.BranchCode,
                            Batch = string.Empty,
                            Source = "SD",
                            Remarks = arInvoice.Remarks,
                            Narration = arInvoice.InvoiceNotes ?? arInvoice.Remarks,
                            JvDate = postedOn,
                            Amount = arInvoice.TotalAmount ?? 0,
                            DocNum = arInvoice.InvoiceNumber,
                            CDate = createdOn,
                            Posted = true,
                            PostedDate = postedOn,
                            SiteCode = arInvoice.SiteCode

                        };

                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();

                        var jvId = JV.Id;



                        var branchAuths = await _context.FinBranchesAuthorities.Select(e => new { e.FinBranchCode, e.AppAuth })
                            .Where(e => e.FinBranchCode == arInvoice.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = (int)arInvoice.CompanyId,
                                    BranchCode = arInvoice.BranchCode,
                                    JvDate = postedOn,
                                    TranSource = "SD",
                                    Trantype = arInvoice.IsCreditConverted ? "Credit" : "Invoice",
                                    DocNum = arInvoice.InvoiceRefNumber,
                                    LoginId = Convert.ToInt32(item.AppAuth),
                                    AppRemarks = arInvoice.Remarks,
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
                                BranchCode = arInvoice.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = arInvoice.Remarks,
                                CrAmount = obj1.CrAmount,
                                DrAmount = obj1.DrAmount,
                                FinAcCode = obj1.FinAcCode,
                                Description = arInvoice.InvoiceNotes,
                                CostAllocation = costallocations.Id,
                                CostSegCode = customer.CustCode,
                                SiteCode = arInvoice.SiteCode

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
                            Remarks1 = arInvoice.Remarks,
                            Remarks2 = "SnD Invoice",
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
                                TransDate = postedOn,
                                PostedFlag = true,
                                PostDate = postedOn,
                                Jvnum = item.JournalVoucherId.ToString(),
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "SD",
                                ExRate = 0,
                                SiteCode = arInvoice.SiteCode
                            };
                            ledgerList.Add(ledger);
                        }
                        if (ledgerList.Count > 0)
                        {
                            await _context.AccountsLedgers.AddRangeAsync(ledgerList);
                            await _context.SaveChangesAsync();
                        }
                
                    #endregion




                    await transaction.CommitAsync();

                    return new() { IsSuccess = true, ErrorMsg = "" };
                }
                catch (Exception ex)
                {
                  await  transaction.RollbackAsync();
                    Log.Error("Error in PostSndInvoiceByInvoiceId Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess = false, ErrorMsg = ex.Message };

                }
            }
        }

    }

    #endregion






    #region CancelSndInvoice
    public class CancelSndInvoice : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public long id { get; set; }
    }
    public class CancelSndInvoiceQueryHandler : IRequestHandler<CancelSndInvoice, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CancelSndInvoiceQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CancelSndInvoice request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info CancelSndInvoice method start----");

                var invoice = await _context.SndTranInvoice.FirstOrDefaultAsync(e => e.Id == request.id);
                if (invoice == null)
                    return -1;
                invoice.IsVoid = true;
                invoice.UpdatedBy = request.User.UserId;
                invoice.UpdatedOn = DateTime.UtcNow;

                _context.SndTranInvoice.Update(invoice);
                await _context.SaveChangesAsync();
                return request.id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CancelSndInvoice Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;

            }
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }
    #endregion


}
