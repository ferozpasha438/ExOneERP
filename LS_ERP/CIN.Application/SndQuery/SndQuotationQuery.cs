using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InventoryDtos;
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
    #region GetSndQuotationPagedList

    public class GetSndQuotationPagedList : IRequest<PaginatedList<TblSndTranQuotationPagedListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSndQuotationPagedListHandler : IRequestHandler<GetSndQuotationPagedList, PaginatedList<TblSndTranQuotationPagedListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndQuotationPagedListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblSndTranQuotationPagedListDto>> Handle(GetSndQuotationPagedList request, CancellationToken cancellationToken)
        {
            try
            {



                Log.Info("----Info GetSndQuotationPagedList method start----");

                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
                var invoices = _context.SndTranQuotations.Include(e=>e.SysWarehouse).ThenInclude(e=>e.SysCompanyBranch).AsNoTracking();//.Where(e => e.BranchCode == request.User.BranchCode);
                var companies = _context.Companies.AsNoTracking();
                var customers = _context.OprCustomers.AsNoTracking();
                var custApproval = _context.TblSndTrnApprovalsList.Where(e=>e.ServiceType== (short)EnumSndApprovalServiceType.SndQuotation).AsNoTracking();
               
                
                //var cQuotations = _context.TrnCustomerQuotations.AsNoTracking();


                var authorities =await _context.TblSndAuthoritiesList.ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).ToListAsync();
                bool isArab = request.User.Culture.IsArab();


                if (request.Input.Id > 0)
                {
                    var invoiceItems = _context.SndTranQuotationItems.AsNoTracking();
                    var invoiceIds = invoiceItems.Where(e => e.ItemCode == request.Input.Id.ToString()).Select(e => e.QuotationId);

                    invoices = invoices.Where(e => invoiceIds.Any(id => id == e.Id));
                }

                bool isDate = DateTime.TryParse(request.Input.Query, out DateTime invoiceDate);
                var reports = from IV in invoices
                              join CM in companies
                              on IV.CompanyId equals CM.Id
                              join CS in customers
                              on IV.CustomerId equals CS.Id

                              where 
                              
                              (
                         CS.CustName.Contains(request.Input.Query) ||
                         EF.Functions.Like(CS.CustArbName, "%" + request.Input.Query + "%") ||
                         IV.QuotationNumber.Contains(request.Input.Query) ||
                         IV.SpQuotationNumber.Contains(request.Input.Query) ||
                         //IV.TotalAmount.ToString().Contains(request.Input.Query.Replace(",", "")) ||
                         (isDate && IV.QuotationDate == invoiceDate)
                              )
                              select new TblSndTranQuotationPagedListDto
                              {
                                  Id = IV.Id,
                                  //SpQuotationNumber = IV.SpQuotationNumber,
                                  QuotationNumber = IV.QuotationNumber == string.Empty? IV.SpQuotationNumber : IV.QuotationNumber,
                                  SpQuotationNumber=IV.SpQuotationNumber,
                                  QuotationDate = IV.QuotationDate,
                                  CreatedOn = IV.CreatedOn,
                                  QuotationDueDate = IV.QuotationDueDate,
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
                                  QuotationRefNumber = IV.QuotationRefNumber,
                                  LpoContract = IV.LpoContract,
                                  PaymentTermId = IV.SndSalesTermsCode.SalesTermsCode,
                                  PaymentTermName = IV.SndSalesTermsCode.SalesTermsName,
                                  DueDays = IV.SndSalesTermsCode.SalesTermsDueDays,
                                  
                                  TaxIdNumber = IV.TaxIdNumber,
                                   IsFinalRevision=IV.IsFinalRevision,

                                  OriginalQuotationId=IV.OriginalQuotationId,
                                  RevisedNumber=IV.RevisedNumber,
                                  IsRevised=IV.IsRevised,
                                  IsConvertedSndQuotationToInvoice = IV.IsConvertedSndQuotationToInvoice,
                                  IsConvertedToOrder=IV.IsConvertedSndQuotationToOrder,
                                  IsConvertedSndQuotationToDeliveryNote = IV.IsConvertedSndQuotationToDeliveryNote,

                                  ApprovedUser = custApproval.Any(e => e.AppAuth == request.User.UserId && e.ServiceCode == IV.Id.ToString() && e.ServiceType== (short)EnumSndApprovalServiceType.SndQuotation && e.IsApproved),
                                  
                                  
                                  
                                  IsApproved = (custApproval.Where(e => e.ServiceCode == IV.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndQuotation &&  e.IsApproved).Any())||(IV.IsApproved),
                                
                                   IsVoid=IV.IsVoid,
                                   

                                 HasAuthority = _context.TblSndAuthoritiesList.Where(e=>e.BranchCode== IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth==request.User.UserId).ToList().Count >0,
                                 BranchCode= IV.SysWarehouse.SysCompanyBranch.BranchCode,
                                 WarehouseCode=IV.WarehouseCode,
                                  Authority = _context.TblSndAuthoritiesList.Where(e => e.BranchCode == IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth == request.User.UserId).ToList().Count > 0 ? _context.TblSndAuthoritiesList.ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.BranchCode == IV.SysWarehouse.SysCompanyBranch.BranchCode && e.AppAuth == request.User.UserId)
                                  : new TblSndAuthoritiesDto { 
                                      AppAuth=request.User.UserId,

                                      CanCreateSndQuotation=false,
                                      CanEditSndQuotation=false,
                                      CanApproveSndQuotation=false,
                                   
                                      CanVoidSndQuotation=false,
                                      CanConvertSndQuotationToInvoice = false,
                                      CanConvertSndQuotationToOrder = false,
                                      CanReviseSndQuotation = false,
                                      CanConvertSndQuotationToDeliveryNote = false,
                                       CanApproveSndInvoice=false,
                                       CanConvertSndDeliveryNoteToInvoice=false,
                                        CanCreateSndInvoice=false,
                                     CanEditSndInvoice=false,
                                     CanSettleSndInvoice=false,
                                      CanPostSndInvoice=false,
                                      CanVoidSndInvoice=false,
                                       BranchCode= IV.SysWarehouse.SysCompanyBranch.BranchCode,
                                        



                                  }
                              };







                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    reports = aprv switch
                    {
                        "approved" => reports.Where(e => (e.IsApproved && e.IsFinalRevision) && !e.IsVoid),
                        "unapproved" => reports.Where(e => !e.IsApproved && !e.IsVoid),
                        "original" => reports.Where(e => e.Id==e.OriginalQuotationId && !e.IsVoid),
                        "finalquot" => reports.Where(e => e.IsFinalRevision && !e.IsVoid),
                        "cancelled" => reports.Where(e => e.IsVoid),
                        _ => reports.Where(e=>!e.IsVoid)
                    };
                }
                else {
                    reports = reports.Where(e => !e.IsVoid);
                }
                //.OrderByDescending(t => t.CustomerId)
                // .ProjectTo<TblTranQuotationDto>(_mapper.ConfigurationProvider)
                var nreports = await reports.OrderBy(request.Input.OrderBy).Where(e=>e.HasAuthority || isAdmin).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetSndQuotationPagedList method Exit----");
                return nreports;
            }
            catch (Exception ex)
            {


                Log.Error("Error in GetSndQuotationPagedList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }

    }

    #endregion


    #region CreateSndQuotation

    public class CreateSndQuotation : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputTblSndTranQuotationDto Input { get; set; }
    }
    public class CreateSndQuotationQueryHandler : IRequestHandler<CreateSndQuotation, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSndQuotationQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CreateSndQuotation request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateQuotation method start----");
                    byte revisedNumber = 0;
                    long originalId = 0;
                    string quotNumber = string.Empty;
                    long inputId = request.Input.Id;
                    bool isrevised = false;
                    if (request.Input.SaveType == EnumQuotationSaveType.Revise)
                    {
                        var inputQuotation = await _context.SndTranQuotations.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        originalId = inputQuotation.OriginalQuotationId;
                        quotNumber = inputQuotation.QuotationNumber;



                        if (originalId > 0)
                        {
                            var revisedList= await _context.SndTranQuotations.AsNoTracking().OrderByDescending(e => e.RevisedNumber).Where(e => e.OriginalQuotationId == originalId).ToListAsync();
                            var recentRevisedQuotation = revisedList.FirstOrDefault(e => e.OriginalQuotationId == originalId);
                            revisedNumber = (byte)(recentRevisedQuotation.RevisedNumber + 1);

                            revisedList.ForEach(e=>{ e.IsFinalRevision = false; });
                            _context.SndTranQuotations.UpdateRange(revisedList);
                            await _context.SaveChangesAsync();
                        }
                    }



                    string spQuotationNumber = $"{"Q"+ new Random().Next(99, 9999999).ToString()}";

                    TblSndTranQuotation Quotation = new();
                    if(request.Input.SaveType==EnumQuotationSaveType.Revise)
                    {
                      
                        request.Input.Id = 0;

                        request.Input.RevisedNumber = revisedNumber;
                        isrevised = true;
                        request.Input.OriginalQuotationId =originalId;


                    }
                  

                    var obj = request.Input;

                    if (request.Input.Id > 0)
                    {

                        Quotation = await _context.SndTranQuotations.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                        spQuotationNumber = Quotation.SpQuotationNumber;
                        
                        Quotation.CustomerId = obj.CustomerId;
                        Quotation.QuotationDate = Convert.ToDateTime(obj.QuotationDate);
                        Quotation.QuotationDueDate = Convert.ToDateTime(obj.QuotationDueDate);
                        Quotation.ServiceDate1 = obj.ServiceDate1;
                        Quotation.CompanyId = obj.CompanyId;
                        Quotation.WarehouseCode = obj.WarehouseCode;
                        Quotation.QuotationRefNumber = obj.QuotationRefNumber;

                        Quotation.LpoContract = obj.LpoContract;
                        Quotation.PaymentTermId = obj.PaymentTermId;
                        Quotation.TaxIdNumber = obj.TaxIdNumber;
                        Quotation.QuotationNotes = obj.QuotationNotes;
                        Quotation.Remarks = obj.Remarks;


                        Quotation.SubTotal = obj.SubTotal;
                        Quotation.DiscountAmount = obj.DiscountAmount ?? 0;
                        Quotation.AmountBeforeTax = obj.AmountBeforeTax ?? 0;
                        Quotation.TaxAmount = obj.TaxAmount;
                        Quotation.TotalAmount = obj.TotalAmount;
                        Quotation.TotalPayment = obj.TotalPayment ?? 0;
                        Quotation.AmountDue = obj.AmountDue ?? 0;
                        Quotation.FooterDiscount = obj.FooterDiscount;

                        Quotation.OriginalQuotationId = obj.OriginalQuotationId;
                        Quotation.IsConvertedSndQuotationToInvoice = false;
                        Quotation.IsConvertedSndQuotationToOrder = false;
                        Quotation.IsConvertedSndQuotationToDeliveryNote = false;
                        Quotation.IsFinalRevision = true;


                        if (obj.CustName.HasValue())
                        {
                            Quotation.CustName = obj.CustName;
                            Quotation.CustArbName = obj.CustArbName;
                        }

                        var items = _context.SndTranQuotationItems.Where(e => e.QuotationId == request.Input.Id);
                        _context.RemoveRange(items);


                        _context.SndTranQuotations.Update(Quotation);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (obj.TotalAmount.Value==0)
                            return 0;

                        Quotation = new()
                        {
                            SpQuotationNumber = originalId >0? string.Empty:spQuotationNumber,
                            QuotationNumber = originalId>0 ? quotNumber:string.Empty,
                            QuotationDate = Convert.ToDateTime(obj.QuotationDate),
                            QuotationDueDate = Convert.ToDateTime(obj.QuotationDueDate),
                            CompanyId = obj.CompanyId,


                            SubTotal = obj.SubTotal,
                            DiscountAmount = obj.DiscountAmount ?? 0,
                            AmountBeforeTax = obj.AmountBeforeTax ?? 0,
                            TaxAmount = obj.TaxAmount,
                            TotalAmount = obj.TotalAmount,
                            TotalPayment = obj.TotalPayment ?? 0,
                            AmountDue = obj.AmountDue ?? 0,


                            IsDefaultConfig = true,
                            CreatedOn = Convert.ToDateTime(obj.QuotationDate),
                            CreatedBy = request.User.UserId,
                            CustomerId = obj.CustomerId,
                            QuotationStatus = "Open",
                            TaxIdNumber = obj.TaxIdNumber,
                            QuotationModule = obj.QuotationModule,
                            QuotationNotes = obj.QuotationNotes,
                            Remarks = obj.Remarks,
                            QuotationRefNumber = obj.QuotationRefNumber,
                            LpoContract = obj.LpoContract,
                            VatPercentage = obj.VatPercentage ?? 0,
                            PaymentTermId = obj.PaymentTermId,
                            WarehouseCode = obj.WarehouseCode,
                            ServiceDate1 = obj.ServiceDate1,
                           IsFinalRevision=true,
                            CustName = obj.CustName.HasValue() ? obj.CustName : string.Empty,
                            CustArbName = obj.CustArbName.HasValue() ? obj.CustArbName : string.Empty,
                            FooterDiscount = obj.FooterDiscount,

                            IsRevised=isrevised,
                            RevisedNumber=revisedNumber,
                            IsApproved= isrevised,
                             IsConvertedSndQuotationToInvoice=false,
                              IsConvertedSndQuotationToOrder=false,
                            IsConvertedSndQuotationToDeliveryNote = false,
                              IsVoid=false,
                               
                            OriginalQuotationId=originalId,
                        };


                         _context.SndTranQuotations.Add(Quotation);
                        await _context.SaveChangesAsync();

                        if (inputId==0 && request.Input.SaveType!=EnumQuotationSaveType.Revise)
                        {
                            Quotation.OriginalQuotationId = Quotation.Id;
                            _context.SndTranQuotations.Update(Quotation);
                            await _context.SaveChangesAsync();

                        }



                    }


                    Log.Info("----Info CreateUpdateQuotation method Exit----");

                    var quotationId = Quotation.Id;
                    var quotationItems = request.Input.ItemList;
                    if (quotationItems.Count > 0)
                    {
                        List<TblSndTranQuotationItem> quotationItemsList = new();

                        foreach (var obj1 in quotationItems)
                        {
                            var item = _context.InvItemMaster.FirstOrDefault(e=>e.ItemCode==obj1.ItemCode);
                            var convFactor = _context.InvItemsUOM.FirstOrDefault(e=>e.ItemCode==obj1.ItemCode && e.ItemUOM==item.ItemBaseUnit).ItemConvFactor;
                            var QuotationItem = new TblSndTranQuotationItem
                            {
                                QuotationId = quotationId,
                                QuotationNumber =  Quotation.QuotationNumber,
                                QuotationType = obj1.QuotationType,
                                ItemCode = obj1.ItemCode,
                                Quantity = obj1.Quantity,
                                UnitPrice = obj1.UnitPrice,
                                UnitType=obj1.UnitType,
                                SubTotal = obj1.SubTotal,
                                DiscountAmount = obj1.DiscountAmount,
                                AmountBeforeTax = obj1.AmountBeforeTax,
                                TaxAmount = obj1.TaxAmount,
                                TotalAmount = obj1.TotalAmount,
                                IsDefaultConfig = true,
                                CreatedOn = Convert.ToDateTime(obj.QuotationDate),
                                CreatedBy = (int)request.UserId,
                                Description = obj1.Description,
                                TaxTariffPercentage = obj1.TaxTariffPercentage,
                                Discount = obj1.Discount,
                            };
                            quotationItemsList.Add(QuotationItem);
                        }
                        if (quotationItemsList.Count > 0)
                        {
                            await _context.SndTranQuotationItems.AddRangeAsync(quotationItemsList);
                            await _context.SaveChangesAsync();
                        }

                    }



                    if (request.Input.SaveType == EnumQuotationSaveType.SaveAndApprove )
                    {

                        var warehouse = await _context.InvWarehouses.FirstOrDefaultAsync(e => e.WHCode == request.Input.WarehouseCode);

                        TblSndTrnApprovals approval = new()
                        {

                            BranchCode = warehouse.WHBranchCode,
                            AppAuth = request.User.UserId,
                            CreatedOn = DateTime.UtcNow,
                            ServiceCode = Quotation.Id.ToString(),
                            ServiceType = (short)EnumSndApprovalServiceType.SndQuotation,
                            AppRemarks = "Save And Approve",
                            IsApproved = true,
                        };

                        await _context.TblSndTrnApprovalsList.AddAsync(approval);
                        await _context.SaveChangesAsync();

                        if (!Quotation.QuotationNumber.HasValue())
                        {
                            int sequenceNumber = 0;
                            var invSeq = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == Quotation.SysWarehouse.WHBranchCode);
                            if (invSeq is null)
                            {
                                sequenceNumber = 1;
                                TblSequenceNumberSetting setting = new()
                                {
                                    SDQuoteNumber = sequenceNumber,
                                    BranchCode = Quotation.SysWarehouse.WHBranchCode,
                                };
                                await _context.Sequences.AddAsync(setting);
                            }
                            else
                            {
                                sequenceNumber = invSeq.SDQuoteNumber + 1;
                                invSeq.SDQuoteNumber = sequenceNumber;
                                _context.Sequences.Update(invSeq);
                            }
                            await _context.SaveChangesAsync();

                            Quotation.QuotationNumber = sequenceNumber.ToString();
                            Quotation.SpQuotationNumber = string.Empty;

                            Quotation.IsApproved = true;          //extra statement with respect to normal Approval

                            _context.SndTranQuotations.Update(Quotation);
                            await _context.SaveChangesAsync();
                        }

                       
                    }


                    await transaction.CommitAsync();

                    return Quotation.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSndQuotation Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion


    #region GetSingleSndQuotationById

    public class GetSingleSndQuotationById : IRequest<TblSndTranQuotationDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetSingleSndQuotationByIdHandler : IRequestHandler<GetSingleSndQuotationById, TblSndTranQuotationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSndQuotationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSndTranQuotationDto> Handle(GetSingleSndQuotationById request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetSingleSndQuotationById method start----");
            try
            {
                var quotation = await _context.SndTranQuotations.Include(e => e.SysWarehouse).ThenInclude(e => e.SysCompanyBranch).ThenInclude(e => e.SysCompany)
                       .FirstOrDefaultAsync(e => e.Id == request.Id);

                var items = await _context.SndTranQuotationItems
                    .Where(e => e.QuotationId == request.Id )

                    .ToListAsync();



                var productUnitTypes = _context.TranProducts.Include(e => e.UnitType).AsNoTracking()
                    .Select(e => new { e.Id, pNameEN = e.NameEN, uNameEN = e.UnitType.NameEN });


                
             

            

                TblSndTranQuotationDto quotationDto = new()
                {
                    Id=quotation.Id,
                    CustomerId = quotation.CustomerId,
                    QuotationDate = quotation.QuotationDate,
                    QuotationDueDate = quotation.QuotationDueDate,
                    CompanyId = quotation.CompanyId,
                    WarehouseCode = quotation.SysWarehouse.WHCode,
                    QuotationRefNumber = quotation.QuotationRefNumber,
                    LpoContract = quotation.LpoContract,
                    PaymentTermId = quotation.PaymentTermId,
                    QuotationNumber = quotation.QuotationNumber,
                    ServiceDate1 = quotation.ServiceDate1,
                   OriginalQuotationId=quotation.OriginalQuotationId,
                    RevisedNumber=quotation.RevisedNumber,
                     IsRevised=quotation.IsRevised,
                    SubTotal = quotation.SubTotal,
                    TaxAmount = quotation.TaxAmount,
                    TotalAmount = quotation.TotalAmount,
                    DiscountAmount = quotation.DiscountAmount,
                    PaidAmount = 0,
                    IsFinalRevision=quotation.IsFinalRevision,
                    CreatedOn = quotation.CreatedOn,
                    TaxIdNumber = quotation.TaxIdNumber,
                    QuotationNotes = quotation.QuotationNotes,
                    Remarks = quotation.Remarks,
                    CustName = quotation.CustName,
                    CustArbName = quotation.CustArbName,
                    CustomerName = quotation.CustName,
                    LogoImagePath = quotation.SysWarehouse.SysCompanyBranch.SysCompany.LogoURL,
                    FooterDiscount = (short)quotation.FooterDiscount
                };

                List<TblSndTranQuotationItemDto> itemList = new();

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

                    TblSndTranQuotationItemDto itemDto = new()
                    {

                        ItemId = Item.Id,

                        ItemCode = item.ItemCode,
                        ItemName = pNameEN,
                        Description = item.Description,
                        Quantity =  item.Quantity,
                        UnitType = item.UnitType,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        DiscountAmount = item.DiscountAmount,

                        TaxTariffPercentage = item.TaxTariffPercentage,
                        TaxAmount = item.TaxAmount,
                        TotalAmount =item.TotalAmount,
                    };

                    itemList.Add(itemDto);
                }

              

                quotationDto.ItemList = itemList;

                Log.Info("----Info GetSingleSndQuotationById method Exit----");
                return quotationDto;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSingleSndQuotationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;

            }
        }

    }

    #endregion


    //#region CreateSndQuotationApproval
    //public class CreateSndQuotationApproval : UserIdentityDto, IRequest<bool>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public TblSndTrnApprovalsDto Input { get; set; }
    //}
    //public class CreateSndQuotationApprovalQueryHandler : IRequestHandler<CreateSndQuotationApproval, bool>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateSndQuotationApprovalQueryHandler(IMapper mapper, CINDBOneContext context)
    //    {
    //        _mapper = mapper;
    //        _context = context;
    //    }

    //    public async Task<bool> Handle(CreateSndQuotationApproval request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info CreateSndQuotationApproval method start----");

    //                var obj = await _context.SndTranQuotations.Include(e => e.SysWarehouse).ThenInclude(x => x.SysCompanyBranch).FirstOrDefaultAsync(e => e.Id == request.Input.Id);
    //                var branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode);

    //                if (await _context.TblSndTrnApprovalsList.AnyAsync(e => e.ServiceCode == obj.Id.ToString() && e.ServiceType == (short)EnumSndApprovalServiceType.SndQuotation && e.AppAuth == request.User.UserId && e.BranchCode == obj.SysWarehouse.SysCompanyBranch.BranchCode && e.IsApproved))
    //                    return true;

    //                TblSndTrnApprovals approval = new()
    //                {

    //                    BranchCode = obj.SysWarehouse.WHBranchCode,
    //                    AppAuth = request.User.UserId,
    //                    CreatedOn = DateTime.UtcNow,
    //                    ServiceCode = obj.Id.ToString(),
    //                    ServiceType = (short)EnumSndApprovalServiceType.SndQuotation,
    //                    AppRemarks = request.Input.AppRemarks,
    //                    IsApproved = true,
    //                };

    //                await _context.TblSndTrnApprovalsList.AddAsync(approval);
    //                await _context.SaveChangesAsync();
    //                int quotationSeq = 0;
    //                if (!obj.QuotationNumber.HasValue())
    //                {
    //                    var invSeq = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == obj.SysWarehouse.WHBranchCode);
    //                    if (invSeq is null)
    //                    {
    //                        quotationSeq = 1;
    //                        TblSequenceNumberSetting setting = new()
    //                        {
    //                            SDQuoteNumber = quotationSeq,
    //                            BranchCode = obj.SysWarehouse.WHBranchCode,
    //                        };
    //                        await _context.Sequences.AddAsync(setting);
    //                    }
    //                    else
    //                    {
    //                        quotationSeq = invSeq.SDQuoteNumber + 1;
    //                        invSeq.SDQuoteNumber = quotationSeq;
    //                        _context.Sequences.Update(invSeq);
    //                    }
    //                    await _context.SaveChangesAsync();

    //                    obj.QuotationNumber = quotationSeq.ToString();
    //                    obj.SpQuotationNumber = string.Empty;
    //                    _context.SndTranQuotations.Update(obj);
    //                    await _context.SaveChangesAsync();


                       

    //                }
    //                var quotationsList = await _context.SndTranQuotationItems.AsNoTracking().Where(e => e.QuotationId == obj.Id).ToListAsync();

    //                if (quotationsList.Count > 0)
    //                {
    //                    foreach ( var quot in quotationsList)
    //                    {
    //                        quot.QuotationNumber = quotationSeq.ToString();
    //                    }


                       
    //                    _context.SndTranQuotationItems.UpdateRange(quotationsList);
    //                    await _context.SaveChangesAsync();
    //                }
    //                await transaction.CommitAsync();
    //                return true;
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in CreateSndQuotationApproval Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return false;
    //            }
    //        }
    //    }
    //}
    //#endregion


    #region CancelSndQuotation
    public class CancelSndQuotation : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public long id { get; set; }
    }
    public class CancelSndQuotationQueryHandler : IRequestHandler<CancelSndQuotation, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CancelSndQuotationQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CancelSndQuotation request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info CancelSndQuotation method start----");

                var Quotation = await _context.SndTranQuotations.FirstOrDefaultAsync(e => e.Id == request.id);
                if (Quotation == null)
                    return -1;

                var QuotationsList = await _context.SndTranQuotations.Where(e => e.OriginalQuotationId == Quotation.OriginalQuotationId).ToListAsync();

                QuotationsList.ForEach(quot=> {

                    quot.IsVoid = true;
                    quot.UpdatedBy = request.User.UserId;
                    quot.UpdatedOn = DateTime.UtcNow;

                });

                _context.SndTranQuotations.UpdateRange(QuotationsList);
                await _context.SaveChangesAsync();
                return request.id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CancelSndQuotation Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;

            }
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }
    #endregion


    #region QuotationStockAvailabilty
    public class QuotationStockAvailabilty : UserIdentityDto, IRequest<List<ItemStockAvailabilityDto>>
    {
        public UserIdentityDto User { get; set; }
        public InputQuotationStockAvailabilityDto inputDto { get; set; }
    }
    public class QuotationStockAvailabiltyQueryHandler : IRequestHandler<QuotationStockAvailabilty, List<ItemStockAvailabilityDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public QuotationStockAvailabiltyQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ItemStockAvailabilityDto>> Handle(QuotationStockAvailabilty request, CancellationToken cancellationToken)
        {

            try
            {
                List<ItemStockAvailabilityDto> Res = new();
                foreach (var Item in request.inputDto.ItemList) {
                    ItemStockAvailabilityDto resItem = new();
                    resItem.ItemCode = Item.ItemCode;
                    resItem.Quantity = Item.Quantity.Value;
                    resItem.UnitType = Item.UnitType;
                    resItem.IUM = await _context.InvItemsUOM.AsNoTracking().ProjectTo<TblErpInvItemsUOMDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.ItemCode == Item.ItemCode && e.ItemUOM == Item.UnitType);
                    resItem.StockDetails = await _context.InvItemInventory.AsNoTracking().ProjectTo<TblErpInvItemInventoryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e=>e.ItemCode==Item.ItemCode &&e.WHCode==request.inputDto.WarehouseCode);
                    resItem.StockStatus = 0;
                   
                    Res.Add(resItem);
                }



                List<ItemStockAvailabilityDto> GroupByItemCodeRes = new();

               var ItemCodes = Res.GroupBy(e => e.ItemCode);
                foreach (var ItemCode in ItemCodes)
                {
                    
                    ItemStockAvailabilityDto groupByItem = new();
                    groupByItem.StockDetails = new();
                    groupByItem.ItemCode = ItemCode.Key;
                    groupByItem.Quantity = 0;
                    groupByItem.StockDetails.QtyOH = 0;
                    groupByItem.StockDetails.QtyOnSalesOrder = 0;
                    groupByItem.StockDetails.QtyReserved = 0;
                    groupByItem.ItemMaster = await _context.InvItemMaster.AsNoTracking().ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e=>e.ItemCode==ItemCode.Key);
                    foreach (var item in Res.Where(e => e.ItemCode == ItemCode.Key)) {
                        groupByItem.Quantity += (item.Quantity*item.IUM.ItemConvFactor);
                        groupByItem.StockDetails.QtyOH += item.StockDetails is not null?item.StockDetails.QtyOH:0;
                        groupByItem.StockDetails.QtyOnSalesOrder+= item.StockDetails is not null?item.StockDetails.QtyOnSalesOrder:0;
                        groupByItem.StockDetails.QtyReserved += item.StockDetails is not null? item.StockDetails.QtyReserved:0;
                    } ;

                    
                        groupByItem.AvailableQuantity = groupByItem.StockDetails.QtyOH - groupByItem.StockDetails.QtyOnSalesOrder - groupByItem.StockDetails.QtyReserved;
                        groupByItem.StockStatus =groupByItem.AvailableQuantity-groupByItem.Quantity;
                    
                    GroupByItemCodeRes.Add(groupByItem);
                }
                    Log.Info("----Info QuotationStockAvailabilty method start----");


                return GroupByItemCodeRes;
            }
            catch (Exception ex)
            {
                Log.Error("Error in QuotationStockAvailabilty Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;

            }
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }
    #endregion
}
