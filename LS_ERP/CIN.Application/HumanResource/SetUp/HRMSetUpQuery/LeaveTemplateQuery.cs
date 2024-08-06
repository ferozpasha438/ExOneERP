using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using FluentValidation.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{

    #region GetPagedList

    public class GetLeaveTemplateList : IRequest<PaginatedList<TblHRMSysLeaveTemplateDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetLeaveTemplateListHandler : IRequestHandler<GetLeaveTemplateList, PaginatedList<TblHRMSysLeaveTemplateDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTemplateListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysLeaveTemplateDto>> Handle(GetLeaveTemplateList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetLeaveTemplateList method start----");
                var search = request.Input.Query;
                var list = await _context.LeaveTemplates.AsNoTracking()
                     .Where(e => (e.TemplateCode.Contains(search) || e.TemplateNameEn.Contains(search)) || e.TemplateNameAr.Contains(search))
                   .OrderByDescending(x => x.Id)
                    .ProjectTo<TblHRMSysLeaveTemplateDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetLeaveTemplateList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLeaveTemplateList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetLeaveTemplateById

    public class GetLeaveTemplateById : IRequest<TblHRMSysLeaveTemplateDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetLeaveTemplateByIdHandler : IRequestHandler<GetLeaveTemplateById, TblHRMSysLeaveTemplateDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTemplateByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysLeaveTemplateDto> Handle(GetLeaveTemplateById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetLeaveTemplateById method start----");
            try
            {
                var leaveType = await _context.LeaveTemplates.AsNoTracking()
                    .ProjectTo<TblHRMSysLeaveTemplateDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetLeaveTemplateById method end----");

                if (leaveType is not null)
                    return leaveType;
                else
                    return new();
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLeaveTemplateById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetLeaveTemplateMappingList

    public class GetLeaveTemplateMappingList : IRequest<List<TblHRMSysLeaveTemplateMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TemplateCode { get; set; }
    }

    public class GetLeaveTemplateMappingListHandler : IRequestHandler<GetLeaveTemplateMappingList, List<TblHRMSysLeaveTemplateMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTemplateMappingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblHRMSysLeaveTemplateMappingDto>> Handle(GetLeaveTemplateMappingList request, CancellationToken cancellationToken)
        {
            try
            {
                var isArab = request.User.Culture.IsArab();
                Log.Info("----Info GetLeaveTemplateMappingList method start----");

                var list = await _context.LeaveTypes
                    .AsNoTracking()
                    .Where(e => e.IsActive)
                    .Select(e => new TblHRMSysLeaveTemplateMappingDto()
                    {
                        LeaveTypeCode = e.LeaveTypeCode,
                        LeaveTypeName = isArab ? e.LeaveTypeAr : e.LeaveTypeEn,
                        IsChecked = e.Type == 1 ? true : false //Type 1 = Accrual, 2 = Pro-Rata

                    })
                     .ToListAsync();

                if (request.TemplateCode.HasValue())
                {
                    var leaveTypeCodes = _context.LeaveTemplateMappings.AsNoTracking()
                         .Where(e => e.TemplateCode == request.TemplateCode)
                         .Select(e => new { e.LeaveTypeCode, e.Count });

                    foreach (var item in list)
                    {
                        var leaveType = leaveTypeCodes.FirstOrDefault(e => e.LeaveTypeCode == item.LeaveTypeCode);
                        item.Count = leaveType?.Count ?? 0;
                        item.CanSubmit = leaveType is not null;
                        //item.IsChecked = leaveTypeCodes.Any(e => e.LeaveTypeCode == item.LeaveTypeCode);
                    }
                }

                Log.Info("----Info GetLeaveTemplateMappingList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLeaveTemplateMappingList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateLeaveTemplate

    public class CreateUpdateLeaveTemplate : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysLeaveTemplateDto Input { get; set; }
    }
    public class CreateUpdateLeaveTemplateHandler : IRequestHandler<CreateUpdateLeaveTemplate, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateLeaveTemplateHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateLeaveTemplate request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateLeaveTemplate method start----");
                    var obj = request.Input;
                    var leaveTypeCodes = request.Input.LeaveTypeCodes;

                    TblHRMSysLeaveTemplate leaveTemplate = await _context.LeaveTemplates.FirstOrDefaultAsync(e => e.Id == request.Input.Id) ?? new();
                    leaveTemplate.TemplateNameEn = obj.TemplateNameEn;
                    leaveTemplate.TemplateNameAr = obj.TemplateNameAr;
                    leaveTemplate.GradeCode = obj.GradeCode;
                    leaveTemplate.PositionCode = obj.PositionCode;
                    leaveTemplate.Remarks = obj.Remarks;
                    leaveTemplate.IsActive = obj.IsActive;

                    if (leaveTemplate.Id > 0)
                    {
                        leaveTemplate.ModifiedBy = request.User.UserId;
                        leaveTemplate.Modified = DateTime.Now;
                        _context.LeaveTemplates.Update(leaveTemplate);
                    }
                    else
                    {
                        leaveTemplate.TemplateCode = obj.TemplateCode.ToUpper();
                        leaveTemplate.CreatedBy = request.User.UserId;
                        leaveTemplate.Created = DateTime.Now;
                        await _context.LeaveTemplates.AddAsync(leaveTemplate);
                    }

                    await _context.SaveChangesAsync();


                    if (leaveTypeCodes is not null && leaveTypeCodes.Count > 0)
                    {
                        //leaveTypeCodes = leaveTypeCodes.Where(e => e.Count > 0).ToList();
                        List<TblHRMSysLeaveTemplateMapping> mappings = new();
                        foreach (var typeCode in leaveTypeCodes)
                        {
                            mappings.Add(new()
                            {
                                TemplateCode = leaveTemplate.TemplateCode,
                                LeaveTypeCode = typeCode.LeaveTypeCode,
                                Count = typeCode.Count
                            });
                        }

                        if (mappings.Count > 0)
                        {
                            var tempMappings = _context.LeaveTemplateMappings.Where(e => e.TemplateCode == leaveTemplate.TemplateCode);
                            if (tempMappings.Any())
                                _context.LeaveTemplateMappings.RemoveRange(tempMappings);
                            //await _context.SaveChangesAsync();

                            await _context.LeaveTemplateMappings.AddRangeAsync(mappings);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateLeaveTemplate method Exit----");
                    return ApiMessageInfo.Status(1, leaveTemplate.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateLeaveTemplate Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(ex.Message);
                }
            }
        }
    }

    #endregion

    #region DeleteLeaveTemplate
    public class DeleteLeaveTemplate : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLeaveTemplateHandler : IRequestHandler<DeleteLeaveTemplate, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLeaveTemplateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLeaveTemplate request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info DeleteLeaveTemplate method start----");
                    if (request.Id > 0)
                    {
                        var template = await _context.LeaveTemplates.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.LeaveTemplates.Remove(template);

                        var tempMappings = _context.LeaveTemplateMappings.Where(e => e.TemplateCode == template.TemplateCode);
                        if (tempMappings.Any())
                            _context.LeaveTemplateMappings.RemoveRange(tempMappings);

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        Log.Info("----Info DeleteLeaveTemplate method end----");
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in DeleteLeaveTemplate Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetLeaveTemplateSelectListItem
    public class GetLeaveTemplateSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeLeaveInfoFilterDto Filter { get; set; }
    }

    public class GetLeaveTemplateSelectListItemHandler : IRequestHandler<GetLeaveTemplateSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTemplateSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetLeaveTemplateSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var filter = request.Filter;
            var list = await _context.LeaveTemplates.AsNoTracking()
                .Where(e => (string.IsNullOrEmpty(filter.PositionCode) ? e.PositionCode == null : e.PositionCode == filter.PositionCode) &&
                (string.IsNullOrEmpty(filter.GradeCode) ? e.GradeCode == null : e.GradeCode == filter.GradeCode)).
                OrderByDescending(e => e.Id).
                Select(e => new CustomSelectListItem { Text = isArab ? e.TemplateNameAr : e.TemplateNameEn, Value = e.TemplateCode.ToString() }).
                ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
