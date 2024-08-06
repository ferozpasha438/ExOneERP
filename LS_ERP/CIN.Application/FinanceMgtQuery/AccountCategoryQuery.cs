using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using log4net.Config;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{

    #region SingleItem

    public class GetAccountCode : IRequest<TblFinDefMainAccountsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAccountCodeHandler : IRequestHandler<GetAccountCode, TblFinDefMainAccountsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinDefMainAccountsDto> Handle(GetAccountCode request, CancellationToken cancellationToken)
        {
            var item = await _context.FinMainAccounts.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblFinDefMainAccountsDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion


    #region GetSelectMainAccountsList
    public class GetSelectAccountCategoryList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectMainAccountsListHandler : IRequestHandler<GetSelectAccountCategoryList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectMainAccountsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectAccountCategoryList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.FinMainAccounts
                .Where(e=>e.Status == true)
                .AsNoTracking()
                .Where(e => e.FinAcCode.Contains(search) || e.FinAcName.Contains(search))
              .Select(e => new CustomSelectListItem
              {

                  Text = "(" + e.FinAcCode + ") " + e.FinAcName,
                  TextTwo = e.FinAcCode,
                  Value = e.FinAcCode,
                  //Value = "(" + e.FinAcCode + ") " + e.FinAcName,

                  /*Text = e.FinAcCode, Value = e.FinAcCode.ToString()*/
              })
                 .ToListAsync(cancellationToken);
            return list;
        }        
    }
    #endregion


    #region CheckAccountCode

    public class CheckAccountCode : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class CheckAccountCodeHandler : IRequestHandler<CheckAccountCode, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckAccountCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckAccountCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckAccountCode method start----");
            if (request.Input.HasValue())
                return await _context.FinMainAccounts.AnyAsync(e => e.FinAcCode == request.Input.Trim());
            return false;
        }
    }

    #endregion


    #region GetCategoryTypeList

    public class GetCategoryTypeList : IRequest<AccountRootCategoryListDto>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCategoryTypeListHandler : IRequestHandler<GetCategoryTypeList, AccountRootCategoryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountRootCategoryListDto> Handle(GetCategoryTypeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCategoryTypeList method start----");

            var accTypes = await _context.FinSysAccountTypes.AsNoTracking().OrderBy(e => e.TypeBal).ToListAsync();
            //var subCategories = _context.FinAccountSubCategories.AsNoTracking();
            //var accCodes = _context.FinMainAccounts.AsNoTracking();

            Log.Info("----Info GetCategoryTypeList method Ends----");
            List<AccountRootCategoryDto> assetList = new();

            foreach (var item in accTypes)
            {
                assetList.Add(new AccountRootCategoryDto { Name = item.TypeCode, List = await GetCatGListForTypeId(item.TypeCode) });
            }

            var finSetup = await _context.FinSysFinanialSetups.FirstOrDefaultAsync();

            return new AccountRootCategoryListDto { List = assetList, FInSysGenAcCode = finSetup?.FInSysGenAcCode ?? false };
            //return new AccountRootCategoryListDto {
            //new(){ Name = nameof(AcCategoryEnumType.Asset),
            //    List = await GetCatGListForTypeId(nameof(AcCategoryEnumType.Asset)) },
            //new(){ Name = nameof(AcCategoryEnumType.Liability), List = await GetCatGListForTypeId(nameof(AcCategoryEnumType.Liability)) },
            //new(){ Name = nameof(AcCategoryEnumType.Expense), List = await GetCatGListForTypeId(nameof(AcCategoryEnumType.Expense)) },
            //new(){ Name = nameof(AcCategoryEnumType.Income), List = await GetCatGListForTypeId(nameof(AcCategoryEnumType.Income))},
            //};
        }

        private async Task<List<AccountCategoryDto>> GetCatGListForTypeId(string catTypeId)
        {
            var categories = _context.FinAccountCategories.AsNoTracking();
            var subCategories = _context.FinAccountSubCategories.AsNoTracking();
            var accCodes = _context.FinMainAccounts.AsNoTracking();
            List<AccountCategoryDto> acCatGList = new();
            var catGList = await categories.Where(e => e.FinType == catTypeId).OrderBy(e => e.Id).Select(e => new { e.FinCatCode, e.FinCatName }).ToListAsync();
            if (catGList.Any())
            {
                AccountCategoryDto accountCategoryDto = new();
                foreach (var catItem in catGList)
                {
                    accountCategoryDto = new() { FinCatCode = catItem.FinCatCode, FinCatName = catItem.FinCatName };
                    var subCatGList = await subCategories.Where(e => e.FinCatCode == catItem.FinCatCode).Select(e => new AccountSubCategoryDto { FinCatCode = e.FinCatCode, FinSubCatCode = e.FinSubCatCode, FinSubCatName = e.FinSubCatName })
                        .ToListAsync();
                    if (subCatGList.Any())
                    {
                        List<AccountSubCategoryDto> sCatItemList = new();
                        foreach (var subCatItem in subCatGList)
                        {
                            AccountSubCategoryDto sCatItem = new();
                            var acCodes = await accCodes.Where(e => e.FinSubCatCode == subCatItem.FinSubCatCode)
                                .Select(e => new AccountAccountCodeDto { FinAcName = e.FinAcName, FinAcCode = e.FinAcCode, Id = e.Id })
                                .ToListAsync();
                            sCatItem = new() { FinCatCode = catItem.FinCatCode, FinSubCatCode = subCatItem.FinSubCatCode, FinSubCatName = subCatItem.FinSubCatName, List = acCodes };
                            sCatItemList.Add(sCatItem);
                        }
                        accountCategoryDto.List = sCatItemList;
                    }
                    acCatGList.Add(accountCategoryDto);
                }
            }
            return acCatGList;
        }
    }




    #endregion


    #region GetSelectAcCategoryList

    public class GetSelectAcCategoryList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectAcCategoryListHandler : IRequestHandler<GetSelectAcCategoryList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectAcCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectAcCategoryList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectAcCategoryList method start----");
            var item = await _context.FinAccountCategories.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinCatName, Value = e.FinCatCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectAcCategoryList method Ends----");
            return item;
        }
    }

    #endregion


    #region GetSelectMainAccountList

    public class GetSelectMainAccountList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool FinIsIntegrationAC { get; set; }
        public bool IsAutoComplete { get; set; }
        public string Search { get; set; }
    }

    public class GetSelectMainAccountListHandler : IRequestHandler<GetSelectMainAccountList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectMainAccountListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectMainAccountList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectMainAccountList method start----");
            var obj = _context.FinMainAccounts
                .Where(e => e.Status == true)
                .AsNoTracking();
            if (request.IsAutoComplete)
                obj = obj.Where(e => //e.FinIsIntegrationAC == request.FinIsIntegrationAC);
            (e.FinAcCode.Contains(request.Search))
            || e.FinAcName.Contains(request.Search));

            var newObj = await obj
               .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinAcName, TextTwo = e.FinAcCode, Value = "(" + e.FinAcCode + ") " + e.FinAcName, })
                  .ToListAsync(cancellationToken);

            Log.Info("----Info GetSelectMainAccountList method Ends----");
            return newObj;
        }
    }

    #endregion


    #region GetAcCategoryList

    public class GetAcCategoryList : IRequest<List<TblFinDefAccountSubCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAcCategoryListHandler : IRequestHandler<GetAcCategoryList, List<TblFinDefAccountSubCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinDefAccountSubCategoryDto>> Handle(GetAcCategoryList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAcCategoryList method start----");
            var item = await _context.FinAccountCategories.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .ProjectTo<TblFinDefAccountSubCategoryDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAcCategoryList method Ends----");
            return item;
        }
    }

    #endregion

    #region CreateAcCategory

    public class CreateAcCategory : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinDefAccountCategoryDto Input { get; set; }
    }

    public class CreateAcCategoryQueryHandler : IRequestHandler<CreateAcCategory, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAcCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAcCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateAcCategoryQuery method start----");

                var obj = request.Input;
                TblFinDefAccountCategory cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.FinAccountCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.FinType = obj.FinType;
                cObj.FinCatName = obj.FinCatName;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.FinAccountCategories.Update(cObj);
                }
                else
                {
                    //if (await _context.FinAccountCategories.AnyAsync(e => e.FinCatName == obj.FinCatName))
                    //    return ApiMessageInfo.DuplicateInfo(obj.FinCatName);

                    cObj.FinCatCode = obj.FinCatCode.ToUpper();
                    var catGCount = await _context.FinAccountCategories.Where(e => e.FinType == obj.FinType).CountAsync();
                    cObj.CreatedOn = DateTime.Now;

                    cObj.FinCatLastSeq = (short)(catGCount + 1);
                    await _context.FinAccountCategories.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateAcCategoryQuery method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateAcCategoryQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion

    #region CreateAcSubCategory

    public class CreateAcSubCategory : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinDefAccountSubCategoryDto Input { get; set; }
    }

    public class CreateAcSubCategoryQueryHandler : IRequestHandler<CreateAcSubCategory, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAcSubCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAcSubCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateAcSubCategoryQuery method start----");

                var obj = request.Input;
                TblFinDefAccountSubCategory cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.FinAccountSubCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.FinCatCode = obj.FinCatCode.ToUpper();
                cObj.FinSubCatName = obj.FinSubCatName;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.FinAccountSubCategories.Update(cObj);
                }
                else
                {
                    //if (await _context.FinAccountSubCategories.AnyAsync(e => e.FinSubCatCode == obj.FinSubCatCode))
                    //    return ApiMessageInfo.DuplicateInfo(obj.FinSubCatCode);

                    cObj.FinSubCatCode = obj.FinSubCatCode.ToUpper();
                    var subCount = await _context.FinAccountSubCategories.CountAsync(e => e.FinCatCode == obj.FinCatCode);
                    cObj.CreatedOn = DateTime.Now;
                    cObj.FinSubCatLastSeq = (short)(subCount + 1);
                    await _context.FinAccountSubCategories.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateAcSubCategoryQuery method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateAcSubCategoryQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion

    #region CreateAccountCode

    public class CreateAccountCode : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinDefMainAccountsDto Input { get; set; }
    }

    public class CreateAccountCodeQueryHandler : IRequestHandler<CreateAccountCode, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAccountCodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAccountCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateAccountCodeQuery method start----");

                var obj = request.Input;
                TblFinDefMainAccounts cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.FinMainAccounts.FirstOrDefaultAsync(e => e.Id == obj.Id);

                var finSetUp = await _context.FinSysFinanialSetups.OrderBy(e => e.Id).FirstOrDefaultAsync();


                cObj.FinAcName = obj.FinAcName;
                cObj.FinAcAlias = obj.FinAcAlias;
                cObj.FinAcDesc = obj.FinAcDesc;
                cObj.FinIsPayCode = obj.FinIsPayCode;
                cObj.FinPayCodetype = obj.FinPayCodetype;
                cObj.FinIsIntegrationAC = obj.FinIsIntegrationAC;
                cObj.FinIsRevenue = obj.FinIsRevenue;
                cObj.FinIsRevenuetype = obj.FinIsRevenuetype;
                cObj.Status = obj.Status;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.FinMainAccounts.Update(cObj);
                }
                else
                {
                    var accCodeCount = await _context.FinMainAccounts.CountAsync(e => e.FinSubCatCode == obj.FinSubCatCode);
                    var finSubCategory = await _context.FinAccountSubCategories.FirstOrDefaultAsync(e => e.FinSubCatCode == obj.FinSubCatCode);
                    var finCategory = await _context.FinAccountCategories.FirstOrDefaultAsync(e => e.FinCatCode == finSubCategory.FinCatCode);

                    cObj.Fintype = finCategory.FinType;
                    cObj.FinCatCode = finSubCategory.FinCatCode;
                    cObj.FinSubCatCode = obj.FinSubCatCode;
                    cObj.CreatedOn = DateTime.Now;
                    cObj.FinActLastSeq = (short)(accCodeCount + 1);

                    if (finSetUp.FInSysGenAcCode)
                    {

                        var accountType = await _context.FinSysAccountTypes.FirstOrDefaultAsync(e => e.TypeCode == finCategory.FinType);
                        cObj.FinAcCode = accountType.TypeBal.Trim() + "" + finCategory.FinCatLastSeq.ToString(GetPrefixLen(finSetUp.FinAcCatLen)) + "" + finSubCategory.FinSubCatLastSeq.ToString(GetPrefixLen(finSetUp.FinAcSubCatLen)) + "" + cObj.FinActLastSeq.ToString(GetPrefixLen(finSetUp.FinAcLen));
                        //cObj.FinAcCode = $"{accountType.TypeBal.Trim()}{finCategory.FinCatLastSeq:}{finSubCategory.FinSubCatLastSeq:0#}{cObj.FinActLastSeq:0##}";
                    }
                    else
                    {
                        cObj.FinAcCode = obj.FinAcCode;
                        //var segment = await _context.AcCodeSegments.ToListAsync();
                        //if (segment.Any())
                        //{
                        //    short seg1 = segment.FirstOrDefault(e => e.Type.ToLower() == "segment1").Segment;
                        //    short seg2 = segment.FirstOrDefault(e => e.Type.ToLower() == "segment2").Segment;

                        //    cObj.FinAcCode = (seg1, seg2) switch
                        //    {
                        //        (1, 2) => $"01-23-{obj.FinAcCode}",
                        //        (2, 1) => $"23-01-{obj.FinAcCode}",
                        //        (2, 3) => $"{obj.FinAcCode}-01-23",
                        //        (3, 2) => $"{obj.FinAcCode}-23-01",
                        //        (1, 3) => $"01-{obj.FinAcCode}-23",
                        //        (3, 1) => $"23-{obj.FinAcCode}-01",
                        //        (_, _) => $"01-23-{obj.FinAcCode}"
                        //    };
                        //}
                        //else
                        //    cObj.FinAcCode = $"01-23-{obj.FinAcCode}";
                    }

                    await _context.FinMainAccounts.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateAccountCodeQuery method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateAccountCodeQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }

        private string GetPrefixLen(short len) => "0000000000".Substring(0, len);
        //private string GetPrefixLen(short len) => "0##########".Substring(0, len+1);
    }
    #endregion
}
