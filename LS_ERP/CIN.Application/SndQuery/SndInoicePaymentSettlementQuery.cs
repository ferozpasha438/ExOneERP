using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SndQuery
{

    #region CreateSndInvoicePaymentSettlement

    public class CreateSndInvoicePaymentSettlement : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public IOTblSndTranInvoicePaymentSettlementsDto Input { get; set; }
    }
    public class CreateSndInvoicePaymentSettlementQueryHandler : IRequestHandler<CreateSndInvoicePaymentSettlement, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSndInvoicePaymentSettlementQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CreateSndInvoicePaymentSettlement request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateSndInvoicePaymentSettlement method start----");

                    var invoice = await _context.SndTranInvoice.AsNoTracking().Include(e=>e.SndSalesTermsCode).FirstOrDefaultAsync(e=>e.Id==request.Input.InvoiceId);
                    if(invoice==null)
                    {
                        return -1;//invalid invoice Id
                    }


                    if (request.Input.PaymentType == "Credit")   //settlement only with credit
                    {
                        if (invoice.SndSalesTermsCode.SalesTermsDueDays == 0)
                        {
                            return -2;

                        }
                        //invoice.IsCreditConverted = true;
                        invoice.IsSettled = true;
                       // invoice.InvoiceStatusId = (int)InvoiceStatusIdType.Credit;
                        _context.SndTranInvoice.Update(invoice);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return invoice.Id;
                    }

                    else if (request.Input.PaymentType == "Cash" && request.Input.PaymentsList.Count == 0)
                    {
                        return -3;

                    }
                


                    var settlements = await _context.TblSndTranInvoicePaymentSettlementsList.Where(e=>e.InvoiceId==request.Input.InvoiceId).ToListAsync();
                    if (settlements.Count > 0)
                    {
                        _context.TblSndTranInvoicePaymentSettlementsList.RemoveRange(settlements);
                        await _context.SaveChangesAsync();
                    }

                    List<TblSndTranInvoicePaymentSettlements> Settlements = new();
                    for (var i=0;i<request.Input.PaymentsList.Count;i++) {
                        Settlements.Add(
                            new TblSndTranInvoicePaymentSettlements
                            {
                                PaymentCode= request.Input.PaymentsList[i].PaymentCode,
                                InvoiceId= request.Input.InvoiceId,
                                DueDate=invoice.InvoiceDueDate,
                                SettledAmount = request.Input.PaymentsList[i].SettledAmount,
                                SettledDate = DateTime.UtcNow,
                                SettledBy=request.User.UserId,
                                Remarks=request.Input.PaymentsList[i].Remarks,
                                 IsPaid=false
                            }
                            
                            );



                    }

                    await _context.TblSndTranInvoicePaymentSettlementsList.AddRangeAsync(Settlements);

                    await _context.SaveChangesAsync();



                    invoice.IsSettled = true;
                   
                    _context.SndTranInvoice.Update(invoice);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();



                    return  request.Input.InvoiceId;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateSndInvoicePaymentSettlement Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion




}
