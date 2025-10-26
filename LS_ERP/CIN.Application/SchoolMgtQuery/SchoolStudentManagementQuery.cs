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
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SalesSetup;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetSchoolStudentList
    public class GetSchoolStudentManagementList : IRequest<PaginatedList<TblDefSchoolStudentMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolStudentManagementListHandler : IRequestHandler<GetSchoolStudentManagementList, PaginatedList<TblDefSchoolStudentMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentManagementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefSchoolStudentMasterDto>> Handle(GetSchoolStudentManagementList request, CancellationToken cancellationToken)
        {
            try
            {

                var schoolStudentList = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider)
                                      .OrderByDescending(e=>e.Id).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return schoolStudentList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion


    #region GetAllStudentsList
    public class GetAllStudentsList : IRequest<List<TblDefSchoolStudentMasterDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetAllStudentsListHandler : IRequestHandler<GetAllStudentsList, List<TblDefSchoolStudentMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllStudentsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolStudentMasterDto>> Handle(GetAllStudentsList request, CancellationToken cancellationToken)
        {
            try
            {

                var schoolStudentList = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider).Where(x => x.StuAdmDate != null)
                                      .ToListAsync();

                return schoolStudentList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion    

    #region Create_And_Update
    public class CreateSchoolStudentManagement : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefSchoolStudentMasterDto SchoolStudentMasterDto { get; set; }
    }

    public class CreateSchoolStudentManagementHandler : IRequestHandler<CreateSchoolStudentManagement, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSchoolStudentManagementHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSchoolStudentManagement request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateSchoolStudentManagement method start----");

                var obj = request.SchoolStudentMasterDto;


                TblDefSchoolStudentMaster SchoolStudentMaster = new();
                if (obj.Id > 0)
                    SchoolStudentMaster = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                SchoolStudentMaster.Id = obj.Id;
                SchoolStudentMaster.StuRegNum = obj.StuRegNum;
                SchoolStudentMaster.StuAdmNum = obj.StuAdmNum;
                SchoolStudentMaster.StuRegDate = obj.StuRegDate;
                SchoolStudentMaster.StuName = obj.StuName;
                SchoolStudentMaster.StuName2 = obj.StuName2;
                SchoolStudentMaster.Alias = obj.Alias;
                SchoolStudentMaster.FatherName = obj.FatherName;
                SchoolStudentMaster.MotherName = obj.MotherName;
                SchoolStudentMaster.DateofBirth = obj.DateofBirth;
                SchoolStudentMaster.Age = obj.Age;
                SchoolStudentMaster.GradeCode = obj.GradeCode;
                SchoolStudentMaster.GradeSectionCode = obj.GradeSectionCode;
                SchoolStudentMaster.LangCode = obj.LangCode;
                SchoolStudentMaster.AcademicYear = obj.AcademicYear;
                SchoolStudentMaster.GenderCode = obj.GenderCode;
                SchoolStudentMaster.PTGroupCode = obj.PTGroupCode;
                SchoolStudentMaster.IDNumber = obj.IDNumber;
                SchoolStudentMaster.NatCode = obj.NatCode;
                SchoolStudentMaster.ReligionCode = obj.ReligionCode;
                SchoolStudentMaster.MotherToungue = obj.MotherToungue;
                SchoolStudentMaster.FeeStructCode = obj.FeeStructCode;
                SchoolStudentMaster.TransportationRequired = obj.TransportationRequired;
                SchoolStudentMaster.PickNDropZone = obj.PickNDropZone;
                SchoolStudentMaster.TransportationFee = obj.TransportationFee;
                SchoolStudentMaster.VehicleTransport = obj.VehicleTransport;
                SchoolStudentMaster.RegisteredPhone = obj.RegisteredPhone;
                SchoolStudentMaster.RegisteredEmail = obj.RegisteredEmail;
                SchoolStudentMaster.PAddress1 = obj.PAddress1;
                SchoolStudentMaster.City = obj.City;
                SchoolStudentMaster.ZipCode = obj.ZipCode;
                SchoolStudentMaster.Country = obj.Country;
                SchoolStudentMaster.Phone = obj.Phone;
                SchoolStudentMaster.Mobile = obj.Mobile;
                SchoolStudentMaster.Remarks1 = obj.Remarks1;
                SchoolStudentMaster.Remarks2 = obj.Remarks2;
                SchoolStudentMaster.Remarks3 = obj.Remarks3;
                SchoolStudentMaster.Image1Path = obj.Image1Path;
                SchoolStudentMaster.Image2Path = obj.Image2Path;
                SchoolStudentMaster.AdmissionType = obj.AdmissionType;
                SchoolStudentMaster.ShortListDate = obj.ShortListDate;
                SchoolStudentMaster.ShortListedBy = obj.ShortListedBy;
                SchoolStudentMaster.DateofAdmission = obj.DateofAdmission;
                SchoolStudentMaster.StuConvDate = obj.StuConvDate;
                SchoolStudentMaster.StuConvBy = obj.StuConvBy;
                SchoolStudentMaster.BloodGroup = obj.BloodGroup;
                SchoolStudentMaster.Height = obj.Height;
                SchoolStudentMaster.Weight = obj.Weight;
                SchoolStudentMaster.PhysicalDisability = obj.PhysicalDisability;
                SchoolStudentMaster.PhysicalDisabilityNotes = obj.PhysicalDisabilityNotes;
                SchoolStudentMaster.WearSpects = obj.WearSpects;
                SchoolStudentMaster.SpecialAssistance = obj.SpecialAssistance;
                SchoolStudentMaster.SpecialAssistanceNotes = obj.SpecialAssistanceNotes;
                SchoolStudentMaster.AcademicsScale = obj.AcademicsScale;
                SchoolStudentMaster.AttentivenessScale = obj.AttentivenessScale;
                SchoolStudentMaster.HomeWorkScale = obj.HomeWorkScale;
                SchoolStudentMaster.ProjectWorkScale = obj.ProjectWorkScale;
                SchoolStudentMaster.SportsPhysicalScale = obj.SportsPhysicalScale;
                SchoolStudentMaster.DiciplineAttitude = obj.DiciplineAttitude;
                SchoolStudentMaster.SignatureImage1 = obj.SignatureImage1;
                SchoolStudentMaster.SignatureImage2 = obj.SignatureImage2;
                SchoolStudentMaster.CreatedOn = obj.CreatedOn;
                SchoolStudentMaster.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {
                    _context.DefSchoolStudentMaster.Update(SchoolStudentMaster);
                }
                else
                {
                    await _context.DefSchoolStudentMaster.AddAsync(SchoolStudentMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateSchoolStudentManagement method Exit----");
                return SchoolStudentMaster.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateSchoolStudentManagement Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolStudent : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class DeleteSchoolStudentHandler : IRequestHandler<DeleteSchoolStudent, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolStudentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolStudent request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Master start----");

                if (request.Id > 0)
                {
                    var studentmaster = await _context.DefSchoolStudentMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(studentmaster);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Student Master");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion



    #region All_Student_Master_Create_And_Update
    public class AllSchoolStudentMasterData : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public AllStudentMasterDataDto AllStudentMasterDataDto { get; set; }
    }

    public class AllSchoolStudentMasterDataHandler : IRequestHandler<AllSchoolStudentMasterData, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AllSchoolStudentMasterDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AllSchoolStudentMasterData request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateSchoolStudentManagement method start----");
            TblDefSchoolStudentMaster SchoolStudentMaster = new();
            string stuAdmNum = string.Empty;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    bool IsNewFeeStruct = false;
                    var obj = request.AllStudentMasterDataDto;
                    if (obj.Id > 0)
                    {
                        SchoolStudentMaster = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        if (SchoolStudentMaster.FeeStructCode != obj.FeeStructCode)
                            IsNewFeeStruct = true;
                    }


                    int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                    var branchDetails = await _context.SchoolBranches.OrderByDescending(x=>x.Id).
                                                      FirstOrDefaultAsync(x => x.BranchCode == obj.BranchCode);
                    if (branchDetails != null && obj.Id == 0)
                    {
                        stuAdmNum = acadamicYear.ToString().Substring(2, 2) + (branchDetails.BranchPrefix ?? string.Empty) + Convert.ToString(branchDetails.NextStuNum);

                        branchDetails.NextStuNum = branchDetails.NextStuNum + 1;
                        _context.SchoolBranches.Update(branchDetails);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        stuAdmNum = SchoolStudentMaster.StuAdmNum;
                    }
                    SchoolStudentMaster.Id = obj.Id;
                    SchoolStudentMaster.StuRegNum = stuAdmNum;
                    SchoolStudentMaster.StuAdmNum = stuAdmNum;
                    SchoolStudentMaster.StuAdmDate = obj.StuAdmDate;
                    SchoolStudentMaster.StuName = obj.StuName;
                    SchoolStudentMaster.StuName2 = obj.StuName2;
                    SchoolStudentMaster.Alias = obj.Alias;
                    SchoolStudentMaster.FatherName = obj.FatherName;
                    SchoolStudentMaster.MotherName = obj.MotherName;
                    SchoolStudentMaster.DateofBirth = obj.DateofBirth;
                    SchoolStudentMaster.Age = obj.Age;
                    SchoolStudentMaster.BranchCode = obj.BranchCode;
                    SchoolStudentMaster.GradeCode = obj.GradeCode;
                    SchoolStudentMaster.GradeSectionCode = obj.GradeSectionCode;
                    SchoolStudentMaster.LangCode = obj.LangCode;
                    SchoolStudentMaster.AcademicYear = acadamicYear;
                    SchoolStudentMaster.GenderCode = obj.GenderCode;
                    SchoolStudentMaster.PTGroupCode = obj.PTGroupCode;
                    SchoolStudentMaster.StuIDNumber = obj.StuIDNumber;
                    SchoolStudentMaster.IDNumber = obj.IDNumber;
                    SchoolStudentMaster.NatCode = obj.NatCode;
                    SchoolStudentMaster.ReligionCode = obj.ReligionCode;
                    SchoolStudentMaster.MotherToungue = obj.MotherToungue;
                    SchoolStudentMaster.FeeStructCode = obj.FeeStructCode;
                    SchoolStudentMaster.TransportationRequired = obj.TransportationRequired;
                    SchoolStudentMaster.PickNDropZone = obj.PickNDropZone;
                    SchoolStudentMaster.TransportationFee = obj.TransportationFee ?? 0;
                    SchoolStudentMaster.VehicleTransport = obj.VehicleTransport;
                    SchoolStudentMaster.RegisteredPhone = obj.RegisteredPhone;
                    SchoolStudentMaster.RegisteredEmail = obj.RegisteredEmail;
                    SchoolStudentMaster.IsActive = obj.IsActive;
                    if (!string.IsNullOrEmpty(obj.StudentImageFileName))
                        SchoolStudentMaster.Image1Path = obj.StudentImageFileName;
                    SchoolStudentMaster.TaxApplicable = obj.TaxApplicable;
                    SchoolStudentMaster.PAddress1 = obj.PAddress1;
                    SchoolStudentMaster.BuildingName = obj.BuildingName;
                    SchoolStudentMaster.City = obj.City;
                    SchoolStudentMaster.ZipCode = obj.ZipCode;
                    SchoolStudentMaster.Phone = obj.Phone;
                    SchoolStudentMaster.Mobile = obj.Mobile;
                    SchoolStudentMaster.BloodGroup = obj.BloodGroup;
                    SchoolStudentMaster.Height = obj.Height;
                    SchoolStudentMaster.Weight = obj.Weight;
                    SchoolStudentMaster.PhysicalDisability = obj.PhysicalDisability;
                    SchoolStudentMaster.PhysicalDisabilityNotes = obj.PhysicalDisabilityNotes;
                    SchoolStudentMaster.WearSpects = false;
                    SchoolStudentMaster.SpecialAssistance = obj.SpecialAssistance;
                    SchoolStudentMaster.SpecialAssistanceNotes = obj.SpecialAssistanceNotes;
                    SchoolStudentMaster.AcademicsScale = obj.AcademicsScale;
                    SchoolStudentMaster.AttentivenessScale = obj.AttentivenessScale;
                    SchoolStudentMaster.HomeWorkScale = obj.HomeWorkScale;
                    SchoolStudentMaster.ProjectWorkScale = obj.ProjectWorkScale;
                    SchoolStudentMaster.SportsPhysicalScale = obj.SportsPhysicalScale;
                    SchoolStudentMaster.DiciplineAttitude = obj.DiciplineAttitude;
                    if (!string.IsNullOrEmpty(obj.FatherSignatureFileName))
                        SchoolStudentMaster.SignatureImage1 = obj.FatherSignatureFileName;
                    if (!string.IsNullOrEmpty(obj.MotherSignatureFileName))
                        SchoolStudentMaster.SignatureImage2 = obj.MotherSignatureFileName;
                    SchoolStudentMaster.CreatedOn = DateTime.Now;
                    SchoolStudentMaster.CreatedBy = Convert.ToString(request.User.UserId);
                    SchoolStudentMaster.BranchCode = obj.BranchCode;
                    if (obj.Id > 0)
                    {
                        _context.DefSchoolStudentMaster.Update(SchoolStudentMaster);
                    }
                    else
                    {
                        await _context.DefSchoolStudentMaster.AddAsync(SchoolStudentMaster);
                    }
                    await _context.SaveChangesAsync();

                    #region CreateParentLogin
                    TblParentsLogin fatherLogin = new();
                    fatherLogin = await _context.TblParentsLogin.AsNoTracking().FirstOrDefaultAsync(x=>x.RegisteredPhone==obj.FatherMobile);
                    if (fatherLogin == null)
                    {
                        fatherLogin = new();
                        fatherLogin.RegisteredPhone = obj.FatherMobile;
                        fatherLogin.RegisteredEmail = obj.FatherEmail;
                        fatherLogin.Password= obj.FatherMobile;
                        fatherLogin.IsActive= true;
                        fatherLogin.IsApprove= true;
                        fatherLogin.CurrentLogin= true;
                        fatherLogin.InactiveOn= DateTime.Now.AddYears(10);
                        fatherLogin.ApproveDate= DateTime.Now;
                        fatherLogin.RegistedDate= DateTime.Now;
                        await _context.TblParentsLogin.AddAsync(fatherLogin);
                        await _context.SaveChangesAsync();
                    }

                    TblParentsLogin motherLogin = new();
                    motherLogin = await _context.TblParentsLogin.AsNoTracking().FirstOrDefaultAsync(x => x.RegisteredPhone == obj.MotherMobile);
                    if (motherLogin == null)
                    {
                        motherLogin = new();
                        motherLogin.RegisteredPhone = obj.MotherMobile;
                        motherLogin.RegisteredEmail = obj.MotherEmail;
                        motherLogin.Password = obj.MotherMobile;
                        motherLogin.IsActive = true;
                        motherLogin.IsApprove = true;
                        motherLogin.CurrentLogin = true;
                        motherLogin.InactiveOn = DateTime.Now.AddYears(10);
                        motherLogin.ApproveDate = DateTime.Now;
                        motherLogin.RegistedDate = DateTime.Now;
                        await _context.TblParentsLogin.AddAsync(motherLogin);
                        await _context.SaveChangesAsync();
                    }
                    #endregion





                    #region tblSndDefCustomerCategory
                    TblSndDefCustomerCategory categoryData = new();
                    categoryData = await _context.SndCustomerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CustCatCode == obj.GradeCode);
                    if (categoryData == null)
                    {
                        var acedemicClassGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(x => x.GradeCode == obj.GradeCode);
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
                    TblSndDefCustomerMaster customerMaster = await _context.OprCustomers.AsNoTracking().FirstOrDefaultAsync(e => e.CustCode == stuAdmNum);
                    if (customerMaster == null)
                        customerMaster = new();
                    customerMaster.CustCode = stuAdmNum;
                    customerMaster.CustName = obj.StuName;
                    customerMaster.CustArbName = obj.StuName2;
                    customerMaster.CustAlias = obj.Alias;
                    var userTypeData = await _context.UserTypes.FirstOrDefaultAsync(x => x.UerType.ToLower() == "student");
                    customerMaster.CustType = (short)userTypeData.Id;
                    customerMaster.CustCatCode = obj.GradeCode;
                    customerMaster.CustRating = 1;
                    var paymentTermData = await _context.SndSalesTermsCodes.AsNoTracking().FirstOrDefaultAsync(x => x.SalesTermsCode == "FeePayTerm");
                    if (paymentTermData != null)
                        customerMaster.SalesTermsCode = paymentTermData.SalesTermsCode;
                    else
                        customerMaster.SalesTermsCode = "CASH";
                    customerMaster.CustDiscount = 0;
                    customerMaster.CustCrLimit = 0;
                    customerMaster.CustSalesRep = "Admin";
                    customerMaster.CustSalesArea = "Admin";
                    customerMaster.CustARAc = firstCustData?.CustARAc;
                    customerMaster.CustLastPaidDate = DateTime.Now;
                    customerMaster.CustLastSalesDate = DateTime.Now;
                    customerMaster.CustLastPayAmt = 0;
                    customerMaster.CustAddress1 = obj.BuildingName + ' ' + obj.PAddress1;
                    customerMaster.CustCityCode1 = obj.City;
                    customerMaster.CustMobile1 = obj.FatherMobile;
                    customerMaster.CustPhone1 = obj.Phone;
                    customerMaster.CustEmail1 = obj.FatherEmail;
                    customerMaster.CustContact1 = obj.FatherName;
                    customerMaster.CustAddress2 = obj.BuildingName + ' ' + obj.PAddress1;
                    customerMaster.CustCityCode2 = obj.City;
                    customerMaster.CustMobile2 = obj.MotherMobile;
                    customerMaster.CustPhone2 = obj.Phone;
                    customerMaster.CustEmail2 = obj.MotherEmail;
                    customerMaster.CustContact2 = obj.MotherName;
                    customerMaster.CustUDF1 = "-";
                    customerMaster.CustUDF2 = "-";
                    customerMaster.CustUDF3 = "-";
                    customerMaster.CustAllowCrsale = false;
                    customerMaster.CustAlloCrOverride = false;
                    customerMaster.CustOnHold = false;
                    customerMaster.CustAlloChkPay = false;
                    customerMaster.CustSetPriceLevel = false;
                    customerMaster.CustPriceLevel = 0;
                    customerMaster.CustIsVendor = false;
                    customerMaster.CustArAcBranch = false;
                    customerMaster.CustArAcCode = firstCustData?.CustArAcCode;
                    customerMaster.CustDefExpAcCode = firstCustData?.CustDefExpAcCode;
                    customerMaster.CustARAdjAcCode = firstCustData?.CustARAdjAcCode;
                    customerMaster.CustARDiscAcCode = firstCustData?.CustARDiscAcCode;
                    customerMaster.CreatedOn = DateTime.Now;
                    customerMaster.IsActive = true;
                    customerMaster.VATNumber = "-";
                    customerMaster.CustOutStandBal = 0;
                    customerMaster.CustAvailCrLimit = 0;
                    customerMaster.CustNameAliasAr = obj.StuName2;
                    customerMaster.CustNameAliasEn = obj.StuName;
                    if (customerMaster.Id == 0)
                        await _context.OprCustomers.AddAsync(customerMaster);
                    else
                        _context.OprCustomers.Update(customerMaster);
                    await _context.SaveChangesAsync();
                    #endregion

                    #region FeeHeaderBlock
                    var taxData = await _context.SystemTaxes.FirstOrDefaultAsync();
                    if (obj.Id == 0)
                    {
                        // studentfeeHeader = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        var dataList = await _context.SchoolFeeStructureDetails
                                    .Where(e => e.FeeStructCode == obj.FeeStructCode).GroupBy(r => new { r.TermCode })
                                                        .Select(r => new CustomSysSchoolFeeStructureDetails
                                                        {
                                                            TermCode = r.Key.TermCode,
                                                            FeeAmount = r.Sum(x => x.FeeAmount),
                                                        }).ToListAsync();
                        foreach (var item in dataList)
                        {
                            TblDefStudentFeeHeader studentfeeHeader = new();
                            studentfeeHeader.StuAdmNum = stuAdmNum;
                            studentfeeHeader.FeeStructCode = obj.FeeStructCode;
                            studentfeeHeader.TermCode = item.TermCode;
                            var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                            studentfeeHeader.FeeDueDate = termDetails.FeeDueDate;
                            studentfeeHeader.TotFeeAmount = item.FeeAmount;
                            studentfeeHeader.DiscAmount = 0;
                            studentfeeHeader.NetFeeAmount = item.FeeAmount;
                            studentfeeHeader.IsPaid = false;
                            studentfeeHeader.AcademicYear = acadamicYear;
                            studentfeeHeader.IsCompletelyPaid = false;
                            if (obj.TaxApplicable && taxData != null)
                            {
                                studentfeeHeader.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                studentfeeHeader.NetFeeAmount = item.FeeAmount + studentfeeHeader.TaxAmount;
                            }
                            await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        if (IsNewFeeStruct)
                        {
                            var stuFeeDetailsList = await _context.DefStudentFeeDetails.Where(x => x.StuAdmNum == stuAdmNum && x.AcademicYear == acadamicYear && x.IsPaid != true).ToListAsync();
                            if (stuFeeDetailsList != null && stuFeeDetailsList.Count > 0)
                            {
                                _context.DefStudentFeeDetails.RemoveRange(stuFeeDetailsList);
                                await _context.SaveChangesAsync();
                            }
                            var stuFeeHeaderList = await _context.DefStudentFeeHeader.Where(x => x.StuAdmNum == stuAdmNum && x.AcademicYear== acadamicYear && x.IsPaid != true).ToListAsync();
                            if (stuFeeHeaderList!=null && stuFeeHeaderList.Count>0)
                            {
                                _context.DefStudentFeeHeader.RemoveRange(stuFeeHeaderList);
                                await _context.SaveChangesAsync();
                            }

                            var dataList = await _context.SchoolFeeStructureDetails
                                    .Where(e => e.FeeStructCode == obj.FeeStructCode).GroupBy(r => new { r.TermCode })
                                                        .Select(r => new CustomSysSchoolFeeStructureDetails
                                                        {
                                                            TermCode = r.Key.TermCode,
                                                            FeeAmount = r.Sum(x => x.FeeAmount),
                                                        }).ToListAsync();
                            foreach (var item in dataList)
                            {
                                TblDefStudentFeeHeader studentfeeHeader = new();
                                studentfeeHeader.StuAdmNum = stuAdmNum;
                                studentfeeHeader.FeeStructCode = obj.FeeStructCode;
                                studentfeeHeader.TermCode = item.TermCode;
                                var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                                studentfeeHeader.FeeDueDate = termDetails.FeeDueDate;
                                studentfeeHeader.TotFeeAmount = item.FeeAmount;
                                studentfeeHeader.DiscAmount = 0;
                                studentfeeHeader.NetFeeAmount = item.FeeAmount;
                                studentfeeHeader.IsPaid = false;
                                studentfeeHeader.AcademicYear = acadamicYear;
                                studentfeeHeader.IsCompletelyPaid = false;
                                if (obj.TaxApplicable && taxData != null)
                                {
                                    studentfeeHeader.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                    studentfeeHeader.NetFeeAmount = item.FeeAmount + studentfeeHeader.TaxAmount;
                                }
                                await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                                await _context.SaveChangesAsync();
                            }

                            List<TblSysSchoolFeeStructureDetailsDto> feedetails = await _context.SchoolFeeStructureDetails.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.FeeStructCode == obj.FeeStructCode).ToListAsync();
                            //var feeStructureHeader = await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.FeeStructCode == obj.FeeStructCode);
                            foreach (var item in feedetails)
                            {
                                TblDefStudentFeeDetails studentFeeDetails = new();
                                studentFeeDetails.StuAdmNum = stuAdmNum;
                                studentFeeDetails.FeeStructCode = obj.FeeStructCode;
                                studentFeeDetails.TermCode = item.TermCode;
                                studentFeeDetails.FeeCode = item.FeeCode;
                                studentFeeDetails.FeeAmount = item.FeeAmount;
                                studentFeeDetails.MaxDiscPer = item.MaxDiscPer;
                                studentFeeDetails.DiscPer = 0;
                                studentFeeDetails.NetDiscAmt = 0;
                                studentFeeDetails.NetFeeAmount = item.FeeAmount;
                                studentFeeDetails.IsPaid = false;
                                studentFeeDetails.IsLateFee = false; //feeStructureHeader.ApplyLateFee;
                                studentFeeDetails.IsAddedManaully = false;
                                studentFeeDetails.AddedBy = Convert.ToString(request.User.UserId);
                                studentFeeDetails.AddedOn = DateTime.Now;
                                studentFeeDetails.IsVoided = false;
                                if (obj.TaxApplicable && taxData != null)
                                {
                                    studentFeeDetails.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                    studentFeeDetails.TaxCode = taxData.TaxCode;
                                    studentFeeDetails.NetFeeAmount = item.FeeAmount + studentFeeDetails.TaxAmount;
                                }
                                await _context.DefStudentFeeDetails.AddAsync(studentFeeDetails);
                                await _context.SaveChangesAsync();
                            }


                        }
                        else
                        {
                            if (obj.TaxApplicable && taxData != null)
                            {
                                var stuFeeHeaderList = await _context.DefStudentFeeHeader.Where(x => x.StuAdmNum == stuAdmNum && x.AcademicYear == acadamicYear && x.IsPaid != true).ToListAsync();
                                foreach (var item in stuFeeHeaderList)
                                {
                                    item.TaxAmount = (item.NetFeeAmount / 100) * taxData.Taxper01;
                                    item.NetFeeAmount = item.TotFeeAmount + item.TaxAmount;
                                }
                                _context.DefStudentFeeHeader.UpdateRange(stuFeeHeaderList);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                var stuFeeHeaderList = await _context.DefStudentFeeHeader.Where(x => x.StuAdmNum == stuAdmNum && x.AcademicYear == acadamicYear && x.IsPaid != true).ToListAsync();
                                foreach (var item in stuFeeHeaderList)
                                {
                                    item.TaxAmount = 0;
                                    item.NetFeeAmount = item.TotFeeAmount + item.TaxAmount;
                                }
                                _context.DefStudentFeeHeader.UpdateRange(stuFeeHeaderList);
                                await _context.SaveChangesAsync();
                            }
                        }                        
                    }
                    #endregion
                    #region FeeDetailsBlock

                    if (obj.Id == 0)
                    {
                        //studentFeeDetails = await _context.DefStudentFeeDetails.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        List<TblSysSchoolFeeStructureDetailsDto> feedetails = await _context.SchoolFeeStructureDetails.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.FeeStructCode == obj.FeeStructCode).ToListAsync();
                        //var feeStructureHeader = await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.FeeStructCode == obj.FeeStructCode);
                        foreach (var item in feedetails)
                        {
                            TblDefStudentFeeDetails studentFeeDetails = new();
                            studentFeeDetails.StuAdmNum = stuAdmNum;
                            studentFeeDetails.FeeStructCode = obj.FeeStructCode;
                            studentFeeDetails.TermCode = item.TermCode;
                            studentFeeDetails.FeeCode = item.FeeCode;
                            studentFeeDetails.FeeAmount = item.FeeAmount;
                            studentFeeDetails.MaxDiscPer = item.MaxDiscPer;
                            studentFeeDetails.DiscPer = 0;
                            studentFeeDetails.NetDiscAmt = 0;
                            studentFeeDetails.NetFeeAmount = item.FeeAmount;
                            studentFeeDetails.IsPaid = false;
                            studentFeeDetails.IsLateFee = false; //feeStructureHeader.ApplyLateFee;
                            studentFeeDetails.IsAddedManaully = false;
                            studentFeeDetails.AddedBy = Convert.ToString(request.User.UserId);
                            studentFeeDetails.AddedOn = DateTime.Now;
                            studentFeeDetails.IsVoided = false;
                            if (obj.TaxApplicable && taxData != null)
                            {
                                studentFeeDetails.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                studentFeeDetails.TaxCode = taxData.TaxCode;
                                studentFeeDetails.NetFeeAmount = item.FeeAmount + studentFeeDetails.TaxAmount;
                            }
                            await _context.DefStudentFeeDetails.AddAsync(studentFeeDetails);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        if (obj.TaxApplicable && taxData != null)
                        {
                            var stuFeeDetailsList = await _context.DefStudentFeeDetails.Where(x => x.StuAdmNum == stuAdmNum).ToListAsync();
                            foreach (var item in stuFeeDetailsList)
                            {
                                item.TaxCode = taxData.TaxCode;
                                item.TaxAmount = (item.NetFeeAmount / 100) * taxData.Taxper01;
                                item.NetFeeAmount = item.FeeAmount + item.TaxAmount;
                            }
                            _context.DefStudentFeeDetails.UpdateRange(stuFeeDetailsList);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            var stuFeeDetailsList = await _context.DefStudentFeeDetails.Where(x => x.StuAdmNum == stuAdmNum).ToListAsync();
                            foreach (var item in stuFeeDetailsList)
                            {
                                item.TaxCode = null;
                                item.TaxAmount = 0;
                                item.NetFeeAmount = item.FeeAmount + item.TaxAmount;
                            }
                            _context.DefStudentFeeDetails.UpdateRange(stuFeeDetailsList);
                            await _context.SaveChangesAsync();
                        }
                    }
                    #endregion

                    #region ParentDetails
                    TblDefStudentGuardiansSiblings fatherDetails = new();
                    fatherDetails = await _context.DefStudentGuardiansSiblings.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == obj.StuAdmNum && e.Remarks == "Student_Master_Father");
                    if (fatherDetails == null)
                        fatherDetails = new();
                    fatherDetails.StuAdmNum = stuAdmNum;
                    fatherDetails.RelationType = "father";
                    fatherDetails.Name = obj.FatherName;
                    fatherDetails.Occupation = obj.FatherOccupation;
                    fatherDetails.Deisgnation = obj.FatherDesignation;
                    fatherDetails.Mobile1 = obj.FatherMobile;
                    fatherDetails.email = obj.FatherEmail;
                    fatherDetails.Remarks = "Student_Master_Father";
                    if (fatherDetails.Id > 0)
                    {
                        _context.DefStudentGuardiansSiblings.Update(fatherDetails);
                    }
                    else
                    {
                        await _context.DefStudentGuardiansSiblings.AddAsync(fatherDetails);
                    }
                    await _context.SaveChangesAsync();

                    TblDefStudentGuardiansSiblings motherDetails = new();
                    motherDetails = await _context.DefStudentGuardiansSiblings.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == obj.StuAdmNum && e.Remarks == "Student_Master_Mother");
                    if (motherDetails == null)
                        motherDetails = new();
                    motherDetails.StuAdmNum = stuAdmNum;
                    motherDetails.RelationType = "mother";
                    motherDetails.Name = obj.MotherName;
                    motherDetails.Occupation = obj.MotherOccupation;
                    motherDetails.Deisgnation = obj.MotherDesignation;
                    motherDetails.Mobile1 = obj.MotherMobile;
                    motherDetails.email = obj.MotherEmail;
                    motherDetails.Remarks = "Student_Master_Mother";
                    if (motherDetails.Id > 0)
                    {
                        _context.DefStudentGuardiansSiblings.Update(motherDetails);
                    }
                    else
                    {
                        await _context.DefStudentGuardiansSiblings.AddAsync(motherDetails);
                    }
                    await _context.SaveChangesAsync();
                    #endregion

                    await transaction.CommitAsync();
                    Log.Info("----Info CreateSchoolStudentManagement method Exit----");
                    return SchoolStudentMaster.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateSchoolStudentManagement Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }


    }



    #endregion

    #region GetAllStudentMasterList
    //public class GetAllStudentMasterList : IRequest<PaginatedList<AllSchoolStudentMasterData>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public PaginationFilterDto Input { get; set; }

    //}

    //public class GetAllStudentMasterListHandler : IRequestHandler<GetAllStudentMasterList, PaginatedList<AllSchoolStudentMasterData>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetAllStudentMasterListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<PaginatedList<AllSchoolStudentMasterData>> Handle(GetAllStudentMasterList request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            List<AllSchoolStudentMasterData> allSchoolStudentMasterDataList = new List<AllSchoolStudentMasterData>();

    //            var schoolStudentList = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TblDefSchoolStudentMasterDto>(_mapper.ConfigurationProvider)
    //                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

    //            return allSchoolStudentMasterDataList;

    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }


    //}

    #region GetStudentFeeHeaderByStuID
    public class GetStudentFeeHeaderByStuID : IRequest<PaginatedList<TblDefStudentFeeHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetStudentFeeHeaderByStuIDHandler : IRequestHandler<GetStudentFeeHeaderByStuID, PaginatedList<TblDefStudentFeeHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentFeeHeaderByStuIDHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentFeeHeaderDto>> Handle(GetStudentFeeHeaderByStuID request, CancellationToken cancellationToken)
        {
            try
            {
                var stuResult = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Input.Id);
                var studentFeeHeader = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider).Where(e => e.StuAdmNum == stuResult.StuAdmNum)
                                              .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                return studentFeeHeader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion

    #endregion
}