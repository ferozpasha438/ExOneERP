using AutoMapper;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.DB;
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

    #region GetPurchaseOrderSummaryList

    public class GetPurchaseOrderSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendCode { get; set; }
        public string Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public string ItemCode { get; set; }
        public string OrderType { get; set; }
        public bool IsAllVendors { get; set; }
    }
    public class GetPurchaseOrderSummaryListHandler : IRequestHandler<GetPurchaseOrderSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseOrderSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetPurchaseOrderSummaryList request, CancellationToken cancellationToken)
        {
            var poHeaders = _context.purchaseOrderHeaders
                .Include(e => e.Vendor)
                .AsNoTracking();
            bool isArab = request.User.Culture.IsArab();

            // var itemMaster = _context.InvItemMaster.AsNoTracking();

            if (request.OrderType.HasValue())
            {
                if (request.OrderType == "po")
                    poHeaders = poHeaders.Where(e => e.Trantype == "2");
                else
                    poHeaders = poHeaders.Where(e => e.Trantype == "1");
            }

            if (request.Type.HasValue())
            {
                if (request.Type != "All")
                {
                    if (request.Type == "closed")
                        poHeaders = poHeaders.Where(e => e.IsPaid);
                    else
                        poHeaders = poHeaders.Where(e => !e.IsPaid);
                }
            }

            if (!request.IsAllVendors)
                if (request.VendCode.HasValue())
                    poHeaders = poHeaders.Where(e => e.VendCode == request.VendCode);

            if (request.BranchCode.HasValue())
                poHeaders = poHeaders.Where(e => e.BranchCode == request.BranchCode);

            if (request.ItemCode.HasValue())
            {
                var poLineItemIds = _context.purchaseOrderDetails.Where(e => e.TranItemCode == request.ItemCode).Select(e => e.TranId);
                poHeaders = poHeaders.Where(e => poLineItemIds.Any(lineId => lineId == e.TranNumber));

            }

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //var cStatements1 = poHeaders.Where(e => e.TranDate < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new
                //{
                //    e.VendCode,
                //    e.Vendor.VendName                   
                //});

                //foreach (var item in statements1)
                //{
                //    summaryList1.Add(new()
                //    {
                //        VendCode = item.Key.VendCode,
                //        TranDate = item.Key.TranDate,
                //        VendName = item.Key.VendName,
                //        InvoiceNumber = inv?.InvoiceNumber,
                //        Remarks = inv?.Remarks,
                //        Trantype = item.Key.Trantype,
                //        DrAmount = item.Sum(e => e.DrAmount),
                //        CrAmount = item.Sum(e => e.CrAmount),
                //        Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //    });
                //}

                //poHeaders = poHeaders.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
                poHeaders = poHeaders.Where(e => EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.VendCode);
            //var statements = (await poHeaders.ToListAsync()).GroupBy(e => new { e.VendCode, e.Vendor.VendName, e.Trantype });
            var statements = await poHeaders.OrderByDescending(e => e.Id).ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    VendName = isArab ? (item.Vendor?.VendArbName ?? String.Empty) : (item.Vendor?.VendName ?? String.Empty),
                    InvoiceNumber = item.Trantype == "1" ? item.PurchaseRequestNO : item.PurchaseOrderNO,
                    TranDate = item.TranDate,
                    Trantype = item.Trantype == "1" ? "PR" : "PO",
                    DocNumber = item.DocNumber,
                    Remarks = item.InvRefNumber,
                    DrAmount = item.TranTotalCost,
                    CrAmount = item.Taxes,
                    OpeningBalance = item.TranDiscAmount,
                    Balance = item.TranTotalCost + item.Taxes
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount ?? 0),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount ?? 0),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance ?? 0),
                TotalOpeningAmount = summaryList.Sum(e => e.OpeningBalance) + summaryList1.Sum(e => e.OpeningBalance ?? 0),
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion


    #region GetPurchaseOrderReturnSummaryList


    public class GetPurchaseOrderReturnSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendCode { get; set; }
        public string Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public string ItemCode { get; set; }
        public string OrderType { get; set; }
        public bool IsAllVendors { get; set; }
    }
    public class GetPurchaseOrderReturnSummaryListHandler : IRequestHandler<GetPurchaseOrderReturnSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseOrderReturnSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetPurchaseOrderReturnSummaryList request, CancellationToken cancellationToken)
        {
            var poHeaders = _context.purchaseReturnHeader
                .Include(e => e.Vendor)
                .AsNoTracking();
            bool isArab = request.User.Culture.IsArab();

            // var itemMaster = _context.InvItemMaster.AsNoTracking();

            if (request.Type.HasValue())
            {
                if (request.Type != "All")
                {
                    if (request.Type == "closed")
                        poHeaders = poHeaders.Where(e => e.IsPaid);
                    else
                        poHeaders = poHeaders.Where(e => !e.IsPaid);
                }
            }

            if (!request.IsAllVendors)
                if (request.VendCode.HasValue())
                    poHeaders = poHeaders.Where(e => e.VendCode == request.VendCode);

            if (request.BranchCode.HasValue())
                poHeaders = poHeaders.Where(e => e.BranchCode == request.BranchCode);

            if (request.ItemCode.HasValue())
            {
                var poLineItemIds = _context.purchaseReturnDetails.Where(e => e.TranItemCode == request.ItemCode).Select(e => e.TranId);
                poHeaders = poHeaders.Where(e => poLineItemIds.Any(lineId => lineId == e.TranNumber));

            }

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //var cStatements1 = poHeaders.Where(e => e.TranDate < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new
                //{
                //    e.VendCode,
                //    e.Vendor.VendName                   
                //});

                //foreach (var item in statements1)
                //{
                //    summaryList1.Add(new()
                //    {
                //        VendCode = item.Key.VendCode,
                //        TranDate = item.Key.TranDate,
                //        VendName = item.Key.VendName,
                //        InvoiceNumber = inv?.InvoiceNumber,
                //        Remarks = inv?.Remarks,
                //        Trantype = item.Key.Trantype,
                //        DrAmount = item.Sum(e => e.DrAmount),
                //        CrAmount = item.Sum(e => e.CrAmount),
                //        Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //    });
                //}

                //poHeaders = poHeaders.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
                poHeaders = poHeaders.Where(e => EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.VendCode);
            //var statements = (await poHeaders.ToListAsync()).GroupBy(e => new { e.VendCode, e.Vendor.VendName, e.Trantype });
            var statements = await poHeaders.OrderByDescending(e => e.Id).ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    VendName = isArab ? item.Vendor.VendArbName : item.Vendor.VendName,
                    InvoiceNumber = item.TranNumber,
                    TranDate = item.TranDate,
                    Trantype = "PRT",
                    DocNumber = item.DocNumber,
                    Remarks = item.InvRefNumber,
                    DrAmount = item.TranTotalCost,
                    CrAmount = item.Taxes,
                    OpeningBalance = item.TranDiscAmount,
                    Balance = item.TranTotalCost + item.Taxes
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount ?? 0),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount ?? 0),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance ?? 0),
                TotalOpeningAmount = summaryList.Sum(e => e.OpeningBalance) + summaryList1.Sum(e => e.OpeningBalance ?? 0),
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion

    #region GetPurchaseOrderGrnSummaryList

    public class GetPurchaseOrderGrnSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendCode { get; set; }
        public string Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public string ItemCode { get; set; }
        public bool IsAllVendors { get; set; }
    }
    public class GetPurchaseOrderGrnSummaryListHandler : IRequestHandler<GetPurchaseOrderGrnSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseOrderGrnSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetPurchaseOrderGrnSummaryList request, CancellationToken cancellationToken)
        {
            var grnHeader = _context.GRNHeaders.Include(e => e.Vendor).AsNoTracking();
            bool isArab = request.User.Culture.IsArab();

            if (request.Type.HasValue())
            {
                if (request.Type != "All")
                {
                    if (request.Type == "closed")
                        grnHeader = grnHeader.Where(e => e.IsPaid);
                    else
                        grnHeader = grnHeader.Where(e => !e.IsPaid);
                }
            }

            if (!request.IsAllVendors)
                if (request.VendCode.HasValue())
                    grnHeader = grnHeader.Where(e => e.VendCode == request.VendCode);

            if (request.BranchCode.HasValue())
                grnHeader = grnHeader.Where(e => e.BranchCode == request.BranchCode);

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //var cStatements1 = poHeaders.Where(e => e.TranDate < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new
                //{
                //    e.VendCode,
                //    e.Vendor.VendName                   
                //});

                //foreach (var item in statements1)
                //{
                //    summaryList1.Add(new()
                //    {
                //        VendCode = item.Key.VendCode,
                //        TranDate = item.Key.TranDate,
                //        VendName = item.Key.VendName,
                //        InvoiceNumber = inv?.InvoiceNumber,
                //        Remarks = inv?.Remarks,
                //        Trantype = item.Key.Trantype,
                //        DrAmount = item.Sum(e => e.DrAmount),
                //        CrAmount = item.Sum(e => e.CrAmount),
                //        Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //    });
                //}

                //grnHeader = grnHeader.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
                grnHeader = grnHeader.Where(e => EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.VendCode);
            //var statements = (await poHeaders.ToListAsync()).GroupBy(e => new { e.VendCode, e.Vendor.VendName, e.Trantype });
            var statements = await grnHeader.OrderByDescending(e => e.Id).ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    VendName = isArab ? item.Vendor.VendArbName : item.Vendor.VendName,
                    InvoiceNumber = item.Trantype == "1" ? item.PurchaseRequestNO : item.PurchaseOrderNO,
                    TranDate = item.TranDate,
                    Trantype = item.Trantype == "1" ? "PR" : "PO",
                    DocNumber = item.DocNumber,
                    Remarks = item.InvRefNumber,
                    DrAmount = item.TranTotalCost,
                    CrAmount = item.Taxes,
                    OpeningBalance = item.TranDiscAmount,
                    Balance = item.TranTotalCost + item.Taxes
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount ?? 0),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount ?? 0),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance ?? 0),
                TotalOpeningAmount = summaryList.Sum(e => e.OpeningBalance) + summaryList1.Sum(e => e.OpeningBalance ?? 0),
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion

    #region GetVendorPOList

    public class GetVendorPOList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public bool IsAllVendors { get; set; }
    }
    public class GetVendorPOListHandler : IRequestHandler<GetVendorPOList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorPOListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorPOList request, CancellationToken cancellationToken)
        {

            var invoices = _context.TranVenInvoices.AsNoTracking();
            var grnHeaders = _context.GRNHeaders.Include(e => e.Vendor).AsNoTracking();
            //var poHeaders = _context.purchaseOrderHeaders.Include(e => e.Vendor).AsNoTracking();
            bool isArab = request.User.Culture.IsArab();

            if (!request.IsAllVendors)
                if (request.VendCode.HasValue())
                    grnHeaders = grnHeaders.Where(e => e.VendCode == request.VendCode);

            if (request.BranchCode.HasValue())
                grnHeaders = grnHeaders.Where(e => e.BranchCode == request.BranchCode);

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //var cStatements1 = poHeaders.Where(e => e.TranDate < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new
                //{
                //    e.VendCode,
                //    e.Vendor.VendName                   
                //});

                //foreach (var item in statements1)
                //{
                //    summaryList1.Add(new()
                //    {
                //        VendCode = item.Key.VendCode,
                //        TranDate = item.Key.TranDate,
                //        VendName = item.Key.VendName,
                //        InvoiceNumber = inv?.InvoiceNumber,
                //        Remarks = inv?.Remarks,
                //        Trantype = item.Key.Trantype,
                //        DrAmount = item.Sum(e => e.DrAmount),
                //        CrAmount = item.Sum(e => e.CrAmount),
                //        Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //    });
                //}

                // grnHeaders = grnHeaders.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
                grnHeaders = grnHeaders.Where(e => EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.VendCode);
            var statements = (await grnHeaders.ToListAsync()).GroupBy(e => new { e.VendCode, e.Vendor.VendName, e.Vendor.VendArbName });
            //var statements = await grnHeaders.OrderByDescending(e => e.Id).ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                var vendorId = await _context.VendorMasters.Where(e => e.VendCode == item.Key.VendCode).Select(e => e.Id).FirstOrDefaultAsync();
                CustomerBalanceSummaryDto balanceObj = new()
                {
                    VendCode = item.Key.VendCode,
                    VendName = isArab ? item.Key.VendArbName : item.Key.VendName,
                    DocNumber = (await invoices.CountAsync(e => e.CustomerId == vendorId)).ToString(),
                    DrAmount = item.Sum(e => e.TranTotalCost),
                    OpeningBalance = item.Sum(e => e.TranDiscAmount),
                    Balance = item.Sum(e => e.TranTotalCost) - item.Sum(e => e.TranDiscAmount),
                    CrAmount = item.Sum(e => e.Taxes)
                };

                balanceObj.Remarks = (balanceObj.Balance + balanceObj.CrAmount).ToString();
                summaryList.Add(balanceObj);
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount ?? 0),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount ?? 0),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance ?? 0),
                TotalOpeningAmount = summaryList.Sum(e => e.OpeningBalance) + summaryList1.Sum(e => e.OpeningBalance ?? 0),
            };
            summaryListDto.NetTotalBalanceAmount = summaryListDto.TotalBalance + summaryListDto.TotalCrAmount;

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion


    #region GetPOItemAnalysisSummary

    public class GetPOItemAnalysisSummary : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public string ItemCode { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
        // public bool IsAllVendors { get; set; }
    }
    public class GetPOItemAnalysisSummaryHandler : IRequestHandler<GetPOItemAnalysisSummary, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPOItemAnalysisSummaryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetPOItemAnalysisSummary request, CancellationToken cancellationToken)
        {
            var grnDetails = _context.GRNDetails
                .Include(e => e.Vendor)
                .Include(e => e.Trans)
                .Include(e => e.InvItemMaster)
                .AsNoTracking();

            //  var wareHouseList = _context.InvWarehouses.AsNoTracking();
            //var pHeader = _context.OpmCustPaymentHeaders.AsNoTracking();
            //var pCustPayments = _context.OpmCustomerPayments.AsNoTracking();
            var invoices = _context.TranVenInvoices.AsNoTracking();

            // if (!request.IsAllVendors)
            if (request.VendCode.HasValue())
                grnDetails = grnDetails.Where(e => e.VendCode == request.VendCode);


            if (request.ItemCode.HasValue())
                grnDetails = grnDetails.Where(e => e.TranItemCode == request.ItemCode);

            if (request.BranchCode.HasValue())
                grnDetails = grnDetails.Where(e => e.BranchCode == request.BranchCode);

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);

            List<List<CustomerBalanceSummaryDto>> summaryItemsList = new();
            bool hasDates = false;

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                hasDates = true;
                // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode });

                //grnDetails = grnDetails.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
                grnDetails = grnDetails.Where(e => EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Year, e.TranDate.Month, e.TranDate.Day) <= request.DateTo);
                // cStatements = cStatements1.Union(cStatements);

            }
            int reportCount = 0;
            IEnumerable<IGrouping<string, TblPopTrnGRNDetails>> statements = null;
            bool isPovendanls = false;
            if (request.Type.HasValue() && request.Type == "povendanls")
            {
                isPovendanls = true;
                reportCount = await grnDetails.CountAsync();
                grnDetails = grnDetails.Pagination(request.ReportIndex, request.ReportCount);
                statements = (await grnDetails.ToListAsync()).GroupBy(e => e.VendCode);// e.SndCustomerMaster.CustName });
            }
            else
            {
                reportCount = await grnDetails.CountAsync();
                grnDetails = grnDetails.Pagination(request.ReportIndex, request.ReportCount);
                statements = (await grnDetails.ToListAsync()).GroupBy(e => e.TranItemCode);// e.SndCustomerMaster.CustName });
            }

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            // var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

            bool isArab = request.User.Culture.IsArab();
            decimal? openingBalance = 0;
            decimal? totalBalance = 0, totalOpeningBalance = 0, totalItemQty = 0, totalDrAmount = 0, totalCrAmount = 0;
            decimal? grandTotalBalance = 0, grandTotalOpeningBalance = 0, grandTotalItemQty = 0, grandTotalDrAmount = 0, grandTotalCrAmount = 0;

            List<CustomSelectListItem> WarehouseItems = null;

            foreach (var statement in statements)
            {
                List<CustomerBalanceSummaryDto> summaryList = new();

                //if (hasDates)
                //{
                //    var customerStatemnt = cStatements1.Where(e => e.TranDate < request.DateFrom && e.VendCode == statement.Key.VendCode);
                //    var customer = await cStatements.FirstOrDefaultAsync(e => e.VendCode == statement.Key.VendCode);
                //    openingBalance = customerStatemnt.Sum(e => e.DrAmount) - customerStatemnt.Sum(e => e.CrAmount);

                //    summaryList.Add(new()
                //    {
                //        IsOpening = true,
                //        VendCode = statement.Key.VendCode,
                //        VendName = isArab ? customer.Vendor.VendArbName : customer.Vendor.VendName,
                //        OpeningBalance = openingBalance
                //    });
                //}
                //else
                //    openingBalance = 0;
                //  var vendorId = await _context.VendorMasters.Where(e => e.VendCode == item.Key.VendCode).Select(e => e.Id).FirstOrDefaultAsync();
                // var invoiceCount = (await invoices.CountAsync(e => e.CustomerId == vendorId)).ToString();

                summaryList.AddRange(statement.Select(item => new CustomerBalanceSummaryDto
                {
                    ItemCode = item.TranItemCode,
                    ItemName = isArab ? item.InvItemMaster.ShortNameAr : item.InvItemMaster.ShortName,
                    VendCode = item.VendCode,
                    VendName = isArab ? item.Vendor.VendArbName : item.Vendor.VendName,
                    Warehouse = item.Trans.WHCode,
                    //InvoiceNumber = item.TranId,
                    Remarks = invoices.Count(e => e.CustomerId == item.Vendor.Id).ToString(),
                    Trantype = item.TranItemQty.ToString(),
                    DrAmount = item.TranTotCost,
                    OpeningBalance = item.DiscAmt,
                    Balance = item.TranTotCost - item.DiscAmt,
                    CrAmount = item.TaxAmount,
                    NetTotalBalanceAmount = item.TranTotCost - item.DiscAmt + item.TaxAmount,
                })
                 .ToList());

                if (!isPovendanls)
                {
                    var items = summaryList.Where(e => !e.IsClosing && e.ItemCode == statement.Key);
                    (WarehouseItems ??= new List<CustomSelectListItem>()).AddRange(
                        items.Select(item => new CustomSelectListItem()
                        {
                            Text = item.ItemName,
                            TextTwo = item.Warehouse,
                            Value = item.Trantype
                        }));
                }

                //totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += hsummaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                totalItemQty = summaryList.Sum(e => decimal.Parse(e.Trantype));
                totalDrAmount = summaryList.Sum(e => e.DrAmount);
                totalCrAmount = summaryList.Sum(e => e.CrAmount);
                totalBalance = summaryList.Sum(e => e.Balance);
                totalOpeningBalance = summaryList.Sum(e => e.OpeningBalance);

                summaryList.Add(new()
                {
                    IsClosing = true,
                    Remarks = isArab ? "مجموع :" : "Total :",
                    Trantype = totalItemQty.ToString(),
                    DrAmount = totalDrAmount,
                    CrAmount = totalCrAmount,
                    Balance = totalBalance,
                    OpeningBalance = totalOpeningBalance
                });

                summaryItemsList.Add(summaryList);

                grandTotalDrAmount += totalDrAmount;
                grandTotalOpeningBalance += totalOpeningBalance;
                grandTotalBalance += totalBalance;
                grandTotalCrAmount += totalCrAmount;
                grandTotalItemQty += totalItemQty;
            }

            var grandTotalList = summaryItemsList.Where(e => e.Any(item => item.IsClosing == true));
            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

                TotalItemQty = grandTotalItemQty,
                TotalDrAmount = grandTotalDrAmount,
                TotalCrAmount = grandTotalCrAmount,
                TotalBalance = grandTotalBalance,
                TotalOpeningAmount = grandTotalOpeningBalance
            };
            summaryListDto.NetTotalBalanceAmount = summaryListDto.TotalBalance + summaryListDto.TotalCrAmount;

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
            summaryListDto.WarehouseItems = WarehouseItems;
            return summaryListDto;
        }
    }

    #endregion



    //#region GetPOVendorAnalysisSummary

    //public class GetPOVendorAnalysisSummary : IRequest<CustomerBalanceSummaryListDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string VendCode { get; set; }
    //    public DateTime? DateFrom { get; set; }
    //    public DateTime? DateTo { get; set; }
    //    public string BranchCode { get; set; }
    //    // public bool IsAllVendors { get; set; }
    //}
    //public class GetPOVendorAnalysisSummaryHandler : IRequestHandler<GetPOVendorAnalysisSummary, CustomerBalanceSummaryListDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetPOVendorAnalysisSummaryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<CustomerBalanceSummaryListDto> Handle(GetPOVendorAnalysisSummary request, CancellationToken cancellationToken)
    //    {
    //        var grnDetails = _context.GRNDetails
    //            .Include(e => e.Vendor)
    //            .Include(e => e.InvItemMaster)
    //            .AsNoTracking();

    //        //var grnDetails1 = grnDetails;
    //        //var pHeader = _context.OpmCustPaymentHeaders.AsNoTracking();
    //        //var pCustPayments = _context.OpmCustomerPayments.AsNoTracking();
    //        var invoices = _context.TranVenInvoices.AsNoTracking();

    //        // if (!request.IsAllVendors)
    //        if (request.VendCode.HasValue())
    //            grnDetails = grnDetails.Where(e => e.VendCode == request.VendCode);

    //        if (request.BranchCode.HasValue())
    //            grnDetails = grnDetails.Where(e => e.BranchCode == request.BranchCode);

    //        //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);

    //        List<List<CustomerBalanceSummaryDto>> summaryItemsList = new();
    //        bool hasDates = false;

    //        if (request.DateFrom is not null && request.DateTo is not null)
    //        {
    //            hasDates = true;
    //            // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode });

    //            grnDetails = grnDetails.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);

    //            // cStatements = cStatements1.Union(cStatements);

    //        }

    //        var statements = (await grnDetails.ToListAsync()).GroupBy(e => new { e.TranItemCode });// e.SndCustomerMaster.CustName });

    //        var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
    //        var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

    //        // var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

    //        bool isArab = request.User.Culture.IsArab();
    //        decimal? openingBalance = 0;
    //        decimal? totalBalance = 0, totalOpeningBalance = 0, totalItemQty = 0, totalDrAmount = 0, totalCrAmount = 0;
    //        decimal? grandTotalBalance = 0, grandTotalOpeningBalance = 0, grandTotalItemQty = 0, grandTotalDrAmount = 0, grandTotalCrAmount = 0;

    //        foreach (var statement in statements)
    //        {
    //            List<CustomerBalanceSummaryDto> summaryList = new();

    //            //if (hasDates)
    //            //{
    //            //    var customerStatemnt = cStatements1.Where(e => e.TranDate < request.DateFrom && e.VendCode == statement.Key.VendCode);
    //            //    var customer = await cStatements.FirstOrDefaultAsync(e => e.VendCode == statement.Key.VendCode);
    //            //    openingBalance = customerStatemnt.Sum(e => e.DrAmount) - customerStatemnt.Sum(e => e.CrAmount);

    //            //    summaryList.Add(new()
    //            //    {
    //            //        IsOpening = true,
    //            //        VendCode = statement.Key.VendCode,
    //            //        VendName = isArab ? customer.Vendor.VendArbName : customer.Vendor.VendName,
    //            //        OpeningBalance = openingBalance
    //            //    });
    //            //}
    //            //else
    //            //    openingBalance = 0;
    //            //  var vendorId = await _context.VendorMasters.Where(e => e.VendCode == item.Key.VendCode).Select(e => e.Id).FirstOrDefaultAsync();
    //            // var invoiceCount = (await invoices.CountAsync(e => e.CustomerId == vendorId)).ToString();

    //            summaryList.AddRange(statement.Select(item => new CustomerBalanceSummaryDto
    //            {
    //                //ItemCode = statement.Key.TranItemCode,
    //               // ItemName = isArab ? item.InvItemMaster.ShortNameAr : item.InvItemMaster.ShortName,
    //                VendCode = item.VendCode,
    //                VendName = isArab ? item.Vendor.VendArbName : item.Vendor.VendName,
    //                //InvoiceNumber = item.TranId,
    //                Remarks = invoices.Count(e => e.CustomerId == item.Vendor.Id).ToString(),
    //                Trantype = item.TranItemQty.ToString(),
    //                DrAmount = item.TranTotCost,
    //                OpeningBalance = item.DiscAmt,
    //                Balance = item.TranTotCost - item.DiscAmt,
    //                CrAmount = item.TaxAmount,
    //                NetTotalBalanceAmount = item.TranTotCost - item.DiscAmt + item.TaxAmount,
    //            })
    //             .ToList());

    //            //totalBalance += summaryList.Sum(e => e.Balance);
    //            //totalDrAmount += hsummaryList.Sum(e => e.DrAmount);
    //            //totalCrAmount += summaryList.Sum(e => e.CrAmount);

    //            totalItemQty = summaryList.Sum(e => decimal.Parse(e.Trantype));
    //            totalDrAmount = summaryList.Sum(e => e.DrAmount);
    //            totalCrAmount = summaryList.Sum(e => e.CrAmount);
    //            totalBalance = summaryList.Sum(e => e.Balance);
    //            totalOpeningBalance = summaryList.Sum(e => e.OpeningBalance);

    //            summaryList.Add(new()
    //            {
    //                IsClosing = true,
    //                Remarks = isArab ? "مجموع :" : "Total :",
    //                Trantype = totalItemQty.ToString(),
    //                DrAmount = totalDrAmount,
    //                CrAmount = totalCrAmount,
    //                Balance = totalBalance,
    //                OpeningBalance = totalOpeningBalance
    //            });

    //            summaryItemsList.Add(summaryList);

    //            grandTotalDrAmount += totalDrAmount;
    //            grandTotalOpeningBalance += totalOpeningBalance;
    //            grandTotalBalance += totalBalance;
    //            grandTotalCrAmount += totalCrAmount;
    //            grandTotalItemQty += totalItemQty;
    //        }

    //        var grandTotalList = summaryItemsList.Where(e => e.Any(item => item.IsClosing == true));
    //        CustomerBalanceSummaryListDto summaryListDto = new()
    //        {
    //            ComapnyName = company.CompanyName,
    //            LogoURL = company.LogoURL,
    //            BranchName = companyBranch.BranchName,
    //            Address = company.CompanyAddress,
    //            Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,

    //            TotalItemQty = grandTotalItemQty,
    //            TotalDrAmount = grandTotalDrAmount,
    //            TotalCrAmount = grandTotalCrAmount,
    //            TotalBalance = grandTotalBalance,
    //            TotalOpeningAmount = grandTotalOpeningBalance
    //        };
    //        summaryListDto.NetTotalBalanceAmount = summaryListDto.TotalBalance + summaryListDto.TotalCrAmount;

    //        //summaryListDto.AddRange(summaryList1);
    //        summaryListDto.ListItems = summaryItemsList;
    //        return summaryListDto;
    //    }
    //}

    //#endregion
}
