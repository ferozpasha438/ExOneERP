using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using CIN.Domain.SchoolMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SchoolMgtQuery
{
    public class SchoolTeacherSubjectsQuery
    {
    }
    #region GetSchoolTeacherSubjectsByTeacherCode 
    public class GetSchoolTeacherSubjectsByTeacherCode : IRequest<List<TblDefSchoolTeacherSubjectsMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetSchoolTeacherSubjectsByTeacherCodeHandler : IRequestHandler<GetSchoolTeacherSubjectsByTeacherCode, List<TblDefSchoolTeacherSubjectsMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolTeacherSubjectsByTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherSubjectsMappingDto>> Handle(GetSchoolTeacherSubjectsByTeacherCode request, CancellationToken cancellationToken)
        {
            List<TblDefSchoolTeacherSubjectsMappingDto> result = new();
            result = await _context.DefSchoolTeacherSubjectsMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherSubjectsMappingDto>(_mapper.ConfigurationProvider).Where(e => e.TeacherCode == request.TeacherCode).ToListAsync();
            return result;
        }
    }

    #endregion

    #region SchoolTeacherSubjectsMapping

    public class SchoolTeacherSubjectsMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblDefSchoolTeacherSubjectsMappingDto> SchoolTeacherSubjectsMappingList { get; set; }
    }

    public class SchoolTeacherSubjectsMappingHandler : IRequestHandler<SchoolTeacherSubjectsMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public SchoolTeacherSubjectsMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(SchoolTeacherSubjectsMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info SchoolTeacherSubjectsMapping method start----");
                    List<TblDefSchoolTeacherSubjectsMapping> subjectList = new();
                    if (request.SchoolTeacherSubjectsMappingList.Count() > 0)
                    {
                        var oldList = await _context.DefSchoolTeacherSubjectsMapping.Where(e => e.TeacherCode == request.SchoolTeacherSubjectsMappingList.FirstOrDefault().TeacherCode).ToListAsync();
                        _context.DefSchoolTeacherSubjectsMapping.RemoveRange(oldList);
                        foreach (var teacherSubject in request.SchoolTeacherSubjectsMappingList)
                        {
                            TblDefSchoolTeacherSubjectsMapping detail = new()
                            {
                                Id = teacherSubject.Id,
                                AdminSkillLevel = teacherSubject.AdminSkillLevel,
                                GradeCode = teacherSubject.GradeCode,
                                SubjectCode = teacherSubject.SubjectCode,
                                TeacherCode = teacherSubject.TeacherCode,
                                TeachingSkillLevel = teacherSubject.TeachingSkillLevel
                            };
                            subjectList.Add(detail);
                        }
                        await _context.DefSchoolTeacherSubjectsMapping.AddRangeAsync(subjectList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info SchoolTeacherSubjectsMapping  method Exit----");
                    await transaction.CommitAsync();
                    return subjectList.FirstOrDefault().Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in SchoolTeacherSubjectsMapping  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion

    #region GetTeacherSubjectsByGradeCode 
    public class GetTeacherSubjectsByGradeCode : IRequest<List<TblSysSchoolAcademicsSubectsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetTeacherSubjectsByGradeCodeHandler : IRequestHandler<GetTeacherSubjectsByGradeCode, List<TblSysSchoolAcademicsSubectsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherSubjectsByGradeCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolAcademicsSubectsDto>> Handle(GetTeacherSubjectsByGradeCode request, CancellationToken cancellationToken)
        {
            List<TblSysSchoolAcademicsSubectsDto> result = new();
            var subjectList = await _context.DefSchoolTeacherSubjectsMapping.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode && e.GradeCode == request.GradeCode).Select(x => x.SubjectCode).Distinct().ToListAsync();
            if (subjectList != null && subjectList.Count > 0)
            {
                result = await _context.SysSchoolAcademicsSubects.AsNoTracking().ProjectTo<TblSysSchoolAcademicsSubectsDto>(_mapper.ConfigurationProvider)
                                        .Where(x => subjectList.Contains(x.SubCodes)).ToListAsync();
            }
            return result;
        }
    }

    #endregion
}
