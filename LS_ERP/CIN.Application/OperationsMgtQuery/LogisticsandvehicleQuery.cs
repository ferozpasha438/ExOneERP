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

    #region GetLogisticsandvehiclesPagedList

    public class GetLogisticsandvehiclesPagedList : IRequest<PaginatedList<TblOpLogisticsandvehicleDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetLogisticsandvehiclesPagedListHandler : IRequestHandler<GetLogisticsandvehiclesPagedList, PaginatedList<TblOpLogisticsandvehicleDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLogisticsandvehiclesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpLogisticsandvehicleDto>> Handle(GetLogisticsandvehiclesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpLogisticsandvehicles.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.VehicleNumber.Contains(search) ||
                            e.VehicleNameInEnglish.Contains(search) ||
                            e.VehicleNameInArabic.Contains(search)||
                            search == "" || search == null

                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblOpLogisticsandvehicleDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateLogisticsandvehicle
    public class CreateLogisticsandvehicle : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpLogisticsandvehicleDto LogisticsandvehicleDto { get; set; }
    }

    public class CreateLogisticsandvehicleHandler : IRequestHandler<CreateLogisticsandvehicle, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateLogisticsandvehicleHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateLogisticsandvehicle request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateLogisticsandvehicle method start----");



                var obj = request.LogisticsandvehicleDto;


                TblOpLogisticsandvehicle Logisticsandvehicle = new();
                if (obj.Id > 0)
                    Logisticsandvehicle = await _context.TblOpLogisticsandvehicles.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    if (_context.TblOpLogisticsandvehicles.Any(x => x.VehicleNumber == obj.VehicleNumber.ToUpper()))
                    {
                        return -1;
                    }
                    Logisticsandvehicle.VehicleNumber = obj.VehicleNumber.ToUpper();
                }
                
                Logisticsandvehicle.VehicleNameInEnglish = obj.VehicleNameInEnglish;
                Logisticsandvehicle.VehicleNameInArabic = obj.VehicleNameInArabic;
                Logisticsandvehicle.DailyFuelCost = obj.DailyFuelCost;
                Logisticsandvehicle.DailyMiscCost = obj.DailyMiscCost;
                Logisticsandvehicle.EstimatedDailyMaintenanceCost = obj.EstimatedDailyMaintenanceCost;
                Logisticsandvehicle.OtherDailyOperationCost = obj.OtherDailyOperationCost;
                Logisticsandvehicle.TotalDailyServiceCost = obj.TotalDailyServiceCost;
                Logisticsandvehicle.DailyServicePrice = obj.DailyServicePrice;
                Logisticsandvehicle.ValueofVehicle = obj.ValueofVehicle;
                Logisticsandvehicle.Vehicletype = obj.Vehicletype;
                Logisticsandvehicle.MinMargin = obj.MinMargin;
                Logisticsandvehicle.IsActive = obj.IsActive;
                Logisticsandvehicle.Remarks = obj.Remarks;




                if (obj.Id > 0)
                {
                    Logisticsandvehicle.VehicleNumber = obj.VehicleNumber;
                    Logisticsandvehicle.ModifiedOn = DateTime.Now;
                    _context.TblOpLogisticsandvehicles.Update(Logisticsandvehicle);
                }
                else
                {

                    Logisticsandvehicle.CreatedOn = DateTime.Now;
                    await _context.TblOpLogisticsandvehicles.AddAsync(Logisticsandvehicle);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateLogisticsandvehicle method Exit----");
                return Logisticsandvehicle.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateLogisticsandvehicle Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetLogisticsandvehicleByCode
    public class GetLogisticsandvehicleByCode : IRequest<TblOpLogisticsandvehicle>
    {
        public UserIdentityDto User { get; set; }
        public string LogisticsandvehicleCode { get; set; }
    }

    public class GetLogisticsandvehicleByLogisticsandvehicleCodeHandler : IRequestHandler<GetLogisticsandvehicleByCode, TblOpLogisticsandvehicle>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLogisticsandvehicleByLogisticsandvehicleCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpLogisticsandvehicle> Handle(GetLogisticsandvehicleByCode request, CancellationToken cancellationToken)
        {
            var obj = await _context.TblOpLogisticsandvehicles.AsNoTracking().FirstOrDefaultAsync(e => e.VehicleNumber == request.LogisticsandvehicleCode);

            return obj;
        }
    }

    #endregion

    #region GetLogisticsandvehicleById
    public class GetLogisticsandvehicleById : IRequest<TblOpLogisticsandvehicleDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetLogisticsandvehicleByIdHandler : IRequestHandler<GetLogisticsandvehicleById, TblOpLogisticsandvehicleDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLogisticsandvehicleByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpLogisticsandvehicleDto> Handle(GetLogisticsandvehicleById request, CancellationToken cancellationToken)
        {

            TblOpLogisticsandvehicleDto obj = await _context.TblOpLogisticsandvehicles.AsNoTracking().ProjectTo<TblOpLogisticsandvehicleDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return obj;
        }
    }

    #endregion

    #region GetSelectLogisticsandvehicleList

    public class GetSelectLogisticsandvehicleList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectLogisticsandvehicleListHandler : IRequestHandler<GetSelectLogisticsandvehicleList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectLogisticsandvehicleListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectLogisticsandvehicleList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpLogisticsandvehicles.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.VehicleNameInEnglish, Value = e.VehicleNumber, TextTwo = e.VehicleNameInArabic })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetLogisticsandvehicleCodes

    public class GetLogisticsandvehicleCodes : IRequest<List<string>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetLogisticsandvehicleCodesHandler : IRequestHandler<GetLogisticsandvehicleCodes, List<string>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLogisticsandvehicleCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(GetLogisticsandvehicleCodes request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpLogisticsandvehicles.AsNoTracking()
              .Select(e => e.VehicleNumber)
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteLogisticsandvehicle
    public class DeleteLogisticsandvehicle : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLogisticsandvehicleQueryHandler : IRequestHandler<DeleteLogisticsandvehicle, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLogisticsandvehicleQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLogisticsandvehicle request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteLogisticsandvehicle start----");

                if (request.Id > 0)
                {
                    var Logisticsandvehicle = await _context.TblOpLogisticsandvehicles.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Logisticsandvehicle);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteLogisticsandvehicle");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region GetAutoSelectLogisticsandvehicleList

    public class GetAutoSelectLogisticsandvehicleList : IRequest<List<TblOpLogisticsandvehicleDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SearchKey; 
    }

    public class GetAutoSelectLogisticsandvehicleListHandler : IRequestHandler<GetAutoSelectLogisticsandvehicleList, List<TblOpLogisticsandvehicleDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAutoSelectLogisticsandvehicleListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpLogisticsandvehicleDto>> Handle(GetAutoSelectLogisticsandvehicleList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpLogisticsandvehicles.AsNoTracking().ProjectTo<TblOpLogisticsandvehicleDto>(_mapper.ConfigurationProvider).Where(e=>e.VehicleNumber.Contains(request.SearchKey)||e.VehicleNameInEnglish.Contains(request.SearchKey)||
            e.VehicleNameInArabic.Contains(request.SearchKey)||request.SearchKey==null)             
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion
}
