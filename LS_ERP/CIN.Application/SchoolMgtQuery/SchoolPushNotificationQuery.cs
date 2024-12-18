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
using CIN.Application.SchoolMgtDto;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetAllList
    public class GetParentPushNotificationList : IRequest<SchoolPushNotificationParentDto>
    {
        public UserIdentityDto User { get; set; }
        public string Mobile { get; set; }

    }

    public class GetParentPushNotificationListHandler : IRequestHandler<GetParentPushNotificationList, SchoolPushNotificationParentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetParentPushNotificationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolPushNotificationParentDto> Handle(GetParentPushNotificationList request, CancellationToken cancellationToken)
        {

            return new SchoolPushNotificationParentDto
            {
                countNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => e.RegisteredMobile == request.Mobile).CountAsync(),
                ParentPushNotifications = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => e.RegisteredMobile == request.Mobile).ToListAsync()
            };

        }


    }


    #endregion

    #region GetParentNotificationCount
    public class GetParentNotificationCount : IRequest<ParentNotificationCountDto>
    {
        public UserIdentityDto User { get; set; }
        public string Mobile { get; set; }

    }

    public class GetParentNotificationCountHandler : IRequestHandler<GetParentNotificationCount, ParentNotificationCountDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetParentNotificationCountHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ParentNotificationCountDto> Handle(GetParentNotificationCount request, CancellationToken cancellationToken)
        {
            ParentNotificationCountDto ParentNotificationCount = new();



            ParentNotificationCount.TotalCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => e.RegisteredMobile == request.Mobile && e.NotifyTo == "Student").CountAsync();
            ParentNotificationCount.ReadCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => (e.RegisteredMobile == request.Mobile && e.NotifyTo == "Student") && e.IsRead == true).CountAsync();
            ParentNotificationCount.UnreadCountNotification = await _context.PushNotificationParent.AsNoTracking().ProjectTo<TblSysSchoolPushNotificationParentDto>(_mapper.ConfigurationProvider).Where(e => (e.RegisteredMobile == request.Mobile && e.NotifyTo == "Student") && e.IsRead == false).CountAsync();

            return ParentNotificationCount;
        }
    }


}


#endregion

#region Update Notification

public class UpdateNotification : IRequest<int>
{
    public UserIdentityDto User { get; set; }

    public int MessageId { get; set; }
}

public class UpdateNotificationHandler : IRequestHandler<UpdateNotification, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public UpdateNotificationHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateNotification request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Info("----Info UpdateNotification method start----");
            TblSysSchoolPushNotificationParent NotificationParent = new();
            if (request.MessageId > 0)
                NotificationParent = await _context.PushNotificationParent.AsNoTracking().FirstOrDefaultAsync(e => e.MsgNoteId == request.MessageId && e.NotifyTo == "Student");


            if (NotificationParent is not null)
            {
                NotificationParent.IsRead = true;
                _context.PushNotificationParent.Update(NotificationParent);
            }
            await _context.SaveChangesAsync();
            Log.Info("----Info UpdateNotification method Exit----");
            return NotificationParent == null ? 0 : NotificationParent.Id;
        }
        catch (Exception ex)
        {
            Log.Error("Error in UpdateNotification Method");
            Log.Error("Error occured time : " + DateTime.UtcNow);
            Log.Error("Error message : " + ex.Message);
            Log.Error("Error StackTrace : " + ex.StackTrace);
            return 0;
        }

    }
}
#endregion


#region GetIndividualNotificationList

public class GetIndividualNotificationList : IRequest<PaginatedList<EditBulkNotificationsDto>>
{
    public UserIdentityDto User { get; set; }
    public PaginationFilterDto Input { get; set; }
}

public class GetIndividualNotificationListHandler : IRequestHandler<GetIndividualNotificationList, PaginatedList<EditBulkNotificationsDto>>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public GetIndividualNotificationListHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<PaginatedList<EditBulkNotificationsDto>> Handle(GetIndividualNotificationList request, CancellationToken cancellationToken)
    {
        var result = await _context.SchoolNotifications.AsNoTracking()
                                   .Join(_context.SchoolNotificationFilters, sn => sn.Id, snf => snf.NotificationId, (sn, snf) => new
                                   {
                                       sn.Id,
                                       sn.NotificationTitle,
                                       sn.NotificationTitle_Ar,
                                       sn.NotificationMessage,
                                       sn.NotificationMessage_Ar,
                                       sn.IsApproved,
                                       sn.NotificationType,
                                       sn.AcadamicYear,
                                       snf.BranchCode,
                                       snf.GenderCode,
                                       snf.GradeCode,
                                       snf.NationalityCode,
                                       snf.PickUpAndDropZone,
                                       snf.PTGroupCode,
                                       snf.SectionCode
                                   })
                                   .Select(x => new EditBulkNotificationsDto
                                   {
                                       Id = x.Id,
                                       AcadamicYear = x.AcadamicYear,
                                       NotificationTitle = x.NotificationTitle,
                                       NotificationTitle_Ar = x.NotificationTitle_Ar,
                                       NotificationMessage = x.NotificationMessage,
                                       NotificationMessage_Ar = x.NotificationMessage_Ar,
                                       NotificationType = x.NotificationType,
                                       BranchCode = x.BranchCode,
                                       GenderCode = x.BranchCode,
                                       GradeCode = x.GradeCode,
                                       NationalityCode = x.NationalityCode,
                                       PickUpAndDropZone = x.PickUpAndDropZone,
                                       PTGroupCode = x.PTGroupCode,
                                       SectionCode = x.SectionCode,
                                       IsApproved = x.IsApproved
                                   })
                                   .Where(x => x.NotificationType == 3)
                                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
        return result;
    }
}

#endregion

#region GetIndividualNotificationById
public class GetIndividualNotificationById : IRequest<SysSchoolNotificationsDto>
{
    public UserIdentityDto User { get; set; }
    public string Code { get; set; }
    public int TypeId { get; set; } // Teacher=1,Student=2
}

public class GetIndividualNotificationByIdHandler : IRequestHandler<GetIndividualNotificationById, SysSchoolNotificationsDto>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;
    public GetIndividualNotificationByIdHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SysSchoolNotificationsDto> Handle(GetIndividualNotificationById request, CancellationToken cancellationToken)
    {
        SysSchoolNotificationsDto resultData = new SysSchoolNotificationsDto();
        string mobileNumber = string.Empty;

        if (request.TypeId == 1) // teacher
        {
            mobileNumber = await _context.DefSchoolTeacherMaster.AsNoTracking()
                                 .Where(e => e.TeacherCode == request.Code)
                                 .Select(x => x.PMobile1).SingleOrDefaultAsync();
        }
        else if (request.TypeId == 2) // Student
        {
            mobileNumber = await _context.DefSchoolStudentMaster.AsNoTracking()
                                   .Where(e => e.StuAdmNum == request.Code)
                                   .Select(x => x.Mobile)
                                   .SingleOrDefaultAsync();
        }
        if (!string.IsNullOrEmpty(mobileNumber))
        {
            int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking()
                                   .OrderByDescending(x => x.AcademicYear)
                                   .Select(x => x.AcademicYear)
                                   .FirstOrDefaultAsync();
            resultData = await _context.SchoolNotifications.AsNoTracking().ProjectTo<SysSchoolNotificationsDto>(_mapper.ConfigurationProvider)
                                   .OrderByDescending(x => x.Id)
                                   .FirstOrDefaultAsync(e => e.MobileNumber == mobileNumber && e.NotificationType == request.TypeId && e.AcadamicYear == acadamicYear);
        }
        return resultData;
    }
}
#endregion

#region SaveWebIndividualNotification
public class SaveWebIndividualNotification : IRequest<int>
{
    public UserIdentityDto User { get; set; }
    public IndividualNotificationsDto notificationDTO { get; set; }
}

public class SaveWebIndividualNotificationHandler : IRequestHandler<SaveWebIndividualNotification, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public SaveWebIndividualNotificationHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(SaveWebIndividualNotification request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Info("----Info SaveWebIndividualNotification method start----");
            var obj = request.notificationDTO;
            int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking()
                                   .OrderByDescending(x => x.AcademicYear)
                                   .Select(x => x.AcademicYear)
                                   .FirstOrDefaultAsync();
            string mobileNumber = string.Empty;
            if (request.notificationDTO.NotificationType == 1) // teacher
            {
                mobileNumber = await _context.DefSchoolTeacherMaster.AsNoTracking()
                                     .Where(e => e.TeacherCode == request.notificationDTO.Code)
                                     .Select(x => x.PMobile1).SingleOrDefaultAsync();
            }
            else if (request.notificationDTO.NotificationType == 2) // Student
            {
                mobileNumber = await _context.DefSchoolStudentMaster.AsNoTracking()
                                       .Where(e => e.StuAdmNum == request.notificationDTO.Code)
                                       .Select(x => x.Mobile)
                                       .SingleOrDefaultAsync();
            }
            TblSysSchoolNotifications notification = new();
            if (obj.Id > 0)
                notification = await _context.SchoolNotifications.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
            notification.Id = obj.Id;
            notification.AcadamicYear = acadamicYear;
            notification.NotificationType = obj.NotificationType;
            notification.NotificationTitle = obj.NotificationTitle;
            notification.NotificationTitle_Ar = obj.NotificationTitle_Ar;
            notification.NotificationMessage = obj.NotificationMessage;
            notification.NotificationMessage_Ar = obj.NotificationMessage_Ar;
            notification.MobileNumber = mobileNumber;
            notification.CreatedBy = Convert.ToString(request.User.UserId);
            notification.CreatedDate = DateTime.Now;
            notification.IsApproved = false;
            notification.ApprovedDate = DateTime.Now;
            if (obj.Id > 0)
            {
                _context.SchoolNotifications.Update(notification);
            }
            else
            {
                await _context.SchoolNotifications.AddAsync(notification);
            }
            await _context.SaveChangesAsync();
            Log.Info("----Info SaveWebIndividualNotification method Exit----");
            return notification.Id;
        }
        catch (Exception ex)
        {
            Log.Error("Error in SaveWebIndividualNotification Method");
            Log.Error("Error occured time : " + DateTime.UtcNow);
            Log.Error("Error message : " + ex.Message);
            Log.Error("Error StackTrace : " + ex.StackTrace);
            return 0;
        }
    }
}
#endregion

#region CreateNotification
public class CreateNotification : IRequest<int>
{
    public UserIdentityDto User { get; set; }
    public BulkNotificationsDto notificationDTO { get; set; }
}

public class CreateNotificationHandler : IRequestHandler<CreateNotification, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public CreateNotificationHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateNotification request, CancellationToken cancellationToken)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Log.Info("----Info CreateNotification method start----");
                var obj = request.notificationDTO;
                int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking()
                                       .OrderByDescending(x => x.AcademicYear)
                                       .Select(x => x.AcademicYear)
                                       .FirstOrDefaultAsync();
                string mobileNumber = string.Empty;
                TblSysSchoolNotifications notification = new();
                if (obj.Id > 0)
                    notification = await _context.SchoolNotifications.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                notification.Id = obj.Id;
                notification.AcadamicYear = acadamicYear;
                notification.NotificationType = obj.NotificationType;
                notification.NotificationTitle = obj.NotificationTitle;
                notification.NotificationTitle_Ar = obj.NotificationTitle_Ar;
                notification.NotificationMessage = obj.NotificationMessage;
                notification.NotificationMessage_Ar = obj.NotificationMessage_Ar;
                notification.MobileNumber = mobileNumber;
                notification.CreatedBy = Convert.ToString(request.User.UserId);
                notification.CreatedDate = DateTime.Now;
                notification.IsApproved = false;
                notification.ApprovedDate = DateTime.Now;
                if (obj.Id > 0)
                {
                    _context.SchoolNotifications.Update(notification);
                }
                else
                {
                    await _context.SchoolNotifications.AddAsync(notification);
                }
                await _context.SaveChangesAsync();
                var filters = await _context.SchoolNotificationFilters
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(e => e.NotificationId == obj.Id);
                if (filters != null)
                {
                    _context.SchoolNotificationFilters.Remove(filters);
                    await _context.SaveChangesAsync();
                }
                TblSysSchoolNotificationFilters notificationFilters = new();
                notificationFilters.NotificationId = notification.Id;
                notificationFilters.BranchCode = obj.BranchCode;
                notificationFilters.GradeCode = obj.GradeCode;
                notificationFilters.SectionCode = obj.SectionCode;
                notificationFilters.NationalityCode = obj.NationalityCode;
                notificationFilters.PTGroupCode = obj.PTGroupCode;
                notificationFilters.GenderCode = obj.GenderCode;
                notificationFilters.PickUpAndDropZone = obj.PickUpAndDropZone;
                await _context.SchoolNotificationFilters.AddAsync(notificationFilters);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                Log.Info("----Info CreateNotification method Exit----");
                return notification.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error in CreateNotification Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
}
#endregion

#region GetNotificationById
public class GetNotificationById : IRequest<EditBulkNotificationsDto>
{
    public UserIdentityDto User { get; set; }
    public int Id { get; set; }
}

public class GetNotificationByIdHandler : IRequestHandler<GetNotificationById, EditBulkNotificationsDto>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;
    public GetNotificationByIdHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<EditBulkNotificationsDto> Handle(GetNotificationById request, CancellationToken cancellationToken)
    {
        EditBulkNotificationsDto resultData = new EditBulkNotificationsDto();

        int acadamicYear = await _context.SysSchoolAcademicBatches.AsNoTracking()
                               .OrderByDescending(x => x.AcademicYear)
                               .Select(x => x.AcademicYear)
                               .FirstOrDefaultAsync();
        if (acadamicYear > 0)
        {
            resultData = await _context.SchoolNotifications.AsNoTracking()
                                   .Join(_context.SchoolNotificationFilters, sn => sn.Id, snf => snf.NotificationId, (sn, snf) => new
                                   {
                                       sn.Id,
                                       sn.NotificationTitle,
                                       sn.NotificationTitle_Ar,
                                       sn.NotificationMessage,
                                       sn.NotificationMessage_Ar,
                                       sn.IsApproved,
                                       sn.NotificationType,
                                       sn.AcadamicYear,
                                       snf.BranchCode,
                                       snf.GenderCode,
                                       snf.GradeCode,
                                       snf.NationalityCode,
                                       snf.PickUpAndDropZone,
                                       snf.PTGroupCode,
                                       snf.SectionCode
                                   })
                                   .Select(x => new EditBulkNotificationsDto
                                   {
                                       Id = x.Id,
                                       NotificationTitle = x.NotificationTitle,
                                       NotificationTitle_Ar = x.NotificationTitle_Ar,
                                       NotificationMessage = x.NotificationMessage,
                                       NotificationMessage_Ar = x.NotificationMessage_Ar,
                                       NotificationType = x.NotificationType,
                                       BranchCode = x.BranchCode,
                                       GenderCode = x.GenderCode,
                                       GradeCode = x.GradeCode,
                                       NationalityCode = x.NationalityCode,
                                       PickUpAndDropZone = x.PickUpAndDropZone,
                                       PTGroupCode = x.PTGroupCode,
                                       SectionCode = x.SectionCode,
                                       IsApproved = x.IsApproved
                                   }).FirstOrDefaultAsync(x => x.Id == request.Id);
        }
        return resultData;
    }
}
#endregion

#region NotificationApprovalById
public class NotificationApprovalById : IRequest<bool>
{
    public UserIdentityDto User { get; set; }
    public TeacherNotificationsDto Input { get; set; }
}

public class NotificationApprovalByIdHandler : IRequestHandler<NotificationApprovalById, bool>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;
    public NotificationApprovalByIdHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(NotificationApprovalById request, CancellationToken cancellationToken)
    {
        bool result = false;
        var resultData = await _context.SchoolNotifications.AsNoTracking()
                               .SingleOrDefaultAsync(e => e.Id == request.Input.Id);
        if (resultData != null)
        {
            resultData.IsApproved = true;
            resultData.ApprovedDate = DateTime.Now;
            resultData.ApprovedBy = Convert.ToString(request.User.UserId);
            _context.SchoolNotifications.Update(resultData);
            await _context.SaveChangesAsync();

            TblSysSchoolPushNotificationParent notificationParent = new();
            notificationParent.Title = resultData.NotificationTitle;
            notificationParent.Title_Ar = resultData.NotificationTitle_Ar;
            notificationParent.NotifyMessage = resultData.NotificationMessage;
            notificationParent.NotifyMessage_Ar = resultData.NotificationMessage_Ar;
            notificationParent.RegisteredMobile = resultData.MobileNumber;
            notificationParent.MsgNoteId = resultData.Id;
            if (request.Input.NotificationType == 1)
                notificationParent.NotifyTo = "Teacher";
            else
                notificationParent.NotifyTo = "Student";
            notificationParent.IsRead = false;
            notificationParent.NotifyDate = DateTime.Now;
            await _context.PushNotificationParent.AddAsync(notificationParent);
            await _context.SaveChangesAsync();

            result = true;
        }
        return result;
    }
}
#endregion

#region BulkWebNotificationApproval
public class BulkWebNotificationApproval : IRequest<int>
{
    public UserIdentityDto User { get; set; }
    public int Id { get; set; }
}

public class BulkWebNotificationApprovalHandler : IRequestHandler<BulkWebNotificationApproval, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public BulkWebNotificationApprovalHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(BulkWebNotificationApproval request, CancellationToken cancellationToken)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Log.Info("----Info BulkWebNotificationApproval method start----");
                var notificationData = await _context.SchoolNotifications.SingleOrDefaultAsync(e => e.Id == request.Id);
                if (notificationData != null)
                {
                    notificationData.ApprovedBy = Convert.ToString(request.User.UserId);
                    notificationData.IsApproved = true;
                    _context.SchoolNotifications.Update(notificationData);
                    await _context.SaveChangesAsync();
                    var notificationFilterData = await _context.SchoolNotificationFilters
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(e => e.NotificationId == request.Id);
                    if (notificationFilterData != null)
                    {
                        var studentList = await _context.DefSchoolStudentMaster
                                                   .AsNoTracking()
                                                   .ToListAsync();
                        if (studentList != null && studentList.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(notificationFilterData.BranchCode))
                            {
                                studentList = studentList.Where(x => x.BranchCode == notificationFilterData.BranchCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.GradeCode))
                            {
                                studentList = studentList.Where(x => x.GradeCode == notificationFilterData.GradeCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.NationalityCode))
                            {
                                studentList = studentList.Where(x => x.NatCode == notificationFilterData.NationalityCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.SectionCode))
                            {
                                studentList = studentList.Where(x => x.GradeSectionCode == notificationFilterData.SectionCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.PTGroupCode))
                            {
                                studentList = studentList.Where(x => x.PTGroupCode == notificationFilterData.PTGroupCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.GenderCode))
                            {
                                studentList = studentList.Where(x => x.GenderCode == notificationFilterData.GenderCode)
                                                  .ToList();
                            }
                            if (!string.IsNullOrEmpty(notificationFilterData.PickUpAndDropZone))
                            {
                                studentList = studentList.Where(x => x.PickNDropZone == notificationFilterData.PickUpAndDropZone)
                                                  .ToList();
                            }
                            if (studentList != null && studentList.Count > 0)
                            {
                                List<TblSysSchoolPushNotificationParent> lstNotifications = new();
                                foreach (var item in studentList)
                                {
                                    TblSysSchoolPushNotificationParent notificationParent = new();
                                    notificationParent.Title = notificationData.NotificationTitle;
                                    notificationParent.Title_Ar = notificationData.NotificationTitle_Ar;
                                    notificationParent.NotifyMessage = notificationData.NotificationMessage;
                                    notificationParent.NotifyMessage_Ar = notificationData.NotificationMessage_Ar;
                                    notificationParent.RegisteredMobile = item.Mobile;
                                    notificationParent.MsgNoteId = notificationData.Id;
                                    notificationParent.NotifyTo = "Student";
                                    notificationParent.IsRead = false;
                                    notificationParent.NotifyDate = DateTime.Now;
                                    lstNotifications.Add(notificationParent);
                                }
                                await _context.PushNotificationParent.AddRangeAsync(lstNotifications);
                                await _context.SaveChangesAsync();
                                await transaction.CommitAsync();
                            }
                        }
                    }
                    Log.Info("----Info BulkWebNotificationApproval method Exit----");
                    return notificationData.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error in BulkWebNotificationApproval Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
}

#endregion

#region GetWebTopNotifications
public class GetWebTopNotifications : IRequest<List<SchoolNotificationDto>>
{
    public UserIdentityDto User { get; set; }
}

public class GetWebTopNotificationsHandler : IRequestHandler<GetWebTopNotifications, List<SchoolNotificationDto>>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;
    public GetWebTopNotificationsHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<SchoolNotificationDto>> Handle(GetWebTopNotifications request, CancellationToken cancellationToken)
    {
        List<SchoolNotificationDto> schoolNotificationDtos = new();
        var startDate = await _context.SysSchoolAcademicBatches.AsNoTracking()
                               .OrderByDescending(x => x.AcademicYear)
                               .Select(x => x.AcademicStartDate)
                               .FirstOrDefaultAsync();
        var userData = await _context.SystemLogins.AsNoTracking()
                               .Where(x => x.Id == request.User.UserId)
                               .FirstOrDefaultAsync();
        if (userData != null)
        {
            string mobileNumber = null;
            var teacherData = await _context.DefSchoolTeacherMaster.AsNoTracking()
                                           .Where(x => x.SysLoginId == request.User.UserId)
                                           .FirstOrDefaultAsync();

            if (teacherData != null)
            {
                mobileNumber = teacherData.PMobile1;
                schoolNotificationDtos = await _context.PushNotificationParent.AsNoTracking().
                                           Where(x => x.NotifyDate >= startDate && (x.UserId == userData.LoginId
                                               || x.RegisteredMobile == teacherData.PMobile1
                                               || x.RegisteredMobile == teacherData.SMobile2) && x.NotifyTo.ToLower()== "teacher")
                                           .OrderByDescending(x => x.Id)
                                           .Select(x => new SchoolNotificationDto()
                                           {
                                               Id = x.Id,
                                               FromName = x.FromName,
                                               FromUserId = x.FromUserId,
                                               IsRead = x.IsRead,
                                               MsgNoteId = x.MsgNoteId,
                                               NotifyDate = x.NotifyDate,
                                               NotifyMessage = x.NotifyMessage,
                                               NotifyMessage_Ar = x.NotifyMessage_Ar,
                                               NotifyTo = x.NotifyTo,
                                               ReadDateTime = x.ReadDateTime,
                                               RegisteredMobile = x.RegisteredMobile,
                                               Title = x.Title,
                                               Title_Ar = x.Title_Ar,
                                               UserID = x.UserId
                                           }).ToListAsync();
            }
            else
            {
                schoolNotificationDtos = await _context.PushNotificationParent.AsNoTracking().
                                           Where(x => x.NotifyDate >= startDate && x.NotifyTo.ToLower() == "admin")
                                           .OrderByDescending(x => x.Id)
                                           .Select(x => new SchoolNotificationDto()
                                           {
                                               Id = x.Id,
                                               FromName = x.FromName,
                                               FromUserId = x.FromUserId,
                                               IsRead = x.IsRead,
                                               MsgNoteId = x.MsgNoteId,
                                               NotifyDate = x.NotifyDate,
                                               NotifyMessage = x.NotifyMessage,
                                               NotifyMessage_Ar = x.NotifyMessage_Ar,
                                               NotifyTo = x.NotifyTo,
                                               ReadDateTime = x.ReadDateTime,
                                               RegisteredMobile = x.RegisteredMobile,
                                               Title = x.Title,
                                               Title_Ar = x.Title_Ar,
                                               UserID = x.UserId
                                           }).ToListAsync();
            }

        }
        return schoolNotificationDtos;
    }
}
#endregion


#region Send Staff & Teacher Notification

public class SendParentNotification : IRequest<int>
{
    public UserIdentityDto User { get; set; }
    public SendNotificationDto sendNotificationDTO { get; set; }
}

public class SendNotificationHandler : IRequestHandler<SendParentNotification, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public SendNotificationHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(SendParentNotification request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Info("----Info SendParentNotification method start----");
            var obj = request.sendNotificationDTO;

            string mobileNumber = string.Empty;
            string notifyTo = string.Empty;
            if (request.sendNotificationDTO.Contact == "Teacher")
            {
                var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.PrimaryBranchCode == request.sendNotificationDTO.Branch).FirstOrDefaultAsync();


                var teacherCode = await _context.DefSchoolTeacherClassMapping.AsNoTracking()
                                  .Where(e => e.GradeCode == request.sendNotificationDTO.Grade)
                                 .Select(x => x.TeacherCode).FirstOrDefaultAsync();


                if (teacherCode != null)
                {
                    mobileNumber = await _context.DefSchoolTeacherMaster.AsNoTracking()
                                     .Where(e => (e.PrimaryBranchCode == request.sendNotificationDTO.Branch && e.TeacherCode == teacherCode))
                                     .Select(x => x.PMobile1).SingleOrDefaultAsync();
                    notifyTo = "Teacher";
                }

            }
            else if (request.sendNotificationDTO.Contact == "Admin" || request.sendNotificationDTO.Contact == "Pickup")
            {
                mobileNumber = await _context.SchoolBranches.AsNoTracking()
                                       .Where(e => e.BranchCode == request.sendNotificationDTO.Branch)
                                       .Select(x => x.Mobile)
                                       .SingleOrDefaultAsync();
                var studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.StuAdmNum == obj.StudentAdmNum);
                obj.NotifyMessage = obj.NotifyMessage.Replace("<Name of Student>", studentDetails.StuName);
                obj.NotifyMessage_Ar = obj.NotifyMessage_Ar.Replace("<Name of Student>", studentDetails.StuName2);
                obj.NotifyMessage = obj.NotifyMessage.Replace("<Grade>", studentDetails.GradeCode);
                obj.NotifyMessage_Ar = obj.NotifyMessage_Ar.Replace("<Grade>", studentDetails.GradeCode);
                notifyTo = "Admin";
            }
            TblSysSchoolPushNotificationParent notification = new();
            if (obj.Id > 0)
                notification = await _context.PushNotificationParent.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
            notification.Id = obj.Id;
            notification.NotifyDate = DateTime.Now;
            notification.NotifyMessage = obj.NotifyMessage;
            notification.Title = obj.Title;
            notification.Title_Ar = obj.Title_Ar;
            notification.NotifyMessage = obj.NotifyMessage;
            notification.NotifyMessage_Ar = obj.NotifyMessage_Ar;
            notification.RegisteredMobile = mobileNumber;
            notification.IsRead = false;
            notification.NotifyTo = notifyTo;
            if (obj.Id > 0)
            {
                _context.PushNotificationParent.Update(notification);
            }
            else
            {
                await _context.PushNotificationParent.AddAsync(notification);
            }
            await _context.SaveChangesAsync();
            Log.Info("----Info Send Parent Notification method Exit----");
            return notification.Id;
        }
        catch (Exception ex)
        {
            Log.Error("Error in Send Parent Notification Method");
            Log.Error("Error occured time : " + DateTime.UtcNow);
            Log.Error("Error message : " + ex.Message);
            Log.Error("Error StackTrace : " + ex.StackTrace);
            return 0;
        }
    }
}

#endregion

#region Get Notification Template
public class GetPickupNotificationTemplate : IRequest<PickupNotificationTemplateDto>
{
    public UserIdentityDto User { get; set; }
    public string TemplateType { get; set; }

}


public class PickupNotificationTemplateHandler : IRequestHandler<GetPickupNotificationTemplate, PickupNotificationTemplateDto>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public PickupNotificationTemplateHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PickupNotificationTemplateDto> Handle(GetPickupNotificationTemplate request, CancellationToken cancellationToken)
    {
        PickupNotificationTemplateDto notificationTemplate = new PickupNotificationTemplateDto();
        notificationTemplate = await _context.NotificaticationTemplates.AsNoTracking()
            .ProjectTo<PickupNotificationTemplateDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Type == "Pickup");

        return notificationTemplate;
    }

}

#endregion

#region SaveStudentTemplateNoticefication
public class SaveStudentTemplateNoticefication : IRequest<int>
{
    public UserIdentityDto User { get; set; }
    public string StuAdmNum { get; set; }
    public int TypeID { get; set; }
}

public class SaveStudentTemplateNoticeficationHandler : IRequestHandler<SaveStudentTemplateNoticefication, int>
{
    private readonly CINDBOneContext _context;
    private readonly IMapper _mapper;

    public SaveStudentTemplateNoticeficationHandler(CINDBOneContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(SaveStudentTemplateNoticefication request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Info("----Info SaveStudentTemplateNoticefication method start----");
            string templateEn = string.Empty;
            string templateAr = string.Empty;
            if (request.TypeID == 1)
            {
                var notificationTemplate = await _context.NotificaticationTemplates.AsNoTracking()
                                              .FirstOrDefaultAsync(x => x.Type == "Student Arrival at the School Gate");
                var studentDetails = await _context.DefSchoolStudentMaster.AsNoTracking()
                                              .FirstOrDefaultAsync(x => x.StuAdmNum == request.StuAdmNum);
                var currentDate = DateTime.Now;
                
                if (notificationTemplate != null && studentDetails != null)
                {
                    var existNotification = await _context.PushNotificationParent.AsNoTracking()
                                              .FirstOrDefaultAsync(k => (k.RegisteredMobile == studentDetails.Mobile || k.RegisteredMobile==studentDetails.RegisteredPhone) && k.NotifyTo == "Student"
                                                && k.NotifyMessage.Contains("arrived in the school Premises at") && k.NotifyDate.Value.Date== currentDate.Date);
                    if (existNotification==null)
                    {
                        templateEn = notificationTemplate.Template_En;
                        templateAr = notificationTemplate.Template_Ar;
                        templateEn = templateEn.Replace("<Name of the Parent>", studentDetails.FatherName);
                        templateAr = templateAr.Replace("<Name of the Parent>", studentDetails.FatherName);
                        templateEn = templateEn.Replace("<Name of the Student>", studentDetails.StuName);
                        templateAr = templateAr.Replace("<Name of the Student>", studentDetails.StuName2);
                        templateEn = templateEn.Replace("<time>", currentDate.TimeOfDay.ToString());
                        templateAr = templateAr.Replace("<time>", currentDate.TimeOfDay.ToString());
                        templateEn = templateEn.Replace("<date>", currentDate.Date.ToString());
                        templateAr = templateAr.Replace("<date>", currentDate.Date.ToString());
                        templateEn = templateEn.Replace("<name of School>", "HVS");
                        templateAr = templateAr.Replace("<name of School>", "HVS");
                        TblSysSchoolPushNotificationParent notification = new();
                        notification.MsgNoteId = 0;
                        notification.NotifyDate = currentDate;
                        notification.NotifyMessage = templateEn;
                        notification.NotifyMessage_Ar = templateAr;
                        notification.IsRead = false;
                        notification.Title = string.Empty;
                        notification.Title_Ar = string.Empty;
                        notification.NotifyTo = "Student";
                        notification.FromUserId = Convert.ToString(request.User.UserId);
                        notification.RegisteredMobile = studentDetails.Mobile ?? studentDetails.RegisteredPhone;
                        await _context.PushNotificationParent.AddAsync(notification);
                        await _context.SaveChangesAsync();
                        Log.Info("----Info SaveStudentTemplateNoticefication method Exit----");
                        return notification.Id;
                    }
                    else
                    {
                        Log.Info("----Info SaveStudentTemplateNoticefication method Exit----");
                        return existNotification.Id;
                    }
                }
            }
            
            return 0;
        }
        catch (Exception ex)
        {
            Log.Error("Error in SaveStudentTemplateNoticefication Method");
            Log.Error("Error occured time : " + DateTime.UtcNow);
            Log.Error("Error message : " + ex.Message);
            Log.Error("Error StackTrace : " + ex.StackTrace);
            return 0;
        }
    }
}
#endregion

