using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
    #region GetAllSubjectByGrade&SemisterMapping 
    public class GetAllSubjectByGradeAndSemisterMapping : IRequest<List<SchoolGradeSubjectMappingDto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
        public string SemisterCode { get; set; }
    }

    public class GetAllSubjectByGradeAndSemisterMappingHandler : IRequestHandler<GetAllSubjectByGradeAndSemisterMapping, List<SchoolGradeSubjectMappingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSubjectByGradeAndSemisterMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SchoolGradeSubjectMappingDto>> Handle(GetAllSubjectByGradeAndSemisterMapping request, CancellationToken cancellationToken)
        {

            var SubjectMapping = await _context.SchoolGradeSubjectMapping.AsNoTracking()
                                    .Where(e => e.GradeCode == request.GradeCode 
                                      && e.SemisterCode == request.SemisterCode)
                                    .Select(x=>new SchoolGradeSubjectMappingDto() 
                                    { 
                                        GradeCode=x.GradeCode,
                                        SemisterCode=x.SemisterCode,
                                        SubCodes=x.SubCodes
                                    })
                                    .ToListAsync();

            return SubjectMapping;
        }
    }



    #endregion

    #region Create And Update

    public class CreateUpdateSchoolGradeSubjectMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }

        public TblSysSchoolGradeSubjectMappingDto SchoolGradeSubjectMapDto { get; set; }
    }

    public class CreateUpdateSchoolGradeSubjectMappingHandler : IRequestHandler<CreateUpdateSchoolGradeSubjectMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolGradeSubjectMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateUpdateSchoolGradeSubjectMapping request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolGradeSubjectMapping method start----");
                var obj = request.SchoolGradeSubjectMapDto;
                TblSysSchoolGradeSubjectMapping GradeSubjectMapp = new();
                if (request.SchoolGradeSubjectMapDto.SubCodes.Count() > 0)
                {
                    List<TblSysSchoolGradeSubjectMapping> schoolGradeSubjectList = new();
                    foreach (var item in request.SchoolGradeSubjectMapDto.SubCodes)
                    {
                        TblSysSchoolGradeSubjectMapping detail = new()
                        {
                            GradeCode = request.SchoolGradeSubjectMapDto.GradeCode,
                            SemisterCode = request.SchoolGradeSubjectMapDto.SemisterCode,
                            SubCodes = item,
                            CreatedOn = DateTime.Now,
                            CreatedBy = Convert.ToString(request.User.UserId)

                        };
                        await _context.SchoolGradeSubjectMapping.AddAsync(detail);
                        schoolGradeSubjectList.Add(detail);
                    }
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolGradeSubjectMapping  method Exit----");
                return 1;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolGradeSubjectMapping  Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion


    #region GetAllSubjectsByGradeCode
    public class GetAllSubjectsByGradeCode : IRequest<List<TblSysSchoolAcademicsSubectsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetAllSubjectsByGradeCodeHandler : IRequestHandler<GetAllSubjectsByGradeCode, List<TblSysSchoolAcademicsSubectsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSubjectsByGradeCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolAcademicsSubectsDto>> Handle(GetAllSubjectsByGradeCode request, CancellationToken cancellationToken)
        {
            List<TblSysSchoolAcademicsSubectsDto> result = new();
            var subjectCodes = await _context.SchoolGradeSubjectMapping.AsNoTracking().Where(e => e.GradeCode == request.GradeCode).Select(x => x.SubCodes).Distinct().ToListAsync();
            if (subjectCodes != null && subjectCodes.Count > 0)
            {
                result= await _context.SysSchoolAcademicsSubects.AsNoTracking().ProjectTo<TblSysSchoolAcademicsSubectsDto>(_mapper.ConfigurationProvider).Where(e => subjectCodes.Contains(e.SubCodes)).ToListAsync();
            }
            return result;
        }
    }



    #endregion



}
