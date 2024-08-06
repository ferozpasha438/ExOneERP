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
using System.IO;

namespace CIN.Application.OperationsMgtQuery
{


    #region GetCustomerComplaintsPagedListByProjectSite

    public class GetCustomerComplaintsPagedListByProjectSite : IRequest<PaginatedList<TblOpCustomerComplaintsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public OprPaginationFilterDto Input { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }

    }

    public class GetCustomerComplaintsPagedListByProjectSiteHandler : IRequestHandler<GetCustomerComplaintsPagedListByProjectSite, PaginatedList<TblOpCustomerComplaintsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerComplaintsPagedListByProjectSiteHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpCustomerComplaintsPaginationDto>> Handle(GetCustomerComplaintsPagedListByProjectSite request, CancellationToken cancellationToken)
        {
            try
            {
                var Users =  _context.SystemLogins;
                var ReasonCodes =  _context.OprReasonCodes;
                var search = request.Input.Query;
                var list = _context.TblOpCustomerComplaints
                  .Where(e => //e.CompanyId == request.CompanyId &&
                                (e.ProjectCode.Contains(search) ||
                                e.SiteCode.Contains(search) ||
                                e.ReasonCode.Contains(search) ||

                                e.BranchCode.Contains(search) ||
                                search == "" || search == null
                                 )).Select(cvf => new TblOpCustomerComplaintsPaginationDto
                                 {
                                     Id = cvf.Id,
                                     BranchCode = cvf.BranchCode,
                                     CreatedBy = cvf.CreatedBy,
                                     CreatedOn = cvf.CreatedOn,
                                     CustomerCode = cvf.CustomerCode,
                                     IsClosed = cvf.IsClosed,
                                     IsInprogress = cvf.IsInprogress,
                                     IsOpen = cvf.IsOpen,
                                     ModifiedBy = cvf.ModifiedBy,
                                     ModifiedOn = cvf.ModifiedOn,
                                     ProjectCode = cvf.ProjectCode,
                                     ReasonCode = cvf.ReasonCode,
                                     SiteCode = cvf.SiteCode,
                                     CanEdit = cvf.CreatedBy == request.User.UserId,
                                        ReasonCodeNameAr=ReasonCodes.FirstOrDefault(e=>e.ReasonCode==cvf.ReasonCode).ReasonCodeNameArb??"",
                                      ReasonCodeNameEng=ReasonCodes.FirstOrDefault(e=>e.ReasonCode==cvf.ReasonCode).ReasonCodeNameEng??"",
                                      Status=cvf.IsOpen?"Open":cvf.IsInprogress?"Inprogress":cvf.IsClosed?"Closed":"N/A"
                                      ,ProofForComplaint=cvf.ProofForComplaint,
                                      ComplaintDate=cvf.ComplaintDate,
                                      ProofForAction=cvf.ProofForAction,
                                      ClosedBy=cvf.ClosedBy,
                                      ClosingDate=cvf.ClosingDate,



                                 }).Where(e2=>
                                (
                                e2.ReasonCodeNameEng.Contains(search)||
                                e2.ReasonCodeNameAr.Contains(search)||
                                
                                search == "" || search == null
                                 )) ;
                if(!string.IsNullOrEmpty(request.ProjectCode))
                {
                    if(request.ProjectCode!="-NA-" && request.ProjectCode!=null)
                    list = list.Where(e=>e.ProjectCode==request.ProjectCode);

                }
                 if(!string.IsNullOrEmpty(request.SiteCode))
                {
                    if (request.SiteCode != "-NA-"&& request.SiteCode !=null)

                        list = list.Where(e=>e.SiteCode==request.SiteCode);

                }
                
                
                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    list = aprv switch
                    {
                        "closed" => list.Where(e => e.IsClosed),
                        "open" => list.Where(e => e.IsOpen),
                        "inProgress" => list.Where(e => e.IsInprogress),
                        _ => list
                    };
                } 
                
                if (!string.IsNullOrEmpty(request.Input.ListType))
                {
                    list = list.Where(e => e.ReasonCode == request.Input.ListType);
                }
                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetCustomerComplaintsPagedListByProjectSite method Exit----");
                return nreports;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
    #endregion



    #region CreateUpdateCustomerComplaint
    public class CreateCustomerComplaint : IRequest<CreateUpdateResultDto>
    {
        public UserIdentityDto User { get; set; }
        public InputTblOpCustomerComplaintDto CustomerComplaintDto { get; set; }
    }

    public class CreateCustomerComplaintHandler : IRequestHandler<CreateCustomerComplaint, CreateUpdateResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerComplaintHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResultDto> Handle(CreateCustomerComplaint request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    Log.Info("----Info CreateUpdateCustomerComplaint method start----");
                    var ProjectSite = await _context.TblOpProjectSites.SingleOrDefaultAsync(e => e.ProjectCode == request.CustomerComplaintDto.ProjectCode
                      && e.CustomerCode == request.CustomerComplaintDto.CustomerCode
                      && e.SiteCode == request.CustomerComplaintDto.SiteCode);
                    if (ProjectSite is null)
                    {
                        return new() { ErrorId = -1, ErrorMsg = "No project Site Found", IsSuccess = false };
                    }

                    var CustomerData = await _context.OprCustomers.SingleOrDefaultAsync(e => e.CustCode == request.CustomerComplaintDto.CustomerCode);
                    if (CustomerData is null)
                    {
                        return new() { ErrorId = -1, ErrorMsg = "No Customer Found", IsSuccess = false };
                    }

                    var obj = request.CustomerComplaintDto;


               
                    
                        var CustomerComplaint = await _context.TblOpCustomerComplaints.FirstOrDefaultAsync(e => e.Id == obj.Id)??new();
                    CustomerComplaint.IsOpen = false;
                    CustomerComplaint.IsInprogress = false;
                    CustomerComplaint.IsClosed = false;

                    if (obj.Action == "new" || obj.Action == "edit")
                    {

                        CustomerComplaint.CustomerCode = obj.CustomerCode;
                        CustomerComplaint.ProjectCode = obj.ProjectCode;
                        CustomerComplaint.SiteCode = obj.SiteCode;
                        CustomerComplaint.ReasonCode = obj.ReasonCode;
                        CustomerComplaint.BranchCode = ProjectSite.BranchCode;
                        CustomerComplaint.ComplaintDate = obj.ComplaintDate;
                        CustomerComplaint.ComplaintDescription = obj.ComplaintDescription;
                        CustomerComplaint.ComplaintBy = obj.ComplaintBy;
                        CustomerComplaint.BookedBy = obj.BookedBy;
                        CustomerComplaint.IsActionRequired = obj.IsActionRequired;






                        CustomerComplaint.IsOpen = true;

                    }
                    else if (obj.Action == "closing")
                    {
                        CustomerComplaint.ActionDescription = obj.ActionDescription;

                        CustomerComplaint.IsClosed = true;
                        CustomerComplaint.ClosedBy = obj.ClosedBy ?? request.User.UserId;
                        CustomerComplaint.ClosingDate = DateTime.UtcNow;
                    }
                    else if (obj.Action == "startAction")
                    {
                        CustomerComplaint.IsInprogress = true;

                    }


                    if (obj.Id > 0)
                    {
                        CustomerComplaint.ModifiedBy = request.User.UserId;
                        CustomerComplaint.ModifiedOn = DateTime.Now;
                        _context.TblOpCustomerComplaints.Update(CustomerComplaint);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        CustomerComplaint.CreatedBy = request.User.UserId;
                        CustomerComplaint.CreatedOn = DateTime.Now;
                        await _context.TblOpCustomerComplaints.AddAsync(CustomerComplaint);
                        await _context.SaveChangesAsync();

                    }



                    string FileName = CustomerComplaint.Id.ToString();
                    if (obj.Action == "new" || obj.Action == "edit")
                    {
                        if (!string.IsNullOrEmpty(obj.ProofForComplaintFileName))
                        {
                            if (obj.ProofForComplaintIForm != null && obj.ProofForComplaintIForm.Length > 0)
                            {
                                var guid = Guid.NewGuid().ToString();
                                // string name = string.Empty;
                                //name = Path.GetFileNameWithoutExtension(obj.ProofForComplaintIForm.FileName);
                                // guid = $"{guid}_{name}_{ Path.GetExtension(obj.ProofForComplaintIForm.FileName)}";
                                guid = $"{guid}_{FileName}{ Path.GetExtension(obj.ProofForComplaintIForm.FileName)}";
                                obj.ProofForComplaintFileName += guid;
                                var filePath = Path.Combine(obj.WebRootForComplaints, guid);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    obj.ProofForComplaintIForm.CopyTo(stream);
                                }
                                CustomerComplaint.ProofForComplaint = obj.ProofForComplaintFileName;
                                _context.TblOpCustomerComplaints.Update(CustomerComplaint);
                                await _context.SaveChangesAsync();

                            }

                        }
                    }

                    else if (obj.Action == "closing")
                    {

                        if (!string.IsNullOrEmpty(obj.ProofForActionFileName))
                        {
                            if (obj.ProofForActionIForm != null && obj.ProofForActionIForm.Length > 0)
                            {
                                var guid = Guid.NewGuid().ToString();
                                //string name = string.Empty;
                                //name = Path.GetFileNameWithoutExtension(obj.ProofForActionIForm.FileName);
                                //guid = $"{guid}_{name}_{ Path.GetExtension(obj.ProofForActionIForm.FileName)}";
                                guid = $"{guid}_{FileName}{ Path.GetExtension(obj.ProofForActionIForm.FileName)}";
                                obj.ProofForActionFileName += guid;
                                var filePath = Path.Combine(obj.WebRootForActions, guid);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    obj.ProofForActionIForm.CopyTo(stream);
                                }

                                CustomerComplaint.ProofForAction = obj.ProofForActionFileName;
                                _context.TblOpCustomerComplaints.Update(CustomerComplaint);
                                await _context.SaveChangesAsync();

                            }

                        }


                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateCustomerComplaint method Exit----");
                    return new() { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateCustomerComplaint Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess = false, ErrorId = 0, ErrorMsg = "Something went wrong" };
                }
            }
        }
    }

    #endregion


    #region GetCustomerComplaintById
    public class GetCustomerComplaintById : IRequest<GetCustomerComplaintDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetCustomerComplaintByIdHandler : IRequestHandler<GetCustomerComplaintById, GetCustomerComplaintDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerComplaintByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetCustomerComplaintDto> Handle(GetCustomerComplaintById request, CancellationToken cancellationToken)
        {




            var CustomerComplaint = await _context.TblOpCustomerComplaints.AsNoTracking().ProjectTo<GetCustomerComplaintDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);



            if (CustomerComplaint is not null)
            {
                var CustomerData = await _context.OprCustomers.FirstOrDefaultAsync(e => e.CustCode == CustomerComplaint.CustomerCode);
                var ProjectData = await _context.OP_HRM_TEMP_Projects.FirstOrDefaultAsync(e => e.ProjectCode == CustomerComplaint.ProjectCode);
                var SiteData = await _context.OprSites.FirstOrDefaultAsync(e => e.CustomerCode == CustomerComplaint.CustomerCode && e.SiteCode == CustomerComplaint.SiteCode);
                var CreatedByData = await _context.SystemLogins.FirstOrDefaultAsync(e => e.Id == CustomerComplaint.CreatedBy);
                var ReasonCodeData = await _context.OprReasonCodes.FirstOrDefaultAsync(e => e.ReasonCode == CustomerComplaint.ReasonCode);
                var UsersData = await _context.SystemLogins.ToListAsync();

                CustomerComplaint.CustomerNameEng = CustomerData.CustName;
                CustomerComplaint.CustomerNameArb = CustomerData.CustArbName;
                CustomerComplaint.CustomerAddress = CustomerData.CustAddress1;

                CustomerComplaint.ProjectNameAr = ProjectData.ProjectNameArb;
                CustomerComplaint.ProjectNameEng = ProjectData.ProjectNameEng;

                CustomerComplaint.SiteNameEng = SiteData.SiteName;
                CustomerComplaint.SiteNameAr = SiteData.SiteArbName;
                CustomerComplaint.SiteAddress = SiteData.SiteAddress;

                CustomerComplaint.NameCreatedBy = CreatedByData.UserName;
                CustomerComplaint.ReasonCodeNameAr = ReasonCodeData.ReasonCodeNameArb;
                CustomerComplaint.ReasonCodeNameEng = ReasonCodeData.ReasonCodeNameEng;


                CustomerComplaint.NameBookedBy = UsersData.SingleOrDefault(e=>e.Id==CustomerComplaint.BookedBy).UserName;

                if (CustomerComplaint.IsClosed)
                {
                    CustomerComplaint.NameClosedBy = UsersData.SingleOrDefault(e => e.Id == CustomerComplaint.ClosedBy).UserName;
                }

                else
                {
                    CustomerComplaint.NameClosedBy = null ;
                }


            }
            return CustomerComplaint;
        }
    }

    #endregion



    #region StartActionForCustomerComplaint             //making InProgress
    public class StartActionForCustomerComplaint : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class StartActionForCustomerComplaintQueryHandler : IRequestHandler<StartActionForCustomerComplaint, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public StartActionForCustomerComplaintQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(StartActionForCustomerComplaint request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info StartActionForCustomerComplaintQuery start----");

               
                    var CustomerComplaint = await _context.TblOpCustomerComplaints.FirstOrDefaultAsync(e => e.Id == request.Id);
                CustomerComplaint.IsOpen = false;
                CustomerComplaint.IsInprogress = true;
                CustomerComplaint.IsClosed = false;
                _context.TblOpCustomerComplaints.Update(CustomerComplaint);

                    await _context.SaveChangesAsync();

                    return request.Id;
              
            }
            catch (Exception ex)
            {

                Log.Error("Error in StartActionForCustomerComplaintQuery");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion




    #region DeleteCustomerComplaint
    public class DeleteCustomerComplaint : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCustomerComplaintQueryHandler : IRequestHandler<DeleteCustomerComplaint, int>
    {
       
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerComplaintQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCustomerComplaint request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteCustomerComplaint start----");

                if (request.Id > 0)
                {
                    var CustomerComplaint = await _context.TblOpCustomerComplaints.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(CustomerComplaint);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteCustomerComplaint");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion






}
