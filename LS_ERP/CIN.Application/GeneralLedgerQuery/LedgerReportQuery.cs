using AutoMapper;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.GeneralLedger.CashVoucher;
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


namespace CIN.Application.GeneralLedgerQuery
{

    #region LedgerAnalysysReportList

    public class LedgerAnalysysReportList : IRequest<LedgerReportListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class LedgerAnalysysReportListHandler : IRequestHandler<LedgerAnalysysReportList, LedgerReportListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public LedgerAnalysysReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LedgerReportListDto> Handle(LedgerAnalysysReportList request, CancellationToken cancellationToken)
        {
            var mainAccounts = _context.FinMainAccounts.AsNoTracking();
            var ledgers = _context.AccountsLedgers.Select(e => new LedgerReportDto
            {
                FinAcCode = e.AcCode,
                PostDate = e.PostDate,
                DrAmount = e.DrAmount ?? 0,
                CrAmount = e.CrAmount ?? 0
            });

            var list = await _context.AccountsLedgers.AsNoTracking()
                    .Select(e => new LedgerReportDto
                    {
                        FinAcCode = e.AcCode,
                        FinAcName = string.Empty,

                        DrAmount = 0,
                        CrAmount = 0,
                        Balance = 0,

                        ChangeDrAmount = 0,
                        ChangeCrAmount = 0,
                        ChangeBalance = 0,

                        ClosingDrAmount = 0,
                        ClosingCrAmount = 0,
                        ClosingBalance = 0

                    }).Distinct().ToListAsync();

            List<LedgerReportDto> summaryList1 = new();
            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //Opening Balance calculating
                var list1 = (await ledgers.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) < request.DateFrom).ToListAsync()).GroupBy(e => e.FinAcCode).ToList();


                List<LedgerReportDto> Items1 = list.Select(e => new LedgerReportDto
                {
                    FinAcCode = e.FinAcCode,
                    DrAmount = list1.Where(li => e.FinAcCode == li.Key).Sum(li => li.Sum(li => li.DrAmount)),
                    CrAmount = list1.Where(li => e.FinAcCode == li.Key).Sum(li => li.Sum(li => li.CrAmount)),
                }).ToList();

                //if (list1.Any())
                //    Items1 = (from l in list
                //              join l1 in list1
                //              on l.FinAcCode equals l1.Key
                //               into L_Left
                //              from l1_Left in L_Left.DefaultIfEmpty()
                //              select new LedgerReportDto
                //              {
                //                  FinAcCode = l.FinAcCode,
                //                  DrAmount = l1_Left.Sum(e => e.DrAmount),
                //                  CrAmount = l1_Left.Sum(e => e.CrAmount)
                //              }).ToList();


                //foreach (var item in list)
                //{
                //    item.FinAcCode = list1.
                //    //Items1.Add(
                //    //    new LedgerReportDto
                //    //    {
                //    //        FinAcCode = item.Key,
                //    //        DrAmount = item.Sum(e => e.DrAmount),
                //    //        CrAmount = item.Sum(e => e.CrAmount)
                //    //    });
                //}


                //Changing Balance Calculating
                //List<LedgerReportDto> summaryList2 = new();
                var list2 = (await ledgers.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) >= request.DateFrom
                                                    && EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) <= request.DateTo).ToListAsync()).GroupBy(e => e.FinAcCode).ToList();


                List<LedgerReportDto> summaryList2 = list2.Select(item => new LedgerReportDto
                {
                    FinAcCode = item.Key,
                    DrAmount = item.Sum(e => e.DrAmount),
                    CrAmount = item.Sum(e => e.CrAmount)
                }).ToList();

                //foreach (var item in list2)
                //{
                //    summaryList2.Add(
                //        new LedgerReportDto
                //        {
                //            FinAcCode = item.Key,
                //            DrAmount = item.Sum(e => e.DrAmount),
                //            CrAmount = item.Sum(e => e.CrAmount)
                //        });
                //}



                //foreach (var item in summaryList2)
                //{
                //    item.ChangeDrAmount = item.DrAmount;
                //    item.ChangeCrAmount = item.CrAmount;
                //}

                foreach (var item in Items1)
                {
                    item.ChangeDrAmount = summaryList2.FirstOrDefault(e => e.FinAcCode == item.FinAcCode)?.DrAmount ?? 0;
                    item.ChangeCrAmount = summaryList2.FirstOrDefault(e => e.FinAcCode == item.FinAcCode)?.CrAmount ?? 0;

                }



                //var Items2 = (from l in Items1
                //              join l2 in summaryList2
                //              on l.FinAcCode equals l2.FinAcCode
                //              into L_Left
                //              from l2_Left in L_Left.DefaultIfEmpty()
                //              select new LedgerReportDto
                //              {
                //                  FinAcCode = l.FinAcCode,
                //                  DrAmount = l.DrAmount,
                //                  CrAmount = l.CrAmount,
                //                  ChangeDrAmount = l2_Left.DrAmount,
                //                  ChangeCrAmount = l2_Left.CrAmount
                //              }).ToList();

                summaryList1.AddRange(Items1);


            }

            summaryList1 = summaryList1.Distinct().ToList();
            foreach (var item in summaryList1)
            {
                item.FinAcName = (await mainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == item.FinAcCode)).FinAcName;

                item.Balance = (item.DrAmount - item.CrAmount);
                item.ChangeBalance = (item.ChangeDrAmount - item.ChangeCrAmount);

                item.ClosingDrAmount = (item.DrAmount + item.ChangeDrAmount);
                item.ClosingCrAmount = (item.CrAmount + item.ChangeCrAmount);
                item.ClosingBalance = (item.ClosingDrAmount - item.ClosingCrAmount);
            }



            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
              .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            var ledger = new LedgerReportListDto
            {
                List = summaryList1.OrderBy(e=>e.FinAcCode).ToList(),
                TotalDrBalance = summaryList1.Sum(e => e.DrAmount),
                TotalCrBalance = summaryList1.Sum(e => e.CrAmount),
                TotalBalance = summaryList1.Sum(e => e.Balance),
            };

            if (company is not null)
            {
                ledger.Company = new()
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
            return ledger;

        }
    }

    #endregion


    #region LedgerReportList

    public class LedgerReportList : IRequest<LedgerReportListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class LedgerReportListHandler : IRequestHandler<LedgerReportList, LedgerReportListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public LedgerReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LedgerReportListDto> Handle(LedgerReportList request, CancellationToken cancellationToken)
        {

            var list = await _context.AccountsLedgers.AsNoTracking()
                    .Select(e => new LedgerReportDto
                    {
                        Jvnum = e.Jvnum,
                        TransDate = e.TransDate,
                        PostDate = e.PostDate,
                        DrAmount = e.DrAmount,
                        CrAmount = e.CrAmount,



                    }).ToListAsync();

            decimal? prevBal = 0;
            foreach (var item in list)
            {
                //item.Balance = Sql.Ext.Sum(item.CrAmount).Over().ToValue();
                item.Balance = (item.DrAmount - item.CrAmount) + prevBal;
                prevBal = item.Balance;

            }


            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
            .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            return new LedgerReportListDto
            {
                List = list,
                TotalDrBalance = list.Sum(e => e.DrAmount),
                TotalCrBalance = list.Sum(e => e.CrAmount),
                TotalBalance = list.Sum(e => e.Balance),
                CompanyName = branch?.SysCompany.CompanyName ?? string.Empty
            };
        }
    }

    #endregion

    #region ViewLedgerReportList

    public class ViewLedgerReportList : IRequest<ViewLedgerReportItemListDto>
    {
        public UserIdentityDto User { get; set; }
        public string FinAcCode { get; set; }
        public string BranchCode { get; set; }
        public string Seg1 { get; set; }
        public string Seg2 { get; set; }
        public string CostAllocation { get; set; }
        public string CostSegCode { get; set; }
        public string Batch { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class ViewLedgerReportListHandler : IRequestHandler<ViewLedgerReportList, ViewLedgerReportItemListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ViewLedgerReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ViewLedgerReportItemListDto> Handle(ViewLedgerReportList request, CancellationToken cancellationToken)
        {
            //bool isDate = DateTime.TryParse(request.From, out DateTime trnDate);
            var list = _context.AccountsLedgers.AsNoTracking().Where(e => e.AcCode == request.FinAcCode);


            if (request.BranchCode.HasValue())
                list = list.Where(e => e.BranchCode == request.BranchCode);

            if (request.Batch.HasValue())
            {
                var jvIds = _context.JournalVouchers.Where(e => e.Batch == request.Batch).Select(e => e.Id);
                //var jvItemIds = _context.JournalVoucherItems.Where(e => jvIds.Any(jid => jid == e.JournalVoucherId)).Select(e => e.Id);

                list = list.Where(e => jvIds.Any(jid => jid.ToString() == e.Jvnum));
            }

            if (request.Seg1.HasValue())
            {
                var jvItemIds = _context.JournalVoucherItems.Where(e => e.Batch == request.Seg1).Select(e => e.JournalVoucherId);

                list = list.Where(e => jvItemIds.Any(jid => jid.ToString() == e.Jvnum));
            }

            if (request.Seg2.HasValue())
            {
                var jvItemIds = _context.JournalVoucherItems.Where(e => e.Batch2 == request.Seg2).Select(e => e.JournalVoucherId);

                list = list.Where(e => jvItemIds.Any(jid => jid.ToString() == e.Jvnum));
            }

            if (request.CostAllocation.HasValue())
            {
                //if (request.CostSegCode.HasValue())
                //{
                //    var jvItemIds = _context.JournalVoucherItems.Where(e => e.CostAllocation.ToString() == request.CostAllocation && e.CostSegCode == request.CostSegCode).Select(e => e.JournalVoucherId);
                //    list = list.Where(e => jvItemIds.Any(jid => jid.ToString() == e.Jvnum));
                //}
                //else
                //{
                //    var jvItemIds = _context.JournalVoucherItems.Where(e => e.CostAllocation.ToString() == request.CostAllocation).Select(e => e.JournalVoucherId);
                //    list = list.Where(e => jvItemIds.Any(jid => jid.ToString() == e.Jvnum));

                //}

                if (request.CostSegCode.HasValue())
                {
                    list = list.Where(e => e.CostAllocation.ToString() == request.CostAllocation && e.CostSegCode == request.CostSegCode);
                }
                else
                {
                    list = list.Where(e => e.CostAllocation.ToString() == request.CostAllocation);
                }
            }


            ViewLedgerReportListDto openingLedger = null;

            if (request.From is not null && request.To is not null)
            {
                var list1 = list.Where(e => EF.Functions.DateFromParts(e.TransDate.Year, e.TransDate.Month, e.TransDate.Day) < request.From);
                if (await list1.AnyAsync())
                {
                    var drAmt = list1.Sum(e => e.DrAmount);
                    var crAmt = list1.Sum(e => e.CrAmount);
                    var balAmt = drAmt - crAmt;

                    openingLedger = new ViewLedgerReportListDto
                    {
                        IsOpening = true,
                        TransDate = request.From.Value,
                        //Remarks = "Opening Balance",
                        DrAmount = balAmt > 0 ? balAmt : 0,
                        CrAmount = balAmt < 0 ? (-1) * balAmt : 0,
                        Balance = 0,
                    };

                    //list = list.Where(e => e.TransDate >= request.From && e.TransDate <= request.To);
                    //if (await list.AnyAsync())
                    //{
                    //    openingLedger = new ViewLedgerReportListDto
                    //    {
                    //        DrAmount = e.DrAmount,
                    //        CrAmount = e.CrAmount,
                    //        Remarks = "Opening Balance",
                    //        Narration = e.Narration,
                    //        Source = e.Source,
                    //        FinAcCode = e.AcCode,
                    //        Balance = 0,
                    //        TransDate = e.TransDate
                    //    };

                    //    list = list1.Concat(list);

                    //}
                }

                list = list.Where(e => EF.Functions.DateFromParts(e.TransDate.Year, e.TransDate.Month, e.TransDate.Day) >= request.From
                && EF.Functions.DateFromParts(e.TransDate.Year, e.TransDate.Month, e.TransDate.Day) <= request.To);
            }

            var newList = await list.Select(e => new ViewLedgerReportListDto
            {
                Jvnum = e.Jvnum,
                PostDate = e.PostDate,
                DrAmount = e.DrAmount,
                CrAmount = e.CrAmount,
                Remarks = e.Remarks,
                Narration = e.Narration,
                Source = e.Source,
                FinAcCode = e.AcCode,
                Balance = 0,
                TransDate = e.TransDate
            }).ToListAsync();

            if (openingLedger is not null)
                newList.Insert(0, openingLedger);

            decimal? totalDrAmount = 0, totalCrAmount = 0, totalBalance = 0;
            decimal? prevBal = 0;
            foreach (var item in newList)
            {
                totalDrAmount += item.DrAmount;
                totalCrAmount += item.CrAmount;

                //item.Balance = Sql.Ext.Sum(item.CrAmount).Over().ToValue();
                item.Balance = (item.DrAmount - item.CrAmount) + prevBal;
                prevBal = item.Balance;

            }

            totalBalance = totalDrAmount - totalCrAmount;

            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
              .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            var viewLedger = new ViewLedgerReportItemListDto()
            {
                List = newList,
                CompanyName = company.CompanyName,
                Address = company.CompanyAddress,
                Logo = company.LogoURL,

                TotalDrAmount = totalDrAmount,
                TotalCrAmount = totalCrAmount,
                TotalBalance = totalBalance
            };

            if (company is not null)
            {
                viewLedger.Company = new()
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

            return viewLedger;
        }
    }

    #endregion

    #region AccountVoucherPrint Old

    //public class AccountVoucherPrint : IRequest<AccountVoucherPrintListDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    //public string FinAcCode { get; set; }
    //    public string BranchCode { get; set; }
    //    public DateTime? From { get; set; }
    //    public DateTime? To { get; set; }
    //    public string VoucherType { get; set; }
    //    public string Narration { get; set; }
    //    public string Batch { get; set; }
    //    public string Remarks { get; set; }
    //}

    //public class AccountVoucherPrintHandler : IRequestHandler<AccountVoucherPrint, AccountVoucherPrintListDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public AccountVoucherPrintHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<AccountVoucherPrintListDto> Handle(AccountVoucherPrint request, CancellationToken cancellationToken)
    //    {
    //        //bool isDate = DateTime.TryParse(request.From, out DateTime trnDate);
    //        var list = _context.AccountsLedgers.AsNoTracking().Where(e =>
    //           EF.Functions.Like(e.Narration, "%" + request.Narration + "%")
    //        || EF.Functions.Like(e.Batch, "%" + request.Batch + "%")
    //        || EF.Functions.Like(e.Remarks, "%" + request.Remarks + "%")

    //        );

    //        if (request.From is not null && request.To is not null)
    //            list = list.Where(e => e.TransDate >= request.From && e.TransDate <= request.To);

    //        if (request.BranchCode.HasValue())
    //            list = list.Where(e => e.BranchCode == request.BranchCode);

    //        var newList = await list.Select(e => new AccountVoucherPrintDto
    //        {
    //            Jvnum = e.Jvnum,
    //            PostDate = e.PostDate,
    //            DrAmount = e.DrAmount,
    //            CrAmount = e.CrAmount,
    //            Remarks = e.Remarks,
    //            Narration = e.Narration,
    //            FinAcCode = e.AcCode,
    //            Balance = 0,
    //            TransDate = e.TransDate
    //        }).ToListAsync();


    //        decimal? prevBal = 0;
    //        foreach (var item in newList)
    //        {
    //            //item.Balance = Sql.Ext.Sum(item.CrAmount).Over().ToValue();
    //            item.Balance = (item.DrAmount - item.CrAmount) + prevBal;
    //            prevBal = item.Balance;

    //        }
    //        return newList;
    //    }
    //}

    #endregion

    #region AccountVoucherPrint New

    public class AccountVoucherPrint : IRequest<AccountVoucherPrintListDto>
    {
        public UserIdentityDto User { get; set; }
        //public string FinAcCode { get; set; }
        public string BranchCode { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string VoucherType { get; set; }
        //public string TransactionType { get; set; }
        public string Narration { get; set; }
        public string DocNum { get; set; }
        public string Remarks { get; set; }
    }

    public class AccountVoucherPrintHandler : IRequestHandler<AccountVoucherPrint, AccountVoucherPrintListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AccountVoucherPrintHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountVoucherPrintListDto> Handle(AccountVoucherPrint request, CancellationToken cancellationToken)
        {

            var custPayments = _context.TrnCustomerPayments.Include(e => e.SysCompanyBranch).AsNoTracking().Select(e => new AccountVoucherPrintDto
            {
                BranchCode = Convert.ToString(e.BranchCode),
                BranchName = Convert.ToString(e.SysCompanyBranch.BranchName),
                VoucherNumber = e.VoucherNumber,
                TranDate = e.TranDate,
                CustCode = Convert.ToString(e.CustCode),
                PayCode = Convert.ToString(e.PayCode),
                PayType = Convert.ToString(e.PayType),
                Amount = e.Amount,
                DocNum = Convert.ToString(e.DocNum),
                Checkdate = e.Checkdate,
                CheckNumber = Convert.ToString(e.CheckNumber),
                Preparedby = Convert.ToString(e.Preparedby),
                Remarks = Convert.ToString(e.Remarks),
                Narration = Convert.ToString(e.Narration),
            });
            var vendPayments = _context.TrnVendorPayments.Include(e => e.SysCompanyBranch).AsNoTracking().Select(e => new AccountVoucherPrintDto
            {
                BranchCode = Convert.ToString(e.BranchCode),
                BranchName = Convert.ToString(e.SysCompanyBranch.BranchName),
                VoucherNumber = e.VoucherNumber,
                TranDate = e.TranDate,
                CustCode = Convert.ToString(e.VendCode),
                PayCode = Convert.ToString(e.PayCode),
                PayType = Convert.ToString(e.PayType),
                Amount = e.Amount,
                DocNum = Convert.ToString(e.DocNum),
                Checkdate = e.Checkdate,
                CheckNumber = Convert.ToString(e.CheckNumber),
                Preparedby = Convert.ToString(e.Preparedby),
                Remarks = Convert.ToString(e.Remarks),
                Narration = Convert.ToString(e.Narration),
            });

            var custVendList = custPayments.Union(vendPayments);

            custVendList = custVendList.Where(e =>
               EF.Functions.Like(e.Narration, "%" + request.Narration + "%")
            && EF.Functions.Like(e.DocNum, "%" + request.DocNum + "%")
            && EF.Functions.Like(e.Remarks, "%" + request.Remarks + "%")
            && EF.Functions.Like(e.BranchCode, "%" + request.BranchCode + "%")
            && EF.Functions.Like(e.PayType, "%" + request.VoucherType + "%")

            );

            if (request.From is not null && request.To is not null)
                custVendList = custVendList.Where(e => e.TranDate >= request.From && e.TranDate <= request.To);

            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
              .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            var company = branch?.SysCompany;

            var voucherPrintList = new AccountVoucherPrintListDto
            {
                List = await custVendList.OrderByDescending(e => e.VoucherNumber).ToListAsync()
            };


            if (company is not null)
            {
                voucherPrintList.Company = new()
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

            return voucherPrintList;
        }
    }

    #endregion

    #region LedgerBranchViewList

    public class LedgerBranchViewList : IRequest<List<LedgerBranchViewListDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class LedgerBranchViewListHandler : IRequestHandler<LedgerBranchViewList, List<LedgerBranchViewListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public LedgerBranchViewListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LedgerBranchViewListDto>> Handle(LedgerBranchViewList request, CancellationToken cancellationToken)
        {
            var list = await _context.FinBranchesMainAccounts.Include(e => e.SysCompanyBranch)
                .AsNoTracking().ToListAsync();

            var newList = list.GroupBy(e => e.FinBranchCode);

            List<LedgerBranchViewListDto> itemsList = new();
            foreach (var item in newList)
            {
                LedgerBranchViewListDto ledgerBranchViewListDto = new()
                {
                    BranchName = item.FirstOrDefault().SysCompanyBranch.BranchName,
                    List = await _context.FinBranchesMainAccounts.AsNoTracking()
                    .Where(e => e.FinBranchCode == item.Key)
                    .Select(e => new CustomSelectListItem
                    {
                        Text = e.FinAcCode,
                        TextTwo = "(" + e.FinAcCode + ") " + e.FinMainAccounts.FinAcName,
                        Value = item.Key
                    }).ToListAsync()

                };
                itemsList.Add(ledgerBranchViewListDto);
            }

            return itemsList;
        }
    }

    #endregion

}
