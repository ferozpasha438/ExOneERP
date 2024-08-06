using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
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

namespace CIN.Application.TeacherAppMgtQuery
{
    //#region Get_Teacher_Notification_List

    //public class GetTeacherNotificationList : IRequest<List<TblSysSchoolPushNotificationParentDto>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string TeacherCode { get; set; }

    //}

    //public class GetTeacherNotificationListHandler : IRequestHandler<GetTeacherNotificationList, List<TblSysSchoolPushNotificationParentDto>>
    //{
    //    private CINDBOneContext _context;
    //    private IMapper _mapper;
    //    private List<TblSysSchoolPushNotificationParentDto> list;

    //    // private List<TblSysSchoolPushNotificationParentDto> list;

    //    public GetTeacherNotificationListHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        context = _context;
    //        mapper = _mapper;
    //    }

    //    public async Task<List<TblSysSchoolPushNotificationParentDto>> Handle(GetTeacherNotificationList request, CancellationToken cancellationToken)
    //    {
    //      //  var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TeacherCode == request.TeacherCode);

    //        var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider)
    //                                        .ToListAsync();
    //        //     var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo.Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

    //        if (teacher != null)
    //        {
    //            list = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => e.RegisteredMobile == "").ToListAsync();

    //        }


    //        return list;
    //    }

    //}


    //#endregion


    #region GetListOfNotification
    public class GetTeacherNotificationList : IRequest<SchoolPushNotificationTeacherDto>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetTeacherNotificationListHandler : IRequestHandler<GetTeacherNotificationList, SchoolPushNotificationTeacherDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherNotificationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolPushNotificationTeacherDto> Handle(GetTeacherNotificationList request, CancellationToken cancellationToken)
        {

            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();


            return new SchoolPushNotificationTeacherDto
            {
                // countNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e =>e.RegisteredMobile==teacher.PMobile1 && e.NotifyTo == "Teacher").CountAsync(),
                TeacherPushNotifications = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => (e.RegisteredMobile == teacher.PMobile1 && e.NotifyTo == "Teacher") && e.IsRead == false).ToListAsync()
            };

        }


    }


    #endregion

    #region GetTeacherNotificationCount
    public class GetTeacherNotificationCount : IRequest<TeacherNotificationCountDto>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }

    }

    public class GetTeacherNotificationCountHandler : IRequestHandler<GetTeacherNotificationCount, TeacherNotificationCountDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherNotificationCountHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TeacherNotificationCountDto> Handle(GetTeacherNotificationCount request, CancellationToken cancellationToken)
        {
            TeacherNotificationCountDto TeacherNotification = new();
            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).FirstOrDefaultAsync();

            if (teacher != null)
            {


                TeacherNotification.TotalCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => e.RegisteredMobile == teacher.PMobile1 && e.NotifyTo == "Teacher").CountAsync();
                TeacherNotification.ReadCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => (e.RegisteredMobile == teacher.PMobile1 && e.NotifyTo == "Teacher") && e.IsRead == true).CountAsync();
                TeacherNotification.UnreadCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => (e.RegisteredMobile == teacher.PMobile1 && e.NotifyTo == "Teacher") && e.IsRead == false).CountAsync();
            }
            return TeacherNotification;
        }
    }

    #endregion

    #region Teacher Update Notification

    public class TeacherUpdateNotification : IRequest<int>
    {
        public UserIdentityDto User { get; set; }

        public int MessageId { get; set; }
    }

    public class TeacherUpdateNotificationHandler : IRequestHandler<TeacherUpdateNotification, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public TeacherUpdateNotificationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(TeacherUpdateNotification request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Teacher Update Notification Method start----");
                TblSysSchoolPushNotificationParent NotificationParent = new();
                if (request.MessageId > 0)
                    NotificationParent = await _context.PushNotificationParent.AsNoTracking().FirstOrDefaultAsync(e => e.MsgNoteId == request.MessageId && e.NotifyTo == "Teacher");


                if (request.MessageId > 0)
                {
                    NotificationParent.IsRead = true;
                    _context.PushNotificationParent.Update(NotificationParent);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Teacher Update Notification Method Exit----");
                return NotificationParent.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Teacher Update Notification Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }
    #endregion

    //#region Send Staff & Teacher Notification

    //public class SendParentNotification : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public SendNotificationDto sendNotificationDTO { get; set; }
    //}

    //public class SendNotificationHandler : IRequestHandler<SendParentNotification, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public SendNotificationHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(SendParentNotification request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info SendParentNotification method start----");
    //            var obj = request.sendNotificationDTO;

    //            string mobileNumber = string.Empty;
    //            string notifyTo = string.Empty;
    //            if (request.sendNotificationDTO.Contact == "Teacher") 
    //            {
    //              //  var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.PrimaryBranchCode == request.sendNotificationDTO.Branch).FirstOrDefaultAsync();


    //                    var teacherCode = await _context.DefSchoolTeacherClassMapping.AsNoTracking()
    //                                      .Where(e => e.GradeCode == request.sendNotificationDTO.Grade)
    //                                     .Select(x => x.TeacherCode).FirstOrDefaultAsync();


    //                if(teacherCode != null) { 
    //                    mobileNumber = await _context.DefSchoolTeacherMaster.AsNoTracking()
    //                                     .Where(e => (e.PrimaryBranchCode == request.sendNotificationDTO.Branch && e.TeacherCode== teacherCode))
    //                                     .Select(x => x.PMobile1).SingleOrDefaultAsync();
    //                    notifyTo = "Teacher";
    //                }

    //            }
    //            else if (request.sendNotificationDTO.Contact == "Admin") 
    //            {
    //                mobileNumber = await _context.SchoolBranches.AsNoTracking()
    //                                       .Where(e => e.BranchCode == request.sendNotificationDTO.Branch)
    //                                       .Select(x => x.Mobile)
    //                                       .SingleOrDefaultAsync();
    //                notifyTo = "Admin";
    //            }
    //            TblSysSchoolPushNotificationParent notification = new();
    //            if (obj.Id > 0)
    //                notification = await _context.PushNotificationParent.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
    //            notification.Id = obj.Id;
    //            notification.NotifyDate = DateTime.Now;
    //            notification.NotifyMessage = obj.NotifyMessage;
    //            notification.Title = obj.Title;
    //            notification.Title_Ar = obj.Title_Ar;
    //            notification.NotifyMessage = obj.NotifyMessage;
    //            notification.NotifyMessage_Ar = obj.NotifyMessage_Ar;
    //            notification.RegisteredMobile = mobileNumber;
    //            notification.IsRead = false;
    //            notification.NotifyTo = notifyTo;
    //            if (obj.Id > 0)
    //            {
    //                _context.PushNotificationParent.Update(notification);
    //            }
    //            else
    //            {
    //                await _context.PushNotificationParent.AddAsync(notification);
    //            }
    //            await _context.SaveChangesAsync();
    //            Log.Info("----Info Send Parent Notification method Exit----");
    //            return notification.Id;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in Send Parent Notification Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }
    //    }
    //}

    //#endregion
}








