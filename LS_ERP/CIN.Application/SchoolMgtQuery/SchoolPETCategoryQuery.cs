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
    public class GetSysSchoolPETCategoryList : IRequest<PaginatedList<TblSysSchoolPETCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolPETCategoryListHandler : IRequestHandler<GetSysSchoolPETCategoryList, PaginatedList<TblSysSchoolPETCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolPETCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolPETCategoryDto>> Handle(GetSysSchoolPETCategoryList request, CancellationToken cancellationToken)
        {

           
            var petCategories = await _context.SysSchoolPETCategory.AsNoTracking().ProjectTo<TblSysSchoolPETCategoryDto>(_mapper.ConfigurationProvider)
                                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return petCategories;
        }


    }


    #endregion

    #region GetById

    public class GetSysSchoolPETCategorById : IRequest<TblSysSchoolPETCategoryDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolPETCategorByIdHandler : IRequestHandler<GetSysSchoolPETCategorById, TblSysSchoolPETCategoryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolPETCategorByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolPETCategoryDto> Handle(GetSysSchoolPETCategorById request, CancellationToken cancellationToken)
        {

            TblSysSchoolAcademicsSubectsDto obj = new();
            var SchoolPETCategory = await _context.SysSchoolPETCategory.AsNoTracking().ProjectTo<TblSysSchoolPETCategoryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolPETCategory;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSysSchoolPETCategory : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolPETCategoryDto SchoolPETCategoryDto { get; set; }
    }

    public class CreateUpdateSysSchoolPETCategoryHandler : IRequestHandler<CreateUpdateSysSchoolPETCategory, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSysSchoolPETCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSysSchoolPETCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSysSchoolPETCategory method start----");

                var obj = request.SchoolPETCategoryDto;


                TblSysSchoolPETCategory SchoolPETCategory = new();
                if (obj.Id > 0)
                    SchoolPETCategory = await _context.SysSchoolPETCategory.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolPETCategory.Id = obj.Id;
                SchoolPETCategory.PETName = obj.PETName;
                SchoolPETCategory.PETName2 = obj.PETName2;
                SchoolPETCategory.IsActive = obj.IsActive;
                SchoolPETCategory.CreatedOn = DateTime.Now;
                SchoolPETCategory.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolPETCategory.Update(SchoolPETCategory);
                }
                else
                {
                    SchoolPETCategory.PETCode = obj.PETCode.ToUpper();
                    await _context.SysSchoolPETCategory.AddAsync(SchoolPETCategory);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSysSchoolPETCategory method Exit----");
                return SchoolPETCategory.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSysSchoolPETCategory Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSysSchoolPETCategory : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSysSchoolPETCategoryHandler : IRequestHandler<DeleteSysSchoolPETCategory, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSysSchoolPETCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSysSchoolPETCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSysSchoolPETCategory start----");

                if (request.Id > 0)
                {
                    var schoolPETCategory = await _context.SysSchoolPETCategory.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(schoolPETCategory);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSysSchoolPETCategory");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
