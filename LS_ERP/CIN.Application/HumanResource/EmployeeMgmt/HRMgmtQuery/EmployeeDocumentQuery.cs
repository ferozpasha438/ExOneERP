using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetPagedList

    public class GetEmployeeDocumentList : IRequest<PaginatedList<TblHRMTrnEmployeeDocumentInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeDocumentListHandler : IRequestHandler<GetEmployeeDocumentList, PaginatedList<TblHRMTrnEmployeeDocumentInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeDocumentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeDocumentInfoDto>> Handle(GetEmployeeDocumentList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeDocumentList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeDocument in _context.EmployeeDocuments
                                  join documentType in _context.DocumentTypes on employeeDocument.DocumentTypeCode equals documentType.DocumentTypeCode
                                  select new TblHRMTrnEmployeeDocumentInfoDto
                                  {
                                      Id = employeeDocument.Id,
                                      EmployeeID = employeeDocument.EmployeeID,
                                      DocumentTypeCode = employeeDocument.DocumentTypeCode,
                                      DocumentTypeName = isArab ? documentType.DocumentTypeNameAr : documentType.DocumentTypeNameEn,
                                      IsVerified = employeeDocument.IsVerified,
                                      DocumentNumber = employeeDocument.DocumentNumber,
                                      Name = employeeDocument.Name,
                                      FileName = employeeDocument.FileName,
                                      IsActive = employeeDocument.IsActive,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) &&
                                  (e.DocumentTypeName.Contains(search) || e.DocumentNumber.Contains(search) || e.Name.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeDocumentList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeDocumentList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeDocumentById

    public class GetEmployeeDocumentById : IRequest<TblHRMTrnEmployeeDocumentInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeDocumentByIdHandler : IRequestHandler<GetEmployeeDocumentById, TblHRMTrnEmployeeDocumentInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeDocumentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeDocumentInfoDto> Handle(GetEmployeeDocumentById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeDocumentById method start----");
            try
            {
                var employeeDocument = await _context.EmployeeDocuments.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeDocumentInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeDocumentById method end----");

                if (employeeDocument is not null)
                    return employeeDocument;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeDocumentById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeAddress

    public class CreateUpdateEmployeeDocument : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public List<TblHRMTrnEmployeeDocumentInfoDto> Input { get; set; }
    }
    public class CreateUpdateEmployeeDocumentHandler : IRequestHandler<CreateUpdateEmployeeDocument, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeDocumentHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeDocument request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeDocument method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeDocumentInfo employeeDocument = new();

                    foreach (var p in obj)
                        await Task.Run(async () =>
                        {

                            //obj.ForEach(async (p) =>
                            //{
                            if (p.Id > 0)
                            {
                                employeeDocument = await _context.EmployeeDocuments
                                    .FirstOrDefaultAsync(e => e.Id == p.Id && e.EmployeeID == p.EmployeeID);

                                employeeDocument.DocumentTypeCode = p.DocumentTypeCode;
                                employeeDocument.IsVerified = p.IsVerified;
                                employeeDocument.DocumentNumber = p.DocumentNumber;
                                employeeDocument.Name = p.Name;
                                employeeDocument.FileName = p.FileName;
                                employeeDocument.IsActive = p.IsActive;
                                employeeDocument.ModifiedBy = request.User.UserId;
                                employeeDocument.Modified = DateTime.Now;

                                _context.EmployeeDocuments.Update(employeeDocument);
                            }
                            else
                            {
                                employeeDocument = new()
                                {
                                    EmployeeID = p.EmployeeID,
                                    DocumentTypeCode = p.DocumentTypeCode,
                                    IsVerified = p.IsVerified,
                                    DocumentNumber = p.DocumentNumber,
                                    Name = p.Name,
                                    FileName = p.FileName,
                                    IsActive = p.IsActive,
                                    CreatedBy = request.User.UserId,
                                    Created = DateTime.Now,
                                };
                                await _context.EmployeeDocuments.AddAsync(employeeDocument);
                            }
                            //});
                        }).ConfigureAwait(false);


                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeDocument method Exit----");
                    return ApiMessageInfo.Status(1, employeeDocument.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeDocument Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeDocument
    public class DeleteEmployeeDocument : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class DeleteEmployeeDocumentHandler : IRequestHandler<DeleteEmployeeDocument, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeDocumentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeDocument request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeDocument method start----");
                if (request.Id > 0)
                {
                    var employeeDocument = await _context.EmployeeDocuments
                        .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                    _context.Remove(employeeDocument);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeDocument method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeDocument Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
