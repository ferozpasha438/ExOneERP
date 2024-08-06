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
using CIN.Application.OperationsMgtDtos.DMC;
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery
{

    #region GetEmployeesPagedList

    public class GetEmployeesPagedList : IRequest<PaginatedList<HRM_TRAN_EmployeeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeesPagedListHandler : IRequestHandler<GetEmployeesPagedList, PaginatedList<HRM_TRAN_EmployeeDto>>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetEmployeesPagedListHandler(DMCContext contextDMC, AutoMapper.IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<PaginatedList<HRM_TRAN_EmployeeDto>> Handle(GetEmployeesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _contextDMC.HRM_TRAN_Employees.AsNoTracking()
              .Where(e =>   (e.EmployeeID.ToString().Contains(search) ||
                            e.EmployeeNumber.Contains(search)||
                            e.EmployeeName_AR.Contains(search)||
                            e.EmployeeName.Contains(search)||
                            search == "" || search == null

                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    


    //#region CreateUpdateEmployee   update 01052022
    //public class CreateEmployee : IRequest<long>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public HRM_TRAN_EmployeeDto EmployeeDto { get; set; }
    //}

    //public class CreateEmployeeHandler : IRequestHandler<CreateEmployee, long>
    //{
    //    private readonly DMCContext _contextDMC;
    //    private readonly IMapper _mapper;

    //    public CreateEmployeeHandler(DMCContext contextDMC, IMapper mapper)
    //    {
    //        _contextDMC = contextDMC;
    //        _mapper = mapper;
    //    }

    //    public async Task<long> Handle(CreateEmployee request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info CreateUpdateEmployee method start----");



    //            var obj = request.EmployeeDto;


    //            HRM_TRAN_Employee Employee = new();
    //            if (obj.ID > 0)
    //                Employee = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeID == obj.ID);
    //            else
    //            {
    //                if (_contextDMC.HRM_TRAN_Employees.Any(x => x.EmployeeNumber == obj.EmployeeNumber  || x.EmployeeID ==obj.EmployeeID))
    //                {
    //                    return -1;
    //                }
    //                Employee.EmployeeNumber = obj.EmployeeNumber.ToUpper();
    //            }
    //            Employee.EmployeeID = obj.EmployeeID;
    //            Employee.EmployeeName = obj.EmployeeName.ToUpper();

    //            if (obj.ID > 0)
    //            {
    //                Employee.EmployeeName = obj.EmployeeName;
    //                Employee.ModifiedDate = DateTime.Now;
    //                _contextDMC.HRM_TRAN_Employees.Update(Employee);
    //            }
    //            else
    //            {

    //                Employee.CreatedDate = DateTime.Now;
    //                Employee.CreatedBy = request.User.UserId;
    //                await _contextDMC.HRM_TRAN_Employees.AddAsync(Employee);
    //            }
    //            await _contextDMC.SaveChangesAsync();
    //            Log.Info("----Info CreateUpdateEmployee method Exit----");
    //            return Employee.EmployeeID;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in CreateUpdateEmployee Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }
    //    }
    //}

    //#endregion









    #region GetEmployeeByEmployeeNumber
    public class GetEmployeeByEmployeeNumber : IRequest<HRM_TRAN_EmployeeDto>
    {
        public UserIdentityDto User { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class GetEmployeeByEmployeeNumberHandler : IRequestHandler<GetEmployeeByEmployeeNumber, HRM_TRAN_EmployeeDto>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetEmployeeByEmployeeNumberHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<HRM_TRAN_EmployeeDto> Handle(GetEmployeeByEmployeeNumber request, CancellationToken cancellationToken)
        {
            var Employee = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber);
           
            return Employee;
        }
    }

    #endregion

    #region GetEmployeeById
    public class GetEmployeeById : IRequest<HRM_TRAN_EmployeeDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeById, HRM_TRAN_EmployeeDto>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetEmployeeByIdHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<HRM_TRAN_EmployeeDto> Handle(GetEmployeeById request, CancellationToken cancellationToken)
        {
            var Employee = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeeID == request.Id);
            
            return Employee;
        }
    }

    #endregion


    #region GetSelectEmployeeList

    public class GetSelectEmployeeList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectEmployeeListHandler : IRequestHandler<GetSelectEmployeeList, List<LanCustomSelectListItem>>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetSelectEmployeeListHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetSelectEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _contextDMC.HRM_TRAN_Employees
               .AsNoTracking()
              .Select(e => new LanCustomSelectListItem { Text = e.EmployeeName, Value = e.EmployeeNumber, TextTwo = e.EmployeeID.ToString(),TextAr=e.EmployeeName_AR })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetAutoFillEmployeeList

    public class GetAutoFillEmployeeList : IRequest<List<HRM_TRAN_EmployeeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetAutoFillEmployeeListHandler : IRequestHandler<GetAutoFillEmployeeList, List<HRM_TRAN_EmployeeDto>>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetAutoFillEmployeeListHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<HRM_TRAN_EmployeeDto>> Handle(GetAutoFillEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _contextDMC.HRM_TRAN_Employees
               .AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).Where(e=>e.EmployeeName.Contains(request.Search)||e.EmployeeNumber.Contains(request.Search) || e.EmployeeName_AR.Contains(request.Search) || request.Search==null).ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetAutoSelectEmployeeList

    public class GetAutoSelectEmployeeList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetAutoSelectEmployeeListHandler : IRequestHandler<GetAutoSelectEmployeeList, List<LanCustomSelectListItem>>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetAutoSelectEmployeeListHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetAutoSelectEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _contextDMC.HRM_TRAN_Employees.AsNoTracking()
                .Where(e =>(
                (e.EmployeeName.Contains(request.Search) || e.EmployeeNumber.Contains(request.Search)|| request.Search == null)))
              .Select(e => new LanCustomSelectListItem { Text = e.EmployeeNumber, Value = e.EmployeeID.ToString(), TextTwo = e.EmployeeName,TextAr=e.EmployeeName_AR })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteEmployee
    public class DeleteEmployee : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeQueryHandler : IRequestHandler<DeleteEmployee, int>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public DeleteEmployeeQueryHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployee start----");

                if (request.Id > 0)
                {
                    var Employee = await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e => e.EmployeeID == request.Id);
                    _contextDMC.Remove(Employee);

                    await _contextDMC.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteEmployee");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
   
    
    #region GetSelectEmployeeList2->Name,Ar_Name,EmpNumber

    public class GetSelectEmployeeList2 : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectEmployeeList2Handler : IRequestHandler<GetSelectEmployeeList2, List<CustomSelectListItem>>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetSelectEmployeeList2Handler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectEmployeeList2 request, CancellationToken cancellationToken)
        {

            var list = await _contextDMC.HRM_TRAN_Employees
               .AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.EmployeeName, Value = e.EmployeeNumber, TextTwo = e.EmployeeName_AR })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion






    #region GetEmployeesPrimarySiteLogsPagedList

    public class GetEmployeesPrimarySiteLogsPagedList : IRequest<PaginatedList<EmployeePrimarySiteLogsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeesPrimarySiteLogsPagedListHandler : IRequestHandler<GetEmployeesPrimarySiteLogsPagedList, PaginatedList<EmployeePrimarySiteLogsDto>>
    {
        private readonly DMCContext _contextDMC;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesPrimarySiteLogsPagedListHandler(DMCContext contextDMC,CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _contextDMC = contextDMC;
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<EmployeePrimarySiteLogsDto>> Handle(GetEmployeesPrimarySiteLogsPagedList request, CancellationToken cancellationToken)
        {
            try
            {
                string search = string.IsNullOrEmpty(request.Input.Query)?"012":request.Input.Query;
                var PrimarySiteLog = _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.OrderByDescending(e=>e.EmployeePrimarySitesLogID);//.OrderByDescending(n => n.TransferredDate);
                var Sites = _contextDMC.HRM_DEF_Sites;


                var PrimarySiteLog_Site = PrimarySiteLog.Join(Sites, p => p.SiteCode, s => s.SiteCode, (p, s) => new
                {
                    p.EmployeeNumber,
                    p.SiteCode,
                    //s.SiteArbName,
                    //s.SiteName,
                    s.SiteName_EN,
                    s.SiteName_AR,
                    p.TransferredDate,
                    p.EmployeePrimarySitesLogID
                });



                var list = await _contextDMC.HRM_TRAN_Employees.Where(e => e.IsActive.Value &&
                                 (e.EmployeeNumber.Trim().Contains(search.Trim()) ||
                                 e.EmployeeName_AR.Trim().Contains(search.Trim()) ||
                                 e.EmployeeName.Trim().Contains(search.Trim()) ||
                                 search == "" || search == null)).Select(e => new EmployeePrimarySiteLogsDto
                {
                    EmployeeName = e.EmployeeName,
                    EmployeeNameAr = e.EmployeeName_AR,
                    EmployeeNumber = e.EmployeeNumber,
                    PrimarySiteCode = PrimarySiteLog_Site.Any(p => p.EmployeeNumber == e.EmployeeNumber) ? PrimarySiteLog_Site.FirstOrDefault(s => s.EmployeeNumber == e.EmployeeNumber).SiteCode : "NA",
                    PrimarySiteName = PrimarySiteLog_Site.Any(p => p.EmployeeNumber == e.EmployeeNumber) ? PrimarySiteLog_Site.FirstOrDefault(s => s.EmployeeNumber == e.EmployeeNumber).SiteName_AR : "NA",
                    PrimarySiteNameAr = PrimarySiteLog_Site.Any(p => p.EmployeeNumber == e.EmployeeNumber) ? PrimarySiteLog_Site.FirstOrDefault(s => s.EmployeeNumber == e.EmployeeNumber).SiteName_EN : "NA",
                    LastTransferDate =  PrimarySiteLog_Site.Any(p => p.EmployeeNumber == e.EmployeeNumber) ? PrimarySiteLog_Site.FirstOrDefault(s => s.EmployeeNumber == e.EmployeeNumber).TransferredDate :Convert.ToDateTime( DateTime.UtcNow,CultureInfo.InvariantCulture),
                    PrimarySiteLogId = PrimarySiteLog_Site.Any(p => p.EmployeeNumber == e.EmployeeNumber) ? PrimarySiteLog_Site.FirstOrDefault(s => s.EmployeeNumber == e.EmployeeNumber).EmployeePrimarySitesLogID : 0,

                }).OrderBy(request.Input.OrderBy)
                   
                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return list;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
    #endregion


    #region GetEmployeesPrimarySiteLogs

    public class GetEmployeesPrimarySiteLogs : IRequest<List<EmployeePrimarySiteLogsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class GetEmployeesPrimarySiteLogsHandler : IRequestHandler<GetEmployeesPrimarySiteLogs, List<EmployeePrimarySiteLogsDto>>
    {
        private readonly DMCContext _contextDMC;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesPrimarySiteLogsHandler(DMCContext contextDMC, CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _contextDMC = contextDMC;
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<EmployeePrimarySiteLogsDto>> Handle(GetEmployeesPrimarySiteLogs request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.EmployeeNumber;
                var Sites = _contextDMC.HRM_DEF_Sites;
                var employees = _contextDMC.HRM_TRAN_Employees.Where(e => e.EmployeeNumber == search);

                var ResList = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.Where(e=>e.EmployeeNumber==search).Select(e=> new EmployeePrimarySiteLogsDto (){
                PrimarySiteCode=e.SiteCode,
                PrimarySiteLogId=e.EmployeePrimarySitesLogID,
                 EmployeeNumber=e.EmployeeNumber,
                 LastTransferDate=e.TransferredDate,
                  EmployeeName=employees.FirstOrDefault(emp=>emp.EmployeeNumber==e.EmployeeNumber).EmployeeName??"-NA-",
                  EmployeeNameAr=employees.FirstOrDefault(emp=>emp.EmployeeNumber==e.EmployeeNumber).EmployeeName_AR??"-NA-",
                PrimarySiteName=Sites.FirstOrDefault(s=>s.SiteCode==e.SiteCode).SiteName_EN??"-NA-",
                PrimarySiteNameAr=Sites.FirstOrDefault(s=>s.SiteCode==e.SiteCode).SiteName_AR??"-NA-",
                }).ToListAsync();


                
                



                return ResList;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
    #endregion

    #region AddUpdatePrimarySiteLog   
    public class AddUpdatePrimarySiteLog : IRequest<CreateUpdateResultDto>
    {
        public UserIdentityDto User { get; set; }
        public HRM_TRAN_EmployeePrimarySites_LogDto LogDto { get; set; }
    }

    public class AddUpdatePrimarySiteLogHandler : IRequestHandler<AddUpdatePrimarySiteLog, CreateUpdateResultDto>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public AddUpdatePrimarySiteLogHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResultDto> Handle(AddUpdatePrimarySiteLog request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateEmployee method start----");



                var obj = request.LogDto;


                HRM_TRAN_EmployeePrimarySites_Log log = new();
                if (obj.EmployeePrimarySitesLogID > 0)
                    log = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeePrimarySitesLogID == obj.EmployeePrimarySitesLogID);

                log.EmployeeNumber = obj.EmployeeNumber;
                log.SiteCode = obj.SiteCode;
                log.TransferredDate = obj.TransferredDate;
                

                if (obj.EmployeePrimarySitesLogID > 0)
                {
                    log.EmployeePrimarySitesLogID = obj.EmployeePrimarySitesLogID;
                    log.CreatedDate = obj.CreatedDate;
                    log.CreatedBy = obj.CreatedBy;
                    log.ModifiedDate = DateTime.Now;
                    log.ModifiedBy =request.User.UserId;
                    _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.Update(log);
                    await _contextDMC.SaveChangesAsync();
                }
                else
                {

                    log.CreatedDate = DateTime.Now;
                    log.CreatedBy = request.User.UserId;
                    await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AddAsync(log);
                    await _contextDMC.SaveChangesAsync();
                }
              
                Log.Info("----Info CreateUpdateEmployee method Exit----");
                return new(){IsSuccess=true,ErrorId=(int)log.EmployeePrimarySitesLogID};
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateEmployee Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { IsSuccess=false,ErrorId=0,ErrorMsg="Something went wrong"};
            }
        }
    }

    #endregion

    #region GetPrimaryLogById
    public class GetPrimaryLogById : IRequest<HRM_TRAN_EmployeePrimarySites_LogDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPrimaryLogByIdHandler : IRequestHandler<GetPrimaryLogById, HRM_TRAN_EmployeePrimarySites_LogDto>
    {
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetPrimaryLogByIdHandler(DMCContext contextDMC, IMapper mapper)
        {
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<HRM_TRAN_EmployeePrimarySites_LogDto> Handle(GetPrimaryLogById request, CancellationToken cancellationToken)
        {
            var log = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().ProjectTo<HRM_TRAN_EmployeePrimarySites_LogDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeePrimarySitesLogID == request.Id);
          
            return log;
        }
    }

    #endregion
}
