using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
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
}
