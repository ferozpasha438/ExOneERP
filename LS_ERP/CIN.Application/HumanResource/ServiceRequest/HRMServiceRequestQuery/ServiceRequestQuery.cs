using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.ServiceRequest;
using CIN.Domain.HumanResource.ServiceRequest.HRMServiceRequestDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestQuery
{

    #region GetWaitingApprovalServiceRequestList

    public class GetWaitingApprovalServiceRequestList : IRequest<PaginatedList<TblHRMTrnEmployeeServiceRequestDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetWaitingApprovalServiceRequestListHandler : IRequestHandler<GetWaitingApprovalServiceRequestList, PaginatedList<TblHRMTrnEmployeeServiceRequestDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWaitingApprovalServiceRequestListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeServiceRequestDto>> Handle(GetWaitingApprovalServiceRequestList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetWaitingApprovalServiceRequestList method start----");
                var input = request.Input;
                bool isArab = request.User.Culture.IsArab();
                var userId = request.User.UserId;

                var empControlUserIds = _context.EmployeeControls.Where(e => e.IsUser == true).Select(e => e.UserId);
                if (empControlUserIds.Any(e => e.Value == userId))
                {
                    List<TblHRMTrnEmployeeServiceRequestDto> serviceItemList = new();

                    var serviceQuery = _context.EmployeeServiceRequests.AsNoTracking().Where(e => e.IsApproved == false);
                    var serviceListIdQuery = input.Id == 0 ? serviceQuery : serviceQuery.Where(e => e.Id < input.Id);

                    if (input.Query.HasValue())
                    {
                        serviceListIdQuery = serviceListIdQuery.Where(e => e.ServiceRequestRefNo.Contains(input.Query) || e.ServiceRequestTypeCode.Contains(input.Query));
                    }
                    if (input.Code.HasValue())
                    {
                        int empId = int.Parse(input.Code);
                        if (empId > 0)
                            serviceListIdQuery = serviceListIdQuery.Where(e => e.EmployeeID == empId);
                    }

                    var serviceListIds = await serviceListIdQuery.OrderByDescending(e => e.Id).Select(e => e.Id).ToListAsync();

                    foreach (var sqrId in serviceListIds)
                    {
                        var sqrItem = await _context.EmployeeServiceRequests.Include(e => e.SysServiceRequestType)
                                            .Include(e => e.TrnPersonalInformation).AsNoTracking()
                                            .FirstOrDefaultAsync(e => e.Id == sqrId);

                        var auditInfo = await _context.GetAuditApprovalInfo(sqrId);
                        var approvedCount = await auditInfo.CountAsync();

                        var userlist = await _context.GetApprovalUserWithOrder(isArab, sqrItem.ServiceRequestTypeCode, sqrItem.EmployeeID);
                        int seq = 1;
                        var lastApprovedUser = await _context.EmployeeServiceRequestAudits.Include(e => e.TrnPersonalInformation).AsNoTracking()
                            .Where(e => e.EmployeeServiceRequestID == sqrId || e.ActionID == (int)ProcessStage.Save || e.ActionID == (int)ProcessStage.Submit)
                            .OrderByDescending(e => e.Id).Select(e => new
                            {
                                lastPrecessedUser = isArab ? string.Concat(sqrItem.TrnPersonalInformation.FirstNameAr + " ", sqrItem.TrnPersonalInformation.LastNameAr)
                                              : string.Concat(sqrItem.TrnPersonalInformation.FirstNameEn + " ", sqrItem.TrnPersonalInformation.LastNameEn),
                                remarks = e.Remarks
                            }).FirstOrDefaultAsync();

                        var lastPrecessedUser = $"{lastApprovedUser.lastPrecessedUser} [ {lastApprovedUser.remarks} ]";
                        foreach (var item in userlist)
                        {
                            if (item.IntValue == userId)
                            {
                                if ((seq - approvedCount) == 1)
                                {
                                    input.Id = sqrItem.Id;
                                    serviceItemList.Add(new()
                                    {
                                        Id = sqrItem.Id,
                                        ServiceRequestRefNo = sqrItem.ServiceRequestRefNo,
                                        ServiceRequestTypeCode = isArab ? sqrItem.SysServiceRequestType.ServiceRequestTypeNameAr : sqrItem.SysServiceRequestType.ServiceRequestTypeNameEn,
                                        IsApproved = sqrItem.IsApproved,
                                        EmployeeNumber = sqrItem.TrnPersonalInformation.EmployeeNumber,
                                        EmployeeName = isArab ? string.Concat(sqrItem.TrnPersonalInformation.FirstNameAr + " ", sqrItem.TrnPersonalInformation.LastNameAr)
                                              : string.Concat(sqrItem.TrnPersonalInformation.FirstNameEn + " ", sqrItem.TrnPersonalInformation.LastNameEn),
                                        LastProcessedUser = lastPrecessedUser,
                                    });
                                    break;
                                }
                            }
                            lastPrecessedUser = $"{item.Text} [ processed stage {seq} ]";
                            seq++;
                        }

                        if (input.PageCount == serviceItemList.Count)
                            break;
                    }

                    Log.Info("----Info GetWaitingApprovalServiceRequestList method end----");
                    return new PaginatedList<TblHRMTrnEmployeeServiceRequestDto>(serviceItemList, serviceQuery.Count(), input.Id, input.PageCount);

                    //var list = await _context.EmployeeServiceRequests.Include(e => e.SysServiceRequestType)
                    //.Include(e => e.TrnPersonalInformation)
                    //.AsNoTracking()
                    //.Where(e => (e.ServiceRequestTypeCode.Contains(search) || e.ServiceRequestRefNo.Contains(search)))
                    //.OrderByDescending(x => x.Id)
                    //.Select(e => new TblHRMTrnEmployeeServiceRequestDto
                    //{
                    //    Id = e.Id,
                    //    ServiceRequestRefNo = e.ServiceRequestRefNo,
                    //    ServiceRequestTypeCode = isArab ? e.SysServiceRequestType.ServiceRequestTypeNameAr : e.SysServiceRequestType.ServiceRequestTypeNameEn,
                    //    IsApproved = e.IsApproved,
                    //    EmployeeNumber = e.TrnPersonalInformation.EmployeeNumber,
                    //    EmployeeName = isArab ? string.Concat(e.TrnPersonalInformation.FirstNameAr + " ", e.TrnPersonalInformation.LastNameAr)
                    //                          : string.Concat(e.TrnPersonalInformation.FirstNameEn + " ", e.TrnPersonalInformation.LastNameEn)

                    //})
                    //.PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                    //Log.Info("----Info GetWaitingApprovalServiceRequestList method end----");
                    //return list;
                }

                return new PaginatedList<TblHRMTrnEmployeeServiceRequestDto>(new(), 0, 0, 1);
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetWaitingApprovalServiceRequestList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion


    #region GetMyServiceRequestList

    public class GetMyServiceRequestList : IRequest<PaginatedList<TblHRMTrnEmployeeServiceRequestDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetMyServiceRequestListHandler : IRequestHandler<GetMyServiceRequestList, PaginatedList<TblHRMTrnEmployeeServiceRequestDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMyServiceRequestListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeServiceRequestDto>> Handle(GetMyServiceRequestList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetMyServiceRequestList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await _context.EmployeeServiceRequests.Include(e => e.SysServiceRequestType)
                    .Include(e => e.TrnPersonalInformation)
                    .AsNoTracking()
                     .Where(e => e.CreatedBy == request.User.UserId && (e.ServiceRequestTypeCode.Contains(search) || e.ServiceRequestRefNo.Contains(search)))
                   .OrderByDescending(x => x.Id)
                    .Select(e => new TblHRMTrnEmployeeServiceRequestDto
                    {
                        Id = e.Id,
                        ServiceRequestRefNo = e.ServiceRequestRefNo,
                        ServiceRequestTypeCode = isArab ? e.SysServiceRequestType.ServiceRequestTypeNameAr : e.SysServiceRequestType.ServiceRequestTypeNameEn,
                        IsApproved = e.IsApproved,
                        EmployeeNumber = e.TrnPersonalInformation.EmployeeNumber,
                        EmployeeName = isArab ? string.Concat(e.TrnPersonalInformation.FirstNameAr + " ", e.TrnPersonalInformation.LastNameAr)
                                              : string.Concat(e.TrnPersonalInformation.FirstNameEn + " ", e.TrnPersonalInformation.LastNameEn),


                    })
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetMyServiceRequestList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMyServiceRequestList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion


    #region GetMyServiceRequestById

    public class GetMyServiceRequestById : IRequest<VacationServiceRequestDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public bool IsFromApproval { get; set; }
        public string ServiceRequestType { get; set; }
    }

    public class GetMyServiceRequestByIdHandler : IRequestHandler<GetMyServiceRequestById, VacationServiceRequestDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMyServiceRequestByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VacationServiceRequestDto> Handle(GetMyServiceRequestById request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetMyServiceRequestById method start----");
                bool isArab = request.User.Culture.IsArab();
                var userId = request.User.UserId;

                var srq = await _context.EmployeeServiceRequests.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

                var empInfo = await _context.PersonalInformation.Where(e => e.Id == srq.EmployeeID)
                    .Select(e => new CustomSelectListItem
                    {
                        Text = isArab ? string.Concat(e.FirstNameAr + " ", e.LastNameAr) : string.Concat(e.FirstNameEn + " ", e.LastNameEn),
                        IntValue = e.Id,
                        Value = e.EmployeeNumber,
                    }).FirstOrDefaultAsync();

                var reqList = await _context.EmployeeVacationServiceRequestLeaveDetails.Where(e => e.EmployeeServiceRequestID == srq.Id)
                    .Select(e => new TblHRMTrnEmployeeVacationServiceRequestLeaveDetailsDto
                    {
                        LeaveTypeCode = e.LeaveTypeCode,
                        FromDate = e.FromDate,
                        ToDate = e.ToDate,
                        NoOfDays = e.NoOfDays,
                    }).ToListAsync();

                var document = await _context.EmployeeServiceRequestDocumentDetails.Where(e => e.EmployeeServiceRequestID == srq.Id).FirstOrDefaultAsync();
                var audits = await _context.EmployeeServiceRequestAudits.Where(e => e.EmployeeServiceRequestID == srq.Id)
                    .Select(e => new TblHRMTrnEmployeeServiceRequestAuditDto
                    {
                        Remarks = e.Remarks,
                        ActionID = e.ActionID,
                        EntryBy = e.EntryBy,
                        ActionName = ((ProcessStage)e.ActionID).ToString(),
                        EntryDate = e.EntryDate,
                    }).ToListAsync();

                foreach (var item in audits)
                {
                    if (item.ActionID == (int)ProcessStage.Approved)
                    {
                        var user = await _context.SystemLogins.Select(e => new { e.Id, e.UserName }).FirstOrDefaultAsync(e => e.Id == item.EntryBy);
                        item.EntryName = user.UserName;
                    }
                    else
                    {
                        var empInfoItem = await _context.PersonalInformation.Where(e => e.Id == item.EntryBy).Select(e => new { e.FirstNameEn, e.FirstNameAr, e.LastNameEn, e.LastNameAr }).FirstOrDefaultAsync();
                        item.EntryName = isArab ? string.Concat(empInfoItem.FirstNameAr + " ", empInfoItem.LastNameAr) : string.Concat(empInfoItem.FirstNameEn + " ", empInfoItem.LastNameEn);
                    }
                    item.ActionID = 0;
                }


                VacationServiceRequestDto requestDto = new()
                {
                    ServiceRequestTypeCode = srq.ServiceRequestTypeCode,
                    ServiceRequestRefNo = srq.ServiceRequestRefNo,
                    DocumentName = document.Name,
                    DocumentType = document.DocumentTypeCode,
                    FileName = document.FileName,
                    Remarks = audits.FirstOrDefault()?.Remarks ?? string.Empty,
                    EmployeeInfo = empInfo,
                    List = reqList,
                    Audits = audits,
                    ActionType = srq.ActionID,
                    IsApproved = srq.IsApproved,
                };

                if (request.IsFromApproval && !requestDto.IsApproved)
                {
                    var auditInfo = await _context.GetAuditApprovalInfo(srq.Id);
                    var approvedCount = await auditInfo.CountAsync();

                    //var auditInfo = _context.EmployeeServiceRequestAudits.Where(e => e.EmployeeServiceRequestID == srq.Id).OrderByDescending(e => e.Id);
                    //var lastRejectedId = await auditInfo.Where(e => e.ActionID == (int)ProcessStage.Reject).Select(e => e.Id).FirstOrDefaultAsync();
                    //auditInfo = auditInfo.Where(e => e.Id > lastRejectedId && e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);

                    //check current loggedin user approved
                    var existingApprovalEntries = auditInfo.Select(e => e.EntryBy);
                    if (existingApprovalEntries.Any(ent => ent == userId))
                        requestDto.IsApproved = true;

                    //Check which level approval current loggedin user belongs to
                    if (!requestDto.IsApproved)
                    {
                        var userlist = await _context.GetApprovalUserWithOrder(isArab, srq.ServiceRequestTypeCode, srq.EmployeeID);
                        int seq = 1;
                        foreach (var item in userlist)
                        {
                            if (item.IntValue == userId)
                            {
                                if ((seq - approvedCount) == 1)
                                {
                                    requestDto.IsApproved = false;
                                    break;
                                }
                                else
                                    requestDto.IsApproved = true;
                            }
                            seq++;
                        }
                    }

                }

                Log.Info("----Info GetMyServiceRequestById method end----");
                return requestDto;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMyServiceRequestById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion


    #region GetRequestApprovalSelectListItem
    public class GetRequestApprovalSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ServiceRequestType { get; set; }
    }

    public class GetRequestApprovalSelectListItemHandler : IRequestHandler<GetRequestApprovalSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRequestApprovalSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetRequestApprovalSelectListItem request, CancellationToken cancellationToken)
        {

            bool isArab = request.User.Culture.IsArab();
            var userId = request.User.UserId;
            var list = await _context.GetApprovalUserWithOrder(isArab, request.ServiceRequestType, request.EmployeeId);

            //var contractInfo = await _context.EmployeeContracts.AsNoTracking().Select(e => new { e.EmployeeID, e.BranchCode }).FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeId);
            //var approvalMatrix = _context.ServiceRequestApprovalAuthorityMatrix.Include(e => e.SysApprovalAuthority)
            //    .Include(e => e.TrnPersonalInformation).AsNoTracking()
            //    .Where(e => e.BranchCode == contractInfo.BranchCode && e.ServiceRequestTypeCode == request.ServiceRequestType)
            //    .OrderBy(e => e.Id);

            //var empControlUserIds = _context.EmployeeControls.Where(e => e.IsUser == true).Select(e => new { e.UserId, e.EmployeeID });
            //var list = await (from matrix in approvalMatrix
            //                  join emp in empControlUserIds
            //                  on matrix.ManagerEmployeeID equals emp.EmployeeID
            //                  select new CustomSelectListItem
            //                  {
            //                      Text = isArab ? matrix.SysApprovalAuthority.ApprovalAuthorityNameAr : matrix.SysApprovalAuthority.ApprovalAuthorityNameEn,
            //                      TextTwo = isArab ? string.Concat(matrix.TrnPersonalInformation.FirstNameAr + " ", matrix.TrnPersonalInformation.LastNameAr)
            //                                 : string.Concat(matrix.TrnPersonalInformation.FirstNameEn + " ", matrix.TrnPersonalInformation.LastNameEn),
            //                      IntValue = emp.UserId.Value
            //                  }).ToListAsync();


            //// var list = await _context.ServiceRequestApprovalAuthorityMatrix.Include(e => e.SysApprovalAuthority)
            //// .Include(e => e.TrnPersonalInformation).AsNoTracking()
            //// .Where(e => e.BranchCode == contractInfo.BranchCode && e.ServiceRequestTypeCode == request.ServiceRequestType)
            //// .OrderBy(e => e.Id)
            ////.Select(e => new CustomSelectListItem
            ////{
            ////    Text = isArab ? e.SysApprovalAuthority.ApprovalAuthorityNameAr : e.SysApprovalAuthority.ApprovalAuthorityNameEn,
            ////    TextTwo = isArab ? string.Concat(e.TrnPersonalInformation.FirstNameAr + " ", e.TrnPersonalInformation.LastNameAr)
            ////    : string.Concat(e.TrnPersonalInformation.FirstNameEn + " ", e.TrnPersonalInformation.LastNameEn)
            ////})
            //// .ToListAsync(cancellationToken);

            var auditInfo = await _context.GetAuditApprovalInfo(request.Id);

            //var auditInfo = _context.EmployeeServiceRequestAudits.Where(e => e.EmployeeServiceRequestID == /*request*/.Id).OrderByDescending(e => e.Id);
            //var lastRejectedId = await auditInfo.Where(e => e.ActionID == (int)ProcessStage.Reject).Select(e => e.Id).FirstOrDefaultAsync();
            //auditInfo = auditInfo.Where(e => e.Id > lastRejectedId && e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);

            var approvedCount = await auditInfo.CountAsync();

            int seq = 1;
            bool isApproved = false;
            foreach (var item in list)
            {
                if (isApproved)
                {
                    item.ShortValue = 2;
                    isApproved = false;
                }
                //check current loggedin user approved
                var existingApprovalEntries = auditInfo.Select(e => e.EntryBy);
                if (existingApprovalEntries.Any(ent => ent == item.IntValue))
                {
                    isApproved = true;
                    item.ShortValue = 1;
                }

                item.IntValue = seq++;
            }
            return list;

        }
    }
    #endregion



    #region GetVacationPolicyForEmployee
    public class GetVacationPolicyForEmployee : IRequest<CustomSelectListItem>
    {
        public int EmployeeId { get; set; }
    }

    public class GetVacationPolicyForEmployeeHandler : IRequestHandler<GetVacationPolicyForEmployee, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVacationPolicyForEmployeeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomSelectListItem> Handle(GetVacationPolicyForEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetVacationPolicyForEmployee method start----");
                var contractInfo = await _context.EmployeeContracts.Include(e => e.SysVacationPolicy).AsNoTracking()
                    .Where(e => e.EmployeeID == request.EmployeeId)
                    .Select(e => new { e.SysVacationPolicy.VacationDurationInMonths, e.SysVacationPolicy.MaximumDaysAllowed })
                    .FirstOrDefaultAsync();

                if (contractInfo is null)
                    return new() { Text = "You do not have Vacation Policy" };

                var logInfo = await _context.EmployeeVacationDateLogs.Where(e => e.EmployeeID == request.EmployeeId)
                    .OrderByDescending(e => e.Id)
                    //.Select(e => e.ToDate)
                    .FirstOrDefaultAsync();

                Log.Info("----Info GetVacationPolicyForEmployee method end----");

                if (logInfo is not null)
                {
                    var rValue = DateTime.Now;
                    var lValue = logInfo.ToDate;

                    int totalMonths = (rValue.Month - lValue.Month) + 12 * (rValue.Year - lValue.Year);
                    int totalDays = (rValue.Day - lValue.Day) + 12 * (rValue.Month - lValue.Month);
                    if (totalMonths >= contractInfo.VacationDurationInMonths && totalDays > 0)
                    {
                        return new() { IntValue = contractInfo.MaximumDaysAllowed };
                    }
                    return new() { Text = $"You are still not completed {contractInfo.VacationDurationInMonths} months", IntValue = 0 };
                }

                return new() { IntValue = contractInfo.MaximumDaysAllowed };
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetVacationPolicyForEmployee Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateVacationRequest

    public class CreateUpdateVacationRequest : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public VacationServiceRequestDto Input { get; set; }
    }
    public class CreateUpdateVacationRequestHandler : IRequestHandler<CreateUpdateVacationRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVacationRequestHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateVacationRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateVacationRequest method start----");
                    var obj = request.Input;
                    var emp = obj.EmployeeInfo;
                    var list = obj.List;
                    var userId = request.User.UserId;
                    TblHRMTrnEmployeeServiceRequest serviceReq = await _context.EmployeeServiceRequests.FirstOrDefaultAsync(e => e.Id == obj.Id);

                    if (obj.Id > 0)
                    {

                        serviceReq.ActionID = obj.ActionType;
                        serviceReq.Modified = DateTime.Now;
                        serviceReq.ModifiedBy = userId;
                        _context.EmployeeServiceRequests.Update(serviceReq);


                        //audit
                        TblHRMTrnEmployeeServiceRequestAudit employeeServiceRequestAudit = new()
                        {
                            EmployeeServiceRequestID = serviceReq.Id,
                            ActionID = obj.ActionType,
                            EntryBy = emp.IntValue,
                            EntryDate = DateTime.Now,
                            ServiceRequestProcessStageID = 0,
                            Remarks = obj.Remarks
                        };
                        await _context.EmployeeServiceRequestAudits.AddAsync(employeeServiceRequestAudit);
                        await _context.SaveChangesAsync();

                        //document
                        if (obj.FileName.HasValue())
                        {
                            var oldDocument = await _context.EmployeeServiceRequestDocumentDetails.FirstOrDefaultAsync(e => e.EmployeeServiceRequestID == serviceReq.Id);
                            if (oldDocument is not null)
                            {
                                _context.EmployeeServiceRequestDocumentDetails.Remove(oldDocument);
                                await _context.SaveChangesAsync();
                            }

                            TblHRMTrnEmployeeServiceRequestDocumentDetails employeeServiceRequestDocumentDetails = new()
                            {
                                EmployeeServiceRequestID = serviceReq.Id,
                                Name = obj.DocumentName,
                                FileName = obj.FileName,
                                DocumentTypeCode = obj.DocumentType,
                                UploadedDate = DateTime.Now,
                                UploadedBy = userId,
                                ServiceRequestProcessStageID = 0,
                            };
                            await _context.EmployeeServiceRequestDocumentDetails.AddAsync(employeeServiceRequestDocumentDetails);
                        }


                        //LeaveInfo
                        if (list is not null && list.Count > 0)
                        {
                            var reqList = _context.EmployeeVacationServiceRequestLeaveDetails.Where(e => e.EmployeeServiceRequestID == serviceReq.Id);
                            if (await reqList.AnyAsync())
                            {
                                _context.EmployeeVacationServiceRequestLeaveDetails.RemoveRange(reqList);
                                await _context.SaveChangesAsync();
                            }

                            List<TblHRMTrnEmployeeVacationServiceRequestLeaveDetails> vacationServiceRequestLeaveDetails = new();
                            foreach (var item in list)
                            {

                                TblHRMTrnEmployeeVacationServiceRequestLeaveDetails vacationServiceRequestLeaveDetail = new()
                                {
                                    EmployeeServiceRequestID = serviceReq.Id,
                                    FromDate = item.FromDate,
                                    ToDate = item.ToDate,
                                    LeaveTypeCode = item.LeaveTypeCode,
                                    NoOfDays = item.NoOfDays,
                                };
                                vacationServiceRequestLeaveDetails.Add(vacationServiceRequestLeaveDetail);
                            }

                            if (vacationServiceRequestLeaveDetails.Count > 0)
                            {
                                await _context.EmployeeVacationServiceRequestLeaveDetails.AddRangeAsync(vacationServiceRequestLeaveDetails);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        var serviceType = await _context.ServiceRequestTypes.FirstOrDefaultAsync(e => e.ServiceRequestTypeCode == obj.ServiceRequestTypeCode);
                        //servicerequest
                        serviceReq = new()
                        {
                            ServiceRequestRefNo = "S",
                            ServiceRequestTypeCode = obj.ServiceRequestTypeCode,
                            ActionID = obj.ActionType,
                            EmployeeID = emp.IntValue,
                            ServiceRequestProcessStageID = 0,
                            IsApproved = false,
                            IsActive = true,
                            Created = DateTime.Now,
                            CreatedBy = userId,
                        };

                        await _context.EmployeeServiceRequests.AddAsync(serviceReq);
                        await _context.SaveChangesAsync();

                        string ServiceRequestRefNo = $"SRQ{serviceType.ServiceRequestTypeCode}{DateTime.Now.Year}{serviceReq.Id:d9}";
                        var servicerequest = await _context.EmployeeServiceRequests.FirstOrDefaultAsync(e => e.Id == serviceReq.Id);
                        servicerequest.ServiceRequestRefNo = ServiceRequestRefNo;

                        //audit
                        TblHRMTrnEmployeeServiceRequestAudit employeeServiceRequestAudit = new()
                        {
                            EmployeeServiceRequestID = serviceReq.Id,
                            ActionID = obj.ActionType,
                            EntryBy = emp.IntValue,
                            EntryDate = DateTime.Now,
                            ServiceRequestProcessStageID = 0,
                            Remarks = obj.Remarks
                        };
                        await _context.EmployeeServiceRequestAudits.AddAsync(employeeServiceRequestAudit);

                        //document
                        TblHRMTrnEmployeeServiceRequestDocumentDetails employeeServiceRequestDocumentDetails = new()
                        {
                            EmployeeServiceRequestID = serviceReq.Id,
                            Name = obj.DocumentName,
                            FileName = obj.FileName,
                            DocumentTypeCode = obj.DocumentType,
                            UploadedDate = DateTime.Now,
                            UploadedBy = userId,
                            ServiceRequestProcessStageID = 0,
                        };
                        await _context.EmployeeServiceRequestDocumentDetails.AddAsync(employeeServiceRequestDocumentDetails);

                        //LeaveInfo
                        List<TblHRMTrnEmployeeVacationServiceRequestLeaveDetails> vacationServiceRequestLeaveDetails = new();
                        foreach (var item in list)
                        {

                            TblHRMTrnEmployeeVacationServiceRequestLeaveDetails vacationServiceRequestLeaveDetail = new()
                            {
                                EmployeeServiceRequestID = serviceReq.Id,
                                FromDate = item.FromDate,
                                ToDate = item.ToDate,
                                LeaveTypeCode = item.LeaveTypeCode,
                                NoOfDays = item.NoOfDays,
                            };
                            vacationServiceRequestLeaveDetails.Add(vacationServiceRequestLeaveDetail);
                        }

                        if (vacationServiceRequestLeaveDetails.Count > 0)
                        {
                            await _context.EmployeeVacationServiceRequestLeaveDetails.AddRangeAsync(vacationServiceRequestLeaveDetails);
                            await _context.SaveChangesAsync();
                        }

                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateVacationRequest method Exit----");
                    return ApiMessageInfo.Status(1, serviceReq.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateVacationRequest Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(ex.Message + " " + ex.StackTrace);
                }
            }
        }
    }

    #endregion


    #region RejectApproveVacationRequest
    public class RejectApproveVacationRequest : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public ServiceRequestDataDto Input { get; set; }
    }
    public class RejectApproveVacationRequestHandler : IRequestHandler<RejectApproveVacationRequest, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public RejectApproveVacationRequestHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(RejectApproveVacationRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info RejectApproveVacationRequest method start----");
                    var obj = request.Input;
                    var userId = request.User.UserId;

                    TblHRMTrnEmployeeServiceRequest serviceReq = await _context.EmployeeServiceRequests.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    if (serviceReq is null)
                        return ApiMessageInfo.Status("No ServiceRequest");

                    var empControlUserIds = _context.EmployeeControls.Where(e => e.IsUser == true).Select(e => e.UserId);
                    if (!empControlUserIds.Any(e => e.Value == userId))
                        return ApiMessageInfo.Status("don't have approval permission");

                    if (obj.ActionType == (int)ProcessStage.Approved)
                    {

                        var brachCode = await _context.EmployeeContracts.Where(e => e.EmployeeID == serviceReq.EmployeeID).Select(e => e.BranchCode).FirstOrDefaultAsync();
                        var approvals = _context.ServiceRequestApprovalAuthorityMatrix.Where(e => e.BranchCode == brachCode);
                        var approvers = approvals.Select(e => e.ManagerEmployeeID);


                        var approversCount = await approvers.CountAsync();
                        var approvedCount = 0;

                        var audits = await _context.GetAuditApprovalInfo(serviceReq.Id);
                        //var audits = _context.EmployeeServiceRequestAudits.Where(e => e.EmployeeServiceRequestID == serviceReq.Id).OrderByDescending(e => e.Id);
                        //var lastRejectedId = await audits.Where(e => e.ActionID == (int)ProcessStage.Reject).Select(e => e.Id).FirstOrDefaultAsync();
                        //audits = audits.Where(e => e.Id > lastRejectedId && e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);


                        approvedCount = await audits.CountAsync();

                        //////check if it has any rejected rows,else none
                        ////var hasRejected = await audits.AnyAsync(e => e.ActionID == (int)ProcessStage.Reject);
                        ////if (hasRejected)
                        ////{
                        ////    var lastRejectedId = await audits.Where(e => e.ActionID == (int)ProcessStage.Reject).Select(e => e.Id).FirstOrDefaultAsync();
                        ////    audits = audits.Where(e => e.Id > lastRejectedId && e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);
                        ////    approvedCount = await audits.CountAsync();
                        ////}
                        ////else
                        ////{
                        ////    audits = audits.Where(e => e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);
                        ////    approvedCount = await audits.CountAsync();
                        ////}

                        //check duplicate approval entry 
                        var existingApprovalEntries = audits.Select(e => e.EntryBy);
                        if (existingApprovalEntries.Any(ent => ent == userId))
                            return ApiMessageInfo.Status("you have already approved");

                        //check all approvals are completed
                        if (approversCount == approvedCount)
                            return ApiMessageInfo.Status("all approvals are completed");

                        if ((approversCount - approvedCount) == 1)
                        {
                            serviceReq.IsApproved = true;
                        }
                    }


                    serviceReq.ActionID = obj.ActionType;
                    serviceReq.Modified = DateTime.Now;
                    serviceReq.ModifiedBy = userId;
                    _context.EmployeeServiceRequests.Update(serviceReq);

                    if (serviceReq.IsApproved)
                    {
                        var requestInfos = await _context.EmployeeVacationServiceRequestLeaveDetails.Where(e => e.EmployeeServiceRequestID == serviceReq.Id).ToListAsync();
                        if (requestInfos.Any())
                        {
                            List<TblHRMTrnEmployeeLeaveInformation> employeeLeaveInformations = new();
                            foreach (var item in requestInfos)
                            {
                                var template = await _context.LeaveTemplateMappings.Select(e => new { e.LeaveTypeCode, e.TemplateCode, e.Count })
                                    .FirstOrDefaultAsync(e => e.LeaveTypeCode == item.LeaveTypeCode);

                                employeeLeaveInformations.Add(new()
                                {
                                    TemplateCode = template.TemplateCode,
                                    LeaveTypeCode = item.LeaveTypeCode,
                                    Assigned = 0,
                                    Availed = item.NoOfDays,
                                    EmployeeID = serviceReq.EmployeeID,
                                    IsActive = true,
                                    TranDate = DateTime.Now,
                                    Created = DateTime.Now,
                                    CreatedBy = userId,
                                });
                            }

                            if (employeeLeaveInformations.Count > 0)
                            {
                                await _context.EmployeeLeaveInformations.AddRangeAsync(employeeLeaveInformations);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }


                    TblHRMTrnEmployeeServiceRequestAudit employeeServiceRequestAudit = new()
                    {
                        EmployeeServiceRequestID = serviceReq.Id,
                        ActionID = obj.ActionType,
                        EntryBy = userId,
                        EntryDate = DateTime.Now,
                        ServiceRequestProcessStageID = 0,
                        Remarks = obj.Remarks,
                    };
                    await _context.EmployeeServiceRequestAudits.AddAsync(employeeServiceRequestAudit);
                    await _context.SaveChangesAsync();





                    await transaction.CommitAsync();

                    Log.Info("----Info RejectApproveVacationRequest method Exit----");
                    return ApiMessageInfo.Status(1, serviceReq.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in RejectApproveVacationRequest Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region ApprovalVacationRequestList

    public class ApprovalVacationRequestList : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public ApprovalListDto Input { get; set; }
    }
    public class ApprovalVacationRequestListHandler : IRequestHandler<ApprovalVacationRequestList, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ApprovalVacationRequestListHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(ApprovalVacationRequestList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info ApprovalVacationRequestList method start----");
            var obj = request.Input;
            var userId = request.User.UserId;
            var ActionType = (int)ProcessStage.Approved;//obj.ActionType;
            short approvalCount = 0;
            if (!obj.Ids.Any())
                return ApiMessageInfo.Status("No Approvals found");

            foreach (var serviceId in obj.Ids)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        TblHRMTrnEmployeeServiceRequest serviceReq = await _context.EmployeeServiceRequests.FirstOrDefaultAsync(e => e.Id == serviceId);
                        if (serviceReq is not null)
                        {
                            var empControlUserIds = _context.EmployeeControls.Where(e => e.IsUser == true).Select(e => e.UserId);
                            //check loggedin user has approval mapping
                            if (empControlUserIds.Any(e => e.Value == userId))
                            {
                                var brachCode = await _context.EmployeeContracts.Where(e => e.EmployeeID == serviceReq.EmployeeID).Select(e => e.BranchCode).FirstOrDefaultAsync();
                                var approvals = _context.ServiceRequestApprovalAuthorityMatrix.Where(e => e.BranchCode == brachCode);
                                var approvers = approvals.Select(e => e.ManagerEmployeeID);

                                var approversCount = await approvers.CountAsync();
                                var approvedCount = 0;

                                var audits = await _context.GetAuditApprovalInfo(serviceReq.Id);
                                approvedCount = await audits.CountAsync();

                                //check no duplicate approval entry 
                                var existingApprovalEntries = audits.Select(e => e.EntryBy);
                                if (!existingApprovalEntries.Any(ent => ent == userId))
                                {
                                    //check all approvals are not completed
                                    if (approversCount != approvedCount)
                                    {
                                        if ((approversCount - approvedCount) == 1)
                                        {
                                            serviceReq.IsApproved = true;
                                        }

                                        serviceReq.ActionID = ActionType;
                                        serviceReq.Modified = DateTime.Now;
                                        serviceReq.ModifiedBy = userId;
                                        _context.EmployeeServiceRequests.Update(serviceReq);
                                        await _context.SaveChangesAsync();

                                        //if it is last approval in the sequence
                                        if (serviceReq.IsApproved)
                                        {
                                            var requestInfos = await _context.EmployeeVacationServiceRequestLeaveDetails.Where(e => e.EmployeeServiceRequestID == serviceReq.Id)
                                                .OrderBy(e => e.Id).ToListAsync();
                                            if (requestInfos.Any())
                                            {
                                                List<TblHRMTrnEmployeeLeaveInformation> employeeLeaveInformations = new();
                                                foreach (var item in requestInfos)
                                                {
                                                    var template = await _context.LeaveTemplateMappings.Select(e => new { e.LeaveTypeCode, e.TemplateCode, e.Count })
                                                        .FirstOrDefaultAsync(e => e.LeaveTypeCode == item.LeaveTypeCode);

                                                    employeeLeaveInformations.Add(new()
                                                    {
                                                        TemplateCode = template.TemplateCode,
                                                        LeaveTypeCode = item.LeaveTypeCode,
                                                        Assigned = 0,
                                                        Availed = item.NoOfDays,
                                                        EmployeeID = serviceReq.EmployeeID,
                                                        IsActive = true,
                                                        TranDate = DateTime.Now,
                                                        Created = DateTime.Now,
                                                        CreatedBy = userId,
                                                    });
                                                }

                                                if (employeeLeaveInformations.Count > 0)
                                                {
                                                    await _context.EmployeeLeaveInformations.AddRangeAsync(employeeLeaveInformations);
                                                    await _context.SaveChangesAsync();
                                                }


                                                TblHRMTrnEmployeeVacationDateLog tblHRMTrnEmployeeVacationDateLog = new()
                                                {
                                                    EmployeeID = serviceReq.EmployeeID,
                                                    EmployeeServiceRequestID = serviceReq.Id,
                                                    FromDate = requestInfos.First().FromDate,
                                                    ToDate = requestInfos.Last().ToDate,
                                                    IsActive = true,
                                                    Created = DateTime.Now,
                                                    CreatedBy = userId,
                                                };
                                                await _context.EmployeeVacationDateLogs.AddAsync(tblHRMTrnEmployeeVacationDateLog);
                                                await _context.SaveChangesAsync();
                                            }
                                        }

                                        TblHRMTrnEmployeeServiceRequestAudit employeeServiceRequestAudit = new()
                                        {
                                            EmployeeServiceRequestID = serviceReq.Id,
                                            ActionID = ActionType,
                                            EntryBy = userId,
                                            EntryDate = DateTime.Now,
                                            ServiceRequestProcessStageID = 0,
                                            Remarks = obj.Remarks,
                                        };

                                        await _context.EmployeeServiceRequestAudits.AddAsync(employeeServiceRequestAudit);
                                        await _context.SaveChangesAsync();

                                        approvalCount++;

                                        await transaction.CommitAsync();
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in ApprovalVacationRequestList Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return ApiMessageInfo.Status(0);// ex.Message + " " + ex.InnerException.Message + " " + ex.StackTrace);
                    }
                }
            }

            Log.Info("----Info ApprovalVacationRequestList method Exit----");
            return ApiMessageInfo.Status(1, approvalCount);
        }
    }

    #endregion



    #region CreateVacationReleaseExit

    public class CreateVacationReleaseExit : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateVacationReleaseExitDto Input { get; set; }
    }
    public class CreateVacationReleaseExitHandler : IRequestHandler<CreateVacationReleaseExit, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateVacationReleaseExitHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateVacationReleaseExit request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateVacationReleaseExit method start----");
            var obj = request.Input;
            var userId = request.User.UserId;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //var obj = request.Input;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateVacationReleaseExit Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);// ex.Message + " " + ex.InnerException.Message + " " + ex.StackTrace);
                }
            }

            Log.Info("----Info CreateVacationReleaseExit method Exit----");
            return ApiMessageInfo.Status(1);
        }
    }

    #endregion


    #region CreateVacationReportEntry
    public class CreateVacationReportEntry : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateVacationReportEntryDto Input { get; set; }
    }
    public class CreateVacationReportEntryHandler : IRequestHandler<CreateVacationReportEntry, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateVacationReportEntryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateVacationReportEntry request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateVacationReportEntry method start----");
            var obj = request.Input;
            var userId = request.User.UserId;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateVacationReportEntry Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);// ex.Message + " " + ex.InnerException.Message + " " + ex.StackTrace);
                }
            }

            Log.Info("----Info CreateVacationReportEntry method Exit----");
            return ApiMessageInfo.Status(1);
        }
    }

    #endregion
}
