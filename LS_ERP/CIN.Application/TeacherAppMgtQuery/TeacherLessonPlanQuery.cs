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
    #region CreateLessonPlanInfo
    public class CreateTeacherLessonPlanInfo : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TeacherLessonPlanInfoDto LessonPlanInfoDto { get; set; }
    }

    public class CreateLessonPlanInfoHandler : IRequestHandler<CreateTeacherLessonPlanInfo, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateLessonPlanInfoHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateTeacherLessonPlanInfo request, CancellationToken cancellationToken)
        {
            Log.Info("----Info Create Lesson Plan Info method start----");
            TblLessonPlanHeader lessonPlanHeader = new();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.LessonPlanInfoDto;
                    if (obj.Id > 0)
                        lessonPlanHeader = await _context.LessonPlanHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    lessonPlanHeader.Id = obj.Id;
                    if (obj.Id == 0)
                        lessonPlanHeader.LessonPlanCode = "LPC" + Convert.ToString(new Random().Next(999, 99999)) + Convert.ToString(new Random().Next(999, 99999));
                    lessonPlanHeader.BranchCode = obj.BranchCode;
                    lessonPlanHeader.GradeCode = obj.GradeCode;
                    lessonPlanHeader.SectionCode = obj.SectionCode;
                    lessonPlanHeader.SubCodes = obj.SubCodes;
                    lessonPlanHeader.EstEndDate = obj.EstEndDate;
                    lessonPlanHeader.EstStartDate = obj.EstStartDate;
                    lessonPlanHeader.NumOfDays = obj.NumOfDays;
                    lessonPlanHeader.NumOfLessons = obj.NumOfLessons;
                    lessonPlanHeader.TeacherCode = obj.TeacherCode;
                    if (obj.Id > 0)
                    {
                        _context.LessonPlanHeader.Update(lessonPlanHeader);
                    }
                    else
                    {
                        await _context.LessonPlanHeader.AddAsync(lessonPlanHeader);
                    }
                    await _context.SaveChangesAsync();
                    if (lessonPlanHeader.Id > 0)
                    {
                        var details = await _context.LessonPlanDetails.AsNoTracking()
                                       .Where(e => e.LessonPlanCode == lessonPlanHeader.LessonPlanCode)
                                       .ToListAsync();
                        if (details != null && details.Count() > 0)
                        {
                            _context.LessonPlanDetails.RemoveRange(details);
                            await _context.SaveChangesAsync();
                        }
                        foreach (var item in obj.LessonPlanDetailsRows)
                        {
                            TblLessonPlanDetails tblLessonPlanDetails = new();
                            tblLessonPlanDetails.LessonPlanCode = lessonPlanHeader.LessonPlanCode;
                            tblLessonPlanDetails.GradeCode = obj.GradeCode;
                            tblLessonPlanDetails.SectionCode = obj.SectionCode;
                            tblLessonPlanDetails.SubCodes = obj.SubCodes;
                            tblLessonPlanDetails.Chapter = item.Chapter;
                            tblLessonPlanDetails.LessonName = item.LessonName;
                            tblLessonPlanDetails.LessonName2 = item.LessonName2;
                            tblLessonPlanDetails.NumOfSessions = item.NumOfSessions;
                            tblLessonPlanDetails.EstStartDate = item.TopicDate;
                            tblLessonPlanDetails.EstEndDate = item.TopicDate;
                            tblLessonPlanDetails.StartTime = TimeSpan.Parse(item.StartTime);
                            tblLessonPlanDetails.EndTime = TimeSpan.Parse(item.EndTime);
                            tblLessonPlanDetails.Topics = item.Topics;
                            tblLessonPlanDetails.Topics2 = item.Topics2;
                            tblLessonPlanDetails.ActualTecherCode = obj.TeacherCode;
                            await _context.LessonPlanDetails.AddAsync(tblLessonPlanDetails);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info Create Lesson Plan  method Exit----");
                    return lessonPlanHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Lesson Plan  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }
    #endregion

    #region GetLessonPlanList

    public class GetLessonPlanList : IRequest<List<TeacherLessonPlanInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetLessonPlanListHandler : IRequestHandler<GetLessonPlanList, List<TeacherLessonPlanInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetLessonPlanListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<TeacherLessonPlanInfoDto>> Handle(GetLessonPlanList request, CancellationToken cancellationToken)
        {
            //var lessonPlanHeaderlist = await _context.LessonPlanHeader.AsNoTracking()
            //                             .ProjectTo<TblLessonPlanHeaderDto>(_mapper.ConfigurationProvider)
            //                             .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            //return lessonPlanHeaderlist;
            List<TeacherLessonPlanInfoDto> lessonPlanList = new();
            var acadamicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                    ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                    OrderByDescending(x => x.AcademicYear).
                                                    FirstOrDefaultAsync();

            if (acadamicDetails != null)
            {
                lessonPlanList = await _context.LessonPlanHeader.AsNoTracking()
                                        .Where(e => e.BranchCode == request.BranchCode
                                              && e.TeacherCode == request.TeacherCode && e.GradeCode == request.GradeCode)
                                        .Select(x => new TeacherLessonPlanInfoDto()
                                        {
                                            Id = x.Id,
                                            LessonPlanCode = x.LessonPlanCode,
                                            GradeCode = x.GradeCode,
                                            BranchCode = x.BranchCode,
                                            SubCodes = x.SubCodes,
                                            EstEndDate = x.EstEndDate,
                                            EstStartDate = x.EstStartDate,
                                            NumOfLessons = x.NumOfLessons,
                                            NumOfDays = x.NumOfDays,
                                            SectionCode = x.SectionCode,
                                            TeacherCode = x.TeacherCode

                                        })
                                        .ToListAsync();
            }
            foreach (var item in lessonPlanList)
            {
                var lessonPlanDtails = await _context.LessonPlanDetails.AsNoTracking().ProjectTo<LessonPlanDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.LessonPlanCode == item.LessonPlanCode).ToListAsync();
                item.LessonPlanDetailsRows = lessonPlanDtails;

            }
            return lessonPlanList;
        }
    }

    #endregion

    #region GetLessonPlanInfoById
    public class GetLessonPlanInfoById : IRequest<TeacherLessonPlanInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetLessonPlanInfoByIdHandler : IRequestHandler<GetLessonPlanInfoById, TeacherLessonPlanInfoDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;
        public GetLessonPlanInfoByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TeacherLessonPlanInfoDto> Handle(GetLessonPlanInfoById request, CancellationToken cancellationToken)
        {
            TeacherLessonPlanInfoDto result = new();
            result = await _context.LessonPlanHeader.AsNoTracking()
                            .Where(e => e.Id == request.Id)
                            .Select(x => new TeacherLessonPlanInfoDto()
                            {
                                Id = x.Id,
                                BranchCode = x.BranchCode,
                                EstEndDate = Convert.ToDateTime(x.EstEndDate),
                                EstStartDate = Convert.ToDateTime(x.EstStartDate),
                                GradeCode = x.GradeCode,
                                LessonPlanCode = x.LessonPlanCode,
                                NumOfDays = x.NumOfDays,
                                NumOfLessons = x.NumOfLessons,
                                SectionCode = x.SectionCode,
                                SubCodes = x.SubCodes,
                                TeacherCode = x.TeacherCode
                            })
                            .FirstOrDefaultAsync();
            if (result != null)
            {
                var details = await _context.LessonPlanDetails.AsNoTracking()
                               .Where(x => x.LessonPlanCode == result.LessonPlanCode)
                               .Select(x => new LessonPlanDetailsDto()
                               {
                                   LessonPlanCode = x.LessonPlanCode,
                                   Chapter = x.Chapter,
                                   EndTime = Convert.ToString(x.EndTime),
                                   LessonName = x.LessonName,
                                   LessonName2 = x.LessonName2,
                                   NumOfSessions = x.NumOfSessions,
                                   StartTime = Convert.ToString(x.StartTime),
                                   TopicDate = Convert.ToDateTime(x.EstStartDate),
                                   Topics = x.Topics,
                                   Topics2 = x.Topics2

                               })
                               .ToListAsync();
                result.LessonPlanDetailsRows = details;
            }
            else
                result = new();
            return result;
        }
    }

    #endregion

    #region GetLessonPlanDetailsById

    public class GetLessonPlanDetailsById : IRequest<LessonPlanDetailsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetLessonPlanDetailsByIdHandler : IRequestHandler<GetLessonPlanDetailsById, LessonPlanDetailsDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;
        public GetLessonPlanDetailsByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<LessonPlanDetailsDto> Handle(GetLessonPlanDetailsById request, CancellationToken cancellationToken)
        {
            LessonPlanDetailsDto result = new();
            if (request.Id > 0)
            {
                result = await _context.LessonPlanDetails.AsNoTracking()
                                .Where(e => e.Id == request.Id)
                                .Select(x => new LessonPlanDetailsDto()
                                {
                                    Id = x.Id,
                                    LessonPlanCode = x.LessonPlanCode,
                                    Chapter = x.Chapter,
                                    EndTime = Convert.ToString(x.EndTime),
                                    LessonName = x.LessonName,
                                    LessonName2 = x.LessonName2,
                                    NumOfSessions = x.NumOfSessions,
                                    StartTime = Convert.ToString(x.StartTime),
                                    TopicDate = Convert.ToDateTime(x.EstStartDate),
                                    Topics = x.Topics,
                                    Topics2 = x.Topics2
                                })
                                .FirstOrDefaultAsync();

            }
            return result;
        }
    }

    #endregion

    #region Delete LessonPlanDetailById 
    public class DeleteLessonPlanDetailById : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLessonPlanDetailByIdHandler : IRequestHandler<DeleteLessonPlanDetailById, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLessonPlanDetailByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLessonPlanDetailById request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteLessonPlanDetailById start----");

                if (request.Id > 0)
                {
                    var lessonPlanDetail = await _context.LessonPlanDetails.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(lessonPlanDetail);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in  DeleteLessonPlanDetailById");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }


        }
    }

    #endregion

    #region Delete LessonPlanDetail_List
    public class DeleteLessonPlanList : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLessonPlanListHandler : IRequestHandler<DeleteLessonPlanList, int>
    {
        
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLessonPlanListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLessonPlanList request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {

                    if (request.Id > 0)
                    {
                        var lPHeader = await _context.LessonPlanHeader.FirstOrDefaultAsync(e => e.Id == request.Id);

                        var LPDetails = await _context.LessonPlanDetails.Where(d => d.LessonPlanCode == lPHeader.LessonPlanCode).ToListAsync();
                        _context.LessonPlanDetails.RemoveRange(LPDetails);
                        await _context.SaveChangesAsync();
                      //  await transaction.CommitAsync();


                        var LPHeader = _context.LessonPlanHeader.Where(d => d.LessonPlanCode == lPHeader.LessonPlanCode).First();
                        _context.LessonPlanHeader.Remove(LPHeader);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in delete Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }


            }
        }
    }

    #endregion



}