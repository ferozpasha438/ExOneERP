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
    #region GetSurveyFormHeaderPagedList

    public class GetSurveyFormHeaderPagedList : IRequest<PaginatedList<TblSndDefSurveyFormHeadDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSurveyFormHeaderPagedListHandler : IRequestHandler<GetSurveyFormHeaderPagedList, PaginatedList<TblSndDefSurveyFormHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormHeaderPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefSurveyFormHeadDto>> Handle(GetSurveyFormHeaderPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprSurveyFormHeads.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.SurveyFormCode.Contains(search) ||
                            e.SurveyFormNameEng.Contains(search) ||
                            e.SurveyFormNameArb.Contains(search) ||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefSurveyFormHeadDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetSurveyFormHeaderById
    public class GetSurveyFormHeaderById : IRequest<TblSndDefSurveyFormHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSurveyFormHeaderByIdHandler : IRequestHandler<GetSurveyFormHeaderById, TblSndDefSurveyFormHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormHeaderByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyFormHeadDto> Handle(GetSurveyFormHeaderById request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyFormHeadDto obj = new();

            var SurForm = await _context.OprSurveyFormHeads.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);

            if (SurForm is not null)

            {
                obj.SurveyFormCode = SurForm.SurveyFormCode;
                obj.SurveyFormNameEng = SurForm.SurveyFormNameEng;
                obj.SurveyFormNameArb = SurForm.SurveyFormNameArb;


                obj.Remarks = SurForm.Remarks;
                
                obj.ModifiedOn = SurForm.ModifiedOn;
                obj.CreatedOn = SurForm.CreatedOn;
                obj.IsActive = SurForm.IsActive;
                
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSurveyFormHeaderBySurveyFormCode
    public class GetSurveyFormHeaderBySurveyFormCode : IRequest<TblSndDefSurveyFormHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public string SurveyFormCode { get; set; }
    }

    public class GetSurveyFormHeaderBySurveyFormCodeHandler : IRequestHandler<GetSurveyFormHeaderBySurveyFormCode, TblSndDefSurveyFormHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormHeaderBySurveyFormCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyFormHeadDto> Handle(GetSurveyFormHeaderBySurveyFormCode request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyFormHeadDto obj = new();

            var SurForm = await _context.OprSurveyFormHeads.AsNoTracking().FirstOrDefaultAsync(e => e.SurveyFormCode == request.SurveyFormCode);

            if (SurForm is not null)

            {
              
                obj.Id = SurForm.Id;
                obj.SurveyFormNameArb = SurForm.SurveyFormNameArb;
                obj.SurveyFormNameEng = SurForm.SurveyFormNameEng;

                obj.Remarks = SurForm.Remarks;
               obj.ModifiedOn = SurForm.ModifiedOn;
                obj.CreatedOn = SurForm.CreatedOn;
                obj.IsActive = SurForm.IsActive;
                
                return obj;
            }
            else return null;
        }
    }

    #endregion


    #region GetSelectSurveyFormHeaderList

    public class GetSelectSurveyFormHeaderList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectSurveyFormHeaderListHandler : IRequestHandler<GetSelectSurveyFormHeaderList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSurveyFormHeaderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSurveyFormHeaderList request, CancellationToken cancellationToken)
        {

            var list = await _context.OprSurveyFormHeads.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.SurveyFormNameEng, Value = e.SurveyFormCode, TextTwo = e.SurveyFormNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSurveyFormHeadById
    public class GetSurveyFormHeadById : IRequest<TblSndDefSurveyFormHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSurveyFormHeadByIdHandler : IRequestHandler<GetSurveyFormHeadById, TblSndDefSurveyFormHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormHeadByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyFormHeadDto> Handle(GetSurveyFormHeadById request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyFormHeadDto obj = new();
            var surveyFormHead = await _context.OprSurveyFormHeads.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (surveyFormHead is not null)
            {
                obj.Id = surveyFormHead.Id;
                obj.SurveyFormNameArb = surveyFormHead.SurveyFormNameArb;
                obj.SurveyFormNameEng = surveyFormHead.SurveyFormNameEng;
                obj.SurveyFormCode = surveyFormHead.SurveyFormCode;
                obj.ModifiedOn = surveyFormHead.ModifiedOn;
                obj.CreatedOn = surveyFormHead.CreatedOn;
                obj.IsActive = surveyFormHead.IsActive;

                return obj;
            }
            else return null;
        }
    }


    #endregion
}
