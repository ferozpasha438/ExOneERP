using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList
    public class GetHolidayCalendarList : IRequest<PaginatedList<TblHRMSysHolidayCalendarDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class GetHolidayCalendarListHandler : IRequestHandler<GetHolidayCalendarList, PaginatedList<TblHRMSysHolidayCalendarDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidayCalendarListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysHolidayCalendarDto>> Handle(GetHolidayCalendarList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetHolidayCalendarList method start----");
                var search = request.Input.Query;
                var list = await _context.HolidayCalendars.AsNoTracking().ProjectTo<TblHRMSysHolidayCalendarDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.HolidayCalendarCode.Contains(search) || e.HolidayCalendarNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetHolidayCalendarList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetHolidayCalendarList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetHolidayCalendarById

    public class GetHolidayCalendarById : IRequest<TblHRMSysHolidayCalendarDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetHolidayCalendarByIdHandler : IRequestHandler<GetHolidayCalendarById, TblHRMSysHolidayCalendarDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidayCalendarByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysHolidayCalendarDto> Handle(GetHolidayCalendarById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetHolidayCalendarById method start----");
            try
            {
                var holidayCalendar = await _context.HolidayCalendars.AsNoTracking()
                    .ProjectTo<TblHRMSysHolidayCalendarDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetHolidayCalendarById method end----");

                if (holidayCalendar is not null)
                {
                    var employeeCalendarMappings = await _context.HolidayCalendarMappings.AsNoTracking()
                    .Where(e => e.HolidayCalendarCode == holidayCalendar.HolidayCalendarCode)
                    .ProjectTo<TblHRMSysHolidayCalendarMappingDto>(_mapper.ConfigurationProvider)
                    .OrderByDescending(e => e.Id)
                    .ToListAsync(cancellationToken);

                    employeeCalendarMappings.ForEach(e => e.Checked = true);

                    if (employeeCalendarMappings is not null && employeeCalendarMappings.Count() > 0)
                    {
                        holidayCalendar.HolidayCalendarMappings = employeeCalendarMappings;
                    }
                    return holidayCalendar;
                }

                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetHolidayCalendarById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateHolidayCalendar

    public class CreateUpdateHolidayCalendar : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysHolidayCalendarDto Input { get; set; }
    }

    public class CreateUpdateHolidayCalendarHandler : IRequestHandler<CreateUpdateHolidayCalendar, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateHolidayCalendarHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateHolidayCalendar request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateHolidayCalendar method start----");
                    var obj = request.Input;
                    TblHRMSysHolidayCalendar holidayCalendar = new();
                    if (request.Input.Id > 0)
                    {
                        holidayCalendar = await _context.HolidayCalendars.FirstOrDefaultAsync(e => e.HolidayCalendarCode == request.Input.HolidayCalendarCode);
                        holidayCalendar.Year = obj.Year;
                        holidayCalendar.HolidayCalendarNameEn = obj.HolidayCalendarNameEn;
                        holidayCalendar.HolidayCalendarNameAr = obj.HolidayCalendarNameAr;
                        holidayCalendar.Remarks = obj.Remarks;
                        holidayCalendar.Id = obj.Id;
                        holidayCalendar.IsActive = obj.IsActive;
                        holidayCalendar.ModifiedBy = request.User.UserId;
                        holidayCalendar.Modified = DateTime.Now;

                        var employeeCalendarMappings = _context.HolidayCalendarMappings.Where(e => e.HolidayCalendarCode == request.Input.HolidayCalendarCode);
                        _context.RemoveRange(employeeCalendarMappings);
                        _context.HolidayCalendars.Update(holidayCalendar);
                    }
                    else
                    {
                        holidayCalendar = new()
                        {
                            HolidayCalendarCode = obj.HolidayCalendarCode,
                            HolidayCalendarNameEn = obj.HolidayCalendarNameEn,
                            HolidayCalendarNameAr = obj.HolidayCalendarNameAr,
                            Year = obj.Year,
                            Remarks = obj.Remarks,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now
                        };
                        await _context.HolidayCalendars.AddAsync(holidayCalendar);
                    }
                    await _context.SaveChangesAsync();

                    var employeeCalendarMappingHolidayCode = holidayCalendar.HolidayCalendarCode;
                    var holidayCalendarMappingsDto = request.Input.HolidayCalendarMappings;
                    if (holidayCalendarMappingsDto.Count() > 0)
                    {
                        List<TblHRMSysHolidayCalendarMapping> employeeCalendarMapping = new();

                        foreach (var holidayCalendarMappingDto in holidayCalendarMappingsDto)
                        {
                            var calendarMapping = new TblHRMSysHolidayCalendarMapping
                            {
                                HolidayCalendarCode = employeeCalendarMappingHolidayCode,
                                HolidayCode = holidayCalendarMappingDto.HolidayCode

                            };
                            employeeCalendarMapping.Add(calendarMapping);
                        }
                        if (employeeCalendarMapping.Count > 0)
                            await _context.HolidayCalendarMappings.AddRangeAsync(employeeCalendarMapping);

                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateHolidayCalendar method Exit----");
                    return ApiMessageInfo.Status(1, holidayCalendar.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePersonalInformation Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region Delete HolidayCalendar
    public class DeleteHolidayCalendar : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteHolidayCalendarHandler : IRequestHandler<DeleteHolidayCalendar, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteHolidayCalendarHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteHolidayCalendar request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info DeleteHolidayCalendar method start----");
                    if (request.Id > 0)
                    {

                        var holidayCalendar = await _context.HolidayCalendars.FirstOrDefaultAsync(e => e.Id == request.Id);

                        var holidayCalendarMappings = await _context.HolidayCalendarMappings
                            .Where(e => e.HolidayCalendarCode == holidayCalendar.HolidayCalendarCode).ToListAsync();

                        if (holidayCalendarMappings is not null)
                        {
                            _context.HolidayCalendarMappings.RemoveRange(holidayCalendarMappings);
                            //await _context.SaveChangesAsync();
                        }
                        _context.HolidayCalendars.Remove(holidayCalendar);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        Log.Info("----Info DeleteHolidayCalendar method end----");
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in DeleteHolidayCalendar Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetHolidayCalendarSelectListItem
    public class GetHolidayCalendarSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetHolidayCalendarSelectListItemHandler : IRequestHandler<GetHolidayCalendarSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidayCalendarSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetHolidayCalendarSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.HolidayCalendars.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.HolidayCalendarNameAr : e.HolidayCalendarNameEn, Value = e.HolidayCalendarCode })
                  .ToListAsync(cancellationToken);

            return list;

        }
    }
    #endregion

}
