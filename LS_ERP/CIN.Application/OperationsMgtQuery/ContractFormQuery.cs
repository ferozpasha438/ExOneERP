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


    #region CreateUpdateCreateContractForm
    public class CreateContractForm : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public ContractFormDto contractFormDto { get; set; }
    }

    public class CreateContractFormHandler : IRequestHandler<CreateContractForm, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateContractFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateContractForm request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {


                    Log.Info("----Info CreateUpdateCreateContractForm method start----");
                    TblOpContractFormHeadDto inputHead = request.contractFormDto.ContractFormHead;
                    List<TblOpContractClausesToContractFormMapDto> inputClauses = request.contractFormDto.ContractClauses;
                    List<TblOpContractClausesToContractFormMap> ExistingClausesmappings = new();
                    List<TblOpContractClausesToContractFormMap> ClausesmappingToBeUpdate = new();
                    List<TblOpContractClausesToContractFormMap> ClausesmappingTobeRemove = new();
                    TblOpContractFormHead contractFormHead = new();
                    if (inputHead.Id>0)
                     contractFormHead = await _context.TblOpContractFormHeads.FirstOrDefaultAsync(e=>e.Id==inputHead.Id);
       

                    contractFormHead.CompanyDetailsEng = inputHead.CompanyDetailsEng;
                    contractFormHead.CompanyDetailsArb = inputHead.CompanyDetailsArb;
                    contractFormHead.CustomerDetailsArb = inputHead.CustomerDetailsArb;
                    contractFormHead.CustomerDetailsEng = inputHead.CustomerDetailsEng;
                    contractFormHead.SiteCode = inputHead.SiteCode;
                    contractFormHead.CustomerCode = inputHead.CustomerCode;
                    contractFormHead.ProjectCode = inputHead.ProjectCode;
                    contractFormHead.PreambleEng = inputHead.PreambleEng;
                    contractFormHead.PreambleArb = inputHead.PreambleArb;
                    contractFormHead.FirstPartyArb = inputHead.FirstPartyArb;
                    contractFormHead.FirstPartyEng = inputHead.FirstPartyEng;
                    contractFormHead.SecondPartyArb = inputHead.SecondPartyArb;
                    contractFormHead.SecondPartyEng = inputHead.SecondPartyEng;
                    contractFormHead.TitleOfServiceArb = inputHead.TitleOfServiceArb;
                    contractFormHead.TitleOfServiceEng = inputHead.TitleOfServiceEng;
                    contractFormHead.IsApproved = false;

                    if (inputHead.Id > 0)
                    {

                        _context.TblOpContractFormHeads.Update(contractFormHead);
                        await _context.SaveChangesAsync();
                         ExistingClausesmappings = _context.TblOpContractClausesToContractFormMap.AsNoTracking().Where(e => e.ContractFormId == contractFormHead.Id).ToList();
                        //_context.TblOpContractClausesToContractFormMap.RemoveRange(ExistingClausesmapping);
                        //await _context.SaveChangesAsync();
                    }
                    else
                    {
                        contractFormHead.CreatedBy = request.User.UserId;
                        await _context.TblOpContractFormHeads.AddAsync(contractFormHead);
                        await _context.SaveChangesAsync();
                    }


                    if (inputClauses.Count>0)
                    {


                        List<TblOpContractClausesToContractFormMap> newMappings = new();
                        foreach (var e in inputClauses)
                        {
                            
                                TblOpContractClausesToContractFormMap map = new()
                                {
                                    ContractFormId = contractFormHead.Id,
                                    ClauseDescriptionArb = e.ClauseDescriptionArb,
                                    ClauseDescriptionEng = e.ClauseDescriptionEng,
                                    ClauseSubTitleArb = e.ClauseSubTitleArb,
                                    ClauseSubTitleEng = e.ClauseSubTitleEng,
                                    ClauseTitleArb = e.ClauseTitleArb,
                                    ClauseTitleEng = e.ClauseTitleEng,
                                    NumberArb = e.NumberArb,
                                    NumberEng = e.NumberEng,
                                    SerialNumber = e.SerialNumber
                                };


                            if (e.Id == 0)
                            {
                                newMappings.Add(map);
                            }
                            else if (ExistingClausesmappings.Any(x => x.Id == e.Id))
                            {
                                map.Id = e.Id;
                                ClausesmappingToBeUpdate.Add(map);

                            }
                           
                        }

                        if (ExistingClausesmappings.Count > 0)
                        {
                            foreach (var e in ExistingClausesmappings)
                            {
                                if (!inputClauses.Any(x => x.Id == e.Id && x.Id != 0))
                                {
                                    ClausesmappingTobeRemove.Add(e);

                                }

                            }


                        }





                        if (newMappings.Count>0)
                        {
                            await _context.TblOpContractClausesToContractFormMap.AddRangeAsync(newMappings);
                            await _context.SaveChangesAsync();
                        }
                        if (ClausesmappingTobeRemove.Count>0)
                        {
                            _context.TblOpContractClausesToContractFormMap.RemoveRange(ClausesmappingTobeRemove);
                            await _context.SaveChangesAsync();
                        }
                        if (ClausesmappingToBeUpdate.Count>0)
                        {
                            _context.TblOpContractClausesToContractFormMap.UpdateRange(ClausesmappingToBeUpdate);
                            await _context.SaveChangesAsync();
                        }
                        
                    }

                  
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateCreateContractForm method Exit----");
                    return contractFormHead.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();                        
                    Log.Error("Error in CreateUpdateCreateContractForm Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion


   





    #region GetContractForm
    public class GetContractForm : IRequest<ContractFormDto>
    {
        public UserIdentityDto User { get; set; }
        //public string CustomerCode { get; set; }
        //public string ProjectCode { get; set; }
        //public string SiteCode { get; set; }

       public InputCustomerProjectSite Input { get; set; }
    }

    public class GetContractFormHandler : IRequestHandler<GetContractForm, ContractFormDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractFormDto> Handle(GetContractForm request, CancellationToken cancellationToken)
        {
            ContractFormDto contractForm = new() { };
            TblOpContractFormHeadDto contractFormHead = request.Input.SiteCode == "" || request.Input.SiteCode is null || string.IsNullOrEmpty(request.Input.SiteCode)|| request.Input.SiteCode=="-NA-"
                ? await _context.TblOpContractFormHeads.ProjectTo<TblOpContractFormHeadDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e =>
                e.ProjectCode == request.Input.ProjectCode

                && e.CustomerCode == request.Input.CustomerCode) :
            await _context.TblOpContractFormHeads.ProjectTo<TblOpContractFormHeadDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e =>
            e.ProjectCode == request.Input.ProjectCode
            && e.SiteCode == request.Input.SiteCode
            && e.CustomerCode == request.Input.CustomerCode);


            if (contractFormHead is not null)
            {
                contractForm.ContractFormHead = contractFormHead;
                contractForm.ContractClauses = await _context.TblOpContractClausesToContractFormMap.AsNoTracking().OrderBy(e=>e.SerialNumber).ProjectTo<TblOpContractClausesToContractFormMapDto>(_mapper.ConfigurationProvider).Where(e => e.ContractFormId == contractFormHead.Id).ToListAsync();

            }
            else
            {
                var DefaultTemplate =await _context.TblOpContractTemplates.FirstOrDefaultAsync();
                var defaultClausesMaps = await _context.tblOpContractTemplateToContractClauseMaps.AsNoTracking().OrderBy(e=>e.SerialNumber).Where(e=>e.ContractTemplateId==DefaultTemplate.Id).ToListAsync();
               var contractClauses=_context.TblOpContractClauses.AsNoTracking();



                contractForm.ContractFormHead = new()
                {
                    Id=0,
                CompanyDetailsEng = DefaultTemplate.CompanyDetailsEng,
                CompanyDetailsArb = DefaultTemplate.CompanyDetailsArb,
                CustomerDetailsArb = DefaultTemplate.CustomerDetailsArb,
                CustomerDetailsEng = DefaultTemplate.CustomerDetailsEng,

                SiteCode = request.Input.SiteCode,
                CustomerCode = request.Input.CustomerCode,
                ProjectCode = request.Input.ProjectCode,

                PreambleEng = DefaultTemplate.PreambleEng,
                PreambleArb = DefaultTemplate.PreambleArb,
                FirstPartyArb = DefaultTemplate.FirstPartyArb,
                FirstPartyEng = DefaultTemplate.FirstPartyEng,
                SecondPartyArb = DefaultTemplate.SecondPartyArb,
                SecondPartyEng = DefaultTemplate.SecondPartyEng,
                TitleOfServiceArb = DefaultTemplate.TitleOfServiceArb,
                TitleOfServiceEng = DefaultTemplate.TitleOfServiceEng,
                IsApproved = false,
            };
                contractForm.ContractClauses = new();
                defaultClausesMaps.ForEach(e => {
                    TblOpContractClausesToContractFormMapDto contractClause = new() {
                        Id=e.ContractClauseId,
                        ClauseDescriptionArb = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseDescriptionArb,
                         ClauseDescriptionEng = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseDescriptionEng,
                          ClauseSubTitleArb = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseSubTitleArb,
                         ClauseSubTitleEng = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseSubTitleEng,
                         ClauseTitleArb = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseTitleArb,
                         ClauseTitleEng = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).ClauseTitleEng,
                         ContractFormId = 0,
                         NumberArb = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).NumberArb,
                         NumberEng = contractClauses.FirstOrDefault(c=>c.Id==e.ContractClauseId).NumberEng,
                          SerialNumber=e.SerialNumber,
                        MappingId=0
                    };

                    contractForm.ContractClauses.Add(contractClause);
                });
            }

            return contractForm;
        }

    }

    #endregion








}
