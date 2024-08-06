using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FleetMgtDtos;
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
using CIN.Domain.FleetMgt;

namespace CIN.Application.FleetMgtQuery
{
    //#region CreateLessonPlanInfo
    //public class CreateTeacherLessonPlanInfo : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public TeacherLessonPlanInfoDto LessonPlanInfoDto { get; set; }
    //}

    //public class CreateLessonPlanInfoHandler : IRequestHandler<CreateTeacherLessonPlanInfo, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateLessonPlanInfoHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateTeacherLessonPlanInfo request, CancellationToken cancellationToken)
    //    {
    //        Log.Info("----Info Create Lesson Plan Info method start----");
    //        TblLessonPlanHeader lessonPlanHeader = new();
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                var obj = request.LessonPlanInfoDto;
    //                if (obj.Id > 0)
    //                    lessonPlanHeader = await _context.LessonPlanHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
    //                lessonPlanHeader.Id = obj.Id;
    //                if (obj.Id == 0)
    //                    lessonPlanHeader.LessonPlanCode = "LPC" + Convert.ToString(new Random().Next(999, 99999)) + Convert.ToString(new Random().Next(999, 99999));
    //                lessonPlanHeader.BranchCode = obj.BranchCode;
    //                lessonPlanHeader.GradeCode = obj.GradeCode;
    //                lessonPlanHeader.SectionCode = obj.SectionCode;
    //                lessonPlanHeader.SubCodes = obj.SubCodes;
    //                lessonPlanHeader.EstEndDate = obj.EstEndDate;
    //                lessonPlanHeader.EstStartDate = obj.EstStartDate;
    //                lessonPlanHeader.NumOfDays = obj.NumOfDays;
    //                lessonPlanHeader.NumOfLessons = obj.NumOfLessons;
    //                lessonPlanHeader.TeacherCode = obj.TeacherCode;
    //                if (obj.Id > 0)
    //                {
    //                    _context.LessonPlanHeader.Update(lessonPlanHeader);
    //                }
    //                else
    //                {
    //                    await _context.LessonPlanHeader.AddAsync(lessonPlanHeader);
    //                }
    //                await _context.SaveChangesAsync();
    //                if (lessonPlanHeader.Id > 0)
    //                {
    //                    var details = await _context.LessonPlanDetails.AsNoTracking()
    //                                   .Where(e => e.LessonPlanCode == lessonPlanHeader.LessonPlanCode)
    //                                   .ToListAsync();
    //                    if (details != null && details.Count() > 0)
    //                    {
    //                        _context.LessonPlanDetails.RemoveRange(details);
    //                        await _context.SaveChangesAsync();
    //                    }
    //                    foreach (var item in obj.LessonPlanDetailsRows)
    //                    {
    //                        TblLessonPlanDetails tblLessonPlanDetails = new();
    //                        tblLessonPlanDetails.LessonPlanCode = lessonPlanHeader.LessonPlanCode;
    //                        tblLessonPlanDetails.GradeCode = obj.GradeCode;
    //                        tblLessonPlanDetails.SectionCode = obj.SectionCode;
    //                        tblLessonPlanDetails.SubCodes = obj.SubCodes;
    //                        tblLessonPlanDetails.Chapter = item.Chapter;
    //                        tblLessonPlanDetails.LessonName = item.LessonName;
    //                        tblLessonPlanDetails.LessonName2 = item.LessonName2;
    //                        tblLessonPlanDetails.NumOfSessions = item.NumOfSessions;
    //                        tblLessonPlanDetails.EstStartDate = item.TopicDate;
    //                        tblLessonPlanDetails.EstEndDate = item.TopicDate;
    //                        tblLessonPlanDetails.StartTime = TimeSpan.Parse(item.StartTime);
    //                        tblLessonPlanDetails.EndTime = TimeSpan.Parse(item.EndTime);
    //                        tblLessonPlanDetails.Topics = item.Topics;
    //                        tblLessonPlanDetails.Topics2 = item.Topics2;
    //                        tblLessonPlanDetails.ActualTecherCode = obj.TeacherCode;
    //                        await _context.LessonPlanDetails.AddAsync(tblLessonPlanDetails);
    //                        await _context.SaveChangesAsync();
    //                    }
    //                }
    //                await transaction.CommitAsync();
    //                Log.Info("----Info Create Lesson Plan  method Exit----");
    //                return lessonPlanHeader.Id;
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in Create Lesson Plan  Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return 0;
    //            }
    //        }
    //    }

    //}
    //#endregion

    #region GetAll
    public class GetRoutePlanList : IRequest<PaginatedList<RoutePlanHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetRoutePlanListHandler : IRequestHandler<GetRoutePlanList, PaginatedList<RoutePlanHeaderDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetRoutePlanListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<PaginatedList<RoutePlanHeaderDto>> Handle(GetRoutePlanList request, CancellationToken cancellationToken)
        {
            var list = await _context.RoutePlanHeader.AsNoTracking().ProjectTo<RoutePlanHeaderDto>(_mapper.ConfigurationProvider)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;
        }
    }
    #endregion

    #region CreateRoutePlan

    public class CreateRoutePlanInfo : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public RoutePlanHeaderDto RoutePlanInfoDto { get; set; }
    }

    public class CreateRoutePlanInfoHandler : IRequestHandler<CreateRoutePlanInfo, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateRoutePlanInfoHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateRoutePlanInfo request, CancellationToken cancellationToken)
        {
            Log.Info("----Info Create Route Plan Details Info method start----");
            TblRoutePlanHeader routePlanHeader = new();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.RoutePlanInfoDto;
                    if (obj.Id > 0)
                    routePlanHeader = await _context.RoutePlanHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    routePlanHeader.Id = obj.Id;
                    if (obj.Id == 0)
                    routePlanHeader.RoutePlanCode = obj.RoutePlanCode;
                    routePlanHeader.RouteNameEn = obj.RouteNameEn;
                    routePlanHeader.RouteNameAr = obj.RouteNameAr;
                    routePlanHeader.IsActive = obj.IsActive;
                    routePlanHeader.CreatedOn =DateTime.Now;
                 
                    if (obj.Id > 0)
                    {
                        _context.RoutePlanHeader.Update(routePlanHeader);
                    }
                    else
                    {
                        await _context.RoutePlanHeader.AddAsync(routePlanHeader);
                    }
                    await _context.SaveChangesAsync();
                    if (routePlanHeader.Id > 0)
                    {
                        var details = await _context.RoutePlanDetails.AsNoTracking()
                                       .Where(e => e.RouteCode == routePlanHeader.RoutePlanCode)
                                       .ToListAsync();
                        //var oldFeeDetailList = await _context.RoutePlanDetails.Where(e => e.RouteCode == routePlanHeader.RoutePlanCode).ToListAsync();
                        //_context.RoutePlanDetails.RemoveRange(oldFeeDetailList);

                      //  List<TblRoutePlanDetails> feeDetailList = new();

                        if (details != null && details.Count() > 0)
                        {
                            _context.RoutePlanDetails.RemoveRange(details);
                            await _context.SaveChangesAsync();
                        }
                        foreach (var item in obj.RoutePlanDetailsRows)
                        {
                            TblRoutePlanDetails tblRoutePlanDetails = new();
                            tblRoutePlanDetails.RoutePlanId = routePlanHeader.Id;
                            tblRoutePlanDetails.RouteCode = item.RouteCode;
                            tblRoutePlanDetails.Flag = item.Flag;
                            tblRoutePlanDetails.IsActive = true;
                            tblRoutePlanDetails.CreatedOn = DateTime.Now;
                           
                             _context.RoutePlanDetails.Update(tblRoutePlanDetails);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info Create Route Plan Details  method Exit----");
                    return routePlanHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Create Route Plan Details  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }



    #endregion

    #region GetRoutePlanInfoById
    public class GetRoutePlanInfoById : IRequest<RoutePlanHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetRoutePlanInfoByIdHandler : IRequestHandler<GetRoutePlanInfoById, RoutePlanHeaderDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;
        public GetRoutePlanInfoByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RoutePlanHeaderDto> Handle(GetRoutePlanInfoById request, CancellationToken cancellationToken)
        {
            RoutePlanHeaderDto result = new();
            result = await _context.RoutePlanHeader.AsNoTracking()
                            .Where(e => e.Id == request.Id)
                            .Select(x => new RoutePlanHeaderDto()
                            {
                                Id = x.Id,
                                RoutePlanCode = x.RoutePlanCode,
                                RouteNameEn = x.RouteNameEn,
                                RouteNameAr = x.RouteNameAr,
                                IsActive=x.IsActive
                            })
                            .FirstOrDefaultAsync();
            if (result != null)
            {
                var details = await _context.RoutePlanDetails.AsNoTracking()
                               .Where(x => x.RoutePlanId == result.Id)
                               .Select(x => new RoutePlanDetailsDto()
                               {
                                   RouteId=x.RoutePlanId,
                                   RouteCode = x.RouteCode,
                                   Flag=x.Flag

                               })
                               .ToListAsync();
                result.RoutePlanDetailsRows = details;
            }
            else
                result = new();
            return result;
        }
    }

    #endregion


    #region GetRoutePlanByRouteCode
 
    public class GetRoutePlanByRouteCode : IRequest<RoutePlanHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public string RouteCode { get; set; }

    }

    public class GetRoutePlanByRouteCodeHandler : IRequestHandler<GetRoutePlanByRouteCode, RoutePlanHeaderDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;
        public GetRoutePlanByRouteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RoutePlanHeaderDto> Handle(GetRoutePlanByRouteCode request, CancellationToken cancellationToken)
        {
            RoutePlanHeaderDto result = new();
            result = await _context.RoutePlanHeader.AsNoTracking()
                            .Where(e => e.RoutePlanCode == request.RouteCode)
                            .Select(x => new RoutePlanHeaderDto()
                            {
                                Id = x.Id,
                                RoutePlanCode = x.RoutePlanCode,
                                RouteNameEn = x.RouteNameEn,
                                RouteNameAr = x.RouteNameAr,
                                IsActive = x.IsActive
                            })
                            .FirstOrDefaultAsync();
            if (result != null)
            {
                var details = await _context.RoutePlanDetails.AsNoTracking()
                               .Where(x => x.RouteCode ==request.RouteCode)
                               .Select(x => new RoutePlanDetailsDto()
                               {
                                   RouteId = x.RoutePlanId,
                                   RouteCode = x.RouteCode,
                                   Flag = x.Flag

                               })
                               .ToListAsync();
                result.RoutePlanDetailsRows = details;
            }
            else
                result = new();
            return result;
        }
    }


    #endregion

    #region Test
    //#region Create_And_Update

    //public class CreateUpdateSchoolFeeStructure : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public TblSysSchoolFeeStructureDto SchoolFeeStructureDto { get; set; }
    //}

    //public class CreateUpdateSchoolFeeStructureHandler : IRequestHandler<CreateUpdateSchoolFeeStructure, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateUpdateSchoolFeeStructureHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateUpdateSchoolFeeStructure request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info Create Update SchoolFeeStructure method start----");

    //                var obj = request.SchoolFeeStructureDto;


    //                TblSysSchoolFeeStructureHeader SchoolFeeStructHead = new();
    //                if (obj.Id > 0)
    //                    SchoolFeeStructHead = await _context.SchoolFeeStructureHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


    //                SchoolFeeStructHead.Id = obj.Id;
    //                SchoolFeeStructHead.GradeCode = obj.GradeCode;
    //                SchoolFeeStructHead.BranchCode = obj.BranchCode;
    //                SchoolFeeStructHead.FeeStructName = obj.FeeStructName;
    //                SchoolFeeStructHead.FeeStructName2 = obj.FeeStructName2;
    //                SchoolFeeStructHead.ApplyLateFee = obj.ApplyLateFee;
    //                SchoolFeeStructHead.LateFeeCode = obj.LateFeeCode;
    //                SchoolFeeStructHead.ActualFeePayable = obj.ActualFeePayable;
    //                SchoolFeeStructHead.Remarks = obj.Remarks;
    //                SchoolFeeStructHead.IsActive = obj.IsActive;
    //                SchoolFeeStructHead.CreatedOn = DateTime.Now;
    //                SchoolFeeStructHead.CreatedBy = obj.CreatedBy;

    //                if (obj.Id > 0)
    //                {

    //                    _context.SchoolFeeStructureHeader.Update(SchoolFeeStructHead);
    //                }
    //                else
    //                {
    //                    SchoolFeeStructHead.FeeStructCode = obj.FeeStructCode.ToUpper();
    //                    await _context.SchoolFeeStructureHeader.AddAsync(SchoolFeeStructHead);
    //                }

    //                await _context.SaveChangesAsync();

    //                if (request.SchoolFeeStructureDto.FeeDetailList.Count() > 0)
    //                {
    //                    var oldFeeDetailList = await _context.SchoolFeeStructureDetails.Where(e => e.FeeStructCode == SchoolFeeStructHead.FeeStructCode).ToListAsync();
    //                    _context.SchoolFeeStructureDetails.RemoveRange(oldFeeDetailList);

    //                    List<TblSysSchoolFeeStructureDetails> feeDetailList = new();
    //                    foreach (var feedetail in request.SchoolFeeStructureDto.FeeDetailList)
    //                    {
    //                        TblSysSchoolFeeStructureDetails detail = new()
    //                        {
    //                            FeeStructCode = SchoolFeeStructHead.FeeStructCode,
    //                            TermCode = feedetail.TermCode,
    //                            FeeCode = feedetail.FeeCode,
    //                            FeeAmount = feedetail.FeeAmount,
    //                            MaxDiscPer = feedetail.MaxDiscPer,
    //                            ActualFeeAmount = feedetail.ActualFeeAmount

    //                        };
    //                        if (detail.Id > 0)
    //                        {
    //                            _context.SchoolFeeStructureDetails.Update(detail);
    //                        }
    //                        else
    //                        {

    //                            await _context.SchoolFeeStructureDetails.AddAsync(detail);
    //                        }

    //                        feeDetailList.Add(detail);
    //                    }
    //                    await _context.SchoolFeeStructureDetails.AddRangeAsync(feeDetailList);
    //                    await _context.SaveChangesAsync();
    //                }

    //                Log.Info("----Info CreateUpdateSchoolFeeStructDetails  method Exit----");
    //                await transaction.CommitAsync();
    //                return SchoolFeeStructHead.Id;
    //            }
    //            catch (Exception ex)
    //            {
    //                await transaction.RollbackAsync();
    //                Log.Error("Error in CreateUpdateSchoolFeeStructureDetails  Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return 0;
    //            }
    //        }
    //    }








    //}
    //#endregion
    #endregion


}
