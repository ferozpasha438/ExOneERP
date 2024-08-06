using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
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
using CIN.Domain.SchoolMgt;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetSchoolMessageDetails
    public class GetSchoolMessageDetails : IRequest<List<TblSchoolMessagesDto>>
    {
        public UserIdentityDto User { get; set; }

        public int Mobile { get; set; }
    }

    public class GetSchoolMessageDetailsHandler : IRequestHandler<GetSchoolMessageDetails, List<TblSchoolMessagesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolMessageDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSchoolMessagesDto>> Handle(GetSchoolMessageDetails request, CancellationToken cancellationToken)
        {

            var schoolMessages = await _context.SchoolMessages.AsNoTracking().ProjectTo<TblSchoolMessagesDto>(_mapper.ConfigurationProvider).Where(e => e.Mobile == request.Mobile).ToListAsync();


            return schoolMessages;
        }


    }

    #endregion


    #region GetSchoolScheduleEvents
    public class GetSchoolScheduleEvents : IRequest<List<TblSysSchooScheduleEventsDto>>
    {
        public UserIdentityDto User { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }


    }

    public class GetSchoolScheduleEventsHandler : IRequestHandler<GetSchoolScheduleEvents, List<TblSysSchooScheduleEventsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolScheduleEventsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchooScheduleEventsDto>> Handle(GetSchoolScheduleEvents request, CancellationToken cancellationToken)
        {

            var schoolEvents = await _context.SchooScheduleEvents.AsNoTracking().ProjectTo<TblSysSchooScheduleEventsDto>(_mapper.ConfigurationProvider).Where(e => e.HDate.Month == request.Month && e.HDate.Year == request.Year).ToListAsync();

            return schoolEvents;
        }


    }

    #endregion

    #region GetScheduleEventsPagedList

    public class GetScheduleEventsPagedList : IRequest<PaginatedList<TblSysSchooScheduleEventsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetScheduleEventsPagedListHandler : IRequestHandler<GetScheduleEventsPagedList, PaginatedList<TblSysSchooScheduleEventsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetScheduleEventsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchooScheduleEventsDto>> Handle(GetScheduleEventsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.SchooScheduleEvents.AsNoTracking()
              .Where(e => (e.BranchCode.Contains(search) ||
                            e.EventName.Contains(search) ||
                            e.EventNameAr.Contains(search) ||
                            e.EventDescription.Contains(search) ||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSysSchooScheduleEventsDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion   

    #region CreateUpdateSheduleEvent
    public class CreateSheduleEvent : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchooScheduleEventsDto SheduleEventDto { get; set; }
    }

    public class CreateSheduleEventHandler : IRequestHandler<CreateSheduleEvent, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSheduleEventHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSheduleEvent request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSheduleEvent method start----");



                var obj = request.SheduleEventDto;


                TblSysSchooScheduleEvents SheduleEvent = new();
                if (obj.Id > 0)
                    SheduleEvent = await _context.SchooScheduleEvents.FirstOrDefaultAsync(e => e.Id == obj.Id);

                SheduleEvent.HDate = obj.HDate;
                SheduleEvent.FromTime = obj.FromTime;
                SheduleEvent.ToTime = obj.ToTime;
                SheduleEvent.EventDescription = obj.EventDescription;
                SheduleEvent.EventDescriptionAr = obj.EventDescriptionAr;
                SheduleEvent.EventName = obj.EventName;
                SheduleEvent.EventNameAr = obj.EventNameAr;
                SheduleEvent.NotesOnEvent = obj.NotesOnEvent;
                SheduleEvent.IsActive = obj.IsActive;
                SheduleEvent.IsApproved = false;
                SheduleEvent.BranchCode = obj.BranchCode;



                if (obj.Id > 0)
                {


                    _context.SchooScheduleEvents.Update(SheduleEvent);
                }
                else
                {

                    SheduleEvent.EventCreatedOn = DateTime.Now;
                    SheduleEvent.EventCreatedBy = request.User.UserId.ToString();
                    await _context.SchooScheduleEvents.AddAsync(SheduleEvent);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSheduleEvent method Exit----");
                return SheduleEvent.Id;

            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSheduleEvent Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion   


    #region GetScheduleEventById
    public class GetScheduleEventById : IRequest<TblSysSchooScheduleEventsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetScheduleEventByIdHandler : IRequestHandler<GetScheduleEventById, TblSysSchooScheduleEventsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetScheduleEventByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSysSchooScheduleEventsDto> Handle(GetScheduleEventById request, CancellationToken cancellationToken)
        {
            TblSysSchooScheduleEventsDto obj = new();
            var SheduleEvent = await _context.SchooScheduleEvents.AsNoTracking().ProjectTo<TblSysSchooScheduleEventsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);

            return SheduleEvent;
        }
    }

    #endregion  


    #region DeleteScheduleEvent
    public class DeleteScheduleEvent : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteScheduleEventQueryHandler : IRequestHandler<DeleteScheduleEvent, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteScheduleEventQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteScheduleEvent request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteScheduleEvent start----");

                if (request.Id > 0)
                {
                    var ScheduleEvent = await _context.SchooScheduleEvents.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(ScheduleEvent);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteScheduleEvent");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region EventApproval
    public class EventApproval : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class EventApprovalHandler : IRequestHandler<EventApproval, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public EventApprovalHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(EventApproval request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info EventApproval method start----");
                var scheduleEvent = await _context.SchooScheduleEvents.SingleOrDefaultAsync(e => e.Id == request.Id);
                if (scheduleEvent != null)
                {
                    scheduleEvent.ApprovedBy = Convert.ToString(request.User.UserId);
                    scheduleEvent.IsApproved = true;
                    _context.SchooScheduleEvents.Update(scheduleEvent);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info EventApproval method Exit----");
                    return scheduleEvent.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in EventApproval Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion   
}
