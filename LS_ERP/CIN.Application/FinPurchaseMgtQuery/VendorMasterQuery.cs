
using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
using CIN.Domain.OpeartionsMgt;
using CIN.Application.FinPurchaseMgtDto;

namespace CIN.Application.FinPurchaseMgtQuery
{   
    #region GetPagedList

    public class GetVendorsPagedList : IRequest<PaginatedList<TblSndDefVendorMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetVendorsPagedListHandler : IRequestHandler<GetVendorsPagedList, PaginatedList<TblSndDefVendorMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefVendorMasterDto>> Handle(GetVendorsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.VendorMasters.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.VendAddress1.Contains(search) ||
                            e.VendAddress2.Contains(search) ||
                            e.VendAlias.Contains(search) ||
                            e.VendArbName.Contains(search) ||
                            e.VendDefExpAcCode.Contains(search) ||
                            e.VendEmail1.Contains(search) ||
                            e.VendName.Contains(search) ||
                            e.VendPoArea.Contains(search) ||
                            e.VendCode.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefVendorMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem GetVendor by Id

    public class GetVendor : IRequest<TblSndDefVendorMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVendorHandler : IRequestHandler<GetVendor, TblSndDefVendorMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefVendorMasterDto> Handle(GetVendor request, CancellationToken cancellationToken)
        {
            TblSndDefVendorMasterDto obj = new();

            var Vendor = await _context.VendorMasters.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (Vendor is not null)
            {
                obj.VendCatCode = Vendor.VendCatCode;
                obj.VATNumber = Vendor.VATNumber;
                obj.CreatedOn = Vendor.CreatedOn;
                obj.VendAddress1 = Vendor.VendAddress1;
                obj.VendAddress2 = Vendor.VendAddress2;
                obj.VendAlias = Vendor.VendAlias;
                obj.VendAlloChkPay = Vendor.VendAlloChkPay;
                obj.VendAlloCrOverride = Vendor.VendAlloCrOverride;
                obj.VendAllowCrsale = Vendor.VendAllowCrsale;
                obj.VendARAc = Vendor.VendARAc;
                obj.VendArAcBranch = Vendor.VendArAcBranch;
                obj.VendArAcCode = Vendor.VendArAcCode;
                obj.VendARAdjAcCode = Vendor.VendARAdjAcCode;
                obj.VendArbName = Vendor.VendArbName;
                obj.VendARDiscAcCode = Vendor.VendARDiscAcCode;
                obj.VendCityCode1 = Vendor.VendCityCode1;
                obj.VendCityCode2 = Vendor.VendCityCode2;
                obj.VendCode = Vendor.VendCode;
                obj.VendContact1 = Vendor.VendContact1;
                obj.VendContact2 = Vendor.VendContact2;
                obj.VendCrLimit = Vendor.VendCrLimit;
                obj.VendDefExpAcCode = Vendor.VendDefExpAcCode;
                obj.VendDiscount = Vendor.VendDiscount;
                obj.VendEmail1 = Vendor.VendEmail1;
                obj.VendEmail2 = Vendor.VendEmail2;
                obj.VendIsVendor = Vendor.VendIsVendor;
                obj.VendLastPaidDate = Vendor.VendLastPaidDate;
                obj.VendLastPayAmt = Vendor.VendLastPayAmt;
                obj.VendLastPoDate = Vendor.VendLastPoDate;
                obj.VendMobile1 = Vendor.VendMobile1;
                obj.VendMobile2 = Vendor.VendMobile2;
                obj.VendName = Vendor.VendName;
                obj.VendOnHold = Vendor.VendOnHold;
                obj.VendPhone1 = Vendor.VendPhone1;
                obj.VendPhone2 = Vendor.VendPhone2;
                obj.VendPriceLevel = Vendor.VendPriceLevel;
                obj.VendRating = Vendor.VendRating;
                obj.VendPoArea = Vendor.VendPoArea;
                obj.VendPoRep = Vendor.VendPoRep;
                obj.VendSetPriceLevel = Vendor.VendSetPriceLevel;
                obj.VendType = Vendor.VendType;
                obj.VendUDF1 = Vendor.VendUDF1;
                obj.VendUDF2 = Vendor.VendUDF2;
                obj.VendUDF3 = Vendor.VendUDF3;
                obj.PoTermsCode = Vendor.PoTermsCode;
                obj.ModifiedOn = Vendor.ModifiedOn;
                obj.CreatedOn = Vendor.CreatedOn;
                obj.IsActive = Vendor.IsActive;
                obj.VendCrLimit = Vendor.VendCrLimit;
                obj.VendAvailCrLimit= Vendor.VendAvailCrLimit;
                obj.VendOutStandBal = Vendor.VendOutStandBal;
                obj.CrNumber = Vendor.CrNumber;
                obj.Iban = Vendor.Iban;

                return obj;
            }
            else return null;
        }

    }
    #endregion

    #region CreateUpdateVendor
    public class CreateVendor : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefVendorMasterDto VendorDto { get; set; }
    }

    public class CreateVendorHandler : IRequestHandler<CreateVendor, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateVendorHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateVendor request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateVendor method start----");


                var obj = request.VendorDto;

                TblSndDefVendorMaster Vendor = new();
                if (obj.Id > 0)
                    Vendor = await _context.VendorMasters.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                //else
                //{
                //    if (_context.VendorMasters.Any(x => x.VendCode == obj.VendCode))
                //    {
                //        return -1;
                //    }
                //    Vendor.VendCode = obj.VendCode.ToUpper();
                //}

                Vendor.VendARAc = obj.VendARAc;
                Vendor.VendOutStandBal = obj.VendOutStandBal ?? 0;
                Vendor.VendAvailCrLimit = obj.VendAvailCrLimit ?? 0;
                Vendor.VendArAcBranch = obj.VendArAcBranch;
                Vendor.VendArAcCode = obj.VendArAcCode;
                Vendor.VendARAdjAcCode = obj.VendARAdjAcCode;
                Vendor.VendArbName = obj.VendArbName;
                Vendor.VendARDiscAcCode = obj.VendARDiscAcCode;
                Vendor.VendCatCode = obj.VendCatCode;
                Vendor.VendCityCode1 = obj.VendCityCode1;
                Vendor.VendCityCode2 = obj.VendCityCode2.HasValue() ? obj.VendCityCode2 : null ;
                Vendor.VendAlias = obj.VendAlias;
                Vendor.VendContact1 = obj.VendContact1;
                Vendor.VendContact2 = obj.VendContact2;
                Vendor.VendCrLimit = obj.VendCrLimit;
                Vendor.VendDefExpAcCode = obj.VendDefExpAcCode;
                Vendor.VendDiscount = obj.VendDiscount;
                Vendor.VendEmail1 = obj.VendEmail1;
                Vendor.VendEmail2 = obj.VendEmail2;
                Vendor.VendIsVendor = obj.VendIsVendor;
                Vendor.VendLastPaidDate = obj.VendLastPaidDate;
                Vendor.VendLastPayAmt = obj.VendLastPayAmt;
                Vendor.VendLastPoDate = obj.VendLastPoDate;
                Vendor.VendMobile1 = obj.VendMobile1;
                Vendor.VendMobile2 = obj.VendMobile2;
                Vendor.VendName = obj.VendName;
                Vendor.VendOnHold = obj.VendOnHold;
                Vendor.VendPhone1 = obj.VendPhone1;
                Vendor.VendPhone2 = obj.VendPhone2;
                Vendor.VendPriceLevel = 1000;
                Vendor.VendRating = obj.VendRating;
                Vendor.VendPoArea = obj.VendPoArea;
                Vendor.VendPoRep = obj.VendPoRep;
                Vendor.VendSetPriceLevel = obj.VendSetPriceLevel;
                Vendor.VendType = obj.VendType;
                Vendor.VendUDF1 = obj.VendUDF1;
                Vendor.VendUDF2 = obj.VendUDF2;
                Vendor.VendUDF3 = obj.VendUDF3;
                Vendor.VendCrLimit = obj.VendCrLimit;
                Vendor.VATNumber = obj.VATNumber;
                Vendor.CrNumber = obj.CrNumber;
                Vendor.Iban = obj.Iban;
                
                
                
                //Vendor.Id = obj.Id;
                // Vendor.ModifiedOn = obj.ModifiedOn;
                Vendor.PoTermsCode = obj.PoTermsCode;
                Vendor.VendAddress1 = obj.VendAddress1;
                Vendor.VendAddress2 = obj.VendAddress2;

                Vendor.IsActive = obj.IsActive;


                if (obj.Id > 0)
                {
                    Vendor.ModifiedOn = DateTime.Now;
                    _context.VendorMasters.Update(Vendor);
                }
                else
                {
                    string vendCode = obj.VendCode;
                    var purchaseConfig = await _context.PurchaseConfigs.FirstOrDefaultAsync();

                    if (purchaseConfig is not null && purchaseConfig.AutoGenCustCode)
                    {
                        var lastCustSeq = await _context.PopVendorCategories.OrderByDescending(e => e.LastSeq).Select(e => e.LastSeq).FirstOrDefaultAsync();
                        var vendCatg = await _context.PopVendorCategories.Where(e => e.VenCatCode == obj.VendCatCode).FirstOrDefaultAsync();

                        //int LastSeq = custCateg.LastSeq;
                        int LastSeq = lastCustSeq;

                        LastSeq = LastSeq + 1;
                        string custSeq = LastSeq.ToString(GetPrefixLen(purchaseConfig.VendLength));

                        if (purchaseConfig.PrefixCatCode)
                            vendCode = (vendCatg.CatPrefix ?? string.Empty) + "" + custSeq;
                        else
                            vendCode = custSeq;

                        vendCatg.LastSeq = LastSeq;
                        _context.PopVendorCategories.Update(vendCatg);
                        await _context.SaveChangesAsync();
                    }
                    
                    Vendor.VendCode = vendCode.ToUpper();
                    Vendor.CreatedOn = DateTime.Now;
                    await _context.VendorMasters.AddAsync(Vendor);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateVendor method Exit----");
                return Vendor.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateVendor Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
        private string GetPrefixLen(short len) => "0000000000".Substring(0, len);
    }



    #endregion CreateUpdateVendor

    #region Get VendorSelectList

    public class GetVendorSelectItemList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetVendorSelectItemListHandler : IRequestHandler<GetVendorSelectItemList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorSelectItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVendorSelectItemList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.VendorMasters.AsNoTracking()
                .Where(e => ( e.VendName.Contains(search) || e.VendCode.Contains(search)) && e.IsActive)
              .Select(e => new CustomSelectListItem { Text = e.VendCode, TextTwo = e.VendArbName, Value = e.VendCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region DeleteVendor
    public class DeleteVendor : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVendorQueryHandler : IRequestHandler<DeleteVendor, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVendorQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVendor request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteVendor start----");

                if (request.Id > 0)
                {
                    var vend = await _context.VendorMasters.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(vend);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteVendor");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region GetVendorByVendorCode
    public class GetVendorByVendorCode : IRequest<TblSndDefVendorMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string VendorCode { get; set; }
    }

    public class GetVendorByVendorCodeHandler : IRequestHandler<GetVendorByVendorCode, TblSndDefVendorMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorByVendorCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefVendorMasterDto> Handle(GetVendorByVendorCode request, CancellationToken cancellationToken)
        {
            TblSndDefVendorMasterDto obj = new();
            var Vendor = await _context.VendorMasters.AsNoTracking().FirstOrDefaultAsync(e => e.VendCode == request.VendorCode);
            if (Vendor is not null)
            {
                obj.Id = Vendor.Id;
                obj.VendCode = Vendor.VendCode;
                obj.VendName = Vendor.VendName;
                obj.VendArbName = Vendor.VendArbName;
                obj.VendAlias = Vendor.VendAlias;
                obj.VendType = Vendor.VendType;
                obj.VendCatCode = Vendor.VendCatCode;
                obj.VendRating = Vendor.VendRating;
                obj.PoTermsCode = Vendor.PoTermsCode;
                obj.VendDiscount = Vendor.VendDiscount;
                obj.VendCrLimit = Vendor.VendCrLimit;
                obj.VendPoRep = Vendor.VendPoRep;
                obj.VendPoArea = Vendor.VendPoArea;
                obj.VendARAc = Vendor.VendARAc;
                obj.VendLastPaidDate = Vendor.VendLastPaidDate;
                obj.VendLastPoDate = Vendor.VendLastPoDate;
                obj.VendLastPayAmt = Vendor.VendLastPayAmt;
                obj.VendAddress1 = Vendor.VendAddress1;
                obj.VendMobile1 = Vendor.VendMobile1;
                obj.VendPhone1 = Vendor.VendPhone1;
                obj.VendEmail1 = Vendor.VendEmail1;
                obj.VendContact1 = Vendor.VendContact1;
                obj.VendAddress2 = Vendor.VendAddress2;
                obj.VendMobile2 = Vendor.VendMobile2;
                obj.VendPhone2 = Vendor.VendPhone2;
                obj.VendEmail2 = Vendor.VendEmail2;
                obj.VendContact2 = Vendor.VendContact2;
                obj.VendUDF1 = Vendor.VendUDF1;
                obj.VendUDF2 = Vendor.VendUDF2;
                obj.VendUDF3 = Vendor.VendUDF3;
                obj.VendAllowCrsale = Vendor.VendAllowCrsale;
                obj.VendAlloCrOverride = Vendor.VendAlloCrOverride;
                obj.VendOnHold = Vendor.VendOnHold;
                obj.VendAlloChkPay = Vendor.VendAlloChkPay;
                obj.VendSetPriceLevel = Vendor.VendSetPriceLevel;
                obj.VendPriceLevel = Vendor.VendPriceLevel;
                obj.VendIsVendor = Vendor.VendIsVendor;
                obj.VendArAcBranch = Vendor.VendArAcBranch;
                obj.VendArAcCode = Vendor.VendArAcCode;
                obj.VendDefExpAcCode = Vendor.VendDefExpAcCode;
                obj.VendARAdjAcCode = Vendor.VendARAdjAcCode;
                obj.VendARDiscAcCode = Vendor.VendARDiscAcCode;
                obj.ModifiedOn = Vendor.ModifiedOn;
                obj.CreatedOn = Vendor.CreatedOn;
                obj.IsActive = Vendor.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion
}
