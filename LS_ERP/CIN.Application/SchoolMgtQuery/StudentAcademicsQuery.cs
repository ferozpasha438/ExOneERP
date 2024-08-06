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
    #region GetAll
    public class GetAllStudentAcademicsList : IRequest<PaginatedList<TblDefStudentAcademicsDto>>
    {
        public UserIdentityDto User { get; set; }
        public  PaginationFilterDto Input { get; set; }
    }

    public class GetAllStudentAcademicsListHandler : IRequestHandler<GetAllStudentAcademicsList, PaginatedList<TblDefStudentAcademicsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetAllStudentAcademicsListHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblDefStudentAcademicsDto>>Handle(GetAllStudentAcademicsList request,CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentAcademics.AsNoTracking().ProjectTo<TblDefStudentAcademicsDto>(_mapper.ConfigurationProvider)
                             .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;
        }
    }
    #endregion

    #region GetById
    public class GetStudentAcademicById : IRequest<TblDefStudentAcademicsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetStudentAcademicByIdHandler : IRequestHandler<GetStudentAcademicById,TblDefStudentAcademicsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentAcademicByIdHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentAcademicsDto> Handle(GetStudentAcademicById request,CancellationToken cancellationToken)
        {
            var studentAcademic = await _context.DefStudentAcademics.AsNoTracking().ProjectTo<TblDefStudentAcademicsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentAcademic;
        }
    }
    #endregion

    #region CreateAndUpdate
    public class CreateUpdateStudentAcademics : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentAcademicsDto Input { get; set; }
    }

    public class CreateUpdateStudentAcademicsHandler : IRequestHandler<CreateUpdateStudentAcademics, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentAcademicsHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int>Handle(CreateUpdateStudentAcademics request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("---- Info CreateUpdate Student Academics method start");
                var obj = request.Input;

                TblDefStudentAcademics StudentAcademic = new();
                if (obj.Id > 0)
                    StudentAcademic = await _context.DefStudentAcademics.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                StudentAcademic.Id = obj.Id;
                StudentAcademic.StuAdmNum = obj.StuAdmNum;
                StudentAcademic.Grade = obj.Grade;
                StudentAcademic.ExamDate = obj.ExamDate;
                StudentAcademic.ExamName = obj.ExamName;
                StudentAcademic.Result = obj.Result;
                StudentAcademic.PassPercent = obj.PassPercent;
                StudentAcademic.AcademicYear = obj.AcademicYear;

                if (obj.Id > 0)
                {
                    _context.DefStudentAcademics.Update(StudentAcademic);
                }
                else
                {
                    await _context.DefStudentAcademics.AddAsync(StudentAcademic);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Student Academics method Exit----");
                return StudentAcademic.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolSemister Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteStudentAcademic : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentAcademicHandler : IRequestHandler<DeleteStudentAcademic,int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentAcademicHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int>Handle(DeleteStudentAcademic request,CancellationToken cancellationToken)
        {
            try
            {
                var studentAcademic = await _context.DefStudentAcademics.FirstOrDefaultAsync(e => e.Id == request.Id);
                                      _context.Remove(studentAcademic);
                await _context.SaveChangesAsync();
                return request.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Delete Student Academics");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

}
