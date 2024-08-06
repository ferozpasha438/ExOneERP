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

    #region Create_And_Update

    public class CreateUpdateSchoolFeeStructure : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolFeeStructureDto SchoolFeeStructureDto { get; set; }
    }

    public class CreateUpdateSchoolFeeStructureHandler : IRequestHandler<CreateUpdateSchoolFeeStructure, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolFeeStructureHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolFeeStructure request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info Create Update SchoolFeeStructure method start----");

                    var obj = request.SchoolFeeStructureDto;


                    TblSysSchoolFeeStructureHeader SchoolFeeStructHead = new();
                    if (obj.Id > 0)
                        SchoolFeeStructHead = await _context.SchoolFeeStructureHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                    SchoolFeeStructHead.Id = obj.Id;
                    SchoolFeeStructHead.GradeCode = obj.GradeCode;
                    SchoolFeeStructHead.BranchCode = obj.BranchCode;
                    SchoolFeeStructHead.FeeStructName = obj.FeeStructName;
                    SchoolFeeStructHead.FeeStructName2 = obj.FeeStructName2;
                    SchoolFeeStructHead.ApplyLateFee = obj.ApplyLateFee;
                    SchoolFeeStructHead.LateFeeCode = obj.LateFeeCode;
                    SchoolFeeStructHead.ActualFeePayable = obj.ActualFeePayable;
                    SchoolFeeStructHead.Remarks = obj.Remarks;
                    SchoolFeeStructHead.IsActive = obj.IsActive;
                    SchoolFeeStructHead.CreatedOn = DateTime.Now;
                    SchoolFeeStructHead.CreatedBy = obj.CreatedBy;
                  
                        if (obj.Id > 0)
                        {

                            _context.SchoolFeeStructureHeader.Update(SchoolFeeStructHead);
                        }
                        else
                        {
                        SchoolFeeStructHead.FeeStructCode = obj.FeeStructCode.ToUpper();
                        await _context.SchoolFeeStructureHeader.AddAsync(SchoolFeeStructHead);
                        }

                        await _context.SaveChangesAsync();
                    
                    if (request.SchoolFeeStructureDto.FeeDetailList.Count() > 0)
                    {
                        var oldFeeDetailList = await _context.SchoolFeeStructureDetails.Where(e => e.FeeStructCode == SchoolFeeStructHead.FeeStructCode).ToListAsync();
                        _context.SchoolFeeStructureDetails.RemoveRange(oldFeeDetailList);

                        List<TblSysSchoolFeeStructureDetails> feeDetailList = new();
                        foreach (var feedetail in request.SchoolFeeStructureDto.FeeDetailList)
                        {
                            TblSysSchoolFeeStructureDetails detail = new()
                            {
                                FeeStructCode = SchoolFeeStructHead.FeeStructCode,
                                TermCode = feedetail.TermCode,
                                FeeCode=feedetail.FeeCode,
                                FeeAmount=feedetail.FeeAmount,
                                MaxDiscPer=feedetail.MaxDiscPer,
                                ActualFeeAmount=feedetail.ActualFeeAmount

                            };
                            if (detail.Id > 0)
                            {
                                _context.SchoolFeeStructureDetails.Update(detail);
                            }
                            else
                            {

                                await _context.SchoolFeeStructureDetails.AddAsync(detail);
                            }

                            feeDetailList.Add(detail);
                        }
                        await _context.SchoolFeeStructureDetails.AddRangeAsync(feeDetailList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateUpdateSchoolFeeStructDetails  method Exit----");
                    await transaction.CommitAsync();
                    return SchoolFeeStructHead.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSchoolFeeStructureDetails  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }






        

    }
    #endregion

    #region Get All Header List
    public class GetSysSchoolFeeStructureHeaderList : IRequest<PaginatedList<TblSysSchoolFeeStructureHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolFeeStructureHeaderListHandler : IRequestHandler<GetSysSchoolFeeStructureHeaderList, PaginatedList<TblSysSchoolFeeStructureHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolFeeStructureHeaderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolFeeStructureHeaderDto>> Handle(GetSysSchoolFeeStructureHeaderList request, CancellationToken cancellationToken)
        {

            
            var schoolFeeStructHeaders = await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider)
                                         .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return schoolFeeStructHeaders;
        }


    }


    #endregion

    #region GetFeeDetailsByFeeStructCode 
    public class GetSchoolFeeDetailsByFeeStructCode : IRequest<TblSysSchoolFeeStructureDto>
    {
        public UserIdentityDto User { get; set; }
        public string FeeStructCode { get; set; }
    }

    public class GetSchoolFeeDetailsByFeeStructCodeHandler : IRequestHandler<GetSchoolFeeDetailsByFeeStructCode, TblSysSchoolFeeStructureDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolFeeDetailsByFeeStructCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSysSchoolFeeStructureDto> Handle(GetSchoolFeeDetailsByFeeStructCode request, CancellationToken cancellationToken)
        {
            TblSysSchoolFeeStructureDto obj = new();
            List<TblSysSchoolFeeStructureDetailsDto> feedetails = await _context.SchoolFeeStructureDetails.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.FeeStructCode == request.FeeStructCode).ToListAsync();
             obj.FeeDetailList= feedetails;
            return obj;
        }
    }

    #endregion

    #region Get Fee Structure Header By Id 
    public class GetFeeStructureHeaderById : IRequest<TblSysSchoolFeeStructureHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetFeeStructureHeaderByIdHandler : IRequestHandler<GetFeeStructureHeaderById, TblSysSchoolFeeStructureHeaderDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetFeeStructureHeaderByIdHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<TblSysSchoolFeeStructureHeaderDto> Handle(GetFeeStructureHeaderById request,CancellationToken cancel)
        {
            
            var feeStructure=await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return feeStructure;

        }

    }
    #endregion

    #region GetDetailofFeeforAllTerm 
    public class GetSchoolFeeDetailsforAllTerm : IRequest<CustomSysSchoolFeeStructureDetails>
    {
        public UserIdentityDto User { get; set; }
        public string FeeStructCode { get; set; }
    }

    public class GetSchoolFeeDetailsforAllTermHandler : IRequestHandler<GetSchoolFeeDetailsforAllTerm, CustomSysSchoolFeeStructureDetails>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolFeeDetailsforAllTermHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSysSchoolFeeStructureDetails> Handle(GetSchoolFeeDetailsforAllTerm request, CancellationToken cancellationToken)
        {
            CustomSysSchoolFeeStructureDetails obj = new();
            //  List<TblSysSchoolFeeStructureDetailsDto> feedetails = await _context.SchoolFeeStructureDetails.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.FeeStructCode == request.FeeStructCode).ToListAsync();

            List<TblSysSchoolFeeStructureDetailsDto> feeTermDetail = await _context.SchoolFeeStructureDetails.AsNoTracking()
                .Where(e => e.FeeStructCode == request.FeeStructCode)
                .ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider)
                .Select(e => new TblSysSchoolFeeStructureDetailsDto { FeeStructCode = e.FeeStructCode, TermCode = e.TermCode, FeeAmount = e.FeeAmount }).ToListAsync();

            var result = await _context.SchoolFeeStructureDetails
                .Where(e => e.FeeStructCode == request.FeeStructCode).GroupBy(r => new { r.TermCode, r.FeeAmount })
                                    .Select(r => new CustomSysSchoolFeeStructureDetails
                                    {
                                        TermCode = r.Key.TermCode,
                                        FeeAmount = r.Sum(x => x.FeeAmount),
                                        TotalCount = r.Count()
                                    }).FirstOrDefaultAsync();

            result.FeeTermCodeDetails = feeTermDetail;

            return result;
        }
    }
    public class CustomSysSchoolFeeStructureDetails
    {
        public CustomSysSchoolFeeStructureDetails()
        {
            FeeTermCodeDetails = new List<TblSysSchoolFeeStructureDetailsDto>();
        }
        public string TermCode { get; set; }
        public int TotalCount { get; set; }
        public decimal FeeAmount { get; set; }
        public List<TblSysSchoolFeeStructureDetailsDto> FeeTermCodeDetails { get; set; }
    }
    #endregion

    

    #region Get_Fee_structure_codes_by_grade_code
    public class GetFeeStructureCodesByGradeCode : IRequest<List<TblSysSchoolFeeStructureHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public string gradeCode { get; set; }
    }

    public class GetFeeStructureCodesByGradeCodeHandler : IRequestHandler<GetFeeStructureCodesByGradeCode, List<TblSysSchoolFeeStructureHeaderDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetFeeStructureCodesByGradeCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblSysSchoolFeeStructureHeaderDto>> Handle(GetFeeStructureCodesByGradeCode request, CancellationToken cancel)
        {

            var feeStructureCodes = await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider).Where(e => e.GradeCode == request.gradeCode).ToListAsync();
            return feeStructureCodes;

        }

    }
    #endregion

    #region GetDetailofFeeforAllTerm 
    public class GetTotalTermFeeByFeeStructCode : IRequest<CustomStudentFeeStructureDetails>
    {
        public UserIdentityDto User { get; set; }
        public string FeeStructCode { get; set; }
    }

    public class GetTotalTermFeeByFeeStructCodeHandler : IRequestHandler<GetTotalTermFeeByFeeStructCode, CustomStudentFeeStructureDetails>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTotalTermFeeByFeeStructCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomStudentFeeStructureDetails> Handle(GetTotalTermFeeByFeeStructCode request, CancellationToken cancellationToken)
        {
            CustomStudentFeeStructureDetails obj = new();
            obj.FeeAmount= await _context.SchoolFeeStructureDetails
                .Where(e => e.FeeStructCode == request.FeeStructCode)
                                    .SumAsync(x=>x.FeeAmount);

            return obj;
        }
    }
    public class CustomStudentFeeStructureDetails
    {
        public string TermCode { get; set; }
        public int TotalCount { get; set; }
        public decimal FeeAmount { get; set; }
    }
    #endregion


    #region Get_Fee_structure_codes_by_grade_code_branchcode
    public class GetFeeStructureCodesByGradeANDBranch : IRequest<List<TblSysSchoolFeeStructureHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public string gradeCode { get; set; }
        public string branchCode { get; set; }
    }

    public class GetFeeStructureCodesByGradeANDBranchHandler : IRequestHandler<GetFeeStructureCodesByGradeANDBranch, List<TblSysSchoolFeeStructureHeaderDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetFeeStructureCodesByGradeANDBranchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblSysSchoolFeeStructureHeaderDto>> Handle(GetFeeStructureCodesByGradeANDBranch request, CancellationToken cancel)
        {

            var feeStructureCodes = await _context.SchoolFeeStructureHeader.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureHeaderDto>(_mapper.ConfigurationProvider).Where(e => e.GradeCode == request.gradeCode && e.BranchCode==request.branchCode).ToListAsync();
            return feeStructureCodes;

        }

    }
    #endregion

}
