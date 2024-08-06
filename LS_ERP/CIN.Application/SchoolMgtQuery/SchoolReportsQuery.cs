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
using System.Runtime.Intrinsics.X86;

namespace CIN.Application.SchoolMgtQuery
{
    #region TermDuePaymentReport
    public class TermDuePaymentReport : IRequest<PaginatedList<SchoolTermDuePaymentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class TermDuePaymentReportHandler : IRequestHandler<TermDuePaymentReport, PaginatedList<SchoolTermDuePaymentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TermDuePaymentReportHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<SchoolTermDuePaymentDto>> Handle(TermDuePaymentReport request, CancellationToken cancellationToken)
        {
            var branchData = await _context.SchoolBranches.AsNoTracking()
                                           .SingleOrDefaultAsync(x => x.BranchCode == request.Input.Code);
            if (branchData != null)
            {
                var termsData = await _context.SysSchoolFeeTerms.AsNoTracking().ToListAsync();

                var result = await _context.DefStudentFeeHeader
                                      .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SFH, SSM) => new
                                      {
                                          SFH.StuAdmNum,
                                          SSM.StuName,
                                          SSM.StuName2,
                                          SSM.GradeCode,
                                          SSM.AcademicYear,
                                          SSM.GradeSectionCode,
                                          SSM.BranchCode,
                                          SFH.TermCode,
                                          SFH.IsPaid,
                                          SFH.FeeDueDate,
                                      })
                                      .Join(_context.SysSchoolFeeTerms, SMFH => SMFH.TermCode, SFT => SFT.TermCode, (SMFH, SFT) => new
                                      {
                                          SMFH.StuAdmNum,
                                          SMFH.StuName,
                                          SMFH.StuName2,
                                          SMFH.GradeCode,
                                          SMFH.AcademicYear,
                                          SMFH.GradeSectionCode,
                                          SMFH.BranchCode,
                                          SMFH.TermCode,
                                          SMFH.IsPaid,
                                          SMFH.FeeDueDate,
                                      })
                                      .AsNoTracking()
                                      .Where(e => (e.BranchCode == request.Input.Code || e.GradeCode == request.Input.Approval || e.AcademicYear == request.Input.Id) && e.IsPaid == false && e.FeeDueDate < DateTime.Now)
                                      .Select(x => new SchoolTermDuePaymentDto
                                      {
                                          BranchCode = x.BranchCode,
                                          GradeCode = x.GradeCode,
                                          GradeSectionCode = x.GradeSectionCode,
                                          StuAdmNum = x.StuAdmNum,
                                          StuName = x.StuName,
                                          StuName2 = x.StuName2
                                      })
                                      .Distinct()
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                if (result != null)
                {
                    foreach (var item in result.Items)
                    {
                        //Dictionary<string, decimal> stuDueFeeDetails = new();
                        for (int i = 0; i < termsData.Count; i++)
                        {
                            var termHeaderData = await _context.DefStudentFeeHeader
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(x => x.StuAdmNum == item.StuAdmNum
                                                 && x.IsPaid == false
                                                 && x.FeeDueDate < DateTime.Now
                                                 && x.TermCode == termsData[i].TermCode);
                            //stuDueFeeDetails.Add(termData.TermCode, termHeaderData != null ? termHeaderData.NetFeeAmount : 0);
                            if (i == 0)
                                item.Term1 = termHeaderData != null ? termHeaderData.NetFeeAmount : 0;
                            if (i == 1)
                                item.Term2 = termHeaderData != null ? termHeaderData.NetFeeAmount : 0;
                            if (i == 2)
                                item.Term3 = termHeaderData != null ? termHeaderData.NetFeeAmount : 0;
                            if (i == 3)
                                item.Term4 = termHeaderData != null ? termHeaderData.NetFeeAmount : 0;

                            item.FeeDue = item.Term1 + item.Term2 + item.Term3 + item.Term4;
                        }
                        //item.TermDues = stuDueFeeDetails;
                        //item.FeeDue = stuDueFeeDetails.Sum(x => x.Value);
                    }
                }
                return result;


                //var result = await _context.DefStudentFeeHeader
                //          .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SFH, SSM) => new {
                //              SFH.StuAdmNum,
                //              SSM.StuName,
                //              SSM.StuName2,
                //              SSM.GradeCode,
                //              SSM.GradeSectionCode,
                //              SSM.BranchCode,
                //              SFH.TermCode,
                //              SFH.IsPaid
                //          })
                //            .AsNoTracking()
                //            .Where(e => e.BranchCode == request.Input.Code && e.IsPaid == false)
                //           .Select(e => new SchoolTermDuePaymentDto { Text = e.TeacherName1, Value = e.Id.ToString(), TextTwo = e.TeacherName2 })
                //              .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            }
            return null;
        }
    }
    #endregion

    #region GetFeeTerms
    public class GetFeeTerms : IRequest<List<string>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetFeeTermsHandler : IRequestHandler<GetFeeTerms, List<string>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetFeeTermsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(GetFeeTerms request, CancellationToken cancellationToken)
        {
            var termCodes = await _context.SysSchoolFeeTerms.AsNoTracking()
                                           .Select(x => x.TermCode)
                                           .ToListAsync();

            return termCodes;
        }
    }
    #endregion

    #region SendTermDuePaymentNotifications
    public class SendTermDuePaymentNotifications : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class SendTermDuePaymentNotificationsHandler : IRequestHandler<SendTermDuePaymentNotifications, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public SendTermDuePaymentNotificationsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(SendTermDuePaymentNotifications request, CancellationToken cancellationToken)
        {
            bool isSendNotification = false;
            var branchData = await _context.SchoolBranches.AsNoTracking()
                                           .SingleOrDefaultAsync(x => x.BranchCode == request.BranchCode);
            if (branchData != null)
            {
                var termsData = await _context.SysSchoolFeeTerms.AsNoTracking().Select(x => x.TermCode).ToListAsync();
                var resultsData = await _context.DefStudentFeeHeader
                                      .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SFH, SSM) => new
                                      {
                                          SFH.StuAdmNum,
                                          SSM.StuName,
                                          SSM.StuName2,
                                          SSM.GradeCode,
                                          SSM.GradeSectionCode,
                                          SSM.BranchCode,
                                          SFH.TermCode,
                                          SFH.IsPaid,
                                          SFH.FeeDueDate
                                      })
                                      .Join(_context.SysSchoolFeeTerms, SMFH => SMFH.TermCode, SFT => SFT.TermCode, (SMFH, SFT) => new
                                      {
                                          SMFH.StuAdmNum,
                                          SMFH.StuName,
                                          SMFH.StuName2,
                                          SMFH.GradeCode,
                                          SMFH.GradeSectionCode,
                                          SMFH.BranchCode,
                                          SMFH.TermCode,
                                          SMFH.IsPaid,
                                          SMFH.FeeDueDate
                                      })
                                      .AsNoTracking()
                                      .Where(e => e.BranchCode == request.BranchCode && e.IsPaid == false && e.FeeDueDate < DateTime.Now)
                                      .Select(x => new SchoolTermDuePaymentDto
                                      {
                                          BranchCode = x.BranchCode,
                                          GradeCode = x.GradeCode,
                                          GradeSectionCode = x.GradeSectionCode,
                                          StuAdmNum = x.StuAdmNum,
                                          StuName = x.StuName,
                                          StuName2 = x.StuName2
                                      })
                                      .Distinct()
                                      .ToListAsync();
                if (resultsData != null && resultsData.Count > 0)
                {
                    foreach (var item in resultsData)
                    {
                        var termHeaderData = await _context.DefStudentFeeHeader
                                             .AsNoTracking()
                                             .Where(x => x.StuAdmNum == item.StuAdmNum
                                                 && x.IsPaid == false
                                                 && x.FeeDueDate < DateTime.Now
                                                 && termsData.Contains(x.TermCode))
                                             .ToListAsync();
                        var notificationDetails = await _context.NotificaticationTemplates.FirstOrDefaultAsync(x => x.Type == "Due Fee");
                        if (notificationDetails != null)
                        {
                            var studentDetails = await _context.DefSchoolStudentMaster.FirstOrDefaultAsync(x => x.StuAdmNum == item.StuAdmNum);
                            if (studentDetails != null)
                            {

                                TblSysSchoolPushNotificationParent notificationDto = new();
                                notificationDto.MsgNoteId = 0;
                                notificationDto.NotifyDate = DateTime.Now;
                                notificationDto.NotifyMessage = string.Empty;
                                notificationDto.NotifyMessage_Ar = string.Empty;
                                notificationDto.RegisteredMobile = studentDetails.Mobile;
                                notificationDto.IsRead = false;
                                notificationDto.Title = "Due Fee";
                                notificationDto.Title_Ar = "Due Fee";
                                notificationDto.NotifyTo = "Student";
                                var userDetails = await _context.SystemLogins.SingleOrDefaultAsync(x => x.Id == request.User.UserId);
                                if (userDetails != null)
                                {
                                    notificationDto.FromName = userDetails.UserName;
                                    notificationDto.FromUserId = userDetails.LoginId;
                                }
                                await _context.PushNotificationParent.AddAsync(notificationDto);
                                await _context.SaveChangesAsync();
                            }
                        }
                        isSendNotification = true;
                    }
                }
                return isSendNotification;
            }
            return isSendNotification;
        }
    }
    #endregion


    #region AcademicFeeTransactionReport
    public class AcademicFeeTransactionReport : IRequest<PaginatedList<AcademicFeeTransactionReportDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class AcademicFeeTransactionReportHandler : IRequestHandler<AcademicFeeTransactionReport, PaginatedList<AcademicFeeTransactionReportDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AcademicFeeTransactionReportHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<AcademicFeeTransactionReportDto>> Handle(AcademicFeeTransactionReport request, CancellationToken cancellationToken)
        {
            var result = await _context.DefStudentFeeHeader
                                      .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SFH, SSM) => new
                                      {
                                          SFH.StuAdmNum,
                                          SSM.StuName,
                                          SSM.StuName2,
                                          SSM.GradeCode,
                                          SSM.GradeSectionCode,
                                          SSM.BranchCode,
                                          SFH.FeeStructCode,
                                          SSM.AcademicYear
                                      })
                                      .AsNoTracking()
                                      .Where(e => e.BranchCode == request.Input.Code || e.GradeCode == request.Input.Approval || e.AcademicYear == request.Input.Id)
                                      .Select(x => new AcademicFeeTransactionReportDto
                                      {
                                          BranchCode = x.BranchCode,
                                          GradeCode = x.GradeCode,
                                          GradeSectionCode = x.GradeSectionCode,
                                          StuAdmNum = x.StuAdmNum,
                                          StuName = x.StuName,
                                          StuName2 = x.StuName2,
                                          FeeStructCode = x.FeeStructCode
                                      })
                                      .Distinct()
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            if (result != null && result.Items.Count > 0)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    var studentFeeDetails = await _context.DefStudentFeeHeader.AsNoTracking()
                                               .Where(x => x.StuAdmNum == result.Items[i].StuAdmNum)
                                               .ToListAsync();
                    result.Items[i].TotalFee = studentFeeDetails.Sum(x => x.TotFeeAmount);
                    result.Items[i].Tax = studentFeeDetails.Sum(x => x.TaxAmount);
                    result.Items[i].NetFee = studentFeeDetails.Sum(x => x.NetFeeAmount);
                    result.Items[i].Paid = studentFeeDetails.Where(x => x.IsPaid == true).Sum(x => x.NetFeeAmount);
                    result.Items[i].Balance = studentFeeDetails.Where(x => x.IsPaid == false).Sum(x => x.NetFeeAmount);
                }
            }
            return result;
        }
    }
    #endregion

    #region StudentFeeListReport
    public class StudentFeeListReport : IRequest<PaginatedList<StudentFeeListReportDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class StudentFeeListReportHandler : IRequestHandler<StudentFeeListReport, PaginatedList<StudentFeeListReportDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public StudentFeeListReportHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<StudentFeeListReportDto>> Handle(StudentFeeListReport request, CancellationToken cancellationToken)
        {
            var result = await _context.DefStudentFeeHeader
                                      .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SFH, SSM) => new
                                      {
                                          SFH.StuAdmNum,
                                          SSM.StuName,
                                          SSM.StuName2,
                                          SSM.GradeCode,
                                          SSM.GradeSectionCode,
                                          SSM.BranchCode,
                                          SSM.NatCode,
                                          SSM.Mobile,
                                          SFH.FeeStructCode,
                                          SSM.AcademicYear
                                      })
                                      .AsNoTracking()
                                      .Where(e => e.BranchCode == request.Input.Code || e.GradeCode == request.Input.Approval || e.AcademicYear == request.Input.Id)
                                      .Select(x => new StudentFeeListReportDto
                                      {
                                          BranchCode = x.BranchCode,
                                          GradeCode = x.GradeCode,
                                          GradeSectionCode = x.GradeSectionCode,
                                          StuAdmNum = x.StuAdmNum,
                                          StuName = x.StuName,
                                          StuName2 = x.StuName2,
                                          FeeStuctureCode = x.FeeStructCode,
                                          Nationality = x.NatCode,
                                          ParentContact = x.Mobile
                                      })
                                      .Distinct()
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            if (result != null && result.Items.Count > 0)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    var studentFeeDetails = await _context.DefStudentFeeHeader.AsNoTracking()
                                               .Where(x => x.StuAdmNum == result.Items[i].StuAdmNum)
                                               .ToListAsync();
                    result.Items[i].FeeAmount = studentFeeDetails.Sum(x => x.NetFeeAmount);
                    result.Items[i].Due = studentFeeDetails.Any(x => x.IsPaid == false);
                }
            }
            return result;
        }
    }
    #endregion

    #region FeeStructureSummaryReport
    public class FeeStructureSummaryReport : IRequest<PaginatedList<FeeStructureSummaryReportDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class FeeStructureSummaryReportHandler : IRequestHandler<FeeStructureSummaryReport, PaginatedList<FeeStructureSummaryReportDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public FeeStructureSummaryReportHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<FeeStructureSummaryReportDto>> Handle(FeeStructureSummaryReport request, CancellationToken cancellationToken)
        {
            var result = await _context.SchoolFeeStructureHeader
                                      .AsNoTracking()
                                      .Where(e => e.BranchCode == request.Input.Code || e.GradeCode == request.Input.Approval)

                                      .Select(x => new FeeStructureSummaryReportDto
                                      {
                                          FeeStructureName = x.FeeStructName,
                                          GradeCode = x.GradeCode,
                                          StructCode = x.FeeStructCode,
                                          Id = x.Id
                                      })
                                      .Distinct()
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            if (result != null && result.Items.Count > 0)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    var studentFeeDetails = await _context.SchoolFeeStructureDetails.AsNoTracking()
                                               .Where(x => x.FeeStructCode == result.Items[i].StructCode)
                                               .ToListAsync();
                    result.Items[i].TotalAmount = studentFeeDetails.Sum(x => x.FeeAmount);
                    result.Items[i].Tax = 0;
                    result.Items[i].NetFee = studentFeeDetails.Sum(x => x.FeeAmount);
                }
            }
            return result;
        }
    }
    #endregion

    #region FeeStructureDetailsReport
    public class FeeStructureDetailsReport : IRequest<PaginatedList<FeeStructureDetailsReportDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }
    public class FeeStructureDetailsReportHandler : IRequestHandler<FeeStructureDetailsReport, PaginatedList<FeeStructureDetailsReportDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public FeeStructureDetailsReportHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<FeeStructureDetailsReportDto>> Handle(FeeStructureDetailsReport request, CancellationToken cancellationToken)
        {
            var result = await _context.SchoolFeeStructureDetails
                                      .AsNoTracking()
                                      .Where(e => e.SchoolFeeStructureHeader.Id == request.Input.Id)
                                      .Include(x => x.SchoolFeeStructureHeader)
                                      .Include(x => x.SysSchoolFeeType)
                                      .Select(x => new FeeStructureDetailsReportDto
                                      {
                                          FeeStructureName = x.SchoolFeeStructureHeader.FeeStructName,
                                          GradeCode = x.SchoolFeeStructureHeader.GradeCode,
                                          StructCode = x.SchoolFeeStructureHeader.FeeStructCode,
                                          TermCode = x.TermCode,
                                          FeeCode = x.SysSchoolFeeType.FeeCode,
                                          FeeTypeName = x.SysSchoolFeeType.FeesName,
                                          FeeTypeName2 = x.SysSchoolFeeType.FeeName2,
                                          TotalAmount = x.FeeAmount,
                                          NetFee = x.FeeAmount,
                                          Tax = 0
                                      })
                                      .Distinct()
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return result;
        }
    }
    #endregion
}
