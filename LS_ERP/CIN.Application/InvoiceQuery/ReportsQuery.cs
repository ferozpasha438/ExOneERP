using AutoMapper;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{

    #region Customer



    #region GetCustomerBalanceSummaryList

    public class GetCustomerBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public bool IsSummary { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerBalanceSummaryListHandler : IRequestHandler<GetCustomerBalanceSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomerBalanceSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.TrnCustomerStatements
                .Include(e => e.SndCustomerMaster)
                .Include(e => e.Invoice)
                .AsNoTracking();
            var custSites = _context.OprSites.AsNoTracking();

            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

            if (request.SiteCode.HasValue())
                cStatements = cStatements.Where(e => e.Invoice.SiteCode == request.SiteCode);

            bool isArab = request.User.Culture.IsArab();
            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                var cStatements1 = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom);
                var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName, e.Trantype });


                foreach (var item in statements1)
                {
                    var inv = item.ToList().FirstOrDefault()?.Invoice;

                    summaryList1.Add(new()
                    {
                        VendCode = item.Key.CustCode,
                        VendName = item.Key.CustName,
                        InvoiceNumber = inv?.InvoiceNumber,
                        Remarks = inv?.Remarks,
                        Trantype = item.Key.Trantype,
                        SiteName = inv != null && inv.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteArbName
                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteName)
                                                     : String.Empty,
                        DrAmount = item.Sum(e => e.DrAmount),
                        CrAmount = item.Sum(e => e.CrAmount),
                        Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                    });
                }

                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);

            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);
            var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName, e.Trantype });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                var inv = item.ToList().FirstOrDefault()?.Invoice;
                summaryList.Add(new()
                {
                    VendCode = item.Key.CustCode,
                    VendName = item.Key.CustName,
                    InvoiceNumber = inv?.InvoiceNumber,
                    Remarks = inv?.Remarks,
                    Trantype = item.Key.Trantype,
                    SiteName = inv != null && inv.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteArbName
                                                                           : custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteName)
                                                                : String.Empty,
                    DrAmount = item.Sum(e => e.DrAmount),
                    CrAmount = item.Sum(e => e.CrAmount),
                    Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion


    #region GetCustomersBalanceSummaryDetailsList

    public class GetCustomersBalanceSummaryDetailsList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        // public bool IsAllVendors { get; set; }
        public bool IsSummary { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomersBalanceSummaryDetailsListHandler : IRequestHandler<GetCustomersBalanceSummaryDetailsList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomersBalanceSummaryDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomersBalanceSummaryDetailsList request, CancellationToken cancellationToken)
        {

            try
            {
                var cStatements = _context.TrnCustomerStatements
                    .Include(e => e.SndCustomerMaster)
                    .Include(e => e.Invoice)
                    .AsNoTracking();

                var cStatements1 = cStatements;
                var pHeader = _context.OpmCustPaymentHeaders.AsNoTracking();
                var pCustPayments = _context.OpmCustomerPayments.AsNoTracking();
                var custSites = _context.OprSites.AsNoTracking();
                //  var custMasters = _context.OprCustomers.AsNoTracking();
                //  var invoiceItems = _context.TranInvoices.AsNoTracking();

                // if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

                if (request.SiteCode.HasValue())
                {
                    if (request.CustCode.HasValue())
                    {
                        var invoiceNumbs = cStatements.Where(e => e.SiteCode == request.SiteCode).Select(e => e.TranNumber);
                        var paymentsPaymentIds = pCustPayments.Where(e => invoiceNumbs.Any(inv => inv == e.InvoiceNumber)).Select(e => e.PaymentId);
                        var pHeaderPaymentNumbers = pHeader.Where(e => paymentsPaymentIds.Any(payId => payId == e.Id)).Select(e => e.PaymentNumber);

                        cStatements = cStatements.Where(e => e.SiteCode == request.SiteCode || pHeaderPaymentNumbers.Any(pnumb => pnumb == e.PaymentId));
                    }
                    else
                        cStatements = cStatements.Where(e => e.SiteCode == request.SiteCode);


                }
                //  List<CustomerBalanceSummaryDto> summaryList1 = new();

                //if (request.DateFrom is not null && request.DateTo is not null)
                //{
                //    var cStatements1 = cStatements.Where(e => e.TranDate < request.DateFrom);
                //    var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });


                //    foreach (var statement in statements1)
                //    {
                //        foreach (var item in collection)
                //        {

                //            summaryList1.Add(new()
                //            {
                //                VendCode = item.Key.CustCode,
                //                VendName = item.Key.CustName,
                //                DrAmount = item.Sum(e => e.DrAmount),
                //                CrAmount = item.Sum(e => e.CrAmount),
                //                Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //            });
                //        }
                //    }

                //    cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);

                //}



                //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);

                List<List<CustomerBalanceSummaryDto>> summaryItemsList = new();
                bool hasDates = false;

                if (request.DateFrom is not null && request.DateTo is not null)
                {
                    hasDates = true;
                    // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode });

                    cStatements = cStatements.Select(e => new TblFinTrnCustomerStatement
                    {
                        InvoiceId = e.InvoiceId,
                        Invoice = e.Invoice,
                        PaymentId = e.PaymentId,
                        CustCode = e.CustCode,
                        SndCustomerMaster = e.SndCustomerMaster,
                        TranDate = e.InvoiceId != null ? e.TranDate : pHeader.Where(ph => ph.PaymentNumber == e.PaymentId).Select(e => e.TranDate).FirstOrDefault(),
                        Remarks1 = e.Remarks1,
                        Trantype = e.Trantype,
                        DrAmount = e.DrAmount,
                        CrAmount = e.CrAmount,

                    });
                    cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);

                    // cStatements = cStatements1.Union(cStatements);

                }



                var reportCount = await cStatements.CountAsync();
                cStatements = cStatements.Pagination(request.ReportIndex, request.ReportCount);

                var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.CustCode });// e.SndCustomerMaster.CustName });

                var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
                var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

                // var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

                bool isArab = request.User.Culture.IsArab();
                decimal? openingBalance = 0;
                decimal? totalBalance = 0, totalDrAmount = 0, totalCrAmount = 0;
                foreach (var statement in statements)
                {
                    List<CustomerBalanceSummaryDto> summaryList = new();

                    if (hasDates)
                    {
                        var customerStatemnt = cStatements1.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom && e.CustCode == statement.Key.CustCode);
                        var customer = await cStatements.FirstOrDefaultAsync(e => e.CustCode == statement.Key.CustCode);
                        openingBalance = customerStatemnt.Sum(e => e.DrAmount) - customerStatemnt.Sum(e => e.CrAmount);

                        summaryList.Add(new()
                        {
                            IsOpening = true,
                            VendCode = statement.Key.CustCode,
                            VendName = isArab ? customer.SndCustomerMaster.CustArbName : customer.SndCustomerMaster.CustName,
                            OpeningBalance = openingBalance
                        });
                    }
                    else
                        openingBalance = 0;


                    //foreach (var item in statement)
                    //{
                    //    var inv = await invoiceItems.FirstOrDefaultAsync(e => e.Id == item.InvoiceId);
                    //    var customer = await custMasters.FirstOrDefaultAsync(e => e.CustCode == statement.Key.CustCode);
                    //    var summaryDto = new CustomerBalanceSummaryDto
                    //    {
                    //        InvoiceId = item.InvoiceId,
                    //        RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                    //        TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                    //        InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                    //        Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                    //        Trantype = item.Trantype,
                    //        VendCode = item.CustCode,
                    //        VendName = isArab ? customer.CustArbName : customer.CustName,
                    //        DrAmount = item.DrAmount,
                    //        CrAmount = item.CrAmount,
                    //        SiteName = item.InvoiceId != null &&
                    //                inv.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteArbName
                    //                                                              : custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteName)
                    //                                 : String.Empty,
                    //        Balance = (item.DrAmount - item.CrAmount)
                    //    };

                    //    summaryList.Add(summaryDto);
                    //}


                    summaryList.AddRange(statement.Select(item => new CustomerBalanceSummaryDto
                    {
                        InvoiceId = item.InvoiceId,
                        RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                        TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                        InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                        Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                        Trantype = item.Trantype,
                        VendCode = item.CustCode,
                        VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                        DrAmount = item.DrAmount,
                        CrAmount = item.CrAmount,
                        SiteName = item.InvoiceId != null &&
                        item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                      : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                         : String.Empty,
                        Balance = (item.DrAmount - item.CrAmount)
                    }).OrderBy(e => e.TranDate)
                     .ToList());




                    //foreach (var item in statement)
                    //{
                    //    summaryList.Add(new()
                    //    {
                    //        InvoiceId = item.InvoiceId,
                    //        RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                    //        TranDate = item.TranDate,
                    //        InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                    //        Remarks = item.Invoice?.Remarks,
                    //        Trantype = item.Trantype,
                    //        VendCode = item.CustCode,
                    //        VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                    //        DrAmount = item.DrAmount,
                    //        CrAmount = item.CrAmount,
                    //        Balance = (item.DrAmount - item.CrAmount)
                    //    });
                    //}

                    totalBalance += summaryList.Sum(e => e.Balance);
                    //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                    //totalCrAmount += summaryList.Sum(e => e.CrAmount);                

                    totalDrAmount = summaryList.GroupBy(e => new { e.InvoiceId, e.DrAmount }).Sum(e => e.Key.DrAmount);
                    totalCrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount);

                    summaryList.Add(new()
                    {
                        Remarks = isArab ? "مجموع :" : "Total :",
                        DrAmount = totalDrAmount,
                        CrAmount = totalCrAmount,
                        Balance = summaryList.Sum(e => e.Balance)
                    });

                    decimal? drOpeningBalance = totalDrAmount + (openingBalance >= 0 ? openingBalance : 0);
                    decimal? crOpeningBalance = totalCrAmount + (openingBalance < 0 ? (-1) * openingBalance : 0);

                    summaryList.Add(new()
                    {
                        IsClosing = true,
                        ClosingBalance = drOpeningBalance - crOpeningBalance
                    });

                    summaryItemsList.Add(summaryList);
                }


                CustomerBalanceSummaryListDto summaryListDto = new()
                {
                    ComapnyName = company.CompanyName,
                    LogoURL = company.LogoURL,
                    BranchName = companyBranch.BranchName,
                    Address = company.CompanyAddress,
                    Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                    //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                    //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                    TotalBalance = totalBalance
                };

                //summaryListDto.AddRange(summaryList1);
                //summaryListDto.ListItems = summaryItemsList;
                summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
                return summaryListDto;


            }
            catch (Exception ex)
            {
                throw;
            }
            //return new()
            //{
            //    List = summaryItemsList,
            //    ComapnyName = company.CompanyName,
            //    LogoURL = company.LogoURL,
            //    BranchName = companyBranch.BranchName,
            //    Address = company.CompanyAddress,
            //    //TotalDrAmount = totalDrAmount,
            //    //TotalCrAmount = totalCrAmount,
            //    TotalBalance = totalBalance//summaryItemsList.Last().Sum(e => e.DrAmount)
            //};


        }
    }

    #endregion


    #region GetCustomerBalanceDetailsList

    public class GetCustomerBalanceDetailsList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        // public bool IsAllVendors { get; set; }
        public bool IsSummary { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerBalanceDetailsListHandler : IRequestHandler<GetCustomerBalanceDetailsList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerBalanceDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomerBalanceDetailsList request, CancellationToken cancellationToken)
        {

            try
            {
                var cStatements = _context.TrnCustomerStatements
                    .Include(e => e.SndCustomerMaster)
                    .Include(e => e.Invoice)
                    .AsNoTracking();

                bool hasSitCode = false;
                var cStatements1 = cStatements;
                var pHeader = _context.OpmCustPaymentHeaders.AsNoTracking();
                var pCustPayments = _context.OpmCustomerPayments.AsNoTracking();
                var custSites = _context.OprSites.AsNoTracking();
                //  var custMasters = _context.OprCustomers.AsNoTracking();
                //  var invoiceItems = _context.TranInvoices.AsNoTracking();

                // if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

                if (request.SiteCode.HasValue())
                {
                    if (request.CustCode.HasValue())
                    {
                        hasSitCode = true;
                        var invoiceNumbs = cStatements.Where(e => e.SiteCode == request.SiteCode).Select(e => e.TranNumber);
                        var paymentsPaymentIds = pCustPayments.Where(e => invoiceNumbs.Any(inv => inv == e.InvoiceNumber)).Select(e => e.PaymentId);
                        var pHeaderPaymentNumbers = pHeader.Where(e => paymentsPaymentIds.Any(payId => payId == e.Id)).Select(e => e.PaymentNumber);

                        cStatements = cStatements.Where(e => e.SiteCode == request.SiteCode);
                        //cStatements = cStatements.Where(e => e.SiteCode == request.SiteCode || pHeaderPaymentNumbers.Any(pnumb => pnumb == e.PaymentId));
                    }
                    else
                    {
                        hasSitCode = false;
                        cStatements = cStatements.Where(e => e.SiteCode == request.SiteCode);
                    }
                }

                //  List<CustomerBalanceSummaryDto> summaryList1 = new();

                //if (request.DateFrom is not null && request.DateTo is not null)
                //{
                //    var cStatements1 = cStatements.Where(e => e.TranDate < request.DateFrom);
                //    var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });


                //    foreach (var statement in statements1)
                //    {
                //        foreach (var item in collection)
                //        {

                //            summaryList1.Add(new()
                //            {
                //                VendCode = item.Key.CustCode,
                //                VendName = item.Key.CustName,
                //                DrAmount = item.Sum(e => e.DrAmount),
                //                CrAmount = item.Sum(e => e.CrAmount),
                //                Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
                //            });
                //        }
                //    }

                //    cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);

                //}



                //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);

                List<List<CustomerBalanceSummaryDto>> summaryItemsList = new();
                bool hasDates = false;

                if (request.DateFrom is not null && request.DateTo is not null)
                {
                    hasDates = true;
                    // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode });

                    cStatements = cStatements.Select(e => new TblFinTrnCustomerStatement
                    {
                        InvoiceId = e.InvoiceId,
                        Invoice = e.Invoice,
                        PaymentId = e.PaymentId,
                        SiteCode = e.SiteCode,
                        CustCode = e.CustCode,
                        SndCustomerMaster = e.SndCustomerMaster,
                        TranDate = (e.InvoiceId != null ? e.TranDate : pHeader.Where(ph => ph.PaymentNumber == e.PaymentId).Select(e => e.TranDate).FirstOrDefault() ?? e.TranDate),
                        Remarks1 = e.Remarks1,
                        Trantype = e.Trantype,
                        DrAmount = e.DrAmount,
                        CrAmount = e.CrAmount,

                    });
                    cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);

                    // cStatements = cStatements1.Union(cStatements);

                }



                var reportCount = await cStatements.CountAsync();
                cStatements = cStatements.Pagination(request.ReportIndex, request.ReportCount);

                var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.CustCode });// e.SndCustomerMaster.CustName });

                var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
                var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

                // var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

                bool isArab = request.User.Culture.IsArab();
                decimal? openingBalance = 0;
                decimal? totalBalance = 0, totalDrAmount = 0, totalCrAmount = 0;

                foreach (var statement in statements)
                {
                    List<CustomerBalanceSummaryDto> summaryList = new();

                    if (hasDates)
                    {
                        var customerStatemnt = cStatements1.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom && e.CustCode == statement.Key.CustCode);
                        if (request.SiteCode.HasValue())
                            customerStatemnt = customerStatemnt.Where(e => e.SiteCode == request.SiteCode);

                        var customer = await cStatements.FirstOrDefaultAsync(e => e.CustCode == statement.Key.CustCode);
                        openingBalance = customerStatemnt.Sum(e => e.DrAmount) - customerStatemnt.Sum(e => e.CrAmount);

                        summaryList.Add(new()
                        {
                            IsOpening = true,
                            VendCode = statement.Key.CustCode,
                            VendName = isArab ? customer.SndCustomerMaster.CustArbName : customer.SndCustomerMaster.CustName,
                            OpeningBalance = openingBalance
                        });
                    }
                    else
                        openingBalance = 0;


                    //foreach (var item in statement)
                    //{
                    //    var inv = await invoiceItems.FirstOrDefaultAsync(e => e.Id == item.InvoiceId);
                    //    var customer = await custMasters.FirstOrDefaultAsync(e => e.CustCode == statement.Key.CustCode);
                    //    var summaryDto = new CustomerBalanceSummaryDto
                    //    {
                    //        InvoiceId = item.InvoiceId,
                    //        RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                    //        TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                    //        InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                    //        Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                    //        Trantype = item.Trantype,
                    //        VendCode = item.CustCode,
                    //        VendName = isArab ? customer.CustArbName : customer.CustName,
                    //        DrAmount = item.DrAmount,
                    //        CrAmount = item.CrAmount,
                    //        SiteName = item.InvoiceId != null &&
                    //                inv.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteArbName
                    //                                                              : custSites.FirstOrDefault(site => site.SiteCode == inv.SiteCode).SiteName)
                    //                                 : String.Empty,
                    //        Balance = (item.DrAmount - item.CrAmount)
                    //    };

                    //    summaryList.Add(summaryDto);
                    //}

                    if (hasSitCode)
                    {

                        var listOfStatements = statement.Select(item => new CustomerBalanceSummaryDto
                        {
                            InvoiceId = item.InvoiceId,
                            PaymentId = item.PaymentId,
                            RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId && e.SiteCode == item.SiteCode).Select(e => e.InvoiceNumber).ToList(),
                            TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                            InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                            Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                            Trantype = item.Trantype,
                            VendCode = item.CustCode,
                            VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                            DrAmount = item.DrAmount,
                            CrAmount = item.CrAmount,
                            SiteCode = item.SiteCode,
                            SiteName = item.InvoiceId != null &&
                                        item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                                      : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                                         : (item.SiteCode.HasValue() ?
                                                                            (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteArbName
                                                                                                      : custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteName)
                                                                                    : String.Empty),
                            Balance = (item.DrAmount - item.CrAmount)
                        }).OrderBy(e => e.TranDate).ToList();

                        foreach (var item in listOfStatements)
                        {
                            summaryList.Add(item);
                            if (item.PaymentId is not null && item.SiteCode.HasValue() && summaryList.Count(e => e.PaymentId == item.PaymentId && e.SiteCode == item.SiteCode) > 1)
                                summaryList.Remove(item);
                        }


                    }
                    else
                    {
                        var singleReceitList = statement.ToList().GroupBy(e => new { e.InvoiceId, e.PaymentId });

                        foreach (var singleReceit in singleReceitList)
                        {
                            if (singleReceit.Where(e => e.PaymentId > 0).Count() > 1)
                            {
                                summaryList.AddRange(singleReceit.Select(item => new CustomerBalanceSummaryDto
                                {
                                    InvoiceId = item.InvoiceId,
                                    PaymentId = item.PaymentId,
                                    RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                                    TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                                    InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                                    Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                                    Trantype = item.Trantype,// + " = Count : " + singleReceit.Key.PaymentId + "_" + singleReceit.Count(),
                                    VendCode = item.CustCode,
                                    VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                                    DrAmount = singleReceit.Sum(e => e.DrAmount),
                                    CrAmount = singleReceit.Sum(e => e.CrAmount),
                                    SiteCode = item.SiteCode,
                                    SiteName = item.InvoiceId != null &&
                                    item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                                     : (item.SiteCode.HasValue() ?
                                                                        (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteArbName
                                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteName)
                                                                                : String.Empty),
                                    Balance = (singleReceit.Sum(e => e.DrAmount) - singleReceit.Sum(e => e.CrAmount))
                                }).Take(1).OrderBy(e => e.TranDate).ToList());
                            }
                            else
                            {
                                summaryList.AddRange(singleReceit.Select(item => new CustomerBalanceSummaryDto
                                {
                                    InvoiceId = item.InvoiceId,
                                    PaymentId = item.PaymentId,
                                    RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                                    TranDate = item.TranDate,//item.InvoiceId != null ? item.TranDate : (pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault()?.TranDate ?? item.TranDate),
                                    InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                                    Remarks = item.Invoice?.Remarks ?? item.Remarks1,
                                    Trantype = item.Trantype,// + " = Count : " + singleReceit.Key.PaymentId + "_" + singleReceit.Count(),
                                    VendCode = item.CustCode,
                                    VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                                    DrAmount = item.DrAmount,
                                    CrAmount = item.CrAmount,
                                    SiteCode = item.SiteCode,
                                    SiteName = item.InvoiceId != null &&
                                    item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                                     : (item.SiteCode.HasValue() ?
                                                                        (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteArbName
                                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.SiteCode).SiteName)
                                                                                : String.Empty),
                                    Balance = (item.DrAmount - item.CrAmount)
                                }).OrderBy(e => e.TranDate).ToList());
                            }
                        }

                    }


                    //For Calculating Running Balance
                    bool firstRec = true; decimal? prevBalance = 0;

                    foreach (var item in summaryList.Where(e => e.IsOpening == false))
                    {
                        if (firstRec)
                        {
                            item.Balance = item.Balance + openingBalance;
                            prevBalance = item.Balance;
                            firstRec = false;
                        }
                        else
                        {
                            item.Balance = item.Balance + prevBalance;
                            prevBalance = item.Balance;

                        }
                    }



                    //foreach (var item in summaryList)
                    //{
                    //    if (item.RefInvoiceIds is not null && item.RefInvoiceIds.Count() > 1)
                    //    {
                    //        item.RefInvoiceIds = pCustPayments.Where(e => e.PaymentId == item.PaymentId && e.SiteCode == item.SiteCode).Select(e => e.InvoiceNumber).ToList();
                    //    }
                    //}



                    //foreach (var item in statement)
                    //{
                    //    summaryList.Add(new()
                    //    {
                    //        InvoiceId = item.InvoiceId,
                    //        RefInvoiceIds = pCustPayments.Where(e => pHeader.Where(ph => ph.PaymentNumber == item.PaymentId).FirstOrDefault().Id == e.PaymentId).Select(e => e.InvoiceNumber).ToList(),
                    //        TranDate = item.TranDate,
                    //        InvoiceNumber = item.Invoice?.InvoiceNumber ?? item.PaymentId.ToString(),
                    //        Remarks = item.Invoice?.Remarks,
                    //        Trantype = item.Trantype,
                    //        VendCode = item.CustCode,
                    //        VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                    //        DrAmount = item.DrAmount,
                    //        CrAmount = item.CrAmount,
                    //        Balance = (item.DrAmount - item.CrAmount)
                    //    });
                    //}

                    totalBalance += summaryList.Sum(e => e.Balance);
                    //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                    //totalCrAmount += summaryList.Sum(e => e.CrAmount);                

                    totalDrAmount = summaryList.GroupBy(e => new { e.InvoiceId, e.DrAmount }).Sum(e => e.Key.DrAmount);
                    totalCrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount);

                    summaryList.Add(new()
                    {
                        Warehouse = "balance_nill",
                        Remarks = isArab ? "مجموع :" : "Total :",
                        DrAmount = totalDrAmount + openingBalance,
                        CrAmount = totalCrAmount,
                        Balance = summaryList.Sum(e => e.Balance)
                    });

                    decimal? drOpeningBalance = totalDrAmount + (openingBalance >= 0 ? openingBalance : 0);
                    decimal? crOpeningBalance = totalCrAmount + (openingBalance < 0 ? (-1) * openingBalance : 0);

                    summaryList.Add(new()
                    {
                        IsClosing = true,
                        ClosingBalance = drOpeningBalance - crOpeningBalance
                    });

                    summaryItemsList.Add(summaryList);
                }

                CustomerBalanceSummaryListDto summaryListDto = new()
                {
                    ComapnyName = company.CompanyName,
                    LogoURL = company.LogoURL,
                    BranchName = companyBranch.BranchName,
                    Address = company.CompanyAddress,
                    Mobile = company.Mobile.HasValue() ? company.Mobile : company.Phone,
                    //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                    //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                    TotalBalance = totalBalance
                };

                //summaryListDto.AddRange(summaryList1);
                //summaryListDto.ListItems = summaryItemsList;

                summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
                return summaryListDto;


            }
            catch (Exception ex)
            {
                throw;
            }
            //return new()
            //{
            //    List = summaryItemsList,
            //    ComapnyName = company.CompanyName,
            //    LogoURL = company.LogoURL,
            //    BranchName = companyBranch.BranchName,
            //    Address = company.CompanyAddress,
            //    //TotalDrAmount = totalDrAmount,
            //    //TotalCrAmount = totalCrAmount,
            //    TotalBalance = totalBalance//summaryItemsList.Last().Sum(e => e.DrAmount)
            //};


        }
    }

    #endregion


    //#region GetCustomerBalanceSummaryList

    //public class GetCustomerBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string CustCode { get; set; }
    //    public DateTime? DateFrom { get; set; }
    //    public DateTime? DateTo { get; set; }
    //    public bool IsAllVendors { get; set; }
    //}
    //public class GetCustomerBalanceSummaryListHandler : IRequestHandler<GetCustomerBalanceSummaryList, CustomerBalanceSummaryListDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetCustomerBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomerBalanceSummaryList request, CancellationToken cancellationToken)
    //    {
    //        var cStatements = _context.TrnCustomerStatements.Include(e => e.SndCustomerMaster).AsNoTracking();
    //        if (!request.IsAllVendors)
    //            if (request.CustCode.HasValue())
    //                cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

    //        bool isArabic = request.User.Culture.IsArab();

    //        List<CustomerBalanceSummaryDto> summaryList1 = new();
    //        if (request.DateFrom is not null && request.DateTo is not null)
    //        {
    //            var cStatements1 = cStatements.Where(e => e.TranDate < request.DateFrom);
    //            // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });


    //            foreach (var item in cStatements1)
    //            {
    //                summaryList1.Add(new()
    //                {
    //                    VendCode = item.CustCode,
    //                    VendName = isArabic ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
    //                    DrAmount = item.DrAmount,
    //                    CrAmount = item.CrAmount,
    //                    Balance = item.DrAmount - item.CrAmount
    //                });
    //            }


    //            cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);


    //        }

    //        //var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });

    //        var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
    //        var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

    //        List<CustomerBalanceSummaryDto> summaryList = new();
    //        foreach (var item in cStatements)
    //        {
    //            summaryList.Add(new()
    //            {
    //                VendCode = item.CustCode,
    //                VendName = isArabic ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
    //                DrAmount = item.DrAmount,
    //                CrAmount = item.CrAmount,
    //                Balance = item.DrAmount - item.CrAmount
    //            });
    //        }

    //        CustomerBalanceSummaryListDto summaryListDto = new()
    //        {
    //            ComapnyName = company.CompanyName,
    //            LogoURL = company.LogoURL,
    //            BranchName = companyBranch.BranchName,
    //            Address = company.CompanyAddress,
    //            TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
    //            TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
    //            TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
    //        };

    //        summaryList.AddRange(summaryList1);
    //        summaryListDto.List = summaryList;
    //        return summaryListDto;

    //    }
    //}

    //#endregion



    #region GetCustomerAllBalanceSummaryList

    public class GetCustomerAllBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }
    }
    public class GetCustomerAllBalanceSummaryListHandler : IRequestHandler<GetCustomerAllBalanceSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerAllBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomerAllBalanceSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.TrnCustomerStatements.Include(e => e.SndCustomerMaster).AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

            bool isArabic = request.User.Culture.IsArab();

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                var cStatements1 = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom);
                // var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });


                foreach (var item in await cStatements1.ToListAsync())
                {
                    summaryList1.Add(new()
                    {
                        VendCode = item.CustCode,
                        VendName = isArabic ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                        DrAmount = item.DrAmount,
                        CrAmount = item.CrAmount,
                        Balance = item.DrAmount - item.CrAmount
                    });
                }


                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);


            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            if (request.Type.HasValue())
            {
                var vendInvoices = _context.OpmCustomerPayments.Where(e => e.CustCode == request.CustCode).Select(e => e.InvoiceNumber);

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cStatements = cStatements.Where(e => vendInvoices.Any(eid => eid == e.TranNumber));
                    else
                        cStatements = cStatements.Where(e => !vendInvoices.Any(eid => eid == e.TranNumber));
                }
            }


            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in await cStatements.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.CustCode,
                    VendName = isArabic ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                    DrAmount = item.DrAmount,
                    CrAmount = item.CrAmount,
                    Balance = item.DrAmount - item.CrAmount
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion

    #region GetCustomerPaymentSummaryList

    public class GetCustomerPaymentSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerPaymentSummaryListHandler : IRequestHandler<GetCustomerPaymentSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerPaymentSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetCustomerPaymentSummaryList request, CancellationToken cancellationToken)
        {
            var cPayments = _context.TrnCustomerPayments.Include(e => e.SndCustomerMaster).AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cPayments = cPayments.Where(e => e.CustCode == request.CustCode);

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);
            var statements = (await cPayments.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndCustomerMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    VendCode = item.Key.CustCode,
                    VendName = item.Key.CustName,
                    DrAmount = item.Sum(e => e.Amount),
                });
            }

            return new()
            {
                List = summaryList,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = summaryList.Sum(e => e.DrAmount)
            };

        }
    }

    #endregion


    //#region GetCustomerVoucherSummaryList

    //public class GetCustomerVoucherSummaryList : IRequest<CustomerVoucherSummaryListDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string Type { get; set; }
    //    public string CustCode { get; set; }
    //    public DateTime? DateFrom { get; set; }
    //    public DateTime? DateTo { get; set; }
    //    public bool IsAllBranches { get; set; }
    //}
    //public class GetCustomerVoucherSummaryListHandler : IRequestHandler<GetCustomerVoucherSummaryList, CustomerVoucherSummaryListDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetCustomerVoucherSummaryListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<CustomerVoucherSummaryListDto> Handle(GetCustomerVoucherSummaryList request, CancellationToken cancellationToken)
    //    {
    //        bool isArab = request.User.Culture.IsArab();
    //        var cPayments = _context.TrnCustomerPayments.Include(e => e.SndCustomerMaster).AsNoTracking();
    //        if (!request.IsAllBranches)
    //            cPayments = cPayments.Where(e => e.BranchCode == request.User.BranchCode);

    //        if (request.CustCode.HasValue())
    //            cPayments = cPayments.Where(e => e.CustCode == request.CustCode);


    //        if (request.DateFrom is not null && request.DateTo is not null)
    //        {
    //            cPayments = cPayments.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
    //        }

    //        var cStatements = (await cPayments.ToListAsync()).GroupBy(e => new { e.CustCode });//, CustName = isArab ? e.SndCustomerMaster.CustArbName : e.SndCustomerMaster.CustName });

    //        var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
    //        var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
    //        var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

    //        List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
    //        foreach (var cStatement in cStatements)
    //        {
    //            List<CustomerVoucherSummaryDto> summaryList = new();
    //            foreach (var item in cStatement)
    //            {
    //                summaryList.Add(new()
    //                {
    //                    VendCode = item.CustCode,
    //                    VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
    //                    DrAmount = item.Amount,
    //                    BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
    //                    CheckNumber = item.CheckNumber,
    //                    CheckDate = item.Checkdate,
    //                    DocNum = item.DocNum,
    //                    PayCode = item.PayCode,
    //                    PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
    //                });
    //            }
    //            summaryItemsList.Add(summaryList);
    //        }

    //        return new()
    //        {
    //            List = summaryItemsList,
    //            ComapnyName = company.CompanyName,
    //            LogoURL = company.LogoURL,
    //            BranchName = companyBranch.BranchName,
    //            Address = company.CompanyAddress,
    //            TotalBalance = summaryItemsList.Sum(e => e.Sum(t => t.DrAmount))
    //        };

    //    }
    //}

    //#endregion



    //#region GetVendorBalanceSummaryList

    //public class GetVendorBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string CustCode { get; set; }
    //    public DateTime? DateFrom { get; set; }
    //    public DateTime? DateTo { get; set; }
    //    public bool IsAllVendors { get; set; }
    //}
    //public class GetVendorBalanceSummaryListHandler : IRequestHandler<GetVendorBalanceSummaryList, CustomerBalanceSummaryListDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetVendorBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorBalanceSummaryList request, CancellationToken cancellationToken)
    //    {
    //        var cStatements = _context.TrnVendorStatements.Include(e => e.SndVendorMaster).AsNoTracking();
    //        if (!request.IsAllVendors)
    //            if (request.CustCode.HasValue())
    //                cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

    //        List<CustomerBalanceSummaryDto> summaryList1 = new();
    //        if (request.DateFrom is not null && request.DateTo is not null)
    //        {
    //            var cStatements1 = cStatements.Where(e => e.TranDate < request.DateFrom);
    //            var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });


    //            foreach (var item in statements1)
    //            {
    //                summaryList1.Add(new()
    //                {
    //                    VendCode = item.Key.VendCode,
    //                    VendName = item.Key.VendName,
    //                    DrAmount = item.Sum(e => e.DrAmount),
    //                    CrAmount = item.Sum(e => e.CrAmount),
    //                    Balance = item.Sum(e => e.CrAmount) - item.Sum(e => e.CrAmount)
    //                });
    //            }

    //            cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);


    //        }

    //        //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);
    //        var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });

    //        var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
    //        var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

    //        List<CustomerBalanceSummaryDto> summaryList = new();
    //        foreach (var item in statements)
    //        {
    //            summaryList.Add(new()
    //            {
    //                VendCode = item.Key.VendCode,
    //                VendName = item.Key.VendName,
    //                DrAmount = item.Sum(e => e.DrAmount),
    //                CrAmount = item.Sum(e => e.CrAmount),
    //                Balance = item.Sum(e => e.CrAmount) - item.Sum(e => e.CrAmount)
    //            });
    //        }

    //        CustomerBalanceSummaryListDto summaryListDto = new()
    //        {
    //            ComapnyName = company.CompanyName,
    //            LogoURL = company.LogoURL,
    //            BranchName = companyBranch.BranchName,
    //            Address = company.CompanyAddress,
    //            TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
    //            TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
    //            TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
    //        };

    //        summaryList.AddRange(summaryList1);
    //        summaryListDto.List = summaryList;
    //        return summaryListDto;

    //    }
    //}

    //#endregion


    #region GetCustomerVoucherSummaryList

    public class GetCustomerVoucherSummaryList : IRequest<CustomerVoucherSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllBranches { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
        //public string Type { get; set; }

    }
    public class GetCustomerVoucherSummaryListHandler : IRequestHandler<GetCustomerVoucherSummaryList, CustomerVoucherSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVoucherSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerVoucherSummaryListDto> Handle(GetCustomerVoucherSummaryList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var cPayments = _context.TrnCustomerInvoices.Include(e => e.SndCustomerMaster).AsNoTracking();

            //if (!request.IsAllBranches)
            //    cPayments = cPayments.Where(e => e.BranchCode == request.User.BranchCode);

            if (request.CustCode.HasValue())
                cPayments = cPayments.Where(e => e.CustCode == request.CustCode);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cPayments = cPayments.Where(e => e.BalanceAmount == 0);
                    else
                        cPayments = cPayments.Where(e => e.BalanceAmount > 0);
                }
            }

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var reportCount = await cPayments.CountAsync();
            cPayments = cPayments.Pagination(request.ReportIndex, request.ReportCount);

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            List<CustomerVoucherSummaryDto> summaryList = new();
            foreach (var item in await cPayments.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.CustCode,
                    VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                    DrAmount = item.InvoiceAmount,
                    BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                    CrAmount = item.PaidAmount,
                    Balance = item.BalanceAmount,
                    //CheckNumber = item.CheckNumber,
                    //CheckDate = item.Checkdate,
                    DocNum = item.DocNum,
                    //  PayCode = item.PayCode,
                    //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                });
            }
            summaryItemsList.Add(summaryList);

            return new()
            {
                List = new() { List = summaryItemsList, ReportCount = reportCount },
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = summaryItemsList.Sum(e => e.Sum(t => t.Balance))
            };

        }
    }

    #endregion



    #region GetCustomerInvoiceSummaryList

    public class GetCustomerInvoiceSummaryList : IRequest<CustomerInvoiceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerInvoiceSummaryListHandler : IRequestHandler<GetCustomerInvoiceSummaryList, CustomerInvoiceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerInvoiceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerInvoiceSummaryListDto> Handle(GetCustomerInvoiceSummaryList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var custInvoices = _context.TrnCustomerInvoices
                .Include(e => e.SndCustomerMaster)
                .Include(e => e.Invoice)
                .AsNoTracking();
            var custSites = _context.OprSites.AsNoTracking();

            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    custInvoices = custInvoices.Where(e => e.CustCode == request.CustCode);

            if (request.SiteCode.HasValue())
                custInvoices = custInvoices.Where(e => e.Invoice.SiteCode == request.SiteCode);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                custInvoices = custInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        custInvoices = custInvoices.Where(e => e.BalanceAmount == 0);
                    else
                        custInvoices = custInvoices.Where(e => e.BalanceAmount > 0);
                }
            }



            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            // var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var payments = _context.OpmCustomerPayments.AsNoTracking();
            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            List<CustomerVoucherSummaryDto> summaryList = new();
            foreach (var item in await custInvoices.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.CustCode,
                    InvoiceNumber = item.InvoiceNumber,
                    VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                    DrAmount = item.NetAmount,
                    CrAmount = item.PaidAmount,
                    // AppliedAmount = item.IsPaid && item.PaidAmount > 0 ? 0 : item.AppliedAmount,
                    AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount),
                    Balance = item.BalanceAmount,
                    NetBalance = item.NetAmount - item.PaidAmount,
                    BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                    //CheckNumber = item.CheckNumber,
                    //CheckDate = item.Checkdate,
                    DocNum = item.DocNum,
                    SiteName = item.Invoice != null && item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                     : String.Empty,
                    InvoiceDate = item.InvoiceDate,
                    ReferenceNumber = item.ReferenceNumber,
                    Trantype = item.Trantype
                    //  PayCode = item.PayCode,
                    //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                });
            }
            // summaryItemsList.Add(summaryList);

            return new()
            {

                Invoices = summaryList,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount),
                TotalAppliedAmount = summaryList.Sum(e => e.AppliedAmount),
                TotalBalance = summaryList.Sum(e => e.Balance),
                TotalNetBalance = summaryList.Sum(e => e.NetBalance),
            };
        }
    }

    #endregion


    #region GetCustomerInvoiceDetailList

    public class GetCustomerInvoiceDetailList : IRequest<CustomerInvoiceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerInvoiceDetailListHandler : IRequestHandler<GetCustomerInvoiceDetailList, CustomerInvoiceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerInvoiceDetailListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerInvoiceSummaryListDto> Handle(GetCustomerInvoiceDetailList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var custInvoices = _context.TrnCustomerInvoices
                .Include(e => e.SndCustomerMaster)
                .Include(e => e.Invoice)
                .AsNoTracking();
            var custSites = _context.OprSites.AsNoTracking();

            // if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                custInvoices = custInvoices.Where(e => e.CustCode == request.CustCode);

            if (request.SiteCode.HasValue())
                custInvoices = custInvoices.Where(e => e.Invoice.SiteCode == request.SiteCode);

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                custInvoices = custInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        custInvoices = custInvoices.Where(e => e.BalanceAmount == 0);
                    else
                        custInvoices = custInvoices.Where(e => e.BalanceAmount > 0);
                }
            }


            var reportCount = await custInvoices.CountAsync();
            custInvoices = custInvoices.Pagination(request.ReportIndex, request.ReportCount);

            var vendInvoiceList = (await custInvoices.ToListAsync()).GroupBy(e => new { e.CustCode });// e.SndVendorMaster.CustName });


            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var payments = _context.OpmCustomerPayments.AsNoTracking();

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            decimal? totalBalance = 0, totalDrAmount = 0, totalCrAmount = 0, totalAppliedAmount = 0;
            foreach (var vendInvoice in vendInvoiceList)
            {
                List<CustomerVoucherSummaryDto> summaryList = new();
                foreach (var item in vendInvoice)
                {
                    summaryList.Add(new()
                    {
                        VendCode = item.CustCode,
                        InvoiceNumber = item.InvoiceNumber,
                        VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                        DrAmount = item.Trantype == "Credit" ? (-1) * item.InvoiceAmount : item.InvoiceAmount,
                        CrAmount = item.PaidAmount,
                        //AppliedAmount = item.IsPaid && item.PaidAmount > 0 ? 0 : item.AppliedAmount,
                        AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount),
                        Balance = item.Trantype == "Credit" ? (-1) * item.BalanceAmount : item.BalanceAmount,
                        //NetBalance = item.NetAmount - item.PaidAmount,
                        BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                        //CheckNumber = item.CheckNumber,
                        //CheckDate = item.Checkdate,
                        DocNum = item.DocNum,

                        SiteName = item.Invoice != null && item.Invoice.SiteCode.HasValue() ? (isArab ? custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteArbName
                                                                                  : custSites.FirstOrDefault(site => site.SiteCode == item.Invoice.SiteCode).SiteName)
                                                     : String.Empty,
                        InvoiceDate = item.InvoiceDate,
                        ReferenceNumber = item.ReferenceNumber,
                        Trantype = item.Trantype
                        //  PayCode = item.PayCode,
                        //
                        //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                    });
                }

                totalDrAmount += summaryList.Sum(e => e.DrAmount);
                totalCrAmount += summaryList.Sum(e => e.CrAmount);
                totalBalance += summaryList.Sum(e => e.Balance);
                totalAppliedAmount += summaryList.Sum(e => e.AppliedAmount);


                summaryList.Add(new()
                {
                    Trantype = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    DrAmount = summaryList.Sum(e => e.DrAmount),
                    CrAmount = summaryList.Sum(e => e.CrAmount),
                    AppliedAmount = summaryList.Sum(e => e.AppliedAmount),
                    Balance = summaryList.Sum(e => e.Balance)
                });

                summaryItemsList.Add(summaryList);
            }
            // summaryItemsList.Add(summaryList);



            return new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,

                List = new() { List = summaryItemsList, ReportCount = reportCount },
                TotalDrAmount = totalDrAmount,
                TotalCrAmount = totalCrAmount,
                TotalAppliedAmount = totalAppliedAmount,
                TotalBalance = totalBalance
            };
        }
    }

    #endregion


    #region GetCustomerVoucherPaymentDetailsList

    public class GetCustomerVoucherPaymentDetailsList : IRequest<OpmCustomerPaymentDetailListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerVoucherPaymentDetailsListHandler : IRequestHandler<GetCustomerVoucherPaymentDetailsList, OpmCustomerPaymentDetailListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVoucherPaymentDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentDetailListDto> Handle(GetCustomerVoucherPaymentDetailsList request, CancellationToken cancellationToken)
        {
            var paymentStatements = _context.OpmCustomerPayments.
                Include(e => e.SndCustomerMaster)
                .Include(e => e.OpmPaymentHeader)
                .AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    paymentStatements = paymentStatements.Where(e => e.CustCode == request.CustCode);

            List<OpmCustomerPaymentDetailDto> summaryList1 = new();


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                paymentStatements = paymentStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            }


            var reportCount = await paymentStatements.CountAsync();
            paymentStatements = paymentStatements.Pagination(request.ReportIndex, request.ReportCount);

            var paymentList = (await paymentStatements.ToListAsync()).GroupBy(e => new { e.CustCode });// e.SndVendorMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);


            bool isArab = request.User.Culture.IsArab();

            List<List<OpmCustomerPaymentDetailDto>> summaryItemsList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;
            foreach (var statement in paymentList)
            {
                List<OpmCustomerPaymentDetailDto> summaryList = new();
                foreach (var item in statement)
                {
                    summaryList.Add(new()
                    {
                        InvoiceNumber = item.InvoiceNumber,
                        TranDate = item.OpmPaymentHeader?.TranDate,
                        VoucherNumber = item.OpmPaymentHeader?.PaymentNumber.ToString() ?? "",
                        CustCode = item.CustCode,
                        Name = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                        InvoiceDate = item.InvoiceDate,
                        DocNum = item.DocNum,
                        Amount = item.Amount,
                        CrAmount = item.CrAmount
                    });
                }

                // totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                summaryList.Add(new()
                {
                    DocNum = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    Amount = summaryList.GroupBy(e => new { e.InvoiceNumber, e.Amount }).Sum(e => e.Key.Amount),
                    CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount)
                });

                summaryItemsList.Add(summaryList);

            }

            OpmCustomerPaymentDetailListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                //TotalBalance = totalBalance
            };

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
            return summaryListDto;
        }
    }

    #endregion


    #region GetCustomerVoucherPaymentSummaryList

    public class GetCustomerVoucherPaymentSummaryList : IRequest<OpmCustomerPaymentHeaderSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerVoucherPaymentSummaryListHandler : IRequestHandler<GetCustomerVoucherPaymentSummaryList, OpmCustomerPaymentHeaderSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVoucherPaymentSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentHeaderSummaryListDto> Handle(GetCustomerVoucherPaymentSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.OpmCustPaymentHeaders.Include(e => e.SndCustomerMaster).AsNoTracking();
            // if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                cStatements = cStatements.Where(e => e.CustCode == request.CustCode);

            List<OpmCustomerPaymentHeaderSummaryDto> summaryList1 = new();


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);
            }

            var statements = await cStatements.ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);


            bool isArab = request.User.Culture.IsArab();


            List<OpmCustomerPaymentHeaderSummaryDto> summaryList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    CustCode = item.CustCode,
                    Name = (item.SndCustomerMaster is not null ? (isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName) : string.Empty),
                    TranDate = item.TranDate,
                    DocNum = item.DocNum,
                    Amount = item.Amount,
                    CrAmount = item.CrAmount
                });
                // totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);               

            }

            OpmCustomerPaymentHeaderSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                //TotalBalance = totalBalance
            };

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = summaryList;
            return summaryListDto;
        }
    }

    #endregion

    #endregion


    #region Vendor



    #region GetVendorBalanceSummaryList

    public class GetVendorBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorBalanceSummaryListHandler : IRequestHandler<GetVendorBalanceSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorBalanceSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.TrnVendorStatements.Include(e => e.SndVendorMaster)
                .Include(e => e.Invoice)
                .AsNoTracking();

            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

            bool isArabic = request.User.Culture.IsArab();

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                var cStatements1 = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });


                foreach (var item in await cStatements1.ToListAsync())
                {
                    summaryList1.Add(new()
                    {
                        VendCode = item.VendCode,

                        VendName = isArabic ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        DrAmount = item.DrAmount,
                        CrAmount = item.CrAmount,
                        Balance = item.CrAmount - item.DrAmount
                    });
                }

                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in await cStatements.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    InvoiceNumber = item.Invoice?.CreditNumber,
                    Remarks = item.Invoice?.Remarks,
                    Trantype = item.Trantype,
                    VendName = isArabic ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                    DrAmount = item.DrAmount,
                    CrAmount = item.CrAmount,
                    Balance = item.CrAmount - item.DrAmount
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion


    #region GetVendorBalanceDetailsList

    public class GetVendorBalanceDetailsList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        // public bool IsAllVendors { get; set; }
        public bool IsSummary { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorBalanceDetailsListHandler : IRequestHandler<GetVendorBalanceDetailsList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorBalanceDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorBalanceDetailsList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.TrnVendorStatements.Include(e => e.SndVendorMaster)
                                .Include(e => e.Invoice)
                                .AsNoTracking();
            // if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

            List<CustomerBalanceSummaryDto> summaryList1 = new();

            //if (request.DateFrom is not null && request.DateTo is not null)
            //{
            //    var cStatements1 = cStatements.Where(e => e.TranDate < request.DateFrom);
            //    var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.CustCode, e.SndVendorMaster.CustName });


            //    foreach (var statement in statements1)
            //    {
            //        foreach (var item in collection)
            //        {

            //            summaryList1.Add(new()
            //            {
            //                VendCode = item.Key.CustCode,
            //                VendName = item.Key.CustName,
            //                DrAmount = item.Sum(e => e.DrAmount),
            //                CrAmount = item.Sum(e => e.CrAmount),
            //                Balance = item.Sum(e => e.DrAmount) - item.Sum(e => e.CrAmount)
            //            });
            //        }
            //    }

            //    cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);

            //}



            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo); //Value.AddDays(1)
            }


            var reportCount = await cStatements.CountAsync();
            cStatements = cStatements.Pagination(request.ReportIndex, request.ReportCount);

            var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.VendCode });// e.SndVendorMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

            bool isArab = request.User.Culture.IsArab();

            List<List<CustomerBalanceSummaryDto>> summaryItemsList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;
            foreach (var statement in statements)
            {
                List<CustomerBalanceSummaryDto> summaryList = new();
                foreach (var item in statement)
                {
                    summaryList.Add(new()
                    {
                        InvoiceId = item.InvoiceId,
                        InvoiceNumber = item.Invoice?.CreditNumber,
                        Remarks = item.Invoice?.Remarks,
                        Trantype = item.Trantype,
                        VendCode = item.VendCode,
                        VendName = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        DrAmount = item.DrAmount,
                        CrAmount = item.CrAmount,
                        Balance = item.DrAmount - item.CrAmount
                    });
                }

                totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                summaryList.Add(new()
                {
                    VendName = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    DrAmount = summaryList.GroupBy(e => new { e.InvoiceId, e.DrAmount }).Sum(e => e.Key.DrAmount),
                    CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount),
                    Balance = summaryList.Sum(e => e.Balance)
                });
                summaryItemsList.Add(summaryList);

            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                TotalBalance = totalBalance
            };

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
            return summaryListDto;
        }
    }

    #endregion


    #region GetVendorAllBalanceSummaryList

    public class GetVendorAllBalanceSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }

    }
    public class GetVendorAllBalanceSummaryListHandler : IRequestHandler<GetVendorAllBalanceSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorAllBalanceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorAllBalanceSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.TrnVendorStatements.Include(e => e.SndVendorMaster).AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

            bool isArabic = request.User.Culture.IsArab();

            List<CustomerBalanceSummaryDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                var cStatements1 = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) < request.DateFrom);
                //var statements1 = (await cStatements1.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });


                foreach (var item in await cStatements1.ToListAsync())
                {
                    summaryList1.Add(new()
                    {
                        VendCode = item.VendCode,
                        VendName = isArabic ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        DrAmount = item.DrAmount,
                        CrAmount = item.CrAmount,
                        Balance = item.CrAmount - item.DrAmount
                    });
                }

                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            if (request.Type.HasValue())
            {
                var vendInvoices = _context.OpmVendorPayments.Where(e => e.VendCode == request.CustCode).Select(e => e.InvoiceNumber);

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cStatements = cStatements.Where(e => vendInvoices.Any(eid => eid == e.TranNumber) || e.Trantype == "Payment");
                    else
                        cStatements = cStatements.Where(e => !vendInvoices.Any(eid => eid == e.TranNumber) && e.Trantype == "Invoice");
                }
            }

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in await cStatements.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    VendName = isArabic ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                    DrAmount = item.DrAmount,
                    CrAmount = item.CrAmount,
                    Balance = item.CrAmount - item.DrAmount
                });
            }

            CustomerBalanceSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                TotalBalance = summaryList.Sum(e => e.Balance) + summaryList1.Sum(e => e.Balance)
            };

            summaryList.AddRange(summaryList1);
            summaryListDto.List = summaryList;
            return summaryListDto;

        }
    }

    #endregion


    #region GetVendorPaymentSummaryList

    public class GetVendorPaymentSummaryList : IRequest<CustomerBalanceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorPaymentSummaryListHandler : IRequestHandler<GetVendorPaymentSummaryList, CustomerBalanceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorPaymentSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerBalanceSummaryListDto> Handle(GetVendorPaymentSummaryList request, CancellationToken cancellationToken)
        {
            var vPayments = _context.TrnVendorPayments.Include(e => e.SndVendorMaster).AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    vPayments = vPayments.Where(e => e.VendCode == request.CustCode);

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                vPayments = vPayments.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            }

            //var statements = (await cStatements.ToListAsync()).GroupBy(e => e.CustCode);
            var statements = (await vPayments.ToListAsync()).GroupBy(e => new { e.VendCode, e.SndVendorMaster.VendName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            List<CustomerBalanceSummaryDto> summaryList = new();
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    VendCode = item.Key.VendCode,
                    VendName = item.Key.VendName,
                    DrAmount = item.Sum(e => e.Amount),
                });
            }

            return new()
            {
                List = summaryList,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = summaryList.Sum(e => e.DrAmount)
            };

        }
    }

    #endregion


    #region GetVendorVoucherSummaryList

    public class GetVendorVoucherSummaryList : IRequest<CustomerVoucherSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllBranches { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorVoucherSummaryListHandler : IRequestHandler<GetVendorVoucherSummaryList, CustomerVoucherSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorVoucherSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerVoucherSummaryListDto> Handle(GetVendorVoucherSummaryList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var cPayments = _context.TrnVendorInvoices.Include(e => e.SndVendorMaster).AsNoTracking();

            //if (!request.IsAllBranches)
            //    cPayments = cPayments.Where(e => e.BranchCode == request.User.BranchCode);

            if (request.CustCode.HasValue())
                cPayments = cPayments.Where(e => e.VendCode == request.CustCode);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }


            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cPayments = cPayments.Where(e => e.BalanceAmount == 0);
                    else
                        cPayments = cPayments.Where(e => e.BalanceAmount > 0);
                }
            }


            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var reportCount = await cPayments.CountAsync();
            cPayments = cPayments.Pagination(request.ReportIndex, request.ReportCount);

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            List<CustomerVoucherSummaryDto> summaryList = new();
            foreach (var item in await cPayments.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    VendName = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                    DrAmount = item.InvoiceAmount,
                    BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                    CrAmount = item.PaidAmount,
                    Balance = item.BalanceAmount,
                    //CheckNumber = item.CheckNumber,
                    //CheckDate = item.Checkdate,
                    DocNum = item.DocNum,
                    //PayCode = item.PayCode,
                    //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                });
            }
            summaryItemsList.Add(summaryList);

            return new()
            {
                List = new() { List = summaryItemsList, ReportCount = reportCount },
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = summaryItemsList.Sum(e => e.Sum(t => t.Balance))
            };

        }
    }

    #endregion


    #region GetVendorInvoiceSummaryList

    public class GetVendorInvoiceSummaryList : IRequest<CustomerInvoiceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorInvoiceSummaryListHandler : IRequestHandler<GetVendorInvoiceSummaryList, CustomerInvoiceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorInvoiceSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerInvoiceSummaryListDto> Handle(GetVendorInvoiceSummaryList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var cPayments = _context.TrnVendorInvoices.Include(e => e.SndVendorMaster).AsNoTracking();
            // if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                cPayments = cPayments.Where(e => e.VendCode == request.CustCode);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cPayments = cPayments.Where(e => e.BalanceAmount == 0);
                    else
                        cPayments = cPayments.Where(e => e.BalanceAmount > 0);
                }
            }



            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var payments = _context.OpmVendorPayments.AsNoTracking();

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            List<CustomerVoucherSummaryDto> summaryList = new();
            foreach (var item in await cPayments.ToListAsync())
            {
                summaryList.Add(new()
                {
                    VendCode = item.VendCode,
                    InvoiceNumber = item.InvoiceNumber,
                    VendName = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                    DrAmount = item.NetAmount,
                    CrAmount = item.PaidAmount,
                    //AppliedAmount = item.IsPaid && item.PaidAmount > 0 ? 0 : item.AppliedAmount,
                    AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount),
                    Balance = item.BalanceAmount,
                    NetBalance = item.NetAmount - item.PaidAmount,
                    BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                    //CheckNumber = item.CheckNumber,
                    //CheckDate = item.Checkdate,
                    DocNum = item.DocNum,
                    InvoiceDate = item.InvoiceDate,
                    ReferenceNumber = item.ReferenceNumber,
                    Trantype = item.Trantype
                    //  PayCode = item.PayCode,
                    //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                });
            }
            // summaryItemsList.Add(summaryList);

            return new()
            {

                Invoices = summaryList,
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalDrAmount = summaryList.Sum(e => e.DrAmount),
                TotalCrAmount = summaryList.Sum(e => e.CrAmount),
                TotalAppliedAmount = summaryList.Sum(e => e.AppliedAmount),
                TotalBalance = summaryList.Sum(e => e.Balance),
                TotalNetBalance = summaryList.Sum(e => e.NetBalance),
            };
        }
    }

    #endregion

    #region GetVendorInvoiceDetailList

    public class GetVendorInvoiceDetailList : IRequest<CustomerInvoiceSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorInvoiceDetailListHandler : IRequestHandler<GetVendorInvoiceDetailList, CustomerInvoiceSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorInvoiceDetailListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerInvoiceSummaryListDto> Handle(GetVendorInvoiceDetailList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var vendInvoices = _context.TrnVendorInvoices.Include(e => e.SndVendorMaster).AsNoTracking();
            // if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                vendInvoices = vendInvoices.Where(e => e.VendCode == request.CustCode);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                vendInvoices = vendInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            if (request.Type.HasValue())
            {

                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        vendInvoices = vendInvoices.Where(e => e.BalanceAmount == 0);
                    else
                        vendInvoices = vendInvoices.Where(e => e.BalanceAmount > 0);
                }
            }


            var reportCount = await vendInvoices.CountAsync();
            vendInvoices = vendInvoices.Pagination(request.ReportIndex, request.ReportCount);

            var vendInvoiceList = (await vendInvoices.ToListAsync()).GroupBy(e => new { e.VendCode });// e.SndVendorMaster.CustName });


            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            //var payCodes = _context.FinAccountlPaycodes.AsNoTracking();


            var payments = _context.OpmVendorPayments.AsNoTracking();

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            decimal? totalBalance = 0, totalDrAmount = 0, totalCrAmount = 0, totalAppliedAmount = 0;
            foreach (var vendInvoice in vendInvoiceList)
            {
                List<CustomerVoucherSummaryDto> summaryList = new();
                foreach (var item in vendInvoice)
                {
                    summaryList.Add(new()
                    {
                        VendCode = item.VendCode,
                        InvoiceNumber = item.InvoiceNumber,
                        VendName = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        DrAmount = item.InvoiceAmount,
                        CrAmount = item.PaidAmount,
                        //AppliedAmount = item.IsPaid && item.PaidAmount > 0 ? 0 : item.AppliedAmount,
                        AppliedAmount = payments.Where(e => e.InvoiceId == item.InvoiceId && !e.IsPaid).Sum(e => e.AppliedAmount),
                        Balance = item.BalanceAmount,
                        //NetBalance = item.NetAmount - item.PaidAmount,
                        BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                        //CheckNumber = item.CheckNumber,
                        //CheckDate = item.Checkdate,
                        DocNum = item.DocNum,
                        InvoiceDate = item.InvoiceDate,
                        ReferenceNumber = item.ReferenceNumber,
                        Trantype = item.Trantype
                        //  PayCode = item.PayCode,
                        //
                        //PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.PayCode).FinPayAcIntgrAC
                    });
                }

                totalDrAmount += summaryList.Sum(e => e.DrAmount);
                totalCrAmount += summaryList.Sum(e => e.CrAmount);
                totalBalance += summaryList.Sum(e => e.Balance);
                totalAppliedAmount += summaryList.Sum(e => e.AppliedAmount);


                summaryList.Add(new()
                {
                    Trantype = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    DrAmount = summaryList.Sum(e => e.DrAmount),
                    CrAmount = summaryList.Sum(e => e.CrAmount),
                    AppliedAmount = summaryList.Sum(e => e.AppliedAmount),
                    Balance = summaryList.Sum(e => e.Balance)
                });

                summaryItemsList.Add(summaryList);
            }
            // summaryItemsList.Add(summaryList);



            return new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,

                List = new() { List = summaryItemsList, ReportCount = reportCount },
                TotalDrAmount = totalDrAmount,
                TotalCrAmount = totalCrAmount,
                TotalAppliedAmount = totalAppliedAmount,
                TotalBalance = totalBalance
            };
        }
    }

    #endregion

    #region GetVendorVoucherPaymentDetailsList

    public class GetVendorVoucherPaymentDetailsList : IRequest<OpmCustomerPaymentDetailListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorVoucherPaymentDetailsListHandler : IRequestHandler<GetVendorVoucherPaymentDetailsList, OpmCustomerPaymentDetailListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorVoucherPaymentDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentDetailListDto> Handle(GetVendorVoucherPaymentDetailsList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.OpmVendorPayments.Include(e => e.SndVendorMaster).AsNoTracking();
            if (!request.IsAllVendors)
                if (request.CustCode.HasValue())
                    cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

            List<OpmCustomerPaymentDetailDto> summaryList1 = new();


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cStatements = cStatements.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            }


            var reportCount = await cStatements.CountAsync();
            cStatements = cStatements.Pagination(request.ReportIndex, request.ReportCount);

            var statements = (await cStatements.ToListAsync()).GroupBy(e => new { e.VendCode });// e.SndVendorMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);


            bool isArab = request.User.Culture.IsArab();

            List<List<OpmCustomerPaymentDetailDto>> summaryItemsList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;
            foreach (var statement in statements)
            {
                List<OpmCustomerPaymentDetailDto> summaryList = new();
                foreach (var item in statement)
                {
                    summaryList.Add(new()
                    {
                        InvoiceNumber = item.InvoiceNumber,
                        CustCode = item.VendCode,
                        Name = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        InvoiceDate = item.InvoiceDate,
                        DocNum = item.DocNum,
                        Amount = item.Amount,
                        CrAmount = item.CrAmount
                    });
                }

                // totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                summaryList.Add(new()
                {
                    DocNum = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    Amount = summaryList.GroupBy(e => new { e.InvoiceNumber, e.Amount }).Sum(e => e.Key.Amount),
                    CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount)
                });

                summaryItemsList.Add(summaryList);

            }

            OpmCustomerPaymentDetailListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                //TotalBalance = totalBalance
            };

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = new() { List = summaryItemsList, ReportCount = reportCount };
            return summaryListDto;
        }
    }

    #endregion

    #region GetVendorVoucherPaymentSummaryList

    public class GetVendorVoucherPaymentSummaryList : IRequest<OpmCustomerPaymentHeaderSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllVendors { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetVendorVoucherPaymentSummaryListHandler : IRequestHandler<GetVendorVoucherPaymentSummaryList, OpmCustomerPaymentHeaderSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorVoucherPaymentSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpmCustomerPaymentHeaderSummaryListDto> Handle(GetVendorVoucherPaymentSummaryList request, CancellationToken cancellationToken)
        {
            var cStatements = _context.OpmVendPaymentHeaders.Include(e => e.SndVendorMaster).AsNoTracking();
            //if (!request.IsAllVendors)
            if (request.CustCode.HasValue())
                cStatements = cStatements.Where(e => e.VendCode == request.CustCode);

            List<OpmCustomerPaymentHeaderSummaryDto> summaryList1 = new();


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cStatements = cStatements.Where(e => EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.TranDate.Value.Year, e.TranDate.Value.Month, e.TranDate.Value.Day) <= request.DateTo);
            }

            var statements = await cStatements.ToListAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);


            bool isArab = request.User.Culture.IsArab();


            List<OpmCustomerPaymentHeaderSummaryDto> summaryList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;
            foreach (var item in statements)
            {
                summaryList.Add(new()
                {
                    CustCode = item.VendCode,
                    Name = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                    TranDate = item.TranDate,
                    DocNum = item.DocNum,
                    Amount = item.Amount,
                    CrAmount = item.CrAmount
                });
                // totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);               

            }

            OpmCustomerPaymentHeaderSummaryListDto summaryListDto = new()
            {
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = summaryList.Sum(e => e.DrAmount) + summaryList1.Sum(e => e.DrAmount),
                //TotalCrAmount = summaryList.Sum(e => e.CrAmount) + summaryList1.Sum(e => e.CrAmount),
                //TotalBalance = totalBalance
            };

            //summaryListDto.AddRange(summaryList1);
            summaryListDto.ListItems = summaryList;
            return summaryListDto;
        }
    }

    #endregion


    #endregion

    #region OPM Customer

    #region GetOpmCustomerSummaryList

    public class GetOpmCustomerSummaryList : IRequest<CustomerVoucherSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllCustomers { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetOpmCustomerSummaryListHandler : IRequestHandler<GetOpmCustomerSummaryList, CustomerVoucherSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmCustomerSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerVoucherSummaryListDto> Handle(GetOpmCustomerSummaryList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var cPayments = _context.OpmCustomerPayments
                .Include(e => e.SndCustomerMaster)
                .Include(e => e.Invoice)
                .Include(e => e.OpmPaymentHeader)
                .AsNoTracking();

            if (request.Type.HasValue())
            {
                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cPayments = cPayments.Where(e => e.IsPaid);
                    else
                        cPayments = cPayments.Where(e => !e.IsPaid);
                }
            }

            //if (request.IsAllCustomers)
            if (request.CustCode.HasValue() && !request.IsAllCustomers)
                cPayments = cPayments.Where(e => e.CustCode == request.CustCode);



            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            }

            var reportCount = await cPayments.CountAsync();
            cPayments = cPayments.Pagination(request.ReportIndex, request.ReportCount);

            var cPaymentsStatements = (await cPayments.OrderByDescending(e => e.Id).ToListAsync()).GroupBy(e => new { e.CustCode });//, CustName = isArab ? e.SndVendorMaster.CustArbName : e.SndVendorMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;

            foreach (var cStatement in cPaymentsStatements)
            {
                List<CustomerVoucherSummaryDto> summaryList = new();
                foreach (var item in cStatement)
                {
                    summaryList.Add(new()
                    {
                        VendCode = item.CustCode,
                        VendName = isArab ? item.SndCustomerMaster.CustArbName : item.SndCustomerMaster.CustName,
                        DrAmount = item.Amount,
                        VoucherNumber = item.OpmPaymentHeader.PaymentNumber.ToString(),
                        TranDate = item.OpmPaymentHeader.TranDate,
                        CrAmount = item.CrAmount,
                        Balance = item.BalanceAmount,
                        InvoiceNumber = item.Invoice.InvoiceNumber,
                        Trantype = item.Invoice.IsCreditConverted ? "Credit" : "Invoice",
                        Remarks = item.Remarks,
                        BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                        CheckNumber = item.OpmPaymentHeader.CheckNumber,
                        CheckDate = item.OpmPaymentHeader.Checkdate,
                        DocNum = item.OpmPaymentHeader.DocNum,
                        PayCode = item.OpmPaymentHeader.PayCode,
                        PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.OpmPaymentHeader.PayCode)?.FinPayAcIntgrAC
                    });
                }

                totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                summaryList.Add(new()
                {
                    CheckNumber = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    DrAmount = summaryList.Select(e => e.DrAmount).Distinct().Sum(amount => amount),
                    CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount),
                    Balance = summaryList.Sum(e => e.Balance)
                });
                summaryItemsList.Add(summaryList);

            }

            return new()
            {
                List = new() { List = summaryItemsList, ReportCount = reportCount },
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                //TotalDrAmount = totalDrAmount,
                //TotalCrAmount = totalCrAmount,
                TotalBalance = totalBalance//summaryItemsList.Last().Sum(e => e.DrAmount)
            };

        }
    }

    #endregion

    #endregion

    #region OPM Vendor


    #region GetOpmVendorSummaryList

    public class GetOpmVendorSummaryList : IRequest<CustomerVoucherSummaryListDto>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsAllCustomers { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetOpmVendorSummaryListHandler : IRequestHandler<GetOpmVendorSummaryList, CustomerVoucherSummaryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpmVendorSummaryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerVoucherSummaryListDto> Handle(GetOpmVendorSummaryList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var cPayments = _context.OpmVendorPayments
                .Include(e => e.SndVendorMaster)
                 .Include(e => e.Invoice)
                .Include(e => e.OpmPaymentHeader)
                .AsNoTracking();

            if (request.Type.HasValue())
            {
                if (request.Type != "All")
                {
                    if (request.Type == "Paid")
                        cPayments = cPayments.Where(e => e.IsPaid);
                    else
                        cPayments = cPayments.Where(e => !e.IsPaid);
                }
            }

            //if (request.IsAllCustomers)
            if (request.CustCode.HasValue() && !request.IsAllCustomers)
                cPayments = cPayments.Where(e => e.VendCode == request.CustCode);



            if (request.DateFrom is not null && request.DateTo is not null)
            {
                cPayments = cPayments.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            }


            var reportCount = await cPayments.CountAsync();
            cPayments = cPayments.Pagination(request.ReportIndex, request.ReportCount);

            var cStatements = (await cPayments.OrderByDescending(e => e.Id).ToListAsync()).GroupBy(e => new { e.VendCode });//, CustName = isArab ? e.SndVendorMaster.CustArbName : e.SndVendorMaster.CustName });

            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var payCodes = _context.FinAccountlPaycodes.AsNoTracking();

            List<List<CustomerVoucherSummaryDto>> summaryItemsList = new();
            decimal? totalBalance = 0;//, totalDrAmount = 0, totalCrAmount = 0;

            foreach (var cStatement in cStatements)
            {
                List<CustomerVoucherSummaryDto> summaryList = new();
                foreach (var item in cStatement)
                {
                    summaryList.Add(new()
                    {
                        VendCode = item.VendCode,
                        VendName = isArab ? item.SndVendorMaster.VendArbName : item.SndVendorMaster.VendName,
                        DrAmount = item.Amount,
                        CrAmount = item.CrAmount,
                        Balance = item.BalanceAmount,
                        InvoiceNumber = item.Invoice.CreditNumber,
                        Trantype = item.Invoice.IsCreditConverted ? "Credit" : "Invoice",
                        Remarks = item.Remarks,
                        BranchName = _context.CompanyBranches.FirstOrDefault(e => e.BranchCode == item.BranchCode).BranchName,
                        CheckNumber = item.OpmPaymentHeader.CheckNumber,
                        CheckDate = item.OpmPaymentHeader.Checkdate,
                        DocNum = item.OpmPaymentHeader.DocNum,
                        PayCode = item.OpmPaymentHeader.PayCode,
                        PayAcCode = payCodes.FirstOrDefault(e => e.FinPayCode == item.OpmPaymentHeader.PayCode).FinPayAcIntgrAC
                    });
                }

                totalBalance += summaryList.Sum(e => e.Balance);
                //totalDrAmount += summaryList.Sum(e => e.DrAmount);
                //totalCrAmount += summaryList.Sum(e => e.CrAmount);

                summaryList.Add(new()
                {
                    CheckNumber = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
                    DrAmount = summaryList.Select(e => e.DrAmount).Distinct().Sum(amount => amount),
                    CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount),
                    Balance = summaryList.Sum(e => e.Balance)
                });
                summaryItemsList.Add(summaryList);

            }

            return new()
            {
                List = new() { List = summaryItemsList, ReportCount = reportCount },
                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = summaryItemsList.Sum(e => e.Sum(t => t.DrAmount))
            };

        }
    }

    #endregion

    #endregion

    #region Get Profit And Loss And Balance List

    public class GetProfitAndLossList : IRequest<ProfitAndLossListDto>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
        //public string CustCode { get; set; }
        //public bool IsAllCustomers { get; set; }
    }
    public class ProfitAndLossListHandler : IRequestHandler<GetProfitAndLossList, ProfitAndLossListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProfitAndLossListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProfitAndLossListDto> Handle(GetProfitAndLossList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();

            var financeSetup = await _context.FinSysFinanialSetups.FirstOrDefaultAsync();

            //DateTime dateTo = request.DateFrom.AddMonths(12);
            DateTime dateFrom = financeSetup.FYOpenDate;
            DateTime dateTo = financeSetup.FYClosingDate;

            // dateTo.AddDays(DateTime.DaysInMonth(dateTo.Year, dateTo.Month));

            var mainAccounts = _context.FinMainAccounts.Select(e => new CustomSelectListItem { Value = e.FinAcCode, Text = e.FinAcName, TextTwo = e.Fintype.ToUpper() });
            var ledgerItems = _context.AccountsLedgers.AsNoTracking()
                .Where(e => e.PostDate >= dateFrom && e.PostDate <= dateTo);


            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //Opening Balance calculating
                ledgerItems = ledgerItems.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) <= request.DateTo);
            }

            var cPayments = ledgerItems.Select(e => new { e.Id, e.AcCode, e.DrAmount, e.CrAmount });

            //if (request.Type.HasValue())
            //{
            //    if (request.Type != "All")
            //    {
            //        if (request.Type == "Paid")
            //            cPayments = cPayments.Where(e => e.IsPaid);
            //        else
            //            cPayments = cPayments.Where(e => !e.IsPaid);
            //    }
            //}

            ////if (request.IsAllCustomers)
            //if (request.CustCode.HasValue() && !request.IsAllCustomers)
            //    cPayments = cPayments.Where(e => e.VendCode == request.CustCode);



            //if (request.DateFrom is not null && request.DateTo is not null)
            //{
            //    cPayments = cPayments.Where(e => e.TranDate >= request.DateFrom && e.TranDate <= request.DateTo);
            //}

            var cStatements = (await cPayments.OrderBy(e => e.AcCode).ToListAsync()).GroupBy(e => e.AcCode);//, CustName = isArab ? e.SndVendorMaster.CustArbName : e.SndVendorMaster.CustName });



            var list = cStatements.Select(e => new ProfitAndLossItemDto
            {
                AcCode = e.Key,

                DrAmount = e.AsQueryable().Sum(d => d.DrAmount),
                CrAmount = e.AsQueryable().Sum(d => d.CrAmount),

                DrAmount_Bal = e.AsQueryable().Sum(d => d.DrAmount * 0),
                CrAmount_Bal = e.AsQueryable().Sum(d => d.CrAmount * 0),

            }).ToList();

            List<ProfitAndLossItemDto> plItems = new();

            foreach (var item in list)
            {
                item.Level = (await mainAccounts.FirstOrDefaultAsync(e => e.Value == item.AcCode.ToString())).TextTwo;
                item.FinAcName = (await mainAccounts.FirstOrDefaultAsync(e => e.Value == item.AcCode)).Text;

                if (item.DrAmount > item.CrAmount)
                    item.DrAmount_Bal = (item.DrAmount - item.CrAmount);

                else if (item.CrAmount > item.DrAmount)
                    item.CrAmount_Bal = (item.CrAmount - item.DrAmount);

                plItems.Add(item);
            }

            foreach (var item in plItems)
            {
                if (item.CrAmount_Bal > 0 && item.Level == "EXPENSE")
                    item.Level = "INCOME";
                else if (item.DrAmount_Bal > 0 && item.Level == "INCOME")
                    item.Level = "EXPENSE";

                else if (item.CrAmount_Bal > 0 && item.Level == "ASSET")
                    item.Level = "LIABILITY";
                else if (item.DrAmount_Bal > 0 && item.Level == "LIABILITY")
                    item.Level = "ASSET";
            }

            decimal? totalProfitLoss = 0;

            if (request.Type == "PL")
                plItems = plItems.Where(e => e.Level == "EXPENSE" || e.Level == "INCOME").ToList();
            if (request.Type == "BS")
            {
                var profitLoss = plItems.Where(e => e.Level == "EXPENSE" || e.Level == "INCOME").ToList();
                totalProfitLoss = profitLoss.Sum(e => e.CrAmount_Bal) - profitLoss.Sum(e => e.DrAmount_Bal);

                plItems = plItems.Where(e => e.Level == "ASSET" || e.Level == "LIABILITY").ToList();
            }
            decimal? totalDrAmount = plItems.Sum(e => e.DrAmount_Bal), totalCrAmount = plItems.Sum(e => e.CrAmount_Bal), totalBalance = totalCrAmount - totalDrAmount;



            var profLoss = new ProfitAndLossListDto() { List = plItems.ToList(), TotalProfitLossAmount = totalProfitLoss, TotalCrAmount = totalCrAmount, TotalDrAmount = totalDrAmount, TotalBalance = totalBalance };

            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
              .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            if (company is not null)
            {
                profLoss.Company = new()
                {
                    CompanyName = company.CompanyName,
                    CompanyAddress = company.CompanyAddress,
                    Phone = company.Phone,
                    LogoURL = company.LogoURL,
                    BranchName = branch.BranchName,
                    //ledger.Fax = company.;
                    //ledger.PoBox = company.;
                };
            }

            return profLoss;



            //List<List<ProfitAndLossItemDto>> summaryItemsList = new();

            //foreach (var cStatement in cStatements)
            //{
            //    List<ProfitAndLossItemDto> summaryList = new();
            //    foreach (var item in cStatement)
            //    {
            //        summaryList.Add(new()
            //        {
            //            AcCode = item.AcCode,
            //            FinAcName = mainAccounts.FirstOrDefault(e => e.Value == item.AcCode).Text,
            //            DrAmount = item.DrAmount,
            //            CrAmount = item.CrAmount,
            //            DrAmount_Bal = item.DrAmount * 0,
            //            CrAmount_Bal = item.CrAmount * 0
            //        });
            //    }

            //    //totalBalance += summaryList.Sum(e => e.Balance);
            //    totalDrAmount += summaryList.Sum(e => e.DrAmount);
            //    totalCrAmount += summaryList.Sum(e => e.CrAmount);

            //    summaryList.Add(new()
            //    {
            //        CheckNumber = request.User.Culture.IsArab() ? "مجموع :" : "Total :",
            //        DrAmount = summaryList.Sum(e => e.DrAmount),
            //        CrAmount = summaryList.Sum(e => e.CrAmount >= 0 ? e.CrAmount : (-1) * e.CrAmount),
            //        Balance = summaryList.Sum(e => e.Balance)
            //    });

            //    summaryItemsList.Add(summaryList);

            //}

            //return new()
            //{
            //    List = summaryItemsList,
            //    ComapnyName = company.CompanyName,
            //    LogoURL = company.LogoURL,
            //    BranchName = companyBranch.BranchName,
            //    Address = company.CompanyAddress,
            //    TotalBalance = summaryItemsList.Sum(e => e.Sum(t => t.DrAmount))
            //};



        }
    }

    #endregion


    #region TaxReporting Pringting
    public class GetTaxReportingPrintList : IRequest<TaxReportingPrintListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
        //public bool IsAllCustomers { get; set; }
    }
    public class GetTaxReportingPrintListHandler : IRequestHandler<GetTaxReportingPrintList, TaxReportingPrintListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTaxReportingPrintListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TaxReportingPrintListDto> Handle(GetTaxReportingPrintList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();

            var custInvoices = _context.TranInvoices.AsNoTracking();
            var venInvoices = _context.TranVenInvoices.AsNoTracking();

            var customers = _context.OprCustomers.AsNoTracking().Select(e => new { e.CustCode, e.CustArbName, e.CustName, e.Id });
            var vendors = _context.VendorMasters.AsNoTracking().Select(e => new { e.VendCode, e.VendArbName, e.VendName, e.Id });

            string branchName = string.Empty;

            if (request.BranchCode.HasValue())
            {
                var cBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode);
                branchName = cBranch.BranchName;

                custInvoices = custInvoices.Where(e => e.BranchCode == request.BranchCode);
                venInvoices = venInvoices.Where(e => e.BranchCode == request.BranchCode);
            }

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //Opening Balance calculating
                custInvoices = custInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
                venInvoices = venInvoices.Where(e => e.InvoiceDate >= request.DateFrom && e.InvoiceDate <= request.DateTo);
            }

            List<TaxReportingPrintDto> list = new();

            var cReports = await custInvoices.OrderByDescending(e => e.InvoiceDate).Select(e => new TaxReportingPrintDto
            {
                TranNumber = e.InvoiceNumber,
                TaxIdNumber = e.TaxIdNumber,
                Code = customers.FirstOrDefault(c => c.Id == e.CustomerId).CustCode,
                TaxCode = string.Empty,
                Date = e.InvoiceDate,
                Name = isArab ? customers.FirstOrDefault(c => c.Id == e.CustomerId).CustArbName : customers.FirstOrDefault(c => c.Id == e.CustomerId).CustName,
                Source = "Sales",
                Type = "Output",
                InputTaxAmount = 0,
                OutputTaxAmount = e.TaxAmount,
                TotalAmount = e.TotalAmount

            }).ToListAsync();

            var vReports = await venInvoices.OrderByDescending(e => e.InvoiceDate).Select(e => new TaxReportingPrintDto
            {
                TranNumber = e.CreditNumber,
                TaxIdNumber = e.TaxIdNumber,
                Code = vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendCode,
                TaxCode = string.Empty,
                Date = e.InvoiceDate,
                Name = isArab ? vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendArbName : vendors.FirstOrDefault(c => c.Id == e.CustomerId).VendName,
                Source = "Purchase",
                Type = "Input",
                InputTaxAmount = e.TaxAmount,
                OutputTaxAmount = 0,
                TotalAmount = e.TotalAmount

            }).ToListAsync();

            list.AddRange(vReports);
            list.AddRange(cReports);

            TaxReportingPrintListDto taxReport = new()
            {
                List = list,
                TotalOutputTaxAomunt = cReports.Sum(e => e.OutputTaxAmount),
                TotalInputTaxAomunt = vReports.Sum(e => e.InputTaxAmount),

                TotalPurchaseAmount = vReports.Sum(e => e.TotalAmount),
                TotalSaleAmount = cReports.Sum(e => e.TotalAmount),
            };
            taxReport.TotalTax = taxReport.TotalOutputTaxAomunt - taxReport.TotalInputTaxAomunt;
            taxReport.TotalAmount = taxReport.TotalSaleAmount - taxReport.TotalPurchaseAmount;


            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
             .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            if (company is not null)
            {
                taxReport.Company = new()
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

            return taxReport;

        }
    }

    #endregion


    #region CustomerRevenueAnalysis Printing
    public class GetCustomerRevenueAnalysis : IRequest<CustomerRevenueAnalysisListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BranchCode { get; set; }
        public string CustCode { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerRevenueAnalysisHandler : IRequestHandler<GetCustomerRevenueAnalysis, CustomerRevenueAnalysisListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerRevenueAnalysisHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerRevenueAnalysisListDto> Handle(GetCustomerRevenueAnalysis request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var costallocations = await _context.CostAllocationSetups.Select(e => new { e.Id, e.CostType }).FirstOrDefaultAsync(e => e.CostType == "Customer");

            var customers = _context.OprCustomers.Select(e => new { e.CustCode, Name = isArab ? e.CustArbName : e.CustName });

            var jvItems = _context.JournalVoucherItems.Where(e => e.CostAllocation == costallocations.Id).Select(e => new
            {
                e.DrAmount,
                e.CrAmount,
                e.FinAcCode,
                e.JournalVoucher.JvDate,
                e.BranchCode,
                e.CostSegCode,
                e.Batch
            });

            string branchName = string.Empty;

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                jvItems = jvItems.Where(e => EF.Functions.DateFromParts(e.JvDate.Value.Year, e.JvDate.Value.Month, e.JvDate.Value.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.JvDate.Value.Year, e.JvDate.Value.Month, e.JvDate.Value.Day) <= request.DateTo);
            }

            if (request.CustCode.HasValue())
            {
                jvItems = jvItems.Where(e => e.CostSegCode == request.CustCode);
            }

            if (request.BranchCode.HasValue())
            {
                var cBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode);
                branchName = cBranch.BranchName;

                jvItems = jvItems.Where(e => e.BranchCode == request.BranchCode);
            }

            var accounts = _context.FinMainAccounts.Where(e => e.FinIsRevenue).Select(e => new { e.FinAcCode, e.FinIsRevenuetype });

            var jvList = from jv in jvItems
                         join ac in accounts
                         on jv.FinAcCode equals ac.FinAcCode
                         select new
                         {
                             jv.DrAmount,
                             jv.CrAmount,
                             jv.FinAcCode,
                             jv.JvDate,
                             jv.BranchCode,
                             jv.CostSegCode,
                             //ac.FinIsRevenue,
                             ac.FinIsRevenuetype
                         };


            var jvItemsList = (await jvList.ToListAsync()).GroupBy(e => e.CostSegCode);
            string customerName = string.Empty;

            List<CustomerRevenueAnalysisDto> list = new();

            foreach (var item in jvItemsList)
            {
                customerName = (await customers.FirstOrDefaultAsync(e => e.CustCode == item.Key)).Name;

                CustomerRevenueAnalysisDto custItem = new()
                {
                    Name = customerName,
                    Rv = item.Where(e => e.FinIsRevenuetype == "rv").Sum(e => e.CrAmount - e.DrAmount),
                    So = item.Where(e => e.FinIsRevenuetype == "so").Sum(e => e.DrAmount - e.CrAmount),
                    De = item.Where(e => e.FinIsRevenuetype == "de").Sum(e => e.DrAmount - e.CrAmount),
                    Ae = item.Where(e => e.FinIsRevenuetype == "ae").Sum(e => e.DrAmount - e.CrAmount),
                    Ot = item.Where(e => e.FinIsRevenuetype == "ot").Sum(e => e.DrAmount - e.CrAmount),
                };

                custItem.ExpTotal = custItem.So + custItem.De + custItem.Ot;
                custItem.GrossTotal = custItem.Rv - custItem.ExpTotal;
                custItem.NetTotal = custItem.GrossTotal - custItem.Ae;

                list.Add(custItem);
            }

            var summary = new CustomerRevenueAnalysisDto
            {
                Rv = list.Sum(e => e.Rv),
                So = list.Sum(e => e.So),
                De = list.Sum(e => e.De),
                Ae = list.Sum(e => e.Ae),
                Ot = list.Sum(e => e.Ot),

                ExpTotal = list.Sum(e => e.ExpTotal),
                GrossTotal = list.Sum(e => e.GrossTotal),
                NetTotal = list.Sum(e => e.NetTotal),


            };


            CustomerRevenueAnalysisListDto custAnalysis = new()
            {
                List = list,
                Summary = summary
            };

            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
             .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            if (company is not null)
            {
                custAnalysis.Company = new()
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

            return custAnalysis;

        }
    }

    #endregion


    #region Customer Or Vendor AgingAnalysis List

    public class GetCustomerVendorAgeingAnalysisList : IRequest<AgeingReportAnalysisListDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Type { get; set; }
        public int ReportCount { get; set; }
        public int ReportIndex { get; set; }
    }
    public class GetCustomerVendorAgeingAnalysisListHandler : IRequestHandler<GetCustomerVendorAgeingAnalysisList, AgeingReportAnalysisListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVendorAgeingAnalysisListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AgeingReportAnalysisListDto> Handle(GetCustomerVendorAgeingAnalysisList request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            bool isCustomer = request.Type == "Customer";
            // var finance = await _context.FinSysFinanialSetups.FirstOrDefaultAsync();

            var custVendInvoices = isCustomer ?
                                             (_context.TrnCustomerInvoices.Include(e => e.SndCustomerMaster)
                                             .Select(e => new { DueDate = EF.Functions.DateFromParts(e.DueDate.Value.Year, e.DueDate.Value.Month, e.DueDate.Value.Day), e.CustCode, e.BalanceAmount })) // = EF.Functions.DateFromParts(e.DueDate.Value.Year, e.DueDate.Value.Month, e.DueDate.Value.Day)
                                         :
                                             (_context.TrnVendorInvoices.Include(e => e.SndVendorMaster)
                                             .Select(e => new { DueDate = EF.Functions.DateFromParts(e.DueDate.Value.Year, e.DueDate.Value.Month, e.DueDate.Value.Day), CustCode = e.VendCode, e.BalanceAmount }));

            custVendInvoices = custVendInvoices.Where(e => e.BalanceAmount > 0).AsNoTracking();
            //vendInvoices = vendInvoices.Where(e => e.BalanceAmount > 0 && (e.DueDate >= finance.FYOpenDate && e.DueDate <= finance.FYClosingDate)).AsNoTracking();

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                custVendInvoices = custVendInvoices.Where(e => EF.Functions.DateFromParts(e.DueDate.Year, e.DueDate.Month, e.DueDate.Day) >= request.DateFrom && EF.Functions.DateFromParts(e.DueDate.Year, e.DueDate.Month, e.DueDate.Day) <= request.DateTo);
            }
            var custVendCodes = custVendInvoices.Select(e => e.CustCode);

            var custOrVendList = isCustomer ?
                                         _context.OprCustomers.AsNoTracking()
                                         .Select(e => new { e.CustCode, e.CustArbName, e.CustName })
                                      :
                                         _context.VendorMasters.AsNoTracking()
                                          .Select(e => new { CustCode = e.VendCode, CustArbName = e.VendArbName, CustName = e.VendName });


            short days = -1 * 30;
            DateTime currentDate = DateTime.Now;// EF.Functions.DateFromParts(tDate.Year, tDate.Month, tDate.Day);
            DateTime toDate = currentDate.AddDays(days);
            // toDate = EF.Functions.DateFromParts(toDate.Year, toDate.Month, toDate.Day);
            // DateTime currentDate = DateTime.Now;
            // currentDate = EF.Functions.DateFromParts(currentDate.Year, currentDate.Month, currentDate.Day);

            if (request.CustCode.HasValue())
                custOrVendList = custOrVendList.Where(e => e.CustCode == request.CustCode);


            var custOrVendAgingList = custOrVendList
                .Where(e => custVendCodes.Any(code => code == e.CustCode))
                .Select(item => new AgeingReportAnalysisDto
                {
                    VendCode = item.CustCode,
                    VendName = isArab ? item.CustArbName : item.CustName,
                    Gpr1 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate && e.DueDate < currentDate).Sum(e => e.BalanceAmount),
                    Gpr2 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate.AddDays(days * 1) && e.DueDate < toDate).Sum(e => e.BalanceAmount),
                    Gpr3 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate.AddDays(days * 2) && e.DueDate < toDate.AddDays(days * 1)).Sum(e => e.BalanceAmount),
                    Gpr4 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate.AddDays(days * 3) && e.DueDate < toDate.AddDays(days * 2)).Sum(e => e.BalanceAmount),
                    Gpr5 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate.AddDays(days * 4) && e.DueDate < toDate.AddDays(days * 3)).Sum(e => e.BalanceAmount),
                    Gpr6 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate >= toDate.AddDays(days * 10 - 35) && e.DueDate < toDate.AddDays(days * 4)).Sum(e => e.BalanceAmount),
                    Gpr7 = custVendInvoices.Where(e => e.CustCode == item.CustCode && e.DueDate < toDate.AddDays(days * 10 - 35)).Sum(e => e.BalanceAmount),

                });


            var reportCount = await custOrVendAgingList.CountAsync();
            custOrVendAgingList = custOrVendAgingList.Pagination(request.ReportIndex, request.ReportCount);
            var customerInvoices = await custOrVendAgingList.ToListAsync();

            foreach (var item in customerInvoices)
            {
                item.Balance = (item.Gpr1 + item.Gpr2 + item.Gpr3 + item.Gpr4 + item.Gpr5 + item.Gpr6);
            }

            AgeingReportAnalysisDto totalGprs = new()
            {
                Gpr1 = customerInvoices.Sum(e => e.Gpr1),
                Gpr2 = customerInvoices.Sum(e => e.Gpr2),
                Gpr3 = customerInvoices.Sum(e => e.Gpr3),
                Gpr4 = customerInvoices.Sum(e => e.Gpr4),
                Gpr5 = customerInvoices.Sum(e => e.Gpr5),
                Gpr6 = customerInvoices.Sum(e => e.Gpr6),
                Gpr7 = customerInvoices.Sum(e => e.Gpr7),
                Balance = customerInvoices.Sum(e => e.Balance),
            };


            var company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == request.User.CompanyId);
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            decimal? totalBalance = customerInvoices.Sum(e => e.Balance);
            customerInvoices.Add(totalGprs);
            return new()
            {

                ComapnyName = company.CompanyName,
                LogoURL = company.LogoURL,
                BranchName = companyBranch.BranchName,
                Address = company.CompanyAddress,
                TotalBalance = totalBalance,
                List = new() { List = customerInvoices, ReportCount = reportCount }
            };
        }
    }

    #endregion

}
