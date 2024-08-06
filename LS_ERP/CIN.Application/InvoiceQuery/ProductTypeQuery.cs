using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;


namespace CIN.Application.InvoiceQuery
{
    #region ListOfData

    public class GetProductTypeList :  IRequest<PaginatedList<ProductTypeDTO>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetProductTypeListQueryHandler : IRequestHandler<GetProductTypeList, PaginatedList<ProductTypeDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductTypeListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ProductTypeDTO>> Handle(GetProductTypeList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProductTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyProductTypes");
                Log.Info("----Info GetProductType method end----");
                var ProductTypes = await _context.TranProductTypes.AsNoTracking()
                         .Where(e => 
                         //e.CompanyId == request.User.CompanyId &&
                        (e.NameEN.Contains(request.Input.Query) || e.NameAR.Contains(request.Input.Query)
                         ))
                         .OrderBy(request.Input.OrderBy)
                        .ProjectTo<ProductTypeDTO>(_mapper.ConfigurationProvider)
                        .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetProductTypes method Exit----");
                return ProductTypes;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProductTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region ListOfDataByCompnay

    public class GetProductTypeCompnayList :  IRequest<List<ProductTypeDTO>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetProductTypeCompnayListQueryHandler : IRequestHandler<GetProductTypeCompnayList, List<ProductTypeDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductTypeCompnayListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ProductTypeDTO>> Handle(GetProductTypeCompnayList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProductTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyProductTypes");
                Log.Info("----Info GetProductType method end----");
                var ProductTypes = await _context.TranProductTypes.AsNoTracking()
                       //  .Where(e => e.CompanyId == request.User.CompanyId)
                         .OrderByDescending(e => e.Id)
                        .ProjectTo<ProductTypeDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                Log.Info("----Info GetProductTypes method Exit----");
                return ProductTypes;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProductTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region Single Item

    public class GetProductType :  IRequest<ProductTypeDTO>
    {
        public UserIdentityDto User { get; set; }
        public int ProductTypeId { get; set; }
    }
    public class GetProductTypesQueryHandler : IRequestHandler<GetProductType, ProductTypeDTO>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductTypesQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProductTypeDTO> Handle(GetProductType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetProductType method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetProductType");
                Log.Info("----Info GetProductType method end----");
                var ProductType = await _context.TranProductTypes.AsNoTracking()
                   .Where(e => e.Id == request.ProductTypeId)
                   .ProjectTo<ProductTypeDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                Log.Info("----Info GetProductType method Exit----");
                return ProductType;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProductType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateProductTypes :  IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ProductTypeDTO ProductTypeDTO { get; set; }
    }
    public class CreateProductTypesQueryHandler : IRequestHandler<CreateProductTypes, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProductTypesQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateProductTypes request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateProductTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_SaveUpdateProductTypes");
                Log.Info("----Info SaveUpdateProductTypes method end----");

                var obj = request.ProductTypeDTO;
                TblTranDefProductType ProductType = new();
                if (obj.Id > 0)
                    ProductType = await _context.TranProductTypes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                ProductType.CompanyId = request.User.CompanyId;
                ProductType.NameEN = obj.NameEN;
                ProductType.NameAR = obj.NameAR;

                if (obj.Id > 0)
                {
                    ProductType.UpdatedBy = (int)request.User.UserId;
                    ProductType.UpdatedOn = DateTime.Now;
                    _context.TranProductTypes.Update(ProductType);

                }
                else
                {
                    bool isExists = await _context.TranProductTypes
                        .Where(e => e.NameEN == obj.NameEN && e.NameAR == obj.NameAR && e.CompanyId == request.User.CompanyId)
                        .AnyAsync();
                    if (!isExists)
                    {
                        ProductType.IsDefaultConfig = true;
                        ProductType.CreatedBy = (int)request.User.UserId;
                        ProductType.CreatedOn = DateTime.Now;
                        await _context.TranProductTypes.AddAsync(ProductType);
                    }
                    else
                        return -1;
                }

                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateProductTypes method Exit----");
                return ProductType.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateProductTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
