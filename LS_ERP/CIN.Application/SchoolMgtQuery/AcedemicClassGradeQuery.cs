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
using CIN.Domain.SalesSetup;
using static System.Net.Mime.MediaTypeNames;

namespace CIN.Application.SchoolMgtQuery
{

    #region GetAll
    public class GetAcedemicClassGradeList : IRequest<PaginatedList<TblSysSchoolAcedemicClassGradeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetAcedemicClassGradeListHandler : IRequestHandler<GetAcedemicClassGradeList, PaginatedList<TblSysSchoolAcedemicClassGradeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcedemicClassGradeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolAcedemicClassGradeDto>> Handle(GetAcedemicClassGradeList request, CancellationToken cancellationToken)
        {

            try
            {

                var classGrades = await _context.SchoolAcedemicClassGrade.AsNoTracking().ProjectTo<TblSysSchoolAcedemicClassGradeDto>(_mapper.ConfigurationProvider)
                                   .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return classGrades;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }


    #endregion


    #region GetSelectAcedemicClassGrades

    public class GetSelectAcedemicClassGrades : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectAcedemicClassGradesHandler : IRequestHandler<GetSelectAcedemicClassGrades, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectAcedemicClassGradesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectAcedemicClassGrades request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectAcedemicClassGrades method start----");
            var item = await _context.SchoolAcedemicClassGrade.AsNoTracking()
                 .Where(e => e.IsActive)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = request.User.Culture != "ar" ? e.GradeName : e.GradeName2, Value = e.GradeCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectAcedemicClassGrades method Ends----");
            return item;
        }
    }

    #endregion

    #region GetById

    public class GetAcedemicClassGradeById : IRequest<TblSysSchoolAcedemicClassGradeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAcedemicClassGradeByIdHandler : IRequestHandler<GetAcedemicClassGradeById, TblSysSchoolAcedemicClassGradeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcedemicClassGradeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolAcedemicClassGradeDto> Handle(GetAcedemicClassGradeById request, CancellationToken cancellationToken)
        {

            TblSysSchoolAcedemicClassGradeDto obj = new();
            var classGrades = await _context.SchoolAcedemicClassGrade.AsNoTracking().ProjectTo<TblSysSchoolAcedemicClassGradeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return classGrades;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateAcedemicClassGrades : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolAcedemicClassGradeDto SchoolAcedemicClassGradeDto { get; set; }
    }

    public class CreateUpdateAcedemicClassGradesHandler : IRequestHandler<CreateUpdateAcedemicClassGrades, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateAcedemicClassGradesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateAcedemicClassGrades request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSchoolAcedemicClassGrades method start----");
                    var obj = request.SchoolAcedemicClassGradeDto;
                    TblSysSchoolAcedemicClassGrade ClassGrade = new();
                    if (obj.Id > 0)
                        ClassGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    ClassGrade.Id = obj.Id;
                    ClassGrade.GradeName = obj.GradeName;
                    ClassGrade.GradeName2 = obj.GradeName2;
                    ClassGrade.GradeOrder = obj.GradeOrder;
                    ClassGrade.IsActive = obj.IsActive;
                    ClassGrade.CreatedOn = DateTime.Now;
                    ClassGrade.CreatedBy = obj.CreatedBy;
                    if (obj.Id > 0)
                    {

                        _context.SchoolAcedemicClassGrade.Update(ClassGrade);
                    }
                    else
                    {
                        ClassGrade.GradeCode = obj.GradeCode.ToUpper();
                        await _context.SchoolAcedemicClassGrade.AddAsync(ClassGrade);
                    }

                    #region tblSndDefCustomerCategory
                    TblSndDefCustomerCategory categoryData = new();
                    categoryData = await _context.SndCustomerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CustCatCode == obj.GradeCode.ToUpper());
                    if (categoryData != null)
                    {
                        categoryData.CustCatName = obj.GradeName;
                        categoryData.CustCatDesc = obj.GradeName2;
                        categoryData.ModifiedOn = DateTime.Now;
                        categoryData.IsActive = obj.IsActive;
                        _context.SndCustomerCategories.Update(categoryData);
                    }
                    else
                    {
                        categoryData = new();
                        categoryData.CustCatCode = obj.GradeCode.ToUpper();
                        categoryData.CustCatName = obj.GradeName;
                        categoryData.CustCatDesc = obj.GradeName2;
                        categoryData.CreatedOn = DateTime.Now;
                        categoryData.IsActive = obj.IsActive;
                        categoryData.CatPrefix = "1";
                        categoryData.LastSeq = 0;
                        await _context.SndCustomerCategories.AddAsync(categoryData);
                    }
                    #endregion
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateSchoolAcedemicClassGrades method Exit----");
                    return ClassGrade.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSchoolAcedemicClassGrades Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteAcedemicClassGrade : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteAcedemicClassGradeHandler : IRequestHandler<DeleteAcedemicClassGrade, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteAcedemicClassGradeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteAcedemicClassGrade request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolAcedemicClassGrade start----");

                if (request.Id > 0)
                {
                    var ClassGrade = await _context.SchoolAcedemicClassGrade.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(ClassGrade);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolAcedemicClassGrade");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region GetAcademicYear

    public class GetAcademicYear : IRequest<string>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAcademicYearHandler : IRequestHandler<GetAcademicYear, string>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcademicYearHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetAcademicYear request, CancellationToken cancellationToken)
        {
            return await _context.SysSchoolAcademicBatches.AsNoTracking().ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).Select(x => x.AcademicYear.ToString()).FirstOrDefaultAsync();
        }
    }
    #endregion

    #region UploadGradeDocument


    public class UploadGradeDocument : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public GradeDoumentDto Dto { get; set; }
    }

    public class UploadGradeDocumentHandler : IRequestHandler<UploadGradeDocument, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UploadGradeDocumentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(UploadGradeDocument request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info UploadGradeDocument method start----");
                var obj = request.Dto;
                TblSysSchoolAcedemicClassGrade classGrade = await _context.SchoolAcedemicClassGrade.AsNoTracking().FirstOrDefaultAsync(e => e.GradeCode == obj.GradeCode);
                classGrade.FileName = obj.UploadFileName;
                _context.SchoolAcedemicClassGrade.Update(classGrade);
                await _context.SaveChangesAsync();
                Log.Info("----Info UploadGradeDocument method Exit----");
                return classGrade.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in UploadGradeDocument Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

}
