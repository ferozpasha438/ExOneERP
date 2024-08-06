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

    public class GetProductList : IRequest<PaginatedList<ProductDTO>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetProductListQueryHandler : IRequestHandler<GetProductList, PaginatedList<ProductDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProducts method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyProducts");
                Log.Info("----Info GetProduct method end----");
                var Products = await _context.TranProducts.AsNoTracking()
                         .Where(e =>
                         //e.CompanyId == request.User.CompanyId &&
                        (e.NameEN.Contains(request.Input.Query) || e.NameAR.Contains(request.Input.Query)
                         ))
                         .OrderBy(request.Input.OrderBy)
                        .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                        .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetProducts method Exit----");
                return Products;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProducts Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region ListOfDataForcompany

    public class GetCompanyProductList : IRequest<List<ProductDTO>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCompanyProductListQueryHandler : IRequestHandler<GetCompanyProductList, List<ProductDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetCompanyProductListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetCompanyProductList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProducts method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyProducts");
                Log.Info("----Info GetProduct method end----");
                var Products = await _context.TranProducts.AsNoTracking()
                         //  .Where(e => e.CompanyId == request.User.CompanyId)
                         .OrderByDescending(e => e.Id)
                        .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                Log.Info("----Info GetProducts method Exit----");
                return Products;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProducts Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #region ListOfDataForcompany

    public class GetProductUnitPice : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string Id { get; set; }
    }

    public class GetProductUnitPiceQueryHandler : IRequestHandler<GetProductUnitPice, ProductUnitPriceDTO>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductUnitPiceQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProductUnitPriceDTO> Handle(GetProductUnitPice request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProductUnitPice method start----");

                //var Product = await _context.TranProducts.AsNoTracking()
                //         .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

                //var Product = await _context.TranProducts.AsNoTracking()
                //         .FirstOrDefaultAsync(e => e.ProductCode.ToLower() == request.Id.Trim().ToLower()
                //|| e.NameEN.ToLower() == request.Id.Trim().ToLower()
                //|| e.NameAR.ToLower() == request.Id.Trim().ToLower());


                //var pdList = await _context.TranProducts.AsNoTracking().Where(e => e.CompanyId == request.CompanyId).ToListAsync();

                //foreach (var Product in pdList)
                //{
                //    if(Product.NameEN == request.Id.ToLower())
                //    {
                //        var unitType = (await _context.HRM_DEF_UnitTypes.FirstOrDefaultAsync(e => e.Id == Product.UnitTypeId));
                //        return new ProductUnitPriceDTO
                //        {
                //            Description = Product.Description,
                //            UnitTypeEN = unitType.NameEN,
                //            UnitTypeAR = unitType.NameAR,
                //            UnitPrice = Product.UnitPrice.ToString()
                //        };
                //    }
                //}

                //var Product = await _context.TranProducts.AsNoTracking().Where(e => e.CompanyId == request.CompanyId)
                //         .FirstOrDefaultAsync(e => request.Id.Trim().ToLower().Contains(e.ProductCode.Trim().ToLower())
                //|| request.Id.Trim().ToLower().Contains(e.NameEN.Trim().ToLower())
                //|| request.Id.Trim().ToLower().Contains(e.NameAR.Trim().ToLower()));

                var Product = await _context.TranProducts.AsNoTracking()
                    .Where(e =>
                    //e.CompanyId == request.User.CompanyId &&
                    e.Id.ToString() == request.Id)
                         .FirstOrDefaultAsync();




                Log.Info("----Info GetProductUnitPice method Exit----");
                if (Product is not null)
                {
                    var unitType = (await _context.TranUnitTypes.FirstOrDefaultAsync(e => e.Id == Product.UnitTypeId));
                    return new ProductUnitPriceDTO
                    {
                        ProductId = Product.Id,
                        Description = Product.Description,
                        UnitTypeEN = unitType.NameEN,
                        UnitTypeAR = unitType.NameAR,
                        UnitPrice = Product.UnitPrice.ToString(),
                        NameEN = Product.NameEN,
                        NameAR = Product.NameAR
                    };
                }
                return new ProductUnitPriceDTO { ProductId = 0, Description = "", UnitPrice = "0", UnitTypeEN = "", UnitTypeAR = "" };
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProductUnitPice Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region Single Item

    public class GetProduct : IRequest<ProductDTO>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetProductsQueryHandler : IRequestHandler<GetProduct, ProductDTO>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProductDTO> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetProduct method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetProduct");
                Log.Info("----Info GetProduct method end----");
                var Product = await _context.TranProducts.AsNoTracking()
                   .Where(e => e.Id == request.Id)
                   .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                Log.Info("----Info GetProduct method Exit----");
                return Product;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProduct Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateProducts : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public ProductDTO ProductDTO { get; set; }
    }
    public class CreateProductsQueryHandler : IRequestHandler<CreateProducts, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProductsQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<long> Handle(CreateProducts request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateProducts method start----");
                Log.Info("Store Proc Name : USP_Invoice_SaveUpdateProducts");
                Log.Info("----Info SaveUpdateProducts method end----");

                var obj = request.ProductDTO;
                TblTranDefProduct Product = new();
                if (obj.Id > 0)
                    Product = await _context.TranProducts.FirstOrDefaultAsync(e => e.Id == obj.Id);

                Product.CompanyId = request.User.CompanyId;
                Product.NameEN = obj.NameEN;
                Product.NameAR = obj.NameAR;
                Product.ProductCode = string.Empty;
                Product.Description = obj.Description;
                Product.ProductTypeId = obj.ProductTypeId;
                Product.UnitPrice = Convert.ToDecimal(obj.UnitPrice);
                Product.CostPrice = Convert.ToDecimal(obj.CostPrice);
                Product.UnitTypeId = obj.UnitTypeId;
                Product.Barcode = string.Empty;
                Product.IsDefaultConfig = true;

                if (obj.Id > 0)
                {
                    Product.UpdatedBy = (int)request.User.UserId;
                    Product.UpdatedOn = DateTime.Now;
                    _context.TranProducts.Update(Product);

                }
                else
                {
                    bool isExists = await _context.TranProducts
                        .Where(e => e.NameEN == obj.NameEN && e.NameAR == obj.NameAR && e.ProductCode == obj.ProductCode && e.CompanyId == request.User.CompanyId)
                        .AnyAsync();
                    if (!isExists)
                    {
                        Product.CreatedBy = (int)request.User.UserId;
                        Product.CreatedOn = DateTime.Now;
                        await _context.TranProducts.AddAsync(Product);
                    }
                }

                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateProducts method Exit----");
                return Product.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateProducts Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion


    #region GetSelectProductList

    public class GetSelectProductList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectProductListHandler : IRequestHandler<GetSelectProductList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProductListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProductList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectProductList method start----");
            var item = _context.TranProducts.AsNoTracking();

            var search = request.Input;
            if (search.HasValue())
                item = item.Where(e => e.NameEN.Contains(search) || e.ProductCode.Contains(search) || e.NameAR.Contains(search));

            var list = await item.OrderByDescending(e => e.Id)
                            .Select(e => new CustomSelectListItem { Text = e.NameEN + "" + e.NameAR, Value = e.Id.ToString() })
              .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectProductList method Ends----");
            return list;
        }
    }

    #endregion

    #region ProductUnitPriceItem

    public class ProductUnitPriceItem : IRequest<ProductUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ProductUnitPriceItemHandler : IRequestHandler<ProductUnitPriceItem, ProductUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ProductUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductUnitPriceDTO> Handle(ProductUnitPriceItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info ProductUnitPriceItem method start----");
            var item = await _context.TranProducts.AsNoTracking()
                .Where(e => e.Id == request.Id)
                 .Include(e => e.UnitType)
               .Select(Product => new ProductUnitPriceDTO
               {
                   ProductId = Product.Id,
                   Description = Product.Description,
                   UnitTypeEN = Product.UnitType.NameEN,
                   UnitTypeAR = Product.UnitType.NameAR,
                   //UnitPrice = Product.UnitPrice.ToString(),
                   UnitPrice = Product.UnitPrice.Value.ToString("0.00"),
                   NameEN = Product.NameEN,
                   NameAR = Product.NameAR
               })
                  .FirstOrDefaultAsync(cancellationToken);

            Log.Info("----Info ProductUnitPriceItem method Ends----");
            return item;
        }
    }

    #endregion
}
