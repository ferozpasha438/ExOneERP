using AutoMapper;
using CIN.Application.Common;
using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.PurchaseSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;

namespace CIN.Application.PurchaseSetupQuery
{

    #region GetPagedList

    public class GetPurchaseshipmentcodeList : IRequest<PaginatedList<TblPopDefVendorShipmentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPurchaseshipmentcodeListHandler : IRequestHandler<GetPurchaseshipmentcodeList, PaginatedList<TblPopDefVendorShipmentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseshipmentcodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblPopDefVendorShipmentDto>> Handle(GetPurchaseshipmentcodeList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.PopVendorShipments.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ShipmentCode.Contains(search) || e.ShipmentName.Contains(search)                                
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblPopDefVendorShipmentDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region SingleItem

    public class GetPurchaseshipmentcode : IRequest<TblPopDefVendorShipmentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPurchaseshipmentcodeHandler : IRequestHandler<GetPurchaseshipmentcode, TblPopDefVendorShipmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchaseshipmentcodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorShipmentDto> Handle(GetPurchaseshipmentcode request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorShipments.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblPopDefVendorShipmentDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion

    #region CreateUpdate

    public class CreatePurchaseshipmentcode : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPopDefVendorShipmentDto Input { get; set; }
    }

    public class CreatePurchaseshipmentcodeQueryHandler : IRequestHandler<CreatePurchaseshipmentcode, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePurchaseshipmentcodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreatePurchaseshipmentcode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreatePurchaseshipmentcode method start----");

                var obj = request.Input;
                TblPopDefVendorShipment cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.PopVendorShipments.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.ShipmentCode = obj.ShipmentCode;
                cObj.ShipmentName = obj.ShipmentName;
                cObj.ShipmentDesc = obj.ShipmentDesc;
                cObj.ShipmentType = obj.ShipmentType;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.PopVendorShipments.Update(cObj);
                }
                else
                {
                    cObj.ShipmentCode = obj.ShipmentCode.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    await _context.PopVendorShipments.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreatePurchaseshipmentcode method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreatePurchaseshipmentcode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


    #region Delete
    public class DeletePurchaseshipmentcode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePurchaseshipmentcodeQueryHandler : IRequestHandler<DeletePurchaseshipmentcode, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePurchaseshipmentcodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePurchaseshipmentcode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Branch = await _context.PopVendorShipments.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Branch);
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
    #region GetShipmentCode

    public class GetShipmentCode : IRequest<TblPopDefVendorCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public string ShipmentCode { get; set; }
    }

    public class GetShipmentCodeHandler : IRequestHandler<GetShipmentCode, TblPopDefVendorCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShipmentCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorCategoryDto> Handle(GetShipmentCode request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorShipments.AsNoTracking()
                   .Where(e => e.ShipmentCode == request.ShipmentCode)
              .ProjectTo<TblPopDefVendorCategoryDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
