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

    //#region GetCustomerVisitFormsPagedList

    //public class GetCustomerVisitFormsPagedList : IRequest<PaginatedList<TblOpCustomerVisitFormPaginatioDto>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public PaginationFilterDto Input { get; set; }

    //}

    //public class GetCustomerVisitFormsPagedListHandler : IRequestHandler<GetCustomerVisitFormsPagedList, PaginatedList<TblOpCustomerVisitFormPaginatioDto>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetCustomerVisitFormsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<PaginatedList<TblOpCustomerVisitFormPaginatioDto>> Handle(GetCustomerVisitFormsPagedList request, CancellationToken cancellationToken)
    //    {
    //        var search = request.Input.Query;
    //        var list = await _context.TblOpCustomerVisitForms.AsNoTracking()
    //          .Where(e => //e.CompanyId == request.CompanyId &&
    //                        (e.ProjectCode.Contains(search) ||
    //                        e.SiteCode.Contains(search) ||
    //                        e.ReasonCode.Contains(search) ||

    //                        e.BranchCode.Contains(search) ||
    //                        search == "" || search == null
    //                         ))
    //           .OrderBy(request.Input.OrderBy).ProjectTo<TblOpCustomerVisitFormPaginatioDto>(_mapper.ConfigurationProvider)
    //             .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

    //        return list;
    //    }
    //}
    //#endregion


    #region GetCustomerVisitFormsPagedListByProjectSite

    public class GetCustomerVisitFormsPagedListByProjectSite : IRequest<PaginatedList<TblOpCustomerVisitFormPaginatioDto>>
    {
        public UserIdentityDto User { get; set; }
        public OprPaginationFilterDto Input { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }

    }

    public class GetCustomerVisitFormsPagedListByProjectSiteHandler : IRequestHandler<GetCustomerVisitFormsPagedListByProjectSite, PaginatedList<TblOpCustomerVisitFormPaginatioDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVisitFormsPagedListByProjectSiteHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpCustomerVisitFormPaginatioDto>> Handle(GetCustomerVisitFormsPagedListByProjectSite request, CancellationToken cancellationToken)
        {
            try
            {
                var Users =  _context.SystemLogins;
                var ReasonCodes =  _context.OprReasonCodes;
                var search = request.Input.Query;
                var list = _context.TblOpCustomerVisitForms
                  .Where(e => //e.CompanyId == request.CompanyId &&
                                (e.ProjectCode.Contains(search) ||
                                e.SiteCode.Contains(search) ||
                                e.ReasonCode.Contains(search) ||

                                e.BranchCode.Contains(search) ||
                                search == "" || search == null
                                 )).Select(cvf => new TblOpCustomerVisitFormPaginatioDto
                                 {
                                     ActionTerms = cvf.ActionTerms,
                                     Id = cvf.Id,
                                     BranchCode = cvf.BranchCode,
                                     ContactNumber = cvf.ContactNumber,
                                     CreatedBy = cvf.CreatedBy,
                                     CreatedOn = cvf.CreatedOn,
                                     CustomerCode = cvf.CustomerCode,
                                     CustomerNotes = cvf.CustomerNotes,
                                     CustomerRemarks = cvf.CustomerRemarks,
                                     IsClosed = cvf.IsClosed,
                                     IsInprogress = cvf.IsInprogress,
                                     IsOpen = cvf.IsOpen,
                                     ModifiedBy = cvf.ModifiedBy,
                                     ModifiedOn = cvf.ModifiedOn,
                                     ProjectCode = cvf.ProjectCode,
                                     ReasonCode = cvf.ReasonCode,
                                     ScheduleDateTime = cvf.ScheduleDateTime,
                                     SiteCode = cvf.SiteCode,
                                     SupervisorId = cvf.SupervisorId,
                                     VisitedBy = cvf.VisitedBy,
                                     VisitedDateTime = cvf.VisitedDateTime,
                                     CanEdit = cvf.CreatedBy == request.User.UserId,
                                     IsCRM = cvf.SupervisorId == request.User.UserId,
                                     NameSupervisorId = Users.FirstOrDefault(e=>e.Id==cvf.SupervisorId).UserName??"",
                                      ReasonCodeNameAr=ReasonCodes.FirstOrDefault(e=>e.ReasonCode==cvf.ReasonCode).ReasonCodeNameArb??"",
                                      ReasonCodeNameEng=ReasonCodes.FirstOrDefault(e=>e.ReasonCode==cvf.ReasonCode).ReasonCodeNameEng??"",
                                      Status=cvf.IsOpen?"Open":cvf.IsInprogress?"Inprogress":cvf.IsClosed?"Closed":"N/A"
                                      



                                 }).Where(e2=>
                                (
                                e2.ReasonCodeNameEng.Contains(search)||
                                e2.ReasonCodeNameAr.Contains(search)||
                                e2.NameSupervisorId.Contains(search)||
                                
                                search == "" || search == null
                                 )) ;
                if(!string.IsNullOrEmpty(request.ProjectCode))
                {
                    if(request.ProjectCode!="-NA-")
                    list = list.Where(e=>e.ProjectCode==request.ProjectCode);

                }
                 if(!string.IsNullOrEmpty(request.SiteCode))
                {
                    if (request.SiteCode != "-NA-")

                        list = list.Where(e=>e.SiteCode==request.SiteCode);

                }
                
                
                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    list = aprv switch
                    {
                        "closed" => list.Where(e => e.IsClosed.Value),
                        "open" => list.Where(e => e.IsOpen.Value),
                        "inProgress" => list.Where(e => e.IsInprogress.Value),
                        _ => list
                    };
                } 
                
                if (!string.IsNullOrEmpty(request.Input.ListType))
                {
                    list = list.Where(e => e.ReasonCode == request.Input.ListType);
                }
                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetPvAllRequestsPagedListByProjectSite method Exit----");
                return nreports;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
    #endregion







    #region CreateUpdateCustomerVisitForm
    public class CreateCustomerVisitForm : IRequest<CreateUpdateResultDto>
    {
        public UserIdentityDto User { get; set; }
        public InputTblOpCustomerVisitFormDto CustomerVisitFormDto { get; set; }
    }

    public class CreateCustomerVisitFormHandler : IRequestHandler<CreateCustomerVisitForm, CreateUpdateResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerVisitFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResultDto> Handle(CreateCustomerVisitForm request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateCustomerVisitForm method start----");
                var ProjectSite = await _context.TblOpProjectSites.SingleOrDefaultAsync(e => e.ProjectCode == request.CustomerVisitFormDto.ProjectCode
                  && e.CustomerCode == request.CustomerVisitFormDto.CustomerCode
                  && e.SiteCode == request.CustomerVisitFormDto.SiteCode);
                if (ProjectSite is null)
                {
                    return new() {  ErrorId=-1,ErrorMsg="No project Site Found",IsSuccess=false};
                }

                 var CustomerData = await _context.OprCustomers.SingleOrDefaultAsync(e => e.CustCode == request.CustomerVisitFormDto.CustomerCode);
                if (CustomerData is null)
                {
                    return new() {  ErrorId=-1,ErrorMsg="No Customer Found",IsSuccess=false};
                }

                var obj = request.CustomerVisitFormDto;


                TblOpCustomerVisitForm CustomerVisitForm = new();
                if (obj.Id > 0)
                    CustomerVisitForm = await _context.TblOpCustomerVisitForms.FirstOrDefaultAsync(e => e.Id == obj.Id);
                CustomerVisitForm.IsOpen = false;
                CustomerVisitForm.IsInprogress = false;
                CustomerVisitForm.IsClosed = false;

                if (obj.Action =="new" || obj.Action == "edit")
                {
                   
                    CustomerVisitForm.CustomerCode = obj.CustomerCode;
                    CustomerVisitForm.ProjectCode = obj.ProjectCode;
                    CustomerVisitForm.SiteCode = obj.SiteCode;
                    CustomerVisitForm.ReasonCode = obj.ReasonCode;
                    CustomerVisitForm.BranchCode = ProjectSite.BranchCode;
                    CustomerVisitForm.ScheduleDateTime = obj.ScheduleDateTime.Value;
                    CustomerVisitForm.SupervisorId = obj.SupervisorId;
                    CustomerVisitForm.ContactNumber = obj.ContactNumber==string.Empty|| string.IsNullOrEmpty(obj.ContactNumber) ? CustomerData.CustContact1: obj.ContactNumber;

                    CustomerVisitForm.IsOpen = true;
                  
                }
                else if (obj.Action == "visit")
                {
                    CustomerVisitForm.SupervisorRemarks = obj.SupervisorRemarks;
                    CustomerVisitForm.VisitedDateTime = obj.VisitedDateTime;
                    CustomerVisitForm.VisitedBy = obj.VisitedBy;
                    CustomerVisitForm.CustomerNotes = obj.CustomerNotes;
                    CustomerVisitForm.CustomerRemarks = obj.CustomerRemarks;
                    CustomerVisitForm.ActionTerms = obj.ActionTerms;
                   
                    
                    CustomerVisitForm.IsClosed = true;
                }
                else if (obj.Action == "confirm")
                {
                    CustomerVisitForm.IsInprogress = true;

                }


                if (obj.Id > 0)
                {
                    CustomerVisitForm.ModifiedBy = request.User.UserId;
                    CustomerVisitForm.ModifiedOn = DateTime.Now;
                    _context.TblOpCustomerVisitForms.Update(CustomerVisitForm);
                }
                else
                {
                    CustomerVisitForm.CreatedBy = request.User.UserId;
                    CustomerVisitForm.CreatedOn = DateTime.Now;
                    await _context.TblOpCustomerVisitForms.AddAsync(CustomerVisitForm);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateCustomerVisitForm method Exit----");
                return new() { IsSuccess=true};
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateCustomerVisitForm Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { IsSuccess=false,ErrorId=0,ErrorMsg="Something went wrong"};
            }
        }
    }

    #endregion


    #region GetCustomerVisitFormById
    public class GetCustomerVisitFormById : IRequest<GetCustomerVisitFormDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetCustomerVisitFormByIdHandler : IRequestHandler<GetCustomerVisitFormById, GetCustomerVisitFormDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVisitFormByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetCustomerVisitFormDto> Handle(GetCustomerVisitFormById request, CancellationToken cancellationToken)
        {




            var CustomerVisitForm = await _context.TblOpCustomerVisitForms.AsNoTracking().ProjectTo<GetCustomerVisitFormDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            
            
            
            if(CustomerVisitForm is not null)
            {
                var CustomerData = await _context.OprCustomers.FirstOrDefaultAsync(e=>e.CustCode==CustomerVisitForm.CustomerCode);
                var ProjectData = await _context.OP_HRM_TEMP_Projects.FirstOrDefaultAsync(e=>e.ProjectCode==CustomerVisitForm.ProjectCode);
                var SiteData = await _context.OprSites.FirstOrDefaultAsync(e=>e.CustomerCode==CustomerVisitForm.CustomerCode && e.SiteCode==CustomerVisitForm.SiteCode);
                var SupervisorData = await _context.SystemLogins.FirstOrDefaultAsync(e=>e.Id==CustomerVisitForm.SupervisorId);
                var CreatedByData = await _context.SystemLogins.FirstOrDefaultAsync(e=>e.Id==CustomerVisitForm.CreatedBy);
                var ReasonCodeData = await _context.OprReasonCodes.FirstOrDefaultAsync(e=>e.ReasonCode==CustomerVisitForm.ReasonCode);
                CustomerVisitForm.CustomerNameEng = CustomerData.CustName;
                CustomerVisitForm.CustomerNameArb = CustomerData.CustArbName;
                CustomerVisitForm.CustomerAddress = CustomerData.CustAddress1;
                
                CustomerVisitForm.ProjectNameAr = ProjectData.ProjectNameArb;
                CustomerVisitForm.ProjectNameEng = ProjectData.ProjectNameEng;
                
                CustomerVisitForm.SiteNameEng = SiteData.SiteName;
                CustomerVisitForm.SiteNameAr = SiteData.SiteArbName;
                CustomerVisitForm.SiteAddress = SiteData.SiteAddress;

                CustomerVisitForm.NameSupervisorId = SupervisorData.UserName;
                CustomerVisitForm.NameCreatedBy = CreatedByData.UserName;
                CustomerVisitForm.ReasonCodeNameAr = ReasonCodeData.ReasonCodeNameArb;
                CustomerVisitForm.ReasonCodeNameEng = ReasonCodeData.ReasonCodeNameEng;

                if (CustomerVisitForm.IsClosed.Value)
                {
                    CustomerVisitForm.NameVisitedBy =_context.SystemLogins.FirstOrDefault(e=>e.Id==CustomerVisitForm.VisitedBy).UserName??"";
                    CustomerVisitForm.SupervisorRemarks = CustomerVisitForm.SupervisorRemarks;
                    CustomerVisitForm.CustomerRemarks = CustomerVisitForm.CustomerRemarks;
                    CustomerVisitForm.CustomerNotes = CustomerVisitForm.CustomerNotes;
                    CustomerVisitForm.ActionTerms = CustomerVisitForm.ActionTerms;
                }

                else { 
                    CustomerVisitForm.NameVisitedBy = "";
                    CustomerVisitForm.SupervisorRemarks = "";
                    CustomerVisitForm.CustomerRemarks = "";
                    CustomerVisitForm.CustomerNotes = "";
                    CustomerVisitForm.ActionTerms = "";
                }


            }
            return CustomerVisitForm;
        }
    }

    #endregion




    #region DeleteCustomerVisitForm
    public class DeleteCustomerVisitForm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCustomerVisitFormQueryHandler : IRequestHandler<DeleteCustomerVisitForm, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerVisitFormQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCustomerVisitForm request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteCustomerVisitForm start----");

                if (request.Id > 0)
                {
                    var CustomerVisitForm = await _context.TblOpCustomerVisitForms.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(CustomerVisitForm);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteCustomerVisitForm");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
