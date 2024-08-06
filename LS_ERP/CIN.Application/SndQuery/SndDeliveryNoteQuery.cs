using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.SndDtos;
using CIN.Application.SndDtos.Comman;
using CIN.DB;
using CIN.Domain;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SndQuotationSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using static CIN.Application.SndDtos.SndDeliveryNoteDto;

namespace CIN.Application.SndQuery
{



    #region GetSingleSndDeliveryNoteById

    public class GetSingleSndDeliveryNoteById : IRequest<TblSndDeliveryNoteDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetSingleSndDeliveryNoteByIdHandler : IRequestHandler<GetSingleSndDeliveryNoteById, TblSndDeliveryNoteDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSndDeliveryNoteByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSndDeliveryNoteDto> Handle(GetSingleSndDeliveryNoteById request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetSingleSndDeliveryNoteById method start----");
            try
            {
                var deliveryNote = await _context.DeliveryNoteHeaders.Include(e => e.SysWarehouse).ThenInclude(e => e.SysCompanyBranch).ThenInclude(e => e.SysCompany)
                       .FirstOrDefaultAsync(e => e.Id == request.Id);

                var items = await _context.DeliveryNoteLines
                    .Where(e => e.DeliveryNoteId == request.Id)

                    .ToListAsync();



                var productUnitTypes = _context.TranProducts.Include(e => e.UnitType).AsNoTracking()
                    .Select(e => new { e.Id, pNameEN = e.NameEN, uNameEN = e.UnitType.NameEN });







                TblSndDeliveryNoteDto deliveryNoteDto = new()
                {
                    Id= deliveryNote.Id,
                    CustomerId = deliveryNote.CustomerId,
                    QuotationDate = deliveryNote.QuotationDate,
                    QuotationDueDate = deliveryNote.QuotationDueDate,
                    QuotationHeadId=deliveryNote.QuotationHeadId,
                    AmountDue=deliveryNote.AmountDue,
                    CompanyId = deliveryNote.CompanyId,
                    WarehouseCode = deliveryNote.SysWarehouse.WHCode,
                    QuotationRefNumber = deliveryNote.QuotationRefNumber,
                    LpoContract = deliveryNote.LpoContract,
                    PaymentTermId = deliveryNote.PaymentTermId,
                    QuotationNumber = deliveryNote.QuotationNumber,
                    ServiceDate1 = deliveryNote.ServiceDate1,
                    OriginalQuotationId = deliveryNote.OriginalQuotationId,
                    RevisedNumber = deliveryNote.RevisedNumber,
                    SubTotal = deliveryNote.SubTotal,
                    TaxAmount = deliveryNote.TaxAmount,
                    TotalAmount = deliveryNote.TotalAmount,
                    DiscountAmount = deliveryNote.DiscountAmount,
                    PaidAmount = 0,
                    CreatedOn = deliveryNote.CreatedOn,
                    TaxIdNumber = deliveryNote.TaxIdNumber,
                    QuotationNotes = deliveryNote.QuotationNotes,
                    Remarks = deliveryNote.Remarks,
                    CustName = deliveryNote.CustName,
                    CustArbName = deliveryNote.CustArbName,
                    CustomerName = deliveryNote.CustName,
                    LogoImagePath = deliveryNote.SysWarehouse.SysCompanyBranch.SysCompany.LogoURL,
                    FooterDiscount = deliveryNote.FooterDiscount,
                    IsClosed=deliveryNote.IsClosed,
                    IsConvertedDeliveryNoteToInvoice=deliveryNote.IsConvertedDeliveryNoteToInvoice,
                    IsConvertedFromQuotation=deliveryNote.IsConvertedFromQuotation,
                     DeliveryNumber=deliveryNote.DeliveryNumber,
                       IsCovertedFromOrder=deliveryNote.IsCovertedFromOrder,
                       
                        
                };

                List<TblSndDeliveryNoteLineDto> itemList = new();

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

                    TblSndDeliveryNoteLineDto itemDto = new()
                    {
                        Id=item.Id,
                        ItemId = Item.Id,

                        ItemCode = item.ItemCode,
                        ItemName = pNameEN,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        UnitType = item.UnitType,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        DiscountAmount = item.DiscountAmount,

                        TaxTariffPercentage = item.TaxTariffPercentage,
                        TaxAmount = item.TaxAmount,
                        TotalAmount = item.TotalAmount,


                        Delivered=item.Delivered,
                        Delivery=item.Delivery,
                        BackOrder=item.BackOrder,
                         DelvFlg1=item.DelvFlg1,
                          DelvFlg2=item.DelvFlg2,
                           DeliveryNoteId=item.DeliveryNoteId,
                            QuotationId=item.QuotationId,
                             QuotationNumber=item.QuotationNumber,
                              SubTotal=item.SubTotal,
                               AmountBeforeTax=item.AmountBeforeTax,

                        
                    };

                    itemList.Add(itemDto);
                }



                deliveryNoteDto.ItemList = itemList;

                Log.Info("----Info GetSingleSndDeliveryNoteById method Exit----");
                return deliveryNoteDto;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSingleSndDeliveryNoteById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;

            }
        }

    }

    #endregion








    #region GenerateSndDeliveryNoteByQuotationId

    public class GenerateSndDeliveryNoteByQuotationId : UserIdentityDto, IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }
    public class GenerateSndDeliveryNoteByQuotationIdQueryHandler : IRequestHandler<GenerateSndDeliveryNoteByQuotationId, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GenerateSndDeliveryNoteByQuotationIdQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateUpadteResultDto> Handle(GenerateSndDeliveryNoteByQuotationId request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    Log.Info("----Info CanGenerateSndDeliveryNoteByQuotationId method start----");
                    var QuotationHeader = await _context.SndTranQuotations.Include(e=>e.SysWarehouse).AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id && !e.IsVoid);
                    if (QuotationHeader == null)
                    {
                        return new() { ErrorId = -1, IsSuccess = false, ErrorMsg = "Invalid Quotation Id" };
                    }
                    if (QuotationHeader.IsConvertedSndQuotationToDeliveryNote == true)
                    {
                        return new() { ErrorId = -2, IsSuccess = false, ErrorMsg = "Quotation Already Converted To Delivery Note" };
                    }
                    if (!QuotationHeader.IsFinalRevision)
                    {
                        return new() { IsSuccess = false, ErrorId = -3, ErrorMsg = "Its Not a Final Quoatation" };
                    }
                
                    List<TblSndTranQuotationItem> QuotationItems = await _context.SndTranQuotationItems.Where(e=>e.QuotationId==request.Id).ToListAsync();
                    if (QuotationItems.Count==0)
                    {
                        return new() { IsSuccess=false,ErrorId=-5,ErrorMsg="NO Quotation Items Found"};
                    }








                    #region generating DeliveryNoteSequenceNumber
                    int sequenceNumber = 0;
                        var invSeq = await _context.Sequences.FirstOrDefaultAsync(e => e.BranchCode == QuotationHeader.SysWarehouse.WHBranchCode);
                        if (invSeq is null)
                        {
                            sequenceNumber = 1;
                            TblSequenceNumberSetting setting = new()
                            {
                                SDDeliveryNumber = sequenceNumber,
                                BranchCode = QuotationHeader.SysWarehouse.WHBranchCode,
                            };
                            await _context.Sequences.AddAsync(setting);
                        await _context.SaveChangesAsync();
                    }
                        else
                        {
                            sequenceNumber = invSeq.SDDeliveryNumber + 1;
                            invSeq.SDDeliveryNumber = sequenceNumber;
                            _context.Sequences.Update(invSeq);
                        await _context.SaveChangesAsync();
                    }
                        

                    #endregion

                    TblSndDeliveryNoteHeader DelvNoteHeader = new() {
                        WarehouseCode = QuotationHeader.WarehouseCode,
                        AmountBeforeTax = QuotationHeader.AmountBeforeTax,
                        AmountDue = QuotationHeader.AmountDue,
                        CompanyId = QuotationHeader.CompanyId,
                        ConvertedBy = request.User.UserId,
                        ConvertedDate = DateTime.UtcNow,
                        CreatedBy = request.User.UserId,
                        CreatedOn = DateTime.UtcNow,
                        CurrencyId = QuotationHeader.CurrencyId,
                        CustArbName = QuotationHeader.CustArbName,
                        CustName = QuotationHeader.CustName,
                        CustomerId = QuotationHeader.CustomerId,
                        DiscountAmount = QuotationHeader.DiscountAmount,
                        FooterDiscount = QuotationHeader.FooterDiscount,
                        IsApproved = false,
                       
                        IsDefaultConfig = QuotationHeader.IsDefaultConfig,
                        IsVoid = false,
                        LpoContract = QuotationHeader.LpoContract,
                        OriginalQuotationId = QuotationHeader.OriginalQuotationId,
                        PaymentTermId = QuotationHeader.PaymentTermId,
                        QuotationDate = QuotationHeader.QuotationDate,
                        QuotationDueDate = QuotationHeader.QuotationDueDate,
                        QuotationHeadId = QuotationHeader.Id,
                        QuotationModule = QuotationHeader.QuotationModule,
                        QuotationNotes = QuotationHeader.QuotationNotes,
                        QuotationNumber = QuotationHeader.QuotationNumber,
                        QuotationRefNumber = QuotationHeader.QuotationRefNumber,
                        QuotationStatus = QuotationHeader.QuotationStatus,
                        SpQuotationNumber = QuotationHeader.SpQuotationNumber,
                        Remarks = "Converted Quotation To Delivery Note",
                        RevisedNumber = QuotationHeader.RevisedNumber,
                        ServiceDate1 = QuotationHeader.ServiceDate1,
                        SubTotal = QuotationHeader.SubTotal,
                        TaxAmount = QuotationHeader.TaxAmount,
                        TaxIdNumber = QuotationHeader.TaxIdNumber,
                        UpdatedBy = 0,
                        TotalPayment = QuotationHeader.TotalPayment,
                        VatPercentage = QuotationHeader.VatPercentage,

                        DeliveryNumber = sequenceNumber.ToString(),
                        IsConvertedFromQuotation = true,
                        IsCovertedFromOrder = false,
                        IsConvertedDeliveryNoteToInvoice = false,

                        IsClosed = false,
                         TotalAmount=QuotationHeader.TotalAmount,
                    };
                    await _context.DeliveryNoteHeaders.AddAsync(DelvNoteHeader);
                    await _context.SaveChangesAsync();


                 List<TblSndDeliveryNoteLine> DeliveryNoteLines = new();
                    foreach (var Item in QuotationItems) {
                        TblSndDeliveryNoteLine DeliveryNoteLine = new() {
                            QuotationNumber = Item.QuotationNumber,
                            DiscountAmount = Item.DiscountAmount,
                            Discount = Item.Discount,
                            Description = Item.Description,
                            IsDefaultConfig = Item.IsDefaultConfig,
                            ItemCode = Item.ItemCode,
                            Quantity = Item.Quantity,
                            DebitMemoId = Item.DebitMemoId,
                            QuotationId = Item.QuotationId,
                            QuotationType = Item.QuotationType,
                            TaxAmount = Item.TaxAmount,
                            TotalAmount = Item.TotalAmount,
                            TaxTariffPercentage = Item.TaxTariffPercentage,
                            SubTotal = Item.SubTotal,
                            UnitPrice = Item.UnitPrice,
                            UnitType = Item.UnitType,
                            AmountBeforeTax = Item.AmountBeforeTax,
                            CreatedBy = request.User.UserId,
                            Remarks = DelvNoteHeader.Remarks,
                             CreatedOn=DateTime.UtcNow,
                             CreditMemoId=Item.CreditMemoId,
                              BackOrder=0,
                               Delivered=0,
                               Delivery=Item.Quantity.Value,
                                
                               DeliveryNoteId= DelvNoteHeader.Id, 
                            DelvFlg1 =false,
                                  DelvFlg2=false,
                                   
                        
                        };
                        DeliveryNoteLines.Add(DeliveryNoteLine);

                    }


                    await _context.DeliveryNoteLines.AddRangeAsync(DeliveryNoteLines);
                    await _context.SaveChangesAsync();


                    QuotationHeader.IsConvertedSndQuotationToDeliveryNote = true;
                    QuotationHeader.UpdatedBy = request.User.UserId;
                    QuotationHeader.UpdatedOn = DateTime.UtcNow;

                    _context.SndTranQuotations.Update(QuotationHeader);
                    await _context.SaveChangesAsync();





                    await transaction.CommitAsync();
                    return new() { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in GenerateSndDeliveryNoteByQuotationId Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { ErrorId = 0, IsSuccess = false, ErrorMsg = "Failed" };
                }
            }
        }
    }

    #endregion

    #region GetSndDeliveryNotePagedList

    public class GetSndDeliveryNotePagedList : IRequest<PaginatedList<TblSndDeliveryNotePagedListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSndDeliveryNotePagedListHandler : IRequestHandler<GetSndDeliveryNotePagedList, PaginatedList<TblSndDeliveryNotePagedListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndDeliveryNotePagedListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblSndDeliveryNotePagedListDto>> Handle(GetSndDeliveryNotePagedList request, CancellationToken cancellationToken)
        {
            try
            {



                Log.Info("----Info GetSndDeliveryNotePagedList method start----");

                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
                var deliveryNotes = _context.DeliveryNoteHeaders.AsNoTracking();//.Where(e => e.BranchCode == request.User.BranchCode);
                var companies = _context.Companies.AsNoTracking();
                var customers = _context.OprCustomers.AsNoTracking();


                //var cDeliveryNotes = _context.TrnCustomerDeliveryNotes.AsNoTracking();


                var authorities = await _context.TblSndAuthoritiesList.ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).ToListAsync();
                bool isArab = request.User.Culture.IsArab();


                if (request.Input.Id > 0)
                {
                    var invoiceItems = _context.DeliveryNoteLines.AsNoTracking();
                    var invoiceIds = invoiceItems.Where(e => e.ItemCode == request.Input.Id.ToString()).Select(e => e.DeliveryNoteId);

                    deliveryNotes = deliveryNotes.Where(e => invoiceIds.Any(id => id == e.Id));
                }

                bool isDate = DateTime.TryParse(request.Input.Query, out DateTime invoiceDate);
                var reports = from IV in deliveryNotes
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
                         IV.DeliveryNumber.Contains(request.Input.Query) ||
                         //IV.TotalAmount.ToString().Contains(request.Input.Query.Replace(",", "")) ||
                         (isDate && IV.QuotationDate == invoiceDate)
                              )
                              select new TblSndDeliveryNotePagedListDto
                              {
                                  Id = IV.Id,
                                  //SpQuotationNumber = IV.SpQuotationNumber,
                                  QuotationNumber = IV.QuotationNumber == string.Empty ? IV.SpQuotationNumber : IV.QuotationNumber,
                                  SpQuotationNumber = IV.SpQuotationNumber,
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

                                  OriginalQuotationId = IV.OriginalQuotationId,
                                  RevisedNumber = IV.RevisedNumber,
                                ConvertedDate=IV.ConvertedDate,
                                 DeliveryNumber=IV.IsConvertedFromQuotation?"Q"+IV.DeliveryNumber:IV.IsCovertedFromOrder?"O"+IV.DeliveryNumber:IV.DeliveryNumber,
                                 IsVoid = IV.IsVoid,
                                  IsClosed=IV.IsClosed,

                                  IsConvertedDeliveryNoteToInvoice=IV.IsConvertedDeliveryNoteToInvoice,
                                   QuotationHeadId=IV.QuotationHeadId,
                                    IsConvertedFromQuotation=IV.IsConvertedFromQuotation,
                                    IsCovertedFromOrder=IV.IsCovertedFromOrder,

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
                        "open" => reports.Where(e => (!e.IsClosed && !e.IsVoid && !e.IsConvertedDeliveryNoteToInvoice)),
                        "closed" => reports.Where(e =>( e.IsClosed && !e.IsVoid)),
                        "convertedToInvoice"=>reports.Where(e=>e.IsConvertedDeliveryNoteToInvoice),
                        "cancelled" => reports.Where(e => e.IsVoid),
                        _ => reports.Where(e => !e.IsVoid)
                    };
                }
                else
                {
                    reports = reports.Where(e => !e.IsVoid);
                }
                var nreports = await reports.OrderBy(request.Input.OrderBy).Where(e => e.HasAuthority || isAdmin).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetSndDeliveryNotePagedList method Exit----");
                return nreports;
            }
            catch (Exception ex)
            {


                Log.Error("Error in GetSndDeliveryNotePagedList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }

    }

    #endregion


    #region CancelSndDeliveryNote
    public class CancelSndDeliveryNote : UserIdentityDto, IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public long id { get; set; }
    }
    public class CancelSndDeliveryNoteQueryHandler : IRequestHandler<CancelSndDeliveryNote, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CancelSndDeliveryNoteQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CancelSndDeliveryNote request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info CancelSndDeliveryNote method start----");

                var DeliveryNote = await _context.DeliveryNoteHeaders.FirstOrDefaultAsync(e => e.Id == request.id);
                if (DeliveryNote == null)
                    return -1;

             

                    DeliveryNote.IsVoid = true;
                    DeliveryNote.UpdatedBy = request.User.UserId;
                    DeliveryNote.UpdatedOn = DateTime.UtcNow;


                _context.DeliveryNoteHeaders.Update(DeliveryNote);
                await _context.SaveChangesAsync();
                return request.id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CancelSndDeliveryNote Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;

            }
        }

        bool IsNotCreditPay(string PaymentType) => Utility.IsNotCreditPay(PaymentType);
    }
    #endregion




    #region UpdateSndDeliveryNoteLineDeliveryQty

    public class UpdateSndDeliveryNoteLineDeliveryQty : UserIdentityDto, IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDeliveryNoteLineDto InputDto { get; set; }
    }
    public class UpdateSndDeliveryNoteLineDeliveryQtyQueryHandler : IRequestHandler<UpdateSndDeliveryNoteLineDeliveryQty, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UpdateSndDeliveryNoteLineDeliveryQtyQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateUpadteResultDto> Handle(UpdateSndDeliveryNoteLineDeliveryQty request, CancellationToken cancellationToken)
        {
            try
                {

                    Log.Info("----Info UpdateSndDeliveryNoteLineDeliveryQty method start----");

                TblSndDeliveryNoteLine obj = new();

                if (request.InputDto.Id>0)
                {
                    obj = await _context.DeliveryNoteLines.FirstOrDefaultAsync(e=>e.Id==request.InputDto.Id);
                }
                if (obj is not null) {
                    obj.Delivery = request.InputDto.Delivery;
                    obj.BackOrder = obj.Quantity.Value - obj.Delivery;
                    _context.DeliveryNoteLines.Update(obj);
                   await _context.SaveChangesAsync();
                
                }
                    return new() { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    Log.Error("Error in UpdateSndDeliveryNoteLineDeliveryQty Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { ErrorId = 0, IsSuccess = false, ErrorMsg = ex.Message };
                }
            
        }
    }

    #endregion
}
