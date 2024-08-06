using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
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
    public class SchoolTeacherQualificationQuery
    {
    }
    #region GetSchoolTeacherQualificationsByTeacherCode 
    public class GetSchoolTeacherQualificationsByTeacherCode : IRequest<List<TblDefSchoolTeacherQualificationDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetSchoolTeacherQualificationsByTeacherCodeHandler : IRequestHandler<GetSchoolTeacherQualificationsByTeacherCode, List<TblDefSchoolTeacherQualificationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolTeacherQualificationsByTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherQualificationDto>> Handle(GetSchoolTeacherQualificationsByTeacherCode request, CancellationToken cancellationToken)
        {
            List<TblDefSchoolTeacherQualificationDto> result = new();
            result = await _context.DefSchoolTeacherQualification.AsNoTracking().ProjectTo<TblDefSchoolTeacherQualificationDto>(_mapper.ConfigurationProvider).Where(e => e.TeacherCode == request.TeacherCode).ToListAsync();
            return result;
        }
    }

    #endregion

    #region SchoolTeacherQualificationsMapping

    public class SchoolTeacherQualificationsMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblDefSchoolTeacherQualificationDto> SchoolTeacherQualificationsList { get; set; }
    }

    public class SchoolTeacherQualificationsMappingHandler : IRequestHandler<SchoolTeacherQualificationsMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public SchoolTeacherQualificationsMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(SchoolTeacherQualificationsMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info SchoolTeacherQualificationsMapping method start----");
                    List<TblDefSchoolTeacherQualification> qualificationList = new();
                    if (request.SchoolTeacherQualificationsList.Count() > 0)
                    {
                        var oldList = await _context.DefSchoolTeacherQualification.Where(e => e.TeacherCode == request.SchoolTeacherQualificationsList.FirstOrDefault().TeacherCode).ToListAsync();
                        _context.DefSchoolTeacherQualification.RemoveRange(oldList);
                        foreach (var teacherQualification in request.SchoolTeacherQualificationsList)
                        {
                            TblDefSchoolTeacherQualification detail = new()
                            {
                                Id = teacherQualification.Id,
                                Grade= teacherQualification.Grade,
                                Institute= teacherQualification.Institute,
                                Percentage= teacherQualification.Percentage,
                                Qualification= teacherQualification.Qualification,
                                TeacherCode= teacherQualification.TeacherCode,
                                Year= teacherQualification.Year
                            };
                            qualificationList.Add(detail);
                        }
                        await _context.DefSchoolTeacherQualification.AddRangeAsync(qualificationList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info SchoolTeacherQualificationsMapping  method Exit----");
                    await transaction.CommitAsync();
                    return qualificationList.FirstOrDefault().Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in SchoolTeacherQualificationsMapping  Method");
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
