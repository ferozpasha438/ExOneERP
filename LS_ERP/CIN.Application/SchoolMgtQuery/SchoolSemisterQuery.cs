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
    public class GetSysSchoolSemisterList : IRequest<PaginatedList<TblSysSchoolSemisterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolSemisterListHandler : IRequestHandler<GetSysSchoolSemisterList, PaginatedList<TblSysSchoolSemisterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolSemisterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolSemisterDto>> Handle(GetSysSchoolSemisterList request, CancellationToken cancellationToken)
        {

            var schoolSemister = await _context.SchoolSemister.AsNoTracking().ProjectTo<TblSysSchoolSemisterDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolSemister;
        }


    }


    #endregion

    #region GetById

    public class GetSysSchoolSemisterById : IRequest<TblSysSchoolSemisterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolSemisterByIdHandler : IRequestHandler<GetSysSchoolSemisterById, TblSysSchoolSemisterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolSemisterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolSemisterDto> Handle(GetSysSchoolSemisterById request, CancellationToken cancellationToken)
        {

            TblSysSchoolSemisterDto obj = new();
            var schoolSemister = await _context.SchoolSemister.AsNoTracking().ProjectTo<TblSysSchoolSemisterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return schoolSemister;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolSemister : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolSemisterDto SchoolSemisterDto { get; set; }
    }

    public class CreateUpdateSchoolSemisterHandler : IRequestHandler<CreateUpdateSchoolSemister, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolSemisterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolSemister request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolSemister method start----");

                var obj = request.SchoolSemisterDto;


                TblSysSchoolSemister SchoolSemister = new();
                if (obj.Id > 0)
                    SchoolSemister = await _context.SchoolSemister.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolSemister.Id = obj.Id;
                SchoolSemister.SemisterName = obj.SemisterName;
                SchoolSemister.SemisterName2 = obj.SemisterName2;
                SchoolSemister.SemisterStartDate = obj.SemisterStartDate;
                SchoolSemister.SemisterEndDate = obj.SemisterEndDate;
                SchoolSemister.IsActive = obj.IsActive;
                SchoolSemister.CreatedOn = DateTime.Now;
                SchoolSemister.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SchoolSemister.Update(SchoolSemister);
                }
                else
                {
                    SchoolSemister.SemisterCode = obj.SemisterCode.ToUpper();
                    await _context.SchoolSemister.AddAsync(SchoolSemister);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolSemister method Exit----");
                return SchoolSemister.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolSemister Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolSemister : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolSemisterHandler : IRequestHandler<DeleteSchoolSemister, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolSemisterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolSemister request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolSemister start----");

                if (request.Id > 0)
                {
                    var schoolSemister = await _context.SchoolSemister.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(schoolSemister);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolSemister");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
