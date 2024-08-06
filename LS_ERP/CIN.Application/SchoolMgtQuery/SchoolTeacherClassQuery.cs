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
    public class SchoolTeacherClassQuery
    {
    }
    #region GetSchoolTeacherClassesByTeacherCode 
    public class GetSchoolTeacherClassesByTeacherCode : IRequest<List<TblDefSchoolTeacherClassMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetSchoolTeacherClassesByTeacherCodeHandler : IRequestHandler<GetSchoolTeacherClassesByTeacherCode, List<TblDefSchoolTeacherClassMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolTeacherClassesByTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherClassMappingDto>> Handle(GetSchoolTeacherClassesByTeacherCode request, CancellationToken cancellationToken)
        {
            List<TblDefSchoolTeacherClassMappingDto> result = new();
            result = await _context.DefSchoolTeacherClassMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherClassMappingDto>(_mapper.ConfigurationProvider).Where(e => e.TeacherCode == request.TeacherCode).ToListAsync();
            return result;
        }
    }

    #endregion

    #region SchoolTeacherClassesMapping

    public class SchoolTeacherClassesMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblDefSchoolTeacherClassMappingDto> SchoolTeacherClassesList { get; set; }
    }

    public class SchoolTeacherClassesMappingHandler : IRequestHandler<SchoolTeacherClassesMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public SchoolTeacherClassesMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(SchoolTeacherClassesMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info SchoolTeacherClassesMapping method start----");
                    List<TblDefSchoolTeacherClassMapping> classesList = new();
                    if (request.SchoolTeacherClassesList.Count() > 0)
                    {
                        var oldList = await _context.DefSchoolTeacherClassMapping.Where(e => e.TeacherCode == request.SchoolTeacherClassesList.FirstOrDefault().TeacherCode).ToListAsync();
                        _context.DefSchoolTeacherClassMapping.RemoveRange(oldList);


                        foreach (var teacherClass in request.SchoolTeacherClassesList)
                        {
                            TblDefSchoolTeacherClassMapping detail = new()
                            {
                                Id = teacherClass.Id,
                                GradeCode = teacherClass.GradeCode,
                                TeacherCode = teacherClass.TeacherCode,
                                SectionCode = teacherClass.SectionCode,
                                IsMapped = teacherClass.IsMapped
                            };
                            classesList.Add(detail);
                        }
                        await _context.DefSchoolTeacherClassMapping.AddRangeAsync(classesList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info SchoolTeacherClassesMapping  method Exit----");
                    await transaction.CommitAsync();
                    return classesList.FirstOrDefault().Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in SchoolTeacherClassesMapping  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion

    #region GetTeacherGradesByTeacherCode 
    public class GetTeacherGradesByTeacherCode : IRequest<List<TblSysSchoolAcedemicClassGradeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetTeacherGradesByTeacherCodeHandler : IRequestHandler<GetTeacherGradesByTeacherCode, List<TblSysSchoolAcedemicClassGradeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherGradesByTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolAcedemicClassGradeDto>> Handle(GetTeacherGradesByTeacherCode request, CancellationToken cancellationToken)
        {
            List<TblSysSchoolAcedemicClassGradeDto> result = new();
            var gradeList = await _context.DefSchoolTeacherClassMapping.AsNoTracking().Where(e => e.TeacherCode == request.TeacherCode).Select(x => x.GradeCode).Distinct().ToListAsync();
            if (gradeList != null && gradeList.Count > 0)
            {
                result= await _context.SchoolAcedemicClassGrade.AsNoTracking().ProjectTo<TblSysSchoolAcedemicClassGradeDto>(_mapper.ConfigurationProvider)
                                   .Where(x=> gradeList.Contains(x.GradeCode)).ToListAsync();
            }
            return result;
        }
    }

    #endregion
}
