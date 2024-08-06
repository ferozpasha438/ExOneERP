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

    #region GetAll
    public class GetSysSchoolAcademicsSubjectsList : IRequest<PaginatedList<TblSysSchoolAcademicsSubectsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolAcademicsSubjectsListHandler : IRequestHandler<GetSysSchoolAcademicsSubjectsList, PaginatedList<TblSysSchoolAcademicsSubectsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolAcademicsSubjectsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolAcademicsSubectsDto>> Handle(GetSysSchoolAcademicsSubjectsList request, CancellationToken cancellationToken)
        {

            
            var academicSubjects = await _context.SysSchoolAcademicsSubects.AsNoTracking().ProjectTo<TblSysSchoolAcademicsSubectsDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return academicSubjects;
        }


    }


    #endregion

    #region GetById

    public class GetSysSchoolAcademicsSubjectsById : IRequest<TblSysSchoolAcademicsSubectsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolAcademicsSubjectsByIdHandler : IRequestHandler<GetSysSchoolAcademicsSubjectsById, TblSysSchoolAcademicsSubectsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolAcademicsSubjectsByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolAcademicsSubectsDto> Handle(GetSysSchoolAcademicsSubjectsById request, CancellationToken cancellationToken)
        {

            TblSysSchoolAcademicsSubectsDto obj = new();
            var AcademicSubject = await _context.SysSchoolAcademicsSubects.AsNoTracking().ProjectTo<TblSysSchoolAcademicsSubectsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return AcademicSubject;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSysSchoolAcademicSubjects : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolAcademicsSubectsDto SchoolAcademicsSubectsDto { get; set; }
    }

    public class CreateUpdateSysSchoolAcademicSubjectsHandler : IRequestHandler<CreateUpdateSysSchoolAcademicSubjects, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSysSchoolAcademicSubjectsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSysSchoolAcademicSubjects request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSysSchoolAcademicSubject method start----");

                var obj = request.SchoolAcademicsSubectsDto;


                TblSysSchoolAcademicsSubects AcademicSubjects = new();
                if (obj.Id > 0)
                    AcademicSubjects = await _context.SysSchoolAcademicsSubects.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                AcademicSubjects.Id = obj.Id;
                AcademicSubjects.SubName = obj.SubName;
                AcademicSubjects.SubName2 = obj.SubName2;
                AcademicSubjects.IsActive = obj.IsActive;
                AcademicSubjects.CreatedOn = DateTime.Now;
                AcademicSubjects.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolAcademicsSubects.Update(AcademicSubjects);
                }
                else
                {
                    AcademicSubjects.SubCodes = obj.SubCodes.ToUpper();
                    await _context.SysSchoolAcademicsSubects.AddAsync(AcademicSubjects);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSysSchoolAcademicSubjects method Exit----");
                return AcademicSubjects.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSysSchoolAcademicSubjects Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSysSchoolAcademicSubject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSysSchoolAcademicSubjectsHandler : IRequestHandler<DeleteSysSchoolAcademicSubject, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSysSchoolAcademicSubjectsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSysSchoolAcademicSubject request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSysSchoolAcademicSubject start----");

                if (request.Id > 0)
                {
                    var subject = await _context.SysSchoolAcademicsSubects.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(subject);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSysSchoolAcademicSubject");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region GetAllGradeSubjectList

    public class GetAllGradeSubjectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetAllGradeSubjectListHandler : IRequestHandler<GetAllGradeSubjectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllGradeSubjectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAllGradeSubjectList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAllGradeSubjectList method start----");
            var item = await _context.SchoolGradeSubjectMapping
                          .Join(_context.SysSchoolAcademicsSubects, map => map.SubCodes, sub => sub.SubCodes, (map, sub) => new { sub.SubCodes, sub.SubName, sub.SubName2,map.GradeCode })
                .AsNoTracking()
                .Where(e =>e.GradeCode == request.GradeCode)
               .Select(e => new CustomSelectListItem { Text = e.SubName, Value = e.SubCodes, TextTwo = e.SubName2 })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetAllGradeSubjectList method Ends----");
            return item;
        }
    }

    #endregion

}
