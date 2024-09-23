using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.Application.SystemSetupDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetEmployeeLeaveInformationById

    public class GetEmployeeLeaveInformationById : IRequest<BaseEmployeeLeaveInformationDto>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeLeaveInformationByIdHandler : IRequestHandler<GetEmployeeLeaveInformationById, BaseEmployeeLeaveInformationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeLeaveInformationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseEmployeeLeaveInformationDto> Handle(GetEmployeeLeaveInformationById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeLeaveInformationById method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                BaseEmployeeLeaveInformationDto baseEmployeeLeaveInformationDto = new();

                if (request.EmployeeID > 0)
                {

                    var employeeLeaves = await (from employeeLeaveInformation in _context.EmployeeLeaveInformations
                                                join personalInformation in _context.PersonalInformation on employeeLeaveInformation.EmployeeID equals personalInformation.Id
                                                //join leaveTemplate in _context.LeaveTemplates on employeeLeaveInformation.TemplateCode equals leaveTemplate.TemplateCode
                                                join leaveType in _context.LeaveTypes on employeeLeaveInformation.LeaveTypeCode equals leaveType.LeaveTypeCode
                                                select new TblHRMTrnEmployeeLeaveInformationDto
                                                {
                                                    Id = employeeLeaveInformation.Id,
                                                    EmployeeID = employeeLeaveInformation.EmployeeID,
                                                    EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                                    TemplateCode = employeeLeaveInformation.TemplateCode,
                                                    //TemplateName = isArab ? leaveTemplate.TemplateNameAr : leaveTemplate.TemplateNameEn,
                                                    LeaveTypeCode = employeeLeaveInformation.LeaveTypeCode,
                                                    LeaveTypeName = isArab ? leaveType.LeaveTypeAr : leaveType.LeaveTypeEn,
                                                    Type = leaveType.Type,
                                                    Assigned = employeeLeaveInformation.Assigned,
                                                    Availed = employeeLeaveInformation.Availed,
                                                    TranDate = employeeLeaveInformation.TranDate,
                                                    Remarks = employeeLeaveInformation.Remarks,
                                                }).AsNoTracking()
                               .Where(e => e.EmployeeID == request.EmployeeID).OrderBy(e => e.Id).ToListAsync(cancellationToken);

                    if (employeeLeaves is not null && employeeLeaves.Count() > 0)
                    {
                        //Retrieve PackageCode.
                        baseEmployeeLeaveInformationDto.LeaveTemplateCode = employeeLeaves.FirstOrDefault().TemplateCode;
                        baseEmployeeLeaveInformationDto.EmployeeLeaves = employeeLeaves;
                    }

                    Log.Info("----Info GetEmployeeLeaveInformationById method end----");
                    return baseEmployeeLeaveInformationDto;
                }
                else
                {
                    Log.Info("----Info GetEmployeeLeaveInformationById method end----");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeLeaveInformationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeLeaveInformation

    public class CreateUpdateEmployeeLeaveInformation : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public BaseEmployeeLeaveInformationDto Input { get; set; }
    }
    public class CreateUpdateEmployeeLeaveInformationHandler : IRequestHandler<CreateUpdateEmployeeLeaveInformation, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeLeaveInformationHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeLeaveInformation request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeLeaveInformation method start----");
                    var obj = request.Input;
                    int _employeeID = 0;
                    TblHRMSysLeaveTemplate leaveTemplate = new();

                    //Retrieve LeaveTemplate details based on TemplateCode.
                    leaveTemplate = await _context.LeaveTemplates.FirstOrDefaultAsync(e => e.TemplateCode == obj.LeaveTemplateCode);

                    if (leaveTemplate is not null)
                    {
                        //Add Leaves from Template.
                        if (obj.EmployeeLeaves.Count() > 0)
                        {
                            _employeeID = obj.EmployeeLeaves.FirstOrDefault().EmployeeID;

                            //Retrieve the list of old EmployeePayrollComponents and delete.
                            var _oldEmployeeLeaves = await _context.EmployeeLeaveInformations.Where(e => e.EmployeeID == _employeeID).ToListAsync();
                            if (_oldEmployeeLeaves is not null) _context.EmployeeLeaveInformations.RemoveRange(_oldEmployeeLeaves);

                            List<TblHRMTrnEmployeeLeaveInformation> employeeLeaves = new();
                            obj.EmployeeLeaves.ForEach(e =>
                            {
                                TblHRMTrnEmployeeLeaveInformation employeeLeave = new TblHRMTrnEmployeeLeaveInformation()
                                {
                                    EmployeeID = e.EmployeeID,
                                    TemplateCode = e.TemplateCode,
                                    LeaveTypeCode = e.LeaveTypeCode,
                                    Availed = e.Availed,
                                    Assigned = e.Assigned,
                                    TranDate = e.TranDate,
                                    Remarks = e.Remarks,
                                    IsActive = true,
                                    CreatedBy = request.User.UserId,
                                    Created = DateTime.Now,
                                };
                                employeeLeaves.Add(employeeLeave);
                            });

                            if (employeeLeaves.Count > 0)
                            {
                                await _context.EmployeeLeaveInformations.AddRangeAsync(employeeLeaves);
                                await _context.SaveChangesAsync();
                            }
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            Log.Info("----Info CreateUpdateEmployeeLeaveInformation method Exit with ConfigureLeaves message ----");
                            return ApiMessageInfo.Status("ConfigureLeaves");
                        }
                    }
                    else
                    {
                        Log.Info("----Info CreateUpdateEmployeeLeaveInformation method Exit with LeaveTemplateDoesNotExist message ----");
                        return ApiMessageInfo.Status("LeaveTemplateDoesNotExist");
                    }
                    Log.Info("----Info CreateUpdateEmployeeLeaveInformation method Exit----");
                    return ApiMessageInfo.Status(1, obj.EmployeeLeaves.Count());
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeLeaveInformation Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion    

    #region GetEmployeeLeaveTemplateMappings

    public class GetEmployeeLeaveTemplateMappings : IRequest<List<TblHRMTrnEmployeeLeaveInformationDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TemplateCode { get; set; }
    }

    public class GetEmployeeLeaveTemplateMappingsHandler : IRequestHandler<GetEmployeeLeaveTemplateMappings, List<TblHRMTrnEmployeeLeaveInformationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeLeaveTemplateMappingsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblHRMTrnEmployeeLeaveInformationDto>> Handle(GetEmployeeLeaveTemplateMappings request, CancellationToken cancellationToken)
        {
            try
            {
                var isArab = request.User.Culture.IsArab();
                Log.Info("----Info GetEmployeeLeaveTemplateMappings method start----");

                var leaveTemplateMappings = await (from mappings in _context.LeaveTemplateMappings
                                                       //join leaveTemplate in _context.LeaveTemplates on mappings.TemplateCode equals leaveTemplate.TemplateCode
                                                   join leaveType in _context.LeaveTypes on mappings.LeaveTypeCode equals leaveType.LeaveTypeCode
                                                   select new TblHRMTrnEmployeeLeaveInformationDto
                                                   {
                                                       Id = 0,
                                                       EmployeeID = 0,
                                                       EmployeeName = string.Empty,
                                                       TemplateCode = mappings.TemplateCode,
                                                       //TemplateName = isArab ? leaveTemplate.TemplateNameAr : leaveTemplate.TemplateNameEn,
                                                       LeaveTypeCode = mappings.LeaveTypeCode,
                                                       LeaveTypeName = isArab ? leaveType.LeaveTypeAr : leaveType.LeaveTypeEn,
                                                       Type = leaveType.Type,
                                                       Assigned = mappings.Count,
                                                       Availed = 0,
                                                       TranDate = DateTime.Now,
                                                       Remarks = @"Leaves assigned from backend.",
                                                   }).AsNoTracking()
                                                   .Where(e => e.TemplateCode == request.TemplateCode)
                                                   .OrderBy(e => e.Id).ToListAsync(cancellationToken);


                Log.Info("----Info GetEmployeeLeaveTemplateMappings method end----");
                return leaveTemplateMappings;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeLeaveTemplateMappings Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion


    #region GetLeaveAdjTransactionList

    public class GetLeaveAdjTransactionList : IRequest<PaginatedList<TblHRMTrnEmployeeLeaveInformationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCreateUpdateLeaveAdjTransactionListHandler : IRequestHandler<GetLeaveAdjTransactionList, PaginatedList<TblHRMTrnEmployeeLeaveInformationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCreateUpdateLeaveAdjTransactionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblHRMTrnEmployeeLeaveInformationDto>> Handle(GetLeaveAdjTransactionList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.EmployeeLeaveInformations.AsNoTracking()
              .Where(e => e.IsLeaveAdjusted == true)
               .OrderByDescending(e => e.Id)
               .Select(e => new TblHRMTrnEmployeeLeaveInformationDto
               {
                   Id = e.Id,
                   EmployeeID = e.EmployeeID,
                   TranDate = e.TranDate,
                   Remarks = e.Remarks,
                   IsActive = e.IsActive,
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            bool isArab = request.User.Culture.IsArab();
            foreach (var item in list.Items)
            {
                var emp = await _context.PersonalInformation.FirstOrDefaultAsync(e => e.Id == item.EmployeeID);
                item.EmployeeNumber = emp.EmployeeNumber;
                item.EmployeeName = isArab ? $"{emp.FirstNameAr} {emp.LastNameAr}" : $"{emp.FirstNameEn} {emp.LastNameEn}";
            }
            return list;
        }
    }

    #endregion


    #region GetLeaveAdjTransactionById

    public class GetLeaveAdjTransactionById : IRequest<TblHRMTrnEmployeeLeaveInformationDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetLeaveAdjTransactionByIdHandler : IRequestHandler<GetLeaveAdjTransactionById, TblHRMTrnEmployeeLeaveInformationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveAdjTransactionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblHRMTrnEmployeeLeaveInformationDto> Handle(GetLeaveAdjTransactionById request, CancellationToken cancellationToken)
        {
            var item = await _context.EmployeeLeaveInformations.AsNoTracking()
              .Where(e => e.IsLeaveAdjusted == true)
               .OrderByDescending(e => e.Id)
               .Select(e => new TblHRMTrnEmployeeLeaveInformationDto
               {
                   Id = e.Id,
                   EmployeeID = e.EmployeeID,
                   TranDate = e.TranDate,
                   Remarks = e.Remarks,
                   IsActive = e.IsActive,
                   TemplateCode = e.TemplateCode,
                   LeaveTypeCode = e.LeaveTypeCode,
                   TypeOfAdj = e.Assigned > 0 ? "Assigned" : "Availed",
                   NoOfDays = e.Assigned > 0 ? e.Assigned : e.Availed,
               })
                 .FirstOrDefaultAsync(e => e.Id == request.Id);

            return item;
        }
    }

    #endregion

    #region CreateUpdateLeaveAdjTransaction

    public class CreateUpdateLeaveAdjTransaction : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateUpdateLeaveAdjTransactionDto Input { get; set; }
    }
    public class CreateUpdateLeaveAdjTransactionHandler : IRequestHandler<CreateUpdateLeaveAdjTransaction, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateLeaveAdjTransactionHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateLeaveAdjTransaction request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateLeaveAdjTransaction method start----");
                var obj = request.Input.EmployeeLeave;


                if (obj is not null)
                {
                    TblHRMTrnEmployeeLeaveInformation employeeLeave = await _context.EmployeeLeaveInformations.FirstOrDefaultAsync(e => e.Id == obj.Id) ?? new();
                    var template = await _context.EmployeeLeaveInformations.Select(e => new { e.LeaveTypeCode, e.TemplateCode }).FirstOrDefaultAsync(e => e.LeaveTypeCode == obj.LeaveTypeCode);

                    employeeLeave.EmployeeID = obj.EmployeeID;
                    employeeLeave.TemplateCode = template.TemplateCode;
                    employeeLeave.LeaveTypeCode = obj.LeaveTypeCode;

                    if (obj.TypeOfAdj == "Availed")
                    {
                        employeeLeave.Availed = obj.NoOfDays;
                        employeeLeave.Assigned = 0;
                    }
                    else if (obj.TypeOfAdj == "Assigned")
                    {
                        employeeLeave.Assigned = obj.NoOfDays;
                        employeeLeave.Availed = 0;
                    }

                    employeeLeave.TranDate = obj.TranDate;
                    employeeLeave.Remarks = obj.Remarks;
                    employeeLeave.IsActive = obj.IsActive;
                    employeeLeave.CreatedBy = request.User.UserId;
                    employeeLeave.Created = DateTime.Now;
                    employeeLeave.IsLeaveAdjusted = true;

                    if (obj.Id > 0)
                    {
                        _context.EmployeeLeaveInformations.Update(employeeLeave);
                    }
                    else
                    {
                        await _context.EmployeeLeaveInformations.AddAsync(employeeLeave);
                    }
                    await _context.SaveChangesAsync();

                    Log.Info("----Info CreateUpdateLeaveAdjTransaction method Exit----");
                    return ApiMessageInfo.Status(1, employeeLeave.Id);
                }

                return ApiMessageInfo.Status(0);

            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateLeaveAdjTransaction Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion

}
