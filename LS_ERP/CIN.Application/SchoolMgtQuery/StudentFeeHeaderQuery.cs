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

namespace CIN.Application.SchoolMgtQuery
{

    #region Get List
    public class GetStudentFeeHeaderList : IRequest<List<TblDefStudentFeeHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetStudentHeaderListHandler : IRequestHandler<GetStudentFeeHeaderList, List<TblDefStudentFeeHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        
        public GetStudentHeaderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentFeeHeaderDto>>Handle(GetStudentFeeHeaderList request,CancellationToken cancellationToken)
        {
            var studentList = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider).ToListAsync();
            return studentList;
        }
    }

    #endregion

    #region GetById
    public class GetStudentFeeHeaderById : IRequest<TblDefStudentFeeHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetStudentFeeHeaderByIdHandler : IRequestHandler<GetStudentFeeHeaderById, TblDefStudentFeeHeaderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentFeeHeaderByIdHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public  async Task<TblDefStudentFeeHeaderDto> Handle(GetStudentFeeHeaderById request,CancellationToken cancellationToken)
        {
            var studentFhId = await _context.DefStudentFeeHeader.AsNoTracking().ProjectTo<TblDefStudentFeeHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentFhId;
        }
    }


    #endregion

    #region Create
    public class CreateUpdateStudentFeeHeader : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentFeeHeaderDto Input { get; set; }
    }
    public class CreateUpdateStudentFeeHeaderHandler : IRequestHandler<CreateUpdateStudentFeeHeader, int>
    {
        private readonly CINDBOneContext _context;
        private  readonly IMapper _mapper;

        public CreateUpdateStudentFeeHeaderHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentFeeHeader request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Method Start-----");
                var obj = request.Input;
                TblDefStudentFeeHeader studentfeeHeader = new();
                if (obj.Id > 0)
                    studentfeeHeader = await _context.DefStudentFeeHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                studentfeeHeader.Id = obj.Id;
                studentfeeHeader.StuAdmNum = obj.StuAdmNum;
                studentfeeHeader.FeeStructCode = obj.FeeStructCode;
                studentfeeHeader.TermCode = obj.TermCode;
                studentfeeHeader.FeeDueDate = obj.FeeDueDate;
                studentfeeHeader.TotFeeAmount = obj.TotFeeAmount;
                studentfeeHeader.DiscAmount = obj.DiscAmount;
                studentfeeHeader.NetFeeAmount = obj.NetFeeAmount;
                studentfeeHeader.DiscReason = obj.DiscReason;
                studentfeeHeader.IsPaid = obj.IsPaid;
                studentfeeHeader.PaidOn = obj.PaidOn;
                studentfeeHeader.PaidTransNum = obj.PaidTransNum;
                studentfeeHeader.PaidRemarks1 = obj.PaidRemarks1;
                studentfeeHeader.PaidRemarks2 = obj.PaidRemarks2;
                studentfeeHeader.JvNumber = obj.JvNumber;
                studentfeeHeader.AcademicYear = obj.AcademicYear;
                studentfeeHeader.IsCompletelyPaid = obj.IsCompletelyPaid;

                if (obj.Id > 0)
                {
                    _context.DefStudentFeeHeader.Update(studentfeeHeader);
                }
                else
                {
                    await _context.DefStudentFeeHeader.AddAsync(studentfeeHeader);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Method Start---- -");
                return studentfeeHeader.Id;
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
    public class DeleteStudentFeeHeader : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentHandler : IRequestHandler<DeleteStudentFeeHeader,int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int>Handle(DeleteStudentFeeHeader request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteStudentFeeHeader start----");

                if (request.Id > 0)
                {
                    var studentFeeHeader = await _context.DefStudentFeeHeader.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(studentFeeHeader);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Student FeeHeader");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        } 
    }
    #endregion
}
