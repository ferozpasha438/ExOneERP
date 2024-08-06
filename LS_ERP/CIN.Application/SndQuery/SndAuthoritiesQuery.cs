using AutoMapper;
using CIN.Application.Common;
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
using CIN.Domain.SND;
using CIN.Application.SndDtos;

namespace CIN.Application.SndQuery
{



    #region CreateUpdateSndAuthorities
    public class CreateSndAuthorities : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndAuthoritiesDto Input { get; set; }
    }

    public class CreateSndAuthoritiesHandler : IRequestHandler<CreateSndAuthorities, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSndAuthoritiesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSndAuthorities request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSndAuthorities method start----");


              
                var obj = request.Input;

                var branchCodes = _context.CompanyBranches.AsNoTracking();

                TblSndAuthorities SndAuthorities = new();
                if (obj.Id > 0)
                    SndAuthorities = await _context.TblSndAuthoritiesList.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
               
                    SndAuthorities.AppAuth = obj.AppAuth;
                }
                SndAuthorities.Id = obj.Id;
                SndAuthorities.BranchCode = obj.BranchCode;
                SndAuthorities.AppLevel = obj.AppLevel;



                SndAuthorities.CanCreateSndInvoice = obj.CanCreateSndInvoice;
                SndAuthorities.CanEditSndInvoice = obj.CanEditSndInvoice;
                SndAuthorities.CanApproveSndInvoice = obj.CanApproveSndInvoice;
                SndAuthorities.CanPostSndInvoice = obj.CanPostSndInvoice;
                SndAuthorities.CanSettleSndInvoice = obj.CanSettleSndInvoice;
                SndAuthorities.CanVoidSndInvoice = obj.CanVoidSndInvoice;
         
                SndAuthorities.CanCreateSndQuotation = obj.CanCreateSndQuotation;
                SndAuthorities.CanEditSndQuotation = obj.CanEditSndQuotation;
                SndAuthorities.CanApproveSndQuotation = obj.CanApproveSndQuotation;
                SndAuthorities.CanConvertSndQuotationToDeliveryNote = obj.CanConvertSndQuotationToDeliveryNote;
                SndAuthorities.CanConvertSndQuotationToOrder = obj.CanConvertSndQuotationToOrder;
                SndAuthorities.CanConvertSndQuotationToInvoice = obj.CanConvertSndQuotationToInvoice;
                SndAuthorities.CanReviseSndQuotation = obj.CanReviseSndQuotation;
                SndAuthorities.CanVoidSndQuotation = obj.CanVoidSndQuotation;
                SndAuthorities.CanConvertSndDeliveryNoteToInvoice = obj.CanConvertSndDeliveryNoteToInvoice;

         

    
                SndAuthorities.IsFinalAuthority = obj.IsFinalAuthority;
           
                bool hasAuthority = (obj.CanCreateSndInvoice||
                    obj.CanEditSndInvoice||
                    obj.CanApproveSndInvoice||
                    obj.CanPostSndInvoice||
                    obj.CanSettleSndInvoice||
                    obj.CanVoidSndInvoice||

                obj.CanCreateSndQuotation||
                obj.CanEditSndQuotation||
                obj.CanApproveSndQuotation||
                 obj.CanConvertSndQuotationToDeliveryNote||
                obj.CanConvertSndQuotationToOrder||
                obj.CanConvertSndQuotationToInvoice||
                 obj.CanReviseSndQuotation||
               obj.CanVoidSndQuotation||
                 obj.CanConvertSndDeliveryNoteToInvoice
                    );




                if (obj.Id > 0 )
                {

                    if (hasAuthority)
                    {
                        SndAuthorities.ModifiedOn = DateTime.Now;

                        _context.TblSndAuthoritiesList.Update(SndAuthorities);
                    }

                    else
                    {
                        _context.TblSndAuthoritiesList.Remove(SndAuthorities);

                    }
                   
                }

                else
                {
                    if (hasAuthority)
                    {

                        SndAuthorities.CreatedOn = DateTime.Now;
                        await _context.TblSndAuthoritiesList.AddAsync(SndAuthorities);
                    }

                    else
                    {
                        return -2;
                    }
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSndAuthorities method Exit----");
                return SndAuthorities.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSndAuthorities Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetSndAuthoritiesByUserId
    public class GetSndAuthoritiesByUserId : IRequest<TblSndAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetSndAuthoritiesByUserIdHandler : IRequestHandler<GetSndAuthoritiesByUserId, TblSndAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndAuthoritiesByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndAuthoritiesDto> Handle(GetSndAuthoritiesByUserId request, CancellationToken cancellationToken)
        {
            TblSndAuthoritiesDto obj = new();
            var SndAuthorities = await _context.TblSndAuthoritiesList.AsNoTracking().ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.AppAuth == request.User.UserId);
            if (SndAuthorities is null)
            {
                obj.Id = SndAuthorities.Id;
                obj.AppLevel = 0;
                obj.IsFinalAuthority = false;

                obj.CanCreateSndInvoice = false;
                obj.CanEditSndInvoice = false;
                obj.CanSettleSndInvoice = false;
                obj.CanPostSndInvoice = false;
                obj.CanVoidSndInvoice = false;
                obj.CanApproveSndInvoice = false;


                obj.CanCreateSndQuotation =          false;
                obj.CanEditSndQuotation =            false;
                obj.CanApproveSndQuotation =         false;
                obj.CanConvertSndQuotationToOrder =  false;
                obj.CanConvertSndQuotationToInvoice =false;
                obj.CanReviseSndQuotation =          false;
                obj.CanVoidSndQuotation = false;
                obj.CanConvertSndQuotationToDeliveryNote = false;

            }
            else
                obj = SndAuthorities;
            return obj;
        }
    }

    #endregion

    #region GetSndAuthoritiesListByUserId
    public class GetSndAuthoritiesListByUserId : IRequest<List<TblSndAuthoritiesDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetSndAuthoritiesListByUserIdHandler : IRequestHandler<GetSndAuthoritiesListByUserId, List<TblSndAuthoritiesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSndAuthoritiesListByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSndAuthoritiesDto>> Handle(GetSndAuthoritiesListByUserId request, CancellationToken cancellationToken)
        {
            TblSndAuthoritiesDto obj = new();
            var SndAuthorities = await _context.TblSndAuthoritiesList.AsNoTracking().ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).Where(e => e.AppAuth == request.User.UserId).ToListAsync();
            if (SndAuthorities.Count==0)
            {
                
                obj.Id = 0;
                
                obj.AppLevel = 0;
             
                obj.IsFinalAuthority = false;

                obj.CanCreateSndInvoice = false;
                obj.CanEditSndInvoice = false;
                obj.CanSettleSndInvoice = false;
                obj.CanPostSndInvoice = false;
                obj.CanVoidSndInvoice = false;
                obj.CanApproveSndInvoice = false;


                obj.CanCreateSndQuotation = false;
                obj.CanEditSndQuotation = false;
                obj.CanApproveSndQuotation = false;
                obj.CanConvertSndQuotationToOrder = false;
                obj.CanConvertSndQuotationToInvoice = false;
                obj.CanReviseSndQuotation = false;
                obj.CanVoidSndQuotation = false;
                obj.CanConvertSndQuotationToDeliveryNote = false;




            }
            else
                SndAuthorities.Add(obj);
            return SndAuthorities;
        }
    }

    #endregion

    #region GetAuthoritiesPagedList

    public class GetAuthoritiesPagedList : IRequest<PaginatedList<TblSndAuthoritiesPagedList>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAuthoritiesPagedListHandler : IRequestHandler<GetAuthoritiesPagedList, PaginatedList<TblSndAuthoritiesPagedList>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthoritiesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndAuthoritiesPagedList>> Handle(GetAuthoritiesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var userNames = _context.SystemLogins.AsNoTracking(); 
            var list = await _context.TblSndAuthoritiesList.AsNoTracking()
                            
              .OrderBy(request.Input.OrderBy).Select(e=> new TblSndAuthoritiesPagedList { 
               Id=e.Id,
               CreatedOn=e.CreatedOn,
               BranchCode=e.BranchCode,
               AppAuth=e.AppAuth,
               AppLevel=e.AppLevel,
               IsFinalAuthority=e.IsFinalAuthority,
               ModifiedOn=e.ModifiedOn,


               CanCreateSndInvoice=e.CanCreateSndInvoice,
               CanApproveSndInvoice=e.CanApproveSndInvoice,
               CanPostSndInvoice=e.CanPostSndInvoice,
               CanEditSndInvoice=e.CanEditSndInvoice,
               CanVoidSndInvoice=e.CanVoidSndInvoice,
               CanSettleSndInvoice=e.CanSettleSndInvoice,

            CanCreateSndQuotation = e.CanCreateSndQuotation,
            CanEditSndQuotation = e.CanEditSndQuotation,
            CanApproveSndQuotation = e.CanApproveSndQuotation,
            CanConvertSndQuotationToOrder = e.CanConvertSndQuotationToOrder,
            CanConvertSndQuotationToInvoice = e.CanConvertSndQuotationToInvoice,
            CanReviseSndQuotation = e.CanReviseSndQuotation,
            CanVoidSndQuotation = e.CanVoidSndQuotation,
                  CanConvertSndQuotationToDeliveryNote = e.CanConvertSndQuotationToDeliveryNote,
                  CanConvertSndDeliveryNoteToInvoice = e.CanConvertSndDeliveryNoteToInvoice,



            UserName = userNames.FirstOrDefault(e => e.Id == request.User.UserId).UserName,
        }).Where(e => e.UserName.Contains(search) ||
              
              search == "" || search == null
              || e.BranchCode.Contains(search) || e.AppAuth.ToString().Contains(search)
              ||e.UserName.Contains(search)
              )
             
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion


    #region GetAuthorityById
    public class GetAuthorityById : IRequest<TblSndAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAuthorityByIdHandler : IRequestHandler<GetAuthorityById, TblSndAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndAuthoritiesDto> Handle(GetAuthorityById request, CancellationToken cancellationToken)
        {

            var Authority = await _context.TblSndAuthoritiesList.AsNoTracking().ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return Authority;
        }
    }
    #region GetAuthorityByBranchUserId
    public class GetAuthorityByBranchUserId : IRequest<TblSndAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public int UserId { get; set; }
    }

    public class GetAuthorityByBranchUserIdHandler : IRequestHandler<GetAuthorityByBranchUserId, TblSndAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByBranchUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndAuthoritiesDto> Handle(GetAuthorityByBranchUserId request, CancellationToken cancellationToken)
        {

            var Authority = await _context.TblSndAuthoritiesList.AsNoTracking().ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.AppAuth == request.UserId);
            return Authority;
        }
    }

    #endregion

    #region GetAuthorityByBranchCurrentUser
    public class GetAuthorityByBranchCurrentUser : IRequest<TblSndAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetAuthorityByBranchCurrentUserIdHandler : IRequestHandler<GetAuthorityByBranchCurrentUser, TblSndAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByBranchCurrentUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndAuthoritiesDto> Handle(GetAuthorityByBranchCurrentUser request, CancellationToken cancellationToken)
        {

            var Authority = await _context.TblSndAuthoritiesList.AsNoTracking().ProjectTo<TblSndAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.AppAuth == request.User.UserId);
            return Authority;
        }
    }

    #endregion
    #endregion
}