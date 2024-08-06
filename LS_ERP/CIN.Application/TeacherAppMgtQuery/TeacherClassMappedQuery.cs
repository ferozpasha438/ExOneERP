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
    #region Teacher_Mapped_Grade_List

    public class GetTeacherGradeMappingList : IRequest<List<TblDefSchoolTeacherClassMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
       

    }

    public class GetTeacherGradeMappingListHandler : IRequestHandler<GetTeacherGradeMappingList, List<TblDefSchoolTeacherClassMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherGradeMappingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherClassMappingDto>> Handle(GetTeacherGradeMappingList request, CancellationToken cancellationToken)
        {
            var TeacherGradeMapList = await _context.DefSchoolTeacherClassMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherClassMappingDto>(_mapper.ConfigurationProvider).Where(e => (e.TeacherCode == request.TeacherCode )).ToListAsync();

            return TeacherGradeMapList;
        }


    }


    #endregion



    #region Teacher_Student_List

    public class GetTeacherStudentList : IRequest<List<TeacherStudentDataDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetTeacherStudentListHandler : IRequestHandler<GetTeacherStudentList, List<TeacherStudentDataDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTeacherStudentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TeacherStudentDataDto>> Handle(GetTeacherStudentList request, CancellationToken cancellationToken)
        {
           List<TeacherStudentDataDto> list = new();
            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(e => e.PrimaryBranchCode == request.BranchCode).FirstOrDefaultAsync();
            if(teacher != null) {
            var teacherGrade = await _context.DefSchoolTeacherClassMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherClassMappingDto>(_mapper.ConfigurationProvider).Where(e => (e.TeacherCode == teacher.TeacherCode & e.GradeCode==request.GradeCode)).FirstOrDefaultAsync();
           
            if (teacherGrade != null)
            {
              list = await _context.DefSchoolStudentMaster.AsNoTracking().ProjectTo<TeacherStudentDataDto>(_mapper.ConfigurationProvider)
                    .Where(e => e.GradeCode == teacherGrade.GradeCode && e.BranchCode == request.BranchCode)
                    .OrderBy(e=>e.GradeCode)
                    .ToListAsync();
            }
            }

            return list;
        }


    }


    #endregion

}
