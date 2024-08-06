using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.DB;
using CIN.Domain.InventorySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventoryQuery
{
    #region GetPagedList

    public class GetCategoryList : IRequest<PaginatedList<tblInvDefCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCategoryListHandler : IRequestHandler<GetCategoryList, PaginatedList<tblInvDefCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<tblInvDefCategoryDto>> Handle(GetCategoryList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvCategories.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ItemCatCode.Contains(search) || e.ItemCatName.Contains(search) ||
                                e.ItemCatPrefix.Contains(search) || e.ItemCatDesc.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<tblInvDefCategoryDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreateCategorye : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public tblInvDefCategoryDto Input { get; set; }
    }

    public class CreateCategoryeQueryHandler : IRequestHandler<CreateCategorye, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateCategorye request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateWarehouse method start----");

                var obj = request.Input;

                if(await _context.InvCategories.AnyAsync(e => e.ItemCatPrefix == obj.ItemCatPrefix))
                    return ApiMessageInfo.Status(ApiMessageInfo.Duplicate("Category Prefix"));

                TblInvDefCategory cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.InvCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.ItemCatCode = obj.ItemCatCode;
                cObj.ItemCatName = obj.ItemCatName;
                cObj.ItemCatDesc = obj.ItemCatDesc;
                cObj.ItemCatPrefix = obj.ItemCatPrefix;
                //cObj.NextItemNumber = 0;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.InvCategories.Update(cObj);
                }
                else
                {
                    cObj.ItemCatCode = obj.ItemCatCode.ToUpper();
                    cObj.IsActive = true;
                    cObj.CreatedOn = DateTime.Now;
                    await _context.InvCategories.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateCategorye method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateCategorye Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteCategory : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCategoryQueryHandler : IRequestHandler<DeleteCategory, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Category = await _context.InvCategories.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Category);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in delete Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
    #region SingleItem

    public class GetCategory : IRequest<tblInvDefCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetcategoryHandler : IRequestHandler<GetCategory, tblInvDefCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetcategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<tblInvDefCategoryDto> Handle(GetCategory request, CancellationToken cancellationToken)
        {
            var item = await _context.InvCategories.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<tblInvDefCategoryDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.Id = (await _context.InvCategories.FirstOrDefaultAsync(e => e.Id == item.Id))?.Id ?? string.Empty;
            return item;

            //var item = await _context.CompanyBranches.AsNoTracking()
            //       .Where(e => e.Id == request.Id)
            //  .ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
            //     .FirstOrDefaultAsync(cancellationToken);
            //item.CompanyName = (await _context.Companies.FirstOrDefaultAsync(e => e.Id == item.CompanyId))?.CompanyName ?? string.Empty;
            //return item;
        }
    }

    #endregion
    #region GetCategorySelectList

    public class GetCategorySelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetSelectListHandler : IRequestHandler<GetCategorySelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCategorySelectList request, CancellationToken cancellationToken)
        {
            var list = _context.InvCategories.AsNoTracking();

            if (request.Search.HasValue())
                list = list.Where(e => e.ItemCatName.Contains(request.Search));

            var cityList = await list.OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ItemCatName, Value = e.ItemCatCode })
                  .ToListAsync(cancellationToken);

            return cityList;
        }
    }

    #endregion
    #region GetCategoryItem

    public class GetCategoryItem : IRequest<tblInvDefCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCatCode { get; set; }
    }

    public class GetCategoryHandler : IRequestHandler<GetCategoryItem, tblInvDefCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<tblInvDefCategoryDto> Handle(GetCategoryItem request, CancellationToken cancellationToken)
        {
            var item = await _context.InvCategories.AsNoTracking()
                   .Where(e => e.ItemCatCode == request.ItemCatCode)
              .ProjectTo<tblInvDefCategoryDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
    #region GetCategoryTypeList

    public class GetCategoryTypeList : IRequest<InvRootSubCategoryListDto>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCategoryTypeListHandler : IRequestHandler<GetCategoryTypeList, InvRootSubCategoryListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<InvRootSubCategoryListDto> Handle(GetCategoryTypeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCategoryTypeList method start----");
            var accTypes = await _context.InvCategories.AsNoTracking().ToListAsync();
            Log.Info("----Info GetCategoryTypeList method Ends----");
            List<InvRootSubCategoryDto> assetList = new();

            foreach (var item in accTypes)
            {
                assetList.Add(new InvRootSubCategoryDto { Name = item.ItemCatCode, List = await GetCatGListForTypeId(item.ItemCatCode) });
            }
            return new InvRootSubCategoryListDto { List = assetList };
        }

        private async Task<List<InvCategoryDto>> GetCatGListForTypeId(string catTypeId)
        {
            var categories = _context.InvCategories.AsNoTracking();
            var subCategories = _context.InvSubCategories.AsNoTracking();
            //var accCodes = _context.FinMainAccounts.AsNoTracking();
            List<InvCategoryDto> acCatGList = new();
            var catGList = await categories.Where(e => e.ItemCatCode == catTypeId).OrderBy(e => e.Id).Select(e => new { e.ItemCatCode, e.ItemCatName }).ToListAsync();
            if (catGList.Any())
            {
                InvCategoryDto accountCategoryDto = new();
                foreach (var catItem in catGList)
                {
                    accountCategoryDto = new() { ItemCatCode = catItem.ItemCatCode, ItemCatName = catItem.ItemCatName };
                    var subCatGList = await subCategories.Where(e => e.ItemCatCode == catItem.ItemCatCode).Select(e => new InvSubCategoryDto { ItemCatCode = e.ItemCatCode, ItemSubCatCode = e.ItemSubCatCode, ItemSubCatName = e.ItemSubCatName })
                        .ToListAsync();
                    if (subCatGList.Any())
                    {
                        List<InvSubCategoryDto> sCatItemList = new();
                        foreach (var subCatItem in subCatGList)
                        {
                            InvSubCategoryDto sCatItem = new();
                            //var acCodes = await accCodes.Where(e => e.FinSubCatCode == subCatItem.FinSubCatCode)
                            //    .Select(e => new AccountAccountCodeDto { FinAcName = e.FinAcName, FinAcCode = e.FinAcCode })
                            //    .ToListAsync();
                            sCatItem = new() { ItemCatCode = catItem.ItemCatCode, ItemSubCatCode = subCatItem.ItemSubCatCode, ItemSubCatName = subCatItem.ItemSubCatName };
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
}
