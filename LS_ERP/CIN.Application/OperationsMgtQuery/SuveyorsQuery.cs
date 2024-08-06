using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
using CIN.Domain.OpeartionsMgt;


namespace CIN.Application.OperationsMgtQuery
{

    #region GetSurveyorsPagedList

    public class GetSurveyorsPagedList : IRequest<PaginatedList<SurveyorDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSurveyorsPagedListHandler : IRequestHandler<GetSurveyorsPagedList, PaginatedList<SurveyorDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyorsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<SurveyorDto>> Handle(GetSurveyorsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            try
            {
                var list = await _context.OprSurveyors.AsNoTracking().OrderBy(request.Input.OrderBy).Select(x => new SurveyorDto
                {
                    Id=x.Id,
                    SurveyorCode = x.SurveyorCode,
                    SurveyorNameArb = x.SurveyorNameArb,
                    SurveyorNameEng = x.SurveyorNameEng,
                    Branch = x.Branch,
                    UserId = x.UserId,
                    UserName = _context.SystemLogins.AsNoTracking().FirstOrDefault(s => s.Id == x.UserId).UserName??"",
                     IsActive=x.IsActive


                })
                     .Where(e => //e.CompanyId == request.CompanyId &&
                                   (e.SurveyorCode.Contains(search) ||
                                   e.SurveyorNameEng.Contains(search) ||
                                   e.UserName.Contains(search) ||
                                   e.SurveyorNameArb.Contains(search) || e.Branch.Contains(search) ||
                                   search == "" || search == null
                                    ))
                      

                        .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSurveyor Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null; 
            }
        }
    }
    #endregion

    #region CreateUpdateSurveyor
    public class CreateSurveyor : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSurveyorDto SurveyorDto { get; set; }
    }

    public class CreateSurveyorHandler : IRequestHandler<CreateSurveyor,int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSurveyorHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSurveyor request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSurveyor method start----");



                var obj = request.SurveyorDto;


                TblSndDefSurveyor Surveyor = new();
                if (obj.Id > 0)
                    Surveyor = await _context.OprSurveyors.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {

                    var sur = await _context.OprSurveyors.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (sur is not null)
                    {
                        Surveyor.SurveyorCode = "SRYR" + (sur.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        Surveyor.SurveyorCode = "SRYR000001";

                    if (_context.OprSurveyors.Any(x => x.SurveyorCode == Surveyor.SurveyorCode))
                    {
                        return -1;
                    }                   
                    else if(_context.OprSurveyors.Any(x => x.UserId == obj.UserId))
                            {
                        return -2;
                    }
                    //Surveyor.SurveyorCode = obj.SurveyorCode.ToUpper();
                }
                Surveyor.Id = obj.Id;
               Surveyor.IsActive = obj.IsActive;
                Surveyor.SurveyorNameEng = obj.SurveyorNameEng;
                Surveyor.SurveyorNameArb = obj.SurveyorNameArb;
                Surveyor.Branch = obj.Branch;
                Surveyor.UserId = obj.UserId;
                if (obj.Id > 0)
                {
                    Surveyor.SurveyorCode = obj.SurveyorCode;
                    Surveyor.ModifiedOn = DateTime.Now;
                    _context.OprSurveyors.Update(Surveyor);
                }
                else
                {

                    Surveyor.CreatedOn = DateTime.Now;
                    await _context.OprSurveyors.AddAsync(Surveyor);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSurveyor method Exit----");
                return Surveyor.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSurveyor Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetSurveyorBySurveyorCode
    public class GetSurveyorBySurveyorCode : IRequest<TblSndDefSurveyorDto>
    {
        public UserIdentityDto User { get; set; }
        public string SurveyorCode { get; set; }
    }

    public class GetSurveyorBySurveyorCodeHandler : IRequestHandler<GetSurveyorBySurveyorCode, TblSndDefSurveyorDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyorBySurveyorCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyorDto> Handle(GetSurveyorBySurveyorCode request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyorDto obj = new();
            var Surveyor = await _context.OprSurveyors.AsNoTracking().ProjectTo<TblSndDefSurveyorDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.SurveyorCode == request.SurveyorCode);
            //if (Surveyor is not null)
            //{
            //    obj.Id = Surveyor.Id;
            //    obj.SurveyorCode = Surveyor.SurveyorCode;
            //    obj.SurveyorCode = Surveyor.SurveyorCode;
            //    obj.SurveyorNameEng = Surveyor.SurveyorNameEng;
            //    obj.SurveyorNameArb = Surveyor.SurveyorNameArb;
            //    obj.ModifiedOn = Surveyor.ModifiedOn;
            //    obj.CreatedOn = Surveyor.CreatedOn;
            //    obj.IsActive = Surveyor.IsActive;
            //    obj.Branch = Surveyor.Branch;
            //    obj.Branch = Surveyor.Branch;
            //    return obj;
            ////}
            //else return null;

            return Surveyor;
        }

    }

    #endregion

    #region GetSurveyorById
    public class GetSurveyorById : IRequest<TblSndDefSurveyorDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSurveyorByIdHandler : IRequestHandler<GetSurveyorById, TblSndDefSurveyorDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyorByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyorDto> Handle(GetSurveyorById request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyorDto obj = new();
            var Surveyor = await _context.OprSurveyors.AsNoTracking().ProjectTo<TblSndDefSurveyorDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            //if (Surveyor is not null)
            //{
            //    obj.Id = Surveyor.Id;
            //    obj.SurveyorCode = Surveyor.SurveyorCode;
            //    obj.SurveyorCode = Surveyor.SurveyorCode;
            //    obj.SurveyorNameEng = Surveyor.SurveyorNameEng;
            //    obj.SurveyorNameArb = Surveyor.SurveyorNameArb;
            //    obj.ModifiedOn = Surveyor.ModifiedOn;
            //    obj.CreatedOn = Surveyor.CreatedOn;
            //    obj.IsActive = Surveyor.IsActive;
            //    obj.Branch = Surveyor.Branch;
            //    return obj;
            //}
            //else return null;
            return Surveyor;
        }
    }

    #endregion

    #region GetSelectSurveyorList

    public class GetSelectSurveyorList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectSurveyorListHandler : IRequestHandler<GetSelectSurveyorList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSurveyorListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSurveyorList request, CancellationToken cancellationToken)
        {

            var list = await _context.OprSurveyors
              .Select(e => new CustomSelectListItem { Text = e.SurveyorNameEng, Value = e.SurveyorCode, TextTwo = e.SurveyorNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectSurveyorList2

    public class GetSelectSurveyorListForBranch : IRequest<List<SurveyorDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetSelectSurveyorListForBranchHandler : IRequestHandler<GetSelectSurveyorListForBranch, List<SurveyorDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSurveyorListForBranchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SurveyorDto>> Handle(GetSelectSurveyorListForBranch request, CancellationToken cancellationToken)
        {

            var list = await _context.OprSurveyors.Where(x=>x.Branch==request.BranchCode)
              .Select(e => new SurveyorDto { SurveyorNameEng = e.SurveyorNameEng,
                  SurveyorCode = e.SurveyorCode, 
                  SurveyorNameArb = e.SurveyorNameArb ,
                  UserId=e.UserId,
                  LoginId=_context.SystemLogins.FirstOrDefault(u=>u.Id==e.UserId).LoginId??"",
                  UserName=_context.SystemLogins.FirstOrDefault(u=>u.Id==e.UserId).UserName??""
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region DeleteSurveyor
    public class DeleteSurveyor : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSurveyorQueryHandler : IRequestHandler<DeleteSurveyor, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSurveyorQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSurveyor request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSurveyor start----");

                if (request.Id > 0)
                {
                    var Surveyor = await _context.OprSurveyors.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Surveyor);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSurveyor");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
