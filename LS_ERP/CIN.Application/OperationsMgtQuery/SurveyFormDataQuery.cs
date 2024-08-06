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



    #region CreateUpdateSurveyFormData
    public class CreateSurveyFormData : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public EditableSurveyFormDataDto Input { get; set; }
    }

    public class CreateSurveyFormDataHandler : IRequestHandler<CreateSurveyFormData, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSurveyFormDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSurveyFormData request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSurveyFormData method start----");
                    if (request.Input.SurveyFormDataEntries.Count() > 0)
                    {
                        var oldEntries = await _context.OprSurveyFormDataEntries.Where(e => e.EnquiryID == request.Input.EnquiryID).ToListAsync();
                        _context.OprSurveyFormDataEntries.RemoveRange(oldEntries);

                        List<TblSndDefSurveyFormDataEntry> newEntriesList = new();
                        foreach (var ent in request.Input.SurveyFormDataEntries)
                        {
                            TblSndDefSurveyFormDataEntry entry = new()
                            {
                                EnquiryID = request.Input.EnquiryID,
                                ElementArbName = ent.ElementArbName,
                                ElementEngName = ent.ElementEngName,
                                ElementType = ent.ElementType,
                                ListValueString = ent.ListValueString,
                                MaxValue = ent.MaxValue,
                                MinValue = ent.MinValue,
                                EntryValue = ent.EntryValue
                            };
                            newEntriesList.Add(entry);
                        }
                        await _context.OprSurveyFormDataEntries.AddRangeAsync(newEntriesList);
                        TblSndDefServiceEnquiries enquiry = new();
                        enquiry = await _context.OprEnquiries.FirstOrDefaultAsync(e => e.EnquiryID == request.Input.EnquiryID);
                        if (request.Input.Action == "Save")
                        {
                            enquiry.IsSurveyCompleted = false;
                            enquiry.IsSurveyInProgress = true;
                            //enquiry.StatusEnquiry = "Survey_In_Progress";

                        }
                        else if (request.Input.Action == "Complete_Survey")
                        {
                            enquiry.IsSurveyCompleted = true;
                            enquiry.IsSurveyInProgress = false;

                            //enquiry.StatusEnquiry = "Survey_Completed";
                        }
                        //else
                        //    enquiry.StatusEnquiry = "Assigned_Surveyor";
                        _context.OprEnquiries.Update(enquiry);

                        await _context.SaveChangesAsync();

                       // var enqHead= _context.OprEnquiryHeaders.FirstOrDefault(e=>e.EnquiryNumber==enquiry.EnquiryNumber);
                        
                       // var temp = _context.OprEnquiries.FirstOrDefault(e => e.StatusEnquiry != enquiry.StatusEnquiry  && e.EnquiryNumber== enqHead.EnquiryNumber);
                       // if (temp == null)
                       //     enqHead.StusEnquiryHead = enquiry.StatusEnquiry;
                       //else     enqHead.StusEnquiryHead = "Survey_In_Progress";
                       // _context.OprEnquiryHeaders.Update(enqHead);
                       //  await _context.SaveChangesAsync();
                    }
                    Log.Info("----Info CreateUpdateSurveyFormData method Exit----");

                    await transaction.CommitAsync();
                    return request.Input.EnquiryID;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateSurveyFormData Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }

    #endregion

    #region GetSurveyFormDataByEnquiryId
    public class GetSurveyFormDataByEnquiryId : IRequest<EditableSurveyFormDataDto>
    {
        public UserIdentityDto User { get; set; }
        public int EnquiryID { get; set; }
    }

    public class GetSurveyFormDataByEnquiryIdHandler : IRequestHandler<GetSurveyFormDataByEnquiryId, EditableSurveyFormDataDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormDataByEnquiryIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<EditableSurveyFormDataDto> Handle(GetSurveyFormDataByEnquiryId request, CancellationToken cancellationToken)
        {
            try
            {

                EditableSurveyFormDataDto obj = new();

                var enquiry = await _context.OprEnquiries.AsNoTracking()
                    .OrderByDescending(e => e.EnquiryID == request.EnquiryID)
                   .ProjectTo<TblSndDefServiceEnquiriesDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(cancellationToken) ?? new();
                if (enquiry is not null)
                {
                    obj.Enquiry = enquiry;
                    var surveyor = await _context.OprSurveyors.AsNoTracking().OrderByDescending(e => e.SurveyorCode == enquiry.SurveyorCode).ProjectTo<TblSndDefSurveyorDto>(_mapper.ConfigurationProvider)
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

                            //if (enquiry.StatusEnquiry != "Survey_In_Progress" && enquiry.StatusEnquiry != "Survey_Completed")
                            if (!enquiry.IsSurveyInProgress && !enquiry.IsSurveyCompleted)
                            {

                                var entries = (from elmp in _context.OprSurveyFormElementsMapp
                                               join el in _context.OprSurveyFormElements on elmp.FormElementCode equals el.FormElementCode
                                               where elmp.SurveyFormCode == surveyFormHead.SurveyFormCode
                                               select new TblSndDefSurveyFormDataEntryDto
                                               { 
                                                   EnquiryID = request.EnquiryID,
                                                   ElementArbName = el.ElementArbName,
                                                   ElementEngName = el.ElementEngName,
                                                   ElementType = el.ElementType,
                                                   FormElementCode = el.FormElementCode,
                                                   ListValueString = el.ListValueString,
                                                   MaxValue = el.MaxValue,
                                                   MinValue = el.MinValue,
                                                   EntryValue = "",
                                               }).ToList();
                                obj.SurveyFormDataEntries = entries;

                            }

                            else
                            {
                                var entries = await _context.OprSurveyFormDataEntries.AsNoTracking().ProjectTo<TblSndDefSurveyFormDataEntryDto>(_mapper.ConfigurationProvider).Where(e => e.EnquiryID == request.EnquiryID).ToListAsync();
                                obj.SurveyFormDataEntries = entries;
                            } 
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
                        return obj;
                    }
                    else return null;
                   
                }
                else return null;
            }

            catch (Exception ex)
            {
                Log.Error("Error in GetSurveyFormDataByEnquiryId Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }


    #endregion
}
