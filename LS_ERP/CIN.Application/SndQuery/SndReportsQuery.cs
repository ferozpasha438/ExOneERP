using AutoMapper;
using CIN.Application.Common;
using CIN.Application.SndDtos;
using CIN.Application.SndDtos.Comman;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SndQuery
{
    #region GetSndInvoiceReportQuery

    public class GetSndInvoiceReportQuery : IRequest<SndInvoiceSumaryListReport>
    {
        public bool? IsSummary { get; set; }
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Type { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetSndInvoiceReportQueryHandler : IRequestHandler<GetSndInvoiceReportQuery, SndInvoiceSumaryListReport>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndInvoiceReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndInvoiceSumaryListReport> Handle(GetSndInvoiceReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            bool isArab = request.User.Culture.IsArab();
            SndInvoiceSumaryListReport Report = new()
            {

                SummaryList = new(),
                DetailList = new(),

                TotAmount = 0,
                TotDiscount = 0,
                TotNetAmountBT = 0,
                TotSalesAmount = 0,
                TotTaxAmount = 0,
                TotCost = 0,
                TotGrossMargin = 0,
                TotGrossMarginPer = 0,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                 
            };

            try
            {
                request.DateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                request.DateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                request.DateFrom = request.DateFrom.Value.AddSeconds(-1 * request.DateFrom.Value.Second);
                request.DateTo = request.DateTo.Value.AddSeconds(-1 * request.DateTo.Value.Second - 1).AddDays(1);
                var invoices = request.DateFrom is not null && request.DateTo is not null? _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e=>e.SysWarehouse).Where(e=>e.InvoiceDate>=request.DateFrom && e.InvoiceDate<=request.DateTo ).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking(): _context.SndTranInvoice.Include(e => e.SysCustomer).OrderByDescending(e=>e.InvoiceDate).ThenByDescending(e=>e.Id);

                if (!string.IsNullOrEmpty(request.Type))
                {
                    invoices = request.Type switch
                    {
                        "All" => invoices,
                        "Approved" => invoices.Where(e => e.IsApproved),
                        "NotApproved" => invoices.Where(e => !e.IsApproved),
                        "Settled" => invoices.Where(e => e.IsSettled),
                        "NotSettled" => invoices.Where(e => !e.IsSettled),
                        "Posted" => invoices.Where(e => e.IsPosted),
                        "NotPosted" => invoices.Where(e => e.IsPosted),
                        _ => invoices
                    };
                }

                if (!string.IsNullOrEmpty(request.CustCode))
                {
                    invoices = invoices.Where(e => e.CustomerCode == request.CustCode);
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    invoices = invoices.Where(e => e.WarehouseCode == request.WhCode);
                }



                List<SndInvoiceSumaryItem> summaryList = new();
                var custApproval = await _context.TblSndTrnApprovalsList.Where(e => e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice).ToListAsync();
                Report.TotalItemsCount = invoices.Count();
                invoices = invoices.Skip(request.PageNumber * request.PageSize).Take(request.PageSize);
                foreach (var inv in invoices)
                {
                    decimal TotalAmount = inv.IsCreditConverted ? -inv.TotalAmount.Value : inv.TotalAmount.Value;
                    decimal TotalCost = inv.IsCreditConverted ? -inv.TotalCost ?? 0 : inv.TotalCost ?? 0;


                    summaryList.Add(new()
                    {
                        Id = inv.Id,
                        InvoiceNumber = inv.InvoiceNumber,
                        CustomerId = inv.CustomerId,
                        IsCreditConverted = inv.IsCreditConverted,
                        //IsApproved=inv.IsApproved,
                        IsApproved = custApproval.Any(e =>/* e.AppAuth == request.User.UserId && */e.ServiceCode == inv.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.IsApproved),
                        IsPosted = inv.IsPosted,
                        IsQtyDeducted = inv.IsQtyDeducted,
                        IsSettled = inv.IsSettled,
                        Source = inv.Source,
                        InvoiceDate = inv.InvoiceDate,
                        InvoiceDueDate = inv.InvoiceDueDate,
                        AmountBeforeTax = inv.AmountBeforeTax,
                        AmountDue = inv.AmountDue,
                        DiscountAmount =inv.IsCreditConverted?-inv.DiscountAmount: inv.DiscountAmount,
                        FooterDiscount = inv.FooterDiscount,
                        SubTotal = inv.IsCreditConverted ? -inv.SubTotal:inv.SubTotal,
                        TotalAmount = TotalAmount,
                        TaxAmount = inv.IsCreditConverted ? -inv.TaxAmount:inv.TaxAmount,
                        TotalCost = TotalCost,

                        CustArbName = inv.CustArbName,
                        CustName = inv.CustName,
                        CustomerName = isArab ? (string.IsNullOrEmpty(inv.CustArbName) ? inv.SysCustomer.CustArbName : inv.CustArbName) : (string.IsNullOrEmpty(inv.CustName) ? inv.SysCustomer.CustName : inv.CustName),
                        CustomerCode = inv.SysCustomer.CustCode,

                        WarehouseCode=inv.WarehouseCode,
                        WarehouseName=inv.SysWarehouse.WHName,
                        GrossMargin=TotalAmount-TotalCost,
                        GrossMarginPer= Math.Abs(TotalAmount)>0?(TotalAmount - TotalCost) /Math.Abs(TotalAmount)*100:0
                    });

                }

                Report.SummaryList = summaryList;

                Report.TotAmount = (decimal)summaryList.Sum(e => e.SubTotal);
                Report.TotDiscount = (decimal)summaryList.Sum(e => e.DiscountAmount);
                Report.TotTaxAmount = (decimal)summaryList.Sum(e => e.TaxAmount);
                Report.TotNetAmountBT = (decimal)summaryList.Sum(e => e.AmountBeforeTax);
                Report.TotSalesAmount = (decimal)summaryList.Sum(e => e.TotalAmount);
                Report.TotCost = (decimal)summaryList.Sum(e => e.TotalCost);
                Report.TotGrossMargin = Report.TotSalesAmount - Report.TotCost;
                Report.TotGrossMarginPer = Math.Abs(Report.TotSalesAmount)>0? Report.TotGrossMargin / Math.Abs(Report.TotSalesAmount)*100:0;
                if (!request.IsSummary.Value)
                {
                   
                    summaryList.ForEach(e =>
                    {
                        var invoiceItems = _context.SndTranInvoiceItem.Include(i=>i.Item).Include(e=>e.Invoice).Where(i => i.InvoiceId == e.Id).ToList();

                        if (invoiceItems.Count > 0)
                        {
                            List<SndInvoiceDetailItemDto> itemList = new();
                            

                            invoiceItems.ForEach(ii => {

                                decimal convFactor =_context.InvItemsUOM.FirstOrDefault(e=>e.ItemCode==ii.ItemCode && e.ItemUOM==ii.UnitType).ItemConvFactor;
                        decimal NetCost = ii.Quantity.Value * convFactor * (ii.ItemAvgCost.Value);
                                itemList.Add(new() {
                                    ItemCode = ii.ItemCode,
                                    ItemAvgCost = ii.ItemAvgCost??0,
                                    DiscountAmount = ii.DiscountAmount,
                                    Quantity = !ii.Invoice.IsCreditConverted?ii.Quantity:-ii.Quantity,
                                    UnitType=ii.UnitType,
                                    UnitPrice = ii.UnitPrice,
                                    TaxAmount = !ii.Invoice.IsCreditConverted ? ii.TaxAmount.Value:- ii.TaxAmount.Value,
                                    SubTotal = !ii.Invoice.IsCreditConverted ? ii.SubTotal:- ii.SubTotal,
                                    TotalAmount = !ii.Invoice.IsCreditConverted ? ii.TotalAmount:- ii.TotalAmount,
                                    TaxTariffPercentage = ii.TaxTariffPercentage,
                                    ItemName = isArab ? ii.Item.ItemDescriptionAr : ii.Item.ItemDescription,
                                    ItemId = ii.Item.Id,
                                    Discount = ii.Discount,
                                    AmountBeforeTax = !ii.Invoice.IsCreditConverted ? ii.AmountBeforeTax:- ii.AmountBeforeTax,
                                    NetCost= !ii.Invoice.IsCreditConverted ? NetCost:-NetCost,
                                    GrossMargin = !ii.Invoice.IsCreditConverted ? ii.TotalAmount.Value-NetCost:-(ii.TotalAmount.Value - NetCost),
                                     GrossMarginPer = Math.Abs(ii.TotalAmount.Value)>0?!ii.Invoice.IsCreditConverted ? (ii.TotalAmount.Value - NetCost)/Math.Abs(ii.TotalAmount.Value)*100:-(ii.TotalAmount.Value - NetCost)/Math.Abs(ii.TotalAmount.Value)*100:0
                                });


                            });
                            Report.DetailList.Add(new()
                            {
                                InvoiceSummary = e,
                                InvoiceLineItems = itemList
                            });

                        }

                           
                        });

                    

                }
                return Report;

            }
            catch (Exception e)
            {
                Report.DetailList = new();
                Report.SummaryList = new();
                
                return Report;
            }        }
        
    }

    #endregion



    #region GetSndItemSalesReportQuery

    public class GetSndItemSalesReportQuery : IRequest<SndItemSalesReportDto>
    {
        public bool? IsSummary { get; set; }
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemId { get; set; }
        public string Type { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetSndItemSalesReportQueryHandler : IRequestHandler<GetSndItemSalesReportQuery, SndItemSalesReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndItemSalesReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndItemSalesReportDto> Handle(GetSndItemSalesReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            bool isArab = request.User.Culture.IsArab();
            SndItemSalesReportDto Report = new()
            {

                TotAmount = 0,
                TotDiscount = 0,
                TotNetAmountBT = 0,
                TotSalesAmount = 0,
                TotTaxAmount = 0,
                TotCost = 0,
                TotGrossMargin = 0,
                TotGrossMarginPer = 0,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                SummaryReport = new(),
                DetailedReport = new(),
            };
            try
            {
                request.DateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                request.DateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                request.DateFrom = request.DateFrom.Value.AddSeconds(-1 * request.DateFrom.Value.Second);
                request.DateTo = request.DateTo.Value.AddSeconds(-1 * request.DateTo.Value.Second - 1).AddDays(1);
                var invoices = request.DateFrom is not null && request.DateTo is not null ? _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e => e.SysWarehouse).Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo && e.IsPosted).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking() : _context.SndTranInvoice.Where(e=>e.IsPosted).Include(e => e.SysCustomer).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking();

                



                var custApproval = await _context.TblSndTrnApprovalsList.Where(e => e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice).ToListAsync();
                
                var invoiceItems = _context.SndTranInvoiceItem.Include(e => e.Item).ThenInclude(e => e.InvUoms).Include(e => e.Invoice.SysWarehouse).Include(e => e.Invoice).ThenInclude(e => e.SysCustomer).AsNoTracking().Where(e => invoices.Any(it => it.Id == e.Invoice.Id)).ToList();
                var uoms = _context.InvItemsUOM;


                if (!string.IsNullOrEmpty(request.CustCode))
                {
                    invoiceItems = invoiceItems.Where(e => e.Invoice.SysCustomer.CustCode == request.CustCode).ToList();
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    invoiceItems = invoiceItems.FindAll(e => e.Invoice.SysWarehouse.WHCode == request.WhCode);
                }

                if (!string.IsNullOrEmpty(request.ItemId))
                {
                    invoiceItems = invoiceItems.FindAll(e => e.Item.Id.ToString() == request.ItemId);
                }

                if (!string.IsNullOrEmpty(request.Type))
                {
                    invoiceItems = request.Type switch
                    {
                        "All" => invoiceItems,
                        "Approved" => invoiceItems.Where(e=> custApproval.Any(a=>a.IsApproved && a.ServiceCode== e.Invoice.Id.ToString())).ToList(),
                        "NotApproved" => invoiceItems.Where(e => !custApproval.Any(a => a.IsApproved && a.ServiceCode == e.Invoice.Id.ToString())).ToList(),
                        "Settled" => invoiceItems.Where(e => e.Invoice.IsSettled).ToList(),
                        "NotSettled" => invoiceItems.Where(e => !e.Invoice.IsSettled).ToList(),
                        "Posted" => invoiceItems.Where(e => e.Invoice.IsPosted).ToList(),
                        "NotPosted" => invoiceItems.Where(e => !e.Invoice.IsPosted).ToList(),
                        _ => invoiceItems
                    };
                }


                foreach(var ii in invoiceItems)
                {
                    if (ii.Invoice.IsCreditConverted)
                    {
                        ii.Quantity = -ii.Quantity.Value;
                        ii.NetQuantity = -ii.NetQuantity.Value;
                        ii.AmountBeforeTax = -ii.AmountBeforeTax.Value;
                        ii.DiscountAmount = -ii.DiscountAmount.Value;
                        ii.SubTotal = -ii.SubTotal.Value;
                        ii.TotalAmount = -ii.TotalAmount.Value;
                        ii.TaxAmount = -ii.TaxAmount.Value;
                    }
                }


                var invoiceItemsGroups = invoiceItems.GroupBy(e => e.ItemCode).Select(g => new
                {
                  
                    ItemCode = g.Key.ToString(),
                    Group = g.ToList()
                }).ToList();

                Report.TotalItemsCount = invoiceItemsGroups.Count();
                invoiceItemsGroups = invoiceItemsGroups.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();


                foreach (var g in invoiceItemsGroups)
                {


                       decimal Amount = g.Group.Sum(e => e.SubTotal.Value);
                    decimal DiscountAmount =g.Group.Sum(e => e.DiscountAmount.Value);
                    decimal Quantity =g.Group.Sum(e => e.NetQuantity.Value);
                    decimal NetPrice =g.Group.Sum(e => e.AmountBeforeTax.Value);
                    decimal SalePrice =g.Group.Sum(e => e.TotalAmount.Value);
                    decimal TaxAmount =g.Group.Sum(e => e.TaxAmount.Value);
                    decimal NetCost =g.Group.Sum(e => e.NetQuantity.Value* e.ItemAvgCost.Value);
                    SndItemSaleSummaryItemDto summary = new() {
                        Id=g.Group.First().Item.Id,
                        ItemCode = g.ItemCode,
                        ItemName = isArab ? g.Group.First().Item.ShortNameAr : g.Group.First().Item.ShortName,
                        Amount = Amount,
                        DiscountAmount = DiscountAmount,
                        Quantity = Quantity,
                        NetPrice = NetPrice,
                        NetCost=  NetCost,
                        AvgCost = NetCost/Quantity,
                        SalePrice = SalePrice,
                        TaxAmount = TaxAmount,
                         GrossMargin=SalePrice - NetCost,
                          GrossMarginPer= Math.Abs(SalePrice)>0?(SalePrice - NetCost) / Math.Abs(SalePrice) * 100:0

                    };


                    Report.SummaryReport.Add(summary);

                    Report.TotAmount += Amount;
                    Report.TotNetAmountBT += NetPrice;
                    Report.TotTaxAmount += TaxAmount;
                    Report.TotDiscount += DiscountAmount;
                    Report.TotSalesAmount += SalePrice;
                    Report.TotCost += NetCost;

                    if (!request.IsSummary.Value)
                    {
                        SndItemSaleDetailedItemDto dReport = new();
                        List<SndDetailedItemLineDto> detailedLines = new();
                        foreach (var det in g.Group.GroupBy(e=>e.Invoice.InvoiceNumber).ToList())
                        {
                            decimal netCost = (decimal)( det.Sum(x=>x.NetQuantity.Value * x.ItemAvgCost.Value));
                            decimal saleAmount = (decimal) det.Sum(x=>x.TotalAmount.Value);
                            decimal grossMargin =(decimal) det.Sum(x=>x.TotalAmount.Value) - netCost;

                            SndDetailedItemLineDto line = new()
                            {
                                Id =det.First().Item.Id,
                                ItemCode =det.First().ItemCode,
                                  ItemName=!isArab?det.First().Item.ShortName:det.First().Item.ShortNameAr,
                                    InvoiceNumber=det.First().Invoice.InvoiceNumber,
                                    InvoiceDate=det.First().Invoice.InvoiceDate,
                                     CustomerCode=det.First().Invoice.CustomerCode,
                                     CustomerName=isArab?det.First().Invoice.SysCustomer.CustArbName:det.First().Invoice.SysCustomer.CustName,
                                WarehouseCode = det.First().Invoice.WarehouseCode,
                                Quantity = (decimal)det.Sum(x=>x.NetQuantity.Value),
                                Amount = (decimal)det.Sum(x=>x.SubTotal.Value),
                                       NetPrice=(decimal)det.Sum(x=>x.AmountBeforeTax.Value),
                                        DiscountAmount= (decimal)det.Sum(x=>x.DiscountAmount.Value),
                                          SalePrice=saleAmount,
                                           TaxAmount= (decimal)det.Sum(x=>x.TaxAmount.Value),
                                           AvgCost= (decimal)det.First().ItemAvgCost.Value,
                                            IsPosted=det.First().Invoice.IsPosted,
                                            IsQuantityDeducted=det.First().Invoice.IsQtyDeducted,
                                            isCreditConverted=det.First().Invoice.IsCreditConverted,
                                              NetCost= netCost,
                                              GrossMargin= grossMargin,
                                              GrossMarginPer= Math.Abs(saleAmount) > 0?  grossMargin /Math.Abs(saleAmount) *100:0,
                            };
                            detailedLines.Add(line);

                        }

                        dReport.Summary = summary;
                        dReport.ItemLines = detailedLines;



                        Report.DetailedReport.Add(dReport);
                        

                    }

                }


                Report.TotGrossMargin = Report.TotSalesAmount - Report.TotCost;
                Report.TotGrossMarginPer = Math.Abs(Report.TotSalesAmount)>0? Report.TotGrossMargin /Math.Abs(Report.TotSalesAmount) * 100:0;


                return Report;

            }
            catch (Exception e)
            {

                return Report;

                // return null;
            }
        }

    }

    #endregion

    #region GetSndCustomerSalesReportQuery

    public class GetSndCustomerSalesReportQuery : IRequest<SndCustomerSalesReportDto>
    {
        public bool? IsSummary { get; set; }
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetSndCustomerSalesReportQueryHandler : IRequestHandler<GetSndCustomerSalesReportQuery, SndCustomerSalesReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndCustomerSalesReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndCustomerSalesReportDto> Handle(GetSndCustomerSalesReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            bool isArab = request.User.Culture.IsArab();
            SndCustomerSalesReportDto Report = new()
            {

                TotAmount = 0,
                TotDiscount = 0,
                TotNetAmountBT = 0,
                TotSalesAmount = 0,
                TotTaxAmount = 0,
                TotCost = 0,
                TotGrossMargin = 0,
                TotGrossMarginPer = 0,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                SummaryReport = new(),
            };
            try
            {
                request.DateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                request.DateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                request.DateFrom = request.DateFrom.Value.AddSeconds(-1 * request.DateFrom.Value.Second);
                request.DateTo = request.DateTo.Value.AddSeconds(-1 * request.DateTo.Value.Second - 1).AddDays(1);
                var invoices = request.DateFrom is not null && request.DateTo is not null ? _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e => e.SysWarehouse).Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo && e.IsPosted).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking() : _context.SndTranInvoice.Include(e => e.SysCustomer).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking();

               


                List<SndInvoiceSumaryItem> invoiceList = new();
                foreach (var inv in invoices)
                {
                    decimal totalAmount = inv.IsCreditConverted ? -inv.TotalAmount ?? 0 : inv.TotalAmount ?? 0;
                    decimal totalCost = inv.IsCreditConverted ? -inv.TotalCost ?? 0 : inv.TotalCost ?? 0;
                    invoiceList.Add(new()
                    {
                        Id = inv.Id,
                        InvoiceNumber = inv.InvoiceNumber,
                        CustomerId = inv.CustomerId,
                        IsCreditConverted = inv.IsCreditConverted,
                        //IsApproved=inv.IsApproved,
                        //IsApproved = custApproval.Any(e =>/* e.AppAuth == request.User.UserId && */e.ServiceCode == inv.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.IsApproved),
                        IsPosted = inv.IsPosted,
                        IsQtyDeducted = inv.IsQtyDeducted,
                        IsSettled = inv.IsSettled,
                        Source = inv.Source,
                        InvoiceDate = inv.InvoiceDate,
                        InvoiceDueDate = inv.InvoiceDueDate,
                        AmountBeforeTax = inv.IsCreditConverted ? -inv.AmountBeforeTax: inv.AmountBeforeTax,
                        AmountDue = inv.AmountDue,
                        DiscountAmount = inv.IsCreditConverted ? -inv.DiscountAmount : inv.DiscountAmount,
                        FooterDiscount = inv.FooterDiscount,
                        SubTotal = inv.IsCreditConverted ? -inv.SubTotal : inv.SubTotal,
                        TotalAmount = totalAmount,
                        TaxAmount = inv.IsCreditConverted ? -inv.TaxAmount : inv.TaxAmount,
                        TotalCost = totalCost,
                        CustArbName = inv.CustArbName,
                        CustName = inv.CustName,
                        CustomerName = isArab ? (string.IsNullOrEmpty(inv.CustArbName) ? inv.SysCustomer.CustArbName : inv.CustArbName) : (string.IsNullOrEmpty(inv.CustName) ? inv.SysCustomer.CustName : inv.CustName),
                        CustomerCode = inv.SysCustomer.CustCode,
                        SysCustomer=inv.SysCustomer,
                        WarehouseCode = inv.WarehouseCode,
                        WarehouseName = inv.SysWarehouse.WHName,
                        GrossMargin = totalAmount - totalCost,
                        GrossMarginPer = totalAmount==0?0:(totalAmount - totalCost) / Math.Abs(totalAmount) * 100,
                    });

                }
                if (!string.IsNullOrEmpty(request.CustCode))
                {
                    invoiceList = invoiceList.Where(e => e.CustomerCode == request.CustCode).ToList();
                }

                var InvoicesGroupByCustomer = invoiceList.GroupBy(e => e.CustomerCode);
                Report.TotalItemsCount = InvoicesGroupByCustomer.Count();
                InvoicesGroupByCustomer = InvoicesGroupByCustomer.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();
                foreach (var igc in InvoicesGroupByCustomer) 
                {
                    decimal totalAmount =igc.Sum(e => e.TotalAmount.Value);
                    decimal totalCost = igc.Sum(e => e.TotalCost.Value);
                    
                    SndCustomerSalesSummaryItemDto invSummary = new()
                    {

                         CustomerCode=igc.First().SysCustomer.CustCode,
                         CustomerName=isArab?igc.First().SysCustomer.CustArbName: igc.First().SysCustomer.CustName,
                           SubTotal=igc.Sum(e=>e.SubTotal.Value),
                            TaxAmount=igc.Sum(e=>e.TaxAmount.Value),
                             InvoiceCount=igc.Count(),
                              InvoicesSummaryList=igc.ToList(),
                                DiscountAmount=igc.Sum(e=>e.DiscountAmount.Value),
                        TotalAmount=totalAmount,
                        TotalCost= totalCost,
                                  GrossMargin =(totalAmount -totalCost),
                                  GrossMarginPer= totalAmount==0?0:((totalAmount - totalCost)/Math.Abs(totalAmount)*100),
                                   AmountBeforeTax=igc.Sum(e=>e.AmountBeforeTax.Value),
                                   Count=igc.Count()
                    };
                    Report.SummaryReport.Add(invSummary);

                }


                Report.TotAmount = Report.SummaryReport.Sum(e=>e.SubTotal);
                Report.TotCost = Report.SummaryReport.Sum(e=>e.TotalCost);
                Report.TotSalesAmount = Report.SummaryReport.Sum(e=>e.TotalAmount);
                Report.TotDiscount = Report.SummaryReport.Sum(e=>e.DiscountAmount);
                Report.TotTaxAmount= Report.SummaryReport.Sum(e=>e.TaxAmount);
                Report.TotNetAmountBT= Report.SummaryReport.Sum(e=>e.AmountBeforeTax);
                Report.TotGrossMargin = Report.TotSalesAmount - Report.TotCost;
                Report.TotGrossMarginPer = Report.TotSalesAmount==0?0: Report.TotGrossMargin / Math.Abs(Report.TotSalesAmount) * 100;
                Report.TotCount = invoiceList.Count();

                return Report;

            }
            catch (Exception e)
            {

                return Report;
                //  return null;
            }
        }

    }

    #endregion
 #region GetCustomerSalesMonthlyReportQuery

    public class GetCustomerSalesMonthlyReportQuery : IRequest<SndCustomerSalesMonthlyReportDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetCustomerSalesMonthlyReportQueryHandler : IRequestHandler<GetCustomerSalesMonthlyReportQuery, SndCustomerSalesMonthlyReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerSalesMonthlyReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndCustomerSalesMonthlyReportDto> Handle(GetCustomerSalesMonthlyReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            bool isArab = request.User.Culture.IsArab();
            SndCustomerSalesMonthlyReportDto Report = new()
            {
                Columns = new(),
                TotSalesAmount = 0,
                TotCost = 0,
                TotGrossMargin = 0,
                TotGrossMarginPer = 0,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                MonthlyReports = new(),
                TotalItemsCount=0,
            };

            try
            {
                request.DateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                request.DateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                request.DateFrom = request.DateFrom.Value.AddSeconds(-1 * request.DateFrom.Value.Second);
                request.DateTo = request.DateTo.Value.AddSeconds(-1 * request.DateTo.Value.Second - 1).AddDays(1);


                //DateTime FromDate =request.DateFrom.Value.AddDays(1-request.DateFrom.Value.Day);
                //DateTime StartDateOfLastMonth = request.DateTo.Value.AddDays(1 - request.DateTo.Value.Day);
                //DateTime ToDate = request.DateTo.Value.AddDays(1 - request.DateTo.Value.Day).AddMonths(1).AddSeconds(-1);

              
                var invoices =  _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e => e.SysWarehouse).Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo && e.IsPosted).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking() ;
               // var invoices = request.DateFrom is not null && request.DateTo is not null ? _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e => e.SysWarehouse).Where(e => e.InvoiceDate >= FromDate && e.InvoiceDate <= ToDate && e.IsPosted).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking() : _context.SndTranInvoice.Include(e => e.SysCustomer).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking();
                List<SndInvoiceSumaryItem> invoiceList = new();
                foreach (var inv in invoices)
                {
                    decimal totalAmount = inv.IsCreditConverted ? -inv.TotalAmount ?? 0 : inv.TotalAmount ?? 0;
                    decimal totalCost = inv.IsCreditConverted ? -inv.TotalCost ?? 0 : inv.TotalCost ?? 0;
                    invoiceList.Add(new()
                    {
                        Id = inv.Id,
                        InvoiceNumber = inv.InvoiceNumber,
                        CustomerId = inv.CustomerId,
                        IsCreditConverted = inv.IsCreditConverted,
                        //IsApproved=inv.IsApproved,
                        //IsApproved = custApproval.Any(e =>/* e.AppAuth == request.User.UserId && */e.ServiceCode == inv.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.IsApproved),
                        IsPosted = inv.IsPosted,
                        IsQtyDeducted = inv.IsQtyDeducted,
                        IsSettled = inv.IsSettled,
                        Source = inv.Source,
                        InvoiceDate = inv.InvoiceDate,
                        InvoiceDueDate = inv.InvoiceDueDate,
                        AmountBeforeTax = inv.AmountBeforeTax,
                        AmountDue = inv.AmountDue,
                        DiscountAmount = inv.IsCreditConverted ? -inv.DiscountAmount : inv.DiscountAmount,
                        FooterDiscount = inv.FooterDiscount,
                        SubTotal = inv.IsCreditConverted ? -inv.SubTotal : inv.SubTotal,
                        TotalAmount = totalAmount,
                        TaxAmount = inv.IsCreditConverted ? -inv.TaxAmount : inv.TaxAmount,
                        TotalCost = totalCost,
                        CustArbName = inv.CustArbName,
                        CustName = inv.CustName,
                        CustomerName = isArab ? (string.IsNullOrEmpty(inv.CustArbName) ? inv.SysCustomer.CustArbName : inv.CustArbName) : (string.IsNullOrEmpty(inv.CustName) ? inv.SysCustomer.CustName : inv.CustName),
                        CustomerCode = inv.SysCustomer.CustCode,
                        SysCustomer=inv.SysCustomer,
                        WarehouseCode = inv.WarehouseCode,
                        WarehouseName = inv.SysWarehouse.WHName,
                        GrossMargin = totalAmount-totalCost,
                        GrossMarginPer = totalAmount==0?0:(totalAmount - totalCost) / Math.Abs(totalAmount) * 100,
                    });

                }
                if (!string.IsNullOrEmpty(request.CustCode))
                {
                    invoiceList = invoiceList.Where(e => e.CustomerCode == request.CustCode).ToList();
                }


                List<SndCustomerSalesMonthlyReportItem> monthlyReports = new();
                var InvoicesGroupByCustomer = invoiceList.GroupBy(e => e.CustomerCode);

                Report.TotalItemsCount = InvoicesGroupByCustomer.Count();
                InvoicesGroupByCustomer = InvoicesGroupByCustomer.Skip(request.PageNumber * request.PageSize).Take(request.PageSize);
               
                foreach (var igc in InvoicesGroupByCustomer) 
                {
                   decimal Cost = igc.Sum(e => e.TotalCost.Value);
                       decimal SalesAmount = igc.Sum(e => e.TotalAmount.Value);
                       

                    SndCustomerSalesMonthlyReportItem monthlyReport = new() {
                        CustomerCode = igc.First().CustomerCode,
                        CustomerName = igc.First().CustomerName,
                        Cost = Cost,
                        SalesAmount = SalesAmount,
                        GrossMargin = SalesAmount - Cost,
                        GrossMarginPer = SalesAmount == 0 ? 0: (SalesAmount - Cost)/Math.Abs(SalesAmount)*100,
                    };
                    //for (DateTime d = FromDate; d < ToDate; d = d.AddMonths(1))
                    for (DateTime d = request.DateFrom.Value; d < request.DateTo.Value; d = d.AddMonths(1))
                    {
                        if(!Report.Columns.Any(e=>e.Month==d.Month && e.Year==d.Year))
                            Report.Columns.Add(d);
                        var invs = igc.Where(e => e.InvoiceDate >= d && e.InvoiceDate <= (d.AddMonths(1).AddSeconds(-1)));
                        decimal cost = (decimal)invs.Sum(e => e.TotalCost);
                           decimal salesAmount = (decimal)invs.Sum(e => e.TotalAmount);

                        SndCustomerSalesMonthlyReportItem monthlyReportForCustomer = new()
                        {
                            CustomerCode=igc.First().CustomerCode,
                            CustomerName=igc.First().CustomerName,
                            Cost=cost,
                            SalesAmount=salesAmount,
                            MonthDt=d,
                             GrossMargin=salesAmount-cost,
                             GrossMarginPer= salesAmount==0?0:(salesAmount - cost)/Math.Abs(salesAmount) *100,
                        };
                        monthlyReport.MonthlyReportsPerCustomer.Add(monthlyReportForCustomer);
                    }
                 
                    
                    monthlyReports.Add(monthlyReport);



                }
                Report.MonthlyReports = monthlyReports;

                Report.TotCost = invoiceList.Sum(e=>e.TotalCost.Value);
                Report.TotSalesAmount = invoiceList.Sum(e=>e.TotalAmount.Value);
                Report.TotGrossMargin = Report.TotSalesAmount - Report.TotCost;
                Report.TotGrossMarginPer = Report.TotSalesAmount==0?0:Report.TotGrossMargin / Math.Abs(Report.TotSalesAmount) * 100;
                Report.TotCount = invoiceList.Count();

                //for (DateTime d = FromDate; d < ToDate; d = d.AddMonths(1))
                for (DateTime d = request.DateFrom.Value; d < request.DateTo.Value; d = d.AddMonths(1))
                {
                    decimal cost = Report.MonthlyReports.Sum(e => e.MonthlyReportsPerCustomer.Where(f => f.MonthDt == d).Sum(g => g.Cost));
                    decimal salesAmount = Report.MonthlyReports.Sum(e => e.MonthlyReportsPerCustomer.Where(f => f.MonthDt == d).Sum(g => g.SalesAmount));
                    decimal grossMargin = salesAmount-cost;
                    Report.MonthlyTotals.Add(new() { 
                    Cost=cost,
                    SalesAmount=salesAmount,
                    GrossMargin= grossMargin,
                    GrossMarginPer= salesAmount == 0 ? 0 : grossMargin / Math.Abs(salesAmount) * 100,
                    MonthDt =d, 
                    });
                
                }


                    return Report;

            }
            catch (Exception e)
            {

                return Report;
            }
        }

    }

    #endregion





    #region GetItemDepartmentReportQuery

    public class GetItemDepartmentReportQuery : IRequest<SndItemDepartmentReportDto>
    {
        public bool? IsSummary { get; set; }
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemId { get; set; }
        public string ItemCategory { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetItemDepartmentReportQueryHandler : IRequestHandler<GetItemDepartmentReportQuery, SndItemDepartmentReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemDepartmentReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndItemDepartmentReportDto> Handle(GetItemDepartmentReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
                var itemCategories =  _context.InvCategories;
                var itemsCategoriesSelectionList = _context.SndTranInvoiceItem.Select(g => new CustomSelectListItem
                {
                    Text = g.Item.InvCategory.ItemCatName,
                    Value = g.Item.ItemCat,
                    TextTwo =g.Item.ItemCat+"-"+g.Item.InvCategory.ItemCatName
                }).ToList();

            bool isArab = request.User.Culture.IsArab();
            SndItemDepartmentReportDto Report = new()
            {


                TotSalesAmount = 0,
                TotCost = 0,
                TotGrossMargin = 0,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                SummaryReport = new(),
                DetailedReport = new(),


                ItemCategoriesSelectionList = itemsCategoriesSelectionList.GroupBy(e=>e.Value).Select(g => new CustomSelectListItem
                {
                    Text = g.First().Text,
                    Value = g.First().Value,
                    TextTwo = g.First().TextTwo
                }).ToList(),
            };
            
                request.DateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
               request.DateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                request.DateFrom = request.DateFrom.Value.AddSeconds(-1 * request.DateFrom.Value.Second);
                request.DateTo = request.DateTo.Value.AddSeconds(-1 * request.DateTo.Value.Second - 1).AddDays(1);

                var invoices = request.DateFrom is not null && request.DateTo is not null ? _context.SndTranInvoice.Include(e => e.SysCustomer).Include(e => e.SysWarehouse).Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking() : _context.SndTranInvoice.Include(e => e.SysCustomer).OrderByDescending(e => e.InvoiceDate).ThenByDescending(e => e.Id).AsNoTracking();

                invoices = invoices.Where(e => e.IsQtyDeducted.Value);



                var custApproval = await _context.TblSndTrnApprovalsList.Where(e => e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice).ToListAsync();

                var invoiceItems = _context.SndTranInvoiceItem.Include(e=>e.Item.InvCategory).Include(e => e.Item).ThenInclude(e => e.InvUoms).Include(e => e.Invoice.SysWarehouse).Include(e => e.Invoice).ThenInclude(e => e.SysCustomer).AsNoTracking().Where(e => invoices.Any(it => it.Id == e.Invoice.Id)).ToList();
                var uoms = _context.InvItemsUOM;


                if (!string.IsNullOrEmpty(request.CustCode))
                {
                    invoiceItems = invoiceItems.Where(e => e.Invoice.SysCustomer.CustCode == request.CustCode).ToList();
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    invoiceItems = invoiceItems.FindAll(e => e.Invoice.SysWarehouse.WHCode == request.WhCode);
                }

                if (!string.IsNullOrEmpty(request.ItemId))
                {
                    invoiceItems = invoiceItems.FindAll(e => e.Item.Id.ToString() == request.ItemId);
                }

                if (!string.IsNullOrEmpty(request.ItemCategory))
                {
                    invoiceItems = invoiceItems.FindAll(e => e.Item.ItemCat == request.ItemCategory);
                }

                foreach (var ii in invoiceItems) {
                    if (ii.Invoice.IsCreditConverted)
                    {
                        ii.NetQuantity = -ii.NetQuantity;
                        ii.Quantity = -ii.Quantity;
                        ii.TotalAmount = -ii.TotalAmount;
                    }
                
                }


                Report.TotSalesAmount = invoiceItems.Sum(e=>e.TotalAmount.Value);
                Report.TotCost = invoiceItems.Sum(e=>e.NetQuantity.Value*e.ItemAvgCost.Value);
                Report.TotGrossMargin = Report.TotSalesAmount - Report.TotCost;

                var invoiceItemsGroupByItemCategory = invoiceItems.GroupBy(g => g.Item.ItemCat).ToList();

                List<SndItemDepartmentReportSummaryItem> summaryList = new();
                Report.TotalItemsCount = invoiceItemsGroupByItemCategory.Count();
                invoiceItemsGroupByItemCategory = invoiceItemsGroupByItemCategory.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();

                invoiceItemsGroupByItemCategory.ForEach(g =>
                {
                    summaryList.Add(new SndItemDepartmentReportSummaryItem
                    {
                        ItemType = g.First().Item.ItemType,
                        ItemCategory=g.First().Item.ItemCat,
                        ItemCategoryName=g.First().Item.InvCategory.ItemCatName,
                        SalesAmount = g.Sum(e => e.TotalAmount.Value),
                        Cost = g.Sum(e => e.NetQuantity.Value * e.ItemAvgCost.Value),
                        GrossMargin = g.Sum(e => e.TotalAmount.Value) - g.Sum(e => e.NetQuantity.Value * e.ItemAvgCost.Value),
                        GrossMarginPer = 0,  //calculate @front-end
                        NetQuantity = g.Sum(e => e.NetQuantity.Value)
                    });
                });


               

                Report.SummaryReport = summaryList;

                 

                    if (!request.IsSummary.Value)
                    {
                    foreach (var sr in summaryList)
                    {
                        //var summaryListGroupByItem = invoiceItems.Where(e => e.Item.ItemType == sr.ItemType).GroupBy(g => new { g.ItemCode, g.Invoice.InvoiceNumber});
                        var summaryListGroupByItem = invoiceItems.Where(e => e.Item.ItemCat == sr.ItemCategory).GroupBy(g => new { g.ItemCode});
                        List<SndInvoiceDetailItemDto> detailItems = new();
                        foreach( var slgbi in summaryListGroupByItem)
                        {
                            decimal totalAmount = slgbi.Sum(e => e.TotalAmount.Value) ;
                            decimal netCost = slgbi.Sum(e => e.ItemAvgCost.Value * e.NetQuantity.Value);
            
                            detailItems.Add(new() { 
                              ItemName=isArab?slgbi.First().Item.ShortNameAr:slgbi.First().Item.ShortName,
                               ItemCode=slgbi.First().Item.ItemCode,
                               ItemId= slgbi.First().Item.Id,
                               TotalAmount= totalAmount,
                               NetCost=netCost,
                               NetQuantity=slgbi.Sum(e=>e.NetQuantity),
                               GrossMargin=totalAmount-netCost,
                               GrossMarginPer= totalAmount==0?0:(totalAmount - netCost)/Math.Abs(totalAmount)*100,
                             //  InvoiceNumber=slgbi.First(e=>e.Invoice.InvoiceNumber==slgbi.Key.InvoiceNumber).Invoice.InvoiceNumber, 
                           //Count= invoiceItems.Where(e=>e.ItemCode==slgbi.Key.ItemCode).GroupBy(e=>e.Invoice.InvoiceNumber).Count(),
                           Count= 1,
                            });

                        }
                        Report.DetailedReport.Add(new() { 
                        Summary=sr,
                        DetailedItems=detailItems.OrderBy(e=>e.ItemCode).ToList()
                        });

                        
                   }

                }

                    return Report;

            }
            catch (Exception e)
            {

                return null;
            }
        }

    }

    #endregion 


    #region GetInventoryStockLedgerReportQuery 

    public class GetInventoryStockLedgerReportQuery : IRequest<InventoryStockLedgerReportDto>
    {
        public UserIdentityDto User { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetInventoryStockLedgerReportQueryHandler : IRequestHandler<GetInventoryStockLedgerReportQuery, InventoryStockLedgerReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryStockLedgerReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<InventoryStockLedgerReportDto> Handle(GetInventoryStockLedgerReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            bool isArab = request.User.Culture.IsArab();
            InventoryStockLedgerReportDto Report = new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

                ReportItems = new(),



            };
            try
            {
                DateTime dateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                DateTime dateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                dateFrom = dateFrom.AddSeconds(-1 * dateFrom.Second);
                dateTo = dateTo.AddSeconds(-1 * dateTo.Second - 1).AddDays(1);

                var history = await _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e =>
               //(e.TranType=="6"||e.TranType=="2")&&
                e.TranDate <= dateTo && e.TranDate >= dateFrom).OrderBy(e=>e.ItemCode).ThenBy(e => e.TranDate).ToListAsync();

                if (!string.IsNullOrEmpty(request.ItemId))
                {
                    history = history.Where(e => e.InvItemMaster.Id.ToString() == request.ItemId).ToList();
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    history = history.FindAll(e => e.InvWarehouses.WHCode == request.WhCode);
                }



                Report.TotalItemsCount = history.Count;
                history =history.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();

                var historyGroupByItem = history.GroupBy(g => g.InvItemMaster.ItemCode).ToList();

                List<InventoryStockLedgerReportPerItemDto> Reportitems = new();
                foreach (var itemGroup in historyGroupByItem)
                {

                    var openinBalHistory = _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e =>
                    //(e.TranType == "6" || e.TranType == "2") &&
                    e.TranDate < dateFrom && e.ItemCode == itemGroup.First().ItemCode).ToList();
                    decimal openingInQty = Math.Abs(openinBalHistory.Where(e => e.TranTotQty > 0).Sum(e => e.TranTotQty));
                    decimal openingOutQty = Math.Abs(openinBalHistory.Where(e => e.TranTotQty < 0).Sum(e => e.TranTotQty));

                    decimal closingInQty = Math.Abs(itemGroup.Where(e => e.TranTotQty > 0).Sum(e => e.TranTotQty) + openingInQty);
                    decimal closingOutQty = Math.Abs(itemGroup.Where(e => e.TranTotQty < 0).Sum(e => e.TranTotQty) - openingOutQty);
                    
                    decimal openingInCost = Math.Abs(openinBalHistory.Where(e => e.TranTotQty > 0 && e.TranType=="6").Sum(e => e.TranTotQty*e.ItemAvgCost)+openinBalHistory.Where(e => e.TranTotQty > 0 && e.TranType!="6").Sum(e => e.TranTotQty*e.TranPrice));
                    decimal openingOutCost = Math.Abs(openinBalHistory.Where(e => e.TranTotQty < 0 && e.TranType == "6").Sum(e => e.TranTotQty * e.ItemAvgCost)+openinBalHistory.Where(e => e.TranTotQty < 0 && e.TranType != "6").Sum(e => e.TranTotQty * e.TranPrice));

                    decimal closingInCost = Math.Abs(itemGroup.Where(e => e.TranTotQty < 0 && e.TranType == "6").Sum(e => e.TranTotQty * e.ItemAvgCost)+itemGroup.Where(e => e.TranTotQty < 0 && e.TranType != "6").Sum(e => e.TranTotQty * e.TranPrice) + openingInCost);
                    decimal closingOutCost = Math.Abs(itemGroup.Where(e => e.TranTotQty > 0 && e.TranType == "6").Sum(e => e.TranTotQty * e.ItemAvgCost)+itemGroup.Where(e => e.TranTotQty > 0 && e.TranType != "6").Sum(e => e.TranTotQty * e.TranPrice) - openingOutCost);

                    SndInventoryHistoryDto openingBal = new()
                    {

                        InQty = openingInQty,
                        OutQty = openingOutQty,
                        BalanceQty = openingInQty - openingOutQty,

                        InCost = openingInCost,
                        OutCost = openingOutCost,
                        BalanceCost = openingInCost - openingOutCost,
                    };

                    SndInventoryHistoryDto closingBal = new()
                    {
                        InQty = closingInQty,
                        OutQty = closingOutQty,
                        BalanceQty = closingInQty - closingOutQty, 
                        
                        InCost = closingInCost,
                        OutCost = closingOutCost,
                        BalanceCost = closingInCost - closingOutCost,
                    };


                    List<SndInventoryHistoryDto> transactions = new();
                    decimal balQty = openingInQty - openingOutQty;
                    decimal balCost = openingInCost - openingOutCost;
                    foreach (var tr in itemGroup)
                    {

                        decimal inCost = tr.TranTotQty > 0 ? tr.TranType == "6" ? Math.Abs(tr.TranTotQty * tr.TranPrice) : tr.TranType != "6" ? Math.Abs(tr.TranPrice * tr.TranTotQty) : 0 : 0;
                        decimal outCost = tr.TranTotQty < 0 ? tr.TranType == "6" ? Math.Abs(tr.TranTotQty * tr.ItemAvgCost) : tr.TranType != "6" ? Math.Abs(tr.TranPrice * tr.TranTotQty) : 0 : 0;



                        balQty += tr.TranTotQty;
                        balCost += (inCost-outCost);

                        transactions.Add(new()
                        {
                            BalanceQty = balQty,
                            BalanceCost = balCost,
                            InQty = tr.TranTotQty > 0 ? tr.TranTotQty : 0,
                            InCost=inCost,
                            OutQty = tr.TranTotQty < 0 ? Math.Abs(tr.TranTotQty) : 0,
                            OutCost=outCost,
                            TranDate = tr.TranDate,
                            TranDescription = tr.TranRemarks,
                            DcNumber = tr.TranNumber,

                            Source = tr.TranType == "2" ? "PO" : tr.TranType == "6" ? "SnD" : tr.TranType, //based on table data given  for 2 different types

                            WarehouseCode = tr.InvWarehouses.WHCode,
                            WarehouseName = tr.InvWarehouses.WHName,

                        });
                    }


                    Reportitems.Add(new()
                    {
                        ClosingBal = closingBal,
                        OpeningBal = openingBal,
                        Transactions = transactions,
                        ItemCode = itemGroup.First().InvItemMaster.ItemCode,
                        ItemId = itemGroup.First().InvItemMaster.Id,
                        ItemName = isArab ? itemGroup.First().InvItemMaster.ShortNameAr : itemGroup.First().InvItemMaster.ShortName,
                    });

                                    }
                Report.ReportItems = Reportitems;
                return Report;
            }
            catch (Exception e)
            {

                return Report;
                //  return null;
            }
        }

    }



    #endregion













    #region GetInventoryStockAnalysisReportQuery

    public class GetInventoryStockAnalysisReportQuery : IRequest<InventoryStockLedgerReportDto>
    {
        public UserIdentityDto User { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetInventoryStockAnalysisReportQueryHandler : IRequestHandler<GetInventoryStockAnalysisReportQuery, InventoryStockLedgerReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryStockAnalysisReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<InventoryStockLedgerReportDto> Handle(GetInventoryStockAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            bool isArab = request.User.Culture.IsArab();
            InventoryStockLedgerReportDto Report = new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

                ReportItems = new(),



            };
            try
            {
                DateTime dateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                DateTime dateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                dateFrom = dateFrom.AddSeconds(-1 * dateFrom.Second);
                dateTo = dateTo.AddSeconds(-1 * dateTo.Second - 1).AddDays(1);

                var history = await _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e =>
              // (e.TranType=="6"||e.TranType=="2")&&
                e.TranDate <= dateTo && e.TranDate >= dateFrom).OrderBy(e => e.TranDate).ToListAsync();


                var openinBalHistory = _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e => 
                e.TranDate < dateFrom
                // && (e.TranType == "6" || e.TranType == "2")
                ).ToList();


                if (!string.IsNullOrEmpty(request.ItemId))
                {
                    history = history.Where(e => e.InvItemMaster.Id.ToString() == request.ItemId).ToList();
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    history = history.FindAll(e => e.InvWarehouses.WHCode == request.WhCode);
                }


                var historyGroupByItem = history.GroupBy(g => g.InvItemMaster.ItemCode).ToList();

                List<InventoryStockLedgerReportPerItemDto> Reportitems = new();
                foreach (var itemGroup in historyGroupByItem)
                {

                    decimal openingInQty = Math.Abs(openinBalHistory.Where(e => e.TranTotQty > 0 && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty));
                    decimal openingOutQty = Math.Abs(openinBalHistory.Where(e => e.TranTotQty < 0 && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty));

                    decimal closingInQty = Math.Abs(itemGroup.Where(e => e.TranTotQty > 0).Sum(e => e.TranTotQty) + openingInQty);
                    decimal closingOutQty = Math.Abs(itemGroup.Where(e => e.TranTotQty < 0).Sum(e => e.TranTotQty) - openingOutQty);
                   
                    decimal openingInCost = Math.Abs(openinBalHistory.Where(e => e.TranTotQty > 0 && e.TranType=="6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.ItemAvgCost)+openinBalHistory.Where(e => e.TranTotQty > 0 && e.TranType!="6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.TranPrice));
                    decimal openingOutCost = Math.Abs(openinBalHistory.Where(e => e.TranTotQty < 0 && e.TranType=="6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.ItemAvgCost)+openinBalHistory.Where(e => e.TranTotQty < 0 && e.TranType!="6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.TranPrice));

                    decimal closingInCost = Math.Abs(itemGroup.Where(e => e.TranTotQty > 0 && e.TranType == "6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.ItemAvgCost)+itemGroup.Where(e => e.TranTotQty > 0 && e.TranType != "6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.TranPrice) + openingInCost);
                    decimal closingOutCost = Math.Abs(itemGroup.Where(e => e.TranTotQty < 0 && e.TranType == "6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.ItemAvgCost)+ itemGroup.Where(e => e.TranTotQty < 0 && e.TranType != "6" && e.ItemCode == itemGroup.First().ItemCode).Sum(e => e.TranTotQty*e.TranPrice) - openingOutCost);

                    SndInventoryHistoryDto openingBal = new()
                    {

                        InQty = openingInQty,
                        OutQty = openingOutQty,
                        BalanceQty = openingInQty - openingOutQty,

                        InCost = openingInCost,
                        OutCost = openingOutCost,
                        BalanceCost = openingInCost - openingOutCost,
                    };

                    SndInventoryHistoryDto closingBal = new()
                    {
                        InQty = closingInQty,
                        OutQty = closingOutQty,
                        BalanceQty = closingInQty - closingOutQty,

                        InCost = closingInCost,
                        OutCost = closingOutCost,
                        BalanceCost = closingInCost - closingOutCost,
                    };

                    Reportitems.Add(new()
                    {
                        ClosingBal = closingBal,
                        OpeningBal = openingBal,
                        TransactionBal= new() { 
                        InQty=closingBal.InQty-openingBal.InQty,
                        OutQty=closingBal.OutQty-openingBal.OutQty,
                        BalanceQty=closingBal.BalanceQty-openingBal.BalanceQty,

                         InCost=closingBal.InCost-openingBal.InCost,
                        OutCost=closingBal.OutCost-openingBal.OutCost,
                        BalanceCost=closingBal.BalanceCost-openingBal.BalanceCost,


                        },

                        ItemCode = itemGroup.First().InvItemMaster.ItemCode,
                        ItemId = itemGroup.First().InvItemMaster.Id,
                        ItemName = isArab ? itemGroup.First().InvItemMaster.ShortNameAr : itemGroup.First().InvItemMaster.ShortName,
                    });

                    if (Reportitems.Count >= (request.PageNumber + 1) * request.PageSize)
                    {
                        Report.TotalItemsCount = historyGroupByItem.Count;
                        Report.ReportItems = Reportitems.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                        return Report;
                    }

                }
                Report.TotalItemsCount = historyGroupByItem.Count;
                Report.ReportItems = Reportitems.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                return Report;
            }
            catch (Exception e)
            {

                return Report;
                //  return null;
            }
        }

    }



    #endregion

#region GetInventoryTransactionsReportQuery

    public class GetInventoryTransactionsReportQuery : IRequest<SndInventoryTransactionsReportDto>
    {
        public UserIdentityDto User { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string TransactionType { get; set; }
        public bool? IsSummary { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetInventoryTransactionsReportQueryHandler : IRequestHandler<GetInventoryTransactionsReportQuery, SndInventoryTransactionsReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryTransactionsReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SndInventoryTransactionsReportDto> Handle(GetInventoryTransactionsReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            bool isArab = request.User.Culture.IsArab();
            SndInventoryTransactionsReportDto Report = new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

                ReportItems = new(),



            };
            try
            {
                var Warehouses =  _context.InvWarehouses;
                DateTime dateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                DateTime dateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                dateFrom = dateFrom.AddSeconds(-1 * dateFrom.Second);
                dateTo = dateTo.AddSeconds(-1 * dateTo.Second - 1).AddDays(1);

                List<SndInventoryTransactionsDto> TransfersList = request.TransactionType == "Transfers" || request.TransactionType == "All" ? _context.IMTransferTransactionHeader.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).Select(e => new SndInventoryTransactionsDto
                {
                    TranDate = e.TranDate,
                    TranDocNumber = e.TranDocNumber,
                    TranLocation = Warehouses.FirstOrDefault(w => e.TranLocation == w.WHCode).WHName,
                    TranNumber = e.TranNumber,
                    TranPostStatus = e.TranPostStatus,
                    TranReference = e.TranReference,
                    TransactionItems = new(),
                    TranToLocation = Warehouses.FirstOrDefault(w => e.TranToLocation == w.WHCode).WHName,
                    TranTotalCost = e.TranTotalCost != 0 ? e.TranTotalCost : _context.IMTransferTransactionDetails.Where(t => t.TranNumber == e.TranNumber).Sum(e=>e.TranTotCost),//e.TranTotalCost,
                    TranType = e.TranType,
                    TranType2 = "Transfers" //e.TranType
                }).ToList(): new();


                 List<SndInventoryTransactionsDto> ReconsilesList = request.TransactionType == "Reconsiles" || request.TransactionType == "All" ? _context.IMStockReconciliationTransactionHeader.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).Select(e => new SndInventoryTransactionsDto
                {
                    TranDate = e.TranDate,
                    TranDocNumber = e.TranDocNumber,
                    TranLocation = Warehouses.FirstOrDefault(w => e.TranLocation == w.WHCode).WHName,
                    TranNumber = e.TranNumber,
                    TranPostStatus = e.TranPostStatus,
                    TranReference = e.TranReference,
                    TransactionItems = new(),
                    TranToLocation = Warehouses.FirstOrDefault(w => e.TranToLocation == w.WHCode).WHName,
                    TranTotalCost = e.TranTotalCost != 0 ? e.TranTotalCost : _context.IMStockReconciliationTransactionDetails.Where(t => t.TranNumber == e.TranNumber).Sum(e => e.TranTotCost),//e.TranTotalCost,
                     TranType = e.TranType,
                     TranType2 = "Reconsiles"//e.TranType
                }).ToList(): new();

                   List<SndInventoryTransactionsDto> IssuesList = request.TransactionType == "Issues" || request.TransactionType == "All" ? _context.IMTransactionHeader.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).Select(e => new SndInventoryTransactionsDto
                {
                    TranDate = e.TranDate,
                    TranDocNumber = e.TranDocNumber,
                    TranLocation = Warehouses.FirstOrDefault(w => e.TranLocation == w.WHCode).WHName,
                    TranNumber = e.TranNumber,
                    TranPostStatus = e.TranPostStatus,
                    TranReference = e.TranReference,
                    TransactionItems = new(),
                    TranToLocation = Warehouses.FirstOrDefault(w => e.TranToLocation == w.WHCode).WHName,
                    TranTotalCost = e.TranTotalCost != 0 ? e.TranTotalCost : _context.IMTransactionDetails.Where(t => t.TranNumber == e.TranNumber).Sum(e => e.TranTotCost),// e.TranTotalCost,
                       TranType = e.TranType,
                       TranType2 = "Issues"//e.TranType
                   }).ToList(): new();
                 List<SndInventoryTransactionsDto> ReceiptsList = request.TransactionType == "Receipts" || request.TransactionType == "All" ? _context.IMReceiptsTransactionHeader.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).Select(e => new SndInventoryTransactionsDto
                {
                    TranDate = e.TranDate,
                    TranDocNumber = e.TranDocNumber,
                    TranLocation = Warehouses.FirstOrDefault(w => e.TranLocation == w.WHCode).WHName,
                    TranNumber = e.TranNumber,
                    TranPostStatus = e.TranPostStatus,
                    TranReference = e.TranReference,
                    TransactionItems = new(),
                    TranToLocation = Warehouses.FirstOrDefault(w => e.TranToLocation == w.WHCode).WHName,
                    TranTotalCost = e.TranTotalCost != 0 ? e.TranTotalCost : _context.IMReceiptsTransactionDetails.Where(t => t.TranNumber == e.TranNumber).Sum(e => e.TranTotCost),//e.TranTotalCost,
                     TranType = e.TranType,
                     TranType2 = "Receipts"//e.TranType
                }).ToList(): new();

                 List<SndInventoryTransactionsDto> AdjustmentsList = request.TransactionType == "Adjustments" || request.TransactionType == "All" ? _context.IMAdjustmentsTransactionHeader.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).Select(e => new SndInventoryTransactionsDto
                {
                    TranDate = e.TranDate,
                    TranDocNumber = e.TranDocNumber,
                    TranLocation = Warehouses.FirstOrDefault(w => e.TranLocation == w.WHCode).WHName,
                    TranNumber = e.TranNumber,
                    TranPostStatus = e.TranPostStatus,
                    TranReference = e.TranReference,
                    TransactionItems = new(),
                    TranToLocation = Warehouses.FirstOrDefault(w => e.TranToLocation == w.WHCode).WHName,
                    TranTotalCost = e.TranTotalCost!=0 ? e.TranTotalCost:_context.IMAdjustmentsTransactionDetails.Where(t => t.TranNumber == e.TranNumber).Sum(e => e.TranTotCost),// e.TranTotalCost,
                     TranType = e.TranType,
                     TranType2 = "Adjustments"//e.TranType
                }).ToList(): new();


                List<SndInventoryTransactionsDto> inventoryTransactionHeaders = new();
               inventoryTransactionHeaders = request.TransactionType switch
                {
                    "Transfers" =>  TransfersList.ToList(),
                    "Reconsiles" =>  ReconsilesList.ToList(),
                    "Issues" => IssuesList.ToList(),
                    "Receipts" => ReceiptsList.ToList(),
                    "Adjustments" => AdjustmentsList.ToList(),
                    "All" => TransfersList.Concat(ReceiptsList).Concat(IssuesList).Concat(ReconsilesList).Concat(AdjustmentsList).Distinct().ToList(),
                    _ => new()
                };

                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    inventoryTransactionHeaders = inventoryTransactionHeaders.Where(e => e.TranLocation==request.WhCode || e.TranToLocation==request.WhCode).ToList();
                }
                
                Report.TotalItemsCount = inventoryTransactionHeaders.Count();

                inventoryTransactionHeaders = inventoryTransactionHeaders.OrderBy(e=>e.TranDate).Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();

                if (!request.IsSummary.Value)
                {
                    List<SndInventoryTransactionsDto> inventoryTransactionItems = new();
                   
                    foreach(var ith in inventoryTransactionHeaders) {
                        ith.TransactionItems = ith.TranType2 switch
                        {
                            "Transfers" => await _context.IMTransferTransactionDetails.Where(e =>e.TranNumber==ith.TranNumber).Select(e=>new SndInventoryTransactionsItemDto { 
                             TranItemCost=e.TranItemCost,
                              TranItemName=e.TranItemName,
                               TranItemQty=e.TranItemQty,
                                TranItemUnit=e.TranItemUnit,
                                 TranTotCost=e.TranTotCost,
                                 TranTotQty=e.TranItemQty* e.TranUOMFactor,
                                  TranUOMFactor=e.TranUOMFactor
                            }).ToListAsync(),
                            "Reconsiles" =>await _context.IMStockReconciliationTransactionDetails.Where(e => e.TranNumber == ith.TranNumber).Select(e => new SndInventoryTransactionsItemDto
                            {
                                TranItemCost = e.TranItemCost,
                                TranItemName = e.TranItemName,
                                TranItemQty = e.TranItemQty,
                                TranItemUnit = e.TranItemUnit,
                                TranTotCost = e.TranTotCost,
                                TranUOMFactor = e.TranUOMFactor,
                                TranTotQty = e.TranItemQty * e.TranUOMFactor,
                            }).ToListAsync(),
                            "Issues" => await _context.IMTransactionDetails.Where(e => e.TranNumber == ith.TranNumber).Select(e => new SndInventoryTransactionsItemDto
                            {
                                TranItemCost = e.TranItemCost,
                                TranItemName = e.TranItemName,
                                TranItemQty = e.TranItemQty,
                                TranItemUnit = e.TranItemUnit,
                                TranTotCost = e.TranTotCost,
                                TranUOMFactor = e.TranUOMFactor,
                                TranTotQty = e.TranItemQty * e.TranUOMFactor,
                            }).ToListAsync(),
                            "Receipts" => await _context.IMReceiptsTransactionDetails.Where(e => e.TranNumber == ith.TranNumber).Select(e => new SndInventoryTransactionsItemDto
                            {
                                TranItemCost = e.TranItemCost,
                                TranItemName = e.TranItemName,
                                TranItemQty = e.TranItemQty,
                                TranItemUnit = e.TranItemUnit,
                                TranTotCost = e.TranTotCost,
                                TranUOMFactor = e.TranUOMFactor,
                                TranTotQty = e.TranItemQty * e.TranUOMFactor,
                            }).ToListAsync(),
                            "Adjustments" =>await  _context.IMAdjustmentsTransactionDetails.Where(e => e.TranNumber == ith.TranNumber).Select(e => new SndInventoryTransactionsItemDto
                            {
                                TranItemCost = e.TranItemCost,
                                TranItemName = e.TranItemName,
                                TranItemQty = e.TranItemQty,
                                TranItemUnit = e.TranItemUnit,
                                TranTotCost = e.TranTotCost,
                                TranUOMFactor = e.TranUOMFactor,
                                TranTotQty = e.TranItemQty * e.TranUOMFactor,
                            }).ToListAsync(),
                            _ => new()
                        };

                    };


                }



                Report.ReportItems = inventoryTransactionHeaders.ToList();
                return Report;
            }
            catch (Exception e)
            {

                return Report;
                //  return null;
            }
        }

    }



    #endregion


    #region GetInventoryStockTransactionAnalysisReportQuery

    public class GetInventoryStockTransactionAnalysisReportQuery : IRequest<StockTransactionAnalysisReportDto>
    {
        public UserIdentityDto User { get; set; }
        public string WhCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetInventoryStockTransactionAnalysisReportQueryHandler : IRequestHandler<GetInventoryStockTransactionAnalysisReportQuery, StockTransactionAnalysisReportDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInventoryStockTransactionAnalysisReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StockTransactionAnalysisReportDto> Handle(GetInventoryStockTransactionAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            bool isArab = request.User.Culture.IsArab();
            StockTransactionAnalysisReportDto Report = new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

                ReportItems = new(),



            };
            try
            {
                DateTime dateFrom = request.DateFrom is not null && request.DateTo is not null ? request.DateFrom.Value : DateTime.Now;
                DateTime dateTo = request.DateFrom is not null && request.DateTo is not null ? request.DateTo.Value : DateTime.Now;
                dateFrom = dateFrom.AddSeconds(-1 * dateFrom.Second);
                dateTo = dateTo.AddSeconds(-1 * dateTo.Second - 1).AddDays(1);

                var history = await _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e =>
               // (e.TranType == "6" || e.TranType == "2")&&
                e.TranDate <= dateTo && e.TranDate >= dateFrom).OrderBy(e => e.TranDate).ToListAsync();
                var openinBalHistory = _context.InvItemInventoryHistory.Include(e => e.InvItemMaster).Include(e => e.InvWarehouses).Where(e =>
              //  (e.TranType == "6" || e.TranType == "2")&&
                e.TranDate < dateFrom).ToList();
                
                //var adjustmentTransactions = _context.IMAdjustmentsTransactionDetails.Where(e =>e.TranDate <= dateTo && e.TranDate >= dateFrom).ToList();
                //var reconsilationTransactions = _context.IMStockReconciliationTransactionDetails.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).ToList();
                //var issuesTransactions = _context.IMTransactionDetails.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).ToList();
                //var transferTransactions = _context.IMTransferTransactionDetails.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).ToList();
                //var receiptsTransactions = _context.IMReceiptsTransactionDetails.Where(e => e.TranDate <= dateTo && e.TranDate >= dateFrom).ToList();


                if (!string.IsNullOrEmpty(request.ItemId))
                {
                    history = history.Where(e => e.InvItemMaster.Id.ToString() == request.ItemId).ToList();
                }
                if (!string.IsNullOrEmpty(request.WhCode))
                {
                    history = history.FindAll(e => e.InvWarehouses.WHCode == request.WhCode);
                }


                var historyGroupByItem = history.GroupBy(g => g.InvItemMaster.ItemCode).ToList();
                Report.TotalItemsCount = historyGroupByItem.Count();
                historyGroupByItem = historyGroupByItem.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();

                List<InventoryStockTransactionAnalysisReportPerItemDto> Reportitems = new();
                foreach (var itemGroup in historyGroupByItem)
                {

                   
                    Report.ReportItems.Add(new() {
                         ItemCode=itemGroup.First().InvItemMaster.ItemCode,
                         ItemId=itemGroup.First().InvItemMaster.Id,
                          ItemName=isArab? itemGroup.First().InvItemMaster.ShortNameAr:itemGroup.First().InvItemMaster.ShortName,
                        OpeningBal = new() 
                        {   Qty= openinBalHistory.Where( e=> e.ItemCode == itemGroup.First().ItemCode).Sum(e=>e.TranTotQty), 
                            Cost=openinBalHistory.Where(e => e.ItemCode == itemGroup.First().ItemCode && e.TranTotQty>0).Sum(e => e.TranPrice*e.TranTotQty),
                             SalePrice= openinBalHistory.Where(e => e.ItemCode == itemGroup.First().ItemCode && e.TranTotQty < 0).Sum(e => e.TranPrice*e.TranTotQty),
                        },
                        SalesBal = new()
                        {
                              Cost= itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode  && e.TranType == "6").Sum(e => e.TranTotQty*e.ItemAvgCost),
                              SalePrice= itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && e.TranType == "6").Sum(e => e.TranPrice * e.TranTotQty),
                              Qty = itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && e.TranType == "6").Sum(e => e.TranTotQty)
                        },
                    PurchaseBal = new()
                        {
                              Cost= itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && (e.TranType == "2")).Sum(e => e.TranPrice*e.TranTotQty),
                              SalePrice= 0,
                              Qty = itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && (e.TranType == "2")).Sum(e => e.TranTotQty)
                        },

                        AdjustmentBal = new()
                        {
                              Cost= itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && (e.TranType!= "6"&&e.TranType!= "2")).Sum(e => e.TranPrice*e.TranTotQty),
                              SalePrice= 0,
                              Qty = itemGroup.Where(e => e.ItemCode == itemGroup.First().ItemCode && (e.TranType != "6" && e.TranType != "2")).Sum(e => e.TranTotQty)
                        },


                    //AdjustmentBal=new()
                    //{
                    //    Cost = adjustmentTransactions.Where(e =>e.TranItemCode== itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranTotCost)
                    //    + reconsilationTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranTotCost)
                    //    + issuesTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranTotCost)
                    //    + transferTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranTotCost)
                    //    + receiptsTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranTotCost)
                    //,
                    //    SalePrice=0,

                    //    Qty = adjustmentTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranItemQty*e.TranUOMFactor)
                    //    + reconsilationTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode ).Sum(e => e.TranItemQty * e.TranUOMFactor)
                    //    + issuesTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranItemQty * e.TranUOMFactor)
                    //    + transferTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode ).Sum(e => e.TranItemQty * e.TranUOMFactor)
                    //    + receiptsTransactions.Where(e => e.TranItemCode == itemGroup.First().InvItemMaster.ItemCode).Sum(e => e.TranItemQty * e.TranUOMFactor)

                    //},
                        ClosingBal = new()
                        {
                            SalePrice=0,
                             Cost=0,
                              Qty=0
                        }
                    });

                   
                }
                Report.ReportItems.ForEach(e => {
                    e.ClosingBal = new()
                    {
                        SalePrice = e.OpeningBal.SalePrice + e.SalesBal.SalePrice,
                        Cost = e.OpeningBal.Cost + e.AdjustmentBal.Cost + e.PurchaseBal.Cost+ e.SalesBal.Cost,
                        Qty = e.OpeningBal.Qty + e.AdjustmentBal.Qty + e.PurchaseBal.Qty+ e.SalesBal.Qty

                    };
                });
                return Report;
            }
            catch (Exception e)
            {
                return Report;
                //  return null;
            }
        }

    }



    #endregion
}
public static class Extensions
{
    public static IEnumerable<List<T>> partition<T>(this List<T> values, int chunkSize)
    {
        for (int i = 0; i < values.Count; i += chunkSize)
        {
            yield return values.GetRange(i, Math.Min(chunkSize, values.Count - i));
        }
    }

}