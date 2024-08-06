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
using CIN.Domain.SystemSetup;

namespace CIN.Application.OperationsMgtQuery
{


    #region CreateUpdateProposalTemplate
    public class CreateProposalTemplate : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProposalTemplateDto ProposalTemplateDto { get; set; }
    }

    public class CreateProposalTemplateHandler : IRequestHandler<CreateProposalTemplate, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProposalTemplateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateProposalTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateProposalTemplate method start----");



                var obj = request.ProposalTemplateDto;


                TblOpProposalTemplate ProposalTemplate = new();
                if (obj.Id > 0)
                {
                   
                    ProposalTemplate = await _context.TblOpProposalTemplates.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    if (ProposalTemplate.CreatedBy != request.User.UserId)
                    {
                        return -1;
                    }
                }
                ProposalTemplate.ProjectCode = obj.ProjectCode;
                ProposalTemplate.SiteCode = obj.SiteCode;
                ProposalTemplate.CustomerCode = obj.CustomerCode;

                ProposalTemplate.Commercial = obj.Commercial;
                ProposalTemplate.CommercialArb = obj.CommercialArb;
                ProposalTemplate.CoveringLetter = obj.CoveringLetter;
                ProposalTemplate.CoveringLetterArb = obj.CoveringLetterArb;
                ProposalTemplate.IssuingAuthority = obj.IssuingAuthority;
                ProposalTemplate.IssuingAuthorityArb = obj.IssuingAuthorityArb;
                ProposalTemplate.NotesArb = obj.NotesArb;
                ProposalTemplate.Notes = obj.Notes;
                ProposalTemplate.TitleOfService = obj.TitleOfService;
                ProposalTemplate.TitleOfServiceArb = obj.TitleOfServiceArb;
              
                if (obj.Id > 0)
                {
                    _context.TblOpProposalTemplates.Update(ProposalTemplate);
                }
                else
                {
                    ProposalTemplate.CreatedBy = request.User.UserId;
                    await _context.TblOpProposalTemplates.AddAsync(ProposalTemplate);
                }



                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateProposalTemplate method Exit----");
                return ProposalTemplate.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateProposalTemplate Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion


    #region GetProposalTemplateById
    public class GetProposalTemplateById : IRequest<TblOpProposalTemplate>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetProposalTemplateByIdHandler : IRequestHandler<GetProposalTemplateById, TblOpProposalTemplate>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProposalTemplateByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProposalTemplate> Handle(GetProposalTemplateById request, CancellationToken cancellationToken)
        {
          
            var template = await _context.TblOpProposalTemplates.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                return template;
       
        }
    }

    #endregion






    #region GetProposalTemplatesSelectionList
    public class GetProposalTemplatesSelectionList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        //public string CustomerCode { get; set; }
        //public string ProjectCode { get; set; }
        //public string SiteCode { get; set; }

        public InputCustomerProjectSite Input { get; set; }
    }

    public class GetProposalTemplatesSelectionListHandler : IRequestHandler<GetProposalTemplatesSelectionList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProposalTemplatesSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetProposalTemplatesSelectionList request, CancellationToken cancellationToken)
        {
            
           var Templates = await _context.TblOpProposalTemplates.AsNoTracking().OrderByDescending(e => e.Id).Where(x=>x.ProjectCode==request.Input.ProjectCode&& (x.SiteCode==request.Input.SiteCode|| request.Input.SiteCode==null) && x.CustomerCode==request.Input.CustomerCode)
               .Select(e => new CustomSelectListItem { Text = e.TitleOfService, Value = e.Id.ToString(),TextTwo=e.TitleOfServiceArb })
                  .ToListAsync(cancellationToken);

            if (Templates.Count == 0)
            {
                Templates= await _context.TblOpProposalTemplates.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TitleOfService, Value = e.Id.ToString(), TextTwo = e.TitleOfServiceArb })
                  .ToListAsync();
            }

            return Templates;

        }
    }

    #endregion








}
