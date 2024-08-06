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


    #region CreateUpdateContractFormTemplate
    public class CreateContractFormTemplate : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public ContractFormTemplateDto ContractFormTemplateDto { get; set; }
    }

    public class CreateContractFormTemplateHandler : IRequestHandler<CreateContractFormTemplate, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateContractFormTemplateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateContractFormTemplate request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateContractFormTemplate method start----");

                    TblOpContractTemplateDto inputHead = request.ContractFormTemplateDto.TemplateHead;
                    List<TblOpContractClauseDto> inputClausesMaps = request.ContractFormTemplateDto.Clauses;
                    List<TblOpContractTemplateToContractClauseMap> ExistingClausesmapping = new();
                    List<TblOpContractTemplateToContractClauseMap> RemoveClausesmapping = new();

                    TblOpContractTemplate contractFormTemplateHead = await _context.TblOpContractTemplates.FirstOrDefaultAsync(e => e.Id == inputHead.Id)??new();

                    contractFormTemplateHead.CompanyDetailsEng = inputHead.CompanyDetailsEng;
                    contractFormTemplateHead.CompanyDetailsArb = inputHead.CompanyDetailsArb;
                    contractFormTemplateHead.CustomerDetailsArb = inputHead.CustomerDetailsArb;
                    contractFormTemplateHead.CustomerDetailsEng = inputHead.CustomerDetailsEng;
                   
                    contractFormTemplateHead.PreambleEng = inputHead.PreambleEng;
                    contractFormTemplateHead.PreambleArb = inputHead.PreambleArb;
                    contractFormTemplateHead.FirstPartyArb = inputHead.FirstPartyArb;
                    contractFormTemplateHead.FirstPartyEng = inputHead.FirstPartyEng;
                    contractFormTemplateHead.SecondPartyArb = inputHead.SecondPartyArb;
                    contractFormTemplateHead.SecondPartyEng = inputHead.SecondPartyEng;
                    contractFormTemplateHead.TitleOfServiceArb = inputHead.TitleOfServiceArb;
                    contractFormTemplateHead.TitleOfServiceEng = inputHead.TitleOfServiceEng;


                    contractFormTemplateHead.IsForProject = inputHead.IsForProject ?? false;
                    contractFormTemplateHead.IsForAddingResources = inputHead.IsForAddingResources??false;
                    contractFormTemplateHead.IsForAddingSite = inputHead.IsForAddingSite ?? false;
                    contractFormTemplateHead.IsForRemovingResources = inputHead.IsForRemovingResources ?? false;
                    

                    if (inputHead.Id > 0)
                    {

                        _context.TblOpContractTemplates.Update(contractFormTemplateHead);
                        await _context.SaveChangesAsync();
                         ExistingClausesmapping =await _context.tblOpContractTemplateToContractClauseMaps.AsNoTracking().Where(e => e.ContractTemplateId == contractFormTemplateHead.Id).ToListAsync();
                     
                    }
                    else
                    {
                        contractFormTemplateHead.CreatedBy = request.User.UserId;
                        await _context.TblOpContractTemplates.AddAsync(contractFormTemplateHead);
                        await _context.SaveChangesAsync();
                    }


                    if (inputClausesMaps.Count > 0)
                    {
                        if (inputHead.Id > 0)
                        {
                            List<TblOpContractTemplateToContractClauseMap> newClausesMaps = new();
                            List<TblOpContractTemplateToContractClauseMap> existingClausesMaps = new();
                            for (int i = 0; i < inputClausesMaps.Count; i++)
                            {
                                TblOpContractTemplateToContractClauseMap clauseMap = new()
                                {
                                    ContractClauseId = inputClausesMaps[i].Id,
                                    ContractTemplateId = contractFormTemplateHead.Id,
                                    SerialNumber = inputClausesMaps[i].SerialNumber
                                };


                                if (inputClausesMaps[i].MappingId == 0)
                                {
                                    newClausesMaps.Add(clauseMap);

                                }
                                else
                                {
                                    clauseMap.Id = inputClausesMaps[i].MappingId;
                                    existingClausesMaps.Add(clauseMap);

                                }

                            }

                            if (newClausesMaps.Count > 0)
                            {
                                await _context.tblOpContractTemplateToContractClauseMaps.AddRangeAsync(newClausesMaps);
                                await _context.SaveChangesAsync();
                            }
                            if (existingClausesMaps.Count > 0)
                            {
                                _context.tblOpContractTemplateToContractClauseMaps.UpdateRange(existingClausesMaps);
                                await _context.SaveChangesAsync();
                            }


                            if (ExistingClausesmapping.Count > 0)
                            {
                                for (int i = 0; i < ExistingClausesmapping.Count; i++)
                                {
                                    if (!inputClausesMaps.Any(e => e.MappingId == ExistingClausesmapping[i].Id))
                                    {
                                        RemoveClausesmapping.Add(ExistingClausesmapping[i]);
                                    }

                                }

                                if (RemoveClausesmapping.Count > 0)
                                {
                                    _context.tblOpContractTemplateToContractClauseMaps.RemoveRange(RemoveClausesmapping);
                                    await _context.SaveChangesAsync();
                                }


                            }

                        }
                        else
                        {
                            List<TblOpContractTemplateToContractClauseMap> newClausesMaps = new();
                            for (int i = 0; i < inputClausesMaps.Count; i++)
                            {
                                TblOpContractTemplateToContractClauseMap clauseMap = new()
                                {
                                    Id=0,
                                    ContractClauseId = inputClausesMaps[i].Id,
                                    ContractTemplateId = contractFormTemplateHead.Id,
                                    SerialNumber = inputClausesMaps[i].SerialNumber,
                                };

                                    newClausesMaps.Add(clauseMap);
                            }
                            await _context.tblOpContractTemplateToContractClauseMaps.AddRangeAsync(newClausesMaps);
                            await _context.SaveChangesAsync();
                        }

                    }


                    await transaction.CommitAsync();
                    return contractFormTemplateHead.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateContractFormTemplate Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion


    #region GetContractFormTemplateById
    public class GetContractFormTemplateById : IRequest<ContractFormTemplateDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetContractFormTemplateByIdHandler : IRequestHandler<GetContractFormTemplateById, ContractFormTemplateDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplateByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractFormTemplateDto> Handle(GetContractFormTemplateById request, CancellationToken cancellationToken)
        {
            var contractClauses = _context.TblOpContractClauses.AsNoTracking();
            ContractFormTemplateDto template = new() { TemplateId=request.Id};

            TblOpContractTemplateDto TemplateHead = new();
            List<TblOpContractTemplateToContractClauseMapDto> ClausesMappings = new();
            List<TblOpContractClauseDto> Clauses = new();
            TemplateHead = await _context.TblOpContractTemplates.ProjectTo<TblOpContractTemplateDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e=>e.Id==request.Id);
            if (TemplateHead.Id>0)
            {
                ClausesMappings = await _context.tblOpContractTemplateToContractClauseMaps.ProjectTo<TblOpContractTemplateToContractClauseMapDto>(_mapper.ConfigurationProvider).Where(e=>e.ContractTemplateId==TemplateHead.Id).ToListAsync();
                if (ClausesMappings.Count>0)
                {
                    ClausesMappings.ForEach(e => {
                        Clauses.Add(new() { 
                       ClauseDescriptionArb= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseDescriptionArb,
                        ClauseDescriptionEng= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseDescriptionEng,
                         ClauseSubTitleArb= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseSubTitleArb,
                          ClauseSubTitleEng= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseSubTitleEng,
                           ClauseTitleArb= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseTitleArb,
                            ClauseTitleEng= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseTitleEng,
                             NumberArb= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).NumberArb,
                              NumberEng= contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).NumberEng,
                              Id= e.ContractClauseId,
                              SerialNumber=e.SerialNumber,
                              MappingId=e.Id,
                             
                        }); 
                    });


                }
                
            }
            template.TemplateHead = TemplateHead;
            template.Clauses = Clauses;
            return template;

        }
    }

    #endregion






    #region GetContractFormTemplatesSelectionList
    public class GetContractFormTemplatesSelectionList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Type { get; set; }
      
    }

    public class GetContractFormTemplatesSelectionListHandler : IRequestHandler<GetContractFormTemplatesSelectionList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplatesSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetContractFormTemplatesSelectionList request, CancellationToken cancellationToken)
        {
            string type = request.Type;
            
           var Templates = await _context.TblOpContractTemplates.Where(e=>(type == "ForProject" && e.IsForProject==true)
           ||(type == "ForAddingSite" && e.IsForAddingSite == true)
           ||(type == "ForAddingResources" && e.IsForAddingResources == true)
           ||(type == "ForRemovingResources" && e.IsForRemovingResources == true
           || string.IsNullOrEmpty(type))
           ).OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.TitleOfServiceEng, Value = e.Id.ToString(),TextTwo=e.TitleOfServiceArb })
                  .ToListAsync(cancellationToken);
            return Templates;

        }
    }

    #endregion



    #region GetContractFormTemplatesPagedList

    public class GetContractFormTemplatesPagedList : IRequest<PaginatedList<ContractTemplatesPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetContractFormTemplatesPagedListHandler : IRequestHandler<GetContractFormTemplatesPagedList, PaginatedList<ContractTemplatesPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplatesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ContractTemplatesPaginationDto>> Handle(GetContractFormTemplatesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpContractTemplates.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.TitleOfServiceArb.Contains(search) ||
                            e.TitleOfServiceEng.Contains(search) ||
                            e.FirstPartyArb.Contains(search) || e.FirstPartyEng.Contains(search) ||
                            e.SecondPartyArb.Contains(search) || e.SecondPartyEng.Contains(search) ||
                            e.PreambleEng.Contains(search) || e.PreambleArb.Contains(search) ||
                            e.CompanyDetailsArb.Contains(search) || e.CompanyDetailsEng.Contains(search) ||
                            e.CustomerDetailsArb.Contains(search) || e.CustomerDetailsEng.Contains(search) ||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy).
               Select(s=>new ContractTemplatesPaginationDto{TitleOfServiceEng=s.TitleOfServiceEng,
               TitleOfServiceArb=s.TitleOfServiceArb,
               CanEdit=s.CreatedBy==request.User.UserId,
               Id=s.Id})              
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion




}
