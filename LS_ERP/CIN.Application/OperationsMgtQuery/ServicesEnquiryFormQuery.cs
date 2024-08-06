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
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery
{



    #region CreateUpdateServicesEnquiryForm
    public class CreateServicesEnquiryForm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ServicesEnquiryFormDto ServicesEnquiryFormDto { get; set; }
    }

    public class CreateServicesEnquiryFormHandler : IRequestHandler<CreateServicesEnquiryForm, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateServicesEnquiryFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateServicesEnquiryForm request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateServicesEnquiryForm method start----");




                    var obj = request.ServicesEnquiryFormDto;
                    bool isConvertedToProject = false;
                   
                    TblSndDefServiceEnquiryHeader ServicesEnquiryHeader = new();
                    if (obj.Id > 0)
                    {
                        ServicesEnquiryHeader = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        isConvertedToProject = ServicesEnquiryHeader.IsConvertedToProject;
                        if(isConvertedToProject)
                        {
                            short version = ServicesEnquiryHeader.Version ?? 0;
                            ServicesEnquiryHeader.Version = short.Parse((version + 1).ToString());
                        }

                                            }
                    else
                    {
                        ServicesEnquiryHeader.Version = 1;
                        ServicesEnquiryHeader.BranchCode = request.User.BranchCode;

                        var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().Where(e=>e.CustomerCode==request.ServicesEnquiryFormDto.CustomerCode).OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                        if (EnqForm is not null)
                        {
                            ServicesEnquiryHeader.EnquiryNumber = request.ServicesEnquiryFormDto.CustomerCode + (EnqForm.Id + 1).ToString();
                        }
                        else
                            ServicesEnquiryHeader.EnquiryNumber = request.ServicesEnquiryFormDto.CustomerCode+"1";

                        if (_context.OprEnquiryHeaders.Any(x => x.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber))
                        {
                            return -1;
                        }


                        //  ServicesEnquiryHeader.EnquiryNumber = obj.EnquiryNumber.ToUpper();

                    }

                    //ServicesEnquiryForm.Id = obj.Id;
                    // ServicesEnquiryForm.ModifiedOn = obj.ModifiedOn;
                    // ServicesEnquiryForm.CreatedOn = obj.CreatedOn;
                    ServicesEnquiryHeader.IsActive = obj.IsActive;
                    // ServicesEnquiryForm.Id = obj.Id;

                    ServicesEnquiryHeader.CustomerCode = obj.CustomerCode;
                    ServicesEnquiryHeader.DateOfEnquiry = Convert.ToDateTime(obj.DateOfEnquiry, CultureInfo.InvariantCulture);
                    ServicesEnquiryHeader.EstimateClosingDate = Convert.ToDateTime(obj.EstimateClosingDate, CultureInfo.InvariantCulture);
                    ServicesEnquiryHeader.UserName = obj.UserName;
                    ServicesEnquiryHeader.TotalEstPrice = obj.TotalEstPrice;
                    ServicesEnquiryHeader.Remarks = obj.Remarks;
                    ServicesEnquiryHeader.StusEnquiryHead = obj.StusEnquiryHead;
                    ServicesEnquiryHeader.BranchCode = obj.BranchCode;




                    if (obj.Id > 0)
                    {
                        ServicesEnquiryHeader.IsConvertedToProject = false;
                        ServicesEnquiryHeader.ModifiedOn = DateTime.Now;
                        _context.OprEnquiryHeaders.Update(ServicesEnquiryHeader);
                    }
                    else
                    {


                        ServicesEnquiryHeader.CreatedOn = DateTime.Now;
                        await _context.OprEnquiryHeaders.AddAsync(ServicesEnquiryHeader);
                    }
                    await _context.SaveChangesAsync();

                    if (request.ServicesEnquiryFormDto.EnquiriesList.Count() > 0)
                    {
                        var oldEnquiriesList = await _context.OprEnquiries.Where(e => e.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber && !e.IsAssignedSurveyor).ToListAsync();
                        
                        
                        _context.OprEnquiries.RemoveRange(oldEnquiriesList);                //which are Assigned surveyors






                        List<TblSndDefServiceEnquiries> enquiriesList = new();
                        foreach (var enq in request.ServicesEnquiryFormDto.EnquiriesList)
                        {
                            if (!enq.IsAssignedSurveyor)
                            {
                                TblSndDefServiceEnquiries enquiry = new()
                                {
                                    EnquiryID = enq.EnquiryID,
                                    EnquiryNumber = ServicesEnquiryHeader.EnquiryNumber,
                                    SiteCode = enq.SiteCode,
                                    ServiceCode = enq.ServiceCode,
                                    UnitCode = enq.UnitCode,
                                    ServiceQuantity = enq.ServiceQuantity,
                                    UnitQuantity = enq.UnitQuantity,
                                    EstimatedPrice = enq.EstimatedPrice,
                                    PricePerUnit = enq.PricePerUnit,
                                    StatusEnquiry ="Open",
                                    IsApproved = false,
                                    IsAssignedSurveyor = false,
                                    IsSurveyCompleted = false,
                                    IsSurveyInProgress =false,
                                    //ModifiedOn = enq.ModifiedOn,
                                    IsActive =true,
                                    SurveyorCode = ""
                                };
                                if (enquiry.EnquiryID > 0 && !enquiry.IsAssignedSurveyor)
                                {
                                    enquiry.ModifiedOn = DateTime.Now;
                                    _context.OprEnquiries.Update(enquiry);
                                }
                                else if (enquiry.EnquiryID == 0)
                                {

                                    enquiry.CreatedOn = DateTime.Now;
                                    await _context.OprEnquiries.AddAsync(enquiry);
                                }

                                enquiriesList.Add(enquiry);

                            }

                           
                        }
                        await _context.OprEnquiries.AddRangeAsync(enquiriesList);
                        await _context.SaveChangesAsync();
                    }
                    Log.Info("----Info CreateUpdateServicesEnquiryForm method Exit----");

                    await transaction.CommitAsync();
                    return ServicesEnquiryHeader.Id;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateServicesEnquiryForm Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion



    
    
    #region changeEnquiryFormStatus
    public class ChangeEnquiryFormStatus : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public string EnquiryNumber { get; set; }
        public string EnquiryStatus { get; set; }
    }

    public class ChangeEnquiryFormStatusHandler : IRequestHandler<ChangeEnquiryFormStatus, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ChangeEnquiryFormStatusHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(ChangeEnquiryFormStatus request, CancellationToken cancellationToken)
        {
           
                try
                {


                TblSndDefServiceEnquiryHeader ServicesEnquiryHeader = new();

                ServicesEnquiryHeader = await _context.OprEnquiryHeaders.FirstOrDefaultAsync(e => e.EnquiryNumber == request.EnquiryNumber);

                ServicesEnquiryHeader.StusEnquiryHead = request.EnquiryStatus;


                        _context.OprEnquiryHeaders.Update(ServicesEnquiryHeader);
                   

                        
                    await _context.SaveChangesAsync();

                   
                    Log.Info("----Info ChangeEnquiryFormStatus method Exit----");

                    
                    return ApiMessageInfo.Status(1, ServicesEnquiryHeader.Id);

                }
                catch (Exception ex)
                {
                    Log.Error("Error in ChangeEnquiryFormStatus Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);

                }
            }
        
    }

    #endregion


    #region GetEnquiryFormByEnquiryNumber
    public class GetEnquiryFormByEnquiryNumber : IRequest<ServicesEnquiryFormDto>
    {
        public UserIdentityDto User { get; set; }
        public string EnquiryNumber { get; set; }
    }

    public class GetEnquiryFormByEnquiryNumberHandler : IRequestHandler<GetEnquiryFormByEnquiryNumber, ServicesEnquiryFormDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiryFormByEnquiryNumberHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServicesEnquiryFormDto> Handle(GetEnquiryFormByEnquiryNumber request, CancellationToken cancellationToken)
        {
            ServicesEnquiryFormDto obj = new();

            var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.EnquiryNumber == request.EnquiryNumber);
            
            if (EnqForm is not null)

            {
                var enquiries = await _context.OprEnquiries.AsNoTracking()
                .Where(e => e.EnquiryNumber == request.EnquiryNumber)
                .Select(e => new TblSndDefServiceEnquiriesDto
                {
                    EnquiryID = e.EnquiryID,
                    SiteCode = e.SiteCode,
                    ServiceCode = e.ServiceCode,
                    UnitCode = e.UnitCode,
                    ServiceQuantity = e.ServiceQuantity,
                    PricePerUnit = e.PricePerUnit,
                    EstimatedPrice = e.EstimatedPrice,
                    SurveyorCode = e.SurveyorCode,
                    StatusEnquiry = e.StatusEnquiry
                }).ToListAsync();

                obj.Id = EnqForm.Id;
                obj.CustomerCode = EnqForm.CustomerCode;
                obj.DateOfEnquiry = EnqForm.DateOfEnquiry;
                obj.EstimateClosingDate = EnqForm.EstimateClosingDate;
                obj.UserName = EnqForm.UserName;
                obj.TotalEstPrice = EnqForm.TotalEstPrice;
                obj.Remarks = EnqForm.Remarks;
                obj.StusEnquiryHead = EnqForm.StusEnquiryHead;
              
                obj.ModifiedOn = EnqForm.ModifiedOn;
                obj.CreatedOn = EnqForm.CreatedOn;
                // obj.DefaultBaseUnit = Service.DefaultBaseUnit;
                //obj.EstimatedServicesPricePerBaseUnit = Service.EstimatedServicesPricePerBaseUnit;
                obj.IsActive = EnqForm.IsActive;
                obj.EnquiriesList = enquiries;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetEnquiryFormById
    public class GetEnquiryFormById : IRequest<ServicesEnquiryFormDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetEnquiryFormByIdHandler : IRequestHandler<GetEnquiryFormById, ServicesEnquiryFormDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiryFormByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServicesEnquiryFormDto> Handle(GetEnquiryFormById request, CancellationToken cancellationToken)
        {
            ServicesEnquiryFormDto obj = new();

            var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);

            if (EnqForm is not null)

            {
                var enquiries = await _context.OprEnquiries.AsNoTracking()
                .Where(e => e.EnquiryNumber == EnqForm.EnquiryNumber)
                .Select(e => new TblSndDefServiceEnquiriesDto
                {
                    EnquiryNumber = e.EnquiryNumber,
                    SiteCode = e.SiteCode,
                    ServiceCode = e.ServiceCode,
                    UnitCode = e.UnitCode,
                    ServiceQuantity = e.ServiceQuantity,
                    PricePerUnit = e.PricePerUnit,
                    EstimatedPrice = e.EstimatedPrice,
                    SurveyorCode = e.SurveyorCode,
                    StatusEnquiry = e.StatusEnquiry
                }).ToListAsync();

                obj.Id = EnqForm.Id;
                obj.CustomerCode = EnqForm.CustomerCode;
                obj.DateOfEnquiry = EnqForm.DateOfEnquiry;
                obj.EstimateClosingDate = EnqForm.EstimateClosingDate;
                obj.UserName = EnqForm.UserName;
                obj.TotalEstPrice = EnqForm.TotalEstPrice;
                obj.Remarks = EnqForm.Remarks;
                obj.StusEnquiryHead = EnqForm.StusEnquiryHead;

                obj.ModifiedOn = EnqForm.ModifiedOn;
                obj.CreatedOn = EnqForm.CreatedOn;
                // obj.DefaultBaseUnit = Service.DefaultBaseUnit;
                //obj.EstimatedServicesPricePerBaseUnit = Service.EstimatedServicesPricePerBaseUnit;
                obj.IsActive = EnqForm.IsActive;
                obj.EnquiriesList = enquiries;
                return obj;
            }
            else return null;
        }
    }

    #endregion




}
