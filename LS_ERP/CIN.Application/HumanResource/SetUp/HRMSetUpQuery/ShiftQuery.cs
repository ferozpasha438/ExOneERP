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
    public class GetShiftList : IRequest<PaginatedList<TblHRMSysShiftDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetShiftListHandler : IRequestHandler<GetShiftList, PaginatedList<TblHRMSysShiftDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShiftListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysShiftDto>> Handle(GetShiftList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetShiftList method start----");
                var search = request.Input.Query;
                var list = await _context.Shifts.AsNoTracking().ProjectTo<TblHRMSysShiftDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.ShiftCode.Contains(search) || e.ShiftNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetGenderList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region GetShiftById
    public class GetShiftById : IRequest<TblHRMSysShiftDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetShiftByIdHandler : IRequestHandler<GetShiftById, TblHRMSysShiftDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShiftByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysShiftDto> Handle(GetShiftById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetShiftById method start----");
            try
            {
                var shift = await _context.Shifts.AsNoTracking()
                    .ProjectTo<TblHRMSysShiftDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetShiftById method end----");

                if (shift is not null)
                    return shift;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateShift

    public class CreateUpdateShift : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysShiftDto Input { get; set; }
    }

    public class CreateUpdateShiftHandler : IRequestHandler<CreateUpdateShift, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateShiftHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateShift request, CancellationToken cancellationToken)
        {
            try
            {

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info CreateUpdateShift method start----");
                        var obj = request.Input;
                        TimeSpan tsInTime = TimeSpan.Parse(obj.InTime);
                        TimeSpan tsOutTime = TimeSpan.Parse(obj.OutTime);
                        TimeSpan tsBreakTime = TimeSpan.Parse(!string.IsNullOrEmpty(obj.BreakTime) ? obj.BreakTime : "00:00:00");
                        TimeSpan tsInGrace = TimeSpan.Parse(!string.IsNullOrEmpty(obj.InGrace) ? obj.InGrace : "00:00:00");
                        TimeSpan tsOutGrace = TimeSpan.Parse(!string.IsNullOrEmpty(obj.OutGrace) ? obj.InGrace : "00:00:00");
                        TimeSpan interval = new TimeSpan(0, 0, 0, 0);

                        DateTime dtInTime = DateTime.Now.Date + tsInTime;
                        DateTime dtOutTime = DateTime.Now.Date + tsOutTime;

                        TblHRMSysShift shift = new();

                        //Shift Timing Calculations
                        if (DateTime.Compare(dtInTime, dtOutTime) > 0)
                            dtOutTime = dtOutTime.AddDays(1);
                        interval = dtOutTime - dtInTime;


                        if (request.Input.Id > 0)
                        {
                            shift = await _context.Shifts.FirstOrDefaultAsync(e => e.ShiftCode == request.Input.ShiftCode);
                            shift.ShiftNameEn = obj.ShiftNameEn;
                            shift.ShiftNameAr = obj.ShiftNameAr;
                            shift.InTime = tsInTime;
                            shift.OutTime = tsOutTime;
                            shift.BreakTime = tsBreakTime;
                            shift.InGrace = tsInGrace;
                            shift.OutGrace = tsOutGrace;
                            shift.WorkingTime = interval;
                            shift.NetWorkingTime = interval - tsBreakTime;
                            shift.Id = obj.Id;
                            shift.IsActive = obj.IsActive;
                            shift.ModifiedBy = request.User.UserId;
                            shift.Modified = DateTime.Now;
                            _context.Shifts.Update(shift);
                        }
                        else
                        {
                            shift = new()
                            {
                                ShiftCode = obj.ShiftCode,
                                ShiftNameEn = obj.ShiftNameEn,
                                ShiftNameAr = obj.ShiftNameAr,
                                InTime = tsInTime,
                                OutTime = tsOutTime,
                                BreakTime = tsBreakTime,
                                InGrace = tsInGrace,
                                OutGrace = tsOutGrace,
                                WorkingTime = interval,
                                NetWorkingTime = interval - tsBreakTime,
                                IsActive = obj.IsActive,
                                CreatedBy = request.User.UserId,
                                Created = DateTime.Now,
                            };
                            await _context.Shifts.AddAsync(shift);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        Log.Info("----Info CreateUpdateBank method Exit----");
                        return ApiMessageInfo.Status(1, shift.Id);
                    }

                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in CreateUpdateShift Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return ApiMessageInfo.Status(0);
                    }

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return ApiMessageInfo.Status(message: ex.Message, 0);
            }
        }
    }

    #endregion

    #region Delete Shift
    public class DeleteShift : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteShiftHandler : IRequestHandler<DeleteShift, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteShiftHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteShift request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteShift method start----");
                if (request.Id > 0)
                {
                    var shift = await _context.Shifts.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(shift);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteShift method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteShift Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region GetShiftSelectListItem
    public class GetShiftSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }
    public class GetShiftSelectListItemHandler : IRequestHandler<GetShiftSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetShiftSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetShiftSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Shifts.AsNoTracking().OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.ShiftNameAr : e.ShiftNameEn, Value = e.ShiftCode })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
    #endregion


}
