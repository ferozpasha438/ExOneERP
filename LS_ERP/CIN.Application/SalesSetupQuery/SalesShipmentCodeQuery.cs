using AutoMapper;
using CIN.Application.Common;
using CIN.Application.SalesSetupDtos;
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
using CIN.Domain.SalesSetup;

namespace CIN.Application.SalesSetupQuery
{
    #region GetPagedList

    public class GetSalesShipmentCodeList : IRequest<PaginatedList<TblSndDefSalesShipmentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSalesShipmentCodeListHandler : IRequestHandler<GetSalesShipmentCodeList, PaginatedList<TblSndDefSalesShipmentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesShipmentCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblSndDefSalesShipmentDto>> Handle(GetSalesShipmentCodeList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.SndSalesShipments.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ShipmentCode.Contains(search) || e.ShipmentName.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefSalesShipmentDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region SingleItem

    public class GetSalesShipmentCode : IRequest<TblSndDefSalesShipmentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSalesShipmentCodeHandler : IRequestHandler<GetSalesShipmentCode, TblSndDefSalesShipmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesShipmentCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSalesShipmentDto> Handle(GetSalesShipmentCode request, CancellationToken cancellationToken)
        {
            var item = await _context.SndSalesShipments.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblSndDefSalesShipmentDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateSalesShipmentCode : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSalesShipmentDto Input { get; set; }
    }

    public class CreateSalesShipmentCodeQueryHandler : IRequestHandler<CreateSalesShipmentCode, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSalesShipmentCodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateSalesShipmentCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateSalesShipmentCode method start----");

                var obj = request.Input;
                TblSndDefSalesShipment cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.SndSalesShipments.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.ShipmentCode = obj.ShipmentCode;
                cObj.ShipmentName = obj.ShipmentName;
                cObj.ShipmentDesc = obj.ShipmentDesc;
                cObj.ShipmentType = obj.ShipmentType;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.SndSalesShipments.Update(cObj);
                }
                else
                {
                    cObj.ShipmentCode = obj.ShipmentCode.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    await _context.SndSalesShipments.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateSalesShipmentCode method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateSalesShipmentCode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


    #region Delete
    public class DeleteSalesShipmentCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSalesShipmentCodeQueryHandler : IRequestHandler<DeleteSalesShipmentCode, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSalesShipmentCodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSalesShipmentCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Branch = await _context.SndSalesShipments.FirstOrDefaultAsync(e => e.Id == request.Id);
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

    public class GetShipmentCode : IRequest<TblSndDefSalesShipmentDto>
    {
        public UserIdentityDto User { get; set; }
        public string ShipmentCode { get; set; }
    }

    public class GetShipmentCodeHandler : IRequestHandler<GetShipmentCode, TblSndDefSalesShipmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShipmentCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSalesShipmentDto> Handle(GetShipmentCode request, CancellationToken cancellationToken)
        {
            var item = await _context.SndSalesShipments.AsNoTracking()
                   .Where(e => e.ShipmentCode == request.ShipmentCode)
              .ProjectTo<TblSndDefSalesShipmentDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
