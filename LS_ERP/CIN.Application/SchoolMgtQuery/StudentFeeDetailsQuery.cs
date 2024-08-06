using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
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
using CIN.Application.SchoolMgtDto;

namespace CIN.Application.SchoolMgtQuery
{
    #region Get List
    public class GetStudentFeeDetailsList : IRequest<List<TblDefStudentFeeDetailsDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetStudentFeeDetailsListHandler : IRequestHandler<GetStudentFeeDetailsList, List<TblDefStudentFeeDetailsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentFeeDetailsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentFeeDetailsDto>> Handle(GetStudentFeeDetailsList request, CancellationToken cancellationToken)
        {
            var stuFeeDetailList = await _context.DefStudentFeeDetails.AsNoTracking().ProjectTo<TblDefStudentFeeDetailsDto>(_mapper.ConfigurationProvider).ToListAsync();
            return stuFeeDetailList;
        }
    }



    #endregion

    #region GetById
    public class GetStudentFeeDetailsById : IRequest<TblDefStudentFeeDetailsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetStudentFeeDetailsByIdHandler : IRequestHandler<GetStudentFeeDetailsById, TblDefStudentFeeDetailsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentFeeDetailsByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentFeeDetailsDto> Handle(GetStudentFeeDetailsById request, CancellationToken cancellationToken)
        {
            var studentFDId = await _context.DefStudentFeeDetails.AsNoTracking().ProjectTo<TblDefStudentFeeDetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentFDId;
        }
    }


    #endregion

    #region CreateUpdate
    public class CreateUpdateStudentFeeDetails : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentFeeDetailsDto Input { get; set; }
    }
    public class CreateUpdateStudentFeeDetailsHandler : IRequestHandler<CreateUpdateStudentFeeDetails, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentFeeDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentFeeDetails request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Method Start-----");
                var obj = request.Input;
                TblDefStudentFeeDetails studentFeeDetails = new();
                if (obj.Id > 0)
                    studentFeeDetails = await _context.DefStudentFeeDetails.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                studentFeeDetails.Id = obj.Id;
                studentFeeDetails.StuAdmNum = obj.StuAdmNum;
                studentFeeDetails.FeeStructCode = obj.FeeStructCode;
                studentFeeDetails.TermCode = obj.TermCode;
                studentFeeDetails.FeeCode = obj.FeeCode;
                studentFeeDetails.FeeAmount = obj.FeeAmount;
                studentFeeDetails.MaxDiscPer = obj.MaxDiscPer;
                studentFeeDetails.DiscPer = obj.DiscPer;
                studentFeeDetails.NetDiscAmt = obj.NetDiscAmt;
                studentFeeDetails.NetFeeAmount = obj.NetFeeAmount;
                studentFeeDetails.IsPaid = obj.IsPaid;
                studentFeeDetails.IsLateFee = obj.IsLateFee;
                studentFeeDetails.IsAddedManaully = obj.IsAddedManaully;
                studentFeeDetails.AddedBy = obj.AddedBy;
                studentFeeDetails.AddedOn = obj.AddedOn;
                studentFeeDetails.IsVoided = obj.IsVoided;
                studentFeeDetails.VoidedBy = obj.VoidedBy;

                if (obj.Id > 0)
                {
                    _context.DefStudentFeeDetails.Update(studentFeeDetails);
                }
                else
                {
                    await _context.DefStudentFeeDetails.AddAsync(studentFeeDetails);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Method Start---- -");
                return studentFeeDetails.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteStudentFeeDetails : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentFeeDetailsHandler : IRequestHandler<DeleteStudentFeeDetails, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentFeeDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteStudentFeeDetails request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Fee Details start----");
                if (request.Id > 0)
                {
                    var studentFeeDetails = await _context.DefStudentFeeDetails.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(studentFeeDetails);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Student Fee Details");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region UpdateStudentFeeDetails
    public class UpdateStudentFeeDetails : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentFeeDetailsDto Input { get; set; }
    }
    public class UpdateStudentFeeDetailsHandler : IRequestHandler<UpdateStudentFeeDetails, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UpdateStudentFeeDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateStudentFeeDetails request, CancellationToken cancellationToken)
        {
            try
            {
                var taxData = await _context.SystemTaxes.FirstOrDefaultAsync();
                int academicYear = await _context.SysSchoolAcademicBatches.AsNoTracking().
                                                     ProjectTo<TblSysSchoolAcademicBatchesDto>(_mapper.ConfigurationProvider).
                                                     OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).
                                                     FirstOrDefaultAsync();
                Log.Info("----Info Update Method Start-----");
                var obj = request.Input;
                var studentDetails = await _context.DefSchoolStudentMaster.FirstOrDefaultAsync(x=>x.StuAdmNum== obj.StuAdmNum);
                TblDefStudentFeeDetails studentFeeDetails = new();
                if (studentDetails!=null)
                {
                    if (obj.Id == 0)
                    {
                        studentFeeDetails.Id = obj.Id;
                        studentFeeDetails.StuAdmNum = obj.StuAdmNum;
                        studentFeeDetails.FeeStructCode = obj.FeeStructCode;
                        studentFeeDetails.TermCode = obj.TermCode;
                        studentFeeDetails.FeeCode = obj.FeeCode;
                        studentFeeDetails.FeeAmount = obj.FeeAmount;
                        studentFeeDetails.MaxDiscPer = 0;
                        studentFeeDetails.DiscPer = 0;
                        studentFeeDetails.NetDiscAmt = 0;
                        studentFeeDetails.NetFeeAmount = obj.FeeAmount;
                        studentFeeDetails.IsPaid = obj.IsPaid;
                        studentFeeDetails.IsLateFee = false;
                        studentFeeDetails.IsAddedManaully = false;
                        studentFeeDetails.AddedBy = obj.AddedBy;
                        studentFeeDetails.AddedOn = obj.AddedOn;
                        studentFeeDetails.IsVoided = false;
                        studentFeeDetails.VoidedBy = obj.VoidedBy;
                        if (studentDetails.TaxApplicable && taxData != null)
                        {
                            studentFeeDetails.TaxAmount = (obj.FeeAmount / 100) * taxData.Taxper01;
                            studentFeeDetails.TaxCode = taxData.TaxCode;
                            studentFeeDetails.NetFeeAmount = obj.FeeAmount + studentFeeDetails.TaxAmount;
                        }
                        await _context.DefStudentFeeDetails.AddAsync(studentFeeDetails);
                    }
                    else
                    {
                        studentFeeDetails = await _context.DefStudentFeeDetails.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        studentFeeDetails.FeeCode = obj.FeeCode;
                        studentFeeDetails.FeeAmount = obj.FeeAmount;
                        studentFeeDetails.MaxDiscPer = 0;
                        studentFeeDetails.DiscPer = 0;
                        studentFeeDetails.NetDiscAmt = 0;
                        studentFeeDetails.NetFeeAmount = obj.FeeAmount;
                        studentFeeDetails.IsPaid = false;
                        studentFeeDetails.AddedBy = obj.AddedBy;
                        studentFeeDetails.AddedOn = obj.AddedOn;
                        studentFeeDetails.IsVoided = obj.IsVoided;
                        studentFeeDetails.VoidedBy = obj.VoidedBy;
                        if (studentDetails.TaxApplicable && taxData != null)
                        {
                            studentFeeDetails.TaxAmount = (obj.FeeAmount / 100) * taxData.Taxper01;
                            studentFeeDetails.TaxCode = taxData.TaxCode;
                            studentFeeDetails.NetFeeAmount = obj.FeeAmount + studentFeeDetails.TaxAmount;
                        }
                        else
                        {
                            studentFeeDetails.TaxAmount = 0;
                            studentFeeDetails.TaxCode = null;
                        }
                        _context.DefStudentFeeDetails.Update(studentFeeDetails);
                        await _context.SaveChangesAsync();
                    }
                    var studentFeeHeader = await _context.DefStudentFeeHeader.FirstOrDefaultAsync(e => e.StuAdmNum == studentFeeDetails.StuAdmNum && e.TermCode == studentFeeDetails.TermCode);
                    _context.Remove(studentFeeHeader);
                    await _context.SaveChangesAsync();
                    var dataList = await _context.DefStudentFeeDetails
                                   .Where(e => e.StuAdmNum == obj.StuAdmNum && e.TermCode == obj.TermCode).GroupBy(r => new { r.TermCode })
                                                       .Select(r => new CustomSysSchoolFeeStructureDetails
                                                       {
                                                           TermCode = r.Key.TermCode,
                                                           FeeAmount = r.Sum(x => x.FeeAmount),
                                                       }).ToListAsync();
                    foreach (var item in dataList)
                    {
                        TblDefStudentFeeHeader studentfeeHeader = new();
                        studentfeeHeader.StuAdmNum = obj.StuAdmNum;
                        studentfeeHeader.FeeStructCode = obj.FeeStructCode;
                        studentfeeHeader.TermCode = obj.TermCode;
                        var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                        studentfeeHeader.FeeDueDate = termDetails.FeeDueDate;
                        studentfeeHeader.TotFeeAmount = item.FeeAmount;
                        studentfeeHeader.DiscAmount = 0;
                        studentfeeHeader.NetFeeAmount = item.FeeAmount;
                        studentfeeHeader.IsPaid = false;
                        studentfeeHeader.AcademicYear = academicYear;
                        studentfeeHeader.IsCompletelyPaid = false;
                        if (studentDetails.TaxApplicable && taxData != null)
                        {
                            studentfeeHeader.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                            studentfeeHeader.NetFeeAmount = item.FeeAmount + studentfeeHeader.TaxAmount;
                        }
                        await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                        await _context.SaveChangesAsync();
                    } 
                }
                Log.Info("----Info Update Method End---- -");
                return studentFeeDetails.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Update Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region DeleteFeeDetailsandUpdateFeeHeader
    public class DeleteFeeDetailsandUpdateFeeHeader : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteFeeDetailsandUpdateFeeHeaderHandler : IRequestHandler<DeleteFeeDetailsandUpdateFeeHeader, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteFeeDetailsandUpdateFeeHeaderHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteFeeDetailsandUpdateFeeHeader request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Fee Details start----");
                string stuAdmNum = string.Empty;
                string termCode = string.Empty;
                string feeStructCode = string.Empty;
                int academicYear = 0;
                if (request.Id > 0)
                {
                    var studentFeeDetails = await _context.DefStudentFeeDetails.FirstOrDefaultAsync(e => e.Id == request.Id);
                    if (studentFeeDetails != null)
                    {
                        stuAdmNum = studentFeeDetails.StuAdmNum;
                        termCode = studentFeeDetails.TermCode;
                        feeStructCode = studentFeeDetails.FeeStructCode;
                        academicYear = studentFeeDetails.AcademicYear;

                        var studentFeeHeader = await _context.DefStudentFeeHeader.FirstOrDefaultAsync(e => e.StuAdmNum == studentFeeDetails.StuAdmNum && e.TermCode == studentFeeDetails.TermCode);
                        _context.Remove(studentFeeHeader);
                        await _context.SaveChangesAsync();

                        _context.Remove(studentFeeDetails);
                        await _context.SaveChangesAsync();

                        var dataList = await _context.DefStudentFeeDetails
                                   .Where(e => e.StuAdmNum == stuAdmNum && e.TermCode == termCode).GroupBy(r => new { r.TermCode })
                                                       .Select(r => new CustomSysSchoolFeeStructureDetails
                                                       {
                                                           TermCode = r.Key.TermCode,
                                                           FeeAmount = r.Sum(x => x.FeeAmount),
                                                       }).ToListAsync();
                        foreach (var item in dataList)
                        {
                            TblDefStudentFeeHeader studentfeeHeader = new();
                            studentfeeHeader.StuAdmNum = stuAdmNum;
                            studentfeeHeader.FeeStructCode = feeStructCode;
                            studentfeeHeader.TermCode = termCode;
                            var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                            studentfeeHeader.FeeDueDate = termDetails.FeeDueDate;
                            studentfeeHeader.TotFeeAmount = item.FeeAmount;
                            studentfeeHeader.DiscAmount = 0;
                            studentfeeHeader.NetFeeAmount = item.FeeAmount;
                            studentfeeHeader.IsPaid = false;
                            studentfeeHeader.AcademicYear = academicYear;
                            studentfeeHeader.IsCompletelyPaid = false;
                            await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                            await _context.SaveChangesAsync();
                        }

                    }
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Student Fee Details");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion
}
