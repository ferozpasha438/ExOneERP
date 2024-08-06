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
    public class GetSysSchoolAcademicBatchesList : IRequest<PaginatedList<TblSysSchoolAcademicBatchesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolAcademicBatchesListHandler : IRequestHandler<GetSysSchoolAcademicBatchesList, PaginatedList<TblSysSchoolAcademicBatchesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolAcademicBatchesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolAcademicBatchesDto>> Handle(GetSysSchoolAcademicBatchesList request, CancellationToken cancellationToken)
        {


            var academicBatches = await _context.SysSchoolAcademicBatches.AsNoTracking().ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider)
                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return academicBatches;
        }


    }


    #endregion

    #region GetSelectSchoolAcademicBatches

    public class GetSelectSchoolAcademicBatches : IRequest<List<CustomSelectListItem>>
    {                
    }

    public class GetSelectSchoolAcademicBatchesHandler : IRequestHandler<GetSelectSchoolAcademicBatches, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSchoolAcademicBatchesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSchoolAcademicBatches request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSchoolAcademicBatches method start----");
            var item = await _context.SysSchoolAcademicBatches.AsNoTracking()                
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.AcademicYear.ToString(), Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSchoolAcademicBatches method Ends----");
            return item;
        }
    }

    #endregion

    #region GetById

    public class GetSysSchoolAcademicBatchesById : IRequest<TblSysSchoolAcademicBatchesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolAcademicBatchesByIdHandler : IRequestHandler<GetSysSchoolAcademicBatchesById, TblSysSchoolAcademicBatchesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolAcademicBatchesByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolAcademicBatchesDto> Handle(GetSysSchoolAcademicBatchesById request, CancellationToken cancellationToken)
        {

            TblSysSchoolAcademicBatchesDto obj = new();
            var AcademicBatch = await _context.SysSchoolAcademicBatches.AsNoTracking().ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return AcademicBatch;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSysSchoolAcademicBatches : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolAcademicBatchesDto SchoolAcademicBatchesDto { get; set; }
    }

    public class CreateUpdateSysSchoolAcademicBatchesHandler : IRequestHandler<CreateUpdateSysSchoolAcademicBatches, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSysSchoolAcademicBatchesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSysSchoolAcademicBatches request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSysSchoolAcademicBatches method start----");

                var obj = request.SchoolAcademicBatchesDto;


                TblSysSchoolAcademicBatches AcademicBatches = new();
                if (obj.Id > 0)
                    AcademicBatches = await _context.SysSchoolAcademicBatches.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                AcademicBatches.Id = obj.Id;

                AcademicBatches.AcademicStartDate = obj.AcademicStartDate;
                AcademicBatches.AcademicEndDate = obj.AcademicEndDate;
                AcademicBatches.CreatedOn = DateTime.Now;
                AcademicBatches.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolAcademicBatches.Update(AcademicBatches);
                }
                else
                {

                    AcademicBatches.AcademicYear = obj.AcademicYear;
                    await _context.SysSchoolAcademicBatches.AddAsync(AcademicBatches);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSysSchoolAcademicBatches method Exit----");
                return AcademicBatches.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSysSchoolAcademicBatches Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolAcademicBatch : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolAcademicBatchHandler : IRequestHandler<DeleteSchoolAcademicBatch, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolAcademicBatchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolAcademicBatch request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolAcademicBatch start----");

                if (request.Id > 0)
                {
                    var batch = await _context.SysSchoolAcademicBatches.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(batch);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolAcademicBatches");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
