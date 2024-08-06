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
    public class GetHolidayList : IRequest<PaginatedList<TblHRMSysHolidayDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class GetHolidayListHandler : IRequestHandler<GetHolidayList, PaginatedList<TblHRMSysHolidayDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidayListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysHolidayDto>> Handle(GetHolidayList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetHolidayList method start----");
                var search = request.Input.Query;
                var list = await _context.Holidays.AsNoTracking().ProjectTo<TblHRMSysHolidayDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.HolidayCode.Contains(search) || e.HolidayNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetHolidayList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetHolidayList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region GetHolidayById
    public class GetHolidayById : IRequest<TblHRMSysHolidayDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetHolidayByIdHandler : IRequestHandler<GetHolidayById, TblHRMSysHolidayDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidayByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysHolidayDto> Handle(GetHolidayById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetHolidayById method start----");
            try
            {
                var holiday = await _context.Holidays.AsNoTracking()
                    .ProjectTo<TblHRMSysHolidayDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetHolidayById method end----");

                if (holiday is not null)
                    return holiday;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetHolidayById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region CreateUpdateHoliday
    public class CreateUpdateHoliday : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysHolidayDto Input { get; set; }
    }
    public class CreateUpdateHolidayHandler : IRequestHandler<CreateUpdateHoliday, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateHolidayHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateHoliday request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateHoliday method start----");
                    var obj = request.Input;
                    TblHRMSysHoliday holiday = new();
                    if (request.Input.Id > 0)
                    {
                        holiday = await _context.Holidays.FirstOrDefaultAsync(e => e.HolidayCode == request.Input.HolidayCode);
                        holiday.HolidayNameEn = obj.HolidayNameEn;
                        holiday.HolidayNameAr = obj.HolidayNameAr;
                        holiday.Date = obj.Date;
                        holiday.Id = obj.Id;
                        holiday.IsActive = obj.IsActive;
                        holiday.ModifiedBy = request.User.UserId;
                        holiday.Modified = DateTime.Now;

                        _context.Holidays.Update(holiday);
                    }
                    else
                    {
                        holiday = new()
                        {
                            HolidayCode = obj.HolidayCode,
                            HolidayNameEn = obj.HolidayNameEn,
                            HolidayNameAr = obj.HolidayNameAr,
                            Date = obj.Date,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Holidays.AddAsync(holiday);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateHoliday method Exit----");
                    return ApiMessageInfo.Status(1, holiday.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateHoliday Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region Delete Holiday  

    public class DeleteHoliday : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteHolidayHandler : IRequestHandler<DeleteHoliday, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteHolidayHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteHoliday request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteHoliday method start----");
                if (request.Id > 0)
                {
                    var holiday = await _context.Holidays.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(holiday);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteHoliday method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteHoliday Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetHolidaySelectListItem

    public class GetHolidaySelectListItem : IRequest<List<TblHRMSysHolidayDto>>
    {
        public UserIdentityDto User { get; set; }
        public int Year { get; set; }
    }

    public class GetHolidaySelectListItemHandler : IRequestHandler<GetHolidaySelectListItem, List<TblHRMSysHolidayDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetHolidaySelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblHRMSysHolidayDto>> Handle(GetHolidaySelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Holidays
                .Where(e => e.Date.Year == request.Year)
                .AsNoTracking().ProjectTo<TblHRMSysHolidayDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(e => e.Id).ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion

}
