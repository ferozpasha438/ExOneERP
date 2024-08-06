using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
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

    public class GetSubCategoryList : IRequest<PaginatedList<TblInvDefSubCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSubCategoryListHandler : IRequestHandler<GetSubCategoryList, PaginatedList<TblInvDefSubCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefSubCategoryDto>> Handle(GetSubCategoryList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvSubCategories.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ItemCatCode.Contains(search) || e.ItemSubCatCode.Contains(search) ||
                                e.ItemSubCatName.Contains(search) || e.ItemSubCatDesc.Contains(search) || e.SubCatKey.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefSubCategoryDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetSubCategory : IRequest<TblInvDefSubCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSubCategoryHandler : IRequestHandler<GetSubCategory, TblInvDefSubCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefSubCategoryDto> Handle(GetSubCategory request, CancellationToken cancellationToken)
        {
            var item = await _context.InvSubCategories.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblInvDefSubCategoryDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class CreateSubCategory : IRequest<(string Message, int SubCategoryId)>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefSubCategoryDto Input { get; set; }
    }

    public class CreateSubCategoryQueryHandler : IRequestHandler<CreateSubCategory, (string msg, int subCategoryId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSubCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int subCategoryId)> Handle(CreateSubCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateSubCategory method start----");

                var obj = request.Input;
                TblInvDefSubCategory cobj = new();
                if (obj.Id > 0)
                    cobj = await _context.InvSubCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);
                //if (string.IsNullOrWhiteSpace(obj.ItemCatCode))
                //    return (ApiMessageInfo.Failed, 0);

                //var Itemcode = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == obj.ItemCatCode);

                cobj.SubCatKey = obj.SubCatKey;
                cobj.ItemCatCode = obj.ItemCatCode;
                cobj.ItemSubCatCode = obj.ItemSubCatCode;
                cobj.ItemSubCatName = obj.ItemSubCatName;
                cobj.ItemSubCatDesc = obj.ItemSubCatDesc;
                cobj.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.InvSubCategories.Update(cobj);
                }
                else
                {
                    cobj.SubCatKey = obj.SubCatKey.ToUpper();
                    if (await _context.InvSubCategories.AnyAsync(e => e.SubCatKey == obj.SubCatKey))
                        return (ApiMessageInfo.Duplicate(nameof(obj.SubCatKey)), 0);

                    await _context.InvSubCategories.AddAsync(cobj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateSubCategory method Exit----");
                return (string.Empty, cobj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateSubCategory Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return (ApiMessageInfo.Failed, 0);
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteSubCategory : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSubCategoryQueryHandler : IRequestHandler<DeleteSubCategory, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSubCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSubCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Subcategory = await _context.InvSubCategories.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Subcategory);
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
    #region GetSubCatCode

    public class GetSubCatCode : IRequest<TblInvDefSubCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string SubCatCode { get; set; }
    }

    public class GetSubcatHandler : IRequestHandler<GetSubCatCode, TblInvDefSubCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubcatHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefSubCategoryDto> Handle(GetSubCatCode request, CancellationToken cancellationToken)
        {
            var item = await _context.InvSubCategories.AsNoTracking()
                   .Where(e => e.SubCatKey == request.SubCatCode)
              .ProjectTo<TblInvDefSubCategoryDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
