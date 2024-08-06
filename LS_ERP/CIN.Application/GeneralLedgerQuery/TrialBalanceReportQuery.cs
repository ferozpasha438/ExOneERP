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
    #region TrialBalanceReportList

    public class TrialBalanceReportList : IRequest<TrialBalanceReportListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class TrialBalanceReportListHandler : IRequestHandler<TrialBalanceReportList, TrialBalanceReportListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TrialBalanceReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TrialBalanceReportListDto> Handle(TrialBalanceReportList request, CancellationToken cancellationToken)
        {
            var ledgerList = _context.AccountsLedgers
                .OrderBy(e => e.AcCode)
                .AsNoTracking();

            if (request.DateFrom is not null && request.DateTo is not null)
            {
                //Opening Balance calculating
                ledgerList = ledgerList.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) >= request.DateFrom
                                               && EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) <= request.DateTo);
            }

            var list = await ledgerList.GroupBy(e => e.AcCode)
                 .Select(e =>
             new TrialBalanceReportDto
             {
                 FinAcCode = e.Key,
                 DrAmount = e.Sum(d => d.DrAmount),
                 CrAmount = e.Sum(d => d.CrAmount),
                 Balance = e.Sum(d => d.DrAmount) - e.Sum(d => d.CrAmount),
                 CrBalance = 0,
                 DrBalance = 0
             }).ToListAsync();


            foreach (var item in list)
            {
                if (item.Balance < 0)
                    item.CrBalance = item.Balance * -1;

                else if (item.Balance > 0)
                    item.DrBalance = item.Balance;

                var mainAC = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == item.FinAcCode);
                item.Description = mainAC.FinAcDesc;
            }

            var branch = await _context.CompanyBranches.Include(e => e.SysCompany)
                .FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            var trialBalance = new TrialBalanceReportListDto
            {
                List = list,
                TotalDrBalance = list.Sum(e => e.DrBalance),
                TotalCrBalance = list.Sum(e => e.CrBalance),
                CompanyName = branch?.SysCompany.CompanyName ?? string.Empty
            };

            var company = branch?.SysCompany;

            if (company is not null)
            {
                trialBalance.Company = new()
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

            return trialBalance;

        }
    }

    #endregion



    #region TrialBalanceCutOffReportList

    public class TrialBalanceCutOffReportList : IRequest<LedgerReportListDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }

    public class TrialBalanceCutOffReportListHandler : IRequestHandler<TrialBalanceCutOffReportList, LedgerReportListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TrialBalanceCutOffReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<LedgerReportListDto> Handle(TrialBalanceCutOffReportList request, CancellationToken cancellationToken)
        {
            //if (request.DateFrom is  null)
            //    return 
            //var ledgerList = _context.AccountsLedgers.OrderBy(e => e.AcCode).AsNoTracking();
            var fyOpenDate = await _context.FinSysFinanialSetups.Select(e=>e.FYOpenDate).FirstOrDefaultAsync();
            var mainAccounts = _context.FinMainAccounts.AsNoTracking();
            var ledgers = _context.AccountsLedgers
                .Where(e=>e.PostDate >= fyOpenDate)
                .Select(e => new LedgerReportDto
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
            if (request.DateFrom is not null)
            {
                //Opening Balance calculating
                //Opening Balance calculating
                var list1 = (await ledgers.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) < request.DateFrom).ToListAsync()).GroupBy(e => e.FinAcCode).ToList();

                List<LedgerReportDto> Items1 = list.Select(e => new LedgerReportDto
                {
                    FinAcCode = e.FinAcCode,
                    DrAmount = list1.Where(li => e.FinAcCode == li.Key).Sum(li => li.Sum(li => li.DrAmount)),
                    CrAmount = list1.Where(li => e.FinAcCode == li.Key).Sum(li => li.Sum(li => li.CrAmount)),
                }).ToList();

                var list2 = (await ledgers.Where(e => EF.Functions.DateFromParts(e.PostDate.Value.Year, e.PostDate.Value.Month, e.PostDate.Value.Day) >= request.DateFrom
                                                ).ToListAsync()).GroupBy(e => e.FinAcCode).ToList();


                List<LedgerReportDto> summaryList2 = list2.Select(item => new LedgerReportDto
                {
                    FinAcCode = item.Key,
                    DrAmount = item.Sum(e => e.DrAmount),
                    CrAmount = item.Sum(e => e.CrAmount)
                }).ToList();

                foreach (var item in Items1)
                {
                    item.ChangeDrAmount = summaryList2.FirstOrDefault(e => e.FinAcCode == item.FinAcCode)?.DrAmount ?? 0;
                    item.ChangeCrAmount = summaryList2.FirstOrDefault(e => e.FinAcCode == item.FinAcCode)?.CrAmount ?? 0;

                }
                summaryList1.AddRange(Items1);
            }


            summaryList1 = summaryList1.Distinct().ToList();
            foreach (var item in summaryList1)
            {
                item.FinAcName = (await mainAccounts.FirstOrDefaultAsync(e => e.FinAcCode == item.FinAcCode)).FinAcDesc;

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
                List = summaryList1.OrderBy(e => e.FinAcCode).ToList(),
                TotalDr = summaryList1.Sum(e => e.DrAmount),
                TotalCr = summaryList1.Sum(e => e.CrAmount),
                TotalDrCutOff = summaryList1.Sum(e => e.ChangeDrAmount),
                TotalCrCutOff = summaryList1.Sum(e => e.ChangeCrAmount),
                TotalDrClosing = summaryList1.Sum(e => e.ClosingDrAmount),
                TotalCrClosing = summaryList1.Sum(e => e.ClosingCrAmount),

                TotalDrBalance = summaryList1.Sum(e => e.Balance),
                TotalCrBalance = summaryList1.Sum(e => e.ChangeBalance),
                TotalBalance = summaryList1.Sum(e => e.ClosingBalance),
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
}
