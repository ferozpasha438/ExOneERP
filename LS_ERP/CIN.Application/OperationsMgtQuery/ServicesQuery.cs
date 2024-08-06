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

    #region GetServicesPagedList

    public class GetServicesPagedList : IRequest<PaginatedList<TblSndDefServiceMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetServicesPagedListHandler : IRequestHandler<GetServicesPagedList, PaginatedList<TblSndDefServiceMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServicesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefServiceMasterDto>> Handle(GetServicesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprServices.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ServiceCode.Contains(search) ||
                            e.ServiceNameEng.Contains(search) ||
                            e.ServiceNameArb.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefServiceMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateService
    public class CreateService : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefServiceMasterDto ServiceDto { get; set; }
    }

    public class CreateServiceHandler : IRequestHandler<CreateService, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateServiceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateService request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateService method start----");



                var obj = request.ServiceDto;


                TblSndDefServiceMaster Service = new();
                if (obj.Id > 0)
                    Service = await _context.OprServices.FirstOrDefaultAsync(e => e.Id == obj.Id);

                else {
                    if (_context.OprServices.Any(x => x.ServiceCode == obj.ServiceCode))
                    {
                        return -1;
                    }
                    Service.ServiceCode = obj.ServiceCode.ToUpper();
                }
             
                //Service.ModifiedOn = obj.ModifiedOn;
                Service.IsActive = obj.IsActive;
                Service.Id = obj.Id;
                Service.ServiceNameEng = obj.ServiceNameEng;
                Service.ServiceNameArb = obj.ServiceNameArb;
                Service.SurveyFormCode = obj.SurveyFormCode;
                //Service.DefaultBaseUnit = obj.DefaultBaseUnit;
                //Service.EstimatedServicesPricePerBaseUnit = obj.EstimatedServicesPricePerBaseUnit;

                if (obj.Id > 0)
                {
                    Service.ModifiedOn = DateTime.Now;
                    _context.OprServices.Update(Service);
                }
                else
                {


                    Service.CreatedOn = DateTime.Now;
                    await _context.OprServices.AddAsync(Service);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateService method Exit----");
                return Service.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateService Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region AttachSurveyFormToService
    public class AttachSurveyFormToService : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefServiceMasterDto Service { get; set; }
      
    }

    public class AttachSurveyFormToServiceHandler : IRequestHandler<AttachSurveyFormToService, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AttachSurveyFormToServiceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(AttachSurveyFormToService request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info AttachSurveyFormToService method start----");



              
                  var  Service = await _context.OprServices.FirstOrDefaultAsync(e => e.ServiceCode ==request.Service.ServiceCode);

                Service.SurveyFormCode = request.Service.SurveyFormCode;
               
                   
                    _context.OprServices.Update(Service);
                
             
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateService method Exit----");
                return ApiMessageInfo.Status(1, Service.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateService Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion







    #region GetServiceByServiceCode
    public class GetServiceByServiceCode : IRequest<TblSndDefServiceMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string ServiceCode { get; set; }
    }

    public class GetServiceByServiceCodeHandler : IRequestHandler<GetServiceByServiceCode, TblSndDefServiceMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceByServiceCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceMasterDto> Handle(GetServiceByServiceCode request, CancellationToken cancellationToken)
        {
            TblSndDefServiceMasterDto obj = new();
            var Service = await _context.OprServices.AsNoTracking().FirstOrDefaultAsync(e => e.ServiceCode == request.ServiceCode);
            if (Service is not null)
            {
                obj.Id = Service.Id;
                obj.ServiceCode = Service.ServiceCode;
                obj.ServiceNameEng = Service.ServiceNameEng;
                obj.ServiceNameArb = Service.ServiceNameArb;
               // obj.DefaultBaseUnit = Service.DefaultBaseUnit;
                //obj.EstimatedServicesPricePerBaseUnit = Service.EstimatedServicesPricePerBaseUnit;
                obj.ModifiedOn = Service.ModifiedOn;
                obj.CreatedOn = Service.CreatedOn;
                obj.IsActive = Service.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetServiceById
    public class GetServiceById : IRequest<TblSndDefServiceMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetServiceByIdHandler : IRequestHandler<GetServiceById, TblSndDefServiceMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceMasterDto> Handle(GetServiceById request, CancellationToken cancellationToken)
        {
            TblSndDefServiceMasterDto obj = new();
            var Service = await _context.OprServices.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (Service is not null)
            {
                obj.Id = Service.Id;
                obj.ServiceCode = Service.ServiceCode;
                
                obj.ServiceNameEng = Service.ServiceNameEng;
                obj.ServiceNameArb = Service.ServiceNameArb;
                obj.SurveyFormCode = Service.SurveyFormCode;
                obj.ModifiedOn = Service.ModifiedOn;
                obj.CreatedOn = Service.CreatedOn;
               // obj.DefaultBaseUnit = Service.DefaultBaseUnit;
                //obj.EstimatedServicesPricePerBaseUnit = Service.EstimatedServicesPricePerBaseUnit;
                obj.IsActive = Service.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetAutoFillServiceList

    public class GetAutoFillServiceList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAutoFillServiceListHandler : IRequestHandler<GetAutoFillServiceList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAutoFillServiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAutoFillServiceList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprServices.AsNoTracking()
                .Where(e => e.ServiceNameEng.Contains(search) || e.ServiceCode.Contains(search)||search==null)
              .Select(e => new CustomSelectListItem { Text = e.ServiceNameEng, Value = e.ServiceCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectServiceList

    public class GetSelectServiceList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectServiceListHandler : IRequestHandler<GetSelectServiceList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectServiceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectServiceList request, CancellationToken cancellationToken)
        {
      
            var list = await _context.OprServices.AsNoTracking()
               
              .Select(e => new CustomSelectListItem { Text = e.ServiceNameEng, Value = e.ServiceCode,TextTwo=e.ServiceNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteService
    public class DeleteService : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteServiceQueryHandler : IRequestHandler<DeleteService, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteServiceQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteService request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteService start----");

                if (request.Id > 0)
                {
                    var service = await _context.OprServices.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(service);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteService");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
