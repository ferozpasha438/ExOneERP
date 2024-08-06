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
    public class GetSchoolNationalityList : IRequest<PaginatedList<TblSysSchoolNationalityDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolNationalityListHandler : IRequestHandler<GetSchoolNationalityList, PaginatedList<TblSysSchoolNationalityDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolNationalityListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolNationalityDto>> Handle(GetSchoolNationalityList request, CancellationToken cancellationToken)
        {

            var schoolNationality = await _context.SysSchoolNationality.AsNoTracking().ProjectTo<TblSysSchoolNationalityDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolNationality;
        }


    }

    #endregion

    #region GetById

    public class GetSchoolNationalityById : IRequest<TblSysSchoolNationalityDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolNationalityByIdHandler : IRequestHandler<GetSchoolNationalityById, TblSysSchoolNationalityDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolNationalityByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolNationalityDto> Handle(GetSchoolNationalityById request, CancellationToken cancellationToken)
        {

            TblSysSchoolNationalityDto obj = new();
            var SchoolNationality = await _context.SysSchoolNationality.AsNoTracking().ProjectTo<TblSysSchoolNationalityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolNationality;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolNationality : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolNationalityDto SchoolNationalityDto { get; set; }
    }

    public class CreateUpdateSchoolNationalityHandler : IRequestHandler<CreateUpdateSchoolNationality, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolNationalityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolNationality request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolNationality method start----");

                var obj = request.SchoolNationalityDto;


                TblSysSchoolNationality SchoolNationality = new();
                if (obj.Id > 0)
                    SchoolNationality = await _context.SysSchoolNationality.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolNationality.Id = obj.Id;
                SchoolNationality.NatName = obj.NatName;
                SchoolNationality.NatName2 = obj.NatName2;
                SchoolNationality.IsActive = obj.IsActive;
                SchoolNationality.CreatedOn = DateTime.Now;
                SchoolNationality.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolNationality.Update(SchoolNationality);
                }
                else
                {
                    SchoolNationality.NatCode = obj.NatCode.ToUpper();
                    await _context.SysSchoolNationality.AddAsync(SchoolNationality);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolNationality method Exit----");
                return SchoolNationality.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolNationality Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolNationality : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolNationalityHandler : IRequestHandler<DeleteSchoolNationality, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolNationalityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolNationality request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolAcedemicClassGrade start----");

                if (request.Id > 0)
                {
                    var SchoolNationality = await _context.SysSchoolNationality.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(SchoolNationality);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolNationality");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
