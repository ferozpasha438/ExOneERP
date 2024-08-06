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
using CIN.Application.SchoolMgtDtos;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetAllSectionsByGradeMapping 
    public class GetAllSectionsByGradeMapping : IRequest<List<TblSysSchoolGradeSectionMapping1Dto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetAllSectionsByGradeMappingHandler : IRequestHandler<GetAllSectionsByGradeMapping, List<TblSysSchoolGradeSectionMapping1Dto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSectionsByGradeMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolGradeSectionMapping1Dto>> Handle(GetAllSectionsByGradeMapping request, CancellationToken cancellationToken)
        {

            var SectionsMapping = await _context.SchoolGradeSectionMapping.AsNoTracking().ProjectTo<TblSysSchoolGradeSectionMapping1Dto>(_mapper.ConfigurationProvider).Where(e => e.GradeCode == request.GradeCode).ToListAsync();

            return SectionsMapping;
        }
    }



    #endregion


    #region Create_And_Update


    public class CreateUpdateSectionsGradeMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public GradeSectionMappingDto SchoolGradeSectionMapDto { get; set; }
    }

    public class CreateUpdateSectionsGradeMappingHandler : IRequestHandler<CreateUpdateSectionsGradeMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSectionsGradeMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSectionsGradeMapping request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolGradeSectionMapping method start----");

                var obj = request.SchoolGradeSectionMapDto;
                var data = await _context.SchoolGradeSectionMapping.FirstOrDefaultAsync(e => e.GradeCode == obj.GradeCode && e.SectionCode == obj.SectionCode);
                if (data != null && string.IsNullOrEmpty(obj.UploadFileName))
                    obj.UploadFileName = data.FileName;
                TblSysSchoolGradeSectionMapping item = new()
                {
                    GradeCode = obj.GradeCode,
                    SectionCode = obj.SectionCode,
                    MaxStrength = obj.MaxStrength,
                    MinStrength = obj.MinStrength,
                    AvgStrength = obj.AvgStrength,
                    FileName = obj.UploadFileName
                };
                if (data != null)
                    _context.SchoolGradeSectionMapping.Remove(data);
                await _context.SchoolGradeSectionMapping.AddAsync(item);
                await _context.SaveChangesAsync();
                if (obj.Page == 0 && !string.IsNullOrEmpty(obj.SectionCodes))
                {
                    foreach (var itemData in obj.SectionCodes.Split(','))
                    {
                        var deleteData = await _context.SchoolGradeSectionMapping.FirstOrDefaultAsync(e => e.GradeCode == obj.GradeCode && e.SectionCode == itemData);
                        if (deleteData != null)
                        {
                            _context.SchoolGradeSectionMapping.Remove(deleteData);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                //TblSysSchoolGradeSectionMapping GradeSectionMapp = new();
                //if (request.SchoolGradeSectionMapDto.SchoolGradeSectionlist.Count > 0)
                //{
                //    List<TblSysSchoolGradeSectionMapping> listItems = new();
                //    foreach (var GradeSectiondetail in request.SchoolGradeSectionMapDto.SchoolGradeSectionlist)
                //    {


                //        listItems.Add(detail);
                //        //if (detail.Id > 0)
                //        //{
                //        //    _context.SchoolGradeSectionMapping.Update(detail);
                //        //}
                //        //else
                //        //{

                //        //    await _context.SchoolGradeSectionMapping.AddAsync(detail);
                //        //}

                //    }
                //    if (listItems.Count > 0)
                //    {
                //        var list = _context.SchoolGradeSectionMapping.Where(e => e.GradeCode == obj.GradeCode);
                //        if (await list.AnyAsync())
                //        {
                //            _context.RemoveRange(list);
                //        }

                //        await _context.SchoolGradeSectionMapping.AddRangeAsync(listItems);
                //        await _context.SaveChangesAsync();

                //        Log.Info("----Info CreateUpdateSchoolGradeSectionMapping  method Exit----");
                //        return 1;
                //    }
                //}
                return item.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolGradeSectionMapping  Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion


    #region GetAllSectionsByGradeCode
    public class GetAllSectionsByGradeCode : IRequest<List<TblSysSchoolGradeSectionListDto>>
    {
        public UserIdentityDto User { get; set; }
        public string GradeCode { get; set; }
    }

    public class GetAllSectionsByGradeCodeHandler : IRequestHandler<GetAllSectionsByGradeCode, List<TblSysSchoolGradeSectionListDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSectionsByGradeCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolGradeSectionListDto>> Handle(GetAllSectionsByGradeCode request, CancellationToken cancellationToken)
        {
            List<TblSysSchoolGradeSectionListDto> tblSysSchoolGradeSectionListDtos = new List<TblSysSchoolGradeSectionListDto>();
            var sectionsMapping = await _context.SchoolGradeSectionMapping.AsNoTracking().ProjectTo<TblSysSchoolGradeSectionMapping1Dto>(_mapper.ConfigurationProvider).Where(e => e.GradeCode == request.GradeCode).ToListAsync();
            if (sectionsMapping is not null)
            {
                List<string> _sectionCodeList = new List<string>();
                _sectionCodeList = sectionsMapping.Select(x => x.SectionCode).ToList();
                tblSysSchoolGradeSectionListDtos = await _context.SchoolSectionsSection.AsNoTracking().ProjectTo<TblSysSchoolSectionsSectionDto>(_mapper.ConfigurationProvider).
                      Where(x => _sectionCodeList.Contains(x.SectionCode)).
                      Select(x => new TblSysSchoolGradeSectionListDto
                      {
                          GradeCode = request.GradeCode,
                          SectionCode = x.SectionCode,
                          SectionName = x.SectionName,
                          SectionName2 = x.SectionName2
                      }).
                      ToListAsync();
            }
            return tblSysSchoolGradeSectionListDtos;
        }
    }



    #endregion

    #region AddStudentsToNextGrade
    public class AddStudentsToNextGrade : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public int AcademicYear { get; set; }
    }

    public class AddStudentsToNextGradeHandler : IRequestHandler<AddStudentsToNextGrade, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AddStudentsToNextGradeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(AddStudentsToNextGrade request, CancellationToken cancellationToken)
        {
            bool reslut = false;
            var schoolStudentList = await _context.DefSchoolStudentMaster.AsNoTracking()
                                       .Where(x => x.IsActive != false && x.AcademicYear != request.AcademicYear)
                                     .ToListAsync();
            var gradeList = await _context.SchoolAcedemicClassGrade.AsNoTracking()
                                       .ToListAsync();
            if (schoolStudentList.Count > 0)
            {
                foreach (var item in schoolStudentList)
                {
                    var stuGradeData = gradeList.FirstOrDefault(x => x.GradeCode == item.GradeCode);
                    if (stuGradeData != null && stuGradeData.GradeOrder != null)
                    {
                        var nextGradeDta = gradeList.FirstOrDefault(x => x.GradeOrder == (stuGradeData.GradeOrder + 1));
                        if (nextGradeDta != null)
                        {
                            item.GradeCode = nextGradeDta.GradeCode;
                            item.AcademicYear = request.AcademicYear;
                            item.IsPromoted = true;
                        }
                    }
                }
                _context.DefSchoolStudentMaster.UpdateRange(schoolStudentList);
                await _context.SaveChangesAsync();
                reslut = true;
            }
            reslut = true;
            return reslut;
        }
    }



    #endregion

    #region AddFeeStructure
    public class AddFeeStructure : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public int AcademicYear { get; set; }
    }

    public class AddFeeStructureHandler : IRequestHandler<AddFeeStructure, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AddFeeStructureHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(AddFeeStructure request, CancellationToken cancellationToken)
        {
            bool reslut = false;
            try
            {
                var schoolStudentList = await _context.DefSchoolStudentMaster.AsNoTracking()
                                       .Where(x => x.IsActive != false && x.AcademicYear == request.AcademicYear 
                                           && x.IsPromoted==true)
                                     .ToListAsync();

                if (schoolStudentList.Count > 0)
                {
                    foreach (var student in schoolStudentList)
                    {
                        var feeStructure = await _context.SchoolFeeStructureHeader.AsNoTracking()
                                                   .Where(e => e.GradeCode == student.GradeCode
                                                   && e.BranchCode == student.BranchCode
                                                   && e.IsActive == true)
                                                   .OrderByDescending(x => x.Id)
                                                   .FirstOrDefaultAsync();


                        #region FeeHeaderBlock
                        var taxData = await _context.SystemTaxes.FirstOrDefaultAsync();
                        if (student.IsPromoted == true)
                        {
                            var dataList = await _context.SchoolFeeStructureDetails
                                        .Where(e => e.FeeStructCode == feeStructure.FeeStructCode).GroupBy(r => new { r.TermCode })
                                                            .Select(r => new CustomSysSchoolFeeStructureDetails
                                                            {
                                                                TermCode = r.Key.TermCode,
                                                                FeeAmount = r.Sum(x => x.FeeAmount),
                                                            }).ToListAsync();
                            if (dataList.Count > 0)
                            {
                                foreach (var item in dataList)
                                {
                                    TblDefStudentFeeHeader studentfeeHeader = new();
                                    studentfeeHeader.StuAdmNum = student.StuAdmNum;
                                    studentfeeHeader.FeeStructCode = feeStructure.FeeStructCode;
                                    studentfeeHeader.TermCode = item.TermCode;
                                    var termDetails = await _context.SysSchoolFeeTerms.AsNoTracking().ProjectTo<TblSysSchoolFeeTermsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.TermCode == item.TermCode);
                                    studentfeeHeader.FeeDueDate = termDetails.FeeDueDate;
                                    studentfeeHeader.TotFeeAmount = item.FeeAmount;
                                    studentfeeHeader.DiscAmount = 0;
                                    studentfeeHeader.NetFeeAmount = item.FeeAmount;
                                    studentfeeHeader.IsPaid = false;
                                    studentfeeHeader.AcademicYear = student.AcademicYear;
                                    studentfeeHeader.IsCompletelyPaid = false;
                                    if (student.TaxApplicable && taxData != null)
                                    {
                                        studentfeeHeader.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                        studentfeeHeader.NetFeeAmount = item.FeeAmount + studentfeeHeader.TaxAmount;
                                    }
                                    await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                                    await _context.SaveChangesAsync();
                                }
                                student.FeeStructCode = feeStructure.FeeStructCode;
                            }

                        }
                        #endregion
                        #region FeeDetailsBlock

                        if (student.IsPromoted == true)
                        {
                            List<TblSysSchoolFeeStructureDetailsDto> feedetails = await _context.SchoolFeeStructureDetails.AsNoTracking().ProjectTo<TblSysSchoolFeeStructureDetailsDto>(_mapper.ConfigurationProvider).Where(e => e.FeeStructCode == feeStructure.FeeStructCode).ToListAsync();
                            foreach (var item in feedetails)
                            {
                                TblDefStudentFeeDetails studentFeeDetails = new();
                                studentFeeDetails.StuAdmNum = student.StuAdmNum;
                                studentFeeDetails.FeeStructCode = feeStructure.FeeStructCode;
                                studentFeeDetails.TermCode = item.TermCode;
                                studentFeeDetails.FeeCode = item.FeeCode;
                                studentFeeDetails.FeeAmount = item.FeeAmount;
                                studentFeeDetails.MaxDiscPer = item.MaxDiscPer;
                                studentFeeDetails.DiscPer = 0;
                                studentFeeDetails.NetDiscAmt = 0;
                                studentFeeDetails.NetFeeAmount = item.FeeAmount;
                                studentFeeDetails.IsPaid = false;
                                studentFeeDetails.IsLateFee = false; //feeStructureHeader.ApplyLateFee;
                                studentFeeDetails.IsAddedManaully = false;
                                studentFeeDetails.AddedBy = Convert.ToString(request.User.UserId);
                                studentFeeDetails.AddedOn = DateTime.Now;
                                studentFeeDetails.IsVoided = false;
                                if (student.TaxApplicable && taxData != null)
                                {
                                    studentFeeDetails.TaxAmount = (item.FeeAmount / 100) * taxData.Taxper01;
                                    studentFeeDetails.TaxCode = taxData.TaxCode;
                                    studentFeeDetails.NetFeeAmount = item.FeeAmount + studentFeeDetails.TaxAmount;
                                }
                                await _context.DefStudentFeeDetails.AddAsync(studentFeeDetails);
                                await _context.SaveChangesAsync();
                            }

                        }
                        #endregion
                        student.IsPromoted = false;
                    }
                    _context.DefSchoolStudentMaster.UpdateRange(schoolStudentList);
                    await _context.SaveChangesAsync();
                    reslut = true;
                }

                reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                throw;
            }
            return reslut;
        }
    }



    #endregion

}