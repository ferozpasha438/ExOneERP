using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.SchoolMgt;
using CIN.Application.SchoolMgtDto;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.SalesSetup;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;

namespace CIN.Application.SchoolMgtQuery
{

    #region GetAll
    public class GetStudentFeeTransactionList : IRequest<PaginatedList<TranFeeTransactionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetStudentFeeTransactionListHandler : IRequestHandler<GetStudentFeeTransactionList, PaginatedList<TranFeeTransactionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentFeeTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TranFeeTransactionDto>> Handle(GetStudentFeeTransactionList request, CancellationToken cancellationToken)
        {

            var list = await _context.FeeTransaction.AsNoTracking().ProjectTo<TranFeeTransactionDto>(_mapper.ConfigurationProvider)
                                    .OrderByDescending(x=>x.Id)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);


            foreach (var item in list.Items)
            {
                item.TranInvoiceId = _context.TranInvoices.FirstOrDefault(x => x.InvoiceNumber == item.ReceiptVoucher)?.Id;
            }
            return list;
        }


    }


    #endregion

    #region GetFeePaymentHistory
    public class GetOnlineFeePaymentHistory : IRequest<List<TblTranFeeTransactionDto>>
    {
        public UserIdentityDto User { get; set; }
        public string AddmissionNumber { get; set; }

    }

    public class GetOnlineFeePaymentHistoryHandler : IRequestHandler<GetOnlineFeePaymentHistory, List<TblTranFeeTransactionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetOnlineFeePaymentHistoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<List<TblTranFeeTransactionDto>> Handle(GetOnlineFeePaymentHistory request, CancellationToken cancellationToken)
        {
            var list = await _context.FeeTransaction.AsNoTracking().ProjectTo<TblTranFeeTransactionDto>(_mapper.ConfigurationProvider).Where(x => (x.IsPaid == true && x.PaidOnline == true) && x.AdmissionNumber == request.AddmissionNumber).ToListAsync();
            return list;
        }
    }

    #endregion

    #region Online Fee Payment

    public class CreateStudentOnlinePayment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public OnlineFeeTransactionDto Input { get; set; }
    }

    public class CreateStudentOnlinePaymentHandler : IRequestHandler<CreateStudentOnlinePayment, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateStudentOnlinePaymentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<int> Handle(CreateStudentOnlinePayment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int feeTranID = 0;
                    string feeVoucherNum = string.Empty;
                    Log.Info("----Info Create Update Online Student Fee Transaction Method Start-----");
                    var obj = request.Input;
                    int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                    var branchDetails = await _context.SchoolBranches.AsNoTracking().
                                                      FirstOrDefaultAsync(x => x.BranchCode == obj.BranchCode);
                    if (branchDetails != null)
                    {
                        feeVoucherNum = acadamicYear.ToString().Substring(2, 2) + Convert.ToString(branchDetails.NextFeeVoucherNum);
                    }

                    var stuTermHeaderDetails = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == obj.StudNum && x.TermCode == obj.TermCode);
                    var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().FirstOrDefaultAsync(x => x.TermCode == obj.TermCode);


                    if (stuTermHeaderDetails != null && termDetails != null)
                    {
                        TblTranFeeTransaction feeTransaction = new();
                        feeTransaction.AdmissionNumber = obj.StudNum;
                        feeTransaction.ReceiptVoucher = feeVoucherNum;
                        feeTransaction.FeeDate = DateTime.Now;
                        feeTransaction.FeeTerm = termDetails.TermName;
                        feeTransaction.FeeStructCode = stuTermHeaderDetails.FeeStructCode;
                        feeTransaction.TermCode = obj.TermCode;
                        feeTransaction.FeeDueDate = stuTermHeaderDetails.FeeDueDate;
                        feeTransaction.TotFeeAmount = stuTermHeaderDetails.TotFeeAmount;
                        feeTransaction.DiscAmount = stuTermHeaderDetails.DiscAmount;
                        feeTransaction.NetFeeAmount = stuTermHeaderDetails.NetFeeAmount;
                        feeTransaction.DiscReason = string.Empty;
                        feeTransaction.IsPaid = true;
                        feeTransaction.PaidDate = DateTime.Now;
                        feeTransaction.PaidTransNum = string.Empty;
                        feeTransaction.PaidRemarks1 = string.Empty;
                        feeTransaction.PaidRemarks2 = string.Empty;
                        feeTransaction.JvNumber = string.Empty;
                        feeTransaction.InvNumber = string.Empty;
                        feeTransaction.PaidOnline = true;
                        feeTransaction.PaidManual = false;
                        feeTransaction.PayCode = "Online";
                        feeTransaction.ReceivedByUser = Convert.ToString(request.User.UserId);
                        feeTransaction.AcademicYear = Convert.ToString(acadamicYear);
                        await _context.FeeTransaction.AddAsync(feeTransaction);
                        await _context.SaveChangesAsync();

                        stuTermHeaderDetails.IsPaid = true;
                        stuTermHeaderDetails.PaidOn = DateTime.Now;
                        _context.DefStudentFeeHeader.Update(stuTermHeaderDetails);
                        await _context.SaveChangesAsync();
                        List<TblDefStudentFeeDetails> feeDetailList = new();
                        feeDetailList = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == obj.StudNum && e.TermCode == obj.TermCode).ToListAsync();
                        foreach (var feeDetail in feeDetailList)
                        {
                            feeDetail.IsPaid = true;
                            feeDetail.AddedOn = DateTime.Now;
                            _context.DefStudentFeeDetails.Update(feeDetail);
                            await _context.SaveChangesAsync();
                        }
                        feeTranID = feeTransaction.Id;
                    }

                    branchDetails.NextFeeVoucherNum = branchDetails.NextFeeVoucherNum + 1;
                    _context.SchoolBranches.Update(branchDetails);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info Create Update Online Fee Transaction  Method Exit----");
                    await transaction.CommitAsync();
                    return feeTranID;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Update  Online Fee Transaction Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region Create_Update
    public class CreateUpdateStudentFeeTransaction : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblTranFeeTransactionDto Input { get; set; }
    }
    public class CreateUpdateStudentFeeTransactionHandler : IRequestHandler<CreateUpdateStudentFeeTransaction, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentFeeTransactionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentFeeTransaction request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info Create Update Student Fee Transaction Method Start-----");
                    var obj = request.Input;
                    TblTranFeeTransaction feeTransaction = new();
                    if (obj.Id > 0)
                        feeTransaction = await _context.FeeTransaction.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    feeTransaction.Id = obj.Id;
                    feeTransaction.AdmissionNumber = obj.AdmissionNumber;
                    feeTransaction.ReceiptVoucher = obj.ReceiptVoucher;
                    feeTransaction.FeeDate = obj.FeeDate;
                    feeTransaction.FeeTerm = obj.FeeTerm;
                    feeTransaction.FeeStructCode = obj.FeeStructCode;
                    feeTransaction.TermCode = obj.TermCode;
                    feeTransaction.FeeDueDate = obj.FeeDueDate;
                    feeTransaction.TotFeeAmount = obj.TotFeeAmount;
                    feeTransaction.DiscAmount = obj.DiscAmount;
                    feeTransaction.NetFeeAmount = obj.NetFeeAmount;
                    feeTransaction.DiscReason = obj.DiscReason;
                    feeTransaction.IsPaid = obj.IsPaid;
                    feeTransaction.PaidDate = obj.PaidDate;
                    feeTransaction.PaidTransNum = obj.PaidTransNum;
                    feeTransaction.PaidRemarks1 = obj.PaidRemarks1;
                    feeTransaction.PaidRemarks2 = obj.PaidRemarks2;
                    feeTransaction.JvNumber = obj.JvNumber;
                    feeTransaction.InvNumber = obj.InvNumber;
                    feeTransaction.PaidOnline = obj.PaidOnline;
                    feeTransaction.PaidManual = obj.PaidManual;
                    feeTransaction.PayCode = obj.PayCode;
                    feeTransaction.ReceivedByUser = obj.ReceivedByUser;
                    feeTransaction.AcademicYear = obj.AcademicYear;
                    if (obj.Id > 0)
                    {
                        _context.FeeTransaction.Update(feeTransaction);
                    }
                    else
                    {
                        await _context.FeeTransaction.AddAsync(feeTransaction);
                    }
                    await _context.SaveChangesAsync();

                    if (feeTransaction.AdmissionNumber != null)
                    {
                        TblDefStudentFeeHeader feeHeaderObj = new();
                        feeHeaderObj = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == obj.AdmissionNumber && e.TermCode == obj.TermCode);
                        feeHeaderObj.IsPaid = true;
                        feeHeaderObj.PaidOn = DateTime.Now;
                        _context.DefStudentFeeHeader.Update(feeHeaderObj);
                        await _context.SaveChangesAsync();
                    }
                    if (feeTransaction.AdmissionNumber != null)
                    {
                        List<TblDefStudentFeeDetails> feeDetailList = new();

                        feeDetailList = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == obj.AdmissionNumber && e.TermCode == obj.TermCode).ToListAsync();
                        foreach (var feeDetail in feeDetailList)
                        {
                            feeDetail.IsPaid = true;
                            feeDetail.AddedOn = DateTime.Now;
                            _context.DefStudentFeeDetails.Update(feeDetail);
                            await _context.SaveChangesAsync();
                        }
                    }
                    Log.Info("----Info Create Update Fee Transaction  Method Exit----");
                    await transaction.CommitAsync();
                    return feeTransaction.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Update Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }


    #endregion

    #region Create_Update
    public class CreateFeeTransaction : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public StuFeeTransactionDto Input { get; set; }
    }
    public class CreateFeeTransactionHandler : IRequestHandler<CreateFeeTransaction, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateFeeTransactionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateFeeTransaction request, CancellationToken cancellationToken)
        {
            bool isPaidPartial = false;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var createdOn = DateTime.Now;
                    long invoiceId = 0;
                    int feeTranID = 0;
                    string feeVoucherNum = string.Empty;

                    decimal totalAmount = 0;
                    decimal totalNetPayment = 0;
                    decimal amountDue = 0;

                    Log.Info("----Info Create Update Student Fee Transaction Method Start-----");
                    var obj = request.Input;
                    int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                    var branchDetails = await _context.SchoolBranches.AsNoTracking().
                                                      FirstOrDefaultAsync(x => x.BranchCode == obj.BranchCode);
                    if (branchDetails != null)
                    {
                        feeVoucherNum = acadamicYear.ToString().Substring(2, 2) + Convert.ToString(branchDetails.NextFeeVoucherNum);
                    }
                    TblErpSysSystemTax taxData = new();
                    #region ARInvoice
                    var studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == obj.AdmissionNumber);
                    if (studentDetails.TaxApplicable)
                    {
                        await _context.SystemTaxes.FirstOrDefaultAsync();
                    }
                    else
                    {
                        taxData = null;
                    }
                    var stuTermFeeDetails = await _context.DefStudentFeeHeader.AsNoTracking()
                                                           .FirstOrDefaultAsync(x => x.StuAdmNum == obj.AdmissionNumber
                                                                  && x.TermCode == obj.TermCode);


                    if (studentDetails != null && stuTermFeeDetails != null)
                    {
                        if ((stuTermFeeDetails.PaidAmount + request.Input.PaidAmount) != stuTermFeeDetails.NetFeeAmount)
                        {
                            isPaidPartial = true;
                            stuTermFeeDetails.IsPartiallyPaid = true;
                            stuTermFeeDetails.IsCompletelyPaid = false;
                            stuTermFeeDetails.IsPaid = false;
                        }
                        else
                        {
                            stuTermFeeDetails.IsPaid = true;
                            stuTermFeeDetails.IsCompletelyPaid = true;
                            stuTermFeeDetails.IsPartiallyPaid = false;
                        }


                        totalAmount = !isPaidPartial ? (stuTermFeeDetails.TotFeeAmount - stuTermFeeDetails.PaidAmount - stuTermFeeDetails.DiscAmount) : request.Input.PaidAmount;
                        totalNetPayment = !isPaidPartial ? ((stuTermFeeDetails.TotFeeAmount - stuTermFeeDetails.PaidAmount - stuTermFeeDetails.DiscAmount) + stuTermFeeDetails.TaxAmount) : request.Input.PaidAmount;
                        amountDue = stuTermFeeDetails.NetFeeAmount - stuTermFeeDetails.PaidAmount - request.Input.PaidAmount;

                        stuTermFeeDetails.PaidAmount = stuTermFeeDetails.PaidAmount + request.Input.PaidAmount;
                        stuTermFeeDetails.DiscAmount = Convert.ToDecimal(request.Input.Discount);
                        stuTermFeeDetails.PaidOn = DateTime.Now;
                        _context.DefStudentFeeHeader.Update(stuTermFeeDetails);
                        await _context.SaveChangesAsync();


                        int jvSeq = 0;
                        var seqquence = await _context.Sequences.FirstOrDefaultAsync();
                        if (seqquence is null)
                        {
                            jvSeq = 1;
                            TblSequenceNumberSetting setting1 = new()
                            {
                                InvoiceSeq = jvSeq
                            };
                            await _context.Sequences.AddAsync(setting1);
                        }
                        else
                        {
                            jvSeq = seqquence.InvoiceSeq + 1;
                            seqquence.InvoiceSeq = jvSeq;
                            _context.Sequences.Update(seqquence);
                        }
                        await _context.SaveChangesAsync();
                        var paymentTerms = await _context.SndSalesTermsCodes.AsNoTracking().FirstOrDefaultAsync(x => x.SalesTermsCode == obj.PayCode && x.IsActive == true);

                        TblTranInvoice tranInvoice = new();
                        tranInvoice.SpInvoiceNumber = string.Empty;
                        tranInvoice.InvoiceNumber = Convert.ToString(jvSeq);
                        tranInvoice.TaxIdNumber = "-";
                        tranInvoice.InvoiceRefNumber = studentDetails.StuIDNumber;
                        tranInvoice.InvoiceDate = createdOn;
                        //tranInvoice.InvoiceDueDate = stuTermHeaderDetails.FeeDueDate;
                        tranInvoice.CurrencyId = null;
                        var companydetails = await _context.CompanyBranches.AsNoTracking().FirstOrDefaultAsync(x => x.BranchCode == studentDetails.BranchCode);
                        if (companydetails != null)
                            tranInvoice.CompanyId = companydetails.CompanyId;
                        tranInvoice.LpoContract = string.Join(",", obj.TermCode);

                        if (paymentTerms != null)
                            tranInvoice.PaymentTerms = paymentTerms.SalesTermsCode;
                        else
                            tranInvoice.PaymentTerms = "CASH";
                        TblSndDefCustomerMaster customer = await _context.OprCustomers.AsNoTracking().FirstOrDefaultAsync(x => x.CustCode == studentDetails.StuAdmNum) ?? new();
                        if (customer.CustCode is null)
                        {
                            #region tblSndDefCustomerCategory
                            TblSndDefCustomerCategory categoryData = new();
                            categoryData = await _context.SndCustomerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CustCatCode == studentDetails.GradeCode);
                            if (categoryData == null)
                            {
                                var acedemicClassGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(x => x.GradeCode == studentDetails.GradeCode);
                                if (acedemicClassGrade != null)
                                {
                                    categoryData = new();
                                    categoryData.CustCatCode = acedemicClassGrade.GradeCode;
                                    categoryData.CustCatName = acedemicClassGrade.GradeName;
                                    categoryData.CustCatDesc = acedemicClassGrade.GradeName2;
                                    categoryData.CreatedOn = DateTime.Now;
                                    categoryData.IsActive = acedemicClassGrade.IsActive;
                                    categoryData.CatPrefix = "1";
                                    categoryData.LastSeq = 0;
                                    await _context.SndCustomerCategories.AddAsync(categoryData);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            #endregion

                            #region tblSndDefCustomerMaster
                            var firstCustData = await _context.OprCustomers.AsNoTracking().FirstOrDefaultAsync();

                            customer.CustCode = studentDetails.StuAdmNum;
                            customer.CustName = studentDetails.StuName;
                            customer.CustArbName = studentDetails.StuName2;
                            customer.CustAlias = studentDetails.Alias;
                            var userTypeData = await _context.UserTypes.FirstOrDefaultAsync(x => x.UerType.ToLower() == "student");
                            customer.CustType = (short)userTypeData.Id;
                            customer.CustCatCode = studentDetails.GradeCode;
                            customer.CustRating = 1;
                            if (paymentTerms != null)
                                customer.SalesTermsCode = paymentTerms.SalesTermsCode;
                            else
                                customer.SalesTermsCode = "CASH";
                            customer.CustDiscount = 0;
                            customer.CustCrLimit = 0;
                            customer.CustSalesRep = "Admin";
                            customer.CustSalesArea = "Admin";
                            customer.CustARAc = firstCustData?.CustARAc;
                            customer.CustLastPaidDate = DateTime.Now;
                            customer.CustLastSalesDate = DateTime.Now;
                            customer.CustLastPayAmt = 0;
                            customer.CustAddress1 = studentDetails.BuildingName + ' ' + studentDetails.PAddress1;
                            customer.CustCityCode1 = studentDetails.City;
                            customer.CustMobile1 = studentDetails.Mobile;
                            customer.CustPhone1 = studentDetails.Phone;
                            customer.CustEmail1 = studentDetails.RegisteredEmail;
                            customer.CustContact1 = studentDetails.FatherName;
                            customer.CustAddress2 = studentDetails.BuildingName + ' ' + studentDetails.PAddress1;
                            customer.CustCityCode2 = studentDetails.City;
                            customer.CustMobile2 = studentDetails.Mobile;
                            customer.CustPhone2 = studentDetails.Phone;
                            customer.CustEmail2 = studentDetails.RegisteredEmail;
                            customer.CustContact2 = studentDetails.MotherName;
                            customer.CustUDF1 = "-";
                            customer.CustUDF2 = "-";
                            customer.CustUDF3 = "-";
                            customer.CustAllowCrsale = false;
                            customer.CustAlloCrOverride = false;
                            customer.CustOnHold = false;
                            customer.CustAlloChkPay = false;
                            customer.CustSetPriceLevel = false;
                            customer.CustPriceLevel = 0;
                            customer.CustIsVendor = false;
                            customer.CustArAcBranch = false;
                            customer.CustArAcCode = firstCustData?.CustArAcCode;
                            customer.CustDefExpAcCode = firstCustData?.CustDefExpAcCode;
                            customer.CustARAdjAcCode = firstCustData?.CustARAdjAcCode;
                            customer.CustARDiscAcCode = firstCustData?.CustARDiscAcCode;
                            customer.CreatedOn = DateTime.Now;
                            customer.IsActive = true;
                            customer.VATNumber = "-";
                            customer.CustOutStandBal = 0;
                            customer.CustAvailCrLimit = 0;
                            customer.CustNameAliasAr = studentDetails.StuName2;
                            customer.CustNameAliasEn = studentDetails.StuName;
                            if (customer.Id == 0)
                                await _context.OprCustomers.AddAsync(customer);
                            else
                                _context.OprCustomers.Update(customer);
                            await _context.SaveChangesAsync();
                            #endregion


                        }





                        tranInvoice.CustomerId = customer?.Id;
                        tranInvoice.BranchCode = studentDetails.BranchCode.ToUpper();
                        tranInvoice.InvoiceStatusId = 1;
                        tranInvoice.SubTotal = request.Input.PaidAmount - request.Input.Discount;  //stuTermFeeDetails.Sum(x => x.TotFeeAmount);
                        tranInvoice.DiscountAmount = request.Input.Discount; //stuTermFeeDetails.Sum(x => x.DiscAmount);
                        tranInvoice.AmountBeforeTax = request.Input.PaidAmount - request.Input.Discount;
                        tranInvoice.TaxAmount = !isPaidPartial ? stuTermFeeDetails.TaxAmount : 0;
                        tranInvoice.TotalAmount = request.Input.PaidAmount - request.Input.Discount;
                        tranInvoice.TotalPayment = 0;
                        tranInvoice.AmountDue = 0;
                        tranInvoice.IsDefaultConfig = true;
                        tranInvoice.CreatedOn = createdOn;
                        tranInvoice.CreatedBy = request.User.UserId;
                        if (taxData != null)
                            tranInvoice.VatPercentage = taxData.Taxper01;
                        else
                            tranInvoice.VatPercentage = 0;
                        tranInvoice.IsCreditConverted = false;
                        tranInvoice.InvoiceStatus = "Closed";
                        tranInvoice.InvoiceModule = "SM";
                        tranInvoice.Remarks = obj.Remarks;
                        tranInvoice.InvoiceNotes = string.Empty;
                        tranInvoice.ServiceDate1 = createdOn.ToString();
                        tranInvoice.CustArbName = studentDetails.StuName2;
                        tranInvoice.CustName = studentDetails.StuName;
                        await _context.TranInvoices.AddAsync(tranInvoice);
                        await _context.SaveChangesAsync();
                        invoiceId = tranInvoice.Id;

                        //var stuTermHeaderDetails = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == obj.AdmissionNumber && x.TermCode == obj.TermCode);
                        var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().FirstOrDefaultAsync(x => x.TermCode == obj.TermCode);
                        var product = await _context.TranProducts.FirstOrDefaultAsync(e => e.ProductCode == termDetails.TermCode);
                        if (product is null || termDetails is null)
                        {
                            product = new();
                            product.NameEN = termDetails.TermName;
                            product.NameAR = termDetails.TermName2;
                            product.ProductCode = termDetails.TermCode;
                            product.CompanyId = companydetails.CompanyId;
                            product.Description = companydetails.Remarks;
                            product.ProductTypeId = 17;
                            product.UnitPrice = 0;
                            product.CostPrice = 0;
                            product.UnitTypeId = 7;
                            product.IsDefaultConfig = true;
                            product.CreatedOn = DateTime.Now;
                            product.CreatedBy = 1;
                            await _context.TranProducts.AddAsync(product);
                            await _context.SaveChangesAsync();
                        }

                        if (stuTermFeeDetails != null && termDetails != null)
                        {
                            TblTranFeeTransaction feeTransaction = new();
                            feeTransaction.AdmissionNumber = obj.AdmissionNumber;
                            feeTransaction.ReceiptVoucher = Convert.ToString(jvSeq);
                            feeTransaction.FeeDate = DateTime.Now;
                            feeTransaction.FeeTerm = termDetails.TermName;
                            feeTransaction.FeeStructCode = stuTermFeeDetails.FeeStructCode;
                            feeTransaction.TermCode = obj.TermCode;
                            feeTransaction.FeeDueDate = stuTermFeeDetails.FeeDueDate;
                            feeTransaction.TotFeeAmount = totalNetPayment - stuTermFeeDetails.DiscAmount;
                            feeTransaction.DiscAmount = stuTermFeeDetails.DiscAmount;
                            feeTransaction.NetFeeAmount = totalNetPayment;
                            feeTransaction.DiscReason = string.Empty;
                            feeTransaction.IsPaid = true;
                            feeTransaction.PaidDate = DateTime.Now;
                            feeTransaction.PaidTransNum = string.Empty;
                            feeTransaction.PaidRemarks1 = obj.Remarks;
                            feeTransaction.PaidRemarks2 = string.Empty;
                            feeTransaction.JvNumber = string.Empty;
                            feeTransaction.InvNumber = string.Empty;
                            feeTransaction.PaidOnline = false;
                            feeTransaction.PaidManual = true;
                            feeTransaction.PayCode = obj.PayCode;
                            feeTransaction.ReceivedByUser = Convert.ToString(request.User.UserId);
                            feeTransaction.AcademicYear = Convert.ToString(acadamicYear);
                            await _context.FeeTransaction.AddAsync(feeTransaction);
                            await _context.SaveChangesAsync();

                            TblTranInvoiceItem tranInvoiceItem = new();
                            tranInvoiceItem.InvoiceNumber = Convert.ToString(jvSeq);
                            tranInvoiceItem.InvoiceId = invoiceId;
                            tranInvoiceItem.CreditMemoId = null;
                            tranInvoiceItem.DebitMemoId = null;
                            tranInvoiceItem.ProductId = product.Id;
                            tranInvoiceItem.Quantity = 1;
                            tranInvoiceItem.UnitPrice = request.Input.PaidAmount;
                            tranInvoiceItem.SubTotal = request.Input.PaidAmount;
                            tranInvoiceItem.DiscountAmount = request.Input.Discount;
                            tranInvoiceItem.AmountBeforeTax = request.Input.PaidAmount - request.Input.Discount;
                            tranInvoiceItem.TaxAmount = !isPaidPartial ? stuTermFeeDetails.TaxAmount : 0;
                            tranInvoiceItem.TotalAmount = request.Input.PaidAmount - request.Input.Discount + tranInvoiceItem.TaxAmount;
                            tranInvoiceItem.IsDefaultConfig = true;
                            tranInvoiceItem.CreatedOn = DateTime.Now;
                            tranInvoiceItem.CreatedBy = request.User.UserId;
                            tranInvoiceItem.Description = stuTermFeeDetails.FeeStructCode;
                            if (taxData != null)
                                tranInvoiceItem.TaxTariffPercentage = taxData.Taxper01;
                            else
                                tranInvoice.VatPercentage = 0;
                            tranInvoiceItem.Discount = 0;
                            tranInvoiceItem.InvoiceType = null;
                            await _context.TranInvoiceItems.AddAsync(tranInvoiceItem);
                            await _context.SaveChangesAsync();

                            //stuTermHeaderDetails.IsPaid = true;
                            //stuTermHeaderDetails.PaidOn = DateTime.Now;
                            //_context.DefStudentFeeHeader.Update(stuTermHeaderDetails);
                            //await _context.SaveChangesAsync();

                            List<TblDefStudentFeeDetails> feeDetailList = new();
                            feeDetailList = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == obj.AdmissionNumber && e.TermCode == obj.TermCode).ToListAsync();
                            foreach (var feeDetail in feeDetailList)
                            {
                                feeDetail.IsPaid = true;
                                feeDetail.AddedOn = DateTime.Now;
                                _context.DefStudentFeeDetails.Update(feeDetail);
                                await _context.SaveChangesAsync();
                            }
                            feeTranID = feeTransaction.Id;
                        }

                        TblFinTrnCustomerApproval approvalArInv = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            LoginId = request.User.UserId,
                            AppRemarks = "Automatic Approval From SM",
                            InvoiceId = tranInvoice.Id,
                            IsApproved = true,
                        };
                        await _context.TrnCustomerApprovals.AddAsync(approvalArInv);
                        await _context.SaveChangesAsync();

                        TblFinTrnCustomerInvoice cInvoice = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            InvoiceNumber = tranInvoice.InvoiceNumber,
                            InvoiceDate = tranInvoice.InvoiceDate,
                            CreditDays = 0,
                            DueDate = tranInvoice.InvoiceDueDate,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            CustCode = customer.CustCode,
                            DocNum = tranInvoice.InvoiceRefNumber,
                            LoginId = request.User.UserId,
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            InvoiceAmount = tranInvoice.TotalAmount,
                            DiscountAmount = tranInvoice.DiscountAmount ?? 0,
                            NetAmount = tranInvoice.TotalAmount,
                            PaidAmount = tranInvoice.TotalAmount,
                            AppliedAmount = 0,
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "Settled From SM",
                            InvoiceId = tranInvoice.Id,
                            IsPaid = true,
                        };
                        cInvoice.BalanceAmount = cInvoice.NetAmount - cInvoice.PaidAmount;
                        await _context.TrnCustomerInvoices.AddAsync(cInvoice);

                        TblFinTrnCustomerStatement cStatement = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            TranNumber = tranInvoice.InvoiceNumber,
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            PaymentType = paymentTerms != null ? paymentTerms.SalesTermsCode : "CASH",
                            PamentCode = "paycode",
                            CheckNumber = "",
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
                            LoginId = request.User.UserId,
                            DrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            CrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            InvoiceId = tranInvoice.Id,
                        };
                        await _context.TrnCustomerStatements.AddAsync(cStatement);

                        TblFinTrnCustomerStatement cPaymentStatement = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = "Payment",
                            TranNumber = tranInvoice.InvoiceNumber,// invoiceSeq.ToString(),
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            PaymentType = paymentTerms != null ? paymentTerms.SalesTermsCode : "CASH",
                            PamentCode = "Paycode",
                            CheckNumber = "",
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = tranInvoice.TotalAmount,
                            InvoiceId = tranInvoice.Id,
                        };
                        await _context.TrnCustomerStatements.AddAsync(cPaymentStatement);

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = tranInvoice.Id,
                            FinAcCode = customer?.CustArAcCode,
                            CrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            DrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            Source = "SM",
                            Type = "paycode",
                            Gl = string.Empty,
                            CreatedOn = createdOn
                        };

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = tranInvoice.Id,
                            FinAcCode = customer?.CustDefExpAcCode,
                            CrAmount = !tranInvoice.IsCreditConverted ? (tranInvoice.TotalAmount - tranInvoice.TaxAmount) : 0,
                            DrAmount = tranInvoice.IsCreditConverted ? (tranInvoice.TotalAmount - tranInvoice.TaxAmount) : 0,
                            Source = "SM",
                            Gl = string.Empty,
                            Type = "Expense",
                            CreatedOn = createdOn
                        };
                        await _context.FinDistributions.AddAsync(distribution1);
                        await _context.FinDistributions.AddAsync(distribution2);

                        //var invoiceItem = await _context.TranInvoiceItems.FirstOrDefaultAsync(e => e.InvoiceId == tranInvoice.Id);
                        //var tax = await _context.SystemTaxes.FirstOrDefaultAsync(e => e.TaxName == Convert.ToInt32(taxData.TaxTariffPercentage).ToString());
                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        if (taxData is not null)
                        {
                            TblFinTrnDistribution distribution3 = new()
                            {
                                InvoiceId = tranInvoice.Id,
                                FinAcCode = taxData?.OutputAcCode01,
                                CrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TaxAmount : 0,
                                DrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TaxAmount : 0,
                                Source = "SM",
                                Gl = string.Empty,
                                Type = "VAT",
                                CreatedOn = createdOn
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



                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            Batch = string.Empty,
                            Source = "SM",
                            Remarks = tranInvoice.Remarks,
                            Narration = tranInvoice.InvoiceNotes ?? tranInvoice.Remarks,
                            JvDate = createdOn,
                            Amount = tranInvoice.TotalAmount ?? 0,
                            DocNum = tranInvoice.InvoiceNumber,
                            CDate = createdOn,
                            Posted = true,
                            PostedDate = createdOn,
                            SiteCode = tranInvoice.SiteCode
                        };
                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();
                        var jvId = JV.Id;

                        var branchAuths = await _context.SysSchoolBranchesAuthority.Select(e => new { e.BranchCode, e.TeacherCode })
                            .Where(e => e.BranchCode == tranInvoice.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                var teacherDetails = await _context.DefSchoolTeacherMaster.AsNoTracking().FirstOrDefaultAsync(x => x.TeacherCode == item.TeacherCode);
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = (int)tranInvoice.CompanyId,
                                    BranchCode = tranInvoice.BranchCode,
                                    JvDate = createdOn,
                                    TranSource = "SM",
                                    Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                                    DocNum = tranInvoice.InvoiceRefNumber,
                                    LoginId = Convert.ToInt32(teacherDetails.SysLoginId),
                                    AppRemarks = tranInvoice.Remarks,
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
                                BranchCode = tranInvoice.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = tranInvoice.Remarks,
                                CrAmount = obj1.CrAmount,
                                DrAmount = obj1.DrAmount,
                                FinAcCode = obj1.FinAcCode,
                                Description = tranInvoice.InvoiceNotes,
                                CostAllocation = costallocations.Id,
                                CostSegCode = customer.CustCode,
                                SiteCode = tranInvoice.SiteCode

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

                            JvDate = createdOn,
                            TranNumber = jvSeq.ToString(),
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
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
                                TransDate = createdOn,
                                PostedFlag = true,
                                PostDate = createdOn,
                                Jvnum = item.JournalVoucherId.ToString(),
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "SM",
                                ExRate = 0,
                                SiteCode = tranInvoice.SiteCode
                            };
                            ledgerList.Add(ledger);
                        }
                        if (ledgerList.Count > 0)
                        {
                            await _context.AccountsLedgers.AddRangeAsync(ledgerList);
                            await _context.SaveChangesAsync();
                        }

                    }
                    #endregion

                    branchDetails.NextFeeVoucherNum = branchDetails.NextFeeVoucherNum + 1;
                    _context.SchoolBranches.Update(branchDetails);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info Create Update Fee Transaction  Method Exit----");
                    await transaction.CommitAsync();
                    return feeTranID;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Update Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }


    #endregion



    #region Create Bulk_Payment
    public class CreateBulkFeeTransaction : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public StuBulkFeeTransactionDto Input { get; set; }
    }
    public class CreateBulkFeeTransactionHandler : IRequestHandler<CreateBulkFeeTransaction, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateBulkFeeTransactionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateBulkFeeTransaction request, CancellationToken cancellationToken)
        {
            bool isPaidPartial = false;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    List<StudentFeePaidDto> studentFeePaidList = new();
                    var createdOn = DateTime.Now;
                    long invoiceId = 0;
                    int feeTranID = 0;
                    string feeVoucherNum = string.Empty;

                    decimal totalNetPayment = 0;

                    Log.Info("----Info Create Update Student Fee Transaction Method Start-----");
                    var obj = request.Input;
                    string termsData = string.Join(',', obj.TermCodes);
                    int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                    var branchDetails = await _context.SchoolBranches.AsNoTracking().
                                                      FirstOrDefaultAsync(x => x.BranchCode == obj.BranchCode);
                    if (branchDetails != null)
                    {
                        feeVoucherNum = acadamicYear.ToString().Substring(2, 2) + Convert.ToString(branchDetails.NextFeeVoucherNum);
                    }
                    TblErpSysSystemTax taxData = new();
                    #region ARInvoice
                    var studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == obj.AdmissionNumber);
                    if (studentDetails.TaxApplicable)
                    {
                        await _context.SystemTaxes.FirstOrDefaultAsync();
                    }
                    else
                    {
                        taxData = null;
                    }

                    var stuTermFeeDetails = await _context.DefStudentFeeHeader.AsNoTracking()
                                                           .Where(x => x.StuAdmNum == obj.AdmissionNumber
                                                                  && obj.TermCodes.Contains(x.TermCode)).ToListAsync();

                    decimal userPaidAmount = request.Input.PaidAmount;
                    decimal userPaidAmount2 = request.Input.PaidAmount;
                    if (studentDetails != null && stuTermFeeDetails != null && stuTermFeeDetails.Count > 0)
                    {
                        foreach (var stuTermFeeDetail in stuTermFeeDetails)
                        {
                            StudentFeePaidDto studentFeePaidDto = new();
                            studentFeePaidDto.Id = stuTermFeeDetail.Id;
                            studentFeePaidDto.FeeStructCode = stuTermFeeDetail.FeeStructCode;
                            studentFeePaidDto.TermCode = stuTermFeeDetail.TermCode;


                            isPaidPartial = false;
                            decimal discountAmount = 0;
                            if (userPaidAmount + stuTermFeeDetail.PaidAmount + stuTermFeeDetail.DiscAmount - stuTermFeeDetail.NetFeeAmount >= 0)
                            {
                                isPaidPartial = false;
                                stuTermFeeDetail.IsPaid = true;
                                stuTermFeeDetail.IsCompletelyPaid = true;
                                stuTermFeeDetail.IsPartiallyPaid = false;
                                discountAmount = Convert.ToDecimal(((stuTermFeeDetail.NetFeeAmount - stuTermFeeDetail.PaidAmount - stuTermFeeDetail.DiscAmount) / 100) * request.Input.DiscountPercentage);

                                studentFeePaidDto.PaidAmount = stuTermFeeDetail.NetFeeAmount - stuTermFeeDetail.PaidAmount;
                                studentFeePaidDto.DiscAmount = discountAmount;
                                studentFeePaidDto.IsPaid = true;
                                studentFeePaidDto.IsCompletelyPaid = true;

                            }
                            else
                            {
                                isPaidPartial = true;
                                stuTermFeeDetail.IsPartiallyPaid = true;
                                stuTermFeeDetail.IsCompletelyPaid = false;
                                stuTermFeeDetail.IsPaid = false;
                                discountAmount = Convert.ToDecimal(((userPaidAmount - stuTermFeeDetail.PaidAmount - stuTermFeeDetail.DiscAmount) / 100) * request.Input.DiscountPercentage);

                                studentFeePaidDto.PaidAmount = userPaidAmount;
                                studentFeePaidDto.DiscAmount = discountAmount;
                                studentFeePaidDto.IsPaid = false;
                                studentFeePaidDto.IsCompletelyPaid = false;
                            }
                            userPaidAmount = userPaidAmount - studentFeePaidDto.PaidAmount;
                            studentFeePaidList.Add(studentFeePaidDto);
                            
                            stuTermFeeDetail.PaidAmount = (stuTermFeeDetail.PaidAmount + userPaidAmount2 + stuTermFeeDetail.DiscAmount - stuTermFeeDetail.NetFeeAmount) >= 0 ? stuTermFeeDetail.NetFeeAmount - discountAmount : userPaidAmount2;
                            stuTermFeeDetail.PaidOn = DateTime.Now;
                            userPaidAmount2 = userPaidAmount;
                            stuTermFeeDetail.DiscAmount = stuTermFeeDetail.DiscAmount + Convert.ToDecimal(discountAmount);
                        }
                        _context.DefStudentFeeHeader.UpdateRange(stuTermFeeDetails);
                        await _context.SaveChangesAsync();

                        totalNetPayment = request.Input.PaidAmount;

                        int jvSeq = 0;
                        var seqquence = await _context.Sequences.FirstOrDefaultAsync();
                        if (seqquence is null)
                        {
                            jvSeq = 1;
                            TblSequenceNumberSetting setting1 = new()
                            {
                                InvoiceSeq = jvSeq
                            };
                            await _context.Sequences.AddAsync(setting1);
                        }
                        else
                        {
                            jvSeq = seqquence.InvoiceSeq + 1;
                            seqquence.InvoiceSeq = jvSeq;
                            _context.Sequences.Update(seqquence);
                        }
                        await _context.SaveChangesAsync();

                        var paymentTerms = await _context.SndSalesTermsCodes.AsNoTracking().FirstOrDefaultAsync(x => x.SalesTermsCode == obj.PayCode && x.IsActive == true);
                        TblTranInvoice tranInvoice = new();
                        tranInvoice.SpInvoiceNumber = string.Empty;
                        tranInvoice.InvoiceNumber = Convert.ToString(jvSeq);
                        tranInvoice.TaxIdNumber = "-";
                        tranInvoice.InvoiceRefNumber = studentDetails.StuIDNumber;
                        tranInvoice.InvoiceDate = createdOn;
                        //tranInvoice.InvoiceDueDate = stuTermHeaderDetails.FeeDueDate;
                        tranInvoice.CurrencyId = null;
                        var companydetails = await _context.CompanyBranches.AsNoTracking().FirstOrDefaultAsync(x => x.BranchCode == studentDetails.BranchCode);
                        if (companydetails != null)
                            tranInvoice.CompanyId = companydetails.CompanyId;
                        tranInvoice.LpoContract = termsData;

                        if (paymentTerms != null)
                            tranInvoice.PaymentTerms = paymentTerms.SalesTermsCode;
                        else
                            tranInvoice.PaymentTerms = "CASH";
                        TblSndDefCustomerMaster customer = await _context.OprCustomers.AsNoTracking().FirstOrDefaultAsync(x => x.CustCode == studentDetails.StuAdmNum) ?? new();
                        if (customer.CustCode is null)
                        {
                            #region tblSndDefCustomerCategory
                            TblSndDefCustomerCategory categoryData = new();
                            categoryData = await _context.SndCustomerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CustCatCode == studentDetails.GradeCode);
                            if (categoryData == null)
                            {
                                var acedemicClassGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(x => x.GradeCode == studentDetails.GradeCode);
                                if (acedemicClassGrade != null)
                                {
                                    categoryData = new();
                                    categoryData.CustCatCode = acedemicClassGrade.GradeCode;
                                    categoryData.CustCatName = acedemicClassGrade.GradeName;
                                    categoryData.CustCatDesc = acedemicClassGrade.GradeName2;
                                    categoryData.CreatedOn = DateTime.Now;
                                    categoryData.IsActive = acedemicClassGrade.IsActive;
                                    categoryData.CatPrefix = "1";
                                    categoryData.LastSeq = 0;
                                    await _context.SndCustomerCategories.AddAsync(categoryData);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            #endregion

                            #region tblSndDefCustomerMaster
                            var firstCustData = await _context.OprCustomers.AsNoTracking().FirstOrDefaultAsync();

                            customer.CustCode = studentDetails.StuAdmNum;
                            customer.CustName = studentDetails.StuName;
                            customer.CustArbName = studentDetails.StuName2;
                            customer.CustAlias = studentDetails.Alias;
                            var userTypeData = await _context.UserTypes.FirstOrDefaultAsync(x => x.UerType.ToLower() == "student");
                            customer.CustType = (short)userTypeData.Id;
                            customer.CustCatCode = studentDetails.GradeCode;
                            customer.CustRating = 1;
                            if (paymentTerms != null)
                                customer.SalesTermsCode = paymentTerms.SalesTermsCode;
                            else
                                customer.SalesTermsCode = "CASH";
                            customer.CustDiscount = 0;
                            customer.CustCrLimit = 0;
                            customer.CustSalesRep = "Admin";
                            customer.CustSalesArea = "Admin";
                            customer.CustARAc = firstCustData?.CustARAc;
                            customer.CustLastPaidDate = DateTime.Now;
                            customer.CustLastSalesDate = DateTime.Now;
                            customer.CustLastPayAmt = 0;
                            customer.CustAddress1 = studentDetails.BuildingName + ' ' + studentDetails.PAddress1;
                            customer.CustCityCode1 = studentDetails.City;
                            customer.CustMobile1 = studentDetails.Mobile;
                            customer.CustPhone1 = studentDetails.Phone;
                            customer.CustEmail1 = studentDetails.RegisteredEmail;
                            customer.CustContact1 = studentDetails.FatherName;
                            customer.CustAddress2 = studentDetails.BuildingName + ' ' + studentDetails.PAddress1;
                            customer.CustCityCode2 = studentDetails.City;
                            customer.CustMobile2 = studentDetails.Mobile;
                            customer.CustPhone2 = studentDetails.Phone;
                            customer.CustEmail2 = studentDetails.RegisteredEmail;
                            customer.CustContact2 = studentDetails.MotherName;
                            customer.CustUDF1 = "-";
                            customer.CustUDF2 = "-";
                            customer.CustUDF3 = "-";
                            customer.CustAllowCrsale = false;
                            customer.CustAlloCrOverride = false;
                            customer.CustOnHold = false;
                            customer.CustAlloChkPay = false;
                            customer.CustSetPriceLevel = false;
                            customer.CustPriceLevel = 0;
                            customer.CustIsVendor = false;
                            customer.CustArAcBranch = false;
                            customer.CustArAcCode = firstCustData?.CustArAcCode;
                            customer.CustDefExpAcCode = firstCustData?.CustDefExpAcCode;
                            customer.CustARAdjAcCode = firstCustData?.CustARAdjAcCode;
                            customer.CustARDiscAcCode = firstCustData?.CustARDiscAcCode;
                            customer.CreatedOn = DateTime.Now;
                            customer.IsActive = true;
                            customer.VATNumber = "-";
                            customer.CustOutStandBal = 0;
                            customer.CustAvailCrLimit = 0;
                            customer.CustNameAliasAr = studentDetails.StuName2;
                            customer.CustNameAliasEn = studentDetails.StuName;
                            if (customer.Id == 0)
                                await _context.OprCustomers.AddAsync(customer);
                            else
                                _context.OprCustomers.Update(customer);
                            await _context.SaveChangesAsync();
                            #endregion


                        }


                        tranInvoice.CustomerId = customer?.Id;
                        tranInvoice.BranchCode = studentDetails.BranchCode.ToUpper();
                        tranInvoice.InvoiceStatusId = 1;
                        tranInvoice.SubTotal = request.Input.PaidAmount - request.Input.Discount;  //stuTermFeeDetails.Sum(x => x.TotFeeAmount);
                        tranInvoice.DiscountAmount = request.Input.Discount; //stuTermFeeDetails.Sum(x => x.DiscAmount);
                        tranInvoice.AmountBeforeTax = request.Input.PaidAmount - request.Input.Discount;
                        tranInvoice.TaxAmount = !isPaidPartial ? stuTermFeeDetails.Sum(x => x.TaxAmount) : 0;
                        tranInvoice.TotalAmount = request.Input.PaidAmount - request.Input.Discount;
                        tranInvoice.TotalPayment = 0;
                        tranInvoice.AmountDue = 0;
                        tranInvoice.IsDefaultConfig = true;
                        tranInvoice.CreatedOn = createdOn;
                        tranInvoice.CreatedBy = request.User.UserId;
                        if (taxData != null)
                            tranInvoice.VatPercentage = taxData.Taxper01;
                        else
                            tranInvoice.VatPercentage = 0;
                        tranInvoice.IsCreditConverted = false;
                        tranInvoice.InvoiceStatus = "Closed";
                        tranInvoice.InvoiceModule = "SM";
                        tranInvoice.Remarks = obj.Remarks;
                        tranInvoice.InvoiceNotes = string.Empty;
                        tranInvoice.ServiceDate1 = createdOn.ToString();
                        tranInvoice.CustArbName = studentDetails.StuName2;
                        tranInvoice.CustName = studentDetails.StuName;
                        await _context.TranInvoices.AddAsync(tranInvoice);
                        await _context.SaveChangesAsync();
                        invoiceId = tranInvoice.Id;

                        //var stuTermHeaderDetails = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(x => x.StuAdmNum == obj.AdmissionNumber &&  obj.TermCodes.Contains(x.TermCode));
                        var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().Where(x => obj.TermCodes.Contains(x.TermCode)).ToListAsync();
                        foreach (var termDetail in termDetails)
                        {
                            var product = await _context.TranProducts.FirstOrDefaultAsync(e => e.ProductCode == termDetail.TermCode);
                            if (product is null || termDetail is null)
                            {
                                product = new();
                                product.NameEN = termDetail.TermName;
                                product.NameAR = termDetail.TermName2;
                                product.ProductCode = termDetail.TermCode;
                                product.CompanyId = companydetails.CompanyId;
                                product.Description = companydetails.Remarks;
                                product.ProductTypeId = 17;
                                product.UnitPrice = 0;
                                product.CostPrice = 0;
                                product.UnitTypeId = 7;
                                product.IsDefaultConfig = true;
                                product.CreatedOn = DateTime.Now;
                                product.CreatedBy = 1;
                                await _context.TranProducts.AddAsync(product);
                                await _context.SaveChangesAsync();
                            }
                        }


                        if (stuTermFeeDetails != null && termDetails != null)
                        {
                            TblTranFeeTransaction feeTransaction = new();
                            feeTransaction.AdmissionNumber = obj.AdmissionNumber;
                            feeTransaction.ReceiptVoucher = Convert.ToString(jvSeq);
                            feeTransaction.FeeDate = DateTime.Now;
                            feeTransaction.FeeTerm = string.Join(',', termDetails.Select(x => x.TermName).ToList());
                            feeTransaction.FeeStructCode = stuTermFeeDetails[0].FeeStructCode;
                            feeTransaction.TermCode = obj.TermCode;
                            feeTransaction.FeeDueDate = stuTermFeeDetails.Min(x => x.FeeDueDate);
                            feeTransaction.TotFeeAmount = totalNetPayment - Convert.ToDecimal(obj.Discount);
                            feeTransaction.DiscAmount = Convert.ToDecimal(obj.Discount);
                            feeTransaction.NetFeeAmount = totalNetPayment;
                            feeTransaction.DiscReason = string.Empty;
                            feeTransaction.IsPaid = true;
                            feeTransaction.PaidDate = DateTime.Now;
                            feeTransaction.PaidTransNum = string.Empty;
                            feeTransaction.PaidRemarks1 = obj.Remarks;
                            feeTransaction.PaidRemarks2 = string.Empty;
                            feeTransaction.JvNumber = string.Empty;
                            feeTransaction.InvNumber = string.Empty;
                            feeTransaction.PaidOnline = false;
                            feeTransaction.PaidManual = true;
                            feeTransaction.PayCode = obj.PayCode;
                            feeTransaction.ReceivedByUser = Convert.ToString(request.User.UserId);
                            feeTransaction.AcademicYear = Convert.ToString(acadamicYear);
                            await _context.FeeTransaction.AddAsync(feeTransaction);
                            await _context.SaveChangesAsync();

                            foreach (var item in studentFeePaidList)
                            {
                                var productData = await _context.TranProducts.FirstOrDefaultAsync(e => e.ProductCode == item.TermCode);
                                TblTranInvoiceItem tranInvoiceItem = new();
                                tranInvoiceItem.InvoiceNumber = Convert.ToString(jvSeq);
                                tranInvoiceItem.InvoiceId = invoiceId;
                                tranInvoiceItem.CreditMemoId = null;
                                tranInvoiceItem.DebitMemoId = null;
                                tranInvoiceItem.ProductId = productData.Id;
                                tranInvoiceItem.Quantity = 1;
                                tranInvoiceItem.UnitPrice = item.PaidAmount;
                                tranInvoiceItem.SubTotal = item.PaidAmount;
                                tranInvoiceItem.DiscountAmount = item.DiscAmount;
                                tranInvoiceItem.AmountBeforeTax = item.PaidAmount - item.DiscAmount;
                                tranInvoiceItem.TaxAmount = item.IsCompletelyPaid ? stuTermFeeDetails.Where(x => x.TermCode == item.TermCode).Sum(x => x.TaxAmount) : 0;
                                tranInvoiceItem.TotalAmount = item.PaidAmount - item.DiscAmount + tranInvoiceItem.TaxAmount;
                                tranInvoiceItem.IsDefaultConfig = true;
                                tranInvoiceItem.CreatedOn = DateTime.Now;
                                tranInvoiceItem.CreatedBy = request.User.UserId;
                                tranInvoiceItem.Description = productData.NameEN ;
                                if (taxData != null)
                                    tranInvoiceItem.TaxTariffPercentage = taxData.Taxper01;
                                else
                                    tranInvoice.VatPercentage = 0;
                                tranInvoiceItem.Discount = 0;
                                tranInvoiceItem.InvoiceType = null;
                                await _context.TranInvoiceItems.AddAsync(tranInvoiceItem);
                                await _context.SaveChangesAsync();
                            }

                            List<TblDefStudentFeeDetails> feeDetailList = new();
                            feeDetailList = await _context.DefStudentFeeDetails.AsNoTracking().Where(e => e.StuAdmNum == obj.AdmissionNumber && obj.TermCodes.Contains(e.TermCode)).ToListAsync();
                            foreach (var feeDetail in feeDetailList)
                            {
                                feeDetail.IsPaid = true;
                                feeDetail.AddedOn = DateTime.Now;
                                _context.DefStudentFeeDetails.Update(feeDetail);
                                await _context.SaveChangesAsync();
                            }
                            feeTranID = feeTransaction.Id;
                        }

                        TblFinTrnCustomerApproval approvalArInv = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            LoginId = request.User.UserId,
                            AppRemarks = "Automatic Approval From SM",
                            InvoiceId = tranInvoice.Id,
                            IsApproved = true,
                        };
                        await _context.TrnCustomerApprovals.AddAsync(approvalArInv);
                        await _context.SaveChangesAsync();

                        TblFinTrnCustomerInvoice cInvoice = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            InvoiceNumber = tranInvoice.InvoiceNumber,
                            InvoiceDate = tranInvoice.InvoiceDate,
                            CreditDays = 0,
                            DueDate = tranInvoice.InvoiceDueDate,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            CustCode = customer.CustCode,
                            DocNum = tranInvoice.InvoiceRefNumber,
                            LoginId = request.User.UserId,
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            InvoiceAmount = tranInvoice.TotalAmount,
                            DiscountAmount = tranInvoice.DiscountAmount ?? 0,
                            NetAmount = tranInvoice.TotalAmount,
                            PaidAmount = tranInvoice.TotalAmount,
                            AppliedAmount = 0,
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "Settled From SM",
                            InvoiceId = tranInvoice.Id,
                            IsPaid = true,
                        };
                        cInvoice.BalanceAmount = cInvoice.NetAmount - cInvoice.PaidAmount;
                        await _context.TrnCustomerInvoices.AddAsync(cInvoice);

                        TblFinTrnCustomerStatement cStatement = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                            TranNumber = tranInvoice.InvoiceNumber,
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            PaymentType = paymentTerms != null ? paymentTerms.SalesTermsCode : "CASH",
                            PamentCode = "paycode",
                            CheckNumber = "",
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
                            LoginId = request.User.UserId,
                            DrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            CrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            InvoiceId = tranInvoice.Id,
                        };
                        await _context.TrnCustomerStatements.AddAsync(cStatement);

                        TblFinTrnCustomerStatement cPaymentStatement = new()
                        {
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            TranDate = createdOn,
                            TranSource = "SM",
                            Trantype = "Payment",
                            TranNumber = tranInvoice.InvoiceNumber,// invoiceSeq.ToString(),
                            CustCode = customer?.CustCode,
                            DocNum = "DocNum",
                            ReferenceNumber = tranInvoice.InvoiceRefNumber,
                            PaymentType = paymentTerms != null ? paymentTerms.SalesTermsCode : "CASH",
                            PamentCode = "Paycode",
                            CheckNumber = "",
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
                            LoginId = request.User.UserId,
                            DrAmount = 0,
                            CrAmount = tranInvoice.TotalAmount,
                            InvoiceId = tranInvoice.Id,
                        };
                        await _context.TrnCustomerStatements.AddAsync(cPaymentStatement);

                        TblFinTrnDistribution distribution1 = new()
                        {
                            InvoiceId = tranInvoice.Id,
                            FinAcCode = customer?.CustArAcCode,
                            CrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            DrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TotalAmount : 0,
                            Source = "SM",
                            Type = "paycode",
                            Gl = string.Empty,
                            CreatedOn = createdOn
                        };

                        TblFinTrnDistribution distribution2 = new()
                        {
                            InvoiceId = tranInvoice.Id,
                            FinAcCode = customer?.CustDefExpAcCode,
                            CrAmount = !tranInvoice.IsCreditConverted ? (tranInvoice.TotalAmount - tranInvoice.TaxAmount) : 0,
                            DrAmount = tranInvoice.IsCreditConverted ? (tranInvoice.TotalAmount - tranInvoice.TaxAmount) : 0,
                            Source = "SM",
                            Gl = string.Empty,
                            Type = "Expense",
                            CreatedOn = createdOn
                        };
                        await _context.FinDistributions.AddAsync(distribution1);
                        await _context.FinDistributions.AddAsync(distribution2);

                        List<TblFinTrnDistribution> distributionsList = new() { distribution1, distribution2 };

                        if (taxData is not null)
                        {
                            TblFinTrnDistribution distribution3 = new()
                            {
                                InvoiceId = tranInvoice.Id,
                                FinAcCode = taxData?.OutputAcCode01,
                                CrAmount = !tranInvoice.IsCreditConverted ? tranInvoice.TaxAmount : 0,
                                DrAmount = tranInvoice.IsCreditConverted ? tranInvoice.TaxAmount : 0,
                                Source = "SM",
                                Gl = string.Empty,
                                Type = "VAT",
                                CreatedOn = createdOn
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



                        TblFinTrnJournalVoucher JV = new()
                        {
                            SpVoucherNumber = string.Empty,
                            VoucherNumber = jvSeq.ToString(),
                            CompanyId = (int)tranInvoice.CompanyId,
                            BranchCode = tranInvoice.BranchCode,
                            Batch = string.Empty,
                            Source = "SM",
                            Remarks = tranInvoice.Remarks,
                            Narration = tranInvoice.InvoiceNotes ?? tranInvoice.Remarks,
                            JvDate = createdOn,
                            Amount = tranInvoice.TotalAmount ?? 0,
                            DocNum = tranInvoice.InvoiceNumber,
                            CDate = createdOn,
                            Posted = true,
                            PostedDate = createdOn,
                            SiteCode = tranInvoice.SiteCode
                        };
                        await _context.JournalVouchers.AddAsync(JV);
                        await _context.SaveChangesAsync();
                        var jvId = JV.Id;

                        var branchAuths = await _context.SysSchoolBranchesAuthority.Select(e => new { e.BranchCode, e.TeacherCode })
                            .Where(e => e.BranchCode == tranInvoice.BranchCode).ToListAsync();
                        if (branchAuths.Count() > 0)
                        {
                            List<TblFinTrnJournalVoucherApproval> jvApprovalList = new();
                            foreach (var item in branchAuths)
                            {
                                var teacherDetails = await _context.DefSchoolTeacherMaster.AsNoTracking().FirstOrDefaultAsync(x => x.TeacherCode == item.TeacherCode);
                                TblFinTrnJournalVoucherApproval approval = new()
                                {
                                    CompanyId = (int)tranInvoice.CompanyId,
                                    BranchCode = tranInvoice.BranchCode,
                                    JvDate = createdOn,
                                    TranSource = "SM",
                                    Trantype = tranInvoice.IsCreditConverted ? "Credit" : "Invoice",
                                    DocNum = tranInvoice.InvoiceRefNumber,
                                    LoginId = Convert.ToInt32(teacherDetails.SysLoginId),
                                    AppRemarks = tranInvoice.Remarks,
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
                                BranchCode = tranInvoice.BranchCode,
                                Batch = string.Empty,
                                Batch2 = string.Empty,
                                Remarks = tranInvoice.Remarks,
                                CrAmount = obj1.CrAmount,
                                DrAmount = obj1.DrAmount,
                                FinAcCode = obj1.FinAcCode,
                                Description = tranInvoice.InvoiceNotes,
                                CostAllocation = costallocations.Id,
                                CostSegCode = customer.CustCode,
                                SiteCode = tranInvoice.SiteCode

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

                            JvDate = createdOn,
                            TranNumber = jvSeq.ToString(),
                            Remarks1 = tranInvoice.Remarks,
                            Remarks2 = "SM Invoice",
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
                                TransDate = createdOn,
                                PostedFlag = true,
                                PostDate = createdOn,
                                Jvnum = item.JournalVoucherId.ToString(),
                                Narration = item.Description,
                                Remarks = item.Remarks,
                                Remarks2 = string.Empty,
                                ReverseFlag = false,
                                VoidFlag = false,
                                Source = "SM",
                                ExRate = 0,
                                SiteCode = tranInvoice.SiteCode
                            };
                            ledgerList.Add(ledger);
                        }
                        if (ledgerList.Count > 0)
                        {
                            await _context.AccountsLedgers.AddRangeAsync(ledgerList);
                            await _context.SaveChangesAsync();
                        }

                    }
                    #endregion

                    branchDetails.NextFeeVoucherNum = branchDetails.NextFeeVoucherNum + 1;
                    _context.SchoolBranches.Update(branchDetails);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info Create Update Fee Transaction  Method Exit----");
                    await transaction.CommitAsync();
                    return feeTranID;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Update Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion







    #region GetFeeTransactionDetails
    public class GetFeeTransactionDetails : IRequest<PrintStuFeeTransactionDto>
    {
        public UserIdentityDto User { get; set; }
        public string ReceiptVoucher { get; set; }

    }

    public class GetFeeTransactionDetailsHandler : IRequestHandler<GetFeeTransactionDetails, PrintStuFeeTransactionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetFeeTransactionDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PrintStuFeeTransactionDto> Handle(GetFeeTransactionDetails request, CancellationToken cancellationToken)
        {
            PrintStuFeeTransactionDto printStuFeeTransaction = new PrintStuFeeTransactionDto();

            var list = await _context.FeeTransaction.AsNoTracking()
                                    .Where(x => x.ReceiptVoucher == request.ReceiptVoucher).ToListAsync();


            printStuFeeTransaction.TotFeeAmount = list.FirstOrDefault().TotFeeAmount;
            printStuFeeTransaction.DiscAmount = list.FirstOrDefault().DiscAmount;
            printStuFeeTransaction.NetFeeAmount = list.FirstOrDefault().NetFeeAmount;
            printStuFeeTransaction.PaidOnline = list.FirstOrDefault().PaidOnline;
            printStuFeeTransaction.TaxAmount = list.FirstOrDefault().TaxAmount;
            printStuFeeTransaction.AcademicYear = list.FirstOrDefault().AcademicYear;
            printStuFeeTransaction.PayCode = list.FirstOrDefault().PayCode;



            printStuFeeTransaction.VoucherDate = list.FirstOrDefault().PaidDate;
            printStuFeeTransaction.VoucherNumber = list.FirstOrDefault().ReceiptVoucher;
            printStuFeeTransaction.AdmissionNumber = list.FirstOrDefault().AdmissionNumber;
            printStuFeeTransaction.TotalFeeAmount = list.Sum(x => x.NetFeeAmount);
            List<TermFeeDTO> termFeeDetails = new List<TermFeeDTO>();
            foreach (var item in list)
            {
                TermFeeDTO termFeeDTO = new TermFeeDTO();
                termFeeDTO.FeeAmount = item.NetFeeAmount;
                var termData = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                termFeeDTO.TermName = termData.TermName;
                termFeeDTO.TermName2 = termData.TermName2;
                termFeeDTO.TotFeeAmount = item.TotFeeAmount;
                termFeeDTO.DiscAmount = item.DiscAmount;
                termFeeDTO.NetFeeAmount = item.NetFeeAmount;
                termFeeDTO.TaxAmount = item.TaxAmount;
                termFeeDetails.Add(termFeeDTO);
            }
            printStuFeeTransaction.TermFeeDetails = termFeeDetails;
            var result = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(e => e.StuAdmNum == printStuFeeTransaction.AdmissionNumber);
            printStuFeeTransaction.StudentName = result.StuName;
            printStuFeeTransaction.StudentName2 = result.StuName2;
            return printStuFeeTransaction;
        }


    }


    #endregion
}
