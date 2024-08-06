using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InvoiceDtos;
using CIN.Application.SndDtos;
using CIN.Application.SndDtos.Comman;
using CIN.Application.SNdDtos;
using CIN.DB;
using CIN.DB.One.Migrations;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SND;
using CIN.Domain.SndQuotationSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using static CIN.Application.SndDtos.SndQuotationDto;

namespace CIN.Application.SndQuery
{
    #region CreateSndApproval
    public class CreateSndApproval : UserIdentityDto, IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public TblSndTrnApprovalsDto Input { get; set; }
    }
    public class CreateSndApprovalQueryHandler : IRequestHandler<CreateSndApproval, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSndApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> Handle(CreateSndApproval request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateSndApproval method start----");

                    if (request.Input.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice)
                    {
                        #region approving invoice

                        var obj = await _context.SndTranInvoice.Include(e => e.SysWarehouse).ThenInclude(x => x.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        var branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode);

                        if (await _context.TblSndTrnApprovalsList.AnyAsync(e => e.ServiceCode == obj.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndInvoice && e.AppAuth == request.User.UserId && e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode && e.IsApproved))
                            return true;

                        TblSndTrnApprovals approval = new()
                        {

                            BranchCode = obj.SysWarehouse.WHBranchCode,
                            AppAuth = request.User.UserId,
                            CreatedOn = DateTime.UtcNow,
                            ServiceCode = obj.Id.ToString(),
                            ServiceType = (short)EnumSndApprovalServiceType.SndInvoice,
                            AppRemarks = request.Input.AppRemarks,
                            IsApproved = true,
                        };

                        await _context.TblSndTrnApprovalsList.AddAsync(approval);
                        await _context.SaveChangesAsync();

                        if (!obj.InvoiceNumber.HasValue())
                        {
                            int invoiceSeq = 0;
                            var invSeq = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.WHBranchCode);
                            if (invSeq is null)
                            {
                                invoiceSeq = 1;
                                TblSequenceNumberSetting setting = new()
                                {
                                    SDInvoiceNumber = invoiceSeq,
                                    BranchCode = obj.SysWarehouse.WHBranchCode,
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

                            obj.InvoiceNumber = invoiceSeq.ToString();
                            obj.SpInvoiceNumber = string.Empty;
                            _context.SndTranInvoice.Update(obj);
                            await _context.SaveChangesAsync();
                        }
                        #endregion
                    }

                    else if (request.Input.ServiceType == (short)EnumSndApprovalServiceType.SndQuotation)
                    {
                        #region approving Quotation
                        var obj = await _context.SndTranQuotations.Include(e => e.SysWarehouse).ThenInclude(x => x.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        var branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode);

                        if (await _context.TblSndTrnApprovalsList.AnyAsync(e => e.ServiceCode == obj.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndQuotation && e.AppAuth == request.User.UserId && e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode && e.IsApproved))
                            return true;

                        TblSndTrnApprovals approval = new()
                        {

                            BranchCode = obj.SysWarehouse.WHBranchCode,
                            AppAuth = request.User.UserId,
                            CreatedOn = DateTime.UtcNow,
                            ServiceCode = obj.Id.ToString(),
                            ServiceType = (short)EnumSndApprovalServiceType.SndQuotation,
                            AppRemarks = request.Input.AppRemarks,
                            IsApproved = true,
                        };

                        await _context.TblSndTrnApprovalsList.AddAsync(approval);
                        await _context.SaveChangesAsync();

                        if (!obj.QuotationNumber.HasValue())
                        {
                            int quotationSeq = 0;
                            var sequence = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.WHBranchCode);
                            if (sequence is null)
                            {
                                quotationSeq = 1;
                                TblSequenceNumberSetting setting = new()
                                {
                                    SDQuoteNumber = quotationSeq,
                                    BranchCode = obj.SysWarehouse.WHBranchCode,
                                };
                                await _context.Sequences.AddAsync(setting);
                            }
                            else
                            {
                                quotationSeq = sequence.SDQuoteNumber + 1;
                                sequence.SDQuoteNumber = quotationSeq;
                                _context.Sequences.Update(sequence);
                            }
                            await _context.SaveChangesAsync();

                            obj.QuotationNumber = quotationSeq.ToString();
                            obj.SpQuotationNumber = string.Empty;
                            _context.SndTranQuotations.Update(obj);
                            await _context.SaveChangesAsync();

                            var quotationsList = await _context.SndTranQuotationItems.AsNoTracking().Where(e => e.QuotationId == obj.Id).ToListAsync();

                            if (quotationsList.Count > 0)
                            {
                                foreach (var quot in quotationsList)
                                {
                                    quot.QuotationNumber = quotationSeq.ToString();
                                }
                                _context.SndTranQuotationItems.UpdateRange(quotationsList);
                                await _context.SaveChangesAsync();
                            }
                        }
                            #endregion

                        }







                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateSndApproval Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return false;
                }
            }
        }
    }
    #endregion
}
