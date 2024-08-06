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
    #region GetCustomerServiceUnitMapsPagedList

    public class GetServiceUnitMapPagedList : IRequest<PaginatedList<TblSndDefServiceUnitMapDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetServiceUnitMapPagedListHandler : IRequestHandler<GetServiceUnitMapPagedList, PaginatedList<TblSndDefServiceUnitMapDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceUnitMapPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefServiceUnitMapDto>> Handle(GetServiceUnitMapPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprServiceUnitMaps.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ServiceCode.Contains(search) ||
                            e.UnitCode.Contains(search))||
                            search == "" || search == null)
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefServiceUnitMapDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateServiceUnitMap
    public class CreateServiceUnitMap : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefServiceUnitMapDto supDto { get; set; }
    }

    public class CreateServiceUnitMapHandler : IRequestHandler<CreateServiceUnitMap, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateServiceUnitMapHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateServiceUnitMap request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateServiceUnitMap method start----");
                var obj = request.supDto;
                TblSndDefServiceUnitMap sup = new();
                if (obj.Id > 0)
                    sup = await _context.OprServiceUnitMaps.FirstOrDefaultAsync(e => e.Id == request.supDto.Id);
                else if(_context.OprServiceUnitMaps.Any(e => e.ServiceCode == request.supDto.ServiceCode&& e.UnitCode==request.supDto.UnitCode))
                {
                    return -1;
                }

                    sup.ServiceCode = obj.ServiceCode;
                    sup.UnitCode = obj.UnitCode;
                
               sup.Id = obj.Id;
                //sup.ModifiedOn = obj.ModifiedOn;
                sup.IsActive = obj.IsActive;

                
                sup.PricePerUnit = obj.PricePerUnit;
                //sup.CreatedOn = obj.CreatedOn;
                
                if (obj.Id > 0)
                {
                    sup.ModifiedOn = DateTime.Now;
                    _context.OprServiceUnitMaps.Update(sup);
                }
                else
                {
                    sup.CreatedOn = DateTime.Now;
                    await _context.OprServiceUnitMaps.AddAsync(sup);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdatesup method Exit----");
                return sup.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateServiceUnitMap Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region ServiceUnitMapByServiceAndUnitCode
    public class GetServiceUnitMapByServiceAndUnitCode : IRequest<TblSndDefServiceUnitMapDto>
    {
        public UserIdentityDto User { get; set; }
        public string ServiceCode { get; set; }
        public string UnitCode { get; set; }
    }

    public class GetServiceUnitMapByServiceAndUnitCodeHandler : IRequestHandler<GetServiceUnitMapByServiceAndUnitCode, TblSndDefServiceUnitMapDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceUnitMapByServiceAndUnitCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceUnitMapDto> Handle(GetServiceUnitMapByServiceAndUnitCode request, CancellationToken cancellationToken)
        {
            TblSndDefServiceUnitMapDto obj = new();
            var ServiceUnitMap = await _context.OprServiceUnitMaps.AsNoTracking().FirstOrDefaultAsync(e => e.ServiceCode == request.ServiceCode && e.UnitCode == request.UnitCode);
            if (ServiceUnitMap is not null)
            {
                obj.Id = ServiceUnitMap.Id;
                obj.ServiceCode = ServiceUnitMap.ServiceCode;
                obj.UnitCode = ServiceUnitMap.UnitCode;
                obj.PricePerUnit = ServiceUnitMap.PricePerUnit;
                obj.ModifiedOn = ServiceUnitMap.ModifiedOn;
                obj.CreatedOn = ServiceUnitMap.CreatedOn;
                obj.IsActive = ServiceUnitMap.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetServiceUnitMapById
    public class GetServiceUnitMapById : IRequest<TblSndDefServiceUnitMapDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetServiceUnitMapByIdHandler : IRequestHandler<GetServiceUnitMapById, TblSndDefServiceUnitMapDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceUnitMapByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceUnitMapDto> Handle(GetServiceUnitMapById request, CancellationToken cancellationToken)
        {
            TblSndDefServiceUnitMapDto obj = new();
            var ServiceUnitMap = await _context.OprServiceUnitMaps.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (ServiceUnitMap is not null)
            {
                obj.Id = ServiceUnitMap.Id;
                obj.ServiceCode = ServiceUnitMap.ServiceCode;
                obj.UnitCode = ServiceUnitMap.UnitCode;
                obj.PricePerUnit = ServiceUnitMap.PricePerUnit;
                obj.ModifiedOn = ServiceUnitMap.ModifiedOn;
                obj.CreatedOn = ServiceUnitMap.CreatedOn;
                obj.IsActive = ServiceUnitMap.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSelectServiceUnitMapList

    public class GetSelectServiceUnitMapList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
            }

    public class GetSelectServiceUnitMapListHandler : IRequestHandler<GetSelectServiceUnitMapList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectServiceUnitMapListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectServiceUnitMapList request, CancellationToken cancellationToken)
        {
           
            var list = await _context.OprServiceUnitMaps.AsNoTracking()
     
              .Select(e => new CustomSelectListItem { Text = e.ServiceCode, Value = e.UnitCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteServiceUnitMap
    public class DeleteServiceUnitMap : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteServiceUnitMapQueryHandler : IRequestHandler<DeleteServiceUnitMap, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteServiceUnitMapQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteServiceUnitMap request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteServiceUnitMap start----");

                if (request.Id > 0)
                {
                    var ServiceUnitMap = await _context.OprServiceUnitMaps.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(ServiceUnitMap);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteServiceUnitMap");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion



    
}
