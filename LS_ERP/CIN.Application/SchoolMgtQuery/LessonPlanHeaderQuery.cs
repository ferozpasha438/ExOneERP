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
    #region Time Table List

    public class GetLessonPlanHeader : IRequest<List<TblLessonPlanHeaderDto>>
    {
        public UserIdentityDto User { get; set; }

        public DateTime StartDate { get; set; }

        public string Grade { get; set; }

        public string BranchCode { get; set; }
    }

    public class GetLessonPlanHeaderHandler : IRequestHandler<GetLessonPlanHeader, List<TblLessonPlanHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetLessonPlanHeaderHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<TblLessonPlanHeaderDto>> Handle(GetLessonPlanHeader requset, CancellationToken cancellationToken)
        {
            var lessonPlanHeaderlist = await _context.LessonPlanHeader.AsNoTracking().ProjectTo<TblLessonPlanHeaderDto>(_mapper.ConfigurationProvider).Where(e => e.EstStartDate == requset.StartDate && e.GradeCode == requset.Grade && e.BranchCode == requset.BranchCode).ToListAsync();
            return lessonPlanHeaderlist;
        }
    }

    #endregion


    #region GetLessonPlanList

    public class GetLessonPlanList : IRequest<PaginatedList<TblLessonPlanHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetLessonPlanListHandler : IRequestHandler<GetLessonPlanList, PaginatedList<TblLessonPlanHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetLessonPlanListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PaginatedList<TblLessonPlanHeaderDto>> Handle(GetLessonPlanList request, CancellationToken cancellationToken)
        {
            var lessonPlanHeaderlist = await _context.LessonPlanHeader.AsNoTracking()
                                         .ProjectTo<TblLessonPlanHeaderDto>(_mapper.ConfigurationProvider)
                                         .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return lessonPlanHeaderlist;
        }
    }

    #endregion


    #region CreateLessonPlanInfo
    public class CreateLessonPlanInfo : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TeacherLessonPlanInfoDto LessonPlanInfoDto { get; set; }
    }

    public class CreateLessonPlanInfoHandler : IRequestHandler<CreateLessonPlanInfo, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateLessonPlanInfoHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateLessonPlanInfo request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateLessonPlanInfo method start----");
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
                        foreach (var item in obj.TableRows)
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
                    Log.Info("----Info CreateLessonPlanInfo method Exit----");
                    return lessonPlanHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateLessonPlanInfo Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
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
                                EstEndDate =Convert.ToDateTime(x.EstEndDate),
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
                               .Select(x=>new LessonPlanDetailsDto() 
                                 {
                                   Chapter=x.Chapter,
                                   EndTime=Convert.ToString(x.EndTime),
                                   LessonName=x.LessonName,
                                   LessonName2=x.LessonName2,
                                   NumOfSessions=x.NumOfSessions,
                                   StartTime= Convert.ToString(x.StartTime),
                                   TopicDate=Convert.ToDateTime(x.EstStartDate),
                                   Topics=x.Topics,
                                   Topics2=x.Topics2

                                 })
                               .ToListAsync();
                result.TableRows = details;
            }
            else
                result = new();
            return result;
        }
    }

    #endregion

}
