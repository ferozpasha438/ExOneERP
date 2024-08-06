using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FleetMgtDtos;
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
using CIN.Domain.FleetMgt;

namespace CIN.Application.FleetMgtQuery
{
    #region GetAll

    public class GetRouteMasterList : IRequest<PaginatedList<RouteMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetRouteMasterListHandler : IRequestHandler<GetRouteMasterList, PaginatedList<RouteMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRouteMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<RouteMasterDto>> Handle(GetRouteMasterList request, CancellationToken cancellationToken)
        {


            var list = await _context.RouteMaster.AsNoTracking().ProjectTo<RouteMasterDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }
    #endregion

    #region Update_Create
    public class CreateUpdateRouteMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public RouteMasterDto Input { get; set; }
    }

    public class CreateUpdateRouteMasterHandler : IRequestHandler<CreateUpdateRouteMaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateRouteMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateRouteMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Brand Master method start----");

                var obj = request.Input;


                TblRouteMaster RouteMaster = new();
                if (obj.Id > 0)
                    RouteMaster = await _context.RouteMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                RouteMaster.Id = obj.Id;
                RouteMaster.RouteCode = obj.RouteCode;
                RouteMaster.RouteName = obj.RouteName;
                RouteMaster.RouteNameAr = obj.RouteNameAr;
                RouteMaster.City = obj.City;
                RouteMaster.RouteLongitude = obj.RouteLongitude;
                RouteMaster.RouteLatitude = obj.RouteLatitude;
                RouteMaster.IsActive = obj.IsActive;
                RouteMaster.CreatedOn = DateTime.Now;
                RouteMaster.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.RouteMaster.Update(RouteMaster);
                }
                else
                {

                    await _context.RouteMaster.AddAsync(RouteMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateRouteMaster method Exit----");
                return RouteMaster.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateRouteMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }
    #endregion

    #region GetById
    public class GetRouteMasterById : IRequest<RouteMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetRouteMasterByIdHandler : IRequestHandler<GetRouteMasterById, RouteMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRouteMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RouteMasterDto> Handle(GetRouteMasterById request, CancellationToken cancellationToken)
        {

            VehicleCompanyMasterDto obj = new();
            var routeMaster = await _context.RouteMaster.AsNoTracking().ProjectTo<RouteMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return routeMaster;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Delete

    public class DeleteRouteMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteRouteMasterHandler : IRequestHandler<DeleteRouteMaster, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteRouteMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteRouteMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Brand Master start----");

                if (request.Id > 0)
                {
                    var brand = await _context.RouteMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(brand);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteBrandMaster");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region Get City List

    public class GetSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectListHandler : IRequestHandler<GetSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectList request, CancellationToken cancellationToken)
        {
            var list = await _context.CityCodes.AsNoTracking()
                    //.Where(e => e.CompanyID == request.CompanyId || e.AccountId == request.AccountId)
                    .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.CityName, Value = e.CityName })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }


    #endregion


    #region Update_Create
    public class CreateUpdateRoutePlanDetails : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public RoutePlanDetailsDto Input { get; set; }
    }

    public class CreateUpdateRoutePlanDetailsHandler : IRequestHandler<CreateUpdateRoutePlanDetails, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateRoutePlanDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateRoutePlanDetails request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Route Plan Details method start----");

                var obj = request.Input;


                TblRoutePlanDetails RoutePlan = new();
                if (obj.Id > 0)
                    RoutePlan = await _context.RoutePlanDetails.FirstOrDefaultAsync(e => e.Id == obj.Id);


                RoutePlan.Id = obj.Id;
                RoutePlan.RoutePlanId = obj.RouteId;
                RoutePlan.RouteCode = obj.RouteCode;
                RoutePlan.Flag = obj.Flag;
                RoutePlan.IsActive = true;
                RoutePlan.CreatedOn = DateTime.Now;



                if (obj.Id > 0)
                {

                    _context.RoutePlanDetails.Update(RoutePlan);
                }
                else
                {

                    await _context.RoutePlanDetails.AddAsync(RoutePlan);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateRoutePlanDetails method Exit----");
                return RoutePlan.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateRoutePlanDetails Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }
    #endregion

    #region Get RouteListByCity

    
    public class GetRouteListByCity : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string City { get; set; }

    }

    public class GetRouteListByCityHandler : IRequestHandler<GetRouteListByCity, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRouteListByCityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetRouteListByCity request, CancellationToken cancellationToken)
        {

            //var list = await _context.RouteMaster.AsNoTracking().ProjectTo<RouteMasterDto>(_mapper.ConfigurationProvider)
            //                        .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            //return list;

            var list = await _context.RouteMaster.AsNoTracking()
                      .Where(e => e.City == request.City)
                    .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.RouteName, Value = e.RouteName })
                  .ToListAsync(cancellationToken);

            return list;

        }


    }

 

    #endregion

}
