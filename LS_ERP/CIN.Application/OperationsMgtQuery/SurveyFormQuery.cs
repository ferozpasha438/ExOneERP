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



    #region CreateUpdateSurveyForm
    public class CreateSurveyForm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSurveyFormDto SurveyFormDto { get; set; }
    }

    public class CreateSurveyFormHandler : IRequestHandler<CreateSurveyForm, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSurveyFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSurveyForm request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSurveyForm method start----");




                    var obj = request.SurveyFormDto;


                    TblSndDefSurveyFormHead SurveyFormHeader = new();
                    if (obj.Id > 0)
                        SurveyFormHeader = await _context.OprSurveyFormHeads.FirstOrDefaultAsync(e => e.Id == obj.Id);


                    else
                    {

                        var sf = await _context.OprSurveyFormHeads.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                        if (sf is not null)
                        {
                            SurveyFormHeader.SurveyFormCode = "FORM" + (sf.Id + 1).ToString().PadLeft(6, '0');
                        }
                        else
                            SurveyFormHeader.SurveyFormCode = "FORM000001";

                        if (_context.OprSurveyFormHeads.Any(x => x.SurveyFormCode == SurveyFormHeader.SurveyFormCode))
                        {
                            return -1;
                        }






                        //if (_context.OprSurveyFormHeads.Any(x => x.SurveyFormCode == obj.SurveyFormCode))
                        //{
                        //    return -1;
                        //}

                        //SurveyFormHeader.SurveyFormCode = obj.SurveyFormCode.ToUpper();
                    }

                    SurveyFormHeader.IsActive = true;
                    SurveyFormHeader.Id = obj.Id;
                  
                    SurveyFormHeader.SurveyFormNameArb = obj.SurveyFormNameArb;
                    SurveyFormHeader.SurveyFormNameEng = obj.SurveyFormNameEng;
                    SurveyFormHeader.Remarks = obj.Remarks;
                    if (obj.Id > 0)
                    {
                        SurveyFormHeader.ModifiedOn = DateTime.Now;
                        _context.OprSurveyFormHeads.Update(SurveyFormHeader);
                    }
                    else
                    {

                        SurveyFormHeader.CreatedOn = DateTime.Now;
                        await _context.OprSurveyFormHeads.AddAsync(SurveyFormHeader);
                    }
                    await _context.SaveChangesAsync();

                    if (request.SurveyFormDto.ElementsList.Count() > 0)
                    {
                        var oldElementsList = await _context.OprSurveyFormElementsMapp.Where(e => e.SurveyFormCode == SurveyFormHeader.SurveyFormCode).ToListAsync();
                        _context.OprSurveyFormElementsMapp.RemoveRange(oldElementsList);

                        List<TblSndDefSurveyFormElementsMapp> elementsList = new();
                        foreach (var ele in request.SurveyFormDto.ElementsList)
                        {
                            TblSndDefSurveyFormElementsMapp element = new()
                            {
                                FormElementCode = ele.FormElementCode,
                                SurveyFormCode = SurveyFormHeader.SurveyFormCode

                            };
                            if (element.Id > 0)
                            {
                                _context.OprSurveyFormElementsMapp.Update(element);
                            }
                            else
                            {


                                await _context.OprSurveyFormElementsMapp.AddAsync(element);
                            }

                            elementsList.Add(element);
                        }
                        await _context.OprSurveyFormElementsMapp.AddRangeAsync(elementsList);
                        await _context.SaveChangesAsync();
                    }
                    Log.Info("----Info CreateUpdateServicesEnquiryForm method Exit----");

                    await transaction.CommitAsync();
                    return SurveyFormHeader.Id;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateSurveyForm Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }
    #endregion



    #region GetSurveyFormTemplateById
    public class GetSurveyFormTemplateById : IRequest<SurveyFormtemplateDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSurveyFormTemplateByIdHandler : IRequestHandler<GetSurveyFormTemplateById, SurveyFormtemplateDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormTemplateByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SurveyFormtemplateDto> Handle(GetSurveyFormTemplateById request, CancellationToken cancellationToken)
        {
            SurveyFormtemplateDto obj = new();
            var surFormHead = await _context.OprSurveyFormHeads.AsNoTracking().OrderByDescending(e => e.Id == request.Id)
                   .ProjectTo<TblSndDefSurveyFormHeadDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(cancellationToken) ?? new();


            if (surFormHead is not null)
            {
                obj.Head = surFormHead;
                var elementsList = (from elmp in _context.OprSurveyFormElementsMapp
                                    join el in _context.OprSurveyFormElements on elmp.FormElementCode equals el.FormElementCode
                                    where elmp.SurveyFormCode == surFormHead.SurveyFormCode
                                    select new TblSndDefSurveyFormElementDto
                                    {
                                        ElementArbName = el.ElementArbName,
                                        ElementEngName = el.ElementEngName,
                                        ElementType = el.ElementType,
                                        FormElementCode = el.FormElementCode,
                                        ListValueString = el.ListValueString,
                                        MaxValue = el.MaxValue,
                                        MinValue = el.MinValue,
                                    }).ToList();
                obj.ElementsList = elementsList;


                return obj;
            }
            else return null;
        }
    }


    #endregion

    #region GetPrintableSurveyFormByEnquiryId
    public class GetPrintableSurveyFormByEnquiryId : IRequest<PrintSurveyFormDto>
    {
        public UserIdentityDto User { get; set; }
        public int EnquiryID { get; set; }
    }

    public class GetPrintableSurveyFormByEnquiryIdHandler : IRequestHandler<GetPrintableSurveyFormByEnquiryId, PrintSurveyFormDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPrintableSurveyFormByEnquiryIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PrintSurveyFormDto> Handle(GetPrintableSurveyFormByEnquiryId request, CancellationToken cancellationToken)
        {
            try
            {

                PrintSurveyFormDto obj = new();
                
                var enquiry = await _context.OprEnquiries.AsNoTracking()
                    .OrderByDescending(e => e.EnquiryID == request.EnquiryID)
                   .ProjectTo<TblSndDefServiceEnquiriesDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(cancellationToken) ?? new();
                if (enquiry is not null)
                {
                   var surveyor= await _context.OprSurveyors.AsNoTracking().OrderByDescending(e=>e.SurveyorCode==enquiry.SurveyorCode).ProjectTo<TblSndDefSurveyorDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken) ?? new();
                    if (surveyor is not null)
                    {
                        obj.Surveyor = surveyor;
                    }
                    else return null;

                    var enquiryHead = await _context.OprEnquiryHeaders.AsNoTracking().OrderByDescending(e => e.EnquiryNumber == enquiry.EnquiryNumber)
              .ProjectTo<TblSndDefServiceEnquiryHeaderDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken) ?? new();
                    if (enquiryHead is not null)
                    {
                        obj.EnquiryHeader = enquiryHead;
                     
                        var customer = await _context.OprCustomers.AsNoTracking().OrderByDescending(e => e.CustCode == enquiryHead.CustomerCode)
               .ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken) ?? new();
                        if (customer is not null)
                        {
                            obj.Customer = customer;
                         }
                        else return null;
                    }
                    else return null;
                    
                    var service = await _context.OprServices.AsNoTracking()
                     .OrderByDescending(e => e.ServiceCode == enquiry.ServiceCode)
                    .ProjectTo<TblSndDefServiceMasterDto>(_mapper.ConfigurationProvider)
                       .FirstOrDefaultAsync(cancellationToken) ?? new();
                    if (service is not null)
                    {
                        obj.Service = service;
                        
                        var surveyFormHead = await _context.OprSurveyFormHeads.AsNoTracking().OrderByDescending(e => e.SurveyFormCode == service.SurveyFormCode)
                   .ProjectTo<TblSndDefSurveyFormHeadDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(cancellationToken) ?? new();

                        if (surveyFormHead is not null)
                        {
                            obj.SurveyFormHeader = surveyFormHead;
                            var elementsList = (from elmp in _context.OprSurveyFormElementsMapp
                                                join el in _context.OprSurveyFormElements on elmp.FormElementCode equals el.FormElementCode
                                                where elmp.SurveyFormCode==surveyFormHead.SurveyFormCode
                                                select new TblSndDefSurveyFormElementDto
                                                {
                                                    ElementArbName = el.ElementArbName,
                                                    ElementEngName = el.ElementEngName,
                                                    ElementType = el.ElementType,
                                                    FormElementCode = el.FormElementCode,
                                                    ListValueString = el.ListValueString,
                                                    MaxValue = el.MaxValue,
                                                    MinValue = el.MinValue,
                                                }).ToList();
                            obj.SurveyFormElements = elementsList;
                        }
                        else
                            return null;

                    }
                    else return null;
                    
                    var site = await _context.OprSites.AsNoTracking().OrderByDescending(e => e.SiteCode == enquiry.SiteCode)
               .ProjectTo<TblSndDefSiteMasterDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken) ?? new();
                    if (site is not null)
                    {
                        obj.Site = site;
                        
                    }
                    else return null;

                    return obj;
                }
                else return null;
            }

            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSurveyForm Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion
}










