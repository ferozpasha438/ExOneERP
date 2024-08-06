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
    public class GetSysSchoolFeeTermsList : IRequest<PaginatedList<TblSysSchoolFeeTermsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolFeeTermsListHandler : IRequestHandler<GetSysSchoolFeeTermsList, PaginatedList<TblSysSchoolFeeTermsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolFeeTermsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolFeeTermsDto>> Handle(GetSysSchoolFeeTermsList request, CancellationToken cancellationToken)
        {

            
            var schoolFeeTerms = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolFeeTerms;
        }


    }


    #endregion

    #region GetById

    public class GetSysSchoolFeeTermById : IRequest<TblSysSchoolFeeTermsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolFeeTermByIdHandler : IRequestHandler<GetSysSchoolFeeTermById, TblSysSchoolFeeTermsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolFeeTermByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolFeeTermsDto> Handle(GetSysSchoolFeeTermById request, CancellationToken cancellationToken)
        {

            TblSysSchoolSemisterDto obj = new();
            var schoolFeeTerm = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return schoolFeeTerm;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolFeeTerms : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolFeeTermsDto SchoolFeeTermsDto { get; set; }
    }

    public class CreateUpdateSchoolFeeTermsHandler : IRequestHandler<CreateUpdateSchoolFeeTerms, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolFeeTermsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolFeeTerms request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolFeeTerms method start----");

                var obj = request.SchoolFeeTermsDto;


                TblSysSchoolFeeTerms SchoolFeeTerms = new();
                if (obj.Id > 0)
                    SchoolFeeTerms = await _context.SysSchoolFeeTerms.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolFeeTerms.Id = obj.Id;
                SchoolFeeTerms.TermName = obj.TermName;
                SchoolFeeTerms.TermName2 = obj.TermName2;
                SchoolFeeTerms.TermStartDate = obj.TermStartDate;
                SchoolFeeTerms.TermEndDate = obj.TermEndDate;
                SchoolFeeTerms.FeeDueDate = obj.FeeDueDate;
                SchoolFeeTerms.Remarks = obj.Remarks;
                SchoolFeeTerms.IsActive = obj.IsActive;
                SchoolFeeTerms.CreatedOn = DateTime.Now;
                SchoolFeeTerms.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolFeeTerms.Update(SchoolFeeTerms);
                }
                else
                {
                    SchoolFeeTerms.TermCode = obj.TermCode.ToUpper();
                    await _context.SysSchoolFeeTerms.AddAsync(SchoolFeeTerms);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolFeeTerms  method Exit----");
                return SchoolFeeTerms.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolFeeTerms  Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolFeeTerms : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolFeeTermsHandler : IRequestHandler<DeleteSchoolFeeTerms, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolFeeTermsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolFeeTerms request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolFeeTerms start----");

                if (request.Id > 0)
                {
                    var schoolFeeTerm = await _context.SysSchoolFeeTerms.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(schoolFeeTerm);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in  DeleteSchoolFeeTerms");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
