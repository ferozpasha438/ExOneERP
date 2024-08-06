using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.PurchaseSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
namespace CIN.Application.PurchaseSetupQuery
{
    #region GetPagedList

    public class GetVenCategoryList : IRequest<PaginatedList<TblPopDefVendorCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCategoryListHandler : IRequestHandler<GetVenCategoryList, PaginatedList<TblPopDefVendorCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblPopDefVendorCategoryDto>> Handle(GetVenCategoryList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.PopVendorCategories.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.VenCatCode.Contains(search) || e.VenCatDesc.Contains(search) ||
                                e.VenCatName.Contains(search) 
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblPopDefVendorCategoryDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreateVenCategorye : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPopDefVendorCategoryDto Input { get; set; }
    }

    public class CreateCategoryeQueryHandler : IRequestHandler<CreateVenCategorye, AppCtrollerDto>
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

        public async Task<AppCtrollerDto> Handle(CreateVenCategorye request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateVenCategorye method start----");

                var obj = request.Input;
                TblPopDefVendorCategory cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.PopVendorCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.VenCatCode = obj.VenCatCode;
                cObj.VenCatName = obj.VenCatName;
                cObj.VenCatDesc = obj.VenCatDesc;
                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.PopVendorCategories.Update(cObj);
                }
                else
                {
                    cObj.IsActive = true;
                    cObj.CreatedOn = DateTime.Now;
                    await _context.PopVendorCategories.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateVenCategorye method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateVenCategorye Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteVenCategory : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCategoryQueryHandler : IRequestHandler<DeleteVenCategory, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCategoryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVenCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Category = await _context.PopVendorCategories.FirstOrDefaultAsync(e => e.Id == request.Id);
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

    public class GetVenCategory : IRequest<TblPopDefVendorCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetcategoryHandler : IRequestHandler<GetVenCategory, TblPopDefVendorCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetcategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorCategoryDto> Handle(GetVenCategory request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorCategories.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblPopDefVendorCategoryDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
   
    #region GetCategoryItem

    public class GetCategoryItem : IRequest<TblPopDefVendorCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string VenCatCode { get; set; }
    }

    public class GetCategoryHandler : IRequestHandler<GetCategoryItem, TblPopDefVendorCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorCategoryDto> Handle(GetCategoryItem request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorCategories.AsNoTracking()
                   .Where(e => e.VenCatCode == request.VenCatCode)
              .ProjectTo<TblPopDefVendorCategoryDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
    
}
