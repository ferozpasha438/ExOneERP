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

namespace CIN.Application.OperationsMgtQuery
{
    public class CustomerMasterQuery
    {
    }
    #region GetPagedList

    public class GetCustomersPagedList : IRequest<PaginatedList<TblSndDefCustomerMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCustomersPagedListHandler : IRequestHandler<GetCustomersPagedList, PaginatedList<TblSndDefCustomerMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomersPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefCustomerMasterDto>> Handle(GetCustomersPagedList request, CancellationToken cancellationToken)
        {
            try
            {

                var search = request.Input.Query;
                var query = request.Input;
                var list = _context.OprCustomers.AsNoTracking()
                  .Where(e => //e.CompanyId == request.CompanyId &&
                                (e.CustAddress1.Contains(search) ||
                                e.CustAddress2.Contains(search) ||
                                e.CustAlias.Contains(search) ||
                                e.CustArbName.Contains(search) ||
                                e.CustDefExpAcCode.Contains(search) ||
                                e.CustEmail1.Contains(search) ||
                                e.CustName.Contains(search) ||
                                e.CustSalesArea.Contains(search) || e.CustCode.Contains(search) ||
                                e.CustCityCode1.Contains(search) ||
                                e.CustCityCode2.Contains(search) ||
                                e.CustCatCode.Contains(search) ||
                                e.CustMobile1.Contains(search) ||
                                e.CustMobile2.Contains(search) ||
                                search == "" || search == null));

                if (query.StatusId is not null && query.StatusId == 11)
                {
                    if (query.Approval == "chk")
                        list = list.Where(e => e.IsActive == true);
                    else if (query.Approval == "unchk")
                        list = list.Where(e => e.IsActive == false);
                }

                list = list.OrderBy(request.Input.OrderBy);

                var result = await list.ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                throw;
            }
        }
    }

    #endregion

    #region SingleItem GetCustomer by Id

    public class GetCustomer : IRequest<TblSndDefCustomerMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCustomerHandler : IRequestHandler<GetCustomer, TblSndDefCustomerMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefCustomerMasterDto> Handle(GetCustomer request, CancellationToken cancellationToken)
        {
            TblSndDefCustomerMasterDto obj = new();

            obj = await _context.OprCustomers.AsNoTracking().ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            //if (customer is not null)
            //{
            //    obj.CustCatCode = customer.CustCatCode;
            //    obj.CreatedOn = customer.CreatedOn;
            //    obj.CustAddress1 = customer.CustAddress1;
            //    obj.CustAddress2 = customer.CustAddress2;
            //    obj.CustAlias = customer.CustAlias;
            //    obj.CustAlloChkPay = customer.CustAlloChkPay;
            //    obj.CustAlloCrOverride = customer.CustAlloCrOverride;
            //    obj.CustAllowCrsale = customer.CustAllowCrsale;
            //    obj.CustARAc = customer.CustARAc;
            //    obj.CustArAcBranch = customer.CustArAcBranch;
            //    obj.CustArAcCode = customer.CustArAcCode;
            //    obj.CustARAdjAcCode = customer.CustARAdjAcCode;
            //    obj.CustArbName = customer.CustArbName;
            //    obj.CustARDiscAcCode = customer.CustARDiscAcCode;
            //    obj.CustCityCode1 = customer.CustCityCode1;
            //    obj.CustCityCode2 = customer.CustCityCode2;
            //    obj.CustCode = customer.CustCode;
            //    obj.CustContact1 = customer.CustContact1;
            //    obj.CustContact2 = customer.CustContact2;
            //    obj.CustCrLimit = customer.CustCrLimit;
            //    obj.CustDefExpAcCode = customer.CustDefExpAcCode;
            //    obj.CustDiscount = customer.CustDiscount;
            //    obj.CustEmail1 = customer.CustEmail1;
            //    obj.CustEmail2 = customer.CustEmail2;
            //    obj.CustIsVendor = customer.CustIsVendor;
            //    obj.CustLastPaidDate = customer.CustLastPaidDate;
            //    obj.CustLastPayAmt = customer.CustLastPayAmt;
            //    obj.CustLastSalesDate = customer.CustLastSalesDate;
            //    obj.CustMobile1 = customer.CustMobile1;
            //    obj.CustMobile2 = customer.CustMobile2;
            //    obj.CustName = customer.CustName;
            //    obj.CustOnHold = customer.CustOnHold;
            //    obj.CustPhone1 = customer.CustPhone1;
            //    obj.CustPhone2 = customer.CustPhone2;
            //    obj.CustPriceLevel = customer.CustPriceLevel;
            //    obj.CustRating = customer.CustRating;
            //    obj.CustSalesArea = customer.CustSalesArea;
            //    obj.CustSalesRep = customer.CustSalesRep;
            //    obj.CustSetPriceLevel = customer.CustSetPriceLevel;
            //    obj.CustType = customer.CustType;
            //    obj.CustUDF1 = customer.CustUDF1;
            //    obj.CustUDF2 = customer.CustUDF2;
            //    obj.CustUDF3 = customer.CustUDF3;
            //    obj.SalesTermsCode = customer.SalesTermsCode;
            //    obj.ModifiedOn = customer.ModifiedOn;
            //    obj.CreatedOn = customer.CreatedOn;
            //    obj.IsActive = customer.IsActive;
            //    return obj;
            //}
            return obj;
        }

    }
    #endregion

    #region CreateUpdateCustomer
    public class CreateCustomer : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefCustomerMasterDto CustomerDto { get; set; }
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomer, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCustomer request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateCustomer method start----");


                var obj = request.CustomerDto;

                TblSndDefCustomerMaster customer = new();
                if (obj.Id > 0)
                    customer = await _context.OprCustomers.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    if (_context.OprCustomers.Any(x => x.CustCode == obj.CustCode))
                    {
                        return -1;
                    }
                    customer.CustCode = obj.CustCode.ToUpper();
                }

                customer.CustARAc = obj.CustARAc;
                customer.CustOutStandBal = obj.CustOutStandBal ?? 0;
                customer.CustAvailCrLimit = obj.CustAvailCrLimit ?? 0;
                customer.CustArAcBranch = obj.CustArAcBranch;
                customer.CustArAcCode = obj.CustArAcCode;
                customer.CustARAdjAcCode = obj.CustARAdjAcCode;
                customer.CustArbName = obj.CustArbName;
                customer.CustARDiscAcCode = obj.CustARDiscAcCode;
                customer.CustCatCode = obj.CustCatCode;
                customer.CustCityCode1 = obj.CustCityCode1;
                customer.CustCityCode2 = obj.CustCityCode2.HasValue() ? obj.CustCityCode2 : null;
                customer.CustAlias = obj.CustAlias;
                customer.CustContact1 = obj.CustContact1;
                customer.CustContact2 = obj.CustContact2;
                customer.CustCrLimit = obj.CustCrLimit;
                customer.CustDefExpAcCode = obj.CustDefExpAcCode;
                customer.CustDiscount = obj.CustDiscount;
                customer.CustEmail1 = obj.CustEmail1;
                customer.CustEmail2 = obj.CustEmail2;
                customer.CustIsVendor = obj.CustIsVendor;
                customer.CustLastPaidDate = obj.CustLastPaidDate;
                customer.CustLastPayAmt = obj.CustLastPayAmt;
                customer.CustLastSalesDate = obj.CustLastSalesDate;
                customer.CustMobile1 = obj.CustMobile1;
                customer.CustMobile2 = obj.CustMobile2;
                customer.CustName = obj.CustName;
                customer.CustOnHold = obj.CustOnHold;
                customer.CustPhone1 = obj.CustPhone1;
                customer.CustPhone2 = obj.CustPhone2;
                customer.CustPriceLevel = obj.CustPriceLevel;                         //staticaly Assigned in angular
                customer.CustRating = obj.CustRating;
                customer.CustSalesArea = obj.CustSalesArea;
                customer.CustSalesRep = obj.CustSalesRep;
                customer.CustSetPriceLevel = obj.CustSetPriceLevel;
                customer.CustType = obj.CustType;
                customer.CustUDF1 = obj.CustUDF1;
                customer.CustUDF2 = obj.CustUDF2;
                customer.CustUDF3 = obj.CustUDF3;
                customer.VATNumber = obj.VATNumber;

                customer.SalesTermsCode = obj.SalesTermsCode;
                customer.CustAddress1 = obj.CustAddress1;
                customer.CustAddress2 = obj.CustAddress2;

                customer.IsActive = obj.IsActive;

                customer.CustAlloChkPay = obj.CustAlloChkPay;
                customer.CustAlloCrOverride = obj.CustAlloCrOverride;
                customer.CustAllowCrsale = obj.CustAllowCrsale;
                customer.CrNumber = obj.CrNumber;
                customer.CustNameAliasEn = obj.CustNameAliasEn;
                customer.CustNameAliasAr = obj.CustNameAliasAr;


                if (obj.Id > 0)
                {
                    customer.ModifiedOn = DateTime.Now;
                    _context.OprCustomers.Update(customer);
                }
                else
                {
                    string custCode = obj.CustCode;
                    var salesConfig = await _context.SalesConfigs.FirstOrDefaultAsync();

                    if (salesConfig is not null && salesConfig.AutoGenCustCode)
                    {
                        var lastCustSeq = await _context.SndCustomerCategories.OrderByDescending(e => e.LastSeq).Select(e => e.LastSeq).FirstOrDefaultAsync();
                        var custCateg = await _context.SndCustomerCategories.Where(e => e.CustCatCode == obj.CustCatCode).FirstOrDefaultAsync();

                        //int LastSeq = custCateg.LastSeq;
                        int LastSeq = lastCustSeq;

                        LastSeq = LastSeq + 1;
                        string custSeq = LastSeq.ToString(GetPrefixLen(salesConfig.CustLength));

                        if (salesConfig.PrefixCatCode)
                            custCode = (custCateg.CatPrefix ?? string.Empty) + "" + custSeq;
                        else
                            custCode = custSeq;

                        custCateg.LastSeq = LastSeq;
                        _context.SndCustomerCategories.Update(custCateg);
                        await _context.SaveChangesAsync();
                    }

                    customer.CustCode = custCode.ToUpper();
                    customer.CreatedOn = DateTime.Now;
                    await _context.OprCustomers.AddAsync(customer);

                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateCustomer method Exit----");
                return customer.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateCustomer Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
        private string GetPrefixLen(short len) => "0000000000".Substring(0, len);
    }



    #endregion CreateUpdateCustomer

    #region Get CustomerSelectList

    public class GetCustomerSelectItemList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCustomerSelectItemListHandler : IRequestHandler<GetCustomerSelectItemList, List<LanCustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerSelectItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetCustomerSelectItemList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprCustomers.AsNoTracking()
                .Where(e => e.CustName.Contains(search) || e.CustArbName.Contains(search) || e.CustCode.Contains(search) || search == null)
              .Select(e => new LanCustomSelectListItem { Text = e.CustName, Value = e.CustCode.ToString(), TextTwo = e.CustArbName })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion



    #region DeleteCustomer
    public class DeleteCustomer : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCustomerQueryHandler : IRequestHandler<DeleteCustomer, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCustomer request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteCustomer start----");

                if (request.Id > 0)
                {
                    var cust = await _context.OprCustomers.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(cust);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteCustomer");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region GetCustomerByCustomerCode
    public class GetCustomerByCustomerCode : IRequest<TblSndDefCustomerMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetCustomerByCustomerCodeHandler : IRequestHandler<GetCustomerByCustomerCode, TblSndDefCustomerMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerByCustomerCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefCustomerMasterDto> Handle(GetCustomerByCustomerCode request, CancellationToken cancellationToken)
        {
            TblSndDefCustomerMasterDto obj = new();
            var Customer = await _context.OprCustomers.AsNoTracking().ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.CustCode == request.CustomerCode);

            return Customer;

        }
    }

    #endregion


    #region CanAutoGenerateCustCode

    public class CanAutoGenerateCustCode : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }

    }

    public class CanAutoGenerateCustCodeHandler : IRequestHandler<CanAutoGenerateCustCode, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CanAutoGenerateCustCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CanAutoGenerateCustCode request, CancellationToken cancellationToken)
        {
            var item = await _context.SalesConfigs.AsNoTracking()
                .FirstOrDefaultAsync();

            return item?.AutoGenCustCode ?? false;
        }
    }

    #endregion



    #region GetCustomersPagedListWithFilter

    public class GetCustomersPagedListWithFilter : IRequest<PaginatedList<TblSndDefCustomerMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public OprFilter FilterInput { get; set; }

    }

    public class GetCustomersPagedListWithFilterHandler : IRequestHandler<GetCustomersPagedListWithFilter, PaginatedList<TblSndDefCustomerMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomersPagedListWithFilterHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefCustomerMasterDto>> Handle(GetCustomersPagedListWithFilter request, CancellationToken cancellationToken)
        {
            try
            {

                var search = request.Input.Query;
                var list = _context.OprCustomers.Where(e => //e.CompanyId == request.CompanyId &&
                                (e.CustAddress1.Contains(search) ||
                                e.CustAddress2.Contains(search) ||
                                e.CustAlias.Contains(search) ||
                                e.CustArbName.Contains(search) ||
                                e.CustDefExpAcCode.Contains(search) ||
                                e.CustEmail1.Contains(search) ||
                                e.CustName.Contains(search) ||
                                e.CustSalesArea.Contains(search) || e.CustCode.Contains(search) ||
                                e.CustCityCode1.Contains(search) ||
                                e.CustCityCode2.Contains(search) ||
                                e.CustCatCode.Contains(search) ||
                                e.CustMobile1.Contains(search) ||
                                e.CustMobile2.Contains(search))
                                || search == null || search == ""
                                ).Select(x => new TblSndDefCustomerMasterDto
                                {
                                    Id = x.Id,
                                    CustCode = x.CustCode,
                                    CustName = x.CustName,
                                    CustArbName = x.CustArbName,
                                    CustCatCode = x.CustCatCode,
                                    CustCityCode1 = x.CustCityCode1,
                                    CustCityCode2 = x.CustCityCode2,
                                    CustMobile1 = x.CustMobile1,
                                    CustMobile2 = x.CustMobile2,
                                    CustPhone1 = x.CustPhone1,
                                    CustPhone2 = x.CustPhone2,
                                    CustContact1 = x.CustContact1,
                                    CustContact2 = x.CustContact2,
                                    CustSalesRep = x.CustSalesRep,
                                    IsActive=x.IsActive,
                                    NumberOfSites = _context.OprSites.Where(c => c.CustomerCode == x.CustCode).Count()
                                });
                if(!string.IsNullOrEmpty(request.FilterInput.BranchCode))
                {
                    list = list.Where(e => e.CustCityCode1 == request.FilterInput.BranchCode);
                }

                if (request.FilterInput.IsActive is not null)
                {
                    list = list.Where(e => e.IsActive == request.FilterInput.IsActive.Value);
                }


                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                return nreports;
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                throw;
            }
        }
    }

    #endregion
}
